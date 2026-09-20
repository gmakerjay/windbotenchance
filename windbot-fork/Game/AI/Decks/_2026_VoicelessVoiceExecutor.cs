// ============================================================================
// CARD AUDIT — _2026_VoicelessVoice (Voiceless Voice — Untargetable Ritual Omni-Negate Control)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Lo, Prayers of the Voiceless Voice | Monster L1   | No   | Yes   | None    | NS/SS: Place Barrier/Radiance; GY revive on SS| Turn 1 starter; place Barrier; self-revive   | Already used this turn                      |
// | Saffira, Dragon Queen              | Monster L6   | Yes  | Yes   | Discard | Discard+dump Prayers -> search; GY Ritual SS  | Main Phase 1: search Skull Guardian -> SS    | Already used this turn                      |
// | Diviner of the Herald              | Monster L2 T | Yes  | No    | Send ED | Send Herald of the Arc Light -> search Ritual | Normal Summon starter; dump Herald -> search | No Herald in Extra Deck                     |
// | Skull Guardian, Protector          | Ritual L7    | Yes  | Yes   | None    | 4100 ATK under Lo; Omni-Negate; search on SS  | On Summon search; Quick Omni-Negate under Lo  | No Lo on field for negate                   |
// | Saffira, Divine Dragon             | Ritual L7    | Yes  | Yes   | None    | Shuffles spells to pop up to 2 opp cards      | Turn 2 board breaker                          | Opponent controls 0 cards                   |
// | Sauravis, Ancient & Ascended       | Ritual L7    | Yes  | Yes   | Discard | Handtrap target negate; on field SS negate    | Opponent targets friendly card or SS monster  | No targets                                  |
// | Barrier of the Voiceless Voice     | Spell Cont   | Yes  | Yes   | None    | Untargetable LIGHT monsters; search every turn| Turn 1 setup: search Saffira/Skull Guardian  | Already on field                            |
// | Prayers of the Voiceless Voice     | Spell Ritual | Yes  | Yes   | Tribute | Ritual Summon LIGHT Ritual; GY recover        | Ritual Summon Skull Guardian                  | No valid tribute / monster in hand          |
// | Radiance of the Voiceless Voice    | Trap Cont    | Yes  | Yes   | Shuffle | Non-targeting pop up to cards shuffled; SS Rit| Opponent threat on field / Quick disruption  | No targets on opponent field                |
// | Pre-Preparation of Rites           | Spell Normal | Yes  | Yes   | None    | Search Prayers + Skull Guardian in 1 card     | Turn 1 starter: immediate +1 search          | Deck has no Prayers or Skull Guardian       |
// | Preparation of Rites               | Spell Normal | No   | No    | None    | Search Level <= 7 Ritual monster; add spell GY| Main Phase search: search Skull Guardian      | Deck has no Level <= 7 Rituals              |
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
    [Deck("_2026_VoicelessVoice", "_2026_VoicelessVoice")]
    public class _2026_VoicelessVoiceExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int LoThePrayers = 25801745;
            public const int SaffiraDragonQueen = 51296484;
            public const int DivinerOfTheHerald = 92919429;
            public const int SkullGuardian = 10774240;
            public const int SaffiraDivineDragon = 10804018;
            public const int SauravisAncientAscended = 4810828;
            public const int SauravisDragonSage = 88284599;

            // Spells & Traps
            public const int BarrierOfTheVoicelessVoice = 98477480;
            public const int PrayersOfTheVoicelessVoice = 52472775;
            public const int BlessingOfTheVoicelessVoice = 39114494;
            public const int RadianceOfTheVoicelessVoice = 86310763;
            public const int PrePreparationOfRites = 13048472;
            public const int PreparationOfRites = 44155002;
            public const int PotOfProsperity = 84211599;
            public const int CalledByTheGrave = 24224830;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int HeraldOfTheArcLight = 79606837;
            public const int ElderEntityNtss = 80532587;
            public const int BaronneDeFleur = 84815190;
            public const int DynaMondo = 54447022;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int RelinquishedAnima = 94259633;
            public const int Bagooska = 2625939;
            public const int AbyssDweller = 21044178;
            public const int IPMasquerena = 65741786;
            public const int TyPhon = 12470404;
        }

        public _2026_VoicelessVoiceExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Pre-Preparation of Rites Instant Search ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "VV-PrePrep-Starter",
                RequiredCards = new List<int> { CardId.PrePreparationOfRites },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.PrePreparationOfRites, ActionType = ExecutorType.Activate, Description = "Search Prayers and Skull Guardian" }
                },
                FallbackLineName = "VV-Lo-Setup"
            });

            // ── Line 2: Lo 1-Card Setup Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "VV-Lo-Setup",
                RequiredCards = new List<int> { CardId.LoThePrayers },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.LoThePrayers, ActionType = ExecutorType.Summon, Description = "Normal Summon Lo" },
                    new() { CardId = CardId.LoThePrayers, ActionType = ExecutorType.Activate, Description = "Place Barrier of the Voiceless Voice" }
                },
                FallbackLineName = "VV-Diviner-Starter"
            });

            // ── Line 3: Diviner Dump Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "VV-Diviner-Starter",
                RequiredCards = new List<int> { CardId.DivinerOfTheHerald },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.DivinerOfTheHerald, ActionType = ExecutorType.Summon, Description = "Normal Summon Diviner of the Herald" },
                    new() { CardId = CardId.DivinerOfTheHerald, ActionType = ExecutorType.Activate, Description = "Dump Herald of the Arc Light to search" }
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
            AddExecutor(ExecutorType.Activate, CardId.SauravisAncientAscended, SauravisHandtrapActivate);

            // Skull Guardian — 4100 ATK Quick Omni-Negate
            AddExecutor(ExecutorType.Activate, CardId.SkullGuardian, SkullGuardianActivate);

            // Radiance of the Voiceless Voice — Non-targeting pop
            AddExecutor(ExecutorType.Activate, CardId.RadianceOfTheVoicelessVoice, RadianceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: RITUAL SEARCHERS & SPELL CARDS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePrepActivate);
            AddExecutor(ExecutorType.Activate, CardId.PreparationOfRites, PrepActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);

            AddExecutor(ExecutorType.Activate, CardId.BarrierOfTheVoicelessVoice, BarrierActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlessingOfTheVoicelessVoice, BlessingActivate);
            AddExecutor(ExecutorType.Activate, CardId.PrayersOfTheVoicelessVoice, PrayersActivate);

            // Saffira, Dragon Queen discard to search & GY Ritual Summon
            AddExecutor(ExecutorType.Activate, CardId.SaffiraDragonQueen, SaffiraDragonQueenActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMONS & MONSTER EFFECTS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.LoThePrayers, LoSummon);
            AddExecutor(ExecutorType.Activate, CardId.LoThePrayers, LoEffect);

            AddExecutor(ExecutorType.Summon, CardId.DivinerOfTheHerald, DivinerSummon);
            AddExecutor(ExecutorType.Activate, CardId.DivinerOfTheHerald, DivinerEffect);

            // GY Triggers (Herald of the Arc Light search / Elder Entity N'tss pop)
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight, HeraldEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssEffect);

            // Ritual Monster Summons
            AddExecutor(ExecutorType.SpSummon, CardId.SkullGuardian);
            AddExecutor(ExecutorType.SpSummon, CardId.SaffiraDivineDragon);
            AddExecutor(ExecutorType.Activate, CardId.SaffiraDivineDragon, SaffiraDivineDragonEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SauravisAncientAscended);

            // Fallback summon
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: EXTRA DECK UTILITY
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.DynaMondo, DynaMondoSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DynaMondo, DynaMondoEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.RadianceOfTheVoicelessVoice, SpellSetStrategy);
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

        private bool SauravisHandtrapActivate()
        {
            // Negate an effect targeting friendly cards
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool SkullGuardianActivate()
        {
            ClientCard lastCard = LastChainCard;
            // Case 1: Quick Effect Omni-Negate
            if (lastCard != null && lastCard.Controller == 1)
            {
                bool hasLo = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.LoThePrayers);
                return hasLo;
            }

            // Case 2: On Ritual Summon search
            AI.SelectCard(CardId.SauravisAncientAscended, CardId.RadianceOfTheVoicelessVoice, CardId.BarrierOfTheVoicelessVoice);
            return true;
        }

        private bool RadianceActivate()
        {
            // Shuffles 1 LIGHT Ritual or S/T from hand/GY to pop cards on field
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                ClientCard shuffleTarget = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.PrayersOfTheVoicelessVoice || c.Id == CardId.SaffiraDragonQueen)
                                        ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.PrayersOfTheVoicelessVoice);
                ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                    ?? Enemy.GetSpells().FirstOrDefault();

                if (shuffleTarget != null && oppTarget != null)
                {
                    AI.SelectCard(shuffleTarget);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCHERS & SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool PrePrepActivate()
        {
            AI.SelectCard(CardId.PrayersOfTheVoicelessVoice);
            AI.SelectNextCard(CardId.SkullGuardian);
            return true;
        }

        private bool PrepActivate()
        {
            AI.SelectCard(CardId.SkullGuardian, CardId.SaffiraDragonQueen, CardId.SauravisAncientAscended);
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            var banishList = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.HeraldOfTheArcLight &&
                c.Id != CardId.ElderEntityNtss
            ).Take(6).ToList();

            if (banishList.Count >= 3)
            {
                AI.SelectCard(banishList);
                return true;
            }
            return false;
        }

        private bool BarrierActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.BarrierOfTheVoicelessVoice);
            }
            // Once per turn search from deck
            AI.SelectCard(CardId.SaffiraDragonQueen, CardId.SkullGuardian, CardId.PrayersOfTheVoicelessVoice);
            return true;
        }

        private bool BlessingActivate()
        {
            return true;
        }

        private bool PrayersActivate()
        {
            // Tribute Lo as entire tribute
            ClientCard loTribute = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.LoThePrayers)
                                ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.LoThePrayers);
            if (loTribute != null)
            {
                AI.SelectCard(loTribute);
            }
            return true;
        }

        private bool SaffiraDragonQueenActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard self + dump Prayers to search Skull Guardian
                AI.SelectCard(CardId.PrayersOfTheVoicelessVoice);
                AI.SelectNextCard(CardId.SkullGuardian, CardId.SauravisAncientAscended);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Banish self from GY to Ritual Summon Skull Guardian
                ClientCard loTribute = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.LoThePrayers)
                                    ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.LoThePrayers);
                if (loTribute != null)
                {
                    AI.SelectCard(loTribute);
                }
                return true;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool LoSummon()
        {
            return true;
        }

        private bool LoEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Place Barrier or Radiance face-up in Spell/Trap zone
                if (!Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.BarrierOfTheVoicelessVoice))
                {
                    AI.SelectCard(CardId.BarrierOfTheVoicelessVoice);
                }
                else
                {
                    AI.SelectCard(CardId.RadianceOfTheVoicelessVoice);
                }
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Self-revive upon LIGHT Ritual Summon
                return true;
            }
            return true;
        }

        private bool DivinerSummon()
        {
            return true;
        }

        private bool DivinerEffect()
        {
            // Dump Herald of the Arc Light to search Ritual card
            AI.SelectCard(CardId.HeraldOfTheArcLight);
            return true;
        }

        private bool HeraldEffect()
        {
            // Search Ritual Monster or Ritual Spell
            if (!Bot.HasInHand(CardId.SkullGuardian))
            {
                AI.SelectCard(CardId.SkullGuardian);
            }
            else if (!Bot.HasInHand(CardId.PrayersOfTheVoicelessVoice))
            {
                AI.SelectCard(CardId.PrayersOfTheVoicelessVoice);
            }
            else
            {
                AI.SelectCard(CardId.SaffiraDragonQueen, CardId.SauravisAncientAscended);
            }
            return true;
        }

        private bool NtssEffect()
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

        private bool SaffiraDivineDragonEffect()
        {
            var oppTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (oppTargets.Count > 0)
            {
                AI.SelectCard(oppTargets);
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK UTILITY
        // ═══════════════════════════════════════════════════════════════

        private bool DynaMondoSpSummon()
        {
            // Only summon in MP2 or when Ritual is already in GY
            return Duel.Phase == DuelPhase.Main2 && Bot.Graveyard.Any(c => c.IsMonster() && c.HasType(CardType.Ritual));
        }

        private bool DynaMondoEffect()
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

        private bool BaronneSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.Id == CardId.DivinerOfTheHerald && m.Level == 6);
        }

        private bool BaronneEffect()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1) return true;
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            // NEVER sacrifice Lo or Skull Guardian for S:P
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Count(m => m.Id != CardId.LoThePrayers && m.Id != CardId.SkullGuardian) >= 2;
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
                    var preferred = cards.Where(c => c.Id == CardId.BarrierOfTheVoicelessVoice ||
                                                    c.Id == CardId.LoThePrayers ||
                                                    c.Id == CardId.SkullGuardian ||
                                                    c.Id == CardId.PrayersOfTheVoicelessVoice ||
                                                    c.Id == CardId.SaffiraDragonQueen).ToList();
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
