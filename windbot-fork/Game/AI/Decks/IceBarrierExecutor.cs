// ============================================================================
// CARD AUDIT — IceBarrier (Modernized STR51 Freezing Chains - Terminal World Synchro Lock)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Revealer of the Ice Barrier        | Monster L4   | Yes  | Yes   | Discard | SS Ice Barrier Tuner from Deck; WATER lock    | Main Phase: summon Mirror Mage               | Already used or field full                  |
// | Mirror Mage of the Ice Barrier     | Monster L2 T | Yes  | Yes   | Tribute | Tribute to summon up to 3 tokens; search on GY| Main Phase: generate tokens / Level mod      | Field full                                  |
// | Georgius, Swordman of the Ice Bar. | Monster L6 T | Yes  | Yes   | None    | SS self if IB on field; revive L5- on summon  | Have IB on field; revive Mirror Mage/Revealer| Field full                                  |
// | Speaker for the Ice Barriers       | Monster L4   | Yes  | Yes   | None    | SS self if IB on field; GY banish for token   | Extender from hand; token maker in GY        | Field full                                  |
// | General Wayne of the Ice Barrier   | Monster L5   | Yes  | Yes   | None    | SS self if opp has monster; search IB S/T     | Opponent controls monster; search S/T        | Field full                                  |
// | Prior of the Ice Barrier           | Monster L2   | Yes  | No    | Tribute | SS self from hand; tribute to revive any IB   | Extender or revive high-level IB             | If locked from L5+                          |
// | General Raiho of the Ice Barrier   | Monster L6   | No   | No    | None    | Opponent must discard 1 or monster eff neg.   | Opponent turn: Lancea summons Raiho          | Field full                                  |
// | Medium of the Ice Barrier          | Monster L7   | No   | No    | None    | Opponent can only activate 1 S/T per turn     | Opponent turn: Lancea summons Medium         | Field full                                  |
// | Warlock of the Ice Barrier         | Monster L3   | No   | No    | None    | Both players must set Spells before activating| Opponent turn: Lancea summons Warlock        | Field full                                  |
// | Gameciel, the Sea Turtle Kaiju     | Monster L8   | No   | No    | Tribute | Tributes opponent boss; searchable by IB trap | Opponent has dangerous boss/tower            | Opponent board empty                        |
// | Medallion of the Ice Barrier       | Spell Normal | No   | No    | None    | Search ANY Ice Barrier monster from Deck      | Need Revealer, Mirror Mage, Georgius, etc.   | Deck has no targets                         |
// | Freezing Chains of the Ice Barrier | Spell Contin | Yes  | Yes   | Target  | Revive L4- IB; grant Extra Deck immunity      | Have L4- IB in GY to revive                  | Field full                                  |
// | Winds Over the Ice Barrier         | Spell Normal | Yes  | Yes   | Tribute | Tribute IBs to SS from Deck; GY retrieve      | Need combo pieces from Deck                  | Field empty                                 |
// | Ice Barrier (Trap)                 | Trap Normal  | Yes  | Yes   | None    | Negate attack + 0 ATK; GY banish search Kaiju | Opponent attacks; GY Quick effect in MP      | No targets                                  |
// | Lancea, Ancestral Dragon of Ice M. | Synchro L10  | Yes  | Yes   | None    | Opp SS -> SS IB from Hand/Deck/Extra/GY + pos | Opponent Special Summons; float on leave     | Field full                                  |
// | Trishula, Zero Dragon of Ice Bar.  | Synchro L11  | Yes  | Yes   | None    | On Synchro: Banish up to 3 cards opp controls | Board breaking or Lancea float               | Opponent has no cards                       |
// | Trishula, Dragon of the Ice Barrier| Synchro L9   | No   | No    | None    | Banish 1 from hand, 1 from field, 1 from GY   | Going second or mid-game banish              | Opponent has no cards                       |
// | Icejade Gymir Aegirine             | Synchro L10  | Yes  | Yes   | None    | Quick: Monsters unaffected & cannot be destr. | Opponent activates destruction/removal       | Already protected                           |
// | Adamancipator Risen - Dragite      | Synchro L8   | Yes  | Yes   | None    | Negate Spell/Trap activation if WATER in GY   | Opponent activates Spell/Trap                | No WATER in GY                              |
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
    [Deck("IceBarrier", "IceBarrier")]
    public class IceBarrierExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Revealer = 18319762;
            public const int MirrorMage = 9396662;
            public const int GeorgiusSwordman = 32991027;
            public const int Speaker = 44308317;
            public const int GeneralWayne = 81825063;
            public const int Prior = 50088247;
            public const int GeneralRaiho = 81275309;
            public const int Medium = 88494899;
            public const int Warlock = 18482591;
            public const int GamecielKaiju = 55063751;
            public const int AshBlossom = 14558127;
            public const int Nibiru = 27204311;
            public const int IceBarrierToken = 44308318;

            // Spells & Traps
            public const int Medallion = 84206435;
            public const int FreezingChains = 43582229;
            public const int WindsOver = 17197110;
            public const int FoolishBurialGoods = 35726888;
            public const int FoolishBurial = 81439173;
            public const int TripleTacticsTalent = 25311006;
            public const int CrossoutDesignator = 65681983;
            public const int HarpieFeatherDuster = 18144506;
            public const int IceBarrierTrap = 34293667;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int LanceaAncestralDragon = 96402918;
            public const int TrishulaZeroDragon = 70980824;
            public const int TrishulaDragon = 52687916;
            public const int IcejadeGymirAegirine = 86682165;
            public const int SwordsoulChengying = 96633955;
            public const int AdamancipatorDragite = 9464441;
            public const int WhiteAuraWhale = 5614808;
            public const int CrocodragonArchethys = 87188910;
            public const int EkhajarDescendantDragon = 65424481;
            public const int GungnirDragon = 65749035;
            public const int CoralDragon = 42566602;
            public const int BrionacDragon = 50321796;
            public const int DewlorenTigerKing = 70583986;
            public const int SPLittleKnight = 29301450;
        }

        internal IceBarrierPlugin Plugin { get; private set; }

        public IceBarrierExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new IceBarrierPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin?.ResetTurnState();
        }

        private void RegisterHelperModules()
        {
            // 1. Layer 2 Central Core Ace Card Protection
            ResourcePlan.RegisterAceCards(
                CardId.LanceaAncestralDragon,
                CardId.TrishulaZeroDragon,
                CardId.TrishulaDragon,
                CardId.IcejadeGymirAegirine,
                CardId.SwordsoulChengying,
                CardId.AdamancipatorDragite,
                CardId.GeneralRaiho,
                CardId.Medium
            );
            HeuristicGuard.RegisterAceCards(
                CardId.LanceaAncestralDragon,
                CardId.TrishulaZeroDragon,
                CardId.TrishulaDragon,
                CardId.IcejadeGymirAegirine,
                CardId.SwordsoulChengying,
                CardId.AdamancipatorDragite,
                CardId.GeneralRaiho,
                CardId.Medium
            );

            // 2. Layer 2 Handtrap Bait & Combo Starters
            BaitPlanner.RegisterComboStarters(
                CardId.Medallion,
                CardId.Revealer,
                CardId.MirrorMage,
                CardId.GeorgiusSwordman
            );
            BaitPlanner.RegisterBaitCards(
                CardId.FoolishBurialGoods,
                CardId.FoolishBurial,
                CardId.WindsOver
            );

            // 3. Layer 2 High Value Chain Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.LanceaAncestralDragon,
                CardId.TrishulaZeroDragon,
                CardId.IcejadeGymirAegirine,
                CardId.AdamancipatorDragite,
                CardId.Revealer,
                CardId.MirrorMage
            );
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Revealer 1-Card Starter into Lancea ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "IceBarrier-Revealer-Starter",
                RequiredCards = new List<int> { CardId.Revealer },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Revealer, ActionType = ExecutorType.Summon, Description = "Normal Summon Revealer of the Ice Barrier" },
                    new() { CardId = CardId.Revealer, ActionType = ExecutorType.Activate, Description = "Revealer discards 1 card to SS Mirror Mage from Deck" },
                    new() { CardId = CardId.MirrorMage, ActionType = ExecutorType.Activate, Description = "Mirror Mage tributes Revealer to summon Tokens" }
                },
                FallbackLineName = "IceBarrier-Medallion-Starter"
            });

            // ── Line 2: Medallion Search Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "IceBarrier-Medallion-Starter",
                RequiredCards = new List<int> { CardId.Medallion },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Medallion, ActionType = ExecutorType.Activate, Description = "Activate Medallion to search Revealer or Mirror Mage" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, DefaultNibiru);

            // Dragite Spell/Trap Negation
            AddExecutor(ExecutorType.Activate, CardId.AdamancipatorDragite, DragiteActivate);

            // Gymir Aegirine Quick Monster Immunity Protection
            AddExecutor(ExecutorType.Activate, CardId.IcejadeGymirAegirine, GymirAegirineActivate);

            // Lancea Ancestral Dragon — Boss Disruption Trigger on Opponent's Special Summon
            AddExecutor(ExecutorType.Activate, CardId.LanceaAncestralDragon, LanceaActivate);

            // Trishula Zero Dragon Banish 3 on Synchro Summon & Float Trigger
            AddExecutor(ExecutorType.Activate, CardId.TrishulaZeroDragon, TrishulaZeroActivate);

            // Classic Trishula Banish 3 zones on Summon
            AddExecutor(ExecutorType.Activate, CardId.TrishulaDragon, TrishulaDragonActivate);

            // Swordsoul Chengying Banish on field & GY
            AddExecutor(ExecutorType.Activate, CardId.SwordsoulChengying, ChengyingActivate);

            // Ice Barrier Trap: Negate attack on field, or Banish in GY to search Kaiju
            AddExecutor(ExecutorType.Activate, CardId.IceBarrierTrap, IceBarrierTrapActivate);

            // Triple Tactics Talent
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS (Going Second / Turn 2)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.GamecielKaiju, GamecielSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, HarpieDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.WhiteAuraWhale, WhiteAuraWhaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.BrionacDragon, BrionacActivate);
            AddExecutor(ExecutorType.Activate, CardId.GungnirDragon, GungnirActivate);
            AddExecutor(ExecutorType.Activate, CardId.EkhajarDescendantDragon, EkhajarActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: CONSISTENCY ENGINES, STARTERS & SEARCHERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Medallion, MedallionActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurialGoods, FoolishBurialGoodsActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);

            // Revealer Normal Summon & Ignition to summon Mirror Mage from Deck
            AddExecutor(ExecutorType.Summon, CardId.Revealer, RevealerSummon);
            AddExecutor(ExecutorType.Activate, CardId.Revealer, RevealerActivate);

            // Mirror Mage: Token generation ignition & GY search on sent
            AddExecutor(ExecutorType.Activate, CardId.MirrorMage, MirrorMageActivate);

            // Freezing Chains of the Ice Barrier: Revives Level 4- from GY
            AddExecutor(ExecutorType.Activate, CardId.FreezingChains, FreezingChainsActivate);

            // Georgius: Special Summon from hand & Revive Level 5- on summon
            AddExecutor(ExecutorType.Activate, CardId.GeorgiusSwordman, GeorgiusActivate);

            // Speaker: Special Summon from hand & GY banish token generation
            AddExecutor(ExecutorType.Activate, CardId.Speaker, SpeakerActivate);

            // General Wayne: Special Summon from hand & search S/T on summon
            AddExecutor(ExecutorType.Activate, CardId.GeneralWayne, WayneActivate);

            // Prior: Special Summon from hand & Tribute to revive
            AddExecutor(ExecutorType.Activate, CardId.Prior, PriorActivate);

            // Winds Over the Ice Barrier: Tribute to SS from Deck & GY recycle
            AddExecutor(ExecutorType.Activate, CardId.WindsOver, WindsOverActivate);

            // Coral Dragon draw trigger on sent to GY
            AddExecutor(ExecutorType.Activate, CardId.CoralDragon, CoralDragonActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: EXTRA DECK SYNCHRO CLIMBING
            // ═══════════════════════════════════════════════════════════════
            // Priority 1: Lancea Ancestral Dragon (Level 10 Ace)
            AddExecutor(ExecutorType.SpSummon, CardId.LanceaAncestralDragon, LanceaSpSummon);

            // Priority 2: Icejade Gymir Aegirine (Level 10 Board Protection)
            AddExecutor(ExecutorType.SpSummon, CardId.IcejadeGymirAegirine, GymirSpSummon);

            // Priority 3: Swordsoul Supreme Sovereign - Chengying (Level 10 Finisher)
            AddExecutor(ExecutorType.SpSummon, CardId.SwordsoulChengying, ChengyingSpSummon);

            // Priority 4: Adamancipator Risen - Dragite (Level 8 S/T Negation)
            AddExecutor(ExecutorType.SpSummon, CardId.AdamancipatorDragite, DragiteSpSummon);

            // Priority 5: White Aura Whale (Level 8 Board Wipe on Turn 2)
            AddExecutor(ExecutorType.SpSummon, CardId.WhiteAuraWhale, WhiteAuraWhaleSpSummon);

            // Priority 6: Crocodragon Archethys (Level 9 Draw 2+)
            AddExecutor(ExecutorType.SpSummon, CardId.CrocodragonArchethys, CrocodragonSpSummon);

            // Priority 7: Trishula Zero / Trishula
            AddExecutor(ExecutorType.SpSummon, CardId.TrishulaZeroDragon, TrishulaZeroSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TrishulaDragon, TrishulaSpSummon);

            // Priority 8: Mid-range Synchros: Coral Dragon, Brionac, Dewloren
            AddExecutor(ExecutorType.SpSummon, CardId.CoralDragon, CoralDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BrionacDragon, BrionacSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DewlorenTigerKing, DewlorenSpSummon);

            // Priority 9: S:P Little Knight (Main 2 utility)
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: NORMAL SUMMONS, REPOSITIONS & SETS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.MirrorMage, NormalSummonTuner);
            AddExecutor(ExecutorType.Summon, CardId.Speaker, NormalSummonExtender);
            AddExecutor(ExecutorType.Summon, CardId.Prior, NormalSummonExtender);
            AddExecutor(ExecutorType.Summon, CardId.Warlock, NormalSummonFloodgate);

            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.IceBarrierTrap, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION HANDLERS
        // ═══════════════════════════════════════════════════════════════

        private bool AshBlossomActivate()
        {
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool ImpermanenceActivate()
        {
            return DefaultInfiniteImpermanence();
        }

        private bool CrossoutActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Negate dangerous handtraps activated by opponent
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.Id == CardId.AshBlossom ||
                    LastChainCard.Id == CardId.InfiniteImpermanence ||
                    LastChainCard.Id == CardId.Nibiru)
                {
                    AI.SelectAnnounceID(LastChainCard.Id);
                    return true;
                }
            }
            return false;
        }

        private bool IsCurrentCardNegated()
        {
            return Card != null && DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool DragiteActivate()
        {
            // Negate opponent's Spell/Trap activation if we have a WATER monster in GY
            if (IsCurrentCardNegated()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1 &&
                (LastChainCard.IsSpell() || LastChainCard.IsTrap()) &&
                Bot.Graveyard.Any(c => c.HasAttribute(CardAttribute.Water)))
            {
                return true;
            }
            return false;
        }

        private bool GymirAegirineActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Quick effect: Make our face-up monsters unaffected and indestuctible by opponent effects
            if (Duel.Player == 1 || Duel.CurrentChain.Any(c => c.Controller == 1 && (c.IsSpell() || c.IsTrap() || c.IsMonster())))
            {
                return true;
            }
            return false;
        }

        private bool LanceaActivate()
        {
            // Trigger 1: Opponent Special Summons monster -> Special Summon Ice Barrier from Deck/Hand/Extra/GY
            // Trigger 2: Leaves field by opponent card -> Special Summon Ice Barrier Synchro from Extra Deck
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Select target based on opponent board state via Strategy
                ClientCard bestTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(
                    Bot.Deck.Concat(Bot.Hand).Concat(Bot.Graveyard).Concat(Bot.ExtraDeck)
                       .Where(c => c != null && c.HasSetcode(0x81)).ToList()
                );
                if (bestTarget != null)
                {
                    AI.SelectCard(bestTarget.Id);
                }
                return true;
            }
            else if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                // Leaves field floater -> Trishula Zero (70980824) to banish 3!
                AI.SelectCard(CardId.TrishulaZeroDragon, CardId.TrishulaDragon, CardId.EkhajarDescendantDragon);
                return true;
            }
            return true;
        }

        private bool TrishulaZeroActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // When Synchro Summoned: Banish up to 3 cards the opponent controls
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                                    .OrderByDescending(c => c.Attack).ToList();
                if (oppCards.Count > 0)
                {
                    AI.SelectCard(oppCards.Take(3).ToList());
                    return true;
                }
            }
            // Float effect when destroyed by opponent: Special Summon Trishula from Extra/GY
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.TrishulaDragon);
                return true;
            }
            return true;
        }

        private bool TrishulaDragonActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Banish 1 from hand, 1 from field, 1 from GY
            return true;
        }

        private bool ChengyingActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Banish 1 card from opponent field and 1 from GY
            var oppField = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault()
                        ?? Enemy.GetSpells().FirstOrDefault();
            var oppGrave = Enemy.Graveyard.OrderByDescending(c => c.Attack).FirstOrDefault();

            var targets = new List<ClientCard>();
            if (oppField != null) targets.Add(oppField);
            if (oppGrave != null) targets.Add(oppGrave);
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool IceBarrierTrapActivate()
        {
            // On field: Attack declaration -> negate attack, ATK to 0
            if (Card.Location == CardLocation.SpellZone)
            {
                return true;
            }

            // In GY: Quick effect -> Banish self, send Gameciel Kaiju from Deck to GY and add it to hand
            if (Card.Location == CardLocation.Grave)
            {
                // Dump Gameciel (55063751) and immediately add to hand!
                AI.SelectCard(CardId.GamecielKaiju);
                return true;
            }

            return false;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Option 0: Draw 2 cards (default best consistency)
            // Option 1: Take control of opponent monster
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500))
            {
                AI.SelectOption(1);
            }
            else
            {
                AI.SelectOption(0);
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BOARD BREAKERS
        // ═══════════════════════════════════════════════════════════════

        private bool GamecielSpSummon()
        {
            // Tribute opponent's biggest threat (boss / tower / negator / highest threat score)
            ClientCard target = Enemy.GetMonsters().Where(m => m.IsFaceup())
                                     .OrderByDescending(m => Plugin.ThreatImpl.EvaluateThreatScore(m))
                                     .ThenByDescending(m => m.Attack)
                                     .FirstOrDefault();
            if (target != null && (Plugin.ThreatImpl.EvaluateThreatScore(target) >= 50 || target.Attack >= 2000 || CardIntelligence.IsKnownNegator(target.Id)))
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HarpieDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool WhiteAuraWhaleActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Raigeki opponent attack position monsters on summon
            return Enemy.GetMonsters().Any(m => m.IsAttack());
        }

        private bool BrionacActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Discard to bounce opponent cards
            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (oppCards.Count > 0 && Bot.Hand.Count > 0)
            {
                AI.SelectCard(oppCards.OrderByDescending(c => c.Attack).FirstOrDefault());
                return true;
            }
            return false;
        }

        private bool GungnirActivate()
        {
            if (IsCurrentCardNegated()) return false;
            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (oppCards.Count > 0 && Bot.Hand.Count > 0)
            {
                AI.SelectCard(oppCards.OrderByDescending(c => c.Attack).FirstOrDefault());
                return true;
            }
            return false;
        }

        private bool EkhajarActivate()
        {
            if (IsCurrentCardNegated()) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  STARTERS & EXTENDERS
        // ═══════════════════════════════════════════════════════════════

        private bool MedallionActivate()
        {
            // Non-OPT ROTA for Ice Barrier: Always search key piece!
            AI.SelectCard(CardId.Revealer, CardId.MirrorMage, CardId.GeorgiusSwordman, CardId.Speaker, CardId.GeneralWayne);
            return true;
        }

        private bool FoolishBurialGoodsActivate()
        {
            // Send Ice Barrier trap to GY (ready to search Gameciel Kaiju!)
            AI.SelectCard(CardId.IceBarrierTrap, CardId.FreezingChains);
            return true;
        }

        private bool FoolishBurialActivate()
        {
            // Send Mirror Mage (triggers search when sent to GY) or Georgius
            AI.SelectCard(CardId.MirrorMage, CardId.GeorgiusSwordman, CardId.Speaker);
            return true;
        }

        private bool RevealerSummon()
        {
            return true;
        }

        private bool RevealerActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Discard 1 card -> Special Summon Mirror Mage from Deck!
            AI.SelectCard(CardId.MirrorMage);
            return true;
        }

        private bool MirrorMageActivate()
        {
            // Effect 1: In monster zone -> Tribute 1 other monster (or token) to summon up to 3 tokens
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsCurrentCardNegated()) return false;
                // If we already control tokens or fodder, tribute fodder first
                ClientCard tributeTarget = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.IceBarrierToken ||
                                                                                 m.Id == CardId.Revealer ||
                                                                                 m.Id == CardId.Speaker ||
                                                                                 m.Id == CardId.Prior);
                if (tributeTarget != null)
                {
                    AI.SelectCard(tributeTarget);
                }
                // Declare number of tokens to summon (1 to 3 depending on zone count)
                int freeZones = Math.Max(1, Math.Min(3, 5 - Bot.GetMonsterCount()));
                AI.SelectNumber(freeZones);
                return true;
            }

            // Effect 2: Sent to Graveyard -> Search any Ice Barrier card!
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.FreezingChains, CardId.GeorgiusSwordman, CardId.Revealer, CardId.Medallion, CardId.GeneralWayne, CardId.Speaker);
                return true;
            }

            return false;
        }

        private bool FreezingChainsActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Revive Level 4 or lower Ice Barrier: Mirror Mage > Revealer > Speaker > Prior
            AI.SelectCard(CardId.MirrorMage, CardId.Revealer, CardId.Speaker, CardId.Prior);
            return true;
        }

        private bool GeorgiusActivate()
        {
            // In Hand: Special Summon self if we control an Ice Barrier monster
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x81));
            }

            // On Summon: Revive Level 5 or lower Ice Barrier from GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsCurrentCardNegated()) return false;
                AI.SelectCard(CardId.MirrorMage, CardId.Revealer, CardId.Speaker, CardId.Prior);
                return true;
            }

            return false;
        }

        private bool SpeakerActivate()
        {
            // In Hand: Special Summon self if we control an Ice Barrier
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x81));
            }

            // In GY: Banish self to Special Summon 1 Ice Barrier Token
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }

            return false;
        }

        private bool WayneActivate()
        {
            // In Hand: Special Summon if opp controls monster and we control Ice Barrier
            if (Card.Location == CardLocation.Hand)
            {
                return Enemy.GetMonsterCount() > 0 && Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x81));
            }

            // On Summon: Search Ice Barrier Spell/Trap
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsCurrentCardNegated()) return false;
                AI.SelectCard(CardId.Medallion, CardId.FreezingChains, CardId.WindsOver, CardId.IceBarrierTrap);
                return true;
            }

            return false;
        }

        private bool PriorActivate()
        {
            // In Hand: Special Summon self
            if (Card.Location == CardLocation.Hand)
            {
                // Only if we haven't or won't summon Level 5+ this turn, or as last resort
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x81));
            }

            // On Field: Tribute to revive any Ice Barrier
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.LanceaAncestralDragon, CardId.GeneralRaiho, CardId.GeorgiusSwordman, CardId.Revealer);
                return true;
            }

            return false;
        }

        private bool WindsOverActivate()
        {
            // Field: Tribute IBs to SS from Deck
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (Bot.GetMonsters().Any(m => m.Id == CardId.IceBarrierToken || m.Id == CardId.Speaker || m.Id == CardId.Prior))
                {
                    AI.SelectCard(CardId.MirrorMage, CardId.Revealer, CardId.GeorgiusSwordman);
                    return true;
                }
            }

            // GY: Banish to retrieve Ice Barrier from GY/banished
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.Revealer, CardId.MirrorMage, CardId.GeorgiusSwordman);
                return true;
            }

            return false;
        }

        private bool CoralDragonActivate()
        {
            // Draw 1 when sent to GY
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHRO SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool LanceaSpSummon()
        {
            // Primary Level 10 Boss: Always summon if we have materials!
            return true;
        }

        private bool GymirSpSummon()
        {
            // Level 10 Protection: Summon if we already have Lancea or going second
            return Bot.HasInMonstersZone(CardId.LanceaAncestralDragon) || Duel.Turn > 1;
        }

        private bool ChengyingSpSummon()
        {
            // Level 10 Finisher: Summon to push lethal battle damage
            return Bot.HasInMonstersZone(CardId.LanceaAncestralDragon) || Util.IsTurn1OrMain2();
        }

        private bool DragiteSpSummon()
        {
            // Level 8 Spell/Trap Negate
            return true;
        }

        private bool WhiteAuraWhaleSpSummon()
        {
            // Level 8 Board Wipe on Turn 2
            return Duel.Turn > 1 && Enemy.GetMonsters().Any(m => m.IsAttack());
        }

        private bool CrocodragonSpSummon()
        {
            // Level 9 Draw Engine
            return true;
        }

        private bool TrishulaZeroSpSummon()
        {
            // Level 11 Board Wipe
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool TrishulaSpSummon()
        {
            // Level 9 Hand/Field/GY banish
            return Enemy.GetMonsterCount() > 0 || Enemy.Hand.Count > 0 || Enemy.Graveyard.Count > 0;
        }

        private bool CoralDragonSpSummon()
        {
            // Level 6 Synchro Tuner: Step into Level 10 Lancea (6 + 4)
            return true;
        }

        private bool BrionacSpSummon()
        {
            // Level 6 Bounce
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool DewlorenSpSummon()
        {
            return true;
        }

        private bool SPLittleKnightSpSummon()
        {
            // Only summon in Main 2 when not locked into WATER
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Count(m => !m.HasAttribute(CardAttribute.Water)) >= 2;
        }

        private bool SPLittleKnightActivate()
        {
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  NORMAL SUMMONS & SETS
        // ═══════════════════════════════════════════════════════════════

        private bool NormalSummonTuner()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool NormalSummonExtender()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool NormalSummonFloodgate()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        public bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            if (Card.HasType(CardType.Link)) return false;

            // 1. 0 ATK monsters or Handtraps in Attack position -> ALWAYS switch to Defense!
            if (Card.IsAttack() && (Card.Attack == 0 || CardIntelligence.IsHandtrap(Card.Id)))
                return true;

            // 2. High DEF / Low ATK (DEF > ATK and ATK < 1800, e.g. Speaker 1000/1800, Mirror Mage 1000/400)
            if (Card.IsAttack() && Card.Defense > Card.Attack && Card.Attack < 1800)
            {
                if (Duel.Phase == DuelPhase.Main1 && ShouldRushAttack) return false;
                return true;
            }

            // 3. High ATK monsters (>= 1800) in Defense position -> switch to Attack to push battle damage
            if (Card.IsDefense() && Card.Attack >= 1800 && Card.Attack >= Card.Defense)
            {
                if (!Util.IsAllEnemyBetter(true))
                    return true;
            }

            return DefaultMonsterRepos();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search (hint 506 = HINTMSG_ATOHAND or all candidates from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    ClientCard bestSearch = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                    if (bestSearch != null)
                    {
                        var result = new List<ClientCard> { bestSearch };
                        var others = cards.Where(c => c != bestSearch).Take(max - 1);
                        result.AddRange(others);
                        if (result.Count >= min) return result.Take(max).ToList();
                    }
                }

                // Special Summon target (hint 509 = HINTMSG_SPSUMMON)
                if (hint == 509)
                {
                    ClientCard bestSpSummon = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                    if (bestSpSummon != null)
                    {
                        var result = new List<ClientCard> { bestSpSummon };
                        var others = cards.Where(c => c != bestSpSummon).Take(max - 1);
                        result.AddRange(others);
                        if (result.Count >= min) return result.Take(max).ToList();
                    }
                }

                // Discard cost (hint 501 = HINTMSG_DISCARD)
                if (hint == 501)
                {
                    ClientCard discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (discardTarget != null)
                    {
                        var result = new List<ClientCard> { discardTarget };
                        var others = cards.Where(c => c != discardTarget).Take(max - 1);
                        result.AddRange(others);
                        if (result.Count >= min) return result.Take(max).ToList();
                    }
                }

                // Removal (hint 503 [REMOVE], hint 502 [DESTROY], hint 504 [TOGRAVE], hint 505 [RTOHAND]): ALWAYS target Enemy cards!
                if (hint == 503 || hint == 502 || hint == 504 || hint == 505)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                    }
                }

                // Synchro Materials (hint 512): Use Tokens & low-cost fodder first, protect Bosses!
                if (hint == 512)
                {
                    var sorted = Plugin.MaterialImpl.SortMaterials(cards, min);
                    if (sorted.Count >= min)
                    {
                        return sorted.Take(max).ToList();
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps (Ash, Veiler, Imperm) & 0 ATK monsters: ALWAYS DEFENSE!
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Low ATK (DEF > ATK && ATK < 1800, e.g. Speaker 1000/1800) -> DEFENSE
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800, Lancea 3300, Gymir 3000, Chengying 3000) -> ATTACK
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool OnSelectHand()
        {
            // True = Go First to set up Lancea + Raiho / Medium / Georgius lock
            return true;
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;
            // Reject opponent effect prompts
            if (card.Controller == 1) return false;

            // Always accept beneficial Lancea, Trishula Zero, Mirror Mage, Georgius triggers
            return true;
        }
    }
}
