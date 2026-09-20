// ============================================================================
// CARD AUDIT — Tenpai (Tenpai Dragon — The Ultimate Going-Second OTK God)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Tenpai Dragon Paidra               | Monster L4   | Yes  | Yes   | None    | On NS/SS: Search Sangen Spell/Trap; battle prot| Main Phase: search Sangen Summoning/Kaimen   | Already used this turn                      |
// | Tenpai Dragon Chundra              | Monster L4 T | Yes  | Yes   | None    | SS from hand if Dragon battles; battle SS deck| Battle Phase: SS self & SS Fadra from Deck   | Already used this turn                      |
// | Tenpai Dragon Fadra                | Monster L4   | Yes  | Yes   | None    | Dragon battle destruction prot; revive FIRE Dr| Battle Phase: revive Paidra/Chundra from GY  | Already used this turn                      |
// | Tenpai Dragon Genroku              | Monster L3   | Yes  | Yes   | Tribute | Tribute self: SS 1 Tenpai monster from Deck   | Main/Battle: SS Chundra/Paidra from Deck     | Already used this turn                      |
// | Sangen Summoning                   | Spell Field  | Yes  | Yes   | Destroy | Blanket MP1 effect immunity; pop 1 card -> srch| Main Phase 1: gain total immunity + search   | Already on field                            |
// | Sangen Kaimen                      | Spell Quick  | Yes  | Yes   | None    | Add/SS FIRE Dragon + force Battle Phase       | Main/Battle: search or swarm + jump to Battle| Already used this turn                      |
// | Sangenpai Bident Dragion           | Synchro L7 T | Yes  | Yes   | Target  | On Synchro: Revive FIRE Dragon; GY revive     | Battle Phase: tune L4+L4 -> revive tuner     | Already summoned                            |
// | Sangenpai Transcendent Dragion     | Synchro L10  | Yes  | Yes   | None    | Force opp attack; lock opp Battle effects     | Battle Phase: complete lockdown              | Already on field                            |
// | Trident Dragion                    | Synchro L10  | Yes  | No    | Destroy | Pop up to 2 friendly cards -> attack 3 times  | Battle Phase OTK: pop Sangen Summoning/Fadra | Opponent already defeated                   |
// | Super Polymerization               | Spell Quick  | No   | No    | Discard | Non-respondable fusion using enemy monsters   | Main Phase: break enemy board                | Opponent controls < 2 valid materials       |
// | Dark Ruler No More                 | Spell Normal | Yes  | No    | None    | Negate all face-up opponent monsters on field | Main Phase 1: shut down enemy negates        | Opponent controls 0 face-up effect monsters |
// | Forbidden Droplet                  | Spell Quick  | No   | No    | Send S/M| Send cards to GY -> negate and halve ATK      | Chain to opponent or Main Phase board break   | No cards to send / already negated          |
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
    [Deck("Tenpai", "Tenpai")]
    public class TenpaiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int TenpaiDragonPaidra = 39931513;
            public const int TenpaiDragonChundra = 91810826;
            public const int TenpaiDragonFadra = 65326118;
            public const int TenpaiDragonGenroku = 23657016;

            // Spells & Traps
            public const int SangenSummoning = 30336082;
            public const int SangenKaimen = 66730191;
            public const int SangenFuro = 55484152;
            public const int SangenKaiho = 25388971;
            public const int PotOfProsperity = 84211599;
            public const int DarkRulerNoMore = 54693926;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int LightningStorm = 14532163;
            public const int CalledByTheGrave = 24224830;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int SangenpaiBidentDragion = 82570174;
            public const int SangenpaiTranscendentDragion = 18969888;
            public const int TridentDragion = 39402797;
            public const int MoonlightRoseDragon = 33698022;
            public const int KuibeltTheBladeDragon = 87837090;
            public const int BystialDisPater = 27572350;
            public const int HiSpeedroidChanbara = 42110604;
            public const int Garura = 11765832;
            public const int Mudragon = 54757758;
            public const int EarthGolem = 62111090;
            public const int StarvingVenom = 41209827;
        }

        public TenpaiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Paidra Field Spell Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-Paidra-Setup",
                RequiredCards = new List<int> { CardId.TenpaiDragonPaidra },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TenpaiDragonPaidra, ActionType = ExecutorType.Summon, Description = "Normal Summon Paidra" },
                    new() { CardId = CardId.TenpaiDragonPaidra, ActionType = ExecutorType.Activate, Description = "Paidra searches Sangen Summoning" },
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Activate Sangen Summoning (Field Spell)" }
                },
                FallbackLineName = "Tenpai-Kaimen-Starter"
            });

            // ── Line 2: Sangen Summoning Field Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-Summoning-Starter",
                RequiredCards = new List<int> { CardId.SangenSummoning },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SangenSummoning, ActionType = ExecutorType.Activate, Description = "Activate Sangen Summoning" }
                }
            });

            // ── Line 3: Sangen Kaimen Quick-Play Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Tenpai-Kaimen-Starter",
                RequiredCards = new List<int> { CardId.SangenKaimen },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SangenKaimen, ActionType = ExecutorType.Activate, Description = "Activate Sangen Kaimen to search or SS Paidra/Chundra" }
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
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS & SUPER POLYMERIZATION
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: FIELD SPELLS, SEARCHERS & SPELL CARDS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.SangenSummoning, SangenSummoningActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenKaimen, SangenKaimenActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenFuro, SangenFuroActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenKaiho, SangenKaihoActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: TENPAI SUMMONS & MONSTER EFFECTS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonGenroku, TenpaiGenrokuActivate);
            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonPaidra, PaidraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonPaidra, PaidraEffect);

            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonChundra, ChundraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonChundra, ChundraEffect);

            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonFadra, FadraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonFadra, FadraEffect);

            // Fallback summon for any remaining body
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHROS (BATTLE PHASE OTK LADDER)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.TridentDragion, TridentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TridentDragion, TridentDragionEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiTranscendentDragion, TranscendentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiTranscendentDragion, TranscendentDragionEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiBidentDragion, BidentDragionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiBidentDragion, BidentDragionEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.HiSpeedroidChanbara, BaronneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HiSpeedroidChanbara, BaronneEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, DisPaterEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.KuibeltTheBladeDragon, KuibeltSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KuibeltTheBladeDragon, KuibeltEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.MoonlightRoseDragon, MoonlightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MoonlightRoseDragon, MoonlightEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: SPELL SETS & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SangenKaimen, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, SpellSetStrategy);

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

        private bool ForbiddenDropletActivate()
        {
            // Only negate opponent monsters that are faceup and not disabled
            var oppTargets = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsDisabled()).ToList();
            if (oppTargets.Count == 0) return false;

            // Choose expendable cards to send (Sangen Furo, extra spells, or tokens)
            var sendFodder = Bot.Hand.Concat(Bot.GetSpells())
                .Where(c => c != Card && c.Id != CardId.SangenSummoning && c.Id != CardId.TenpaiDragonPaidra)
                .Take(oppTargets.Count)
                .ToList();

            if (sendFodder.Count > 0)
            {
                AI.SelectCard(sendFodder);
                AI.SelectNextCard(oppTargets);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BOARD BREAKERS
        // ═══════════════════════════════════════════════════════════════

        private bool DarkRulerNoMoreActivate()
        {
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled());
        }

        private bool LightningStormActivate()
        {
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
            if (Bot.Hand.Count < 2) return false; // Needs 1 discard cost
            var enemyMonsters = Enemy.GetMonsters().Where(m => m.IsFaceup()).ToList();
            if (enemyMonsters.Count < 2) return false;

            // SuperPoly should fuse opponent's monsters into Garura, Mudragon, or Starving Venom
            AI.SelectCard(enemyMonsters);
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            // Banish 3 or 6 non-essential Extra Deck cards, strictly preserving Trident Dragion and Bident Dragion
            var banishList = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.TridentDragion &&
                c.Id != CardId.SangenpaiBidentDragion &&
                c.Id != CardId.SangenpaiTranscendentDragion
            ).Take(6).ToList();

            if (banishList.Count >= 3)
            {
                AI.SelectCard(banishList);
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
                // Activate Field Spell
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.SangenSummoning);
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                // Field Spell pop-and-search effect: destroy 1 friendly card to search any Tenpai
                ClientCard popTarget = Bot.GetSpells().FirstOrDefault(s => s != Card && s.IsFaceup())
                                    ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.TenpaiDragonFadra || m.Id == CardId.TenpaiDragonGenroku)
                                    ?? Card; // If nothing else, Sangen Summoning can destroy itself to search

                if (popTarget != null)
                {
                    AI.SelectCard(popTarget);
                    AI.SelectNextCard(CardId.TenpaiDragonChundra, CardId.TenpaiDragonPaidra, CardId.TenpaiDragonFadra);
                    return true;
                }
            }
            return true;
        }

        private bool SangenKaimenActivate()
        {
            // Search or Special Summon FIRE Dragon
            if (!Bot.HasInHand(CardId.TenpaiDragonPaidra) && !Bot.HasInMonstersZone(CardId.TenpaiDragonPaidra))
            {
                AI.SelectCard(CardId.TenpaiDragonPaidra);
            }
            else if (!Bot.HasInHand(CardId.TenpaiDragonChundra) && !Bot.HasInMonstersZone(CardId.TenpaiDragonChundra))
            {
                AI.SelectCard(CardId.TenpaiDragonChundra);
            }
            else
            {
                AI.SelectCard(CardId.TenpaiDragonFadra, CardId.TenpaiDragonGenroku);
            }
            return true;
        }

        private bool SangenFuroActivate()
        {
            return true;
        }

        private bool SangenKaihoActivate()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TENPAI MONSTERS & EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool TenpaiGenrokuActivate()
        {
            // Tribute to Special Summon Tenpai from Deck
            AI.SelectCard(CardId.TenpaiDragonChundra, CardId.TenpaiDragonPaidra);
            return true;
        }

        private bool PaidraSummon()
        {
            return true;
        }

        private bool PaidraEffect()
        {
            // Search Sangen Summoning or Sangen Kaimen
            if (!Bot.HasInSpellZone(CardId.SangenSummoning) && !Bot.HasInHand(CardId.SangenSummoning))
            {
                AI.SelectCard(CardId.SangenSummoning);
            }
            else
            {
                AI.SelectCard(CardId.SangenKaimen, CardId.SangenKaiho, CardId.SangenFuro);
            }
            return true;
        }

        private bool ChundraSummon()
        {
            return true;
        }

        private bool ChundraEffect()
        {
            // Special Summon from Hand or Deck
            AI.SelectCard(CardId.TenpaiDragonFadra, CardId.TenpaiDragonPaidra, CardId.TenpaiDragonGenroku);
            return true;
        }

        private bool FadraSummon()
        {
            return true;
        }

        private bool FadraEffect()
        {
            // Revive FIRE Dragon from GY
            ClientCard target = Bot.Graveyard.Where(c => c.IsMonster() && c.Attribute == (int)CardAttribute.Fire)
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS (BATTLE PHASE OTK)
        // ═══════════════════════════════════════════════════════════════

        private bool BidentDragionSpSummon()
        {
            // Level 7 Synchro: Tune L4 Tuner (Chundra) + L4 Non-Tuner (Paidra/Fadra)
            return true;
        }

        private bool BidentDragionEffect()
        {
            // On summon revive FIRE Dragon Tuner from GY
            ClientCard tuner = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TenpaiDragonChundra);
            if (tuner != null)
            {
                AI.SelectCard(tuner);
                return true;
            }
            return true;
        }

        private bool TridentDragionSpSummon()
        {
            // Level 10 Synchro: L7 (Bident Dragion) + L4 (Chundra/Tuner)
            // Or only summon during Battle Phase to unleash triple attack
            return Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main1;
        }

        private bool TridentDragionEffect()
        {
            // Destroy up to 2 friendly cards to attack 3 times!
            // Pick Sangen Summoning, Sangen Furo, or disposable monsters (Fadra/Paidra)
            var popTargets = Bot.GetSpells().Where(s => s.Id == CardId.SangenSummoning || s.Id == CardId.SangenFuro)
                .Concat(Bot.GetMonsters().Where(m => m != Card && m.Id != CardId.SangenpaiBidentDragion))
                .Take(2)
                .ToList();

            if (popTargets.Count > 0)
            {
                AI.SelectCard(popTargets);
                return true;
            }
            return true;
        }

        private bool TranscendentDragionSpSummon()
        {
            return true;
        }

        private bool TranscendentDragionEffect()
        {
            return true;
        }

        private bool BaronneSpSummon()
        {
            return true;
        }

        private bool BaronneEffect()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                return true; // Omni-negate
            }
            // Pop 1 card on field
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool DisPaterSpSummon()
        {
            return true;
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

        private bool KuibeltSpSummon()
        {
            return true;
        }

        private bool KuibeltEffect()
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

        private bool MoonlightSpSummon()
        {
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Level >= 5);
        }

        private bool MoonlightEffect()
        {
            ClientCard oppTarget = Enemy.GetMonsters().Where(m => m.IsFaceup() && m.Level >= 5).OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
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

        public override bool OnSelectYesNo(long desc)
        {
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search (hint 506 = HINTMSG_ATOHAND or all candidates from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.Where(c => c.Id == CardId.SangenSummoning ||
                                                    c.Id == CardId.TenpaiDragonPaidra ||
                                                    c.Id == CardId.TenpaiDragonChundra ||
                                                    c.Id == CardId.SangenKaimen).ToList();
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
