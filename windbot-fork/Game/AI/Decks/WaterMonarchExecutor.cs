// ============================================================================
// CARD AUDIT — WaterMonarch (Water Monarch / Grizzly Control)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Mobius the Frost Monarch    | Monster L6   | No   | No    | Tribute1| Pop up to 2 Spells/Traps on Tribute Summon    | Opponent controls 1-2 Spells/Traps           | Opponent controls 0 Spells/Traps            |
// | Zaborg the Thunder Monarch  | Monster L6   | No   | No    | Tribute1| Pop 1 monster on Tribute Summon               | Opponent controls 1+ monsters                | Opponent controls 0 monsters (mandatory pop)|
// | Mother Grizzly              | Monster L4   | No   | No    | None    | Battle destroyed: SS WATER <=1500 ATK from dck| Pull Sinister Serpent / Yomi Ship / Grizzly   | No WATER targets left in deck               |
// | Sinister Serpent            | Monster L1   | No   | No    | None    | Standby Phase: return from GY to hand         | Every Standby Phase if in GY                 | Already in hand / field                     |
// | Yomi Ship                   | Monster L3   | No   | No    | None    | Battle destroyed: destroys battling monster   | Crash or be attacked by big beater           | Facing token / harmless monster             |
// | Gravekeeper's Spy           | Monster L4   | No   | No    | None    | FLIP: SS 1 Gravekeeper with <=1500 ATK from dck| Set face-down 2000 DEF; SS 2nd Spy on flip    | Hand has 0 targets in deck                  |
// | D.D. Assailant              | Monster L4   | No   | No    | None    | Battle destroyed by opp monster: banish both  | Normal summon 1700 beater; crash into boss   | Battling harmless weak monster               |
// | Breaker the Magical Warrior | Monster L4   | Yes  | No    | 1 Cntr  | 1600+300 ATK; remove counter to pop 1 S/T     | Pop threatening backrow                       | Opponent controls 0 Spells/Traps            |
// | Sangan                      | Monster L3   | No   | No    | None    | Sent field to GY: search <=1500 ATK monster   | Trigger search: Sinister / Tsukuyomi / Faith  | None                                        |
// | Tribe-Infecting Virus       | Monster L4   | No   | No    | Discard | Declare Type: destroy all face-up of that type| Discard Sinister Serpent to wipe enemy board  | Discarding irreplaceable card               |
// | Magician of Faith           | Monster L1   | No   | No    | None    | FLIP: Return 1 Spell from GY to hand          | Retrieve Pot / Charity / Duo / Snatch / Storm | GY has 0 Spells                             |
// | Morphing Jar                | Monster L2   | No   | No    | None    | FLIP: Discard hand, draw 5 cards              | Hand is small (<= 2 cards)                    | Hand has 4+ good cards                      |
// | Tsukuyomi                   | Monster L4   | No   | No    | None    | Normal/Flip: flip 1 monster face-down DEF     | Loop TER (destroy equip) or reuse MoF / Spy   | No valid face-up monsters                   |
// | Kycoo the Ghost Destroyer   | Monster L4   | No   | No    | None    | 1800 ATK; battle dmg: banish 2 opp GY monsters| Attack to deal dmg and banish GY threats      | Opponent GY has 0 monsters                  |
// | Ninja Grandmaster Sasuke    | Monster L4   | No   | No    | None    | 1800 ATK; destroys face-up DEF mon at dmg step| Attack face-up DEF monster (Tokens/Reaper)   | Opponent has stronger ATK monsters          |
// | Des Wombat                  | Monster L3   | No   | No    | None    | 1600 ATK; effect damage to controller is 0    | Normal summon beater; immune to burn damage   | None                                        |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate when available                | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3, discard 2 (discard Sinister Serpent)  | Always activate; discard Sinister / dead card | Hand has <= 1 card                          |
// | Delinquent Duo              | Spell Normal | No   | No    | 1000 LP | Pay 1000 LP: Opponent discards 2 cards        | Early game hand rip (Opp hand >= 2)           | Bot LP <= 1000                              |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent controls 0 face-up monsters        |
// | Premature Burial            | Spell Equip  | No   | No    | 800 LP  | Pay 800 LP: SS monster from GY (Monarch/TER)  | Revive Mobius / Zaborg / TER for game push    | Bot LP <= 800 or GY has no targets          |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Opponent controls 2+ S/T and > our S/T        | We control key face-up continuous / equips  |
// | Mystical Space Typhoon      | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on field                 | Remove dangerous continuous/field/backrow     | No valid targets                            |
// | Nobleman of Crossout        | Spell Normal | No   | No    | None    | Banish 1 face-down monster; strip copies      | Banish opponent face-down DEF monster         | Opponent controls 0 face-down monsters      |
// | Book of Moon                | Spell Quick  | No   | No    | None    | Change 1 face-up monster to face-down DEF     | Stop enemy attack / reset TER / flip MoF      | No valid targets                            |
// | Scapegoat                   | Spell Quick  | No   | No    | None    | SS 4 Sheep Tokens (Lv1) in DEF                | Opponent attacks or end of opponent turn      | Our turn (blocks normal/flip summons)       |
// | Metamorphosis               | Spell Normal | No   | No    | Tribute | Tribute 1 monster: SS Fusion with same Level  | Tribute Token/Sinister -> TER; Monarch -> Lv6 | No valid targets on field                   |
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with 1+ monsters             | Harmless weak attack                        |
// | Torrential Tribute          | Trap Normal  | No   | No    | None    | Destroy all monsters on field on summon       | Opponent summons boss / swarms board          | We control established Monarch/TER board    |
// | Ring of Destruction         | Trap Normal  | No   | No    | None    | Destroy face-up monster; both take damage     | Remove dangerous monster or lethal burn       | ATK >= our LP (suicide prevention)          |
// | Call of the Haunted         | Trap Cont    | No   | No    | None    | Special Summon 1 monster from GY in ATK       | Revive Monarch during opponent EP or push     | No monsters in GY                           |
// | Bottomless Trap Hole        | Trap Normal  | No   | No    | None    | Destroy and banish summoned monster >=1500 ATK| High ATK summon disruption                    | Summoned monster < 1500 ATK                 |
// | Solemn Judgment             | Trap Counter | No   | No    | Half LP | Pay half LP: Negate Summon or Spell/Trap card | Stop Heavy Storm, Snatch Steal, or enemy boss | Trivial opponent plays or Bot LP <= 1000    |
// | Sakuretsu Armor             | Trap Normal  | No   | No    | None    | Destroy attacking monster                     | Destroy attacker >= 1500 ATK                  | Attacker is weak and harmless               |
// | Trap Dustshoot              | Trap Normal  | No   | No    | None    | Standby: Reveal opp hand, return 1 mon to deck| Opponent has >= 4 cards in hand in Standby    | Opp hand < 4 or not Standby Phase           |
// | Horn of Heaven              | Trap Counter | No   | No    | Tribute1| Tribute 1 monster: negate Summon & destroy    | Tribute Token/Sinister to stop dangerous boss | Only have Monarch / Boss on field           |
// | Thousand-Eyes Restrict      | Fusion L1    | Yes  | No    | None    | Equip 1 opp monster; lock attacks & positions | Suck enemy biggest monster                    | Already has equipped monster                |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("WaterMonarch", "WaterMonarch")]
    [Deck("GOAT_WaterMonarch", "WaterMonarch")]
    public class WaterMonarchExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters
            public const int MobiusTheFrostMonarch = 4929256;
            public const int ZaborgTheThunderMonarch = 51945556;
            public const int MotherGrizzly = 57839750;
            public const int SinisterSerpent = 8131171;
            public const int YomiShip = 51534754;
            public const int GravekeepersSpy = 24317029;
            public const int DdAssailant = 70074904;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int BreakerTheMagicalWarriorAlt = 71413902;
            public const int Sangan = 26202165;
            public const int TribeInfectingVirus = 33184167;
            public const int MagicianOfFaith = 31560081;
            public const int MorphingJar = 33508719;
            public const int Tsukuyomi = 34853266;
            public const int KycooTheGhostDestroyer = 88240808;
            public const int NinjaGrandmasterSasuke = 4041838;
            public const int DesWombat = 9637706;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int HeavyStorm = 19613556;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int NoblemanOfCrossout = 71044499;
            public const int BookOfMoon = 14087893;
            public const int Scapegoat = 73915051;
            public const int SheepToken = 73915052;
            public const int Metamorphosis = 46411259;

            // Traps
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int RingOfDestruction = 83555666;
            public const int RingOfDestructionAlt = 83555667;
            public const int CallOfTheHaunted = 97077563;
            public const int CallOfTheHauntedAlt = 97077564;
            public const int BottomlessTrapHole = 29401950;
            public const int SolemnJudgment = 41420027;
            public const int SakuretsuArmor = 56120475;
            public const int TrapDustshoot = 64697231;
            public const int HornOfHeaven = 98069388;

            // Extra Deck (Fusions)
            public const int ThousandEyesRestrict = 63519819;
            public const int DarkBalterTheTerrible = 80071763;
            public const int FiendSkullDragon = 66235877;
            public const int DarkFlareKnight = 13722870;
            public const int DarkBladeTheDragonKnight = 86805855;
            public const int MysticalSand = 32751480;
            public const int KingDragun = 13756293;
            public const int GatlingDragon = 87751584;
            public const int DarkPaladin = 98502113;
            public const int CyberTwinDragon = 74157028;
            public const int ArcanaKnightJoker = 6150044;
            public const int ArcanaKnightJokerAlt = 6150045;
            public const int CyberEndDragon = 1546123;
            public const int CyberEndDragonAlt = 1546124;
            public const int BlueEyesUltimateDragon = 23995349;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.MobiusTheFrostMonarch,
            CardId.ZaborgTheThunderMonarch,
            CardId.ThousandEyesRestrict,
            CardId.CyberTwinDragon,
            CardId.GatlingDragon,
            CardId.DarkBalterTheTerrible,
            CardId.DarkPaladin
        };

        internal WaterMonarchPlugin Plugin { get; private set; }
        private ClientCard _tsukuyomiTarget = null;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public WaterMonarchExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            Plugin = new WaterMonarchPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        private void RegisterHelperModules()
        {
            // 1. Ace Card Protection
            ResourcePlan.RegisterAceCards(BossMonsters);
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // 2. Combo Starters & Bait
            BaitPlanner.RegisterComboStarters(
                CardId.MotherGrizzly,
                CardId.GravekeepersSpy,
                CardId.BreakerTheMagicalWarrior,
                CardId.PotOfGreed,
                CardId.GracefulCharity
            );
            BaitPlanner.RegisterBaitCards(
                CardId.DelinquentDuo,
                CardId.MysticalSpaceTyphoon,
                CardId.NoblemanOfCrossout
            );

            // 3. Negators & High Value Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.ThousandEyesRestrict,
                CardId.MobiusTheFrostMonarch,
                CardId.ZaborgTheThunderMonarch,
                CardId.SolemnJudgment,
                CardId.MirrorForce,
                CardId.TorrentialTribute,
                CardId.RingOfDestruction
            );
        }

        private void RegisterExecutors()
        {
            // 1. Hand Disruption & Power Draws
            AddExecutor(ExecutorType.Activate, CardId.SinisterSerpent, SinisterSerpentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // 2. Removal Spells & Board Wipes
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // 3. Thousand-Eyes Restrict & Metamorphosis Engine
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThousandEyesRestrict, TerEffect);

            // 4. Tsukuyomi Loop & Flip Re-activators
            AddExecutor(ExecutorType.Summon, CardId.Tsukuyomi, TsukuyomiSummon);
            AddExecutor(ExecutorType.Activate, CardId.Tsukuyomi, TsukuyomiEffect);

            // 5. Tribe-Infecting Virus Board Wipe
            AddExecutor(ExecutorType.Summon, CardId.TribeInfectingVirus, TribeInfectingVirusSummon);
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // 6. Monarch Tribute Summons & Trigger Effects
            AddExecutor(ExecutorType.Summon, CardId.MobiusTheFrostMonarch, MobiusTributeSummon);
            AddExecutor(ExecutorType.Activate, CardId.MobiusTheFrostMonarch, MobiusEffect);
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheThunderMonarch, ZaborgTributeSummon);
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheThunderMonarch, ZaborgEffect);

            // 7. Beatdown & Tech Summons
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior, BreakerSummon);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Summon, CardId.DdAssailant, DdAssailantSummon);
            AddExecutor(ExecutorType.Activate, CardId.DdAssailant, DdAssailantEffect);
            AddExecutor(ExecutorType.Summon, CardId.NinjaGrandmasterSasuke, SasukeSummon);
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer, KycooSummon);
            AddExecutor(ExecutorType.Activate, CardId.KycooTheGhostDestroyer, KycooEffect);
            AddExecutor(ExecutorType.Summon, CardId.DesWombat, DesWombatSummon);

            // 8. Water Float & Defense Sets
            AddExecutor(ExecutorType.Activate, CardId.MotherGrizzly, MotherGrizzlyEffect);
            AddExecutor(ExecutorType.Summon, CardId.MotherGrizzly, MotherGrizzlySummon);
            AddExecutor(ExecutorType.MonsterSet, CardId.MotherGrizzly);
            AddExecutor(ExecutorType.Activate, CardId.YomiShip, YomiShipEffect);
            AddExecutor(ExecutorType.Summon, CardId.YomiShip, YomiShipSummon);
            AddExecutor(ExecutorType.MonsterSet, CardId.YomiShip);
            AddExecutor(ExecutorType.Activate, CardId.GravekeepersSpy, GravekeepersSpyEffect);
            AddExecutor(ExecutorType.MonsterSet, CardId.GravekeepersSpy);
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith, MagicianOfFaithEffect);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.Sangan, SanganEffect);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.SinisterSerpent);

            // 9. Quick-Play Spells & Defensive Traps
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, TorrentialTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SakuretsuArmor, SakuretsuArmorEffect);
            AddExecutor(ExecutorType.Activate, CardId.BottomlessTrapHole, BottomlessTrapHoleEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapDustshoot, TrapDustshootEffect);
            AddExecutor(ExecutorType.Activate, CardId.HornOfHeaven, HornOfHeavenEffect);

            // 10. Backrow Setting
            AddExecutor(ExecutorType.SpellSet, CardId.Scapegoat);
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon);
            AddExecutor(ExecutorType.SpellSet, CardId.MirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.TorrentialTribute);
            AddExecutor(ExecutorType.SpellSet, CardId.RingOfDestruction);
            AddExecutor(ExecutorType.SpellSet, CardId.CallOfTheHaunted);
            AddExecutor(ExecutorType.SpellSet, CardId.SakuretsuArmor);
            AddExecutor(ExecutorType.SpellSet, CardId.BottomlessTrapHole);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapDustshoot);
            AddExecutor(ExecutorType.SpellSet, CardId.HornOfHeaven);

            // 11. Repositioning
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // First turn advantage in GOAT
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _tsukuyomiTarget = null;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXECUTION LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool SinisterSerpentEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool HeavyStormEffect()
        {
            int enemySpells = Enemy.GetSpellCount();
            int ourSpells = Bot.GetSpellCount();
            return enemySpells >= 2 && enemySpells > ourSpells;
        }

        private bool NoblemanOfCrossoutEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFacedown());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SnatchStealEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null && target.Attack >= 1500)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PrematureBurialEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(Bot.Graveyard.Where(c => c.IsMonster()).ToList());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CallOfTheHauntedEffect()
        {
            // Activate during opponent's End Phase or during our turn for push
            if (Duel.Player == 1 && Duel.Phase != DuelPhase.End) return false;
            var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(Bot.Graveyard.Where(c => c.IsMonster()).ToList());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MetamorphosisEffect()
        {
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return false;

            // 1. Level 1 Tribute (Sheep Token or Sinister Serpent) -> Thousand-Eyes Restrict
            var lv1 = monsters.FirstOrDefault(m => m.Level == 1 && (m.Id == CardId.SheepToken || m.Id == CardId.SinisterSerpent));
            if (lv1 != null && !Bot.HasInMonstersZone(CardId.ThousandEyesRestrict) && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(lv1);
                AI.SelectNextCard(CardId.ThousandEyesRestrict);
                return true;
            }

            // 2. Level 6 Tribute (Mobius or Zaborg if weakened / targeted) -> Dark Blade or Dark Flare Knight
            var lv6 = monsters.FirstOrDefault(m => m.Level == 6);
            if (lv6 != null && (lv6.Attack < 2400 || Enemy.GetMonsterCount() >= 2))
            {
                AI.SelectCard(lv6);
                AI.SelectNextCard(new[] { CardId.DarkBladeTheDragonKnight, CardId.DarkFlareKnight, CardId.MysticalSand });
                return true;
            }

            // 3. Level 5 Tribute -> Dark Balter the Terrible / Fiend Skull Dragon
            var lv5 = monsters.FirstOrDefault(m => m.Level == 5);
            if (lv5 != null)
            {
                AI.SelectCard(lv5);
                AI.SelectNextCard(new[] { CardId.DarkBalterTheTerrible, CardId.FiendSkullDragon });
                return true;
            }

            // 4. Level 8 Tribute -> Cyber Twin Dragon / Gatling Dragon
            var lv8 = monsters.FirstOrDefault(m => m.Level == 8);
            if (lv8 != null)
            {
                AI.SelectCard(lv8);
                AI.SelectNextCard(new[] { CardId.CyberTwinDragon, CardId.GatlingDragon, CardId.DarkPaladin });
                return true;
            }

            return false;
        }

        private bool TerEffect()
        {
            // TER can only have 1 monster equipped at a time
            if (Card.EquipCards != null && Card.EquipCards.Count > 0)
                return false;

            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TsukuyomiSummon()
        {
            // Loop 1: TER is equipped -> flip it down to destroy the equip card and reset for next suck
            var equippedTer = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.ThousandEyesRestrict && m.EquipCards != null && m.EquipCards.Count > 0);
            if (equippedTer != null)
            {
                _tsukuyomiTarget = equippedTer;
                return true;
            }

            // Loop 2: Magician of Faith is face-up -> flip it down to reuse its spell recursion
            var faceupFaith = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.MagicianOfFaith);
            if (faceupFaith != null)
            {
                _tsukuyomiTarget = faceupFaith;
                return true;
            }

            // Loop 3: Gravekeeper's Spy is face-up -> flip it down to reuse flip summon
            var faceupSpy = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.GravekeepersSpy);
            if (faceupSpy != null && Bot.Deck.Any(c => c.Id == CardId.GravekeepersSpy))
            {
                _tsukuyomiTarget = faceupSpy;
                return true;
            }

            // Battle interaction: Enemy monster with high ATK but low DEF
            int ourMaxAtk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Select(m => m.Attack).DefaultIfEmpty(1100).Max();
            var flippableEnemy = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.Defense <= Math.Max(1100, ourMaxAtk) && m.Attack >= 1500 && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (flippableEnemy != null)
            {
                _tsukuyomiTarget = flippableEnemy;
                return true;
            }

            return false;
        }

        private bool TsukuyomiEffect()
        {
            if (_tsukuyomiTarget != null)
            {
                AI.SelectCard(_tsukuyomiTarget);
                _tsukuyomiTarget = null;
                return true;
            }

            var fallback = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && (m.Id == CardId.ThousandEyesRestrict || m.Id == CardId.MagicianOfFaith || m.Id == CardId.GravekeepersSpy))
                           ?? Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();

            if (fallback != null)
            {
                AI.SelectCard(fallback);
                return true;
            }
            return false;
        }

        private bool MobiusTributeSummon()
        {
            // Only summon if opponent has 1+ Spells/Traps to pop
            if (Enemy.GetSpellCount() == 0) return false;

            var tributeFodder = GetTributeFodder();
            if (tributeFodder != null)
            {
                AI.SelectCard(tributeFodder);
                return true;
            }
            return false;
        }

        private bool MobiusEffect()
        {
            // Select up to 2 opponent Spells/Traps
            var targets = Enemy.GetSpells().Where(s => s != null).OrderByDescending(s => s.IsFaceup() ? 10 : 5).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool ZaborgTributeSummon()
        {
            // Only summon if opponent has 1+ monsters to destroy (mandatory effect!)
            if (Enemy.GetMonsterCount() == 0) return false;

            var tributeFodder = GetTributeFodder();
            if (tributeFodder != null)
            {
                AI.SelectCard(tributeFodder);
                return true;
            }
            return false;
        }

        private bool ZaborgEffect()
        {
            var target = Enemy.GetMonsters().Where(m => m != null && !IsTargetImmune(m)).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private ClientCard GetTributeFodder()
        {
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return null;

            // Preferred fodder: Token > Sinister Serpent > face-up Spy > face-up MoF > Sangan > Mother Grizzly
            return monsters.OrderBy(m => GetFodderPriority(m)).FirstOrDefault();
        }

        private int GetFodderPriority(ClientCard m)
        {
            if (m.Id == CardId.SheepToken) return 1;
            if (m.Id == CardId.SinisterSerpent) return 2;
            if (m.Id == CardId.MagicianOfFaith && m.IsFaceup()) return 3;
            if (m.Id == CardId.GravekeepersSpy && m.IsFaceup()) return 4;
            if (m.Id == CardId.Sangan) return 5;
            if (m.Id == CardId.MotherGrizzly) return 6;
            if (m.Id == CardId.YomiShip) return 7;
            if (m.Id == CardId.DdAssailant) return 8;
            return 999; // Never tribute Monarchs or TER
        }

        private bool TribeInfectingVirusSummon()
        {
            // Summon if we have Sinister Serpent in hand, or opponent has 2+ monsters
            return Bot.HasInHand(CardId.SinisterSerpent) || Enemy.GetMonsterCount() >= 2;
        }

        private bool TribeInfectingVirusEffect()
        {
            var enemyFaceup = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyFaceup.Count == 0) return false;

            // Group by race to find most impactful wipe
            var bestGroup = enemyFaceup.GroupBy(m => m.Race).OrderByDescending(g => g.Count()).FirstOrDefault();
            if (bestGroup == null) return false;

            AI.SelectRace((CardRace)bestGroup.Key);

            // Discard Sinister Serpent if possible
            if (Bot.HasInHand(CardId.SinisterSerpent))
            {
                AI.SelectCard(CardId.SinisterSerpent);
            }
            return true;
        }

        private bool BreakerSummon()
        {
            return true;
        }

        private bool BreakerEffect()
        {
            var target = Enemy.GetSpells().FirstOrDefault(s => s != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DdAssailantSummon()
        {
            return true;
        }

        private bool DdAssailantEffect()
        {
            // Activate if battling monster has ATK >= 1500 or is in defense
            if (Enemy.BattlingMonster != null && (Enemy.BattlingMonster.Attack >= 1500 || Enemy.BattlingMonster.IsDefense() || IsAceCard(Enemy.BattlingMonster)))
                return true;
            return true;
        }

        private bool SasukeSummon()
        {
            return true;
        }

        private bool KycooSummon()
        {
            return true;
        }

        private bool KycooEffect()
        {
            var targets = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).Take(2).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool DesWombatSummon()
        {
            return true;
        }

        private bool MotherGrizzlySummon()
        {
            // Only normal summon if we need field presence or have no other starter
            return Bot.GetMonsterCount() == 0;
        }

        private bool MotherGrizzlyEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool YomiShipSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool YomiShipEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool GravekeepersSpyEffect()
        {
            return true;
        }

        private bool MagicianOfFaithEffect()
        {
            // Priority: Pot > Charity > Duo > Snatch > Storm > Metamorphosis > Book of Moon
            var target = Bot.Graveyard.Where(c => c != null && c.IsSpell())
                .OrderBy(c => GetSpellRecursionPriority(c.Id))
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private int GetSpellRecursionPriority(int cardId)
        {
            if (cardId == CardId.PotOfGreed) return 1;
            if (cardId == CardId.GracefulCharity) return 2;
            if (cardId == CardId.DelinquentDuo) return 3;
            if (cardId == CardId.SnatchSteal) return 4;
            if (cardId == CardId.HeavyStorm) return 5;
            if (cardId == CardId.Metamorphosis) return 6;
            if (cardId == CardId.BookOfMoon) return 7;
            if (cardId == CardId.PrematureBurial) return 8;
            return 99;
        }

        private bool SanganEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool BookOfMoonEffect()
        {
            // Case 1: Interrupt opponent attack
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                var attacker = Enemy.BattlingMonster ?? Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500 && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (attacker != null)
                {
                    AI.SelectCard(attacker);
                    return true;
                }
            }

            // Case 2: Reset our equipped TER to suck again
            var equippedTer = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.ThousandEyesRestrict && m.EquipCards != null && m.EquipCards.Count > 0);
            if (equippedTer != null && Duel.Player == 0)
            {
                AI.SelectCard(equippedTer);
                return true;
            }

            // Case 3: Flip enemy high ATK beater face-down to run over
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                int ourMaxAtk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Select(m => m.Attack).DefaultIfEmpty(0).Max();
                var enemyTarget = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack >= 1800 && m.Defense <= ourMaxAtk && !IsTargetImmune(m));
                if (enemyTarget != null)
                {
                    AI.SelectCard(enemyTarget);
                    return true;
                }
            }

            return false;
        }

        private bool ScapegoatEffect()
        {
            if (Duel.Player == 0) return false; // Never activate on our turn (blocks summons)
            // Activate on opponent attack or End Phase
            return Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.End;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool TorrentialTributeEffect()
        {
            // Activate if opponent summoned and controls 2+ monsters or a boss >= 2000 ATK
            int enemyMonsters = Enemy.GetMonsterCount();
            var lastSummoned = LastChainCard;
            bool enemyBoss = lastSummoned != null && lastSummoned.Controller == 1 && lastSummoned.Attack >= 2000;

            // Don't blow up our own Monarch / TER
            bool weHaveBoss = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (weHaveBoss && !enemyBoss) return false;

            return enemyMonsters >= 2 || enemyBoss;
        }

        private bool RingOfDestructionEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null && target.Attack >= 1500)
            {
                // Never suicide!
                if (target.Attack >= Bot.LifePoints) return false;

                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SakuretsuArmorEffect()
        {
            return DefaultUniqueTrap();
        }

        private bool BottomlessTrapHoleEffect()
        {
            return DefaultUniqueTrap();
        }

        private bool SolemnJudgmentEffect()
        {
            // Never activate if LP is critical (<= 1000)
            if (Bot.LifePoints <= 1000) return false;

            // Negate field wipes or enemy boss summons
            if (LastChainCard != null)
            {
                if (LastChainCard.Id == CardId.HeavyStorm || LastChainCard.Id == CardId.SnatchSteal || LastChainCard.Id == CardId.DelinquentDuo)
                    return true;
            }

            return Duel.LastSummonPlayer == 1;
        }

        private bool TrapDustshootEffect()
        {
            return Duel.Player == 1 && Duel.Phase == DuelPhase.Standby && Enemy.Hand.Count >= 4;
        }

        private bool HornOfHeavenEffect()
        {
            var tribute = Bot.GetMonsters().FirstOrDefault(m => m != null && (m.Id == CardId.SheepToken || m.Id == CardId.SinisterSerpent));
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool MonsterRepos()
        {
            // Tokens always in DEF
            if (Card.Id == CardId.SheepToken)
                return Card.IsAttack();

            // Spy with 2000 DEF in DEF
            if (Card.Id == CardId.GravekeepersSpy)
                return Card.IsAttack();

            // TER with 0 ATK and no equip in DEF
            if (Card.Id == CardId.ThousandEyesRestrict && (Card.EquipCards == null || Card.EquipCards.Count == 0))
                return Card.IsAttack();

            // Attack with beaters
            if (Card.Attack >= 1600 && Card.IsDefense())
                return true;

            return DefaultMonsterRepos();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Search / Add to Hand (Sangan / Magician of Faith)
            if (hint == 506 /* HINTMSG_ATOHAND */ || (hint == 505 /* HINTMSG_RTOHAND */ && cards.All(c => c.Controller == 0)))
            {
                var target = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 2. Special Summon (Mother Grizzly / Call of the Haunted / Premature Burial)
            if (hint == 509 /* HINTMSG_SPSUMMON */)
            {
                var target = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 3. Discard Cost (Graceful Charity / Tribe-Infecting Virus)
            if (hint == 501 /* HINTMSG_DISCARD */)
            {
                if (min == 1)
                {
                    var target = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (target != null)
                        return new List<ClientCard> { target };
                }
                else
                {
                    var sorted = Plugin.MaterialImpl.SortMaterials(cards, min);
                    if (sorted != null && sorted.Count >= min)
                        return sorted.Take(min).ToList();
                }
            }

            // 4. Tribute / Release Cost (Monarchs / Horn of Heaven / Metamorphosis)
            if (hint == 500 /* HINTMSG_RELEASE */)
            {
                var sorted = Plugin.MaterialImpl.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                    return sorted.Take(min).ToList();
            }

            // 5. Removal Targets (Mobius / Zaborg / Ring / Tribe / Assailant)
            if (hint == 502 /* HINTMSG_DESTROY */ || hint == 503 /* HINTMSG_REMOVE */)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    var sorted = enemyTargets.OrderByDescending(c => GetCardThreatScore(c, hint)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
