// ============================================================================
// CARD AUDIT — Kashtira (Zone-Lockout Macro-Cosmos Control & Face-Down Banish)
// ============================================================================
// | Card Name                     | Type        | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |--------------------------------|-------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Kashtira Fenrir (32909498)     | Monster L7  | Yes  | Yes   | None    | SS if no monsters; search Kashtira monster;   | Main Phase 1 starter; face-down banish enemy | Monster zone full                           |
// | Kashtira Unicorn (68304193)    | Monster L7  | Yes  | Yes   | None    | SS if no monsters; search Kashtira S/T;       | Main Phase 1 starter; rip opp Extra Deck     | Monster zone full                           |
// | Kashtira Riseheart (31149212)  | Monster L4  | Yes  | Yes   | None    | SS if Kashtira on field; banish Kashtira deck | Extender into Rank 7; trigger Big Bang       | SS blocked / already Lv7                    |
// | Kashtira Ogre (94392192)       | Monster L7  | Yes  | Yes   | None    | SS if no monsters; search Kashtira Trap;      | Main Phase search / body                     | Monster zone full                           |
// | Scareclaw Kashtira (78534861)  | Monster L7  | Yes  | Yes   | Banish  | Quick SS by banishing Kashtira; negate battle | Battle Phase / Extender                      | No Kashtira to banish                        |
// | Tearlaments Kashtira (4928565) | Monster L7  | Yes  | Yes   | Banish  | Quick SS by banishing Kashtira; mill 3        | Extender                                     | No Kashtira to banish                        |
// | Pressured Planet Wraitsoth     | Spell Field | Yes  | Yes   | None    | Search Kashtira monster; pop 1 card on lock   | Main Phase 1 priority search                 | Already on field                            |
// | Kashtiratheosis (34447918)     | Spell Normal| Yes  | Yes   | Target  | SS Kashtira from Deck with different attr     | Target Unicorn -> SS Fenrir/Riseheart        | No valid target on field/deck                |
// | Kashtira Birth (69540484)      | Spell Cont  | Yes  | Yes   | None    | NS Lv7 without tribute; revive Kashtira; banish| Main Phase 1 normal / revive from GY/banish  | Already active on field                     |
// | Kashtira Big Bang (33925864)   | Trap Normal | Yes  | Yes   | Detach  | Non-targeting board shrink; banish to attach  | Banished by Riseheart / reactive defense      | No Shangri-Ira on field                     |
// | Kashtira Preparations (21639276)| Trap Cont   | Yes  | Yes   | None    | SS Kashtira from banished/hand; rip opp hand  | Opponent turn summon & hand rip              | No Kashtira in hand/banish                  |
// | Kashtira Shangri-Ira (73542331)| Xyz Rank 7  | Yes  | Yes   | Detach  | Standby SS from Deck; lock opp zone on banish | Extra Deck Turn 1 Boss #1                     | Opponent zones all locked                   |
// | Kashtira Arise-Heart (48626373)| Xyz Rank 7  | No   | No    | 3 Mat   | Macro Cosmos on legs; attach banished; banish | Extra Deck Turn 1 Boss #2 (1-card overlay)    | Less than 3 materials for banish            |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Kashtira", "2026_Kashtira")]
    [Deck("Kashtira", "2026_Kashtira")]
    [Deck("Kashtira Stun", "2026_Kashtira")]
    public class KashtiraExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int KashtiraFenrir = 32909498;
            public const int KashtiraUnicorn = 68304193;
            public const int KashtiraRiseheart = 31149212;
            public const int KashtiraOgre = 94392192;
            public const int ScareclawKashtira = 78534861;
            public const int TearlamentsKashtira = 4928565;
            public const int DimensionShifter = 91800273;
            public const int AshBlossom = 14558127;
            public const int Nibiru = 27204311;

            // Spells & Traps
            public const int PressuredPlanetWraitsoth = 71832012;
            public const int Terraforming = 73628505;
            public const int Kashtiratheosis = 34447918;
            public const int KashtiraBirth = 69540484;
            public const int PotOfProsperity = 84211599;
            public const int TripleTacticsTalent = 25311006;
            public const int BookOfMoon = 14087893;
            public const int KashtiraPreparations = 21639276;
            public const int KashtiraBigBang = 33925864;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int KashtiraShangriIra = 73542331;
            public const int KashtiraAriseHeart = 48626373;
            public const int DivineArsenalAAZEUS = 90448279;
            public const int DarkArmedDragonOfAnnihilation = 78144171;
            public const int RedEyesFlareMetalDragon = 44405066;
            public const int MechaPhantomBeastDracossack = 22110647;
            public const int Number11BigEye = 80117527;
            public const int Number89Diablosis = 95474755;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int InfinitrackGoliath = 23689428;
            public const int MereologicAggregator = 9940036;
        }

        private readonly KashtiraPlugin _plugin;

        public KashtiraExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new KashtiraPlugin(this);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 0: EMERGENCY COUNTERS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, ShifterCondition);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomCondition);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruCondition);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 1: SEARCH ENGINES & FIELD SPELLS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingCondition);
            AddExecutor(ExecutorType.Activate, CardId.PressuredPlanetWraitsoth, WraitsothActivateCondition);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, ProsperityCondition);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 2: SPECIAL SUMMON STARTERS (NO MONSTERS CONTROLLED)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraUnicorn, StarterSpSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraFenrir, StarterSpSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraOgre, StarterSpSummonCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 3: ON-FIELD STARTER SEARCH EFFECTS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.KashtiraUnicorn, UnicornSearchCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraFenrir, FenrirSearchCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraOgre, OgreSearchCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 4: EXTENDERS (THEOSIS, BIRTH, RISEHEART)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Kashtiratheosis, TheosisCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraBirth, BirthActivateCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraBirth, BirthReviveCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraRiseheart, RiseheartSpSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraRiseheart, RiseheartEffectCondition);

            // Normal Summon Birth Tribute-Free or Normal Summon
            AddExecutor(ExecutorType.Summon, CardId.KashtiraFenrir, NormalSummonLv7Condition);
            AddExecutor(ExecutorType.Summon, CardId.KashtiraUnicorn, NormalSummonLv7Condition);
            AddExecutor(ExecutorType.Summon, CardId.KashtiraOgre, NormalSummonLv7Condition);
            AddExecutor(ExecutorType.Summon, CardId.KashtiraRiseheart, RiseheartNormalCondition);

            // Quick Summon Extenders
            AddExecutor(ExecutorType.SpSummon, CardId.ScareclawKashtira, ScareclawSpSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.TearlamentsKashtira, TearlamentsSpSummonCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 5: EXTRA DECK XYZ SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraShangriIra, ShangriIraSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraShangriIra, ShangriIraEffectCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraAriseHeart, AriseHeartSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraAriseHeart, AriseHeartEffectCondition);

            // Alternative Rank 7 Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.Number11BigEye, BigEyeSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.Number11BigEye);
            AddExecutor(ExecutorType.SpSummon, CardId.RedEyesFlareMetalDragon, FlareMetalSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.DarkArmedDragonOfAnnihilation, DarkArmedSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.DarkArmedDragonOfAnnihilation);

            // Board Wipe & Recovery
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUS, ZeusSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUS, ZeusEffectCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyphonSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 6: TRAPS & REACTIVE DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.KashtiraBigBang, BigBangCondition);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraPreparations, PreparationsCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.KashtiraPreparations);
            AddExecutor(ExecutorType.SpellSet, CardId.KashtiraBigBang);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon, SpellSetTrapCondition);

            // Smart Repositioning
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        //  CONDITIONS & STRATEGY HOOKS
        // ═══════════════════════════════════════════════════════════════

        private bool ShifterCondition()
        {
            return Bot.Graveyard.Count == 0 && Duel.Turn <= 2;
        }

        private bool ImpermanenceCondition()
        {
            return DefaultInfiniteImpermanence();
        }

        private bool AshBlossomCondition()
        {
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool NibiruCondition()
        {
            return Enemy.GetMonsterCount() >= 3 && DefaultNibiru();
        }

        private bool BookOfMoonCondition()
        {
            if (Duel.Player == 1)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.HasType(CardType.Link) && m.Attack >= 2000);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TerraformingCondition()
        {
            return !Bot.HasInHandOrInSpellZone(CardId.PressuredPlanetWraitsoth);
        }

        private bool WraitsothActivateCondition()
        {
            return !Bot.HasInSpellZone(CardId.PressuredPlanetWraitsoth);
        }

        private bool ProsperityCondition()
        {
            return Bot.ExtraDeck.Count >= 6 && Bot.Hand.Count <= 4;
        }

        private bool TripleTacticsCondition()
        {
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(1); // Steal 1 monster
                return true;
            }
            AI.SelectOption(0); // Draw 2 cards
            return true;
        }

        private bool StarterSpSummonCondition()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool UnicornSearchCondition()
        {
            // Search Priority: Theosis > Birth > Wraitsoth
            return true;
        }

        private bool FenrirSearchCondition()
        {
            // If in battle/trigger: banish face-down
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Player == 1)
            {
                var banishTarget = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault()
                    ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup());
                if (banishTarget != null)
                {
                    AI.SelectCard(banishTarget);
                    return true;
                }
            }
            return true;
        }

        private bool OgreSearchCondition()
        {
            return true;
        }

        private bool TheosisCondition()
        {
            var target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0x189) && !m.HasType(CardType.Xyz));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BirthActivateCondition()
        {
            return !Bot.HasInSpellZone(CardId.KashtiraBirth);
        }

        private bool BirthReviveCondition()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                var target = Bot.Graveyard.Concat(Bot.Banished)
                    .FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x189) && !c.HasType(CardType.Xyz));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool RiseheartSpSummonCondition()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x189));
        }

        private bool RiseheartEffectCondition()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Level == 4)
            {
                // Banish Big Bang from Deck to trigger material detach, or banish Kashtira card to become Level 7!
                return true;
            }
            return false;
        }

        private bool NormalSummonLv7Condition()
        {
            if (Bot.HasInSpellZone(CardId.KashtiraBirth)) return true;
            return Bot.GetMonsterCount() == 0;
        }

        private bool RiseheartNormalCondition()
        {
            return Bot.GetMonsterCount() == 0 || Bot.GetMonsters().Any(m => m.Level == 7);
        }

        private bool ScareclawSpSummonCondition()
        {
            return Bot.Graveyard.Any(c => c.HasSetcode(0x189)) || Bot.Hand.Any(c => c.HasSetcode(0x189) && c != Card);
        }

        private bool TearlamentsSpSummonCondition()
        {
            return Bot.Graveyard.Any(c => c.HasSetcode(0x189)) || Bot.Hand.Any(c => c.HasSetcode(0x189) && c != Card);
        }

        private bool ShangriIraSummonCondition()
        {
            if (Bot.HasInMonstersZone(CardId.KashtiraShangriIra)) return false;
            var lv7 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 7).ToList();
            return lv7.Count >= 2;
        }

        private bool ShangriIraEffectCondition()
        {
            // Standby Phase: Summon Fenrir or Unicorn from Deck!
            return true;
        }

        private bool AriseHeartSummonCondition()
        {
            if (Bot.HasInMonstersZone(CardId.KashtiraAriseHeart)) return false;
            // Can overlay with 3 Level 7s or on top of any Kashtira monster if Shangri-Ira used effect
            var lv7 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 7).ToList();
            if (lv7.Count >= 3) return true;
            if (Bot.HasInMonstersZone(CardId.KashtiraShangriIra))
            {
                var baseKashtira = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0x189) && m.Id != CardId.KashtiraShangriIra);
                return baseKashtira != null;
            }
            return false;
        }

        private bool AriseHeartEffectCondition()
        {
            // Quick effect: detach 3 materials -> banish 1 card on field face-down
            if (Card.Overlays.Count >= 3)
            {
                var target = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault()
                    ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        private bool BigEyeSummonCondition()
        {
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500) &&
                   Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 7) >= 2;
        }

        private bool FlareMetalSummonCondition()
        {
            return Enemy.LifePoints <= 2000 && Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 7) >= 2;
        }

        private bool DarkArmedSummonCondition()
        {
            return Enemy.GetMonsterCount() >= 2 && Bot.Banished.Count >= 5 &&
                   Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 7) >= 2;
        }

        private bool ZeusSummonCondition()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Xyz));
        }

        private bool ZeusEffectCondition()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool TyphonSummonCondition()
        {
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 3000);
        }

        private bool BigBangCondition()
        {
            return Card.Location == CardLocation.Removed || Card.Location == CardLocation.SpellZone;
        }

        private bool PreparationsCondition()
        {
            return true;
        }

        private bool SpellSetTrapCondition()
        {
            return Duel.Phase == DuelPhase.Main2 || (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1);
        }

        // ═══════════════════════════════════════════════════════════════
        //  SMART POSITION & REPOSITIONING
        // ═══════════════════════════════════════════════════════════════

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // All Kashtira Bosses & Attackers -> FaceUpAttack
            int[] forceAttackMonsters = {
                CardId.KashtiraFenrir,
                CardId.KashtiraUnicorn,
                CardId.KashtiraRiseheart,
                CardId.KashtiraOgre,
                CardId.KashtiraAriseHeart,
                CardId.DivineArsenalAAZEUS,
                CardId.Number11BigEye,
                CardId.DarkArmedDragonOfAnnihilation,
                CardId.RedEyesFlareMetalDragon
            };

            if (forceAttackMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            // Shangri-Ira: 0 ATK / 3000 DEF -> ALWAYS DEFENSE!
            if (cardId == CardId.KashtiraShangriIra)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            // Handtraps & Walls -> Strictly Defense
            int[] forceDefenceMonsters = {
                CardId.AshBlossom,
                CardId.DimensionShifter,
                CardId.Nibiru
            };

            if (forceDefenceMonsters.Contains(cardId))
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // 1. Shangri-Ira stranded in Attack -> Switch to Defense!
            if (Card.Id == CardId.KashtiraShangriIra && Card.IsAttack())
            {
                return true;
            }

            // 2. Handtraps stranded in Attack -> Switch to Defense!
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
            {
                return true;
            }

            // 3. High ATK monsters in Defense -> Switch to Attack for offensive pushes
            if (Card.IsDefense() && Card.Attack >= 2400 && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
            {
                if (Card.Id != CardId.KashtiraShangriIra)
                    return true;
            }

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SAFE CARD & ZONE SELECTION (OCGCORE PROTOCOL ALIGNED)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Deck Search (HINTMSG_ATOHAND = 506)
            if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
            {
                if (min <= 1 && 1 <= max)
                {
                    // Priority: Wraitsoth > Fenrir > Unicorn > Theosis > Birth > Riseheart
                    var bestSearch = cards.FirstOrDefault(c => c.Id == CardId.PressuredPlanetWraitsoth)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraFenrir && !Bot.HasInHand(CardId.KashtiraFenrir))
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraUnicorn && !Bot.HasInHand(CardId.KashtiraUnicorn))
                        ?? cards.FirstOrDefault(c => c.Id == CardId.Kashtiratheosis && !Bot.HasInHand(CardId.Kashtiratheosis))
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraBirth && !Bot.HasInHand(CardId.KashtiraBirth))
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraRiseheart)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraPreparations);

                    if (bestSearch != null) return new List<ClientCard> { bestSearch };
                }
            }

            // 2. Face-Down Banish Removal (HINTMSG_REMOVE = 503) -> ALWAYS TARGET OPPONENT!
            if (hint == 503)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    var sorted = oppCards.OrderByDescending(c => CardIntelligence.IsKnownNegator(c.Id) ? 10000 : 0)
                        .ThenByDescending(c => c.Attack)
                        .ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // 3. Destruction (HINTMSG_DESTROY = 502) -> ALWAYS TARGET OPPONENT!
            if (hint == 502)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    var sorted = oppCards.OrderByDescending(c => c.Attack).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // 4. Special Summon from Deck (Theosis / Shangri-Ira)
            if (hint == 509)
            {
                if (min <= 1 && 1 <= max)
                {
                    var ssTarget = cards.FirstOrDefault(c => c.Id == CardId.KashtiraFenrir)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraUnicorn)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraRiseheart)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.KashtiraOgre);
                    if (ssTarget != null) return new List<ClientCard> { ssTarget };
                }
            }

            // 5. Riseheart Banish Cost from Deck
            var bigBangDeck = cards.FirstOrDefault(c => c.Id == CardId.KashtiraBigBang && c.Location == CardLocation.Deck);
            if (bigBangDeck != null && min <= 1 && 1 <= max)
            {
                return new List<ClientCard> { bigBangDeck };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Shangri-Ira Zone Locking: Lock Middle Monster Zones first, then EMZ, then S/T Zones
            return _plugin.ZoneLockManager.SelectLockZone(available, location);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  DEDICATED DOMAIN PLUGIN ARCHITECTURE FOR KASHTIRA
    // ════════════════════════════════════════════════════════════════════════
}

