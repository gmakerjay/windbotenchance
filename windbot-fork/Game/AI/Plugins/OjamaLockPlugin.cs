using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════════════════
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: OjamaLockPlugin
    // Implements Strategy, MaterialEvaluator, ThreatEvaluator, and ZoneLockManager
    // for Ojama 5-Zone Complete Lock & ABC-Dragon Buster Hybrid
    // ═══════════════════════════════════════════════════════════════════════════
    public class OjamaLockPlugin : DeckPluginBase
    {
        private readonly OjamaLockExecutor _exec;

        public override string DeckName => "OjamaLock";

        public OjamaLockStrategy StrategyImpl { get; }
        public OjamaLockMaterialEvaluator MaterialImpl { get; }
        public OjamaLockThreatEvaluator ThreatImpl { get; }
        public OjamaZoneLockManager ZoneLockManager { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public OjamaLockPlugin(OjamaLockExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new OjamaLockStrategy(exec);
            MaterialImpl = new OjamaLockMaterialEvaluator(exec);
            ThreatImpl = new OjamaLockThreatEvaluator(exec);
            ZoneLockManager = new OjamaZoneLockManager(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class OjamaZoneLockManager
    {
        private readonly OjamaLockExecutor _exec;

        public OjamaZoneLockManager(OjamaLockExecutor exec)
        {
            _exec = exec;
        }

        public int SelectLockZone(int availableZones, CardLocation location)
        {
            // When locking opponent Monster Zones (Ojama King 3 zones, Ojama Knight 2 zones,
            // Ground Collapse 2 zones, Ojama Pink 1 zone):
            // Center (Zone 2 = 0x4) -> Left-Center (Zone 1 = 0x2) -> Right-Center (Zone 3 = 0x8)
            // -> Left-Edge (Zone 0 = 0x1) -> Right-Edge (Zone 4 = 0x10)
            int[] monsterPref = { 0x4, 0x2, 0x8, 0x1, 0x10, 0x20, 0x40 };
            foreach (int mask in monsterPref)
            {
                if ((availableZones & mask) != 0) return mask;
            }

            return availableZones;
        }
    }

    public class OjamaLockStrategy : IDeckStrategy
    {
        private readonly OjamaLockExecutor _exec;
        public OjamaLockStrategy(OjamaLockExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ojamassimilation: SS ABC pieces (B > A > C)
            var buster = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.BBusterDrake);
            if (buster != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.BBusterDrake))
                return buster;

            var assault = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.AAssaultCore);
            if (assault != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.AAssaultCore))
                return assault;

            var crush = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
            if (crush != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.CCrushWyvern))
                return crush;

            // 2. Tri-Wight: Normal Ojamas (Green, Yellow, Black)
            var normalOjama = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack);
            if (normalOjama != null) return normalOjama;

            // 3. Ojama Country revival: Ojama King > Ojama Knight > Ojama Emperor > Ojama Red
            var king = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaKing);
            if (king != null) return king;

            var knight = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaKnight);
            if (knight != null) return knight;

            var emperor = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaEmperor);
            if (emperor != null) return emperor;

            // 4. Ojama Duo / Ojama Red from hand: Ojama Red > Ojama Blue > Ojama Pink
            var red = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaRed);
            if (red != null) return red;

            var blue = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaBlue);
            if (blue != null) return blue;

            var pink = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPink);
            if (pink != null) return pink;

            // 5. Armed Dragon Thunder
            var dragon = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV5);
            if (dragon != null) return dragon;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ojamagic if we have an active discard outlet (Ojama Pajama, Ojama Country, Ojamatch)
            bool hasDiscardOutlet = _exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaPajama) ||
                                    _exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaCountry) ||
                                    _exec.Bot.HasInHand(OjamaLockExecutor.CardId.Ojamatch) ||
                                    _exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaCountry);
            if (hasDiscardOutlet && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.Ojamagic))
            {
                var magic = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Ojamagic);
                if (magic != null) return magic;
            }

            // 2. Ojamassimilation (The core enabler for ABC-Dragon Buster)
            if (!_exec.Bot.HasInHand(OjamaLockExecutor.CardId.Ojamassimilation))
            {
                var sim = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Ojamassimilation);
                if (sim != null) return sim;
            }

            // 3. Ojama Pajama (Searcher & protection continuous trap)
            if (!_exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaPajama) &&
                !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaPajama))
            {
                var pajama = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPajama);
                if (pajama != null) return pajama;
            }

            // 4. Ojamatch (Extension + Normal Summon + Armed Dragon search)
            if (!_exec.Bot.HasInHand(OjamaLockExecutor.CardId.Ojamatch))
            {
                var match = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Ojamatch);
                if (match != null) return match;
            }

            // 5. Ojama Country (Field spell ATK/DEF invert & GY revive)
            if (!_exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaCountry) &&
                !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaCountry))
            {
                var country = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaCountry);
                if (country != null) return country;
            }

            // 6. Ojama Red (Swarm engine from hand)
            if (!_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaRed))
            {
                var red = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaRed);
                if (red != null) return red;
            }

            // 7. Ojama Pink (Hand refresh + Zone Lock)
            if (!_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaPink))
            {
                var pink = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPink);
                if (pink != null) return pink;
            }

            // 8. Normal Ojamas to complete Ojama King requirement
            bool hasGreen = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaGreen);
            bool hasYellow = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaYellow);
            bool hasBlack = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaBlack);

            if (!hasGreen)
            {
                var green = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaGreen);
                if (green != null) return green;
            }
            if (!hasYellow)
            {
                var yellow = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaYellow);
                if (yellow != null) return yellow;
            }
            if (!hasBlack)
            {
                var black = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaBlack);
                if (black != null) return black;
            }

            // 9. Armed Dragon Thunder LV3 / LV5 for Ojamatch
            var dragon3 = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV3);
            if (dragon3 != null) return dragon3;

            var dragon5 = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV5);
            if (dragon5 != null) return dragon5;

            return candidates.FirstOrDefault();
        }
    }

    public class OjamaLockMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly OjamaLockExecutor _exec;
        public OjamaLockMaterialEvaluator(OjamaLockExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Tokens: Zero cost, perfect fodder
            if (c.HasType(CardType.Token) || c.Id == OjamaLockExecutor.CardId.OjamaToken)
                return 1;

            // Normal Ojamas: Low cost fodder
            if (c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack)
                return 2;

            // Effect Ojamas: Medium cost
            if (c.Id == OjamaLockExecutor.CardId.OjamaBlue ||
                c.Id == OjamaLockExecutor.CardId.OjamaRed ||
                c.Id == OjamaLockExecutor.CardId.OjamaPink)
                return 3;

            // Armed Dragon Thunder LV3 / LV5: Medium cost
            if (c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV3 ||
                c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV5)
                return 4;

            // ABC pieces on field: High cost until ready to Contact Fuse into ABC-Dragon Buster
            if (c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                c.Id == OjamaLockExecutor.CardId.CCrushWyvern)
                return 5;

            // Extra Deck Bosses & Zone Lockers: NEVER sacrifice or use as material
            if (c.Id == OjamaLockExecutor.CardId.OjamaKing ||
                c.Id == OjamaLockExecutor.CardId.OjamaKnight ||
                c.Id == OjamaLockExecutor.CardId.ABCDragonBuster ||
                c.Id == OjamaLockExecutor.CardId.OjamaEmperor ||
                c.Id == OjamaLockExecutor.CardId.AccesscodeTalker ||
                c.Id == OjamaLockExecutor.CardId.SPLittleKnight ||
                c.Id == OjamaLockExecutor.CardId.RoninRaccoonSandayu ||
                c.Id == OjamaLockExecutor.CardId.SkyCavalryCentaurea)
                return 100;

            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ojamagic (Trigger +3: adds Green, Yellow, Black to hand!)
            var magic = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Ojamagic);
            if (magic != null) return magic;

            // 2. Armed Dragon Thunder LV3 (Draws 1 when sent to GY for dragon effect)
            var lv3 = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.ArmedDragonThunderLV3);
            if (lv3 != null) return lv3;

            // 3. Ojama Duo (Has GY banish effect to summon 2 Ojamas from deck!)
            var duo = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaDuo);
            if (duo != null) return duo;

            // 4. Ojama Pink (Draw 1, discard 1, lock 1 opp zone!)
            var pink = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPink);
            if (pink != null) return pink;

            // 5. ABC pieces (Sets up GY banish for ABC-Dragon Buster!)
            var abc = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
            if (abc != null) return abc;

            // 6. Duplicate Normal Ojamas
            var duplicateOjama = candidates.GroupBy(c => c.Id)
                .Where(g => g.Count() > 1 &&
                    (g.Key == OjamaLockExecutor.CardId.OjamaGreen ||
                     g.Key == OjamaLockExecutor.CardId.OjamaYellow ||
                     g.Key == OjamaLockExecutor.CardId.OjamaBlack))
                .SelectMany(g => g).FirstOrDefault();
            if (duplicateOjama != null) return duplicateOjama;

            // 7. Any Normal Ojama
            var normal = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack);
            if (normal != null) return normal;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            // Ojama Pajama substitute: Banish Ojama card from GY first
            var normalGY = candidates?.FirstOrDefault(c =>
                c.Location == CardLocation.Grave &&
                (c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                 c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                 c.Id == OjamaLockExecutor.CardId.OjamaBlack));
            if (normalGY != null) return normalGY;

            return candidates?.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public IList<ClientCard> PickOjamassimilationBanish(IList<ClientCard> candidates, int count)
        {
            // Banish order: GY Normal Ojamas > GY Effect Ojamas > Field Normal Ojamas > Hand Normal Ojamas
            if (candidates == null) return new List<ClientCard>();

            var sorted = candidates.OrderBy(c =>
            {
                if (c.Location == CardLocation.Grave)
                {
                    if (c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                        c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                        c.Id == OjamaLockExecutor.CardId.OjamaBlack)
                        return 1;
                    return 2;
                }
                if (c.Location == CardLocation.MonsterZone) return 3;
                return 4; // Hand
            }).ToList();

            return sorted.Take(count).ToList();
        }
    }

    public class OjamaLockThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly OjamaLockExecutor _exec;
        public OjamaLockThreatEvaluator(OjamaLockExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            // Mass Backrow Wipes
            if (c.Id == 18144506 || c.Id == 18144507 || c.Id == 14532163 || c.Id == 15693423 || c.Id == 43898403)
                score += 90;

            // Continuous Floodgates
            if (c.Id == 82732047 || c.Id == 82732705 || c.Id == 30241314)
                score += 85;

            if (c.IsFaceup())
            {
                if (c.HasType(CardType.Monster))
                {
                    if (c.Attack >= 3000) score += 30;
                    if (CardIntelligence.IsKnownNegator(c.Id)) score += 50;
                    if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 40;
                }
                else if (c.HasType(CardType.Spell) || c.HasType(CardType.Trap))
                {
                    if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) score += 35;
                }
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return EvaluateThreatScore(c) >= 70;
        }
    }
}
