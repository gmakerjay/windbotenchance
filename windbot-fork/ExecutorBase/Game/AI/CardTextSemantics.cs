using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WindBot.Game;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Semantic analysis profile of a card deduced from its card text, types, and stats.
    /// Cached permanently in memory by Card ID to achieve O(1) evaluation speed.
    /// </summary>
    public class CardSemanticProfile
    {
        public int CardId { get; set; }
        public string Name { get; set; }

        /// <summary>Places or manipulates Bushido/Spell/other counters.</summary>
        public bool IsCounterGenerator { get; set; }

        /// <summary>Continuous/Field spell or monster searching cards from deck to hand repeatedly.</summary>
        public bool IsContinuousSearcher { get; set; }

        /// <summary>Special summons from Deck, Extra Deck, or GY continuously.</summary>
        public bool IsContinuousSummoner { get; set; }

        /// <summary>Draws cards from deck while on field.</summary>
        public bool IsContinuousDraw { get; set; }

        /// <summary>Quick Effect or disruption during either player's turn.</summary>
        public bool IsQuickInterruption { get; set; }

        /// <summary>Continuous floodgate or lock on game actions.</summary>
        public bool IsContinuousFloodgate { get; set; }

        /// <summary>Cannot be destroyed by card effects or battle.</summary>
        public bool IsDestructionImmune { get; set; }

        /// <summary>Cannot be targeted by card effects.</summary>
        public bool IsTargetImmune { get; set; }

        /// <summary>Triggers a beneficial effect (search/summon) when destroyed.</summary>
        public bool IsFloaterOnDestruction { get; set; }

        /// <summary>Passive stat boost or shield without generating resources or disruption.</summary>
        public bool IsDecoyShield { get; set; }
    }

    /// <summary>
    /// Dynamic Card Text Semantic Parser & Chokepoint Engine Evaluator.
    /// Analyzes raw card descriptions (in both English and Thai) to evaluate chokepoints,
    /// engine generators, decoys, immunities, and floating hazards dynamically in real-time.
    /// </summary>
    public static class CardTextSemantics
    {
        private static readonly ConcurrentDictionary<int, CardSemanticProfile> ProfileCache =
            new ConcurrentDictionary<int, CardSemanticProfile>();

        // ═══════════════════════════════════════════════════════════════
        //  PRECOMPILED REGEX PATTERNS (English & Thai Bilingual Support)
        // ═══════════════════════════════════════════════════════════════

        private static readonly Regex RegexCounter = new Regex(
            @"(วาง.*counter|นำ.*counter|counter.*ตัว|เคาน์เตอร์|bushido counter|spell counter|place.*counter|places.*counter|remove.*counter|each time.*place.*counter)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexSearch = new Regex(
            @"(จากเด็ค.*ขึ้นมือ|นำ.*ขึ้นมือ|ค้นหา|จากเด็คของคุณขึ้นมือ|จากเด็คขึ้นมือ|from (your|the) deck to (your|the) hand|add (\w+) .*from (your|the) deck)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexSummon = new Regex(
            @"(อัญเชิญแบบพิเศษ.*จากเด็ค|อัญเชิญแบบพิเศษ.*จาก extra deck|อัญเชิญแบบพิเศษ.*จากสุสาน|special summon.*from (your|the) deck|special summon.*from (your|the) extra deck|special summon.*from (your|the) (gy|graveyard))",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexDraw = new Regex(
            @"(จั่วการ์ด|จั่ว \d+ ใบ|draw \d+ card|draw cards)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexQuickTrigger = new Regex(
            @"(quick effect|ควิกเอฟเฟกต์|เทิร์นของฝ่ายตรงข้าม|เทิร์นของทั้งสองฝ่าย|during (your|either player's|the opponent's|opponent's) turn)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexDisruptionAction = new Regex(
            @"(ไร้ผล|ทำลาย|รีมูฟ|นำออกจากเกม|ส่งลงสุสาน|นำกลับขึ้นมือ|ยกเลิก|negate|destroy|banish|return .* to .* hand|send .* to the (gy|graveyard))",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexFloodgate = new Regex(
            @"(ผู้เล่นทั้งสองไม่สามารถ|ฝ่ายตรงข้ามไม่สามารถ|ไม่สามารถอัญเชิญแบบพิเศษ|ไม่สามารถเปิดใช้งาน|ยกเลิกเอฟเฟกต์ของมอนสเตอร์|ทำให้ไร้ผล|neither player can|opponent cannot|cannot special summon|cannot activate|effects of all face-up monsters.*are negated|cannot attack)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexDestructionImmune = new Regex(
            @"(ไม่สามารถถูกทำลาย|ไม่ถูกทำลาย|cannot be destroyed by|unaffected by (other|card|your opponent's)|cannot be destroyed)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexTargetImmune = new Regex(
            @"(ไม่สามารถถูกเลือกเป็นเป้าหมาย|ไม่ตกเป็นเป้าหมาย|cannot be targeted by|cannot target this card)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexFloater = new Regex(
            @"((เมื่อ|หาก)การ์ดใบนี้.*ถูกทำลาย.*:.*(อัญเชิญ|เพิ่ม|นำ.*ขึ้นมือ|จั่ว|ค้นหา)|(if|when) this card.*is destroyed.*:.*(special summon|add|draw|search))",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex RegexStatBoost = new Regex(
            @"(ได้รับ (atk|def)|gain.*(atk|def)|เพิ่ม (atk|def))",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Retrieves or analyzes the semantic profile of a card.
        /// </summary>
        public static CardSemanticProfile GetProfile(ClientCard card)
        {
            if (card == null) return null;
            return GetProfile(card.Id, card.Data?.Name, card.Data?.Description, card.Type);
        }

        /// <summary>
        /// Retrieves or analyzes the semantic profile by card ID and raw description.
        /// </summary>
        public static CardSemanticProfile GetProfile(int cardId, string name, string description, int cardType)
        {
            if (cardId <= 0) return null;

            return ProfileCache.GetOrAdd(cardId, id =>
            {
                var profile = new CardSemanticProfile
                {
                    CardId = id,
                    Name = name ?? string.Empty
                };

                if (string.IsNullOrEmpty(description))
                {
                    return profile;
                }

                string desc = description;

                profile.IsCounterGenerator = RegexCounter.IsMatch(desc);
                profile.IsContinuousSearcher = RegexSearch.IsMatch(desc);
                profile.IsContinuousSummoner = RegexSummon.IsMatch(desc);
                profile.IsContinuousDraw = RegexDraw.IsMatch(desc);

                bool hasQuickTrigger = RegexQuickTrigger.IsMatch(desc);
                bool hasDisruptAction = RegexDisruptionAction.IsMatch(desc);
                profile.IsQuickInterruption = hasQuickTrigger && hasDisruptAction;

                profile.IsContinuousFloodgate = RegexFloodgate.IsMatch(desc);
                profile.IsDestructionImmune = RegexDestructionImmune.IsMatch(desc);
                profile.IsTargetImmune = RegexTargetImmune.IsMatch(desc);
                profile.IsFloaterOnDestruction = RegexFloater.IsMatch(desc);

                bool hasStatBoost = RegexStatBoost.IsMatch(desc);
                bool generatesResources = profile.IsCounterGenerator || profile.IsContinuousSearcher ||
                                          profile.IsContinuousSummoner || profile.IsContinuousDraw ||
                                          profile.IsQuickInterruption || profile.IsContinuousFloodgate;

                profile.IsDecoyShield = (profile.IsDestructionImmune || profile.IsTargetImmune || hasStatBoost) && !generatesResources;

                return profile;
            });
        }

        /// <summary>
        /// Computes dynamic threat score based on card semantics and context (hint).
        /// </summary>
        public static int EvaluateCardThreat(ClientCard card, long hint = 0)
        {
            if (card == null) return 0;

            var profile = GetProfile(card);
            int score = 0;

            bool isContinuousLike = card.HasType(CardType.Continuous) || card.HasType(CardType.Field) || card.HasType(CardType.Equip);

            if (card.IsFaceup())
            {
                if (profile != null)
                {
                    // 1. Counter-based Engines (e.g. Gateway of the Six, Magical Citadel)
                    if (profile.IsCounterGenerator)
                    {
                        score += isContinuousLike ? 7000 : 4500;
                    }

                    // 2. Continuous Searchers / Tutors (e.g. Dark Magical Circle, Black Whirlwind)
                    if (profile.IsContinuousSearcher && !profile.IsFloaterOnDestruction)
                    {
                        score += isContinuousLike ? 6500 : 4000;
                    }

                    // 3. Continuous Special Summon from Deck/GY
                    if (profile.IsContinuousSummoner && !profile.IsFloaterOnDestruction)
                    {
                        score += 5500;
                    }

                    // 4. Continuous Draw Engine (e.g. Six Samurai United, Runick Fountain)
                    if (profile.IsContinuousDraw)
                    {
                        score += 5000;
                    }

                    // 5. Quick Disruptions / Negators
                    if (profile.IsQuickInterruption)
                    {
                        score += 7500;
                    }

                    // 6. Continuous Floodgates
                    if (profile.IsContinuousFloodgate)
                    {
                        score += 9000;
                    }

                    // 7. Decoy Shields (Passive protection / Stat boost with no generator)
                    if (profile.IsDecoyShield)
                    {
                        score += 1000;
                    }
                }

                // Extra deck monster baseline
                if (card.IsMonster() && card.IsExtraCard())
                {
                    score += 4000;
                }

                // Xyz overlay materials provide threat ammo
                if (card.Overlays != null && card.Overlays.Count > 0)
                {
                    score += card.Overlays.Count * 500;
                }
            }

            // ── ACTION / HINT SITUATIONAL MODIFIERS ──
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_TARGET = 551; // script/constant.lua (506 = ATOHAND)

            if (hint == HINTMSG_DESTROY)
            {
                // NEVER waste destruction removal on an indestructible card!
                if (profile != null && profile.IsDestructionImmune)
                {
                    score -= 20000;
                }

                // Penalize destroying floaters that trigger beneficial searches for opponent
                if (profile != null && profile.IsFloaterOnDestruction && card.Id != 48680970)
                {
                    score -= 8000;
                }

                // Exceptional Bonus: Eternal Soul wipes opponent's monsters on destruction!
                if (card.Id == 48680970)
                {
                    score += 15000;
                }
            }

            if (hint == HINTMSG_TARGET)
            {
                // Penalize selecting target-immune cards
                if (profile != null && profile.IsTargetImmune)
                {
                    score -= 20000;
                }
            }

            return score;
        }
    }
}
