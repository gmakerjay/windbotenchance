// =========================================================================================
// CARD AUDIT — 2026_Regenesis
// | Card Name                               | Type    | OPT? | Cost | Effect Summary                                   |
// | :-------------------------------------- | :-----: | :--: | :--: | :----------------------------------------------- |
// | Regenesis Archfiend (95718355)          | Monster | Yes  | None | Main searcher, Synchro/Extender.                 |
// | Regenesis Sage (22938501)               | Monster | Yes  | None | Special Summons and searches Regenesis.          |
// | Regenesis Warrior (96540807)            | Monster | Yes  | None | Quick Effect bounce, self-summon, self-salvage.  |
// | Regenesis Dragon (59323650)             | Monster | Yes  | None | Special Summons on search, dumps Regenesis.       |
// | Regenesis Lord (22812963)               | Monster | Yes  | Banish| Boss monster, sets Regenesis backrow, pierces.   |
// | Fidraulis Harmonia (70088809)           | Monster | Yes  | None | Synchro hand trap: summon, dump, destroy.       |
// | Fallen of the White Dragon (73819701)   | Monster | Yes  | Extra| Treated as Albaz, summons Ecclesia.              |
// | Dogmatika Ecclesia (60303688)           | Monster | Yes  | None | Searcher for Dogmatika cards.                   |
// | Dogmatika Fleurdelis (69680031/73355772)| Monster | Yes  | None | Self-summons, negates monster effect on field.   |
// | Highness Archfiend (11248645)           | Monster | Yes  | None | Custom extender: summons if Fiend, searches.     |
// | Duke Archfiend (85154941)               | Monster | Yes  | None | Custom extender: summons if Archfiend, ATK buff. |
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Regenesis", "2026_Regenesis")]
    public class _2026_RegenesisExecutor : ModernExecutor
    {
        public class CardId
        {
            // Regenesis Archetype
            public const int RegenesisArchfiend = 95718355;
            public const int RegenesisSage = 22938501;
            public const int RegenesisWarrior = 96540807;
            public const int RegenesisDragon = 59323650;
            public const int RegenesisLord = 22812963;
            public const int Regenesis = 31786838;
            public const int RegenesisCode = 67171933;
            public const int RegenesisBirth = 27781371;

            // Dogmatika & White Dragon
            public const int DogmatikaEcclesiaTheVirtuous = 60303688;
            public const int DogmatikaFleurdelisTheKnighted = 69680031;
            public const int DogmatikaFleurdelisTheThunderbolt = 73355772;
            public const int FallenOfTheWhiteDragon = 73819701;
            public const int NadirServant = 1984618;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int DogmatikaPunishment = 82956214;

            // Bystials & Hand Traps
            public const int BystialMagnamhut = 33854624;
            public const int BystialDruiswurm = 6637331;
            public const int LavaGolem = 102380;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int NibiruThePrimalBeing = 27204311;
            public const int FidraulisHarmonia = 70088809;

            // Extenders (Custom Archfiend Synergy)
            public const int HighnessArchfiend = 11248645;
            public const int DukeArchfiend = 85154941;

            // Extra Deck
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int TitanikladTheAshDragon = 41373230;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int RindbrummTheStrikingDragon = 51409648;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int ElderEntityNtss = 80532587;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int DeepSeaPrimaDonna = 50793215;
            public const int DespianLuluwalilith = 53971455;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int TriBrigadeArmsBucephalusII = 10019086;
            public const int AlbaLenatusTheAbyssDragon = 3410461;

            // Side Deck Spells & Traps
            public const int TripleTacticsThrust = 35269904;
            public const int LightningStorm = 14532163;
            public const int DimensionalBarrier = 83326048;
            public const int DragonsMind = 85442146;
        }

        private static readonly int[] AceCardIds = {
            CardId.RegenesisLord,
            CardId.DespianLuluwalilith,
            CardId.EcclesiaAndTheDarkDragon,
            CardId.TheDragonThatDevoursTheDogma,
            CardId.TitanikladTheAshDragon,
            CardId.AlbaLenatusTheAbyssDragon
        };

        private bool _harmoniaUsed = false;
        private bool _normalSummonedThisTurn = false;
        private bool _nadirUsed = false;
        private bool _ecclesiaUsed = false;
        private bool _fallenVirtuousUsed = false;
        private bool _regenesisUsed = false;
        private bool _regenesisCodeUsed = false;
        private bool _regenesisBirthUsed = false;
        private bool _regenesisArchfiendUsed = false;
        private bool _regenesisSageUsed = false;
        private bool _regenesisDragonUsed = false;

        public _2026_RegenesisExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register primary boss monsters for priority AI protection
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // ── Hand Traps ──
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Regenesis-Archfiend-Start",
                RequiredCards = new List<int> { CardId.RegenesisArchfiend },
                FallbackLineName = "Regenesis-Sage-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RegenesisArchfiend, ActionType = ExecutorType.Activate, Description = "Search Regenesis spell" },
                },
                EndBoardScore = 60
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Regenesis-Sage-Fallback",
                RequiredCards = new List<int> { CardId.RegenesisSage },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RegenesisSage, ActionType = ExecutorType.Summon, Description = "Summon Regenesis Sage" },
                    new() { CardId = CardId.RegenesisSage, ActionType = ExecutorType.Activate, Description = "Sage search/summon extender" }
                },
                EndBoardScore = 45,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.RegenesisSage)
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.RegenesisArchfiend, CardId.NadirServant);
            BaitPlanner.RegisterBaitCards(CardId.HighnessArchfiend, CardId.DukeArchfiend);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.RegenesisArchfiend, CardId.NadirServant);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Disruptions
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, FidraulisHarmoniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruEffect);

            // ============================================================
            // TIER 2: Extenders & Free Special Summons
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialMagnamhutSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDruiswurm, BystialDruiswurmSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialDruiswurmEffect);
            
            AddExecutor(ExecutorType.Activate, CardId.HighnessArchfiend, HighnessArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.DukeArchfiend, DukeArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisDragon, RegenesisDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisWarrior, RegenesisWarriorEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisSage, RegenesisSageEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisArchfiend, RegenesisArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonEffect);

            // ============================================================
            // TIER 3: Dogmatika Core / Search Spells
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.NadirServant, NadirServantEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaEcclesiaTheVirtuous, DogmatikaEcclesiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaFleurdelisTheKnighted, DogmatikaFleurdelisEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaFleurdelisTheThunderbolt, DogmatikaFleurdelisEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // ============================================================
            // TIER 4: Archetype Spells & Traps
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Regenesis, RegenesisEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisCode, RegenesisCodeEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisBirth, RegenesisBirthEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaPunishment, PunishmentEffect);

            // ============================================================
            // TIER 5: Generic Board Breakers & Side Cards
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);
            AddExecutor(ExecutorType.Activate, CardId.DragonsMind, DragonsMindEffect);

            // ============================================================
            // TIER 6: Extra Deck Activations
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DespianLuluwalilith, DespianLuluwalilithEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, TheDragonThatDevoursTheDogmaEffect);

            // ============================================================
            // TIER 7: Special Summons (Extra Deck & Ritual/Tribute Bosses)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon, ExtraDeckSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.DespianLuluwalilith, ExtraDeckSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong, ExtraDeckSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.DeepSeaPrimaDonna, ExtraDeckSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.RegenesisLord, RegenesisLordSpSummon);

            // ============================================================
            // TIER 8: Normal Summons
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.HighnessArchfiend, HighnessArchfiendSummon);
            AddExecutor(ExecutorType.Summon, CardId.DukeArchfiend, DukeArchfiendSummon);
            AddExecutor(ExecutorType.Summon, CardId.DogmatikaEcclesiaTheVirtuous, DogmatikaEcclesiaSummon);
            AddExecutor(ExecutorType.Summon, CardId.RegenesisSage, RegenesisSageSummon);

            // ============================================================
            // TIER 9: Set Backrow & Defense
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.DogmatikaPunishment);
            AddExecutor(ExecutorType.SpellSet, CardId.RegenesisBirth);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _harmoniaUsed = false;
            _normalSummonedThisTurn = false;
            _nadirUsed = false;
            _ecclesiaUsed = false;
            _fallenVirtuousUsed = false;
            _regenesisUsed = false;
            _regenesisCodeUsed = false;
            _regenesisBirthUsed = false;
            _regenesisArchfiendUsed = false;
            _regenesisSageUsed = false;
            _regenesisDragonUsed = false;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        // ============================================================
        // ACTIVATION LOGIC & EFFECT HANDLERS
        // ============================================================

        private bool FidraulisHarmoniaEffect()
        {
            if (_harmoniaUsed) return false;
            _harmoniaUsed = true;

            // Step 1: Select which Extra Deck Synchro monster to summon
            // Prioritize removal (Malong) when opponent has threats, otherwise setup (Lulu)
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                AI.SelectCard(new[] {
                    CardId.GoldenCloudBeastMalong,
                    CardId.DespianLuluwalilith,
                    CardId.EcclesiaAndTheDarkDragon,
                    CardId.DeepSeaPrimaDonna
                });
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.DespianLuluwalilith,
                    CardId.GoldenCloudBeastMalong,
                    CardId.EcclesiaAndTheDarkDragon,
                    CardId.DeepSeaPrimaDonna
                });
            }

            // Step 2: Select dump target from deck to GY
            AI.SelectNextCard(new[] {
                CardId.RegenesisArchfiend,
                CardId.RegenesisDragon,
                CardId.RegenesisSage
            });

            // Step 3: If destroy effect applies, target the strongest enemy monster
            if (Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectThirdCard(target);
                }
            }

            return true;
        }

        private bool NibiruEffect()
        {
            if (!SmartHandTrapChain()) return false;
            int oppMonsters = Enemy.GetMonsterCount();
            return oppMonsters >= 2 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2000);
        }

        private ClientCard GetBystialTarget()
        {
            ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            if (target != null) return target;

            target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark))
                && !c.IsCode(CardId.RegenesisArchfiend, CardId.RegenesisLord, CardId.FallenOfTheWhiteDragon));
            if (target != null) return target;

            return Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
        }

        private bool BystialMagnamhutSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard target = GetBystialTarget();
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool BystialMagnamhutEffect()
        {
            // Search a Dragon from deck when sent to GY
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                AI.SelectCard(new[] {
                    CardId.RegenesisDragon,
                    CardId.RegenesisArchfiend,
                    CardId.BystialDruiswurm
                });
            }
            return true;
        }

        private bool BystialDruiswurmSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard target = GetBystialTarget();
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool BystialDruiswurmEffect()
        {
            // Send opponent's SS monster to GY on summon
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                // No valid target to send — don't activate the effect
                return false;
            }
            return true;
        }

        private bool HighnessArchfiendEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend));
                if (!hasFiend) return false;
            }

            if (Bot.GetRemainingCount(CardId.DukeArchfiend, 1) > 0)
            {
                AI.SelectCard(CardId.DukeArchfiend);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisArchfiend);
            }
            return true;
        }

        private bool DukeArchfiendEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasArchfiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x45));
                if (!hasArchfiend) return false;
            }
            return true;
        }

        private bool RegenesisArchfiendEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_regenesisArchfiendUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                bool canReveal = Bot.Hand.Any(c => c != null && c != Card && (c.Attack == 2500 && c.Defense == 2500));
                if (!canReveal) return false;
            }

            _regenesisArchfiendUsed = true;
            if (Bot.GetRemainingCount(CardId.Regenesis, 1) > 0)
            {
                AI.SelectCard(CardId.Regenesis);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisCode, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisCode);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisSage, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisSage);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisBirth, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisBirth);
            }
            return true;
        }

        private bool RegenesisSageEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_regenesisSageUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                bool hasRegenesis = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RegenesisArchfiend, CardId.RegenesisWarrior, CardId.RegenesisDragon, CardId.RegenesisLord));
                if (!hasRegenesis) return false;
            }

            _regenesisSageUsed = true;
            if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisArchfiend);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisWarrior, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisWarrior);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisDragon, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisDragon);
            }
            return true;
        }

        private bool RegenesisWarriorEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool has2500 = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(c => c != null && c.IsFaceup() && (c.BaseAttack == 2500 && c.BaseDefense == 2500));
                if (!has2500) return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool RegenesisDragonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_regenesisDragonUsed) return false;
            _regenesisDragonUsed = true;
            if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisArchfiend);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisSage, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisSage);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisWarrior, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisWarrior);
            }
            return true;
        }

        private bool RegenesisLordSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.BaseAttack == 2500 && c.BaseDefense == 2500) && !IsAceCard(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FallenOfTheWhiteDragonEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                var extraTarget = Bot.ExtraDeck.FirstOrDefault(c => c != null && c.IsCode(CardId.TitanikladTheAshDragon, CardId.AlbionTheBrandedDragon, CardId.RindbrummTheStrikingDragon));
                if (extraTarget != null)
                {
                    AI.SelectCard(extraTarget);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool NadirServantEffect()
        {
            if (_nadirUsed) return false;

            bool hasEcclesia = Bot.GetRemainingCount(CardId.DogmatikaEcclesiaTheVirtuous, 1) > 0;
            bool hasFleurdelis = Bot.GetRemainingCount(CardId.DogmatikaFleurdelisTheThunderbolt, 1) > 0;

            ClientCard extraSend = null;
            int addCardId = 0;

            if (hasEcclesia)
            {
                extraSend = Bot.ExtraDeck.FirstOrDefault(c => c != null && c.IsCode(CardId.GaruraWingsOfResonantLife, CardId.TitanikladTheAshDragon, CardId.ElderEntityNtss));
                addCardId = CardId.DogmatikaEcclesiaTheVirtuous;
            }
            else if (hasFleurdelis)
            {
                extraSend = Bot.ExtraDeck.FirstOrDefault(c => c != null && c.IsCode(CardId.TitanikladTheAshDragon, CardId.ElderEntityNtss, CardId.DespianLuluwalilith));
                addCardId = CardId.DogmatikaFleurdelisTheThunderbolt;
            }

            if (extraSend != null && addCardId != 0)
            {
                _nadirUsed = true;
                AI.SelectCard(extraSend);
                AI.SelectNextCard(addCardId);
                return true;
            }
            return false;
        }

        private bool DogmatikaEcclesiaEffect()
        {
            if (_ecclesiaUsed) return false;
            _ecclesiaUsed = true;
            if (Bot.GetRemainingCount(CardId.DogmatikaFleurdelisTheThunderbolt, 1) > 0)
            {
                AI.SelectCard(CardId.DogmatikaFleurdelisTheThunderbolt);
            }
            else if (Bot.GetRemainingCount(CardId.DogmatikaPunishment, 1) > 0)
            {
                AI.SelectCard(CardId.DogmatikaPunishment);
            }
            else
            {
                return false; // Nothing useful to search
            }
            return true;
        }

        private bool DogmatikaFleurdelisEffect()
        {
            // Negate an opponent's face-up monster effect
            var target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // No valid target — don't waste activation
            return false;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (_fallenVirtuousUsed) return false;
            // Fusion Summon using materials on field/GY
            if (IsSpecialSummonBlocked()) return false;
            // Check we have materials available
            bool hasMaterials = Bot.GetMonsters().Count(c => c != null && c.IsFaceup()) >= 1 ||
                Bot.Graveyard.Any(c => c != null && c.IsMonster());
            if (!hasMaterials) return false;
            _fallenVirtuousUsed = true;
            return true;
        }

        private bool RegenesisEffect()
        {
            if (_regenesisUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                _regenesisUsed = true;
                if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
                {
                    AI.SelectCard(CardId.RegenesisArchfiend);
                }
                else if (Bot.GetRemainingCount(CardId.RegenesisDragon, 1) > 0)
                {
                    AI.SelectCard(CardId.RegenesisDragon);
                }
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.RegenesisArchfiend, CardId.RegenesisLord) && c.IsCanRevive());
                if (target != null)
                {
                    _regenesisUsed = true;
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool RegenesisCodeEffect()
        {
            if (_regenesisCodeUsed) return false;
            _regenesisCodeUsed = true;
            if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisArchfiend);
            }
            else if (Bot.GetRemainingCount(CardId.RegenesisSage, 1) > 0)
            {
                AI.SelectCard(CardId.RegenesisSage);
            }
            return true;
        }

        private bool RegenesisBirthEffect()
        {
            if (_regenesisBirthUsed) return false;
            if (Card.Location == CardLocation.SpellZone)
            {
                // Best timing: chain to opponent's effect or on opponent's turn
                bool isReactive = Duel.LastChainPlayer == 1 || Duel.Player == 1;
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null && isReactive)
                {
                    _regenesisBirthUsed = true;
                    AI.SelectCard(target);
                    if (Bot.GetRemainingCount(CardId.RegenesisDragon, 1) > 0)
                    {
                        AI.SelectNextCard(CardId.RegenesisDragon);
                    }
                    else if (Bot.GetRemainingCount(CardId.RegenesisArchfiend, 1) > 0)
                    {
                        AI.SelectNextCard(CardId.RegenesisArchfiend);
                    }
                    return true;
                }
                // On our turn, only use if target exists and is truly threatening
                if (target != null && Duel.Player == 0 && target.Attack >= 2500)
                {
                    _regenesisBirthUsed = true;
                    AI.SelectCard(target);
                    if (Bot.GetRemainingCount(CardId.RegenesisDragon, 1) > 0)
                    {
                        AI.SelectNextCard(CardId.RegenesisDragon);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool DragonsMindEffect()
        {
            // Only activate if we have a Dragon on field or genuinely need board presence
            bool hasDragon = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Dragon));
            if (hasDragon) return true;
            // Without a Dragon, only activate if board is completely empty and desperate
            if (Bot.GetMonsterCount() == 0 && NeedsBoardPresence()) return true;
            return false;
        }

        private bool IsValuableMonster(ClientCard card)
        {
            if (card == null) return false;
            if (AceCardIds.Contains(card.Id)) return true;
            if (card.IsCode(CardId.RegenesisArchfiend, CardId.RegenesisSage, CardId.RegenesisWarrior, CardId.RegenesisDragon, CardId.GoldenCloudBeastMalong)) return false;
            if (card.IsFaceup() && (card.Attack >= 2500 || card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro))) return true;
            return false;
        }

        private bool CanSummonWithoutValuableMaterials(int targetCardId)
        {
            var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var availableMaterials = ownMonsters.Where(c => !IsValuableMonster(c)).ToList();

            // Most Extra Deck summons in Regenesis are Synchros or Fusions requiring at least 2 materials
            int requiredCount = 2;

            return availableMaterials.Count >= requiredCount;
        }

        private bool ExtraDeckSpSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            if (Card != null)
            {
                var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
                var valuableMaterials = ownMonsters.Where(c => IsValuableMonster(c)).ToList();
                
                if (ownMonsters.Count < 2) return false;

                var nonValuableMaterials = ownMonsters.Where(c => !IsValuableMonster(c)).ToList();
                if (nonValuableMaterials.Count < 2)
                {
                    // We would be forced to use at least one valuable/Ace monster as material
                    bool allowed = false;
                    string reason = "";
                    foreach (var mat in valuableMaterials)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            allowed = true;
                            reason = res.reason;
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        return false; // Block the summon to conserve resources
                    }
                    else
                    {
                        try
                        {
                            AI?.Log(LogLevel.Info, $"[ACE-ALLOW] Allowing Extra Deck summon of {Card.Id} by sacrificing valuable material: {reason}");
                        }
                        catch { }
                    }
                }
            }
            return true;
        }

        private bool EcclesiaAndTheDarkDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Deck.Concat(Bot.Graveyard).FirstOrDefault(c => c != null && c.IsCode(CardId.FallenOfTheWhiteDragon));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        private bool DespianLuluwalilithEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return true;
            // Only activate negate/removal on opponent's turn or when threatened
            if (Duel.Player == 1)
            {
                return Enemy.GetMonsterCount() > 0 || Duel.LastChainPlayer == 1;
            }
            // On our turn, only if opponent has threatening monster
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500);
        }

        private bool GoldenCloudBeastMalongEffect()
        {
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells()).FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TheDragonThatDevoursTheDogmaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return true;
            // Only activate effect if there's a meaningful target
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return true;
        }

        private bool PunishmentEffect()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget()).ToList();
            if (targets.Count == 0) return false;

            var target = targets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).FirstOrDefault();
            if (target == null) return false;

            var extraSend = Bot.ExtraDeck
                .Where(c => c != null && YGOSharp.OCGWrapper.NamedCard.Get(c.Id) != null && YGOSharp.OCGWrapper.NamedCard.Get(c.Id).Attack >= target.Attack)
                .OrderBy(c => {
                    if (c.Id == CardId.ElderEntityNtss) return 0;
                    if (c.Id == CardId.GaruraWingsOfResonantLife) return 1;
                    if (c.Id == CardId.TitanikladTheAshDragon) return 2;
                    return 3;
                })
                .ThenBy(c => YGOSharp.OCGWrapper.NamedCard.Get(c.Id).Attack)
                .FirstOrDefault();

            if (extraSend == null) return false;

            AI.SelectCard(target);
            AI.SelectNextCard(extraSend);
            return true;
        }

        private bool LavaGolemSummon()
        {
            if (_normalSummonedThisTurn) return false;
            if (Enemy.GetMonsterCount() < 2) return false;

            // Only use Lava Golem if opponent has threatening monsters worth removing
            int totalEnemyATK = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .Sum(c => c.Attack);
            bool hasThreatMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.Attack >= 2500 || (!c.IsDisabled() && c.HasType(CardType.Effect))));

            // Don't sacrifice NS just to give opponent a 3000 ATK beater
            // unless their board is genuinely threatening
            if (totalEnemyATK < 3000 && !hasThreatMonster) return false;

            _normalSummonedThisTurn = true;
            return true;
        }

        private bool LightningStormEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1);
                return true;
            }
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectOption(0);
                return true;
            }
            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1);
                return true;
            }
            return false;
        }

        private bool TripleTacticsThrustEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (Duel.Player != 0 || Duel.LastChainPlayer != 1) return false;

            AI.SelectCard(new[] {
                CardId.NadirServant,
                CardId.Regenesis,
                CardId.RegenesisCode,
                CardId.DogmatikaPunishment,
                CardId.DimensionalBarrier
            });
            return true;
        }

        private bool HighnessArchfiendSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool DukeArchfiendSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool DogmatikaEcclesiaSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool RegenesisSageSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool AreSpellsNegatedOrDisabled()
        {
            bool orderActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(61740673) && !c.IsDisabled())
                || Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(61740673) && !c.IsDisabled());

            if (orderActive) return true;

            bool natBeast = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(33198837) && !c.IsDisabled());
            if (natBeast) return true;

            return false;
        }

        // ============================================================
        // OCG WRAPPER EVENT INTERCEPTIONS & FALLBACKS
        // ============================================================

        protected override bool ShouldStopExtending()
        {
            // Fallback Plan: If primary combo starters are negated, stop extending to save backrow and handtraps
            bool isStarterNegated = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.RegenesisArchfiend) && m.IsDisabled());
            if (isStarterNegated)
            {
                return true;
            }

            return base.ShouldStopExtending();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (Card != null && Card.Id == CardId.RegenesisArchfiend && min == 0 && cards.Count > 0)
            {
                var bestReveal = cards.FirstOrDefault(c => c.Attack == 2500 || c.Defense == 2500) ?? cards.OrderByDescending(c => c.Attack).First();
                return new[] { bestReveal };
            }

            if (Card != null && Card.Id == CardId.FidraulisHarmonia && hint == 504)
            {
                bool hasEnemyCards = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
                if (hasEnemyCards)
                {
                    var malong = cards.FirstOrDefault(c => c != null && c.Id == CardId.GoldenCloudBeastMalong);
                    if (malong != null) return new[] { malong };
                }
                var lulu = cards.FirstOrDefault(c => c != null && c.Id == CardId.DespianLuluwalilith);
                if (lulu != null) return new[] { lulu };
            }

            if (hint == 502)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500 && c.HasType(CardType.Effect)) return score + 5000;
                        }
                        return score + c.Attack;
                    }
                    else if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup()) return score + 2000;
                        return score + 100;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Protect valuable monsters from being used as material if possible
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 200;
                    if (IsValuableMonster(c)) return 100;
                    return 0;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] lowStatMonsters = {
                CardId.FidraulisHarmonia,
                CardId.MaxxC,
                CardId.HighnessArchfiend,
                CardId.DukeArchfiend,
                CardId.DogmatikaEcclesiaTheVirtuous
            };

            if (lowStatMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            YGOSharp.OCGWrapper.NamedCard cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.Attack <= 1000 && cardData.Defense > cardData.Attack && positions.Contains(CardPosition.FaceUpDefence))
                {
                    return CardPosition.FaceUpDefence;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var sorted = attackers.Where(c => c != null && !c.Attacked && c.Attack > 0)
                .OrderByDescending(c => c.Attack).ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefenseValue(ClientCard c)
            {
                if (c == null) return 0;
                return c.IsDefense() ? c.Defense : c.Attack;
            }

            var defeatable = defenders.Where(d => attacker.Attack > GetDefenseValue(d))
                .OrderByDescending(d => GetDefenseValue(d)).ToList();
            if (defeatable.Count > 0) return AI.Attack(attacker, defeatable.First());

            if (defenders.All(d => d == null)) return AI.Attack(attacker, null);

            var equal = defenders.Where(d => attacker.Attack == GetDefenseValue(d) && d.IsAttack()).ToList();
            if (equal.Count > 0 && !IsAceCard(attacker) && Bot.LifePoints > Enemy.LifePoints)
                return AI.Attack(attacker, equal.First());

            return null;
        }

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Check for any Ace on field
            int aceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (aceCount >= 1) return true;
            return base.IsBoardStrongEnough();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (IsValuableMonster(c)) return 500;
            return 100;
        }
    }
}

