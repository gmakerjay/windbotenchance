// ============================================================================
// CARD AUDIT — Anime_Shark (Reginald Kastle / Nash's Barian Water Xyz Army)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Ghost Belle & Haunted Mansion      | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate GY move/banish/revive        | Opponent activates GY interaction             | Bot's own turn without threat               |
// | Infinite Impermanence              | Trap Normal  | Yes  | Yes   | None    | Negate 1 face-up monster; column S/T negate   | Opponent monster activates or dangerous boss  | Target already negated                      |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Harpie's Feather Duster            | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls| Opponent controls 1+ Spell/Trap cards         | Opponent controls 0 Spells/Traps            |
// | Forbidden Droplet                  | Spell Quick  | Yes  | No    | Send S/M| Negate monsters without response & halve ATK  | Opponent board with dangerous negators/threats| Opponent controls 0 face-up effect monsters |
// | Foolish Burial                     | Spell Normal | No   | No    | None    | Send 1 monster from Deck to GY (Crystal Shark)| Main Phase 1 setup extender                   | Already have Crystal Shark in GY            |
// | Monster Reborn                     | Spell Normal | No   | No    | None    | Special Summon 1 monster from either GY       | Main Phase 1 extend Xyz materials or boss     | Both GYs empty                              |
// | Barian Untopia                     | Spell Field  | Yes  | Yes   | None    | Protect Barian/Number Xyz; set Barian S/T     | Main Phase 1 setup                            | Already face-up on field                    |
// | Rank-Up-Magic Barian's Force       | Spell Normal | No   | No    | Target  | Rank-up Xyz into Chaos Xyz, steal 1 material  | Control Rank 4/5 Xyz, have C-Xyz in Extra     | No target Xyz on field                      |
// | Virtue Stream                      | Trap Normal  | Yes  | Yes   | Destroy | Destroy 1 WATER mon -> destroy 2 opp cards    | Opponent has 2+ threat cards on field         | Bot controls no WATER monsters              |
// | Xyz Revive Splash                  | Trap Normal  | Yes  | Yes   | Target  | Revive Rank 4 or lower Xyz; GY rank-up WATER  | Target Xyz in GY, or GY banish to rank-up     | No Xyz in GY                                |
// | Buzzsaw Shark                      | Monster L4   | Yes  | Yes   | Target  | SS 1 Fish from Deck with same / +/-1 Level     | Normal/Special Summoned (primary 1-card Xyz)  | Already used this turn                      |
// | Lantern Shark                      | Monster L4   | Yes  | Yes   | None    | On Summon: SS Level 3/4/5 WATER from hand     | In hand with other WATER monster              | Hand has no other WATER monsters            |
// | Crystal Shark                      | Monster L5   | Yes  | Yes   | Target  | SS from hand/GY by halving a WATER ATK        | In hand/GY, need Xyz material                 | Field has no WATER monsters                 |
// | Armored Shark                      | Monster L4   | Yes  | Yes   | Send mon| SS from hand; send WATER from Deck to GY      | In hand, need extender                        | Already used this turn                      |
// | Surfacing Big Jaws                 | Monster L4   | Yes  | Yes   | None    | SS from hand if spell activated; search Shark | In hand, need extender                        | Already used this turn                      |
// | Drake Shark                        | Monster L4   | Yes  | Yes   | None    | SS from hand if WATER added to hand           | In hand, trigger on search                    | Already used this turn                      |
// | N.As.H. Knight                     | Xyz Rank 5   | Yes  | Yes   | Detach 2| Attach #101-107 from Extra + ATTACH OPP MON!  | Opponent controls face-up monster (Non-target)| Opponent controls no monsters               |
// | CXyz N.As.Ch. Knight               | Xyz Rank 6   | Yes  | Yes   | Detach 1| Monster effect immune; cheat Over-Hundred #   | Ranked up from N.As.H.; battle/interruption   | Extra Deck empty                            |
// | Full Armored Crystalzero Lancer    | Xyz Rank 6   | Yes  | Yes   | Detach 1| 3700+ ATK; NEGATE ALL OPP MONSTERS ON FIELD!  | Opponent has active effect monsters on field  | Opponent monsters already negated           |
// | Number C101: Silent Honor DARK     | Xyz Rank 5   | Yes  | Yes   | Target  | Attach 1 Special Summoned opp monster; float  | Opponent controls Special Summoned monster    | No targets                                  |
// | Number 101: Silent Honor ARK       | Xyz Rank 4   | Yes  | Yes   | Detach 2| Attach 1 opp Special Summoned monster in ATK  | Opponent controls ATK Special Summoned monster| No valid targets                            |
// | Valiant Shark Lancer               | Xyz Rank 5   | Yes  | Yes   | Detach 1| Quick: Target 1 opp monster; destroy it       | Opponent monster threat or activation         | Opponent controls 0 monsters                |
// | Number C32: Shark Drake LeVeiss    | Xyz Rank 4   | Yes  | Yes   | Detach 1| Quick: Negate all opp monsters + banish S/T   | Opponent activates monster effect on field    | Already negated                             |
// | Number 37: Hope Woven Dragon Spider| Xyz Rank 4   | Yes  | Yes   | Detach 1| Drop all opp monsters ATK by 1000 on attack   | Battle Phase attack or defense                | No battle                                   |
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
    [Deck("Anime_Shark", "Anime_Shark")]
    public class Anime_SharkExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int BuzzsawShark = 7150545;
            public const int LanternShark = 70156946;
            public const int CrystalShark = 98881700;
            public const int ArmoredShark = 27480536;
            public const int SurfacingBigJaws = 55697723;
            public const int DrakeShark = 81096431;
            public const int AshBlossom = 14558127;
            public const int GhostBelle = 73642296;

            // Spells
            public const int BarianUntopia = 30761649;
            public const int RankUpMagicBariansForce = 47660516;
            public const int FoolishBurial = 81439173;
            public const int MonsterReborn = 83764718;
            public const int HarpiesFeatherDuster = 18144506;
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;

            // Traps
            public const int InfiniteImpermanence = 10045474;
            public const int VirtueStream = 80534031;
            public const int XyzReviveSplash = 32764863;

            // Extra Deck
            public const int NAsHKnight = 34876719;
            public const int CXyzNAsChKnight = 61374414;
            public const int Number101SilentHonorARK = 48739166;
            public const int NumberC101SilentHonorDARK = 12744567;
            public const int FullArmoredCrystalzeroLancer = 99469936;
            public const int ValiantSharkLancer = 23672629;
            public const int Number37SpiderShark = 37279508;
            public const int Number32SharkDrake = 65676461;
            public const int NumberC32SharkDrakeLeVeiss = 7628844;
            public const int Number71RebarianShark = 59479050;
            public const int FullArmoredUtopicRayLancer = 1269512;
        }

        public Anime_SharkExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Buzzsaw Shark 1-Card Xyz ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Shark-Buzzsaw-1Card-Xyz",
                RequiredCards = new List<int> { CardId.BuzzsawShark },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BuzzsawShark, ActionType = ExecutorType.Summon, Description = "Normal Summon Buzzsaw Shark" },
                    new() { CardId = CardId.BuzzsawShark, ActionType = ExecutorType.Activate, Description = "Buzzsaw Shark SS Lantern/Crystal Shark from Deck" },
                    new() { CardId = CardId.NAsHKnight, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon N.As.H. Knight" },
                    new() { CardId = CardId.NAsHKnight, ActionType = ExecutorType.Activate, Description = "N.As.H. Knight attach #101 + attach enemy monster!" }
                },
                FallbackLineName = "Shark-Extender-Xyz"
            });

            // ── Line 2: Lantern Shark Extender Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Shark-Extender-Xyz",
                RequiredCards = new List<int> { CardId.LanternShark },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.LanternShark, ActionType = ExecutorType.Summon, Description = "Normal Summon Lantern Shark" },
                    new() { CardId = CardId.LanternShark, ActionType = ExecutorType.Activate, Description = "Lantern Shark SS WATER monster from hand" },
                    new() { CardId = CardId.Number101SilentHonorARK, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Number 101" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK DISRUPTIONS, FIELD NEGATIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Full Armored Crystalzero Lancer — Negate all face-up opponent monsters
            AddExecutor(ExecutorType.Activate, CardId.FullArmoredCrystalzeroLancer, CrystalzeroNegateActivate);

            // N.As.H. Knight — Non-targeting monster absorption (Quick Effect)
            AddExecutor(ExecutorType.Activate, CardId.NAsHKnight, NAsHKnightAbsorbActivate);

            // CXyz N.As.Ch. Knight — Quick detach & cheat Over-Hundred Numbers
            AddExecutor(ExecutorType.Activate, CardId.CXyzNAsChKnight, CXyzNAsChActivate);

            // Valiant Shark Lancer — Quick targeted destruction
            AddExecutor(ExecutorType.Activate, CardId.ValiantSharkLancer, ValiantSharkDestroyActivate);

            // Number C32: Shark Drake LeVeiss — Quick negate all monsters + banish S/T
            AddExecutor(ExecutorType.Activate, CardId.NumberC32SharkDrakeLeVeiss, LeVeissNegateActivate);

            // Virtue Stream — Destroy 1 friendly WATER to pop 2 opponent cards
            AddExecutor(ExecutorType.Activate, CardId.VirtueStream, VirtueStreamActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE SPELLS & EXTENSION
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.BarianUntopia, BarianUntopiaActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);
            AddExecutor(ExecutorType.Activate, CardId.RankUpMagicBariansForce, BariansForceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMON STARTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.BuzzsawShark, BuzzsawSharkSummon);
            AddExecutor(ExecutorType.Activate, CardId.BuzzsawShark, BuzzsawSharkEffect);

            AddExecutor(ExecutorType.Summon, CardId.LanternShark, LanternSharkSummon);
            AddExecutor(ExecutorType.Activate, CardId.LanternShark, LanternSharkEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: HAND & GY EXTENDER SPECIAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CrystalShark, CrystalSharkEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArmoredShark, ArmoredSharkEffect);
            AddExecutor(ExecutorType.Activate, CardId.SurfacingBigJaws, SurfacingBigJawsEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrakeShark, DrakeSharkEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.SurfacingBigJaws, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArmoredShark, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CrystalShark, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK XYZ SUMMONS & IGNITION REMOVAL
            // ═══════════════════════════════════════════════════════════════

            // Rank 5 N.As.H. Knight
            AddExecutor(ExecutorType.SpSummon, CardId.NAsHKnight, NAsHKnightSpSummon);

            // Rank 6 CXyz N.As.Ch. Knight (Overlay on N.As.H.)
            AddExecutor(ExecutorType.SpSummon, CardId.CXyzNAsChKnight, CXyzNAsChSpSummon);

            // Rank 6 Full Armored Crystalzero Lancer (Overlay on Rank 5)
            AddExecutor(ExecutorType.SpSummon, CardId.FullArmoredCrystalzeroLancer, CrystalzeroSpSummon);

            // Rank 5 Valiant Shark Lancer
            AddExecutor(ExecutorType.SpSummon, CardId.ValiantSharkLancer, ValiantSharkSpSummon);

            // Rank 5 Number C101: Silent Honor DARK (Ignition absorb)
            AddExecutor(ExecutorType.SpSummon, CardId.NumberC101SilentHonorDARK, NumberC101SpSummon);
            AddExecutor(ExecutorType.Activate, CardId.NumberC101SilentHonorDARK, NumberC101AbsorbEffect);

            // Rank 4 Number 101: Silent Honor ARK (Ignition absorb)
            AddExecutor(ExecutorType.SpSummon, CardId.Number101SilentHonorARK, Number101SpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number101SilentHonorARK, Number101AbsorbEffect);

            // Other Xyz Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.NumberC32SharkDrakeLeVeiss, LeVeissSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number32SharkDrake, SharkDrakeSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number37SpiderShark, SpiderSharkSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number37SpiderShark, SpiderSharkEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FullArmoredUtopicRayLancer, UtopicRaySpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.VirtueStream, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.XyzReviveSplash, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & QUICK EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool CrystalzeroNegateActivate()
        {
            // Negate all face-up monsters opponent controls
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool oppHasActive = Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect));
                if (oppHasActive)
                {
                    return true;
                }
            }
            return false;
        }

        private bool NAsHKnightAbsorbActivate()
        {
            // Detach 2 -> Attach #101 from Extra + attach 1 face-up monster on field!
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard oppTarget = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup())
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (oppTarget != null)
                {
                    AI.SelectCard(CardId.Number101SilentHonorARK, CardId.NumberC101SilentHonorDARK);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool CXyzNAsChActivate()
        {
            // Detach 1 -> Special Summon Over-Hundred Number from Extra Deck
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.NumberC101SilentHonorDARK, CardId.Number101SilentHonorARK);
                return true;
            }
            return false;
        }

        private bool ValiantSharkDestroyActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup())
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool LeVeissNegateActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                return true;
            }
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled());
        }

        private bool VirtueStreamActivate()
        {
            // Destroy 1 WATER monster we control -> destroy 2 opponent cards
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2)
            {
                ClientCard fodder = Bot.GetMonsters()
                    .Where(m => m.HasAttribute(CardAttribute.Water) && !m.HasType(CardType.Xyz))
                    .OrderBy(m => m.Attack)
                    .FirstOrDefault()
                    ?? Bot.GetMonsters().FirstOrDefault(m => m.HasAttribute(CardAttribute.Water));

                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    var oppTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Take(2).ToList();
                    AI.SelectNextCard(oppTargets);
                    return true;
                }
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

        private bool GhostBelleActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool InfiniteImpermanenceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ForbiddenDropletActivate()
        {
            var targets = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled())
                .OrderByDescending(m => m.Attack)
                .ToList();

            if (targets.Count > 0)
            {
                ClientCard fodder = Bot.Hand.FirstOrDefault(c => c.Id == CardId.CrystalShark)
                                 ?? Bot.Hand.FirstOrDefault(c => c != Card);
                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    AI.SelectNextCard(targets);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SPELLS & EXTENDERS
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool BarianUntopiaActivate()
        {
            return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.BarianUntopia);
        }

        private bool FoolishBurialActivate()
        {
            AI.SelectCard(CardId.CrystalShark);
            return true;
        }

        private bool MonsterRebornActivate()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BariansForceActivate()
        {
            ClientCard targetXyz = Bot.GetMonsters()
                .FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.Number101SilentHonorARK);

            if (targetXyz != null)
            {
                AI.SelectCard(targetXyz);
                AI.SelectNextCard(CardId.NumberC101SilentHonorDARK);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTER SUMMONS & EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool BuzzsawSharkSummon()
        {
            return true;
        }

        private bool BuzzsawSharkEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(Card);
                AI.SelectNextCard(CardId.LanternShark, CardId.CrystalShark, CardId.ArmoredShark);
                return true;
            }
            return false;
        }

        private bool LanternSharkSummon()
        {
            return true;
        }

        private bool LanternSharkEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard handWater = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.HasAttribute(CardAttribute.Water));
                if (handWater != null)
                {
                    AI.SelectCard(handWater);
                    return true;
                }
            }
            return false;
        }

        private bool CrystalSharkEffect()
        {
            // Special summon from hand or GY by targeting WATER monster on field
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Water));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ArmoredSharkEffect()
        {
            return true;
        }

        private bool SurfacingBigJawsEffect()
        {
            return true;
        }

        private bool DrakeSharkEffect()
        {
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK XYZ SUMMONS & REMOVAL
        // ═══════════════════════════════════════════════════════════════

        private bool NAsHKnightSpSummon()
        {
            return true;
        }

        private bool CXyzNAsChSpSummon()
        {
            return true;
        }

        private bool CrystalzeroSpSummon()
        {
            return true;
        }

        private bool ValiantSharkSpSummon()
        {
            return true;
        }

        private bool NumberC101SpSummon()
        {
            return true;
        }

        private bool NumberC101AbsorbEffect()
        {
            // Attach 1 Special Summoned monster opponent controls
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup())
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true; // GY float revival
        }

        private bool Number101SpSummon()
        {
            return true;
        }

        private bool Number101AbsorbEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup() && m.IsAttack())
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool LeVeissSpSummon()
        {
            return true;
        }

        private bool SharkDrakeSpSummon()
        {
            return true;
        }

        private bool SpiderSharkSpSummon()
        {
            return true;
        }

        private bool SpiderSharkEffect()
        {
            return true;
        }

        private bool UtopicRaySpSummon()
        {
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
                var preferred = cards.Where(c => c.Id == CardId.LanternShark ||
                                                c.Id == CardId.CrystalShark ||
                                                c.Id == CardId.BuzzsawShark ||
                                                c.Id == CardId.Number101SilentHonorARK).ToList();
                if (preferred.Count >= min)
                {
                    return preferred.Take(max).ToList();
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
