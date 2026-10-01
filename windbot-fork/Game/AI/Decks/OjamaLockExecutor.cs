// ============================================================================
// CARD AUDIT — OjamaLock (Championship Grade Ojama ABC & 5-Zone Lockdown)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|
// | ABC-Dragon Buster (1561110)        | Fusion L8    | No   | No    | Discard | Quick: Banish 1 card on field; Tag Out to 3   | Immediate disruption / Opponent key cards    |
// | Therion "King" Regulus (10604644)  | Monster L8   | Yes  | Yes   | Send    | Quick: Negate card or effect (Omni-Negate)    | Opponent activates card or effect            |
// | Ojama King (90140980)              | Fusion L5    | No   | No    | None    | Locks up to 3 of opponent's Monster Zones     | Fusion Summoned via Polymerization           |
// | Ojama Knight (40391316)            | Fusion L5    | No   | No    | None    | Locks up to 2 of opponent's Monster Zones     | Fusion Summoned via Poly / Instant Fusion    |
// | Ground Collapse (90502999)         | Spell Cont.  | No   | No    | None    | Locks 2 of opponent's Monster Zones           | Start of Main Phase when opp zones empty     |
// | Ojamassimilation (2390019)         | Spell Normal | Yes  | Yes   | Banish  | Reveal ABC, banish Ojamas, SS A, B, C from deck| Main Phase 1 starter                         |
// | Union Hangar (66399653)            | Spell Field  | Yes  | Yes   | None    | Search Union monster on act; equip from deck  | Start of Main Phase 1                        |
// | Ojama Pajama (75884822)            | Trap Cont.   | Yes  | Yes   | Discard | Search Ojama + discard; protect destruction   | Every turn during Main Phase                 |
// | Ojamagic (24643836)                | Spell Normal | No   | No    | None    | When sent to GY: Add Yellow, Green, Black (+3)| Discarded by Pajama / Pink / ABC Buster      |
// | Instant Fusion (1845204)           | Spell Normal | Yes  | Yes   | 1000 LP | SS Ojama Knight from Extra Deck (Lock 2 zones)| Main Phase 1 starter                         |
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
    [Deck("OjamaLock", "OjamaLock")]
    public class OjamaLockExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck - Normal Ojamas
            public const int OjamaGreen = 12482652;
            public const int OjamaYellow = 42941100;
            public const int OjamaBlack = 79335209;

            // Effect Ojamas
            public const int OjamaRed = 37132349;
            public const int OjamaBlue = 64627453;
            public const int OjamaPink = 42517468;

            // ABC Machine Pieces & Therion
            public const int AAssaultCore = 30012506;
            public const int BBusterDrake = 77411244;
            public const int CCrushWyvern = 3405259;
            public const int UnionDriver = 99249638;
            public const int TherionKingRegulus = 10604644;
            public const int KingOfTheSwamp = 79109599;

            // Handtraps & Board Breakers
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int HarpieFeatherDuster = 18144506;
            public const int Raigeki = 12580477;
            public const int DarkRulerNoMore = 54693926;

            // Spells
            public const int Ojamassimilation = 2390019;
            public const int Ojamagic = 24643836;
            public const int Polymerization = 24094653;
            public const int InstantFusion = 1845204;
            public const int GroundCollapse = 90502999;
            public const int OjamaCountry = 90011152;
            public const int PotOfAvarice = 67169062;
            public const int UnionHangar = 66399653;
            public const int Terraforming = 73628505;
            public const int TriWight = 96383838;
            public const int UnauthorizedReactivation = 12524259;

            // Traps
            public const int OjamaPajama = 75884822;
            public const int OjamaTrio = 29843091;

            // Extra Deck
            public const int ABCDragonBuster = 1561110;
            public const int OjamaKing = 90140980;
            public const int OjamaKnight = 40391316;
            public const int OjamaEmperor = 34031284;
            public const int RoninRaccoonSandayu = 39972129;
            public const int SkyCavalryCentaurea = 36776089;
            public const int AbyssDweller = 21044178;
            public const int Number41Bagooska = 90590303;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int KnightmarePhoenix = 2857636;
            public const int PlatinumGadget = 40216089;
            public const int OjamaToken = 29843092;
        }

        internal OjamaLockPlugin Plugin { get; private set; }
        private bool _ojamaPajamaSearchUsedThisTurn = false;
        private bool _ojamassimilationHandUsedThisTurn = false;
        private bool _ojamassimilationGraveUsedThisTurn = false;
        private bool _ojamaCountryDiscardUsedThisTurn = false;

        public OjamaLockExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new OjamaLockPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        private void RegisterHelperModules()
        {
            HeuristicGuard.RegisterAceCards(
                CardId.ABCDragonBuster,
                CardId.TherionKingRegulus,
                CardId.OjamaKing,
                CardId.OjamaKnight,
                CardId.PlatinumGadget,
                CardId.OjamaCountry,
                CardId.AbyssDweller,
                CardId.Number41Bagooska
            );
            BaitPlanner.RegisterComboStarters(
                CardId.KingOfTheSwamp,
                CardId.UnionHangar,
                CardId.Terraforming,
                CardId.OjamaPajama,
                CardId.Ojamassimilation,
                CardId.InstantFusion,
                CardId.Polymerization,
                CardId.DarkRulerNoMore,
                CardId.OjamaCountry,
                CardId.PotOfAvarice,
                CardId.OjamaRed
            );
            ChainAdvisor.RegisterHighValueTargets(
                CardId.ABCDragonBuster,
                CardId.TherionKingRegulus,
                CardId.OjamaKing,
                CardId.OjamaKnight,
                CardId.OjamaPajama,
                CardId.OjamaCountry,
                CardId.OjamaTrio,
                CardId.GroundCollapse
            );
        }

        public override bool OnSelectHand() => true; // Always choose to go first!

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _ojamaPajamaSearchUsedThisTurn = false;
            _ojamassimilationHandUsedThisTurn = false;
            _ojamassimilationGraveUsedThisTurn = false;
            _ojamaCountryDiscardUsedThisTurn = false;
            Plugin?.ResetTurnState();
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: HAND PROTECTION, INTERRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, () => DefaultInfiniteImpermanence());

            // Therion Regulus Quick Effect Omni-Negate
            AddExecutor(ExecutorType.Activate, CardId.TherionKingRegulus, TherionRegulusNegate);

            // ABC-Dragon Buster Quick Effect Banish
            AddExecutor(ExecutorType.Activate, CardId.ABCDragonBuster, ABCDragonBusterBanish);

            // ABC-Dragon Buster Quick Effect Tag Out (during opponent turn)
            AddExecutor(ExecutorType.Activate, CardId.ABCDragonBuster, ABCDragonBusterTagOut);

            // Abyss Dweller Quick Effect GY Lockdown
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);

            // Board Breakers & Opponent Disruption
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreActivate);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, () => Card.Location == CardLocation.Hand && Enemy.GetMonsterCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, () => Card.Location == CardLocation.Hand && Enemy.GetSpellCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: STARTERS, SEARCHERS & EQUIPPED SPECIAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // Ojama Country: Field Spell ATK/DEF swap (King 3000 ATK, Knight 2500 ATK) & discard revive!
            AddExecutor(ExecutorType.Activate, CardId.OjamaCountry, OjamaCountryActivate);

            // Ground Collapse: Locks 2 opponent monster zones!
            AddExecutor(ExecutorType.Activate, CardId.GroundCollapse, GroundCollapseActivate);

            // Ojama Pajama: Continuous Trap search & discard (Triggers Ojamagic +3 and sets up GY!)
            AddExecutor(ExecutorType.Activate, CardId.OjamaPajama, OjamaPajamaActivate);

            // Pot of Avarice: Shuffle 5 GY monsters & draw 2!
            AddExecutor(ExecutorType.Activate, CardId.PotOfAvarice, PotOfAvariceActivate);

            // King of the Swamp: Discard to search Polymerization on demand
            AddExecutor(ExecutorType.Activate, CardId.KingOfTheSwamp, KingOfTheSwampActivate);

            AddExecutor(ExecutorType.Activate, CardId.Terraforming, () => true);
            AddExecutor(ExecutorType.Activate, CardId.UnionHangar, UnionHangarActivate);
            AddExecutor(ExecutorType.Activate, CardId.UnauthorizedReactivation, UnauthorizedReactivationActivate);

            // Union Driver banish to equip missing piece from deck
            AddExecutor(ExecutorType.Activate, CardId.UnionDriver, UnionDriverActivate);

            // Equipped Union pieces Special Summon themselves to field (MUST BE BEFORE BOSS SUMMONS!)
            AddExecutor(ExecutorType.Activate, CardId.BBusterDrake, UnionPieceEquippedActivate);
            AddExecutor(ExecutorType.Activate, CardId.AAssaultCore, UnionPieceEquippedActivate);
            AddExecutor(ExecutorType.Activate, CardId.CCrushWyvern, UnionPieceEquippedActivate);

            // Polymerization: Fuse Ojama King (locks 3 zones) or Ojama Knight (locks 2 zones)
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationActivate);

            // Tri-Wight: Revive 3 Level 2 Normal Ojamas from GY to Field!
            AddExecutor(ExecutorType.Activate, CardId.TriWight, TriWightActivate);

            // Instant Fusion: Pay 1000 LP -> SS Ojama Knight -> Lock 2 zones (with Pajama destruction safeguard!)
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionActivate);

            // Ojamassimilation: Banish 3 Ojamas -> SS A, B, C from deck!
            AddExecutor(ExecutorType.Activate, CardId.Ojamassimilation, OjamassimilationActivate);

            // Set Traps early
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaPajama, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.OjamaTrio, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: BOSS MONSTER SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // ABC-Dragon Buster Contact Fusion from Field and/or GY
            AddExecutor(ExecutorType.SpSummon, CardId.ABCDragonBuster, ABCDragonBusterSpSummon);

            // Therion "King" Regulus Special Summon from hand by equipping Machine in GY
            AddExecutor(ExecutorType.Activate, CardId.TherionKingRegulus, TherionRegulusHandSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: UNION PIECES & OJAMA GY TRIGGERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.BBusterDrake, UnionPieceGyActivate);
            AddExecutor(ExecutorType.Activate, CardId.AAssaultCore, UnionPieceGyActivate);
            AddExecutor(ExecutorType.Activate, CardId.CCrushWyvern, UnionPieceGyActivate);
            AddExecutor(ExecutorType.Activate, CardId.Ojamagic);
            AddExecutor(ExecutorType.Activate, CardId.OjamaPink);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK XYZ & LINKS
            // ═══════════════════════════════════════════════════════════════
            // Platinum Gadget: 2 Machine monsters -> Extends Level 4 Machine from hand!
            AddExecutor(ExecutorType.SpSummon, CardId.PlatinumGadget, PlatinumGadgetSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.PlatinumGadget, PlatinumGadgetActivate);

            // Ojama Emperor Link-3
            AddExecutor(ExecutorType.SpSummon, CardId.OjamaEmperor, OjamaEmperorSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.OjamaEmperor, OjamaEmperorActivate);

            // Rank 4 Xyz (using Level 4 Union pieces)
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, BagooskaSpSummon);

            // Rank 2 Xyz (using Level 2 normal Ojamas)
            AddExecutor(ExecutorType.SpSummon, CardId.RoninRaccoonSandayu, () => Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 2 && Plugin.MaterialImpl.GetMaterialCost(m) < 100) >= 2);
            AddExecutor(ExecutorType.Activate, CardId.RoninRaccoonSandayu);

            AddExecutor(ExecutorType.SpSummon, CardId.SkyCavalryCentaurea, SkyCavalryCentaureaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SkyCavalryCentaurea, () => true);

            // I:P Masquerena Link-2
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaActivate);

            // Utility Links
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, () => Bot.GetMonsters().Count(m => m.IsFaceup() && Plugin.MaterialImpl.GetMaterialCost(m) < 100) >= 2 && Enemy.GetSpellCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix);

            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: NORMAL SUMMONS & SWARMING
            // ═══════════════════════════════════════════════════════════════
            // Normal summon Machine pieces FIRST (triggers Union Hangar equip from deck!)
            AddExecutor(ExecutorType.Summon, CardId.BBusterDrake, MachineNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.AAssaultCore, MachineNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CCrushWyvern, MachineNormalSummon);

            // Ojama Red: Normal summon to swarm other Ojamas from hand!
            AddExecutor(ExecutorType.Summon, CardId.OjamaRed, OjamaRedSummon);
            AddExecutor(ExecutorType.Activate, CardId.OjamaRed, () => true);

            // Ojama Blue (battle floater)
            AddExecutor(ExecutorType.Summon, CardId.OjamaBlue, () => Bot.GetMonsterCount() == 0);
            AddExecutor(ExecutorType.MonsterSet, CardId.OjamaBlue, () => Bot.GetMonsterCount() == 0);
            AddExecutor(ExecutorType.Activate, CardId.OjamaBlue);

            // Normal Ojamas fallback
            AddExecutor(ExecutorType.Summon, CardId.OjamaGreen, () => Bot.GetMonsterCount() == 0);
            AddExecutor(ExecutorType.Summon, CardId.OjamaYellow, () => Bot.GetMonsterCount() == 0);
            AddExecutor(ExecutorType.Summon, CardId.OjamaBlack, () => Bot.GetMonsterCount() == 0);
        }

        #region Card Logic

        private bool IsCurrentCardNegated() => Card != null && DefaultCheckWhetherCardIsNegated(Card);

        private bool GroundCollapseActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            return oppFreeZones >= 2 && Bot.GetSpellCount() < 5;
        }

        private bool UnionPieceEquippedActivate()
        {
            // Only activate when equipped in SpellZone to Special Summon itself to the field!
            if (Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool UnionPieceGyActivate()
        {
            // Triggers in GY to add/recycle/special summon
            return Card.Location == CardLocation.Grave;
        }

        private bool UnionHangarActivate()
        {
            if (Card.Location == CardLocation.Hand)
                return !Bot.HasInSpellZone(CardId.UnionHangar);

            return true;
        }

        private bool UnauthorizedReactivationActivate()
        {
            if (IsCurrentCardNegated()) return false;
            var target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasRace(CardRace.Machine));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool UnionDriverActivate()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            int targetId = CardId.BBusterDrake;
            if (Bot.HasInMonstersZone(CardId.BBusterDrake) || Bot.HasInGraveyard(CardId.BBusterDrake))
            {
                targetId = (Bot.HasInMonstersZone(CardId.AAssaultCore) || Bot.HasInGraveyard(CardId.AAssaultCore))
                    ? CardId.CCrushWyvern
                    : CardId.AAssaultCore;
            }
            AI.SelectCard(targetId);
            return true;
        }

        private bool TherionRegulusHandSpSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            var gyMachine = Bot.Graveyard.FirstOrDefault(c => c.HasRace(CardRace.Machine) && c.IsMonster());
            if (gyMachine != null)
            {
                AI.SelectCard(gyMachine);
                return true;
            }
            return false;
        }

        private bool TherionRegulusNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (IsCurrentCardNegated()) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool OjamassimilationActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_ojamassimilationGraveUsedThisTurn) return false;
                if (Bot.Banished.Count(c => IsOjamaCard(c.Id)) >= 3)
                {
                    _ojamassimilationGraveUsedThisTurn = true;
                    return true;
                }
                return false;
            }

            if (_ojamassimilationHandUsedThisTurn) return false;

            bool hasABCInExtra = Bot.ExtraDeck.Any(c => c.Id == CardId.ABCDragonBuster);
            if (!hasABCInExtra) return false;

            int ojamaCount = Bot.Hand.Count(c => IsOjamaMonster(c.Id)) +
                             Bot.GetMonsters().Count(c => IsOjamaMonster(c.Id)) +
                             Bot.Graveyard.Count(c => IsOjamaMonster(c.Id));

            if (ojamaCount >= 1 && Bot.GetMonsterCount() < 5)
            {
                _ojamassimilationHandUsedThisTurn = true;
                AI.SelectCard(CardId.ABCDragonBuster);
                return true;
            }
            return false;
        }

        private bool KingOfTheSwampActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInHand(CardId.Polymerization) && Bot.Deck.Any(c => c.Id == CardId.Polymerization);
        }

        private bool InstantFusionActivate()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (Bot.LifePoints <= 2000) return false; // Rule 16: LP Safety Threshold

            bool hasPajamaProtection = Bot.SpellZone.Any(c => c != null && c.Id == CardId.OjamaPajama && c.IsFaceup());
            bool canLinkBeasts = Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Beast)) >= 2 &&
                                 Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaEmperor);

            if (Duel.Turn == 1 && !hasPajamaProtection && !canLinkBeasts)
            {
                // Without face-up Pajama protection on Turn 1, Ojama Knight will self-destruct at End Phase.
                return false;
            }

            int oppFreeZones = 5 - Enemy.GetMonsterCount();
            return oppFreeZones >= 1 && Bot.GetMonsterCount() < 5 && Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaKnight);
        }

        private bool CanFusionSummonOjamaKing()
        {
            var available = Bot.Hand.Concat(Bot.GetMonsters()).ToList();
            bool hasKotS = available.Any(c => c.Id == CardId.KingOfTheSwamp);
            bool hasGreen = available.Any(c => c.Id == CardId.OjamaGreen);
            bool hasYellow = available.Any(c => c.Id == CardId.OjamaYellow);
            bool hasBlack = available.Any(c => c.Id == CardId.OjamaBlack);

            int namedCount = (hasGreen ? 1 : 0) + (hasYellow ? 1 : 0) + (hasBlack ? 1 : 0);
            bool canFuse = (namedCount == 3) || (namedCount >= 2 && hasKotS);

            return canFuse && Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaKing);
        }

        private bool CanFusionSummonOjamaKnight()
        {
            var available = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => IsOjamaMonster(c.Id)).ToList();
            return available.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.OjamaKnight);
        }

        private bool PolymerizationActivate()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            return CanFusionSummonOjamaKing() || CanFusionSummonOjamaKnight();
        }

        private bool TriWightActivate()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            int normalCount = Bot.Graveyard.Count(c => c.IsMonster() && c.HasType(CardType.Normal) && c.Level <= 2);
            return normalCount >= 3 && Bot.GetMonsterCount() <= 2;
        }

        private bool OjamaPajamaActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (Card.IsFaceup())
                {
                    if (_ojamaPajamaSearchUsedThisTurn) return false;
                    _ojamaPajamaSearchUsedThisTurn = true;
                    return true;
                }
                bool alreadyHasFaceup = Bot.SpellZone.Any(c => c != null && c != Card && c.Id == CardId.OjamaPajama && c.IsFaceup());
                if (alreadyHasFaceup) return false;
                return true;
            }
            return false;
        }

        private bool ABCDragonBusterSpSummon()
        {
            bool hasA = Bot.GetMonsters().Any(c => c.Id == CardId.AAssaultCore) || Bot.Graveyard.Any(c => c.Id == CardId.AAssaultCore);
            bool hasB = Bot.GetMonsters().Any(c => c.Id == CardId.BBusterDrake) || Bot.Graveyard.Any(c => c.Id == CardId.BBusterDrake);
            bool hasC = Bot.GetMonsters().Any(c => c.Id == CardId.CCrushWyvern) || Bot.Graveyard.Any(c => c.Id == CardId.CCrushWyvern);

            if (hasA && hasB && hasC && Bot.ExtraDeck.Any(c => c.Id == CardId.ABCDragonBuster))
            {
                int inGrave = Bot.Graveyard.Count(c => c.Id == CardId.AAssaultCore || c.Id == CardId.BBusterDrake || c.Id == CardId.CCrushWyvern);
                int inField = Bot.GetMonsters().Count(c => c.Id == CardId.AAssaultCore || c.Id == CardId.BBusterDrake || c.Id == CardId.CCrushWyvern);

                if (inGrave >= 3)
                    AI.SelectMaterials(CardLocation.Grave);
                else if (inField >= 3)
                    AI.SelectMaterials(CardLocation.MonsterZone);
                else
                    AI.SelectMaterials(new[] { CardId.AAssaultCore, CardId.BBusterDrake, CardId.CCrushWyvern });

                return true;
            }
            return false;
        }

        private bool ABCDragonBusterBanish()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (IsCurrentCardNegated()) return false;

            var enemyCards = Enemy.GetMonsters().Where(m => !IsTargetImmune(m))
                                  .Concat(Enemy.GetSpells().Where(s => !IsTargetImmune(s)))
                                  .ToList();
            if (enemyCards.Count > 0 && Bot.Hand.Count > 0)
            {
                var target = enemyCards.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                       .ThenByDescending(c => c.Attack)
                                       .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool ABCDragonBusterTagOut()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.End || DefaultCheckWhetherCardIsNegated(Card) || Duel.LastChainPlayer == 1)
                {
                    int banishedPieces = Bot.Banished.Count(c =>
                        c.Id == CardId.AAssaultCore || c.Id == CardId.BBusterDrake || c.Id == CardId.CCrushWyvern);
                    return banishedPieces >= 3 && Bot.GetMonsterCount() <= 2;
                }
            }

            return false;
        }

        private bool MachineNormalSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool OjamaRedSummon()
        {
            // If Union Hangar is active and we have a Machine piece, let the Machine summon first to trigger Hangar!
            if (Bot.HasInSpellZone(CardId.UnionHangar) &&
                Bot.Hand.Any(c => c.Id == CardId.BBusterDrake || c.Id == CardId.AAssaultCore || c.Id == CardId.CCrushWyvern))
            {
                return false;
            }
            return Bot.Hand.Any(c => c != Card && IsOjamaMonster(c.Id));
        }

        private bool OjamaEmperorSpSummon()
        {
            int beasts = Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Beast) &&
                                                      m.Id != CardId.OjamaKing && m.Id != CardId.OjamaKnight);
            return beasts >= 3 && Bot.Graveyard.Any(c => c.Id == CardId.OjamaKing || c.Id == CardId.OjamaKnight);
        }

        private bool OjamaEmperorActivate()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.OjamaKing || c.Id == CardId.OjamaKnight);
            if (target != null && Bot.GetMonsterCount() < 5)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PlatinumGadgetSpSummon()
        {
            var machines = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasRace(CardRace.Machine) &&
                                                        m.Id != CardId.ABCDragonBuster &&
                                                        m.Id != CardId.TherionKingRegulus).ToList();
            return machines.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.PlatinumGadget);
        }

        private bool PlatinumGadgetActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var machineInHand = Bot.Hand.FirstOrDefault(c => c.HasRace(CardRace.Machine) && c.Level <= 4);
                if (machineInHand != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(machineInHand);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool IPMasquerenaSpSummon()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            var nonLinkNonBoss = Bot.GetMonsters().Where(m => m.IsFaceup() && !m.HasType(CardType.Link) &&
                                                              Plugin.MaterialImpl.GetMaterialCost(m) < 100).ToList();
            return nonLinkNonBoss.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.IPMasquerena);
        }

        private bool IPMasquerenaActivate()
        {
            return Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2;
        }

        private bool SPLittleKnightSpSummon()
        {
            var nonBoss = Bot.GetMonsters().Where(m => m.IsFaceup() &&
                                                       Plugin.MaterialImpl.GetMaterialCost(m) < 100).ToList();
            return nonBoss.Count >= 2 && (Duel.Phase == DuelPhase.Main2 || Enemy.GetMonsterCount() > 0 || Duel.Player == 1);
        }

        private bool SPLittleKnightActivate()
        {
            var oppTarget = Enemy.GetMonsters().Where(m => !IsTargetImmune(m)).OrderByDescending(m => m.Attack).FirstOrDefault()
                         ?? Enemy.GetSpells().FirstOrDefault(s => !IsTargetImmune(s));
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool DarkRulerNoMoreActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect));
        }

        private bool OjamaTrioActivate()
        {
            if (Card.Location != CardLocation.SpellZone || Card.IsFaceup()) return false;
            int freeZones = 5 - Enemy.GetMonsterCount();
            return freeZones >= 3 && (Duel.Player == 1 || Duel.Phase == DuelPhase.End);
        }

        private bool PotOfAvariceActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.Graveyard.Count(c => c.IsMonster()) >= 5;
        }

        private bool AbyssDwellerSpSummon()
        {
            var l4 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && Plugin.MaterialImpl.GetMaterialCost(m) < 100).ToList();
            return l4.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.AbyssDweller) &&
                   (Enemy.Graveyard.Count >= 2 || Enemy.GetMonsterCount() > 0 || Duel.Player == 1);
        }

        private bool AbyssDwellerActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;
            return Duel.Player == 1 || Duel.Phase == DuelPhase.Main1;
        }

        private bool BagooskaSpSummon()
        {
            if (!Util.IsTurn1OrMain2()) return false;
            var l4 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && Plugin.MaterialImpl.GetMaterialCost(m) < 100).ToList();
            return l4.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.Number41Bagooska);
        }

        private bool SkyCavalryCentaureaSpSummon()
        {
            var l2 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 2 && Plugin.MaterialImpl.GetMaterialCost(m) < 100).ToList();
            bool enemyHasBoss = Enemy.GetMonsters().Any(m => m.IsFaceup() && (m.Attack >= 2500 || IsTargetImmune(m)));
            return l2.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.SkyCavalryCentaurea) && (enemyHasBoss || Util.IsTurn1OrMain2());
        }

        private bool OjamaCountryActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.OjamaCountry);
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_ojamaCountryDiscardUsedThisTurn) return false;
                bool hasOjamaInHand = Bot.Hand.Any(c => IsOjamaCard(c.Id));
                bool hasOjamaInGY = Bot.Graveyard.Any(c => IsOjamaMonster(c.Id) || c.Id == CardId.OjamaKing || c.Id == CardId.OjamaKnight);
                if (hasOjamaInHand && hasOjamaInGY && Bot.GetMonsterCount() < 5)
                {
                    _ojamaCountryDiscardUsedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool SpellSetStrategy()
        {
            if (Card == null) return false;
            if (Bot.SpellZone.Any(c => c != null && c.Id == Card.Id)) return false;
            if (Card.IsTrap()) return true;
            return false;
        }

        private static bool IsOjamaMonster(int id)
        {
            return id == CardId.OjamaGreen || id == CardId.OjamaYellow || id == CardId.OjamaBlack ||
                   id == CardId.OjamaBlue || id == CardId.OjamaRed || id == CardId.OjamaPink;
        }

        private static bool IsOjamaCard(int id)
        {
            return IsOjamaMonster(id) || id == CardId.Ojamagic || id == CardId.Ojamassimilation ||
                   id == CardId.OjamaPajama || id == CardId.OjamaCountry || id == CardId.OjamaTrio ||
                   id == CardId.OjamaKing || id == CardId.OjamaKnight || id == CardId.OjamaEmperor;
        }

        #endregion

        #region Card Selection & Trigger Handlers

        public override uint OnSelectDisfield(long hint, int count, uint available)
        {
            uint selected = Plugin.ZoneLockManager.SelectDisfieldZones(count, available);
            Logger.WriteLine($"[OJAMA-DISFIELD] count={count} avail=0x{available:X8} selected=0x{selected:X8}");
            return selected;
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            int selected = Plugin.ZoneLockManager.SelectLockPlace(available, location, player);
            Logger.WriteLine($"[OJAMA-PLACE] cardId={cardId} player={player} loc={location} avail=0x{available:X} selected=0x{selected:X}");
            return selected;
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Ojama King (90140980) and Ojama Knight (40391316) zone lock prompt: ALWAYS AGREE to lock max zones!
            if (desc == ((long)CardId.OjamaKing << 4) || desc == ((long)CardId.OjamaKnight << 4))
            {
                Logger.WriteLine($"[OJAMA-YESNO] Agreeing to lock additional zone for Ojama King/Knight! desc={desc}");
                return true;
            }

            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 0a. Ojamassimilation Extra Deck Reveal: ABC-Dragon Buster
            if (Card?.Id == CardId.Ojamassimilation && cards.Any(c => c.Id == CardId.ABCDragonBuster))
            {
                var buster = cards.FirstOrDefault(c => c.Id == CardId.ABCDragonBuster);
                if (buster != null) return new List<ClientCard> { buster };
            }

            // 0c. Polymerization Extra Deck Fusion Monster Selection
            if (Card?.Id == CardId.Polymerization && cards.Any(c => c.Location == CardLocation.Extra))
            {
                if (CanFusionSummonOjamaKing() && cards.Any(c => c.Id == CardId.OjamaKing))
                {
                    Logger.WriteLine("[OJAMA-FUSION] Selecting Ojama King from Extra Deck!");
                    return new List<ClientCard> { cards.First(c => c.Id == CardId.OjamaKing) };
                }
                if (cards.Any(c => c.Id == CardId.OjamaKnight))
                {
                    Logger.WriteLine("[OJAMA-FUSION] Selecting Ojama Knight from Extra Deck!");
                    return new List<ClientCard> { cards.First(c => c.Id == CardId.OjamaKnight) };
                }
            }

            // 0d. Tri-Wight Targets: 3 Normal Level 2 Ojamas in GY
            if (Card?.Id == CardId.TriWight && cards.All(c => c.Location == CardLocation.Grave))
            {
                var normalOjamas = cards.Where(c => c.IsMonster() && c.HasType(CardType.Normal) && c.Level <= 2).ToList();
                if (normalOjamas.Count >= min)
                {
                    Logger.WriteLine($"[OJAMA-TRIWIGHT] Reviving {min} normal Ojamas from GY!");
                    return normalOjamas.Take(Math.Min(max, normalOjamas.Count)).ToList();
                }
            }

            // 0e. Abyss Dweller Detach Material
            if (Card?.Id == CardId.AbyssDweller && cards.All(c => c.Location == CardLocation.Overlay))
            {
                return new List<ClientCard> { cards.First() };
            }

            // 0b. Equip Target (Hint 518 = HINTMSG_EQUIP) -> Prioritize Union Driver from Deck!
            if (hint == 518)
            {
                var driver = cards.FirstOrDefault(c => c.Id == CardId.UnionDriver);
                if (driver != null) return new List<ClientCard> { driver };

                var buster = cards.FirstOrDefault(c => c.Id == CardId.BBusterDrake);
                if (buster != null) return new List<ClientCard> { buster };

                var assault = cards.FirstOrDefault(c => c.Id == CardId.AAssaultCore);
                if (assault != null) return new List<ClientCard> { assault };

                var crush = cards.FirstOrDefault(c => c.Id == CardId.CCrushWyvern);
                if (crush != null) return new List<ClientCard> { crush };

                return new List<ClientCard> { cards.First() };
            }

            // 1a. Ojamassimilation GY Effect: Target 3 banished Ojamas to recycle to deck & draw 1!
            if (Card?.Id == CardId.Ojamassimilation && Card.Location == CardLocation.Grave)
            {
                var banishedOjamas = cards.Where(c => IsOjamaMonster(c.Id)).ToList();
                if (banishedOjamas.Count >= min)
                    return banishedOjamas.Take(Math.Min(max, banishedOjamas.Count)).ToList();
            }

            // 1b. Ojamassimilation Cost: Banish as many Ojamas as possible (up to 3) to summon full A, B, C!
            if (Card?.Id == CardId.Ojamassimilation && (hint == 503 || cards.All(c => c.Controller == 0)))
            {
                var ojamas = cards.Where(c => IsOjamaMonster(c.Id)).ToList();
                if (ojamas.Count > 0)
                {
                    int take = Math.Min(max, Math.Min(3, ojamas.Count));
                    var picked = Plugin.MaterialImpl.PickOjamassimilationBanish(ojamas, take);
                    if (picked.Count >= min) return picked;
                }
            }

            // 2. Ojamassimilation Summon: Distinct A, B, C from Deck
            if (Card?.Id == CardId.Ojamassimilation && (hint == 509 || cards.All(c => c.Location == CardLocation.Deck)))
            {
                var distinctPieces = new List<ClientCard>();
                int[] preferredIds = { CardId.BBusterDrake, CardId.AAssaultCore, CardId.CCrushWyvern };
                foreach (int id in preferredIds)
                {
                    var match = cards.FirstOrDefault(c => c.Id == id && !distinctPieces.Any(p => p.Id == id));
                    if (match != null) distinctPieces.Add(match);
                    if (distinctPieces.Count == max) break;
                }
                if (distinctPieces.Count >= min) return distinctPieces;
            }

            // 3. ABC-Dragon Buster Contact Fusion Banish from Field / GY
            if (cards.All(c => c.Controller == 0) &&
                cards.Any(c => c.Id == CardId.AAssaultCore || c.Id == CardId.BBusterDrake || c.Id == CardId.CCrushWyvern))
            {
                var abcPieces = new List<ClientCard>();
                var a = cards.FirstOrDefault(c => c.Id == CardId.AAssaultCore);
                var b = cards.FirstOrDefault(c => c.Id == CardId.BBusterDrake);
                var c = cards.FirstOrDefault(c => c.Id == CardId.CCrushWyvern);
                if (a != null) abcPieces.Add(a);
                if (b != null) abcPieces.Add(b);
                if (c != null) abcPieces.Add(c);
                if (abcPieces.Count >= min) return abcPieces.Take(max).ToList();
            }

            // 4. Removal / Banish / Destroy (Hints: 502, 503, 505)
            // If ALL cards are our cards, this is a cost or protection (e.g. Ojama Pajama destruction substitute)!
            if (hint == 503 && cards.All(c => c.Controller == 0))
            {
                var sub = Plugin.MaterialImpl.PickDestructionSubstitute(cards, min);
                if (sub != null) return new List<ClientCard> { sub };
            }

            if (hint == 502 || hint == 503 || hint == 505)
            {
                var oppCards = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (oppCards.Count >= min)
                {
                    return oppCards.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                   .ThenByDescending(c => c.Attack)
                                   .Take(Math.Min(max, oppCards.Count))
                                   .ToList();
                }
            }

            // 5. Discard Target (Hint 501) -> Prioritize Ojamagic / Pink / ABC pieces
            if (hint == 501)
            {
                var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null && cards.Contains(discardTarget))
                {
                    var result = new List<ClientCard> { discardTarget };
                    result.AddRange(cards.Where(c => c != discardTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            // 6. Special Summon Target (Hint 509)
            if (hint == 509)
            {
                var ssTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (ssTarget != null && cards.Contains(ssTarget))
                {
                    var result = new List<ClientCard> { ssTarget };
                    result.AddRange(cards.Where(c => c != ssTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            // 7. Search Target (Hint 506) -> ATOHAND only! (Rule 1)
            if (hint == 506)
            {
                // Terraforming: search Union Hangar if not available, otherwise Ojama Country
                if (Card?.Id == CardId.Terraforming)
                {
                    if (!Bot.HasInHand(CardId.UnionHangar) && !Bot.HasInSpellZone(CardId.UnionHangar))
                    {
                        var hangar = cards.FirstOrDefault(x => x.Id == CardId.UnionHangar);
                        if (hangar != null) return new List<ClientCard> { hangar };
                    }
                    var country = cards.FirstOrDefault(x => x.Id == CardId.OjamaCountry);
                    if (country != null) return new List<ClientCard> { country };
                }

                // King of the Swamp: searches Polymerization
                if (Card?.Id == CardId.KingOfTheSwamp)
                {
                    var poly = cards.FirstOrDefault(x => x.Id == CardId.Polymerization);
                    if (poly != null) return new List<ClientCard> { poly };
                }

                // B-Buster Drake deck search: prioritize missing ABC piece
                if (Card?.Id == CardId.BBusterDrake)
                {
                    bool hasC = Bot.HasInHand(CardId.CCrushWyvern) || Bot.HasInMonstersZone(CardId.CCrushWyvern) || Bot.HasInGraveyard(CardId.CCrushWyvern);
                    bool hasA = Bot.HasInHand(CardId.AAssaultCore) || Bot.HasInMonstersZone(CardId.AAssaultCore) || Bot.HasInGraveyard(CardId.AAssaultCore);
                    bool hasB = Bot.HasInHand(CardId.BBusterDrake) || Bot.HasInMonstersZone(CardId.BBusterDrake) || Bot.HasInGraveyard(CardId.BBusterDrake);

                    if (!hasC)
                    {
                        var c = cards.FirstOrDefault(x => x.Id == CardId.CCrushWyvern);
                        if (c != null) return new List<ClientCard> { c };
                    }
                    if (!hasA)
                    {
                        var a = cards.FirstOrDefault(x => x.Id == CardId.AAssaultCore);
                        if (a != null) return new List<ClientCard> { a };
                    }
                    if (!hasB)
                    {
                        var b = cards.FirstOrDefault(x => x.Id == CardId.BBusterDrake);
                        if (b != null) return new List<ClientCard> { b };
                    }

                    // If all pieces exist, search Union Driver or duplicate B / A / C
                    var driver = cards.FirstOrDefault(x => x.Id == CardId.UnionDriver);
                    if (driver != null) return new List<ClientCard> { driver };
                    var anyB = cards.FirstOrDefault(x => x.Id == CardId.BBusterDrake);
                    if (anyB != null) return new List<ClientCard> { anyB };
                    var anyA = cards.FirstOrDefault(x => x.Id == CardId.AAssaultCore);
                    if (anyA != null) return new List<ClientCard> { anyA };
                    var anyC = cards.FirstOrDefault(x => x.Id == CardId.CCrushWyvern);
                    if (anyC != null) return new List<ClientCard> { anyC };
                }

                // A-Assault Core GY retrieve: add B-Buster Drake or C-Crush Wyvern
                if (Card?.Id == CardId.AAssaultCore)
                {
                    var b = cards.FirstOrDefault(x => x.Id == CardId.BBusterDrake);
                    if (b != null) return new List<ClientCard> { b };
                    var c = cards.FirstOrDefault(x => x.Id == CardId.CCrushWyvern);
                    if (c != null) return new List<ClientCard> { c };
                }

                var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (searchTarget != null && cards.Contains(searchTarget))
                {
                    var result = new List<ClientCard> { searchTarget };
                    result.AddRange(cards.Where(c => c != searchTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            // 8. Fusion Material Selection (Hint 500 = HINTMSG_FMATERIAL)
            if (hint == 500)
            {
                var chosen = new List<ClientCard>();
                var green = cards.FirstOrDefault(c => c.Id == CardId.OjamaGreen);
                var yellow = cards.FirstOrDefault(c => c.Id == CardId.OjamaYellow);
                var black = cards.FirstOrDefault(c => c.Id == CardId.OjamaBlack);
                var kots = cards.FirstOrDefault(c => c.Id == CardId.KingOfTheSwamp);

                if (green != null) chosen.Add(green);
                if (yellow != null) chosen.Add(yellow);
                if (black != null) chosen.Add(black);
                if (kots != null && chosen.Count >= 2 && chosen.Count < max) chosen.Add(kots);

                foreach (var c in cards)
                {
                    if (!chosen.Contains(c) && IsOjamaMonster(c.Id))
                        chosen.Add(c);
                    if (chosen.Count == max) break;
                }

                if (chosen.Count >= min)
                    return chosen.Take(max).ToList();

                var sorted = Plugin.MaterialImpl.SortMaterials(cards, min);
                var safe = sorted.Where(c => Plugin.MaterialImpl.GetMaterialCost(c) < 100).ToList();
                if (safe.Count >= min)
                    return safe.Take(Math.Min(max, safe.Count)).ToList();
            }

            // 9. Link / Xyz Material Selection (Hint 533 = HINTMSG_LMATERIAL, Hint 513 = HINTMSG_XMATERIAL)
            if (hint == 533 || hint == 513)
            {
                var sorted = Plugin.MaterialImpl.SortMaterials(cards, min);
                // STRICT RULE: Never sacrifice Ace Lockers or Bosses (GetMaterialCost >= 100)
                var safeMaterials = sorted.Where(c => Plugin.MaterialImpl.GetMaterialCost(c) < 100).ToList();
                if (safeMaterials.Count >= min)
                    return safeMaterials.Take(Math.Min(max, safeMaterials.Count)).ToList();
            }

            // 10. Therion Regulus GY Target (Machine monster to equip)
            if (Card?.Id == CardId.TherionKingRegulus && cards.All(c => c.Location == CardLocation.Grave))
            {
                var machine = cards.FirstOrDefault(c => c.HasRace(CardRace.Machine) && c.IsMonster());
                if (machine != null) return new List<ClientCard> { machine };
            }

            // 11. Therion Regulus Cost: Send Therion card from hand or face-up field
            if (Card?.Id == CardId.TherionKingRegulus && cards.Any(c => c.Id == CardId.TherionKingRegulus))
            {
                var regulusCard = cards.FirstOrDefault(c => c.Id == CardId.TherionKingRegulus);
                if (regulusCard != null) return new List<ClientCard> { regulusCard };
            }

            // 12. Pot of Avarice Target: 5 monsters in GY
            if (Card?.Id == CardId.PotOfAvarice && cards.All(c => c.Location == CardLocation.Grave))
            {
                var targets = new List<ClientCard>();
                foreach (var c in cards.Where(c => c.HasType(CardType.Fusion) || c.HasType(CardType.Link) || c.HasType(CardType.Xyz)))
                {
                    targets.Add(c);
                    if (targets.Count == 5) break;
                }
                foreach (var c in cards.Where(c => c.Id == CardId.OjamaGreen || c.Id == CardId.OjamaYellow || c.Id == CardId.OjamaBlack))
                {
                    if (!targets.Contains(c)) targets.Add(c);
                    if (targets.Count == 5) break;
                }
                foreach (var c in cards.Where(c => c.IsMonster()))
                {
                    if (!targets.Contains(c)) targets.Add(c);
                    if (targets.Count == 5) break;
                }
                if (targets.Count >= min) return targets.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectFusionMaterial(cards, min, max);

            // 1. Ojama King Fusion Materials: 1 Green + 1 Yellow + 1 Black (or King of the Swamp substitute)
            var green = cards.FirstOrDefault(c => c.Id == CardId.OjamaGreen);
            var yellow = cards.FirstOrDefault(c => c.Id == CardId.OjamaYellow);
            var black = cards.FirstOrDefault(c => c.Id == CardId.OjamaBlack);
            var kots = cards.FirstOrDefault(c => c.Id == CardId.KingOfTheSwamp);

            var kingMats = new List<ClientCard>();
            if (green != null) kingMats.Add(green);
            if (yellow != null) kingMats.Add(yellow);
            if (black != null) kingMats.Add(black);

            if (kingMats.Count < 3 && kots != null && !kingMats.Contains(kots))
            {
                kingMats.Add(kots);
            }

            if (min >= 3 && kingMats.Count >= min)
            {
                Logger.WriteLine($"[OJAMA-FUSION-MATS] Selecting materials for Ojama King: {string.Join(", ", kingMats.Select(m => m.Name ?? m.Id.ToString()))}");
                return kingMats.Take(max).ToList();
            }

            // 2. Ojama Knight Fusion Materials: 2 Ojama monsters (prioritize duplicate normal Ojamas or effect Ojamas first)
            var ojamaMats = cards.Where(c => IsOjamaMonster(c.Id)).ToList();
            if (ojamaMats.Count >= min)
            {
                var sorted = ojamaMats.OrderByDescending(c => cards.Count(x => x.Id == c.Id) > 1)
                                      .ThenByDescending(c => c.Id == CardId.OjamaPink || c.Id == CardId.OjamaRed)
                                      .Take(max)
                                      .ToList();
                Logger.WriteLine($"[OJAMA-FUSION-MATS] Selecting materials for Ojama Knight: {string.Join(", ", sorted.Select(m => m.Name ?? m.Id.ToString()))}");
                return sorted;
            }

            return base.OnSelectFusionMaterial(cards, min, max);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null || defender == null) return false;

            // 1. Apprentice Illusion Magician combat trick (+2000 ATK to Dark Spellcaster during damage calculation)
            bool apprenticeThreat = Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Id == 30603688) ||
                                    Enemy.Hand.Any(c => c.Id == 30603688);
            if (apprenticeThreat && defender.HasAttribute(CardAttribute.Dark) && defender.HasRace(CardRace.SpellCaster) && defender.Id != 30603688)
            {
                if (attacker.Attack <= defender.Attack + 2000)
                {
                    Logger.WriteLine($"[BATTLE-GUARD] Preventing suicidal attack: {attacker.Name} vs {defender.Name} (+2000 Apprentice buff)!");
                    return false;
                }
            }

            // 2. Crystal Wing Synchro Dragon battle protection
            if (defender.Id == 50954680 && (attacker.Level >= 5 || attacker.Rank >= 5))
            {
                Logger.WriteLine($"[BATTLE-GUARD] Preventing attack into Crystal Wing Synchro Dragon with Level/Rank 5+ monster!");
                return false;
            }

            return base.OnPreBattleBetween(attacker, defender);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // Attack Bosses: ABC-Dragon Buster, Therion Regulus, Platinum Gadget
            if (cardId == CardId.ABCDragonBuster || cardId == CardId.TherionKingRegulus || cardId == CardId.PlatinumGadget)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            // Rule 12: Bagooska MUST always be summoned in FaceUpDefence
            if (cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Defense Walls & Zone Lockers: Ojama King, Ojama Knight
            if (cardId == CardId.OjamaKing || cardId == CardId.OjamaKnight)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;
            if (card.Controller == 1) return false; // Rule 14: Reject opponent optional prompts

            // Always accept Ojama Pajama destruction protection or search trigger
            if (card.Id == CardId.OjamaPajama) return true;

            // Always accept Union piece triggers (A, B, C)
            if (card.Id == CardId.AAssaultCore || card.Id == CardId.BBusterDrake || card.Id == CardId.CCrushWyvern)
                return true;

            // Always accept Ojamagic +3 search trigger
            if (card.Id == CardId.Ojamagic) return true;

            // Always accept Ojama Pink draw/discard/lock trigger
            if (card.Id == CardId.OjamaPink) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        #endregion
    }
}
