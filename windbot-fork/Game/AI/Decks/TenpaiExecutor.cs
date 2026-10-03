// ============================================================================
// CARD AUDIT — Tenpai (Tenpai Dragon — The Ultimate Going-Second OTK God)
// ============================================================================
// | Card Name                      | Level/Type   | OPT? | HOPT? | Cost     | Real Effect Summary & Tactics                         | Activate When                                   | NEVER Activate When                         |
// |--------------------------------|--------------|------|-------|----------|-------------------------------------------------------|-------------------------------------------------|---------------------------------------------|
// | Tenpai Dragon Paidra           | L3 Fire Drag | Yes  | Yes   | None     | On NS/SS: Search Sangen S/T; avoid FIRE Drag b-damage | MP1: Search Sangen Summoning/Kaimen; BP: Synchro| Already used this turn                      |
// | Tenpai Dragon Chundra          | L4 Fire Tuner| Yes  | Yes   | None     | SS from hand; on battle SS L4- FIRE Dragon from Deck  | MP1: SS self; BP: SS Fadra from Deck & Synchro  | Already used this turn                      |
// | Tenpai Dragon Fadra            | L3 Fire Drag | Yes  | Yes   | None     | FIRE Dragon b-indestructible; NS/SS/Battle revive GY  | BP: Revive Paidra/Chundra from GY & Synchro     | Already used this turn                      |
// | Tenpai Dragon Genroku          | L3 Fire Drag | Yes  | Yes   | Tribute  | If added to hand SS self (tuner L3/L4); tribute->deck | MP/BP: Tribute self -> SS Chundra from Deck     | Already used this turn                      |
// | Sangen Summoning (Field)       | Spell Field  | Yes  | Yes   | Discard  | MP1: FIRE Dragons UNAFFECTED; Search Tenpai + discard | MP1: Blanket immunity + search Chundra          | Already active on field                     |
// |                                | (When pop BP)| Yes  | Yes   | None     | Destroyed in BP: TARGET DRAGON SYNCHRO -> DOUBLE ATK! | BP: Trident pops this -> Trident ATK = 6000!   | Not in Battle Phase                         |
// | Sangen Kaimen                  | Spell Quick  | Yes  | Yes   | None     | MP: Search or SS FIRE Drag; BP: Search AND SS!        | MP1: Search dragon; BP: Search & swarm for OTK  | Already used this turn                      |
// | Sangen Furo                    | Spell Cont   | Yes  | No    | None     | Revive monster destroyed in dragon battle; EP GY set  | Battle Phase: revive fallen dragon for extra atk| Already active                              |
// | Sangen Kaiho                   | Trap Normal  | Yes  | Yes   | None     | If opp has more monsters, skip opp MP1 to BP; GY draw | Opp MP1: End their turn; GY: banish draw & SS   | We control equal/more monsters              |
// | Sangenpai Bident Dragion       | L7 Fire Tuner| Yes  | Yes   | Target   | On Synchro: Revive FIRE Drag; GY: 3+ atks -> SS + pop | BP: 4+3=7 Synchro -> revive Paidra (L3) -> 7+3  | Turn 1 if no follow-up                      |
// | Sangenpai Transcendent Dragion | L10 Fire Drag| Yes  | Yes   | None     | All to ATK; opp MUST attack; OPP SILENT IN BP; GY pop | BP: 3000 ATK lockdown; Turn 1: 3000 DEF wall    | Opponent already locked                     |
// | Trident Dragion                | L10 Fire Drag| Yes  | No    | Pop 1-2  | Synchro: Pop 1-2 friendly cards -> 2-3 attacks!       | BP OTK: Pop Summoning -> 6000 ATK x 3 = 18,000! | TURN 1 (NEVER SUMMON ON TURN 1!)            |
// | Hieratic Seal of Heav. Spheres | Link-2 Drag  | Yes  | Yes   | Tribute  | Quick: Tribute 1 -> bounce 1 faceup card + SS 0/0 Drag| Going 1st: Endboard interaction + float Paidra  | Battle Phase Going Second                   |
// | Black Rose Dragon              | L7 Fire Drag | Yes  | No    | None     | On Synchro Summon: Destroy ALL cards on the field!    | MP1 Going 2nd: Emergency board wipe (7=4+3)     | Our board is already winning                |
// | Moonlight Rose Dragon          | L7 Light Drag| Yes  | No    | Target   | On Summon / opp L5+ SS: Bounce 1 opp monster to hand  | Removal against opp high level boss monsters    | No valid opponent targets                   |
// | Kuibelt the Blade Dragon       | L7 Dark Drag | Yes  | No    | Target   | On Synchro Summon: Target 1 card on field; destroy it  | Removal target during BP ladder                 | No valid opponent targets                   |
// | Bystial Dis Pater              | L10 Dark Drag| Yes  | Yes   | Target   | Target banished card: SS to field or negate monster   | Going 1st endboard / Turn 3 grind               | No banished cards available                 |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Tenpai", "Tenpai")]
    public class TenpaiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int TenpaiDragonPaidra = 39931513;   // Level 3 FIRE Dragon (1700/1000)
            public const int TenpaiDragonChundra = 91810826;  // Level 4 FIRE Dragon Tuner (1500/1000)
            public const int TenpaiDragonFadra = 65326118;    // Level 3 FIRE Dragon (1600/1000)
            public const int TenpaiDragonGenroku = 23657016;  // Level 3 FIRE Dragon (0/1000)

            // Spells & Traps
            public const int SangenSummoning = 30336082;      // Field Spell (Blanket MP1 immunity + search + BP double ATK)
            public const int SangenKaimen = 66730191;         // Quick-Play Spell (Search or SS; BP: both!)
            public const int SangenFuro = 55484152;           // Continuous Spell
            public const int SangenKaiho = 25388971;          // Normal Trap (Skip opp MP1 + GY draw/SS)
            public const int PotOfProsperity = 84211599;      // Normal Spell
            public const int CalledByTheGrave = 24224830;     // Quick-Play Spell
            public const int LightningStorm = 14532163;       // Normal Spell
            public const int SuperPolymerization = 48130397;  // Quick-Play Spell
            public const int DarkRulerNoMore = 54693926;      // Normal Spell
            public const int ForbiddenDroplet = 24299458;     // Quick-Play Spell
            public const int AshBlossom = 14558127;           // Level 3 Zombie Tuner Handtrap
            public const int InfiniteImpermanence = 10045474; // Normal Trap Handtrap
            public const int GhostBelle = 73642296;           // Level 3 Zombie Tuner Handtrap
            public const int Nibiru = 27204311;               // Level 11 Rock Handtrap

            // Extra Deck
            public const int SangenpaiBidentDragion = 82570174;         // Level 7 FIRE Dragon Tuner Synchro (2600/2000)
            public const int SangenpaiTranscendentDragion = 18969888;   // Level 10 FIRE Dragon Synchro (3000/3000)
            public const int TridentDragion = 39402797;                 // Level 10 FIRE Dragon Synchro (3000/2800)
            public const int BlackRoseDragon = 73580471;                // Level 7 FIRE Dragon Synchro (2400/1800)
            public const int MoonlightRoseDragon = 33698022;            // Level 7 LIGHT Dragon Synchro (2400/2000)
            public const int KuibeltTheBladeDragon = 87837090;          // Level 7 DARK Dragon Synchro (2500/1900)
            public const int BystialDisPater = 27572350;                // Level 10 DARK Dragon Synchro (3500/3500)
            public const int HieraticSeal = 24361622;                   // Link-2 Dragon (0 ATK)
            public const int Garura = 11765832;                         // Level 6 Winged Beast Fusion (1500/2400)
            public const int Mudragon = 54757758;                       // Level 4 Wyrm Fusion (1900/1600)
            public const int EarthGolem = 62111090;                     // Level 4 Cyberse Fusion (2300/2000)
            public const int StarvingVenom = 41209827;                  // Level 8 Dragon Fusion (2800/2000)
        }

        public TenpaiPlugin Plugin { get; }

        private bool _summoningSearchUsed = false;
        private bool _paidraSearchUsed = false;
        private bool _kaimenUsed = false;
        private bool _chundraDeckSpUsed = false;
        private bool _fadraGyReviveUsed = false;
        private int _attacksDeclaredCount = 0;

        public TenpaiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new TenpaiPlugin(this);
            DeckPlugin = Plugin;

            // BaitPlanner registration
            BaitPlanner.RegisterComboStarters(CardId.TenpaiDragonPaidra, CardId.SangenSummoning, CardId.SangenKaimen);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity, CardId.DarkRulerNoMore, CardId.LightningStorm, CardId.ForbiddenDroplet);

            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _summoningSearchUsed = false;
            _paidraSearchUsed = false;
            _kaimenUsed = false;
            _chundraDeckSpUsed = false;
            _fadraGyReviveUsed = false;
            _attacksDeclaredCount = 0;
            Plugin.ResetTurnState();
        }

        private bool IsGoingFirstTurn()
        {
            return Duel.Turn == 1 && Duel.Player == 0;
        }

        private void RegisterComboLines()
        {
            // ── Going Second OTK Main Route: Paidra -> Summoning -> Chundra -> Battle OTK ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-OTK-Paidra-Route",
                RequiredCards = new List<int> { CardId.TenpaiDragonPaidra },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TenpaiDragonPaidra, ActionType = ExecutorType.Summon, Description = "Normal Summon Paidra" },
                    new() { CardId = CardId.TenpaiDragonPaidra, ActionType = ExecutorType.Activate, Description = "Paidra searches Sangen Summoning (Field Spell)" },
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Activate Sangen Summoning (Blanket MP1 Immunity)" },
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Sangen Summoning searches Chundra, discards fodder" },
                    new() { CardId = CardId.TenpaiDragonChundra, ActionType = ExecutorType.Activate, Description = "Special Summon Chundra from hand" }
                },
                FallbackLineName = "Tenpai-Field-Starter"
            });

            // ── Line 2: Sangen Summoning Field Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-Field-Starter",
                RequiredCards = new List<int> { CardId.SangenSummoning },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Activate Sangen Summoning" },
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Search Chundra or Paidra, discard fodder" }
                },
                FallbackLineName = "Tenpai-Kaimen-Starter"
            });

            // ── Line 3: Sangen Kaimen Quick-Play Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-Kaimen-Starter",
                RequiredCards = new List<int> { CardId.SangenKaimen },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SangenKaimen, ActionType = ExecutorType.Activate, Description = "Activate Sangen Kaimen to search/SS Paidra or Chundra" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, DefaultNibiru);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS (MAIN PHASE 1 ONLY)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: FIELD SPELLS, SEARCHERS & SPELL CARDS
            // ═══════════════════════════════════════════════════════════════
            // Sangen Summoning: In Hand (activate field) -> In Field (search effect) -> In GY (double ATK)
            AddExecutor(ExecutorType.Activate, CardId.SangenSummoning, SangenSummoningActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenKaimen, SangenKaimenActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenFuro, SangenFuroActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenKaiho, SangenKaihoActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: TENPAI MONSTERS & EFFECTS
            // ═══════════════════════════════════════════════════════════════
            // Genroku: In Hand (SS self as Tuner) / On Field (Tribute to SS Chundra from Deck)
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonGenroku, TenpaiGenrokuActivate);

            // Paidra: Normal Summon -> Search -> Battle Phase Quick Synchro
            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonPaidra, PaidraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonPaidra, PaidraEffect);

            // Chundra: Special Summon from Hand -> Battle Trigger -> Quick Synchro
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonChundra, ChundraEffect);
            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonChundra, ChundraSummon);

            // Fadra: Normal Summon -> Revive FIRE Dragon from GY -> Quick Synchro
            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonFadra, FadraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonFadra, FadraEffect);

            // Fallback summon for any remaining body
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHROS & LINKS (OTK LADDER & GOING 1ST)
            // ═══════════════════════════════════════════════════════════════
            // Going 1st Interruption: Hieratic Seal of the Heavenly Spheres (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.HieraticSeal, HieraticSealSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HieraticSeal, HieraticSealEffect);

            // Emergency Level 7 Board Wipe: Black Rose Dragon (4+3=7)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseEffect);

            // Level 7 Bridge & Enabler: Sangenpai Bident Dragion (Tuner, 2600 ATK)
            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiBidentDragion, BidentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiBidentDragion, BidentDragionEffect);

            // Level 10 OTK Finisher: Trident Dragion (3000 ATK -> 6000 ATK x 3 attacks!)
            AddExecutor(ExecutorType.SpSummon, CardId.TridentDragion, TridentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TridentDragion, TridentDragionEffect);

            // Level 10 Battle Lockdown: Sangenpai Transcendent Dragion (Opponent Silent in BP)
            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiTranscendentDragion, TranscendentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiTranscendentDragion, TranscendentDragionEffect);

            // Level 7 Utility Synchros
            AddExecutor(ExecutorType.SpSummon, CardId.MoonlightRoseDragon, MoonlightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MoonlightRoseDragon, MoonlightEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.KuibeltTheBladeDragon, KuibeltSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KuibeltTheBladeDragon, KuibeltEffect);

            // Level 10 Boss: Bystial Dis Pater
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, DisPaterEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: SPELL SETS & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.SangenKaiho, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SangenKaimen, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        private bool IsChainAlreadyNeutralized()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return false;
        }

        private bool AshBlossomActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool GhostBelleActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool ImpermanenceActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Name == lastCard.Name);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.Graveyard.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenDropletActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            var oppTargets = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget()).ToList();
            if (oppTargets.Count == 0) return false;

            // Pick discard fodder via MaterialEvaluator
            var availableFodder = Bot.Hand.Concat(Bot.GetSpells())
                .Where(c => c != Card && c.Id != CardId.SangenSummoning && c.Id != CardId.TenpaiDragonPaidra && c.Id != CardId.TenpaiDragonChundra)
                .ToList();

            if (availableFodder.Count == 0) return false;

            var chosenFodder = availableFodder.Take(Math.Min(oppTargets.Count, availableFodder.Count)).ToList();
            AI.SelectCard(chosenFodder);
            AI.SelectNextCard(oppTargets);
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BOARD BREAKERS (GOING SECOND)
        // ═══════════════════════════════════════════════════════════════

        private bool DarkRulerNoMoreActivate()
        {
            if (IsGoingFirstTurn()) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled());
        }

        private bool LightningStormActivate()
        {
            if (IsGoingFirstTurn()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;

            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // Destroy Spells/Traps
                return true;
            }
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(0); // Destroy Attack Position Monsters
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationActivate()
        {
            if (Bot.Hand.Count < 2) return false; // Requires 1 discard
            var enemyMonsters = Enemy.GetMonsters().Where(m => m.IsFaceup()).ToList();
            if (enemyMonsters.Count < 2) return false;

            // Discard fodder
            var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(Bot.Hand.Where(c => c != Card).ToList());
            if (discardTarget != null)
            {
                AI.SelectCard(discardTarget);
            }
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            // Banish 3 or 6 non-essential Extra Deck cards, strictly preserving Trident, Bident, and Transcendent
            var safeBanish = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.TridentDragion &&
                c.Id != CardId.SangenpaiBidentDragion &&
                c.Id != CardId.SangenpaiTranscendentDragion
            ).Take(6).ToList();

            if (safeBanish.Count >= 3)
            {
                AI.SelectCard(safeBanish);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  FIELD SPELLS & SEARCHERS
        // ═══════════════════════════════════════════════════════════════

        private bool SangenSummoningActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Step 1: Activate Field Spell from Hand (Top priority in MP1 for blanket immunity!)
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.SangenSummoning);
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                // Step 2: Ignition effect on field: Search 1 Tenpai Monster, then discard 1 card
                if (_summoningSearchUsed) return false;
                _summoningSearchUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Step 3: Triggered when destroyed during Battle Phase: Target 1 Dragon Synchro -> Double ATK!
                // Target Trident Dragion if present (3000 -> 6000 ATK!)
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.TridentDragion)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.HasRace(CardRace.Dragon));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        private bool SangenKaimenActivate()
        {
            if (_kaimenUsed) return false;
            _kaimenUsed = true;

            if (Duel.Phase == DuelPhase.Battle)
            {
                // In Battle Phase, Sangen Kaimen can do BOTH: Add to Hand AND Special Summon!
                return true;
            }
            else
            {
                // In Main Phase, choose Option 0: Add 1 Level 4 or lower FIRE Dragon to hand
                AI.SelectOption(0);
                return true;
            }
        }

        private bool SangenFuroActivate()
        {
            return true;
        }

        private bool SangenKaihoActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Trap activation: In Main Phase, if opponent controls more monsters than us -> Skip their MP1!
                if (Duel.Player == 1 && Duel.IsMainPhase())
                {
                    return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
                }
                return false;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY activation: If 3+ attacks declared, banish self -> draw 1 and SS Tenpai dragons from hand!
                return _attacksDeclaredCount >= 3;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TENPAI MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool TenpaiGenrokuActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Added to hand (e.g. searched by Summoning/Kaimen) -> Special Summon self as Tuner!
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On field: Quick effect: Tribute self to Special Summon Chundra (Tuner) or Paidra from Deck!
                // Prioritize Chundra if we need a Tuner for Level 7 Synchro (4+3=7)
                if (!Bot.HasInMonstersZone(CardId.TenpaiDragonChundra) && !Bot.HasInHand(CardId.TenpaiDragonChundra))
                {
                    AI.SelectCard(CardId.TenpaiDragonChundra);
                }
                else
                {
                    AI.SelectCard(CardId.TenpaiDragonPaidra, CardId.TenpaiDragonFadra);
                }
                return true;
            }
            return true;
        }

        private bool PaidraSummon()
        {
            return true;
        }

        private bool PaidraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle)
                {
                    // Battle Phase Quick Synchro:
                    // Paidra (3 non-tuner) tunes with Chundra (4 Tuner) -> Bident Dragion (7)!
                    // Or Paidra (3 non-tuner) tunes with Bident Dragion (7 Tuner) -> Trident Dragion (10)!
                    return true;
                }
                else
                {
                    // On Summon search: Search Sangen Summoning (Field Spell) > Sangen Kaimen > Sangen Kaiho
                    if (_paidraSearchUsed) return false;
                    _paidraSearchUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool ChundraSummon()
        {
            return true;
        }

        private bool ChundraEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // In hand: Special Summon self if we control a FIRE Dragon
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Fire) && m.HasRace(CardRace.Dragon));
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle)
                {
                    // Trigger at Battle Start: Special Summon Level 4 or lower FIRE Dragon from Deck (Fadra)!
                    // Fadra will then trigger to revive Paidra from GY!
                    if (!_chundraDeckSpUsed)
                    {
                        _chundraDeckSpUsed = true;
                        AI.SelectCard(CardId.TenpaiDragonFadra, CardId.TenpaiDragonPaidra);
                        return true;
                    }
                    // Quick Synchro during Battle Phase
                    return true;
                }
            }
            return true;
        }

        private bool FadraSummon()
        {
            return true;
        }

        private bool FadraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle)
                {
                    // Trigger at Battle Start or on Summon: Revive FIRE Dragon from GY
                    if (!_fadraGyReviveUsed)
                    {
                        _fadraGyReviveUsed = true;
                        ClientCard revTarget = Bot.Graveyard.Where(c => c.IsMonster() && c.HasAttribute(CardAttribute.Fire) && c.HasRace(CardRace.Dragon))
                            .OrderByDescending(c => c.Attack)
                            .FirstOrDefault();
                        if (revTarget != null)
                        {
                            AI.SelectCard(revTarget);
                            return true;
                        }
                    }
                    // Quick Synchro during Battle Phase
                    return true;
                }
                else
                {
                    // On Normal/Special Summon in Main Phase: Revive FIRE Dragon from GY
                    ClientCard revTarget = Bot.Graveyard.Where(c => c.IsMonster() && c.HasAttribute(CardAttribute.Fire) && c.HasRace(CardRace.Dragon))
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();
                    if (revTarget != null)
                    {
                        AI.SelectCard(revTarget);
                        return true;
                    }
                }
            }
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS & LINKS (OTK LADDER & GOING 1ST)
        // ═══════════════════════════════════════════════════════════════

        private bool HieraticSealSpSummon()
        {
            // Link-2 Dragon: Only summon when Going First or Main Phase 2 to set up an interruption!
            // Requires 2 Dragon monsters. Do NOT summon if we can OTK in Battle Phase!
            if (Duel.Turn == 1 && Duel.Player == 0)
            {
                return Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Dragon)) >= 2;
            }
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Dragon)) >= 2;
        }

        private bool HieraticSealEffect()
        {
            // Opponent Turn: Tribute self to bounce 1 faceup card on field
            if (Duel.Player == 1)
            {
                ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: true, canBeTarget: true);
                if (oppTarget != null)
                {
                    AI.SelectCard(Card); // Tribute self
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            // When tributed: Special Summon 1 Dragon from Deck with 0 ATK/DEF (Paidra / Genroku)
            AI.SelectCard(CardId.TenpaiDragonPaidra, CardId.TenpaiDragonGenroku, CardId.TenpaiDragonFadra);
            return true;
        }

        private bool BlackRoseSpSummon()
        {
            // Level 7 FIRE Dragon Synchro: 4 (Chundra) + 3 (Paidra/Fadra) = 7!
            // Emergency board wipe when going second against 3+ opponent cards
            if (!IsGoingFirstTurn() && Duel.Phase == DuelPhase.Main1)
            {
                return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 3;
            }
            return false;
        }

        private bool BlackRoseEffect()
        {
            // Destroy all cards on the field!
            return true;
        }

        private bool BidentDragionSpSummon()
        {
            // Level 7 FIRE Dragon Tuner Synchro: 4 (Chundra Tuner) + 3 (Paidra/Fadra) = 7!
            // Core ladder bridge for both Going 1st and Going 2nd
            return true;
        }

        private bool BidentDragionEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On Synchro Summon: Revive Paidra (Level 3 non-tuner) to immediately enable 7 + 3 = 10 Synchro!
                ClientCard paidra = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TenpaiDragonPaidra)
                                 ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TenpaiDragonFadra)
                                 ?? Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasAttribute(CardAttribute.Fire));
                if (paidra != null)
                {
                    AI.SelectCard(paidra);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY Quick Effect: After 3+ attacks declared, Special Summon self and destroy 1 opponent Spell/Trap!
                if (_attacksDeclaredCount >= 3)
                {
                    ClientCard backrow = Enemy.GetSpells().FirstOrDefault(s => !s.IsShouldNotBeTarget());
                    if (backrow != null)
                    {
                        AI.SelectCard(backrow);
                    }
                    return true;
                }
            }
            return true;
        }

        private bool TridentDragionSpSummon()
        {
            // Level 10 FIRE Dragon Synchro: 7 (Bident Dragion Tuner) + 3 (Paidra non-tuner) = 10!
            // STRICT RULE: NEVER SUMMON TRIDENT DRAGION ON TURN 1!
            if (IsGoingFirstTurn()) return false;

            // Summon in Battle Phase (or Main Phase 1 if going second for lethal)
            return Duel.Phase == DuelPhase.Battle || (!IsGoingFirstTurn() && Duel.Phase == DuelPhase.Main1);
        }

        private bool TridentDragionEffect()
        {
            // On Synchro Summon: Destroy up to 2 friendly cards to attack 3 times!
            // CRITICAL: TARGET SANGEN SUMMONING (Field Spell) FIRST!
            // Destroying Sangen Summoning in Battle Phase triggers its effect to DOUBLE Trident's ATK to 6000!
            // 6000 ATK x 3 attacks = 18,000 DAMAGE!
            var popTargets = new List<ClientCard>();

            ClientCard fieldSpell = Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.SangenSummoning);
            if (fieldSpell != null)
            {
                popTargets.Add(fieldSpell);
            }

            // Second target: Sangen Furo, or already-attacked monster (Fadra/Paidra)
            ClientCard secondTarget = Bot.GetSpells().FirstOrDefault(s => s != null && s != fieldSpell && s.Id == CardId.SangenFuro)
                                   ?? Bot.GetMonsters().FirstOrDefault(m => m != Card && m.Id != CardId.SangenpaiBidentDragion && m.Id != CardId.SangenpaiTranscendentDragion);

            if (secondTarget != null)
            {
                popTargets.Add(secondTarget);
            }

            if (popTargets.Count > 0)
            {
                AI.SelectCard(popTargets);
                return true;
            }
            return true;
        }

        private bool TranscendentDragionSpSummon()
        {
            // Level 10 FIRE Dragon Synchro: 7 (Bident Dragion) + 3 (Paidra/Fadra) = 10!
            // Great for both Turn 1 (3000 DEF wall + force attack) and Battle Phase (Complete Lockdown)
            return true;
        }

        private bool TranscendentDragionEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On Synchro Summon: Change all monsters on field to face-up Attack Position!
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY Quick Effect: After 3+ attacks declared, Special Summon self and destroy 1 card on field!
                if (_attacksDeclaredCount >= 3)
                {
                    ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: false, canBeTarget: true);
                    if (oppTarget != null)
                    {
                        AI.SelectCard(oppTarget);
                    }
                    return true;
                }
            }
            return true;
        }

        private bool MoonlightSpSummon()
        {
            // Level 7 LIGHT Dragon Synchro: Bounce opponent Level 5+ monster
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Level >= 5);
        }

        private bool MoonlightEffect()
        {
            ClientCard oppTarget = Enemy.GetMonsters().Where(m => m.IsFaceup() && m.Level >= 5).OrderByDescending(m => Scorer.ThreatScore(m)).FirstOrDefault()
                                ?? GetBestMonsterRemovalTarget(onlyFaceup: true, canBeTarget: true);
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool KuibeltSpSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool KuibeltEffect()
        {
            ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: false, canBeTarget: true);
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool DisPaterSpSummon()
        {
            // Level 10 DARK Dragon Synchro: Great for Turn 1 endboard or grind game
            return IsGoingFirstTurn() || Duel.Phase == DuelPhase.Main2;
        }

        private bool DisPaterEffect()
        {
            ClientCard oppBanish = Enemy.Banished.FirstOrDefault(c => c.IsMonster());
            if (oppBanish != null)
            {
                AI.SelectCard(oppBanish);
                return true;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS, SELECTION & CALLBACKS
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                // Quick-play spells set in MP2, or going first in MP1
                return Duel.Phase == DuelPhase.Main2 || IsGoingFirstTurn();
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card == null) return false;

            // 1. Handtraps or Low ATK walls stranded in Attack -> Switch to Defense!
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
            {
                return true;
            }

            // 2. High ATK Tenpai dragons in Defense -> Switch to Attack for OTK
            if (Card.IsDefense() && Card.Attack >= 1500 && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
            {
                return true;
            }

            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // All Tenpai attacking dragons -> Strictly FaceUpAttack
            int[] forceAttackDragons = {
                CardId.TenpaiDragonPaidra,
                CardId.TenpaiDragonChundra,
                CardId.TenpaiDragonFadra,
                CardId.SangenpaiBidentDragion,
                CardId.SangenpaiTranscendentDragion,
                CardId.TridentDragion,
                CardId.BlackRoseDragon,
                CardId.BystialDisPater,
                CardId.KuibeltTheBladeDragon,
                CardId.MoonlightRoseDragon
            };

            if (forceAttackDragons.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            // Genroku (0/0) & Handtraps -> Strictly Defense
            if (cardId == CardId.TenpaiDragonGenroku || CardIntelligence.IsHandtrap(cardId))
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);

            // Lightning Storm
            if (Card != null && Card.Id == CardId.LightningStorm)
            {
                if (Enemy.GetSpellCount() >= 2) return 1; // Destroy Spells/Traps
                return 0; // Destroy Monsters
            }

            // Sangen Kaimen in Main Phase: Option 0 = Search to hand
            if (Card != null && Card.Id == CardId.SangenKaimen)
            {
                return 0;
            }

            return base.OnSelectOption(options);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Sangen Summoning (Field Spell destroyed in Battle Phase): Double ATK of Dragon Synchro? YES!
            if (desc == Util.GetStringId(CardId.SangenSummoning, 1))
            {
                return true;
            }

            // Sangen Kaimen in Battle Phase:
            // desc 3: Add 1 FIRE Dragon to hand? -> YES!
            // desc 4: Special Summon 1 FIRE Dragon from hand? -> YES!
            if (desc == Util.GetStringId(CardId.SangenKaimen, 3) || desc == Util.GetStringId(CardId.SangenKaimen, 4))
            {
                return true;
            }

            // Trident Dragion: Destroy cards on field to gain extra attacks? -> YES!
            if (desc == Util.GetStringId(CardId.TridentDragion, 0))
            {
                return true;
            }

            // Genroku: Increase level by 1?
            // If we have another Level 3 monster (Paidra or Fadra) and need Level 7 (4+3=7), say YES to make Genroku Level 4!
            if (desc == Util.GetStringId(CardId.TenpaiDragonGenroku, 2))
            {
                bool hasLevel3 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 3);
                return hasLevel3;
            }

            // Sangenpai Bident Dragion in GY: Destroy 1 Spell/Trap? Only if opponent controls Spells/Traps!
            if (desc == Util.GetStringId(CardId.SangenpaiBidentDragion, 3))
            {
                return Enemy.GetSpellCount() > 0;
            }

            // Sangenpai Transcendent Dragion in GY: Destroy 1 card? Only if opponent controls cards!
            if (desc == Util.GetStringId(CardId.SangenpaiTranscendentDragion, 2))
            {
                return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
            }

            // Sangen Kaiho in GY: Special Summon Tenpai dragons from hand? -> YES!
            if (desc == Util.GetStringId(CardId.SangenKaiho, 2))
            {
                return Bot.Hand.Any(c => c.IsMonster() && (c.Id == CardId.TenpaiDragonPaidra || c.Id == CardId.TenpaiDragonChundra || c.Id == CardId.TenpaiDragonFadra));
            }

            // Black Rose Dragon: Destroy all cards on field?
            if (desc == Util.GetStringId(CardId.BlackRoseDragon, 0))
            {
                return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
            }

            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // 1. Deck Search (Hint 506 = HINTMSG_ATOHAND or candidates all from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                    if (searchTarget != null)
                    {
                        return new List<ClientCard> { searchTarget };
                    }
                }

                // 2. Discard (Hint 501 = HINTMSG_DISCARD or hand discard)
                if (hint == 501 || (cards.All(c => c.Location == CardLocation.Hand) && !cancelable))
                {
                    var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (discardTarget != null)
                    {
                        return new List<ClientCard> { discardTarget };
                    }
                }

                // 3. Destruction / Removal (Hint 502 = HINTMSG_DESTROY, Hint 503 = HINTMSG_REMOVE, Hint 504 = HINTMSG_TOGRAVE)
                if (hint == 502 || hint == 503 || hint == 504)
                {
                    // Trident Dragion popping friendly cards
                    if (Card != null && Card.Id == CardId.TridentDragion && cards.Any(c => c.Controller == 0))
                    {
                        var friendlyCards = cards.Where(c => c.Controller == 0).ToList();
                        var popList = new List<ClientCard>();

                        // Sangen Summoning is TOP PRIORITY to double ATK to 6000!
                        var field = friendlyCards.FirstOrDefault(c => c.Id == CardId.SangenSummoning);
                        if (field != null) popList.Add(field);

                        var other = friendlyCards.Where(c => c != field && c.Id != CardId.TridentDragion).OrderBy(c => Plugin.MaterialImpl.GetMaterialCost(c)).FirstOrDefault();
                        if (other != null) popList.Add(other);

                        if (popList.Count >= min)
                        {
                            return popList.Take(Math.Min(max, popList.Count)).ToList();
                        }
                    }

                    // Enemy Removal: ALWAYS target Enemy cards!
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        var sorted = enemyTargets.OrderByDescending(c => Scorer.ThreatScore(c)).ToList();
                        return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                    }
                }

                // 4. Special Summon (Hint 509 = HINTMSG_SPSUMMON)
                if (hint == 509)
                {
                    var spTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                    if (spTarget != null)
                    {
                        return new List<ClientCard> { spTarget };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            // Track attacks declared this turn for Bident / Transcendent GY triggers (requires >= 3 attacks)
            _attacksDeclaredCount++;
            return base.OnSelectAttackTarget(attacker, defenders);
        }
    }
}
