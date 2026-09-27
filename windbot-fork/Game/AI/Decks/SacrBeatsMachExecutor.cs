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
    // 100% verified against cards.cdb and SacrBeatsMach.ydk (Streamlined 42-card Competitive Version)
    // ====================================================================================================
    // | Card Name                               | ID       | Lv | ATK  | DEF  | Type   | Key Role                                  |
    // |-----------------------------------------|----------|----|------|------|--------|-------------------------------------------|
    // | Calamity of the Sacred Beasts - Hamon   | 50251045 | 10 | 4000 | 4000 | Thund  | Hand: search SB Spell / 1000 GY burn      |
    // | Infinity of the Sacred Beasts - Raviel  | 96345184 | 10 | 4000 | 4000 | Fiend  | Hand: search SB Monster / Boardwipe Quick |
    // | Inferno of the Sacred Beasts - Uria     | 23856331 | 10 | 0    | 0    | Pyro   | Hand: search SB Trap / S/T pop Quick      |
    // | Summoner of the Sacred Beasts           | 22734799 | 8  | 2300 | 3000 | Fiend  | SS SB monster from Hand/GY / GY Revive    |
    // | Martyr of the Sacred Beasts             | 59138498 | 1  | 0    | 0    | Fiend  | Place SB Field/Cont S/T / SS 2 Martyrs    |
    // | Sacred Beasts Released                  | 38776201 | 0  | 0    | 0    | Spell  | Add 3 SB monsters -> discard 2 (+1 setup) |
    // | Sacred Beasts Thunderclap               | 1259915  | 0  | 0    | 0    | CSpell | Place 2 Thunderclap + Fallen Paradise     |
    // | Fallen Paradise of the Sacred Beasts    | 65861210 | 0  | 0    | 0    | Field  | Send 3 to SS SB from Deck / Draw 2 cards  |
    // | Sacred Beasts Combined Assault          | 50147815 | 0  | 0    | 0    | Trap   | Quick SS SB + Negate & Pop / GY Fusion    |
    // | Machina Ruinforce                       | 46033517 | 10 | 4600 | 4100 | Mach   | GY SS banish 12+ / Halve LP / Float 3 ban |
    // | Machina Fortress                        | 5556499  | 7  | 2500 | 1600 | Mach   | Discard 8+ -> SS from hand/GY / Battle Pop|
    // | Machina Unclaspare                      | 45674286 | 4  | 1800 | 800  | Mach   | On add: SS -> dump Machina to GY          |
    // | Machina Redeployment                    | 86852702 | 0  | 0    | 0    | Spell  | Discard 1 -> search 2 Machina monsters    |
    // | Cannon Soldier MK-2                     | 14702066 | 5  | 1900 | 1200 | Mach   | Tribute 2 -> 1500 Burn (No OPT loop)      |
    // | Heavy Freight Train Derricrane          | 13647631 | 10 | 2800 | 2000 | Mach   | Trigger SS on EARTH Mach / Xyz detach pop |
    // | Noctilucent Train Bleu Traveler         | 81101309 | 10 | 2500 | 2500 | Mach   | Hand: search Switchyard / GY: double SS!  |
    // | Heavy Knight Babel Decker               | 45116390 | 10 | 500  | 3000 | Mach   | NS no tribute / SS hand / Quick Rk10 eff  |
    // | Flying Pegasus Railroad Stampede        | 88875132 | 4  | 1800 | 1000 | Mach   | NS: Revive EARTH Mach / match Lv to 10    |
    // | Revolving Switchyard                    | 76136345 | 0  | 0    | 0    | Field  | Discard 1 -> search Lv10 EARTH Machine    |
    // | Super B.E.S. Metal Slave                | 41516133 | 11 | 3100 | 3100 | Mach   | Dump BES -> SS + Quick pop 2 cards        |
    // | B.E.S. Blaster Cannon Core              | 84257883 | 9  | 2500 | 3000 | Mach   | SS from hand if opp has more monsters     |
    // | Boss on Parade                          | 4909946  | 0  | 0    | 0    | CSpell | Search BES / Destroy 1 to SS Gray Layer   |
    // | Gray Layer                              | 18616294 | 4  | 1200 | 1000 | Mach   | Tribute: search Machine + SS / GY pop     |
    // | Crystron Rosenix                        | 55326322 | 4  | 1800 | 1000 | Mach   | GY: banish -> create Level 1 token        |
    // | Yomagna the Fire Phantom                | 17350692 | 10 | 3300 | 0    | Pyro   | Shuffle 3 GY -> SS + Super Poly on field  |
    // | Twin Twisters                           | 43898403 | 0  | 0    | 0    | QSpell | Discard 1 -> Destroy up to 2 Spells/Traps |
    // |-----------------------------------------|----------|----|------|------|--------|-------------------------------------------|
    // | The Chaotic Phantasmal Sacred Beasts    | 7894706  | 10 | 5000 | 5000 | Fusion | 3x Monster Negate Quick + Gain LP         |
    // | Armityle the Chaos Phantasm             | 43378048 | 12 | 0    | 0    | Fusion | 10,000 ATK Battle Phase Finisher          |
    // | Varudras, Final Bringer of End Times    | 70636044 | 10 | 3000 | 2000 | Xyz    | Omni-Negate & Destroy (Priority #1)       |
    // | Superdreadnought Gustav Max             | 56910167 | 10 | 3000 | 3000 | Xyz    | Detach 1: 2000 Burn (2x in Extra)         |
    // | Superdreadnought Gustav Rocket          | 92359409 | 10 | 5000 | 3000 | Xyz    | Overlay on Gustav Max / 1k burn + negate  |
    // | Superdreadnought Juggernaut Liebe       | 26096328 | 11 | 4000 | 4000 | Xyz    | Overlay on Rk10 -> 6000 ATK multi-attack  |
    // | Number 81: Super Dora                   | 49032236 | 10 | 3200 | 4000 | Xyz    | Unaffected by card effects                |
    // | Superdreadnought Flying Launcher        | 38354018 | 10 | 3800 | 3000 | Xyz    | Extra Mach NS / Search EARTH Mach / pop ST|
    // | Gear Gigant X                           | 28912357 | 4  | 2300 | 1500 | Xyz    | Detach 1: Search Lv4- Machine             |
    // | Infinitrack River Stormer               | 24701066 | 5  | 2500 | 500  | Xyz    | Detach 1: Search/Dump EARTH Machine       |
    // ====================================================================================================

    [Deck("SacrBeatsMach", "SacrBeatsMach", "Modern")]
    [Deck("ScarbeatMach", "SacrBeatsMach", "Modern")]
    [Deck("scarbeatmach", "SacrBeatsMach", "Modern")]
    [Deck("2026_SacrBeatsMach", "SacrBeatsMach", "Modern")]
    [Deck("2026_ScarbeatMach", "SacrBeatsMach", "Modern")]
    public class _2026_SacrBeatsMachExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Sacred Beasts
            public const int CalamityHamon = 50251045;
            public const int InfinityRaviel = 96345184;
            public const int InfernoUria = 23856331;
            public const int SummonerOfTheSacredBeasts = 22734799;
            public const int MartyrOfTheSacredBeasts = 59138498;
            public const int SacredBeastsReleased = 38776201;
            public const int SacredBeastsThunderclap = 1259915;
            public const int FallenParadiseOfTheSacredBeasts = 65861210;
            public const int SacredBeastsCombinedAssault = 50147815;

            // Main Deck Machina & Trains
            public const int MachinaRuinforce = 46033517;
            public const int MachinaFortress = 5556499;
            public const int MachinaUnclaspare = 45674286;
            public const int MachinaRedeployment = 86852702;
            public const int CannonSoldierMK2 = 14702066;
            public const int HeavyFreightTrainDerricrane = 13647631;
            public const int NoctilucentTrainBleuTraveler = 81101309;
            public const int HeavyKnightBabelDecker = 45116390;
            public const int FlyingPegasusRailroadStampede = 88875132;
            public const int RevolvingSwitchyard = 76136345;

            // Main Deck B.E.S. & Extenders
            public const int SuperBESMetalSlave = 41516133;
            public const int BESBlasterCannonCore = 84257883;
            public const int BossOnParade = 4909946;
            public const int GrayLayer = 18616294;
            public const int CrystronRosenix = 55326322;
            public const int YomagnaTheFirePhantom = 17350692;

            // Board Breakers & Removal
            public const int HarpiesFeatherDuster = 18144506;
            public const int Raigeki = 12580477;
            public const int TwinTwisters = 43898403;
            public const int RaSphereMode = 10000080;

            // Extra Deck
            public const int TheChaoticPhantasmalSacredBeasts = 7894706;
            public const int ArmityleTheChaosPhantasm = 43378048;
            public const int Varudras = 70636044;
            public const int GustavMax = 56910167;
            public const int GustavRocket = 92359409;
            public const int SuperDora = 49032236;
            public const int JuggernautLiebe = 26096328;
            public const int FlyingLauncher = 38354018;
            public const int MixousiaTheConfounder = 80843006;
            public const int PredaplantDragostapelia = 69946549;
            public const int CrystronEleskeletus = 47736165;
            public const int RiverStormer = 24701066;
            public const int GearGigantX = 28912357;
        }

        private static readonly HashSet<int> AceCardIds = new HashSet<int>
        {
            CardId.TheChaoticPhantasmalSacredBeasts,
            CardId.ArmityleTheChaosPhantasm,
            CardId.Varudras,
            CardId.GustavMax,
            CardId.GustavRocket,
            CardId.SuperDora,
            CardId.JuggernautLiebe,
            CardId.MachinaRuinforce,
            CardId.MachinaFortress,
            CardId.SuperBESMetalSlave,
            CardId.CalamityHamon,
            CardId.InfinityRaviel,
            CardId.InfernoUria
        };

        private int _gustavBurnUsed = 0;

        public _2026_SacrBeatsMachExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Reset per turn counters
            AddExecutor(ExecutorType.SpSummon, ResetTurnState);

            // =========================================================================
            // TIER 0: COUNTERS, OMNI-NEGATES & QUICK INTERRUPTIONS (HIGHEST PRIORITY)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.Varudras, ShouldVarudrasActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalActivate);
            AddExecutor(ExecutorType.Activate, CardId.GustavRocket, ShouldGustavRocketActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, ShouldSuperDoraActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperBESMetalSlave, ShouldMetalSlaveQuickPop);
            AddExecutor(ExecutorType.Activate, CardId.InfernoUria, ShouldUriaQuickPop);
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsCombinedAssault, ShouldCombinedAssaultTrapActivate);
            AddExecutor(ExecutorType.Activate, CardId.MachinaRuinforce, ShouldRuinforceBattleNegate);

            // =========================================================================
            // TIER 1: REMOVAL & ANTI-FLOODGATE (BEFORE EXECUTING EXTENSION)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, CardId.TwinTwisters, ShouldTwinTwistersActivate);

            // =========================================================================
            // TIER 3: SEARCH & DECK STARTERS
            // =========================================================================
            // Sacred Beasts Released: Add 3 Sacred Beasts -> discard 2 (+1 setup)
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsReleased, ShouldSacredBeastsReleasedActivate);

            // Sacred Beasts hand reveals
            AddExecutor(ExecutorType.Activate, CardId.CalamityHamon, ShouldCalamityHamonHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfinityRaviel, ShouldInfinityRavielHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfernoUria, ShouldInfernoUriaHandActivate);

            // Machina Redeployment: Discard 1 -> Add 2 Machinas
            AddExecutor(ExecutorType.Activate, CardId.MachinaRedeployment, ShouldMachinaRedeploymentActivate);

            // Noctilucent Train Bleu Traveler: Hand discard -> search Switchyard
            AddExecutor(ExecutorType.Activate, CardId.NoctilucentTrainBleuTraveler, ShouldBleuTravelerHandActivate);

            // Sacred Beasts Thunderclap: Place 2 Thunderclap + 1 Fallen Paradise
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsThunderclap, ShouldThunderclapActivate);

            // Boss on Parade: Search BES / Destroy 1 to SS Gray Layer from Deck
            AddExecutor(ExecutorType.Activate, CardId.BossOnParade, ShouldBossOnParadeActivate);

            // Fallen Paradise of the Sacred Beasts:
            // Hand: place into Field Zone
            // Field: Draw 2 or send 3 to SS from Deck/GY
            AddExecutor(ExecutorType.Activate, CardId.FallenParadiseOfTheSacredBeasts, ShouldFallenParadiseActivate);

            // Revolving Switchyard: Discard 1 -> Add Lv10 EARTH Machine
            AddExecutor(ExecutorType.Activate, CardId.RevolvingSwitchyard, ShouldRevolvingSwitchyardActivate);

            // Sacred Beasts Combined Assault: GY banish -> Fusion Summon Phantasm Boss
            AddExecutor(ExecutorType.Activate, CardId.SacredBeastsCombinedAssault, ShouldCombinedAssaultGYActivate);

            // =========================================================================
            // TIER 4: MONSTER IGNITION & GRAVEYARD EXTENDERS
            // =========================================================================
            // Gray Layer: Tribute -> Search Machine + SS from hand
            AddExecutor(ExecutorType.Activate, CardId.GrayLayer, ShouldGrayLayerActivate);

            // Crystron Rosenix: GY banish -> Level 1 Token (tribute fodder)
            AddExecutor(ExecutorType.Activate, CardId.CrystronRosenix, ShouldRosenixActivate);

            // Noctilucent Train Bleu Traveler: GY double revival (Bleu + another Lv10 EARTH Machine)
            AddExecutor(ExecutorType.Activate, CardId.NoctilucentTrainBleuTraveler, ShouldBleuTravelerGYRevive);

            // Summoner of Sacred Beasts: Hand/Field/GY revives
            AddExecutor(ExecutorType.Activate, CardId.SummonerOfTheSacredBeasts, ShouldSummonerActivate);

            // Martyr of the Sacred Beasts: On summon -> place S/T + SS 2 Martyrs
            AddExecutor(ExecutorType.Activate, CardId.MartyrOfTheSacredBeasts, ShouldMartyrActivate);

            // Super B.E.S. Metal Slave: Dump BES to SS from hand
            AddExecutor(ExecutorType.Activate, CardId.SuperBESMetalSlave, ShouldMetalSlaveHandActivate);

            // Heavy Knight Babel Decker: SS EARTH Machine from hand
            AddExecutor(ExecutorType.Activate, CardId.HeavyKnightBabelDecker, ShouldBabelDeckerHandSS);

            // Flying Pegasus Railroad Stampede: Revive Lv10 EARTH Machine + match level to 10
            AddExecutor(ExecutorType.Activate, CardId.FlyingPegasusRailroadStampede, ShouldPegasusActivate);

            // =========================================================================
            // TIER 5: NORMAL SUMMONS
            // =========================================================================
            // 1. Flying Pegasus Railroad Stampede (revives Lv10 EARTH Machine from GY)
            AddExecutor(ExecutorType.Summon, CardId.FlyingPegasusRailroadStampede, ShouldPegasusSummon);

            // 2. Heavy Knight Babel Decker (no tribute required)
            AddExecutor(ExecutorType.Summon, CardId.HeavyKnightBabelDecker);

            // 3. Gray Layer (searches Machine on tribute)
            AddExecutor(ExecutorType.Summon, CardId.GrayLayer);

            // 4. Martyr of the Sacred Beasts (triggers S/T placement + 2 SS)
            AddExecutor(ExecutorType.Summon, CardId.MartyrOfTheSacredBeasts);

            // =========================================================================
            // TIER 6: SPECIAL SUMMON EXTENDERS
            // =========================================================================
            // Heavy Freight Train Derricrane trigger SS on EARTH Machine summon
            AddExecutor(ExecutorType.Activate, CardId.HeavyFreightTrainDerricrane);

            // B.E.S. Blaster Cannon Core (free SS if opp controls more monsters)
            AddExecutor(ExecutorType.SpSummon, CardId.BESBlasterCannonCore, ShouldBlasterCannonSpSummon);

            // Machina Fortress SS (from hand/GY by discarding 8+ levels)
            AddExecutor(ExecutorType.SpSummon, CardId.MachinaFortress, ShouldFortressSpSummon);

            // Machina Ruinforce SS (from GY by banishing 12+ Machine levels)
            AddExecutor(ExecutorType.SpSummon, CardId.MachinaRuinforce, ShouldRuinforceSpSummon);

            // Machina Ruinforce floating trigger (revives up to 3 banished Machinas on destruction/tribute!)
            AddExecutor(ExecutorType.Activate, CardId.MachinaRuinforce, ShouldRuinforceFloatTrigger);

            // Yomagna the Fire Phantom (SS by shuffling 3 GY cards of same type)
            AddExecutor(ExecutorType.SpSummon, CardId.YomagnaTheFirePhantom);
            AddExecutor(ExecutorType.Activate, CardId.YomagnaTheFirePhantom);

            // =========================================================================
            // TIER 7: EXTRA DECK SUMMONS (PRIORITY: VARUDRAS & PHANTASMAL BOSSES FIRST!)
            // =========================================================================
            // 1. The Chaotic Phantasmal Sacred Beasts (Contact Fusion: Send 3 Lv10 non-normal summonable)
            AddExecutor(ExecutorType.SpSummon, CardId.TheChaoticPhantasmalSacredBeasts, ShouldChaoticPhantasmalSpSummon);

            // 2. Rank 10: Varudras (OMNI-NEGATE & DESTROY — HIGHEST RANK 10 PRIORITY)
            AddExecutor(ExecutorType.SpSummon, CardId.Varudras, ShouldVarudrasSpSummon);

            // 3. Rank 10: Gustav Max (2000 Burn each — after Varudras or when lethal!)
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, ShouldGustavMaxSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, ShouldGustavMaxActivate);

            // 4. Rank 10: Gustav Rocket (Overlays on 0-material Gustav Max with 1 discard)
            AddExecutor(ExecutorType.SpSummon, CardId.GustavRocket, ShouldGustavRocketSpSummon);

            // 5. Armityle the Chaos Phantasm (10,000 ATK finisher)
            AddExecutor(ExecutorType.SpSummon, CardId.ArmityleTheChaosPhantasm, ShouldArmityleSpSummon);

            // 6. Rank 11: Juggernaut Liebe (6000 ATK multi-attack OTK machine)
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, ShouldLiebeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe);

            // 7. Rank 10: Super Dora (3200/4000 Unaffected tower)
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, ShouldSuperDoraSpSummon);

            // 8. Rank 10: Flying Launcher (Extra NS + Search)
            AddExecutor(ExecutorType.SpSummon, CardId.FlyingLauncher, ShouldFlyingLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FlyingLauncher);

            // 9. Rank 4 & 5 Searchers
            AddExecutor(ExecutorType.SpSummon, CardId.GearGigantX, ShouldGearGigantXSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GearGigantX);

            AddExecutor(ExecutorType.SpSummon, CardId.RiverStormer, ShouldRiverStormerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RiverStormer);

            // =========================================================================
            // TIER 8: TRAP SETTING & REPOSITIONS
            // =========================================================================
            AddExecutor(ExecutorType.SpellSet, CardId.SacredBeastsCombinedAssault);
            AddExecutor(ExecutorType.SpellSet, CardId.TwinTwisters, ShouldTwinTwistersSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool ResetTurnState()
        {
            _gustavBurnUsed = 0;
            return false;
        }

        #region Tier 0: Counters & Quick Effects
        private bool ShouldVarudrasActivate()
        {
            // Detach 1: Negate any opponent activation & destroy it, then destroy 1 card on field!
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldChaoticPhantasmalActivate()
        {
            // Up to 3x per turn: Quick negate monster effect & gain LP equal to half ATK
            var dangerousOppMonster = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled())
                .OrderByDescending(m => (m.HasType(CardType.Effect) ? 10000 : 0) + m.Attack)
                .FirstOrDefault();

            if (dangerousOppMonster != null)
            {
                AI.SelectCard(dangerousOppMonster);
                return true;
            }

            if (Duel.LastChainPlayer == 1) return true;
            return false;
        }

        private bool ShouldGustavRocketActivate()
        {
            // Quick effect: Negate monster effect & destroy + 1000 burn
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldSuperDoraActivate()
        {
            // Quick effect: Make our best boss unaffected
            if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Battle)
            {
                var bestTarget = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() && !m.HasType(CardType.Token))
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();
                if (bestTarget != null)
                {
                    AI.SelectCard(bestTarget);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldMetalSlaveQuickPop()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // 1. If opponent has Eternal Soul: ALWAYS destroy it! (Leaves field -> wipes all enemy monsters!)
            if (Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == 48680970)) return true;

            // 2. If we have another BES monster on field (BESBlasterCannonCore), we can safely destroy that one instead of Metal Slave!
            bool hasOtherBES = Bot.GetMonsters().Any(m => m != null && m != Card && m.IsFaceup() && m.Id == CardId.BESBlasterCannonCore);
            if (hasOtherBES && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)) return true;

            // 3. Dodge removal if targeted by opponent
            if (Duel.LastChainPlayer == 1 && Duel.ChainTargets.Contains(Card)) return true;

            // NEVER self-destruct Metal Slave randomly in battle or idle!
            return false;
        }

        private bool ShouldUriaQuickPop()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var enemyST = Enemy.GetSpells().FirstOrDefault(s => s != null);
            if (enemyST != null)
            {
                AI.SelectCard(enemyST);
                return true;
            }
            return false;
        }

        private bool ShouldCombinedAssaultTrapActivate()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            // Special Summon SB monster from Hand or GY in DEF position
            bool hasTarget = Bot.Hand.Any(c => IsSacredBeastMonster(c)) || Bot.Graveyard.Any(c => IsSacredBeastMonster(c));
            return hasTarget;
        }

        private bool ShouldCombinedAssaultGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Banish from GY -> Fusion Summon The Chaotic Phantasmal Sacred Beasts or Armityle
            // Strictly requires 3 Level 10 monsters that cannot be Normal Summoned/Set from Hand or Field
            int validMaterials = Bot.Hand.Count(c => c.IsMonster() && IsNonNormalSummonableLv10(c)) +
                                 Bot.GetMonsters().Count(m => m.IsFaceup() && IsNonNormalSummonableLv10(m));
            return validMaterials >= 3;
        }

        private bool ShouldRuinforceBattleNegate()
        {
            return Duel.Phase == DuelPhase.Battle && Duel.LastChainPlayer == 1 && Bot.LifePoints > 1000;
        }

        private bool ShouldRuinforceFloatTrigger()
        {
            return Card.Location == CardLocation.Grave;
        }
        #endregion

        #region Tier 1: Removal & Anti-Floodgate
        private bool ShouldTwinTwistersActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;

            var targets = Enemy.GetSpells();
            if (targets.Count == 0) return false;

            // Prioritize dangerous enemy continuous / set backrow
            var priorityST = targets.FirstOrDefault(s => Util.GetProblematicEnemyCard() == s || s.IsFaceup() || s.IsFacedown());
            if (priorityST != null)
            {
                AI.SelectCard(priorityST);
            }

            return Bot.Hand.Count(c => c != Card) >= 1;
        }

        private bool ShouldTwinTwistersSet()
        {
            return Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2;
        }
        #endregion

        #region Tier 3: Starters & Spells
        private bool ShouldSacredBeastsReleasedActivate()
        {
            return !DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool ShouldCalamityHamonHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInHandOrInSpellZone(CardId.SacredBeastsThunderclap) ||
                   !Bot.HasInHandOrInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
        }

        private bool ShouldInfinityRavielHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return true;
        }

        private bool ShouldInfernoUriaHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInHandOrInSpellZone(CardId.SacredBeastsCombinedAssault);
        }

        private bool ShouldThunderclapActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            return !Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts) || Bot.GetSpellCount() < 4;
        }

        private bool ShouldBossOnParadeActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            if (Card.Location == CardLocation.Hand) return true;

            // Continuous Spell on field: Destroy 1 to SS Gray Layer from Deck
            bool hasGrayInDeck = Bot.Deck.Any(c => c.Id == CardId.GrayLayer);
            if (!hasGrayInDeck) return false;

            // Only destroy if we have expendable fodder
            return Bot.Hand.Any(c => c.IsMonster() && IsExpendableMonster(c)) ||
                   Bot.GetMonsters().Any(m => m != null && IsExpendableMonster(m));
        }

        private bool ShouldMachinaRedeploymentActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            return Bot.Hand.Count >= 2;
        }

        private bool ShouldBleuTravelerHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInHandOrInSpellZone(CardId.RevolvingSwitchyard);
        }

        private bool ShouldFallenParadiseActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;

            // Hand activation: Always play into Field Zone if not present!
            if (Card.Location == CardLocation.Hand || ActivateDescription == -1)
            {
                return !Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts);
            }

            // Field Zone effect 1: If we control Lv10 Sacred Beast, draw 2 cards!
            if (ActivateDescription == Util.GetStringId(CardId.FallenParadiseOfTheSacredBeasts, 1))
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && IsSacredBeastLv10(m));
            }

            // Field Zone effect 0: Send 3 cards to SS Sacred Beast from Deck/GY
            if (ActivateDescription == Util.GetStringId(CardId.FallenParadiseOfTheSacredBeasts, 0))
            {
                // Spells: 3+ spells available (Thunderclap on field + hand spells)
                int availableSpells = Bot.GetSpells().Count(s => s != null && s.IsFaceup() && s != Card) + Bot.Hand.Count(c => c.IsSpell());
                if (availableSpells >= 3)
                {
                    return true;
                }

                // Monsters: 3+ expendable monsters (Tokens, Martyrs, Gray Layer, Unclaspare — NEVER Aces!)
                int expendables = Bot.GetMonsters().Count(m => m != null && IsExpendableMonster(m)) +
                                  Bot.Hand.Count(c => c.IsMonster() && IsExpendableMonster(c));
                if (expendables >= 3)
                {
                    return true;
                }

                return false;
            }

            // Fallback general query
            return Bot.GetMonsters().Any(m => m.IsFaceup() && IsSacredBeastLv10(m));
        }

        private bool ShouldRevolvingSwitchyardActivate()
        {
            if (DefaultCheckWhetherCardIsNegated(Card)) return false;
            // CRITICAL: NEVER overwrite Fallen Paradise! Fallen Paradise is our irreplaceable core protection + draw engine
            if (Bot.HasInSpellZone(CardId.FallenParadiseOfTheSacredBeasts)) return false;

            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.RevolvingSwitchyard);
            }
            return Bot.Hand.Count >= 1;
        }
        #endregion

        #region Tier 4: Monster Extenders & Ignition
        private bool ShouldGrayLayerActivate()
        {
            if (Card.Location == CardLocation.MonsterZone) return true;
            if (Card.Location == CardLocation.Grave)
            {
                return Util.GetProblematicEnemyCard() != null || Enemy.GetMonsterCount() > 0;
            }
            return false;
        }

        private bool ShouldRosenixActivate()
        {
            return Card.Location == CardLocation.Grave && Bot.GetMonsterCount() < 5;
        }

        private bool ShouldBleuTravelerGYRevive()
        {
            if (Card.Location != CardLocation.Grave) return false;
            bool hasOtherEarthMach = Bot.Graveyard.Any(c => c != Card && c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth) && c.Level == 10);
            return hasOtherEarthMach && Bot.GetMonsterCount() <= 3;
        }

        private bool ShouldSummonerActivate()
        {
            if (Card.Location == CardLocation.Hand) return Bot.Hand.Count >= 2;
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => IsSacredBeastMonster(c) && c.Id != CardId.SummonerOfTheSacredBeasts);
            }
            return false;
        }

        private bool ShouldMartyrActivate()
        {
            return true;
        }

        private bool ShouldMetalSlaveHandActivate()
        {
            return Card.Location == CardLocation.Hand;
        }

        private bool ShouldBabelDeckerHandSS()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            return Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
        }

        private bool ShouldPegasusSummon()
        {
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
        }

        private bool ShouldPegasusActivate()
        {
            // Effect 1: Revive on summon
            if (Card.Location == CardLocation.MonsterZone && Bot.Graveyard.Any(c => c != Card && c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth)))
            {
                return true;
            }
            // Effect 2: Match level to 10 with another face-up Lv10 monster
            if (Card.Location == CardLocation.MonsterZone && Card.Level != 10 && Bot.GetMonsters().Any(m => m.IsFaceup() && m != Card && m.Level == 10))
            {
                var targetLv10 = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m != Card && m.Level == 10);
                if (targetLv10 != null)
                {
                    AI.SelectCard(targetLv10);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Tier 6: Special Summons
        private bool ShouldBlasterCannonSpSummon()
        {
            return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
        }

        private bool ShouldFortressSpSummon()
        {
            int availableLevels = Bot.Hand.Where(c => c.HasRace(CardRace.Machine) && c != Card).Sum(c => c.Level);
            if (Card.Location == CardLocation.Hand) availableLevels += Card.Level;
            return availableLevels >= 8;
        }

        private bool ShouldRuinforceSpSummon()
        {
            int gyMachineLevels = Bot.Graveyard.Where(c => c.HasRace(CardRace.Machine) && c != Card).Sum(c => c.Level);
            return gyMachineLevels >= 12;
        }
        #endregion

        #region Tier 7: Extra Deck Boss Summons
        private bool ShouldChaoticPhantasmalSpSummon()
        {
            // Contact Fusion: Send 3 Lv10 non-normal summonable monsters from field
            var candidates = Bot.GetMonsters().Where(m => m.IsFaceup() && IsNonNormalSummonableLv10(m)).ToList();
            return candidates.Count >= 3;
        }

        private bool ShouldVarudrasSpSummon()
        {
            // Varudras is Rank 10 Priority #1: Omni-Negate & Destroy
            if (Bot.HasInMonstersZone(CardId.Varudras)) return false;

            // Ensure we don't consume The Chaotic Phantasmal Sacred Beasts as material!
            int eligibleLv10s = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10 && m.Id != CardId.TheChaoticPhantasmalSacredBeasts);
            return eligibleLv10s >= 2;
        }

        private bool ShouldGustavMaxSpSummon()
        {
            // If lethal burn (Enemy LP <= 2000 or <= 3000 with Rocket), summon Gustav immediately!
            if (Enemy.LifePoints <= 2000) return true;
            if (Enemy.LifePoints <= 3000 && Bot.Hand.Count >= 1) return true;

            // Varudras is Rank 10 Priority #1 (Omni-Negate): It MUST be summoned first!
            if (!Bot.HasInMonstersZone(CardId.Varudras)) return false;

            // If Varudras is already on field, summon Gustav Max if we have 2 more eligible Level 10s
            int eligibleLv10s = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10 && m.Id != CardId.TheChaoticPhantasmalSacredBeasts && m.Id != CardId.Varudras);
            return eligibleLv10s >= 2;
        }

        private bool ShouldGustavMaxActivate()
        {
            _gustavBurnUsed++;
            return true;
        }

        private bool ShouldGustavRocketSpSummon()
        {
            // Overlay on Gustav Max with 0 materials (requires 1 discard from hand)
            if (Bot.Hand.Count < 1) return false;
            var emptyGustav = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.GustavMax && m.Overlays.Count == 0);
            return emptyGustav != null;
        }

        private bool ShouldArmityleSpSummon()
        {
            if (Duel.Turn == 1) return false;
            var sbs = Bot.GetMonsters().Where(m => m.IsFaceup() && (m.Id == CardId.CalamityHamon || m.Id == CardId.InfinityRaviel || m.Id == CardId.InfernoUria)).ToList();
            return sbs.Count >= 3;
        }

        private bool ShouldLiebeSpSummon()
        {
            // Overlay on Rank 10 during Battle Phase or Going 2nd for 6000 ATK OTK
            if (Duel.Turn == 1) return false;
            // Never overlay on Varudras (Varudras is our omni-negate!)
            var target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() &&
                (m.Id == CardId.GustavRocket ||
                 (m.Id == CardId.GustavMax && m.Overlays.Count == 0) ||
                 m.Id == CardId.SuperDora ||
                 m.Id == CardId.FlyingLauncher));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ShouldSuperDoraSpSummon()
        {
            int eligibleLv10s = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10 && m.Id != CardId.TheChaoticPhantasmalSacredBeasts && m.Id != CardId.Varudras);
            return eligibleLv10s >= 2;
        }

        private bool ShouldFlyingLauncherSpSummon()
        {
            int eligibleLv10s = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 10 && m.Id != CardId.TheChaoticPhantasmalSacredBeasts && m.Id != CardId.Varudras);
            return eligibleLv10s >= 2 && (Bot.HasInHand(CardId.CannonSoldierMK2) || Bot.GetMonsterCount() <= 3);
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
        #endregion

        #region Helpers
        private static bool IsSacredBeastMonster(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.CalamityHamon ||
                   card.Id == CardId.InfinityRaviel ||
                   card.Id == CardId.InfernoUria ||
                   card.Id == CardId.SummonerOfTheSacredBeasts ||
                   card.Id == CardId.MartyrOfTheSacredBeasts;
        }

        private static bool IsSacredBeastLv10(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.CalamityHamon ||
                   card.Id == CardId.InfinityRaviel ||
                   card.Id == CardId.InfernoUria ||
                   card.Id == CardId.TheChaoticPhantasmalSacredBeasts;
        }

        private static bool IsNonNormalSummonableLv10(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.CalamityHamon ||
                   card.Id == CardId.InfinityRaviel ||
                   card.Id == CardId.InfernoUria ||
                   card.Id == CardId.MachinaRuinforce ||
                   card.Id == CardId.YomagnaTheFirePhantom;
        }

        private static bool IsExpendableMonster(ClientCard card)
        {
            if (card == null) return false;
            if (AceCardIds.Contains(card.Id)) return false;
            if (card.HasType(CardType.Token)) return true;
            if (card.Id == CardId.MartyrOfTheSacredBeasts) return true;
            if (card.Id == CardId.CrystronRosenix) return true;
            if (card.Id == CardId.GrayLayer) return true;
            if (card.Id == CardId.MachinaUnclaspare) return true;
            if (card.Id == CardId.SummonerOfTheSacredBeasts) return true;
            if (card.Attack <= 1800 && card.Level <= 4) return true;
            return false;
        }
        #endregion

        #region Selection Handlers (OnSelectCard, OnSelectPosition, OnSelectOption)
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 500: Tribute
            if (hint == 500)
            {
                var list = new List<ClientCard>();

                // Prioritize Ruinforce (triggers float into 3 Machinas!)
                var ruinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ruinforce != null) list.Add(ruinforce);

                // Tokens & Martyrs
                var tokens = cards.Where(c => c.HasType(CardType.Token) || c.Id == CardId.MartyrOfTheSacredBeasts);
                foreach (var t in tokens)
                {
                    if (!list.Contains(t)) list.Add(t);
                }

                // Low ATK expendables (non-Aces)
                foreach (var c in cards.Where(c => !AceCardIds.Contains(c.Id)).OrderBy(c => c.Attack))
                {
                    if (!list.Contains(c)) list.Add(c);
                    if (list.Count >= max) break;
                }

                // Fallback: MUST NEVER return less than min! Fill from available cards ordered by lowest ATK
                if (list.Count < min)
                {
                    foreach (var c in cards.OrderBy(c => c.Attack))
                    {
                        if (!list.Contains(c)) list.Add(c);
                        if (list.Count >= max) break;
                    }
                }

                return list.Take(max).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                var gyPreferred = cards.OrderByDescending(c =>
                    c.Id == CardId.MachinaRuinforce ? 100 :
                    c.Id == CardId.MachinaFortress ? 95 :
                    c.Id == CardId.CrystronRosenix ? 90 :
                    c.Id == CardId.SummonerOfTheSacredBeasts ? 85 :
                    c.Id == CardId.GrayLayer ? 80 :
                    c.Id == CardId.NoctilucentTrainBleuTraveler ? 75 :
                    c.Id == CardId.HeavyFreightTrainDerricrane ? 70 :
                    c.Id == CardId.InfernoUria ? 65 : 10
                ).ToList();
                return gyPreferred.Take(max).ToList();
            }

            // Hint 502: Destroy
            if (hint == 502)
            {
                // Target enemy problematic cards first
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var result = new List<ClientCard>();
                    var eternalSoul = enemyCards.FirstOrDefault(c => c.Id == 48680970);
                    if (eternalSoul != null) result.Add(eternalSoul);

                    var problematic = enemyCards.FirstOrDefault(c => Util.GetProblematicEnemyCard() == c || c.IsSpell() || c.IsTrap());
                    if (problematic != null && !result.Contains(problematic)) result.Add(problematic);

                    foreach (var c in enemyCards.OrderByDescending(c => c.Attack))
                    {
                        if (!result.Contains(c)) result.Add(c);
                        if (result.Count >= max) break;
                    }
                    if (result.Count >= min) return result.Take(max).ToList();
                }

                // Destroying our own card (e.g. Metal Slave cost or Boss on Parade cost):
                var ourCards = cards.Where(c => c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var result = new List<ClientCard>();

                    // If Blaster Cannon Core is an option alongside Metal Slave, ALWAYS sacrifice Blaster Cannon Core!
                    var blaster = ourCards.FirstOrDefault(c => c.Id == CardId.BESBlasterCannonCore);
                    if (blaster != null) result.Add(blaster);

                    // Next, expendable fodder
                    foreach (var c in ourCards.Where(c => IsExpendableMonster(c)).OrderBy(c => c.Attack))
                    {
                        if (!result.Contains(c)) result.Add(c);
                        if (result.Count >= max) break;
                    }

                    // Strictly avoid Ace cards unless forced by min
                    if (result.Count < min)
                    {
                        foreach (var c in ourCards.Where(c => !AceCardIds.Contains(c.Id)).OrderBy(c => c.Attack))
                        {
                            if (!result.Contains(c)) result.Add(c);
                            if (result.Count >= max) break;
                        }
                    }

                    // Absolute fallback: fill to min
                    if (result.Count < min)
                    {
                        foreach (var c in ourCards.OrderBy(c => c.Attack))
                        {
                            if (!result.Contains(c)) result.Add(c);
                            if (result.Count >= max) break;
                        }
                    }
                    return result.Take(max).ToList();
                }
            }

            // Hint 504: Banish / Send
            if (hint == 504)
            {
                // CASE 1: Machina Ruinforce SS (ONLY banish Machines from Graveyard!)
                bool allGraveyard = cards.All(c => c.Location == CardLocation.Grave);
                if (allGraveyard)
                {
                    var gyMachines = cards.Where(c => c.HasRace(CardRace.Machine)).OrderByDescending(c => c.Level).ToList();
                    if (gyMachines.Count >= min)
                    {
                        return gyMachines.Take(max).ToList();
                    }
                }

                // CASE 2: Fallen Paradise send 3 cards (Hand/Field)
                // If Spells available: Pick Thunderclap on field FIRST, then hand spells!
                var faceupThunderclaps = cards.Where(c => c.Id == CardId.SacredBeastsThunderclap && c.Location == CardLocation.SpellZone).ToList();
                if (faceupThunderclaps.Count > 0)
                {
                    var spellList = new List<ClientCard>(faceupThunderclaps);
                    foreach (var s in cards.Where(c => c.IsSpell() && !spellList.Contains(c)))
                    {
                        spellList.Add(s);
                    }
                    if (spellList.Count >= min)
                    {
                        return spellList.Take(max).ToList();
                    }
                }

                // If Monsters: STRICTLY EXCLUDE Ace cards! Only expendable monsters!
                var expendableMons = cards.Where(c => IsExpendableMonster(c)).OrderBy(c => c.Attack).ToList();
                if (expendableMons.Count >= min)
                {
                    return expendableMons.Take(max).ToList();
                }

                // Fallback: Never pick Ace cards if possible
                var nonAces = cards.Where(c => !AceCardIds.Contains(c.Id)).ToList();
                if (nonAces.Count >= min)
                {
                    return nonAces.Take(max).ToList();
                }

                // Absolute fallback: must satisfy min!
                return cards.OrderBy(c => AceCardIds.Contains(c.Id) ? 100 : 0).Take(max).ToList();
            }

            // Hint 505: Search / Add to Hand
            if (hint == 505)
            {
                // Machina searches: Fortress -> Ruinforce -> Derricrane
                var fortress = cards.FirstOrDefault(c => c.Id == CardId.MachinaFortress);
                if (fortress != null && !Bot.HasInHand(CardId.MachinaFortress)) return new List<ClientCard> { fortress };

                var ruinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ruinforce != null && !(Bot.HasInHand(CardId.MachinaRuinforce) || Bot.HasInGraveyard(CardId.MachinaRuinforce))) return new List<ClientCard> { ruinforce };

                var derricrane = cards.FirstOrDefault(c => c.Id == CardId.HeavyFreightTrainDerricrane);
                if (derricrane != null && !Bot.HasInHand(CardId.HeavyFreightTrainDerricrane)) return new List<ClientCard> { derricrane };

                // Sacred Beasts searches: Hamon -> Raviel -> Combined Assault -> Thunderclap
                var hamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (hamon != null && !Bot.HasInHand(CardId.CalamityHamon)) return new List<ClientCard> { hamon };

                var raviel = cards.FirstOrDefault(c => c.Id == CardId.InfinityRaviel);
                if (raviel != null && !Bot.HasInHand(CardId.InfinityRaviel)) return new List<ClientCard> { raviel };

                var assault = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsCombinedAssault);
                if (assault != null) return new List<ClientCard> { assault };

                var thunderclap = cards.FirstOrDefault(c => c.Id == CardId.SacredBeastsThunderclap);
                if (thunderclap != null) return new List<ClientCard> { thunderclap };
            }

            // Hint 506: Select from Deck (Boss on Parade / Martyr / Released)
            if (hint == 506)
            {
                // Boss on Parade SS from Deck: Gray Layer!
                var grayLayer = cards.FirstOrDefault(c => c.Id == CardId.GrayLayer);
                if (grayLayer != null) return new List<ClientCard> { grayLayer };

                // Sacred Beasts Released: Add 3 different SBs
                var sbSelection = new List<ClientCard>();
                var hamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (hamon != null) sbSelection.Add(hamon);
                var raviel = cards.FirstOrDefault(c => c.Id == CardId.InfinityRaviel);
                if (raviel != null && !sbSelection.Contains(raviel)) sbSelection.Add(raviel);
                var uria = cards.FirstOrDefault(c => c.Id == CardId.InfernoUria);
                if (uria != null && !sbSelection.Contains(uria)) sbSelection.Add(uria);

                if (sbSelection.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (!sbSelection.Contains(c)) sbSelection.Add(c);
                        if (sbSelection.Count >= max) break;
                    }
                }

                if (sbSelection.Count >= min) return sbSelection.Take(max).ToList();
            }

            // Hint 508: Send to Graveyard
            if (hint == 508)
            {
                var dumpRuinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (dumpRuinforce != null && !Bot.HasInGraveyard(CardId.MachinaRuinforce)) return new List<ClientCard> { dumpRuinforce };

                var dumpFortress = cards.FirstOrDefault(c => c.Id == CardId.MachinaFortress);
                if (dumpFortress != null && !Bot.HasInGraveyard(CardId.MachinaFortress)) return new List<ClientCard> { dumpFortress };

                var dumpRosenix = cards.FirstOrDefault(c => c.Id == CardId.CrystronRosenix);
                if (dumpRosenix != null) return new List<ClientCard> { dumpRosenix };
            }

            // Hint 509: Special Summon / Extra Deck Selection
            if (hint == 509)
            {
                // Extra Deck Fusion Summon via Combined Assault: The Chaotic Phantasmal Sacred Beasts!
                var chaoticExtra = cards.FirstOrDefault(c => c.Id == CardId.TheChaoticPhantasmalSacredBeasts && c.Location == CardLocation.Extra);
                if (chaoticExtra != null) return new List<ClientCard> { chaoticExtra };

                var armityleExtra = cards.FirstOrDefault(c => c.Id == CardId.ArmityleTheChaosPhantasm && c.Location == CardLocation.Extra);
                if (armityleExtra != null) return new List<ClientCard> { armityleExtra };

                // Fallen Paradise / Ruinforce float / Bleu Traveler
                var ssChaotic = cards.FirstOrDefault(c => c.Id == CardId.TheChaoticPhantasmalSacredBeasts);
                if (ssChaotic != null) return new List<ClientCard> { ssChaotic };

                var ssRuinforce = cards.FirstOrDefault(c => c.Id == CardId.MachinaRuinforce);
                if (ssRuinforce != null) return new List<ClientCard> { ssRuinforce };

                var ssRaviel = cards.FirstOrDefault(c => c.Id == CardId.InfinityRaviel);
                if (ssRaviel != null) return new List<ClientCard> { ssRaviel };

                var ssHamon = cards.FirstOrDefault(c => c.Id == CardId.CalamityHamon);
                if (ssHamon != null) return new List<ClientCard> { ssHamon };

                var ssFortress = cards.FirstOrDefault(c => c.Id == CardId.MachinaFortress);
                if (ssFortress != null) return new List<ClientCard> { ssFortress };
            }

            // Hint 513: Xyz Material Selection
            if (hint == 513)
            {
                // Do NOT use The Chaotic Phantasmal Sacred Beasts or Varudras as material!
                var safeMaterials = cards.Where(c => c.Id != CardId.TheChaoticPhantasmalSacredBeasts && c.Id != CardId.Varudras).ToList();
                if (safeMaterials.Count >= min)
                {
                    return safeMaterials.Take(max).ToList();
                }
                return cards.Take(max).ToList();
            }

            // Hint 549: Attack Target Selection — NEVER intercept! Filter out suicide targets
            if (hint == 549)
            {
                // Filter out suicide targets like Crystal Wing Synchro Dragon (50954680)
                var safeTargets = cards.Where(c => c.Id != 50954680 && c.Id != 21887175).ToList();
                if (safeTargets.Count >= min)
                {
                    return base.OnSelectCard(safeTargets, min, max, hint, cancelable);
                }
                return base.OnSelectCard(cards, min, max, hint, cancelable);
            }

            // Hint 575 or any Card Effect Target Selection: Prioritize Dangerous Opponent Cards
            var oppCards = cards.Where(c => c.Controller == 1).ToList();
            if (oppCards.Count > 0)
            {
                var result = new List<ClientCard>();

                // If targeting Spells/Traps:
                var oppST = oppCards.Where(c => c.IsSpell() || c.IsTrap()).ToList();
                if (oppST.Count > 0)
                {
                    var eternalSoul = oppST.FirstOrDefault(c => c.Id == 48680970);
                    if (eternalSoul != null) result.Add(eternalSoul);

                    var bestST = oppST.FirstOrDefault(c => Util.GetProblematicEnemyCard() == c || c.IsFaceup());
                    if (bestST != null && !result.Contains(bestST)) result.Add(bestST);

                    foreach (var s in oppST)
                    {
                        if (!result.Contains(s)) result.Add(s);
                        if (result.Count >= max) break;
                    }
                    if (result.Count >= min) return result.Take(max).ToList();
                }

                // If targeting Monsters:
                var oppMonsters = oppCards.Where(c => c.Location == CardLocation.MonsterZone && c.IsFaceup()).ToList();
                if (oppMonsters.Count > 0)
                {
                    var dangerousEffect = oppMonsters
                        .Where(m => !m.IsDisabled() && m.HasType(CardType.Effect))
                        .OrderByDescending(m => m.Attack)
                        .FirstOrDefault();

                    if (dangerousEffect != null) result.Add(dangerousEffect);

                    foreach (var m in oppMonsters.OrderByDescending(m => m.Attack))
                    {
                        if (!result.Contains(m)) result.Add(m);
                        if (result.Count >= max) break;
                    }
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.HeavyKnightBabelDecker ||
                cardId == CardId.MartyrOfTheSacredBeasts ||
                cardId == CardId.SummonerOfTheSacredBeasts ||
                cardId == CardId.CrystronRosenix ||
                cardId == CardId.InfernoUria)
            {
                return CardPosition.FaceUpDefence;
            }

            if (cardId == CardId.MachinaRuinforce ||
                cardId == CardId.MachinaFortress ||
                cardId == CardId.GustavMax ||
                cardId == CardId.GustavRocket ||
                cardId == CardId.JuggernautLiebe ||
                cardId == CardId.TheChaoticPhantasmalSacredBeasts ||
                cardId == CardId.ArmityleTheChaosPhantasm ||
                cardId == CardId.CalamityHamon ||
                cardId == CardId.InfinityRaviel ||
                cardId == CardId.Varudras ||
                cardId == CardId.SuperDora)
            {
                return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Option selection for Pegasus level matching: Option 0
            return 0;
        }
        #endregion
    }
}
