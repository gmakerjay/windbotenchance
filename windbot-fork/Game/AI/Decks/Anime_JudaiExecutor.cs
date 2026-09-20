// ============================================================================
// CARD AUDIT — Anime_Judai (Jaden Yuki's HERO Destiny Phoenix & Dark Law Omni-Lock)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Destiny HERO - Destroyer Phoenix   | Fusion L8    | Yes  | Yes   | Destroy | Quick: Destroy 1 own card & 1 on field; rev SP| Opponent has key card, or during chain       | Target would destroy only win con           |
// | Masked HERO Dark Law               | Fusion L6    | No   | Yes   | None    | Opp cards sent to GY banished; rip opp hand   | Opp adds card from deck; floodgate active    | None                                        |
// | Destiny HERO - Plasma              | Monster L8   | No   | No    | Tribute | One-sided Skill Drain; equip 1 opp monster    | Bot has 3 monsters on field; steal opp boss  | Fodder has key active protection            |
// | Elemental HERO Stratos             | Monster L4   | No   | No    | None    | On NS/SS: Search any HERO or pop backrow      | Normal / Special Summoned                    | No HERO in deck and no backrow              |
// | Elemental HERO Shadow Mist         | Monster L4   | Yes  | Yes   | None    | On SS: Search Mask Change; on GY: Search HERO | Special Summoned or sent to GY               | Already used either effect this turn        |
// | Elemental HERO Liquid Soldier      | Monster L4   | Yes  | Yes   | None    | On NS: Revive L4 HERO; Fusion material: Draw 2| Normal Summoned with GY HERO; Fusion used    | No HERO in GY                               |
// | Vision HERO Faris                  | Monster L5   | Yes  | Yes   | Discard | Discard HERO to SS; place Increase in S/T     | In hand with other HERO discard fodder       | Hand has no other HERO                      |
// | Vision HERO Increase               | Monster L3   | Yes  | Yes   | Tribute | In S/T: Tribute HERO to SS; SS Vyon from deck  | In S/T zone and control HERO to tribute      | No Vyon left in deck                        |
// | Vision HERO Vyon                   | Monster L4   | Yes  | No    | Banish  | On NS/SS: Dump HERO; banish HERO: Search Poly | Normal / Special Summoned                    | Deck has no HERO or Poly                    |
// | Destiny HERO - Malicious           | Monster L6   | No   | No    | Banish  | Banish from GY: SS Malicious from Deck        | In GY and Malicious in Deck                  | No Malicious in Deck                        |
// | Destiny HERO - Denier              | Monster L3   | Yes  | Yes   | None    | On NS/SS: Recycle banished D-HERO; SS from GY | On summon or in GY with other D-HERO on field| Already used GY revive this duel            |
// | A Hero Lives                       | Spell Normal | No   | No    | Half LP | SS L4- Elemental HERO from Deck (Stratos/Mist)| Control no face-up monsters                  | Controls face-up monster                    |
// | Fusion Destiny                     | Spell Normal | Yes  | Yes   | None    | Fusion DPE directly from Deck (Malic+Denier)  | Main Phase 1 after or before combo lines      | Already used this turn                      |
// | Mask Change                        | Spell Quick  | No   | No    | Send    | Quick: Send HERO to GY, SS Masked HERO (Dark L| Quick play dodge / Battle Phase / Mist into DL| Target is already Dark Law                  |
// | Polymerization                     | Spell Normal | No   | No    | None    | Standard Fusion Summon                        | Have materials for Sunrise / Absolute Zero   | Not enough materials                        |
// | Miracle Fusion                     | Spell Normal | Yes  | Yes   | Banish  | Fusion from field/GY (Sunrise / Absolute Zero)| In hand with materials in GY/field           | No materials                                |
// | Elemental HERO Sunrise             | Fusion L7    | Yes  | Yes   | None    | On SS: Search Miracle Fusion; pop on attack  | Fusion Summoned                              | No Miracle Fusion in deck                   |
// | Elemental HERO Absolute Zero       | Fusion L8    | No   | No    | None    | On leaving field: Raigeki opp monsters        | Mask Change into Acid / Link fodder          | Opponent has no monsters                    |
// | Masked HERO Acid                   | Fusion L8    | No   | No    | None    | On SS: Harpie's Feather Duster + -300 ATK    | Mask Change from Liquid Soldier/Absolute Zero | Opponent has no Spells/Traps                |
// | Wake Up Your Elemental HERO        | Fusion L10   | Yes  | Yes   | None    | Multi-attack beatdown, burn, floats on destroy| Overwhelming lethal push                     | Need other combo pieces                     |
// | Xtra HERO Cross Crusader           | Link-2       | Yes  | Yes   | Tribute | Revive D-HERO, tribute D-HERO to search HERO  | 2 Warrior monsters on field                  | No D-HERO in GY                             |
// | Xtra HERO Wonder Driver            | Link-2       | Yes  | Yes   | None    | Recycle Fusion / Mask Change from GY to field | HERO summoned to zone it points to           | No spells in GY                             |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Anime_Judai", "Anime_Judai")]
    public class Anime_JudaiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int DestinyHeroPlasma = 83965310;
            public const int DestinyHeroMalicious = 9411399;
            public const int DestinyHeroDenier = 16605586;
            public const int ElementalHeroStratos = 40044918;
            public const int ElementalHeroShadowMist = 50720316;
            public const int ElementalHeroLiquidSoldier = 59392529;
            public const int VisionHeroFaris = 18094166;
            public const int VisionHeroIncrease = 22865492;
            public const int VisionHeroVyon = 27780618;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int Nibiru = 27204311;

            // Spells
            public const int AHeroLives = 8949584;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int Polymerization = 24094653;
            public const int MiracleFusion = 45906428;
            public const int FusionDestiny = 52947044;
            public const int MaskChange = 21143940;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int FoolishBurial = 81439173;

            // Traps
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int DestinyHeroDestroyerPhoenixEnforcer = 60461804;
            public const int WakeUpYourElementalHero = 32828466;
            public const int ElementalHeroSunrise = 22908820;
            public const int ElementalHeroAbsoluteZero = 40854197;
            public const int MaskedHeroDarkLaw = 58481572;
            public const int MaskedHeroBlast = 89870349;
            public const int MaskedHeroAcid = 29095552;
            public const int DestinyHeroDangerous = 30757127;
            public const int XtraHeroCrossCrusader = 58004362;
            public const int XtraHeroWonderDriver = 1948619;
            public const int XtraHeroDreadDecimator = 63813056;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
        }

        public Anime_JudaiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Faris Engine Climb ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "HERO-Faris-Engine",
                RequiredCards = new List<int> { CardId.VisionHeroFaris },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.VisionHeroFaris, ActionType = ExecutorType.Activate, Description = "Discard HERO to SS Faris" },
                    new() { CardId = CardId.VisionHeroFaris, ActionType = ExecutorType.Activate, Description = "Place Increase in Spell/Trap Zone" },
                    new() { CardId = CardId.VisionHeroIncrease, ActionType = ExecutorType.Activate, Description = "Tribute HERO -> SS Increase & Vyon" },
                    new() { CardId = CardId.VisionHeroVyon, ActionType = ExecutorType.Activate, Description = "Vyon dump Shadow Mist" },
                    new() { CardId = CardId.ElementalHeroShadowMist, ActionType = ExecutorType.Activate, Description = "Shadow Mist search HERO" }
                },
                FallbackLineName = "HERO-A-Hero-Lives"
            });

            // ── Line 2: A Hero Lives Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "HERO-A-Hero-Lives",
                RequiredCards = new List<int> { CardId.AHeroLives },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.AHeroLives, ActionType = ExecutorType.Activate, Description = "A Hero Lives -> SS Stratos" },
                    new() { CardId = CardId.ElementalHeroStratos, ActionType = ExecutorType.Activate, Description = "Stratos search Faris" }
                }
            });

            // ── Line 3: Fusion Destiny End Board ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "HERO-Fusion-Destiny",
                RequiredCards = new List<int> { CardId.FusionDestiny },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FusionDestiny, ActionType = ExecutorType.Activate, Description = "Fusion Destiny -> DPE from Deck" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Interruptions (Chain / Enemy Turn)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruActivate);

            // Boss Quick Negates & Interruptions
            AddExecutor(ExecutorType.Activate, CardId.DestinyHeroDestroyerPhoenixEnforcer, DPEPopActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaskChange, MaskChangeActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHeroDarkLaw, DarkLawHandRipActivate);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroSunrise, SunriseAttackPopActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // DPE Standby Phase Infinite GY Revive
            AddExecutor(ExecutorType.Activate, CardId.DestinyHeroDestroyerPhoenixEnforcer, DPEReviveInStandby);

            // -------------------------------------------------------------
            // 2. High Priority Starters & Spells (Main Phase 1)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.AHeroLives, AHeroLivesActivate);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotaActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // Vision HERO Faris Engine (Discard HERO to SS, place Increase)
            AddExecutor(ExecutorType.Activate, CardId.VisionHeroFaris, FarisActivate);
            AddExecutor(ExecutorType.Activate, CardId.VisionHeroIncrease, IncreaseActivate);

            // -------------------------------------------------------------
            // 3. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.ElementalHeroStratos, StratosNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroStratos, StratosActivate);

            AddExecutor(ExecutorType.Summon, CardId.VisionHeroVyon, VyonNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.VisionHeroVyon, VyonActivate);

            AddExecutor(ExecutorType.Summon, CardId.ElementalHeroLiquidSoldier, LiquidSoldierNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroLiquidSoldier, LiquidSoldierActivate);

            AddExecutor(ExecutorType.Summon, CardId.ElementalHeroShadowMist, ShadowMistNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroShadowMist, ShadowMistActivate);

            AddExecutor(ExecutorType.Summon, CardId.DestinyHeroDenier, DenierNormalSummon);

            // -------------------------------------------------------------
            // 4. Special Summons & Extenders
            // -------------------------------------------------------------
            // Malicious GY Banish SS
            AddExecutor(ExecutorType.Activate, CardId.DestinyHeroMalicious, MaliciousActivate);

            // Denier GY SS & recycle Malicious
            AddExecutor(ExecutorType.Activate, CardId.DestinyHeroDenier, DenierActivate);

            // Fusion Destiny (Summon DPE from Deck)
            AddExecutor(ExecutorType.Activate, CardId.FusionDestiny, FusionDestinyActivate);

            // Polymerization & Miracle Fusion
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroSunrise, SunriseSearchActivate);
            AddExecutor(ExecutorType.Activate, CardId.MiracleFusion, MiracleFusionActivate);

            // Absolute Zero float board wipe
            AddExecutor(ExecutorType.Activate, CardId.ElementalHeroAbsoluteZero, AbsoluteZeroActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHeroAcid, AcidActivate);

            // Destiny HERO - Plasma (One-sided Skill Drain & monster steal)
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHeroPlasma, PlasmaSummon);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHeroPlasma, PlasmaEquipActivate);

            // -------------------------------------------------------------
            // 5. Link Summons & Climbing
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHeroCrossCrusader, CrossCrusaderSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHeroCrossCrusader, CrossCrusaderActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.XtraHeroWonderDriver, WonderDriverSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHeroWonderDriver, WonderDriverActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.XtraHeroDreadDecimator, DreadDecimatorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeActivate);

            // Wake Up Your Elemental HERO
            AddExecutor(ExecutorType.Activate, CardId.WakeUpYourElementalHero, WakeUpActivate);

            // -------------------------------------------------------------
            // 6. Backrow Support & Fallbacks
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION LOGIC IMPLEMENTATIONS
        // =================================================================

        private bool DPEPopActivate()
        {
            // CRITICAL BUG FIX: Must be face-up in MonsterZone!
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;

            // Opponent MUST have at least 1 card on field! NEVER activate if enemy field is empty!
            var enemyTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && !IsTargetImmune(c))
                .ToList();
            if (enemyTargets.Count == 0) return false;

            // 1. Opponent's Turn: Quick Disruption
            if (Duel.Player == 1)
            {
                ClientCard target = enemyTargets.FirstOrDefault(m => m.IsMonster() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? enemyTargets.FirstOrDefault(s => s.IsSpell() || s.IsTrap())
                                 ?? enemyTargets.FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(Card); // Target DPE himself to trigger next Standby Phase revival
                    AI.SelectNextCard(target);
                    return true;
                }
            }

            // 2. Our Turn:
            if (Duel.Player == 0)
            {
                // A. Chain interrupt if opponent activated an effect
                if (Duel.LastChainPlayer == 1)
                {
                    ClientCard target = enemyTargets.FirstOrDefault(c => c == LastChainCard) ?? enemyTargets.FirstOrDefault();
                    if (target != null)
                    {
                        AI.SelectCard(Card);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }

                // B. Removal of dangerous floodgates/negators in Main Phase 1 so we can play
                ClientCard criticalThreat = enemyTargets.FirstOrDefault(c => c.IsMonster() && AntiFloodgateHelper.NegateMonsterIds.Contains(c.Id))
                                         ?? enemyTargets.FirstOrDefault(c => c.IsSpell() && c.IsFaceup());
                if (criticalThreat != null)
                {
                    AI.SelectCard(Card);
                    AI.SelectNextCard(criticalThreat);
                    return true;
                }

                // C. In Battle Phase (after DPE attacks with 2500 ATK!) or Main Phase 2: pop remaining enemy card
                if (Duel.Phase >= DuelPhase.BattleStart)
                {
                    ClientCard target = enemyTargets.OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (target != null)
                    {
                        AI.SelectCard(Card);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool DPEReviveInStandby()
        {
            // CRITICAL BUG FIX: MUST be in Graveyard! NEVER return true when in MonsterZone!
            if (Card.Location != CardLocation.Grave) return false;

            // Revive DPE during Standby Phase from GY
            AI.SelectCard(CardId.DestinyHeroDestroyerPhoenixEnforcer, CardId.DestinyHeroPlasma, CardId.DestinyHeroMalicious);
            return true;
        }

        private bool DarkLawHandRipActivate()
        {
            return true; // Random banish from opponent hand
        }

        private bool SunriseAttackPopActivate()
        {
            ClientCard enemyTarget = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup())
                                  ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                                  ?? Enemy.GetMonsters().FirstOrDefault();
            if (enemyTarget != null)
            {
                AI.SelectCard(enemyTarget);
                return true;
            }
            return false;
        }

        private bool SunriseSearchActivate()
        {
            AI.SelectCard(CardId.MiracleFusion);
            return true;
        }

        private bool MaskChangeActivate()
        {
            // Fast dodge or transform into Dark Law
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.ElementalHeroShadowMist)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.ElementalHeroLiquidSoldier || m.Id == CardId.ElementalHeroAbsoluteZero)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.ElementalHeroStratos);

            if (target != null)
            {
                AI.SelectCard(target);
                if (target.Id == CardId.ElementalHeroShadowMist)
                    AI.SelectNextCard(CardId.MaskedHeroDarkLaw);
                else if (target.Id == CardId.ElementalHeroAbsoluteZero || target.Id == CardId.ElementalHeroLiquidSoldier)
                    AI.SelectNextCard(CardId.MaskedHeroAcid);
                else if (target.Id == CardId.ElementalHeroStratos)
                    AI.SelectNextCard(CardId.MaskedHeroBlast);
                return true;
            }
            return false;
        }

        private bool AHeroLivesActivate()
        {
            if (Bot.GetMonsterCount() > 0) return false;
            // Pay half LP: SS Stratos or Shadow Mist
            AI.SelectCard(CardId.ElementalHeroStratos, CardId.ElementalHeroShadowMist);
            return true;
        }

        private bool RotaActivate()
        {
            AI.SelectCard(CardId.ElementalHeroStratos, CardId.VisionHeroFaris, CardId.ElementalHeroShadowMist);
            return true;
        }

        private bool FoolishBurialActivate()
        {
            AI.SelectCard(CardId.DestinyHeroMalicious, CardId.ElementalHeroShadowMist, CardId.DestinyHeroDenier);
            return true;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (Enemy.GetMonsterCount() > 0 && Duel.Turn > 1)
            {
                AI.SelectOption(1); // Take control of opponent monster
                ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            AI.SelectOption(0); // Draw 2 cards
            return true;
        }

        private bool FarisActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard 1 HERO (prefer Shadow Mist or Malicious)
                ClientCard discard = Bot.Hand.FirstOrDefault(c => c.Id == CardId.ElementalHeroShadowMist || c.Id == CardId.DestinyHeroMalicious)
                                  ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.DestinyHeroDenier || c.Id == CardId.ElementalHeroLiquidSoldier)
                                  ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.VisionHeroFaris && c.HasRace(CardRace.Warrior));
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Place Increase in S/T zone
                AI.SelectCard(CardId.VisionHeroIncrease);
                return true;
            }
            return false;
        }

        private bool IncreaseActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Tribute 1 HERO (prefer Faris or low stat HERO, NEVER DPE, Dark Law, or Plasma)
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.VisionHeroFaris)
                                  ?? Bot.GetMonsters().FirstOrDefault(m => m.Id != CardId.DestinyHeroDestroyerPhoenixEnforcer && m.Id != CardId.MaskedHeroDarkLaw && m.Id != CardId.DestinyHeroPlasma && m.Attack <= 1600);
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    AI.SelectNextCard(CardId.VisionHeroVyon);
                    return true;
                }
            }
            return false;
        }

        private bool StratosNormalSummon()
        {
            return true;
        }

        private bool StratosActivate()
        {
            // If enemy has backrow and we control multiple HEROs, pop backrow; otherwise search HERO
            if (Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() > 1)
            {
                AI.SelectOption(0);
                ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            AI.SelectOption(1);
            AI.SelectCard(CardId.VisionHeroFaris, CardId.DestinyHeroPlasma, CardId.ElementalHeroLiquidSoldier, CardId.DestinyHeroMalicious);
            return true;
        }

        private bool VyonNormalSummon()
        {
            return true;
        }

        private bool VyonActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Effect 1: Dump HERO from Deck (Use GetRemainingCount because cards in Bot.Deck have Id == 0)
                if (GetRemainingCount(CardId.ElementalHeroShadowMist) > 0 ||
                    GetRemainingCount(CardId.DestinyHeroMalicious) > 0 ||
                    GetRemainingCount(CardId.DestinyHeroDenier) > 0)
                {
                    AI.SelectCard(CardId.ElementalHeroShadowMist, CardId.DestinyHeroMalicious, CardId.DestinyHeroDenier);
                    return true;
                }
                // Effect 2: Banish HERO from GY to search Poly from Deck
                if (GetRemainingCount(CardId.Polymerization) > 0)
                {
                    ClientCard banish = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.VisionHeroFaris || c.Id == CardId.VisionHeroIncrease)
                                     ?? Bot.Graveyard.FirstOrDefault(c => c.HasRace(CardRace.Warrior) && c.Id != CardId.DestinyHeroMalicious && c.Id != CardId.DestinyHeroDenier && c.Id != CardId.DestinyHeroDestroyerPhoenixEnforcer);
                    if (banish != null)
                    {
                        AI.SelectCard(banish);
                        AI.SelectNextCard(CardId.Polymerization);
                        return true;
                    }
                }
                return true;
            }
            return false;
        }

        private bool LiquidSoldierNormalSummon()
        {
            return Bot.GetMonsterCount() == 0 || Bot.Graveyard.Any(c => c.HasRace(CardRace.Warrior) && c.Level <= 4);
        }

        private bool LiquidSoldierActivate()
        {
            // On NS: revive Stratos or Shadow Mist
            AI.SelectCard(CardId.ElementalHeroStratos, CardId.ElementalHeroShadowMist);
            return true;
        }

        private bool ShadowMistNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool ShadowMistActivate()
        {
            // On SS: search Mask Change
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.MaskChange);
                return true;
            }
            // On GY send: search Faris, Stratos, or Plasma
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.VisionHeroFaris, CardId.ElementalHeroStratos, CardId.DestinyHeroPlasma);
                return true;
            }
            return false;
        }

        private bool DenierNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool MaliciousActivate()
        {
            return true; // SS Malicious from Deck
        }

        private bool DenierActivate()
        {
            // Recycle banished Malicious to top of deck
            return true;
        }

        private bool FusionDestinyActivate()
        {
            // Verify materials in Deck/Hand
            bool hasMalicious = Bot.HasInHand(CardId.DestinyHeroMalicious) || GetRemainingCount(CardId.DestinyHeroMalicious) > 0;
            bool hasDenier = Bot.HasInHand(CardId.DestinyHeroDenier) || GetRemainingCount(CardId.DestinyHeroDenier) > 0;
            bool hasPlasma = Bot.HasInHand(CardId.DestinyHeroPlasma) || GetRemainingCount(CardId.DestinyHeroPlasma) > 0;

            if (hasMalicious && (hasDenier || hasPlasma))
            {
                AI.SelectCard(CardId.DestinyHeroDestroyerPhoenixEnforcer);
                AI.SelectNextCard(CardId.DestinyHeroMalicious, CardId.DestinyHeroDenier, CardId.DestinyHeroPlasma);
                return true;
            }
            return false;
        }

        private bool PolymerizationActivate()
        {
            // Prefer Sunrise (2 HEROs of diff Attributes) or Absolute Zero (HERO + WATER)
            AI.SelectCard(CardId.ElementalHeroSunrise, CardId.ElementalHeroAbsoluteZero, CardId.DestinyHeroDangerous);
            return true;
        }

        private bool MiracleFusionActivate()
        {
            AI.SelectCard(CardId.ElementalHeroSunrise, CardId.ElementalHeroAbsoluteZero, CardId.WakeUpYourElementalHero);
            return true;
        }

        private bool AbsoluteZeroActivate()
        {
            return true; // Raigeki opponent monsters
        }

        private bool AcidActivate()
        {
            return true; // Harpie Feather Duster
        }

        private bool PlasmaSummon()
        {
            // Tribute 3 monsters to summon one-sided Skill Drain boss
            if (Bot.GetMonsterCount() >= 3)
            {
                var tributes = Bot.GetMonsters()
                                  .Where(m => m.Id != CardId.DestinyHeroDestroyerPhoenixEnforcer && m.Id != CardId.MaskedHeroDarkLaw)
                                  .OrderBy(m => m.Attack)
                                  .Take(3)
                                  .ToList();
                if (tributes.Count == 3)
                {
                    AI.SelectCard(tributes);
                    return true;
                }
            }
            return false;
        }

        private bool PlasmaEquipActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                             ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrossCrusaderSummon()
        {
            return Bot.GetMonsterCount() >= 2 && Bot.Graveyard.Any(c => c.Id == CardId.DestinyHeroMalicious || c.Id == CardId.DestinyHeroDenier);
        }

        private bool CrossCrusaderActivate()
        {
            // Revive D-HERO
            if (Bot.Graveyard.Any(c => c.Id == CardId.DestinyHeroMalicious || c.Id == CardId.DestinyHeroDenier))
            {
                AI.SelectCard(CardId.DestinyHeroMalicious, CardId.DestinyHeroDenier);
                return true;
            }
            // Tribute D-HERO to search HERO
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.DestinyHeroMalicious || m.Id == CardId.DestinyHeroDenier);
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                AI.SelectNextCard(CardId.DestinyHeroPlasma, CardId.ElementalHeroLiquidSoldier, CardId.ElementalHeroStratos);
                return true;
            }
            return false;
        }

        private bool WonderDriverSummon()
        {
            return Bot.GetMonsterCount() >= 2 && Bot.Graveyard.Any(c => c.Id == CardId.MaskChange || c.Id == CardId.Polymerization);
        }

        private bool WonderDriverActivate()
        {
            AI.SelectCard(CardId.MaskChange, CardId.Polymerization, CardId.MiracleFusion);
            return true;
        }

        private bool DreadDecimatorSummon()
        {
            return Bot.GetMonsterCount() >= 3;
        }

        private bool SPLittleKnightSummon()
        {
            return Bot.GetMonsterCount() >= 2 && (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
        }

        private bool SPLittleKnightActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard botCard = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.SPLittleKnight);
                ClientCard oppCard = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup());
                if (botCard != null && oppCard != null)
                {
                    AI.SelectCard(botCard);
                    AI.SelectNextCard(oppCard);
                    return true;
                }
            }
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                             ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                             ?? Enemy.GetMonsters().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool KnightmarePhoenixSummon()
        {
            return Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 2;
        }

        private bool KnightmarePhoenixActivate()
        {
            ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool KnightmareUnicornSummon()
        {
            return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0 && Bot.GetMonsterCount() >= 3;
        }

        private bool KnightmareUnicornActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AccesscodeSummon()
        {
            return Bot.GetMonsterCount() >= 3;
        }

        private bool AccesscodeActivate()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool WakeUpActivate()
        {
            // On destruction: SS Warrior from Deck
            AI.SelectCard(CardId.DestinyHeroPlasma, CardId.ElementalHeroStratos);
            return true;
        }

        // =================================================================
        // UNIVERSAL TACTICS & ANTI-PATTERNS
        // =================================================================

        private bool CalledByTheGraveActivate()
        {
            if (Duel.LastChainPlayer == 1 && Duel.CurrentChain.Count > 0)
            {
                ClientCard target = Enemy.Graveyard.LastOrDefault(c => c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool AshBlossomActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool InfiniteImpermanenceActivate()
        {
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 1500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool EffectVeilerActivate()
        {
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 1500 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)))
                                 ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool NibiruActivate()
        {
            // Only on opponent's turn in Main Phase
            if (Duel.Player != 1 || (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2))
                return false;

            if (Enemy.GetMonsterCount() < 2) return false;

            // Do not tribute our own board if we control active boss monsters, unless opponent has lethal
            bool weHaveBoss = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.DestinyHeroDestroyerPhoenixEnforcer || m.Id == CardId.MaskedHeroDarkLaw || m.Id == CardId.DestinyHeroPlasma));
            int enemyTotalAtk = Enemy.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            if (weHaveBoss && enemyTotalAtk < Bot.LifePoints)
            {
                return false;
            }

            // Always select Defense Position for the Primal Being Token!
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap() && Card.Id == CardId.InfiniteImpermanence)
            {
                return true;
            }
            if (Card.Id == CardId.MaskChange)
            {
                return true;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            if (Card.Attack < 1200 && Card.IsAttack()) return true;
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Universal Safeguard: Primal Being Token given to opponent MUST be in Defense!
            if (cardId == 27204312 && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            // Ace Bosses -> FaceUpAttack
            if (cardId == CardId.DestinyHeroDestroyerPhoenixEnforcer ||
                cardId == CardId.MaskedHeroDarkLaw ||
                cardId == CardId.DestinyHeroPlasma ||
                cardId == CardId.ElementalHeroSunrise ||
                cardId == CardId.ElementalHeroAbsoluteZero ||
                cardId == CardId.MaskedHeroAcid ||
                cardId == CardId.AccesscodeTalker)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Small extenders -> FaceUpDefence when possible
            if (cardId == CardId.VisionHeroVyon || cardId == CardId.VisionHeroIncrease || cardId == CardId.DestinyHeroDenier)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Anti-Pattern Rule 1: Isolation of HINTMSG_ATOHAND (506)
            if (hint == 506)
            {
                var preferred = new List<int>
                {
                    CardId.VisionHeroFaris,
                    CardId.ElementalHeroStratos,
                    CardId.MaskChange,
                    CardId.DestinyHeroPlasma,
                    CardId.ElementalHeroLiquidSoldier,
                    CardId.Polymerization,
                    CardId.MiracleFusion,
                    CardId.DestinyHeroMalicious,
                    CardId.DestinyHeroDenier
                };

                var matches = cards.Where(c => preferred.Contains(c.Id))
                                   .OrderBy(c => preferred.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Anti-Pattern Rule 2: Destruction (502) or Banish (503)
            if (hint == 502 || hint == 503)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).Take(max).ToList();
                }

                // If ALL options are our own cards (e.g. DPE self-destruction prompt: "destroy 1 card you control"):
                var ownTargets = cards.Where(c => c.Controller == 0).ToList();
                if (ownTargets.Count > 0)
                {
                    // Target DPE himself first! (He floats and revives next turn)
                    var preferredOwn = ownTargets.FirstOrDefault(c => c.Id == CardId.DestinyHeroDestroyerPhoenixEnforcer)
                                    ?? ownTargets.FirstOrDefault(c => c.Id == CardId.ElementalHeroAbsoluteZero) // Triggers Raigeki on leaving field
                                    ?? ownTargets.FirstOrDefault(c => c.Id != CardId.MaskedHeroDarkLaw && c.Id != CardId.DestinyHeroPlasma && c.IsSpell())
                                    ?? ownTargets.FirstOrDefault(c => c.Id != CardId.MaskedHeroDarkLaw && c.Id != CardId.DestinyHeroPlasma);
                    if (preferredOwn != null)
                    {
                        return new List<ClientCard> { preferredOwn };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
