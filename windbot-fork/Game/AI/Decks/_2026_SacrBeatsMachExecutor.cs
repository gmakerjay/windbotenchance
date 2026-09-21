using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — SacrBeatsMach (Sacred Beasts + Machina + Crystron Trains FTK/OTK)
    // 100% verified against cards.cdb and SacrBeatsMach.ydk
    // ====================================================================================================
    // | Card Name                           | ID       | Lv | ATK  | DEF  | Type   | Key Role                             |
    // |-------------------------------------|----------|----|------|------|--------|--------------------------------------|
    // | Super B.E.S. Metal Slave            | 41516133 | 11 | 3100 | 3100 | Mach   | Dump BES -> SS + pop 2 cards         |
    // | Machina Ruinforce                   | 46033517 | 10 | 4600 | 4100 | Mach   | GY SS banish 12+ / Re-summon 3 ban   |
    // | Calamity of the Sacred Beasts-Hamon | 50251045 | 10 | 4000 | 4000 | Thund  | Reveal: search SB Spell / 1k GY burn |
    // | Infinity of the Sacred Beasts-Raviel| 96345184 | 10 | 4000 | 4000 | Fiend  | Reveal: search SB monster / boardwipe|
    // | Yomagna the Fire Phantom            | 17350692 | 10 | 3300 | 0    | Pyro   | Shuffle 3 GY -> SS + Super Poly eff  |
    // | Heavy Freight Train Derricrane      | 13647631 | 10 | 2800 | 2000 | Mach   | Trigger SS on EARTH Mach / pop 1     |
    // | Noctilucent Train Bleu Traveler     | 81101309 | 10 | 3000 | 3000 | Mach   | Trigger SS on EARTH Mach / draw 1    |
    // | Heavy Knight Babel Decker           | 45116390 | 10 | 500  | 3000 | Mach   | NS no tribute / SS hand / Rk10 eff   |
    // | Machina Fortress                    | 5556499  | 7  | 2500 | 1600 | Mach   | Discard 8+ -> SS from hand/GY        |
    // | Inferno of the Sacred Beasts-Uria   | 23856331 | 10 | 0    | 0    | Pyro   | Reveal: search SB Trap / S/T pop     |
    // | Cannon Soldier MK-2                 | 14702066 | 5  | 1900 | 1200 | Mach   | Tribute 2 -> 1500 Burn (No OPT loop) |
    // | B.E.S. Blaster Cannon Core          | 84257883 | 9  | 2500 | 3000 | Mach   | SS from hand if opp has more mons    |
    // | Summoner of the Sacred Beasts       | 22734799 | 8  | 2300 | 3000 | Fiend  | SS SB monster from hand / GY         |
    // | Crystron Sulfefnir                  | 3422200  | 5  | 2000 | 1500 | Mach   | Discard Crystron -> SS from GY + pop |
    // | Crystron Sulfador                   | 25865565 | 5  | 1900 | 2200 | Mach   | Crystron extender                    |
    // | Convex Knight                       | 43471513 | 4  | 1800 | 500  | Mach   | SS if Machine / copy Lv / dump EARTH |
    // | Crystron Rosenix                    | 55326322 | 4  | 1800 | 1000 | Mach   | GY: banish -> create Level 1 token   |
    // | Flying Pegasus Railroad Stampede    | 88875132 | 4  | 1800 | 1000 | Mach   | NS/SS: revive EARTH Mach / match Lv  |
    // | Machina Unclaspare                  | 45674286 | 4  | 1800 | 800  | Mach   | On add: SS -> dump Machina to GY     |
    // | Gray Layer                          | 18616294 | 4  | 1200 | 1000 | Mach   | Tribute: search Mach + SS / GY pop   |
    // | Crystron Smiger                     | 83443619 | 3  | 1000 | 1800 | Mach   | GY: search Crystron S/T              |
    // | Martyr of the Sacred Beasts         | 59138498 | 1  | 0    | 0    | Fiend  | Place SB Field/Cont S/T on field     |
    // | Card of the Soul                    | 7044562  | 0  | 0    | 0    | Spell  | ATK+DEF = LP (8000) -> search Hamon  |
    // | Machina Redeployment                | 86852702 | 0  | 0    | 0    | Spell  | Discard 1 -> search 2 Machina mons   |
    // | Sacred Beasts Released              | 38776201 | 0  | 0    | 0    | Spell  | Search 3 SB mons -> discard 2        |
    // | Boss on Parade                      | 4909946  | 0  | 0    | 0    | CSpell | Search BES / SS LIGHT Mach from Deck |
    // | Sacred Beasts Thunderclap           | 1259915  | 0  | 0    | 0    | CSpell | Place 2 Thunderclap + Fallen Paradise|
    // | Fallen Paradise of the Sacred Beasts| 65861210 | 0  | 0    | 0    | Field  | SB protection + Draw 2 cards         |
    // | Revolving Switchyard                | 76136345 | 0  | 0    | 0    | Field  | Discard 1 -> search Lv10 EARTH Mach  |
    // |-------------------------------------|----------|----|------|------|--------|--------------------------------------|
    // | The Chaotic Phantasmal Sacred Beasts| 7894706  | 10 | 5000 | 5000 | Fusion | 3x Monster Negate Quick + Gain LP    |
    // | Superdreadnought Juggernaut Liebe   | 26096328 | 11 | 4000 | 4000 | Xyz    | Rk11 overlay -> 6000 ATK multi-attack|
    // | Superdreadnought Gustav Rocket      | 92359409 | 10 | 5000 | 3000 | Xyz    | Overlay on Gustav Max / 1k burn+neg  |
    // | Superdreadnought Flying Launcher    | 38354018 | 10 | 3800 | 3000 | Xyz    | Extra Mach NS / Search EARTH Mach    |
    // | Number 81: Super Dora               | 49032236 | 10 | 3200 | 4000 | Xyz    | Unaffected by card effects           |
    // | Superdreadnought Gustav Max         | 56910167 | 10 | 3000 | 3000 | Xyz    | Detach 1: 2000 Burn                  |
    // | Varudras, Final Bringer of End Times| 70636044 | 10 | 3000 | 2000 | Xyz    | Omni-Negate & Destroy                |
    // | Gear Gigant X                       | 28912357 | 4  | 2300 | 1500 | Xyz    | Detach 1: Search Lv4- Machine        |
    // | Infinitrack River Stormer           | 24701066 | 5  | 2500 | 500  | Xyz    | Detach 1: Search/Dump EARTH Machine  |
    // ====================================================================================================

    [Deck("SacrBeatsMach", "SacrBeatsMach", "Modern")]
    public class _2026_SacrBeatsMachExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int SuperBESMetalSlave = 41516133;
            public const int MachinaRuinforce = 46033517;
            public const int CalamityHamon = 50251045;
            public const int InfinityRaviel = 96345184;
            public const int YomagnaTheFirePhantom = 17350692;
            public const int HeavyFreightTrainDerricrane = 13647631;
            public const int NoctilucentTrainBleuTraveler = 81101309;
            public const int HeavyKnightBabelDecker = 45116390;
            public const int MachinaFortress = 5556499;
            public const int InfernoUria = 23856331;
            public const int RaSphereMode = 10000080;
            public const int BESBlasterCannonCore = 84257883;
            public const int SummonerOfTheSacredBeasts = 22734799;
            public const int CrystronSulfefnir = 3422200;
            public const int CannonSoldierMK2 = 14702066;
            public const int CrystronSulfador = 25865565;
            public const int ConvexKnight = 43471513;
            public const int CrystronRosenix = 55326322;
            public const int FlyingPegasusRailroadStampede = 88875132;
            public const int MachinaUnclaspare = 45674286;
            public const int GrayLayer = 18616294;
            public const int CrystronSmiger = 83443619;
            public const int MartyrOfTheSacredBeasts = 59138498;
            public const int CrystronTristaros = 99471856;

            // Main Deck Spells & Traps
            public const int CardOfTheSoul = 7044562;
            public const int MachinaRedeployment = 86852702;
            public const int MetalfoesFusion = 73594093;
            public const int SacredBeastsReleased = 38776201;
            public const int SmallWorld = 89558743;
            public const int BossOnParade = 4909946;
            public const int CrystronInclusion = 31552317;
            public const int SacredBeastsThunderclap = 1259915;
            public const int FallenParadiseOfTheSacredBeasts = 65861210;
            public const int RevolvingSwitchyard = 76136345;
            public const int SacredBeastsCombinedAssault = 50147815;

            // Extra Deck
            public const int TheChaoticPhantasmalSacredBeasts = 7894706;
            public const int ElShaddollMeshahrail = 32467459;
            public const int MixousiaTheConfounder = 80843006;
            public const int PredaplantDragostapelia = 69946549;
            public const int CrystronEleskeletus = 47736165;
            public const int JuggernautLiebe = 26096328;
            public const int GustavRocket = 92359409;
            public const int FlyingLauncher = 38354018;
            public const int SuperDora = 49032236;
            public const int GustavMax = 56910167;
            public const int Varudras = 70636044;
            public const int Zenmaioh = 77334267;
            public const int RiverStormer = 24701066;
            public const int GearGigantX = 28912357;
        }

        private bool _gustavBurnUsed = false;
        private int _cannonBurnCount = 0;

        public _2026_SacrBeatsMachExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Reset per turn states
            AddExecutor(ExecutorType.SpSummon, ResetTurnState);

            // =========================================================================
            // COUNTERS & FLOODGATE NEGATIONS (HIGHEST PRIORITY)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.Varudras, ShouldVarudrasActivate);
            AddExecutor(ExecutorType.Activate, CardId.GustavRocket, ShouldGustavRocketActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, ShouldSuperDoraActivate);

            // =========================================================================
            // FTK CANNON SOLDIER MK-2 BURN LOOP (PRIORITY 1 IF LIVE)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.CannonSoldierMK2, ShouldCannonSoldierActivate);

            // =========================================================================
            // STARTER & ENGINE SPELLS
            // =========================================================================
            // Card of the Soul: when LP=8000, searches Hamon/Raviel (4000/4000 = 8000)
            AddExecutor(ExecutorType.Activate, CardId.CardOfTheSoul, ShouldCardOfTheSoulActivate);

            // Sacred Beasts Released: Add 3 Sacred Beasts -> discard 2
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsReleased, ShouldSacredBeastsReleasedActivate);

            // Hand reveals for Sacred Beasts
            AddExecutor(ExecutorType.Activate, CardId.CalamityHamon, ShouldCalamityHamonActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfinityRaviel, ShouldInfinityRavielActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfernoUria, ShouldInfernoUriaActivate);

            // Field and Continuous Spells
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsThunderclap, ShouldThunderclapActivate);
            AddExecutor(ExecutorType.Activate, CardId.FallenParadiseOfTheSacredBeasts, ShouldFallenParadiseActivate);
            AddExecutor(ExecutorType.Activate, CardId.BossOnParade, ShouldBossOnParadeActivate);
            AddExecutor(ExecutorType.Activate, CardId.MachinaRedeployment, ShouldMachinaRedeploymentActivate);
            AddExecutor(ExecutorType.Activate, CardId.RevolvingSwitchyard, ShouldRevolvingSwitchyardActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrystronInclusion, ShouldCrystronInclusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.MetalfoesFusion, ShouldMetalfoesFusionActivate);

            // Small World bridge search
            AddExecutor(ExecutorType.Activate, CardId.SmallWorld, ShouldSmallWorldActivate);

            // =========================================================================
            // MONSTER SEARCH / IGNITION EXTENDERS
            // =========================================================================
            // Machina Unclaspare on add -> SS -> dump Ruinforce/Fortress
            AddExecutor(ExecutorType.Activate, CardId.MachinaUnclaspare);

            // Gray Layer tribute search & SS
            AddExecutor(ExecutorType.Activate, CardId.GrayLayer, ShouldGrayLayerActivate);

            // Convex Knight dump & level copy
            AddExecutor(ExecutorType.Activate, CardId.ConvexKnight, ShouldConvexKnightActivate);

            // Crystron GY triggers
            AddExecutor(ExecutorType.Activate, CardId.CrystronRosenix, ShouldRosenixActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrystronSulfefnir, ShouldSulfefnirActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrystronSmiger, ShouldSmigerActivate);

            // Summoner of Sacred Beasts & Martyr
            AddExecutor(ExecutorType.Activate, CardId.SummonerOfTheSacredBeasts, ShouldSummonerActivate);
            AddExecutor(ExecutorType.Activate, CardId.MartyrOfTheSacredBeasts, ShouldMartyrActivate);

            // Super B.E.S. Metal Slave
            AddExecutor(ExecutorType.Activate, CardId.SuperBESMetalSlave, ShouldMetalSlaveActivate);

            // =========================================================================
            // NORMAL SUMMONS
            // =========================================================================
            // If Cannon Soldier MK-2 is in hand and we can Tribute Loop FTK, summon it!
            AddExecutor(ExecutorType.Summon, CardId.CannonSoldierMK2, ShouldCannonSoldierSummon);

            // Flying Pegasus Railroad Stampede (revives Level 10 or 4 from GY)
            AddExecutor(ExecutorType.Summon, CardId.FlyingPegasusRailroadStampede);
            AddExecutor(ExecutorType.Activate, CardId.FlyingPegasusRailroadStampede, ShouldPegasusActivate);

            // Heavy Knight Babel Decker (no tribute)
            AddExecutor(ExecutorType.Summon, CardId.HeavyKnightBabelDecker);
            AddExecutor(ExecutorType.Activate, CardId.HeavyKnightBabelDecker);

            // Gray Layer normal summon
            AddExecutor(ExecutorType.Summon, CardId.GrayLayer);

            // Martyr of the Sacred Beasts normal summon
            AddExecutor(ExecutorType.Summon, CardId.MartyrOfTheSacredBeasts);

            // Convex Knight normal summon
            AddExecutor(ExecutorType.Summon, CardId.ConvexKnight);

            // Unclaspare normal summon if needed
            AddExecutor(ExecutorType.Summon, CardId.MachinaUnclaspare);

            // =========================================================================
            // SPECIAL SUMMON EXTENDERS
            // =========================================================================
            // Heavy Freight Train Derricrane trigger SS
            AddExecutor(ExecutorType.Activate, CardId.HeavyFreightTrainDerricrane);

            // Noctilucent Train Bleu Traveler trigger SS
            AddExecutor(ExecutorType.Activate, CardId.NoctilucentTrainBleuTraveler);

            // Machina Fortress SS
            AddExecutor(ExecutorType.SpSummon, CardId.MachinaFortress, ShouldFortressSpSummon);

            // Machina Ruinforce SS (from GY by banishing 12+ Machine levels)
            AddExecutor(ExecutorType.SpSummon, CardId.MachinaRuinforce, ShouldRuinforceSpSummon);

            // Machina Ruinforce GY floating trigger (revives up to 3 banished Machinas!)
            AddExecutor(ExecutorType.Activate, CardId.MachinaRuinforce);

            // Yomagna the Fire Phantom
            AddExecutor(ExecutorType.SpSummon, CardId.YomagnaTheFirePhantom);
            AddExecutor(ExecutorType.Activate, CardId.YomagnaTheFirePhantom);

            // =========================================================================
            // EXTRA DECK BOSS SUMMONS
            // =========================================================================
            // Rank 10: Gustav Max (2000 Burn)
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, ShouldGustavMaxSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, ShouldGustavMaxActivate);

            // Rank 10: Gustav Rocket (Overlays on Gustav Max with 0 materials!)
            AddExecutor(ExecutorType.SpSummon, CardId.GustavRocket, ShouldGustavRocketSpSummon);

            // Rank 10: Flying Launcher (Extra Normal Summon + Search)
            AddExecutor(ExecutorType.SpSummon, CardId.FlyingLauncher, ShouldFlyingLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FlyingLauncher);

            // Rank 10: Varudras (Omni-Negate)
            AddExecutor(ExecutorType.SpSummon, CardId.Varudras, ShouldVarudrasSpSummon);

            // Rank 10: Super Dora (Unaffected tower)
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, ShouldSuperDoraSpSummon);

            // Rank 11: Juggernaut Liebe (6000 ATK OTK — Battle Phase or going 2nd)
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, ShouldLiebeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe);

            // Rank 4 & 5 Extra Deck
            AddExecutor(ExecutorType.SpSummon, CardId.GearGigantX, ShouldGearGigantXSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GearGigantX);

            AddExecutor(ExecutorType.SpSummon, CardId.RiverStormer, ShouldRiverStormerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RiverStormer);

            // Fusion: The Chaotic Phantasmal Sacred Beasts
            AddExecutor(ExecutorType.SpSummon, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalSpSummon);

            // =========================================================================
            // REPOSITIONS & REMAINING SPELLS
            // =========================================================================
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool ResetTurnState()
        {
            _gustavBurnUsed = false;
            _cannonBurnCount = 0;
            return false;
        }

        #region FTK & Cannon Soldier MK-2 Loop
        private bool ShouldCannonSoldierSummon()
        {
            // Summon Cannon Soldier if we have Ruinforce in GY or another monster to start burning
            return Bot.HasInMonstersZone(CardId.MachinaRuinforce) ||
                   Bot.HasInGraveyard(CardId.MachinaRuinforce) ||
                   Bot.GetMonsterCount() >= 2 ||
                   Enemy.LifePoints <= 1500;
        }

        private bool ShouldCannonSoldierActivate()
        {
            if (Enemy.LifePoints <= 0) return false;

            // Cannon Soldier requires 2 tribute monsters
            var tributeCandidates = Bot.GetMonsters().Where(m => m != null && m.Id != CardId.CannonSoldierMK2).ToList();

            // If opponent has <= 1500 LP, we can even tribute Cannon Soldier itself if 2 monsters total!
            if (Enemy.LifePoints <= 1500 && Bot.GetMonsterCount() >= 2)
            {
                return true;
            }

            // Normal tribute loop: require at least 2 non-Cannon monsters
            if (tributeCandidates.Count >= 2)
            {
                _cannonBurnCount++;
                return true;
            }

            return false;
        }
        #endregion

        #region Starter & Engine Spells
        private bool ShouldCardOfTheSoulActivate()
        {
            // Card of the Soul searches monster with ATK+DEF = current LP (8000 = Hamon/Raviel)
            return Bot.LifePoints == 8000;
        }

        private bool ShouldSacredBeastsReleasedActivate()
        {
            // Search 3 Sacred Beasts -> discard 2
            return !DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool ShouldCalamityHamonActivate()
        {
            // In hand: reveal to search Sacred Beast Spell (Thunderclap or Fallen Paradise)
            return Card.Location == CardLocation.Hand &&
                   !Bot.HasInHandOrInSpellZone(CardId.SacredBeastsThunderclap) &&
                   !Bot.HasInHandOrInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
        }

        private bool ShouldInfinityRavielActivate()
        {
            // In hand: reveal to search Sacred Beast Monster (Martyr or Summoner)
            return Card.Location == CardLocation.Hand;
        }

        private bool ShouldInfernoUriaActivate()
        {
            // In hand: reveal to search Sacred Beast Trap
            return Card.Location == CardLocation.Hand &&
                   !Bot.HasInHandOrInSpellZone(CardId.SacredBeastsCombinedAssault);
        }

        private bool ShouldThunderclapActivate()
        {
            // Place 2 Thunderclap and 1 Fallen Paradise from Deck
            return !Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
        }

        private bool ShouldFallenParadiseActivate()
        {
            // Draw 2 cards if controlling Level 10 Sacred Beast
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.CalamityHamon || m.Id == CardId.InfinityRaviel || m.Id == CardId.InfernoUria || m.Id == CardId.TheChaoticPhantasmalSacredBeasts)))
            {
                return true;
            }
            return false;
        }

        private bool ShouldBossOnParadeActivate()
        {
            return !(Bot.HasInHand(CardId.SuperBESMetalSlave) || Bot.HasInMonstersZone(CardId.SuperBESMetalSlave));
        }

        private bool ShouldMachinaRedeploymentActivate()
        {
            return !DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool ShouldRevolvingSwitchyardActivate()
        {
            return !DefaultCheckWhetherCardIsNegated(Card) && Bot.Hand.Count >= 2;
        }

        private bool ShouldCrystronInclusionActivate()
        {
            return !DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool ShouldMetalfoesFusionActivate()
        {
            // In GY: shuffle into deck to draw 1 card!
            return Card.Location == CardLocation.Grave;
        }

        private bool ShouldSmallWorldActivate()
        {
            return Bot.Hand.Count >= 3;
        }
        #endregion

        #region Monster Extenders & Triggers
        private bool ShouldGrayLayerActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Tribute to search Machine with different attribute
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY banish to pop 1 card when own monster destroyed
                return Util.GetProblematicEnemyCard() != null || Enemy.GetMonsterCount() > 0;
            }
            return false;
        }

        private bool ShouldConvexKnightActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Special summon if Machine on field
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Machine));
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Send EARTH Machine from deck to GY
                return true;
            }
            return false;
        }

        private bool ShouldRosenixActivate()
        {
            // GY banish to create Level 1 Token (great tribute fodder for Cannon Soldier!)
            return Card.Location == CardLocation.Grave && Bot.GetMonsterCount() < 5;
        }

        private bool ShouldSulfefnirActivate()
        {
            return Card.Location == CardLocation.Grave && Bot.Hand.Any(c => c.HasSetcode(0xea));
        }

        private bool ShouldSmigerActivate()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool ShouldSummonerActivate()
        {
            return Card.Location == CardLocation.Hand && Bot.Hand.Count >= 2;
        }

        private bool ShouldMartyrActivate()
        {
            return !Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
        }

        private bool ShouldMetalSlaveActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Quick Effect: pop 2 cards
                return Duel.Player == 1 || Duel.Phase == DuelPhase.Battle || Util.GetProblematicEnemyCard() != null;
            }
            return false;
        }

        private bool ShouldPegasusActivate()
        {
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
        }
        #endregion

        #region Special Summons (Machina Bosses)
        private bool ShouldFortressSpSummon()
        {
            // Need Machines totaling Level 8+
            int availableLevels = Bot.Hand.Where(c => c.HasRace(CardRace.Machine) && c != Card).Sum(c => c.Level);
            if (Card.Location == CardLocation.Hand) availableLevels += Card.Level;
            return availableLevels >= 8;
        }

        private bool ShouldRuinforceSpSummon()
        {
            // Banishes Machines totaling Level 12+ from GY
            int gyMachineLevels = Bot.Graveyard.Where(c => c.HasRace(CardRace.Machine) && c != Card).Sum(c => c.Level);
            return gyMachineLevels >= 12;
        }
        #endregion

        #region Extra Deck Boss Summons
        private bool ShouldGustavMaxSpSummon()
        {
            // Rank 10: 2 Level 10 monsters
            var lv10s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 10).ToList();
            return lv10s.Count >= 2;
        }

        private bool ShouldGustavMaxActivate()
        {
            // Detach 1: 2000 Burn!
            _gustavBurnUsed = true;
            return true;
        }

        private bool ShouldGustavRocketSpSummon()
        {
            // Overlay on Gustav Max with 0 materials!
            var emptyGustav = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.GustavMax && m.Overlays.Count == 0);
            return emptyGustav != null;
        }

        private bool ShouldGustavRocketActivate()
        {
            // Quick effect negate & 1000 burn
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldFlyingLauncherSpSummon()
        {
            // Rank 10 if we have Normal Summonable Machine in hand (like Cannon Soldier)
            return Bot.HasInHand(CardId.CannonSoldierMK2) || Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10) >= 2;
        }

        private bool ShouldVarudrasSpSummon()
        {
            // Going first omni-negate
            return (Duel.Turn == 1 || Duel.Player == 0) &&
                   !Bot.HasInMonstersZone(CardId.Varudras) &&
                   Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10) >= 2;
        }

        private bool ShouldSuperDoraSpSummon()
        {
            return Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10) >= 2;
        }

        private bool ShouldSuperDoraActivate()
        {
            // Target face-up boss to grant immunity
            return Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Battle;
        }

        private bool ShouldLiebeSpSummon()
        {
            // Overlay on Rank 10 in Battle Phase or going second for 6000 ATK OTK
            if (Duel.Turn == 1) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.GustavMax || m.Id == CardId.SuperDora || m.Id == CardId.FlyingLauncher));
        }

        private bool ShouldGearGigantXSpSummon()
        {
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && m.HasRace(CardRace.Machine)).ToList();
            return lv4s.Count >= 2;
        }

        private bool ShouldRiverStormerSpSummon()
        {
            var lv5s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 5 && m.HasRace(CardRace.Machine)).ToList();
            return lv5s.Count >= 2;
        }

        private bool ShouldChaoticPhantasmalSpSummon()
        {
            // 3 Level 10 Sacred Beasts on field
            var sbs = Bot.GetMonsters().Where(m => m.IsFaceup() && (m.Id == CardId.CalamityHamon || m.Id == CardId.InfinityRaviel || m.Id == CardId.InfernoUria)).ToList();
            return sbs.Count >= 3;
        }

        private bool ShouldChaoticPhantasmalActivate()
        {
            // Quick Effect negate monster & gain LP
            return Duel.LastChainPlayer == 1 || Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled());
        }

        private bool ShouldVarudrasActivate()
        {
            return Duel.LastChainPlayer == 1;
        }
        #endregion

        #region Selection Handlers (OnSelectCard, OnSelectOption, OnSelectPosition)
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 500: Tribute
            if (hint == 500)
            {
                // If Cannon Soldier is tributing for FTK:
                // Priority: Machina Ruinforce (will trigger GY float) -> Tokens -> Fodder -> NEVER Cannon Soldier unless lethal
                var list = new List<ClientCard>();

                var ruinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ruinforce != null) list.Add(ruinforce);

                var token = cards.FirstOrDefault(c => c.HasType(CardType.Token));
                if (token != null && !list.Contains(token)) list.Add(token);

                foreach (var c in cards.OrderBy(c => c.Attack))
                {
                    if (c.Id != CardId.CannonSoldierMK2 && !list.Contains(c))
                    {
                        list.Add(c);
                    }
                }

                // If lethal 1500 and still need cards, allow Cannon Soldier
                if (Enemy.LifePoints <= 1500)
                {
                    foreach (var c in cards)
                    {
                        if (!list.Contains(c)) list.Add(c);
                    }
                }

                return list.Take(max).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                // Discard cards with GY effects
                var gyPreferred = cards.OrderByDescending(c =>
                    c.Id == CardId.MachinaRuinforce ? 100 :
                    c.Id == CardId.MachinaFortress ? 90 :
                    c.Id == CardId.CrystronRosenix ? 85 :
                    c.Id == CardId.CrystronSulfefnir ? 80 :
                    c.Id == CardId.CrystronSmiger ? 75 :
                    c.Id == CardId.MetalfoesFusion ? 70 :
                    c.Id == CardId.GrayLayer ? 65 : 10
                ).ToList();
                return gyPreferred.Take(max).ToList();
            }

            // Hint 502: Destroy
            if (hint == 502)
            {
                // Target enemy problematic cards first
                var enemyTargets = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                if (enemyTargets.Count > 0)
                {
                    return enemyTargets.Take(max).ToList();
                }
            }

            // Hint 505: Search / Add to Hand
            if (hint == 505)
            {
                // If we need Cannon Soldier MK-2 for FTK:
                if (!(Bot.HasInHand(CardId.CannonSoldierMK2) || Bot.HasInMonstersZone(CardId.CannonSoldierMK2)))
                {
                    var cannon = cards.FirstOrDefault(c => c.Id == CardId.CannonSoldierMK2);
                    if (cannon != null) return new List<ClientCard> { cannon };
                }

                // Machina searches
                var ruinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ruinforce != null && !(Bot.HasInHand(CardId.MachinaRuinforce) || Bot.HasInMonstersZone(CardId.MachinaRuinforce))) return new List<ClientCard> { ruinforce };

                var unclaspare = cards.FirstOrDefault(c => c.Id == CardId.MachinaUnclaspare);
                if (unclaspare != null) return new List<ClientCard> { unclaspare };

                // Sacred Beasts searches
                var hamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (hamon != null && !Bot.HasInHand(CardId.CalamityHamon)) return new List<ClientCard> { hamon };

                var thunderclap = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsThunderclap);
                if (thunderclap != null) return new List<ClientCard> { thunderclap };
            }

            // Hint 508: Send to Graveyard
            if (hint == 508)
            {
                var dumpRuinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (dumpRuinforce != null) return new List<ClientCard> { dumpRuinforce };

                var dumpFortress = cards.FirstOrDefault(c => c.Id == CardId.MachinaFortress);
                if (dumpFortress != null) return new List<ClientCard> { dumpFortress };

                var dumpRosenix = cards.FirstOrDefault(c => c.Id == CardId.CrystronRosenix);
                if (dumpRosenix != null) return new List<ClientCard> { dumpRosenix };
            }

            // Hint 509: Special Summon
            if (hint == 509)
            {
                // Prioritize Ruinforce -> Fortress -> Cannon Soldier
                var ssRuinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ssRuinforce != null) return new List<ClientCard> { ssRuinforce };

                var ssFortress = cards.FirstOrDefault(c => c.Id == CardId.MachinaFortress);
                if (ssFortress != null) return new List<ClientCard> { ssFortress };

                var ssCannon = cards.FirstOrDefault(c => c.Id == CardId.CannonSoldierMK2);
                if (ssCannon != null) return new List<ClientCard> { ssCannon };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.CannonSoldierMK2 ||
                cardId == CardId.MachinaRuinforce ||
                cardId == CardId.GustavMax ||
                cardId == CardId.GustavRocket ||
                cardId == CardId.JuggernautLiebe ||
                cardId == CardId.TheChaoticPhantasmalSacredBeasts)
            {
                return CardPosition.FaceUpAttack;
            }

            if (cardId == CardId.SuperDora ||
                cardId == CardId.Varudras)
            {
                return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }
        #endregion
    }
}
