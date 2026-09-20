// ============================================================================
// CARD AUDIT — _2026_Centurion (Centur-Ion — Tier 1 Cosmic Blazar Synchro 12 Juggernaut)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Centur-Ion Primera                 | Monster L4 T | Yes  | Yes   | None    | NS/SS: Search Centur-Ion card; S/T zone SS self| Main Phase: search Trudea/Gargoyle/Bonds     | Already used this turn                      |
// | Centur-Ion Trudea                  | Monster L4   | Yes  | Yes   | None    | S/T zone SS self; place 2 Centur-Ion in S/T   | Main Phase: swarm Primera + Gargoyle -> Lv 8  | Already used this turn                      |
// | Centur-Ion Gargoyle II             | Monster L8   | Yes  | Yes   | Send mon| Hand/S/T zone SS self; level 8 extender       | Main Phase extender into Level 12 Synchro    | Field full                                  |
// | Centur-Ion Emeth VI                | Monster L8   | Yes  | Yes   | Place S/T| Quick SS self by placing monster in S/T zone  | Opponent turn / Main Phase extender          | S/T zones full                              |
// | Stand Up Centur-Ion!               | Spell Field  | Yes  | Yes   | Discard | Place Centur-Ion in S/T; Quick Synchro opp trn| Turn 1 setup: place Trudea; Quick Synchro    | Already on field                            |
// | Centur-Ion Bonds                   | Spell Quick  | Yes  | Yes   | None    | Place Centur-Ion from hand/GY into S/T zone   | Extend into S/T zone                          | S/T zones full                              |
// | Wake Up Centur-Ion!                | Spell Quick  | Yes  | Yes   | None    | Special Summon Token (L4 or L8) or foolish    | Need extra material or dump Gargoyle          | Field full                                  |
// | Centur-Ion True Awakening          | Trap Counter | Yes  | Yes   | Send S/T| Omni-Negate Monster/Spell/Trap + destroy      | Opponent activates card/effect                | No Centur-Ion card in S/T zone              |
// | Centur-Ion Phalanx                 | Trap Normal  | Yes  | Yes   | Target  | Banish 1 monster on field (returns End Phase) | Opponent threat / boss monster                | No targets on field                         |
// | Centur-Ion Legatia                 | Synchro L12  | Yes  | Yes   | None    | 3500 ATK; draw 1 + pop highest ATK opp monster| Synchro Summoned: draw + pop opp monster      | Opponent has no monsters                    |
// | Centur-Ion Auxila                  | Synchro L12  | Yes  | Yes   | None    | 3000 ATK; search Centur-Ion S/T; S/T protect  | Synchro Summoned: search True Awakening      | Already on field                            |
// | Crimson Dragon                     | Synchro L12  | Yes  | Yes   | Target  | Tag out with Lv 12 Synchro -> SS Cosmic Blazar| Opponent turn: tag Legatia -> Cosmic Blazar  | No Lv 12 Dragon Synchro in Extra Deck       |
// | Cosmic Blazar Dragon               | Synchro L12  | Yes  | No    | Banish  | Omni-Negate (Activation/Summon/Attack) -> End | Opponent dangerous play / summon / attack     | Already negated this turn                   |
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
    [Deck("_2026_Centurion", "_2026_Centurion")]
    public class _2026_CenturionExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int CenturIonPrimera = 15005145;
            public const int CenturIonTrudea = 42493140;
            public const int CenturIonGargoyleII = 97698279;
            public const int CenturIonEmethVI = 78888899;

            // Spells & Traps
            public const int StandUpCenturIon = 41371602;
            public const int CenturIonBonds = 4160316;
            public const int WakeUpCenturIon = 92907248;
            public const int CenturIonTrueAwakening = 77543769;
            public const int CenturIonPhalanx = 40155014;
            public const int Terraforming = 40318957;
            public const int Bonfire = 67332219;
            public const int PotOfProsperity = 84211599;
            public const int CosmicCyclone = 8267140;
            public const int CalledByTheGrave = 24224830;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int GhostBelle = 73642296;

            // Extra Deck
            public const int CenturIonLegatia = 15982593;
            public const int CenturIonAuxila = 71858682;
            public const int CenturIonPrimeraPrimus = 8841431;
            public const int CosmicBlazarDragon = 21123811;
            public const int RedSupernovaDragon = 99585850;
            public const int CrimsonDragon = 63436931;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int TyPhon = 12470404;
        }

        public _2026_CenturionExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Field Spell Stand Up Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Centurion-StandUp-Starter",
                RequiredCards = new List<int> { CardId.StandUpCenturIon },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.StandUpCenturIon, ActionType = ExecutorType.Activate, Description = "Activate Stand Up Centur-Ion!" }
                },
                FallbackLineName = "Centurion-Trudea-Starter"
            });

            // ── Line 2: Trudea Normal Summon Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Centurion-Trudea-Starter",
                RequiredCards = new List<int> { CardId.CenturIonTrudea },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CenturIonTrudea, ActionType = ExecutorType.Summon, Description = "Normal Summon Centur-Ion Trudea" },
                    new() { CardId = CardId.CenturIonTrudea, ActionType = ExecutorType.Activate, Description = "Trudea places Primera and Gargoyle in S/T zone" }
                },
                FallbackLineName = "Centurion-Primera-Starter"
            });

            // ── Line 3: Primera Normal Summon Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Centurion-Primera-Starter",
                RequiredCards = new List<int> { CardId.CenturIonPrimera },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CenturIonPrimera, ActionType = ExecutorType.Summon, Description = "Normal Summon Centur-Ion Primera" },
                    new() { CardId = CardId.CenturIonPrimera, ActionType = ExecutorType.Activate, Description = "Primera searches Stand Up or Trudea" }
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
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneActivate);

            // Counter Trap Omni-Negate
            AddExecutor(ExecutorType.Activate, CardId.CenturIonTrueAwakening, TrueAwakeningActivate);

            // Phalanx Banish
            AddExecutor(ExecutorType.Activate, CardId.CenturIonPhalanx, PhalanxActivate);

            // Cosmic Blazar Dragon — 4000 ATK Ultimate Omni-Negate
            AddExecutor(ExecutorType.Activate, CardId.CosmicBlazarDragon, CosmicBlazarActivate);

            // Crimson Dragon Tag-Out into Cosmic Blazar
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragon, CrimsonDragonActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: FIELD SPELL & SEARCHERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.Bonfire, BonfireActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);

            AddExecutor(ExecutorType.Activate, CardId.StandUpCenturIon, StandUpActivate);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonBonds, BondsActivate);
            AddExecutor(ExecutorType.Activate, CardId.WakeUpCenturIon, WakeUpActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: TRAP-ZONE JUMP OUTS & MONSTER EFFECTS
            // ═══════════════════════════════════════════════════════════════
            // Jump-out from Spell/Trap zone into Monster Zone
            AddExecutor(ExecutorType.Activate, CardId.CenturIonPrimera, PrimeraEffect);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonTrudea, TrudeaEffect);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonGargoyleII, GargoyleEffect);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonEmethVI, EmethEffect);

            // Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.CenturIonTrudea, TrudeaSummon);
            AddExecutor(ExecutorType.Summon, CardId.CenturIonPrimera, PrimeraSummon);

            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SYNCHRO SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragon, CrimsonDragonSpSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.CenturIonLegatia, LegatiaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonLegatia, LegatiaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.CenturIonAuxila, AuxilaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonAuxila, AuxilaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.CenturIonPrimeraPrimus, PrimusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CenturIonPrimeraPrimus, PrimusEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: TRAP SETS & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.CenturIonTrueAwakening, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CenturIonPhalanx, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool AshBlossomActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool ImpermanenceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool GhostBelleActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool CalledByTheGraveActivate()
        {
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

        private bool CosmicCycloneActivate()
        {
            ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup())
                             ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TrueAwakeningActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                // Send 1 face-up Centur-Ion card from S/T zone to GY as cost
                ClientCard cost = Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.HasSetcode(0x1a2) && s != Card);
                if (cost != null)
                {
                    AI.SelectCard(cost);
                    return true;
                }
            }
            return false;
        }

        private bool PhalanxActivate()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CosmicBlazarActivate()
        {
            // Omni-Negate anything: banish self to negate
            return true;
        }

        private bool CrimsonDragonActivate()
        {
            // Target Level 12 Synchro (Legatia / Auxila) -> tag out into Cosmic Blazar Dragon
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level == 12 && m != Card);
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.CosmicBlazarDragon);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCHERS & SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool TerraformingActivate()
        {
            AI.SelectCard(CardId.StandUpCenturIon);
            return true;
        }

        private bool BonfireActivate()
        {
            // Trudea is Level 4 Pyro
            AI.SelectCard(CardId.CenturIonTrudea);
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            var banishList = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.CosmicBlazarDragon &&
                c.Id != CardId.CrimsonDragon &&
                c.Id != CardId.CenturIonLegatia &&
                c.Id != CardId.CenturIonAuxila
            ).Take(6).ToList();

            if (banishList.Count >= 3)
            {
                AI.SelectCard(banishList);
                return true;
            }
            return false;
        }

        private bool StandUpActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.StandUpCenturIon);
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                // Main Phase: discard 1 to place Centur-Ion in S/T zone
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    ClientCard discard = Bot.Hand.FirstOrDefault(c => c != Card && c.Id != CardId.CenturIonPrimera);
                    if (discard != null && Bot.SpellZone.Count(s => s != null) < 5)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(CardId.CenturIonTrudea, CardId.CenturIonPrimera, CardId.CenturIonGargoyleII);
                        return true;
                    }
                }
                // Opponent Turn: Quick Synchro Summon
                if (Duel.Player == 1)
                {
                    return true;
                }
            }
            return true;
        }

        private bool BondsActivate()
        {
            if (Bot.SpellZone.Count(s => s != null) >= 5) return false;
            AI.SelectCard(CardId.CenturIonTrudea, CardId.CenturIonPrimera, CardId.CenturIonGargoyleII);
            return true;
        }

        private bool WakeUpActivate()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS & TRAP-ZONE JUMP OUTS
        // ═══════════════════════════════════════════════════════════════

        private bool PrimeraSummon()
        {
            return true;
        }

        private bool PrimeraEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Jump out to Monster Zone
                return Bot.GetMonsterCount() < 5;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon search Centur-Ion card
                if (!Bot.HasInSpellZone(CardId.StandUpCenturIon) && !Bot.HasInHand(CardId.StandUpCenturIon))
                {
                    AI.SelectCard(CardId.StandUpCenturIon);
                }
                else if (!Bot.HasInHand(CardId.CenturIonTrudea) && !Bot.HasInMonstersZone(CardId.CenturIonTrudea))
                {
                    AI.SelectCard(CardId.CenturIonTrudea);
                }
                else
                {
                    AI.SelectCard(CardId.CenturIonGargoyleII, CardId.CenturIonTrueAwakening, CardId.CenturIonBonds);
                }
                return true;
            }
            return true;
        }

        private bool TrudeaSummon()
        {
            return true;
        }

        private bool TrudeaEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Jump out to Monster Zone
                return Bot.GetMonsterCount() < 5;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // Place 2 Centur-Ion monsters (Primera + Gargoyle II) in S/T zone
                AI.SelectCard(CardId.CenturIonPrimera, CardId.CenturIonGargoyleII);
                return true;
            }
            return true;
        }

        private bool GargoyleEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return true;
        }

        private bool EmethEffect()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS
        // ═══════════════════════════════════════════════════════════════

        private bool LegatiaSpSummon()
        {
            return true;
        }

        private bool LegatiaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon: pop highest ATK opponent monster
                ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                }
                return true;
            }
            return true;
        }

        private bool AuxilaSpSummon()
        {
            return true;
        }

        private bool AuxilaEffect()
        {
            // Search Centur-Ion True Awakening or Centur-Ion Phalanx
            AI.SelectCard(CardId.CenturIonTrueAwakening, CardId.CenturIonPhalanx, CardId.StandUpCenturIon);
            return true;
        }

        private bool PrimusSpSummon()
        {
            return true;
        }

        private bool PrimusEffect()
        {
            AI.SelectCard(CardId.StandUpCenturIon, CardId.CenturIonTrudea);
            return true;
        }

        private bool CrimsonDragonSpSummon()
        {
            // Level 12 Synchro Dragon
            return true;
        }

        private bool SPLittleKnightSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Count(m => m.Level != 12) >= 2;
        }

        private bool SPLittleKnightEffect()
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
        //  HEURISTICS & CARD SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search (hint 506 = HINTMSG_ATOHAND or all candidates from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.Where(c => c.Id == CardId.StandUpCenturIon ||
                                                    c.Id == CardId.CenturIonPrimera ||
                                                    c.Id == CardId.CenturIonTrudea ||
                                                    c.Id == CardId.CenturIonTrueAwakening).ToList();
                    if (preferred.Count >= min)
                    {
                        return preferred.Take(max).ToList();
                    }
                }

                // Removal (hint 503 [REMOVE], hint 502 [DESTROY], hint 504 [TOGRAVE]): ALWAYS target Enemy cards!
                if (hint == 503 || hint == 502 || hint == 504)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                    }
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
