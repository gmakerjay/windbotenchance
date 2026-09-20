// ============================================================================
// CARD AUDIT — Anime_Pegasus (Maximillion Pegasus's Modern Toon Kingdom Control)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Toon Bookmark                      | Spell Normal | Yes  | Yes   | None    | Search Toon World / Kingdom; GY protect Kingdm| Main Phase 1: search Toon Kingdom             | Already have Toon Kingdom on field          |
// | Toon Table of Contents             | Spell Normal | No   | No    | None    | Search ANY "Toon" card from Deck to hand      | Main Phase 1: search key Toon piece           | Deck has no Toon cards                      |
// | Toon Kingdom                       | Spell Field  | Yes  | No    | Banish 3| All Toons IMMUNE to targeting & destruction   | Main Phase 1 setup                            | Already face-up on field                    |
// | Toon Page-Flip                     | Spell Quick  | Yes  | Yes   | Reveal 3| Special Summon 1 random Toon monster from Deck| Control Toon World; need field presence       | Deck has < 3 Toon monsters                  |
// | Comic Hand                         | Spell Equip  | No   | No    | Equip   | Steal ANY opponent monster, make it direct atk| Opponent controls dangerous boss monster      | Opponent controls 0 face-up monsters        |
// | Shadow Toon                        | Spell Normal | Yes  | Yes   | Target  | Burn opponent equal to target monster's ATK   | Opponent controls monster with high ATK (2500+)| Opponent controls 0 face-up monsters        |
// | Toon Terror                        | Trap Counter | Yes  | Yes   | None    | Omni-Negate (Monster/Spell/Trap) + destroy    | Opponent activates card/effect                | No Toon World or Toon monster on field      |
// | Toon Briefcase                     | Trap Normal  | No   | No    | None    | Shuffle opponent monster summon into Deck     | Opponent summons a monster                    | No Toon monster on field                    |
// | Toon Black Luster Soldier          | Monster L8   | Yes  | Yes   | None    | 3000 ATK direct atk; banish 1 card on field   | In hand; Special Summon or banish enemy card  | Cannot banish                               |
// | Toon Dark Magician                 | Monster L7   | Yes  | Yes   | Discard | Discard 1 Toon -> SS from Deck or search S/T  | In hand/field; discard to swarm Toons         | Hand has no Toon cards to discard           |
// | Red-Eyes Toon Dragon               | Monster L7   | Yes  | Yes   | None    | Special Summon 1 Toon monster from hand       | In hand/field; swarm Toons from hand          | Hand has no other Toons                     |
// | Toon Harpie Lady                   | Monster L4   | Yes  | Yes   | None    | SS if control Toon World; destroy 1 opp S/T   | In hand; pop opponent backrow                 | No Toon World on field                      |
// | Toon Cyber Dragon                  | Monster L5   | No   | No    | None    | SS if opponent controls monster; direct attack| In hand; free Level 5 body                    | Bot controls monsters and opp has none      |
// | Relinquished Anima                 | Link-1       | Yes  | Yes   | None    | Pegasus' signature Link: Steal pointed monster| Link summon; point to opponent monster to steal| Pointed zone has no opponent monster        |
// | S:P Little Knight                  | Link-2       | Yes  | Yes   | Target  | Banish card on summon; quick dodge banish     | Opponent threat or card activation            | Target already removed                      |
// | Number 11: Big Eye                 | Xyz Rank 7   | No   | No    | Detach 1| Permanently take control of 1 opponent monster| Opponent controls dangerous monster           | Opponent controls 0 monsters                |
// | Number 38: Hope Harbinger          | Xyz Rank 8   | Yes  | Yes   | Detach 1| Negate opponent Spell card & attach as material| Opponent activates Spell card                 | Already negated this turn                   |
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
    [Deck("Anime_Pegasus", "Anime_Pegasus")]
    public class Anime_PegasusExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int ToonBlackLusterSoldier = 28711704;
            public const int ToonDarkMagician = 21296502;
            public const int RedEyesToonDragon = 31733941;
            public const int ToonHarpieLady = 64116319;
            public const int ToonCyberDragon = 83629030;
            public const int AshBlossom = 14558127;

            // Spells
            public const int ToonBookmark = 91500017;
            public const int ToonTableOfContents = 89997728;
            public const int ToonKingdom = 43175858;
            public const int ToonPageFlip = 27699122;
            public const int ComicHand = 33453260;
            public const int ShadowToon = 6958551;
            public const int CalledByTheGrave = 24224830;

            // Traps
            public const int ToonTerror = 53094821;
            public const int ToonBriefcase = 5832914;

            // Extra Deck
            public const int RelinquishedAnima = 94259633;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareCerberus = 75452921;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int Number11BigEye = 80117527;
            public const int MechaPhantomBeastDracossack = 22110647;
            public const int HopeHarbinger = 63767246;
        }

        public Anime_PegasusExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Toon Kingdom Setup Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Toon-Kingdom-Setup",
                RequiredCards = new List<int> { CardId.ToonBookmark },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ToonBookmark, ActionType = ExecutorType.Activate, Description = "Bookmark searches Toon Kingdom" },
                    new() { CardId = CardId.ToonKingdom, ActionType = ExecutorType.Activate, Description = "Activate Toon Kingdom (Field Spell)" }
                },
                FallbackLineName = "Toon-Table-Starter"
            });

            // ── Line 2: Toon Table Searcher Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Toon-Table-Starter",
                RequiredCards = new List<int> { CardId.ToonTableOfContents },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ToonTableOfContents, ActionType = ExecutorType.Activate, Description = "Table searches Toon Bookmark or Kingdom" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Toon Terror — Omni-Negate Counter Trap
            AddExecutor(ExecutorType.Activate, CardId.ToonTerror, ToonTerrorActivate);

            // Toon Briefcase — Shuffle opponent summoned monster into Deck
            AddExecutor(ExecutorType.Activate, CardId.ToonBriefcase, ToonBriefcaseActivate);

            // Hope Harbinger — Negate Spell card
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger, HopeHarbingerActivate);

            // S:P Little Knight — Quick dodge / temporary banish
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE SPELLS & SEARCHERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ToonBookmark, ToonBookmarkActivate);
            AddExecutor(ExecutorType.Activate, CardId.ToonTableOfContents, ToonTableActivate);
            AddExecutor(ExecutorType.Activate, CardId.ToonKingdom, ToonKingdomActivate);
            AddExecutor(ExecutorType.Activate, CardId.ToonPageFlip, ToonPageFlipActivate);
            AddExecutor(ExecutorType.Activate, CardId.ComicHand, ComicHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShadowToon, ShadowToonActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: TOON SUMMONS & MONSTER EFFECTS
            // ═══════════════════════════════════════════════════════════════

            // Toon Harpie Lady (SS + pop S/T)
            AddExecutor(ExecutorType.SpSummon, CardId.ToonHarpieLady, ToonHarpieLadySpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ToonHarpieLady, ToonHarpieLadySummon);
            AddExecutor(ExecutorType.Activate, CardId.ToonHarpieLady, ToonHarpieLadyEffect);

            // Toon Cyber Dragon
            AddExecutor(ExecutorType.SpSummon, CardId.ToonCyberDragon, ToonCyberDragonSpSummon);

            // Toon Black Luster Soldier (SS + banish card)
            AddExecutor(ExecutorType.SpSummon, CardId.ToonBlackLusterSoldier, ToonBLSSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ToonBlackLusterSoldier, ToonBLSEffect);

            // Toon Dark Magician (Discard to SS or search)
            AddExecutor(ExecutorType.Activate, CardId.ToonDarkMagician, ToonDMEffect);

            // Red-Eyes Toon Dragon (SS Toon from hand)
            AddExecutor(ExecutorType.Activate, CardId.RedEyesToonDragon, RedEyesToonEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.ToonCyberDragon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: EXTRA DECK STEAL & REMOVAL
            // ═══════════════════════════════════════════════════════════════

            // Relinquished Anima (Steal pointed monster)
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaEffect);

            // Big Eye (Permanently steal opponent monster)
            AddExecutor(ExecutorType.SpSummon, CardId.Number11BigEye, BigEyeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number11BigEye, BigEyeEffect);

            // Hope Harbinger
            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, HopeHarbingerSpSummon);

            // S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);

            // Accesscode Talker
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.ToonTerror, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ToonBriefcase, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & TRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool ToonTerrorActivate()
        {
            // Omni-Negate Counter Trap
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                bool hasToonWorld = Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom);
                bool hasToonMon = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x62));
                return hasToonWorld && hasToonMon;
            }
            return false;
        }

        private bool ToonBriefcaseActivate()
        {
            return Duel.LastSummonPlayer == 1 && Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x62));
        }

        private bool HopeHarbingerActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1 && lastCard.IsSpell();
        }

        private bool SPLittleKnightActivate()
        {
            ClientCard lastCard = LastChainCard;
            // Case 1: Quick Effect in response to opponent's activation (dodge and banish opp monster)
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard oppTarget = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
                ClientCard botTarget = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.SPLittleKnight)
                                    ?? Bot.GetMonsters().OrderBy(m => m.Attack).FirstOrDefault();
                if (oppTarget != null && botTarget != null)
                {
                    AI.SelectCard(new[] { oppTarget, botTarget });
                    return true;
                }
                return false;
            }

            // Case 2: On Link Summon banish effect
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                             ?? Enemy.GetSpells().FirstOrDefault()
                             ?? Enemy.Graveyard.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
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

        private bool AshBlossomActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SPELLS & DRAW/SEARCH ENGINE
        // ═══════════════════════════════════════════════════════════════

        private bool ToonBookmarkActivate()
        {
            // Search Toon Kingdom or Toon Page-Flip
            if (!Bot.HasInSpellZone(CardId.ToonKingdom) && !Bot.HasInHand(CardId.ToonKingdom))
            {
                AI.SelectCard(CardId.ToonKingdom);
            }
            else
            {
                AI.SelectCard(CardId.ToonPageFlip, CardId.ToonTerror, CardId.ComicHand);
            }
            return true;
        }

        private bool ToonTableActivate()
        {
            if (!Bot.HasInSpellZone(CardId.ToonKingdom) && !Bot.HasInHand(CardId.ToonKingdom))
            {
                AI.SelectCard(CardId.ToonBookmark, CardId.ToonKingdom);
            }
            else if (!Bot.HasInHand(CardId.ToonBlackLusterSoldier))
            {
                AI.SelectCard(CardId.ToonBlackLusterSoldier);
            }
            else
            {
                AI.SelectCard(CardId.ToonPageFlip, CardId.ComicHand, CardId.ToonTerror);
            }
            return true;
        }

        private bool ToonKingdomActivate()
        {
            return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom);
        }

        private bool ToonPageFlipActivate()
        {
            // Reveal 3 Toon monsters
            AI.SelectCard(CardId.ToonBlackLusterSoldier, CardId.ToonDarkMagician, CardId.RedEyesToonDragon);
            return true;
        }

        private bool ComicHandActivate()
        {
            if (!Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom))
                return false;

            // Steal 1 face-up opponent monster that is not already stolen/equipped
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && m.Controller == 1 && !m.HasType(CardType.Token) && (m.EquipCards == null || !m.EquipCards.Any(e => e.Id == CardId.ComicHand)))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ShadowToonActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && m.Attack >= 2000)
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool ToonHarpieLadySpSummon()
        {
            return Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom);
        }

        private bool ToonHarpieLadySummon()
        {
            return true;
        }

        private bool ToonHarpieLadyEffect()
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

        private bool ToonCyberDragonSpSummon()
        {
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() == 0;
        }

        private bool ToonBLSSpSummon()
        {
            // Tribute 1 Level 7 or lower Toon or Special Summon
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level <= 7 && m.Id != CardId.ToonBlackLusterSoldier);
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return true;
        }

        private bool ToonBLSEffect()
        {
            // Banish 1 card on field face-up: ONLY target opponent's cards!
            ClientCard target = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault()
                             ?? Enemy.GetSpells().Where(s => s.IsFaceup()).FirstOrDefault()
                             ?? Enemy.GetMonsters().FirstOrDefault()
                             ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ToonDMEffect()
        {
            // Discard 1 Toon card to Special Summon or search
            ClientCard discard = Bot.Hand.FirstOrDefault(c => c.HasSetcode(0x62) && c != Card);
            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.ToonBlackLusterSoldier, CardId.RedEyesToonDragon, CardId.ToonTerror);
                return true;
            }
            return false;
        }

        private bool RedEyesToonEffect()
        {
            ClientCard target = Bot.Hand.FirstOrDefault(c => c.HasSetcode(0x62) && c != Card);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK
        // ═══════════════════════════════════════════════════════════════

        private bool RelinquishedAnimaSpSummon()
        {
            // Only summon if we have Level 1 monster and opponent has a monster in pointed column
            return Enemy.GetMonsterCount() > 0;
        }

        private bool RelinquishedAnimaEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool BigEyeSpSummon()
        {
            // Do NOT summon if we control a Comic Hand monster or can attack directly under Kingdom
            if (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ComicHand))
                return false;
            bool hasKingdom = Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom);
            if (hasKingdom && Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() == 0)
                return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500);
        }

        private bool BigEyeEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HopeHarbingerSpSummon()
        {
            // Only summon in MP2 or if opponent has heavy spell danger
            return Duel.Phase == DuelPhase.Main2 || Enemy.GetMonsterCount() > 0;
        }

        private bool SPLittleKnightSpSummon()
        {
            // NEVER sacrifice a monster stolen by Comic Hand (sends Comic Hand to GY!)
            if (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ComicHand))
                return false;

            // NEVER sacrifice high-ATK Toons under Toon Kingdom in Main Phase 1 (we want to attack directly!)
            bool hasKingdom = Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.ToonKingdom);
            if (hasKingdom && Duel.Phase == DuelPhase.Main1 && Bot.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2000))
                return false;

            // In MP2, S:P is an excellent end-board disruption
            if (Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() >= 2)
                return true;

            // In MP1, only summon if opponent has dangerous cards and we don't have direct attack advantage
            bool oppHasThreats = Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2000) || Enemy.GetSpells().Any();
            return oppHasThreats && Bot.GetMonsters().Count(m => m.IsFaceup() && m.Attack < 2000) >= 2;
        }

        private bool AccesscodeSpSummon()
        {
            return Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2;
        }

        private bool AccesscodeEffect()
        {
            var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS & CARD SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2 || Bot.GetMonsterCount() > 0;
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
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search (hint 506 = HINTMSG_ATOHAND or all candidates from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.Where(c => c.Id == CardId.ToonKingdom ||
                                                    c.Id == CardId.ToonBookmark ||
                                                    c.Id == CardId.ToonBlackLusterSoldier ||
                                                    c.Id == CardId.ToonTerror).ToList();
                    if (preferred.Count >= min)
                    {
                        return preferred.Take(max).ToList();
                    }
                }

                // Removal (Banish hint 503, Destroy hint 502, ToGrave hint 504): ALWAYS target Enemy cards!
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
