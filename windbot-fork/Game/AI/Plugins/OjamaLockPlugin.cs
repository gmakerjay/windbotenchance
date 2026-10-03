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
    // for Championship Grade Ojama ABC Lockdown (Ojama King 3-Zone Lock +
    // ABC-Dragon Buster Quick Banish + Therion Regulus Omni-Negate Protection)
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

        /// <summary>
        /// Handles OCGCore SelectDisableField (Ojama King, Ojama Knight, Ground Collapse).
        /// Bit layout for raw available mask:
        /// Bits 16..20: Opponent MMZ sequences 0..4
        /// Bits 21..22: Opponent EMZ sequences 5..6
        /// Bits 0..4: Player MMZ sequences 0..4
        /// </summary>
        public uint SelectDisfieldZones(int count, uint available)
        {
            // Center (Zone 2 = bit 18) -> Left-Center (Zone 1 = bit 17) -> Right-Center (Zone 3 = bit 19)
            // -> Left-Edge (Zone 0 = bit 16) -> Right-Edge (Zone 4 = bit 20) -> EMZ (bits 21, 22)
            int[] oppMonsterBits = { 18, 17, 19, 16, 20, 21, 22 };
            uint selected = 0;
            int chosenCount = 0;

            foreach (int bit in oppMonsterBits)
            {
                uint mask = 1u << bit;
                if ((available & mask) != 0)
                {
                    selected |= mask;
                    chosenCount++;
                    if (chosenCount >= count) return selected;
                }
            }

            // Fallback: pick any remaining available bits to satisfy count
            for (int bit = 0; bit < 32; ++bit)
            {
                uint mask = 1u << bit;
                if ((available & mask) != 0 && (selected & mask) == 0)
                {
                    selected |= mask;
                    chosenCount++;
                    if (chosenCount >= count) return selected;
                }
            }

            return selected;
        }

        /// <summary>
        /// Handles OCGCore SelectPlace (Ojama Pink, monster summoning zone selection).
        /// </summary>
        public int SelectLockPlace(int available, CardLocation location, int player)
        {
            if (player == 1)
            {
                // Locking opponent zone (e.g. Ojama Pink)
                // Center (Zone 2 = 0x4) -> Left-Center (Zone 1 = 0x2) -> Right-Center (Zone 3 = 0x8)
                // -> Left-Edge (Zone 0 = 0x1) -> Right-Edge (Zone 4 = 0x10) -> EMZ (0x20, 0x40)
                int[] monsterPref = { 0x4, 0x2, 0x8, 0x1, 0x10, 0x20, 0x40 };
                foreach (int mask in monsterPref)
                {
                    if ((available & mask) != 0) return mask;
                }
                return available;
            }

            // Placing our own monster: Prefer MMZ (Zone 2, 1, 3, 0, 4) and avoid EMZ (Rule 11) unless Link
            int[] ourMonsterPref = { 0x4, 0x2, 0x8, 0x1, 0x10 };
            foreach (int mask in ourMonsterPref)
            {
                if ((available & mask) != 0) return mask;
            }

            return available;
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

            // 1. ABC pieces via Ojamassimilation: B-Buster Drake > A-Assault Core > C-Crush Wyvern
            var buster = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.BBusterDrake);
            if (buster != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.BBusterDrake))
                return buster;

            var assault = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.AAssaultCore);
            if (assault != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.AAssaultCore))
                return assault;

            var crush = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
            if (crush != null && !_exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.CCrushWyvern))
                return crush;

            // 2. Therion "King" Regulus (2800 ATK Omni-Negate)
            var regulus = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.TherionKingRegulus);
            if (regulus != null) return regulus;

            // 3. Fusion Bosses (from GY via Ojama Emperor)
            var king = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaKing);
            if (king != null) return king;

            var knight = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaKnight);
            if (knight != null) return knight;

            var emperor = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaEmperor);
            if (emperor != null) return emperor;

            // 4. Normal Ojamas (Green, Yellow, Black)
            var normalOjama = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack);
            if (normalOjama != null) return normalOjama;

            // 5. Effect Ojamas
            var red = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaRed);
            if (red != null) return red;

            var blue = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaBlue);
            if (blue != null) return blue;

            var pink = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPink);
            if (pink != null) return pink;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // ── A. UNION / MACHINE SEARCH (via B-Buster Drake or Union Hangar) ──
            bool hasUnionTarget = candidates.Any(c =>
                c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                c.Id == OjamaLockExecutor.CardId.CCrushWyvern ||
                c.Id == OjamaLockExecutor.CardId.TherionKingRegulus ||
                c.Id == OjamaLockExecutor.CardId.UnionDriver);

            if (hasUnionTarget)
            {
                // 1. If we have a Machine in GY/Field and no Regulus in hand, search Therion "King" Regulus!
                bool hasMachineGY = _exec.Bot.Graveyard.Any(c => c.HasRace(CardRace.Machine) && c.IsMonster());
                if (hasMachineGY && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.TherionKingRegulus))
                {
                    var reg = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.TherionKingRegulus);
                    if (reg != null) return reg;
                }

                // 2. Search missing ABC piece
                bool hasA = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.AAssaultCore) ||
                            _exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.AAssaultCore) ||
                            _exec.Bot.HasInGraveyard(OjamaLockExecutor.CardId.AAssaultCore);
                bool hasB = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.BBusterDrake) ||
                            _exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.BBusterDrake) ||
                            _exec.Bot.HasInGraveyard(OjamaLockExecutor.CardId.BBusterDrake);
                bool hasC = _exec.Bot.HasInHand(OjamaLockExecutor.CardId.CCrushWyvern) ||
                            _exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.CCrushWyvern) ||
                            _exec.Bot.HasInGraveyard(OjamaLockExecutor.CardId.CCrushWyvern);

                if (!hasB)
                {
                    var b = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.BBusterDrake);
                    if (b != null) return b;
                }
                if (!hasA)
                {
                    var a = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.AAssaultCore);
                    if (a != null) return a;
                }
                if (!hasC)
                {
                    var c = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
                    if (c != null) return c;
                }

                var fallbackUnion = candidates.FirstOrDefault(c =>
                    c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                    c.Id == OjamaLockExecutor.CardId.TherionKingRegulus ||
                    c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                    c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
                if (fallbackUnion != null) return fallbackUnion;
            }

            // ── B. OJAMA SEARCH (via Ojama Pajama or Ojama Blue) ──
            // 0. King of the Swamp search Polymerization
            if (contextCard?.Id == OjamaLockExecutor.CardId.KingOfTheSwamp)
            {
                var poly = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Polymerization);
                if (poly != null) return poly;
            }

            // 1. Ojamagic if we have an active discard outlet (Ojama Pajama or ABC Buster) to get +3 hand advantage!
            bool hasDiscardOutlet = _exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaPajama) ||
                                    _exec.Bot.HasInMonstersZone(OjamaLockExecutor.CardId.ABCDragonBuster);
            if (hasDiscardOutlet && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.Ojamagic))
            {
                var magic = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.Ojamagic);
                if (magic != null) return magic;
            }

            // 2. Ojamassimilation (The core engine for ABC-Dragon Buster)
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

            // 3b. Ojama Country (Field spell ATK/DEF swap & revive engine)
            if (!_exec.Bot.HasInSpellZone(OjamaLockExecutor.CardId.OjamaCountry) &&
                !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaCountry))
            {
                var country = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaCountry);
                if (country != null) return country;
            }

            // 4. Ojama Red (Swarm engine from hand)
            if (!_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaRed))
            {
                var red = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaRed);
                if (red != null) return red;
            }

            // 5. Normal Ojamas to complete Fusion requirements
            var green = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaGreen);
            if (green != null && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaGreen)) return green;

            var yellow = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaYellow);
            if (yellow != null && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaYellow)) return yellow;

            var black = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaBlack);
            if (black != null && !_exec.Bot.HasInHand(OjamaLockExecutor.CardId.OjamaBlack)) return black;

            // 6. Ojama Blue (Battle float searcher)
            var blue = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaBlue);
            if (blue != null) return blue;

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

            // High-ATK Tokens (e.g. Kagemusha Raccoon Token copying 3000 ATK): NEVER sacrifice
            if (c.HasType(CardType.Token) && c.Attack >= 2000)
                return 100;

            // Tokens: Zero cost, perfect fodder
            if (c.HasType(CardType.Token) || c.Id == OjamaLockExecutor.CardId.OjamaToken)
                return 1;

            // Normal Ojamas & King of the Swamp: Low cost fodder
            if (c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack ||
                c.Id == OjamaLockExecutor.CardId.KingOfTheSwamp)
                return 2;

            // Effect Ojamas: Medium cost
            if (c.Id == OjamaLockExecutor.CardId.OjamaBlue ||
                c.Id == OjamaLockExecutor.CardId.OjamaRed ||
                c.Id == OjamaLockExecutor.CardId.OjamaPink)
                return 3;

            // ABC pieces on field: Medium cost until ready to Contact Fuse into ABC-Dragon Buster
            if (c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                c.Id == OjamaLockExecutor.CardId.CCrushWyvern)
                return 5;

            // Extra Deck Bosses & Zone Lockers: NEVER sacrifice or use as material
            if (c.Id == OjamaLockExecutor.CardId.OjamaKing ||
                c.Id == OjamaLockExecutor.CardId.OjamaKnight ||
                c.Id == OjamaLockExecutor.CardId.ABCDragonBuster ||
                c.Id == OjamaLockExecutor.CardId.TherionKingRegulus ||
                c.Id == OjamaLockExecutor.CardId.PlatinumGadget ||
                c.Id == OjamaLockExecutor.CardId.OjamaEmperor ||
                c.Id == OjamaLockExecutor.CardId.SPLittleKnight ||
                c.Id == OjamaLockExecutor.CardId.IPMasquerena ||
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

            // 2. Ojama Pink (Draw 1, discard 1, lock 1 opp zone!)
            var pink = candidates.FirstOrDefault(c => c.Id == OjamaLockExecutor.CardId.OjamaPink);
            if (pink != null) return pink;

            // 3. ABC pieces (Sets up GY banish for ABC-Dragon Buster!)
            var abc = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.AAssaultCore ||
                c.Id == OjamaLockExecutor.CardId.BBusterDrake ||
                c.Id == OjamaLockExecutor.CardId.CCrushWyvern);
            if (abc != null) return abc;

            // 4. Duplicate Normal Ojamas
            var duplicateOjama = candidates.GroupBy(c => c.Id)
                .Where(g => g.Count() > 1 &&
                    (g.Key == OjamaLockExecutor.CardId.OjamaGreen ||
                     g.Key == OjamaLockExecutor.CardId.OjamaYellow ||
                     g.Key == OjamaLockExecutor.CardId.OjamaBlack))
                .SelectMany(g => g).FirstOrDefault();
            if (duplicateOjama != null) return duplicateOjama;

            // 5. Any Normal Ojama
            var normal = candidates.FirstOrDefault(c =>
                c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                c.Id == OjamaLockExecutor.CardId.OjamaBlack);
            if (normal != null) return normal;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Ojama Pajama substitute: Banish Ojama card from GY first (normal Ojamas before effect Ojamas)
            var normalGY = candidates.FirstOrDefault(c =>
                c.Location == CardLocation.Grave &&
                (c.Id == OjamaLockExecutor.CardId.OjamaGreen ||
                 c.Id == OjamaLockExecutor.CardId.OjamaYellow ||
                 c.Id == OjamaLockExecutor.CardId.OjamaBlack));
            if (normalGY != null) return normalGY;

            var anyGyOjama = candidates.FirstOrDefault(c => c.Location == CardLocation.Grave && IsOjamaCard(c.Id));
            if (anyGyOjama != null) return anyGyOjama;

            var handOjama = candidates.FirstOrDefault(c => c.Location == CardLocation.Hand && IsOjamaCard(c.Id));
            if (handOjama != null) return handOjama;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        private static bool IsOjamaCard(int id)
        {
            return id == OjamaLockExecutor.CardId.OjamaGreen ||
                   id == OjamaLockExecutor.CardId.OjamaYellow ||
                   id == OjamaLockExecutor.CardId.OjamaBlack ||
                   id == OjamaLockExecutor.CardId.OjamaBlue ||
                   id == OjamaLockExecutor.CardId.OjamaRed ||
                   id == OjamaLockExecutor.CardId.OjamaPink ||
                   id == OjamaLockExecutor.CardId.Ojamagic ||
                   id == OjamaLockExecutor.CardId.Ojamassimilation ||
                   id == OjamaLockExecutor.CardId.OjamaPajama ||
                   id == OjamaLockExecutor.CardId.OjamaCountry ||
                   id == OjamaLockExecutor.CardId.OjamaTrio;
        }

        public IList<ClientCard> PickOjamassimilationBanish(IList<ClientCard> candidates, int count)
        {
            if (candidates == null) return new List<ClientCard>();

            // Banish order: GY Normal Ojamas > GY Effect Ojamas > Field Normal Ojamas > Hand Normal Ojamas
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

            // Key Continuous Floodgates & Engine Pillars
            if (c.Id == 48680970) score += 120; // Eternal Soul
            if (c.Id == 82732705) score += 110; // Skill Drain
            if (c.Id == 66399653) score += 95;  // Union Hangar
            if (c.Id == 47222536) score += 80;  // Dark Magical Circle

            // Bosses & Towers
            if (c.Id == 41721210) score += 150; // Dark Magician the Dragon Knight
            if (c.Id == 50954680) score += 130; // Crystal Wing Synchro Dragon
            if (c.Id == 1561110) score += 140;  // ABC-Dragon Buster
            if (c.Id == 10443957) score += 135; // Cyber Dragon Infinity
            if (c.Id == 4280258) score += 130;  // Apollousa, Bow of the Goddess
            if (c.Id == 21887175) score += 130; // Mekk-Knight Crusadia Avramax

            // Mass Backrow Wipes
            if (c.Id == 18144506 || c.Id == 14532163 || c.Id == 15693423) score += 90;

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
