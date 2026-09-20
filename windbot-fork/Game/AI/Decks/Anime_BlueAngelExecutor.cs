// ============================================================================
// CARD AUDIT — Anime_BlueAngel (Skye Zaizen / Blue Angel's Trickstar Burn & Control)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Droll & Lock Bird                  | Monster L1   | Yes  | No    | Send    | Handtrap: Lock searching/drawing after 1 add  | Opponent adds card from Deck to hand          | Bot needs to add cards this turn            |
// | Infinite Impermanence              | Trap Normal  | Yes  | Yes   | None    | Negate 1 face-up monster; column S/T negate   | Opponent monster activates or dangerous boss  | Target already negated                      |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Harpie's Feather Duster            | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls| Opponent controls 1+ Spell/Trap cards         | Opponent controls 0 Spells/Traps            |
// | Terraforming                       | Spell Normal | No   | No    | None    | Add 1 Field Spell from Deck (Light Stage)     | Main Phase search Light Stage                 | Light Stage already in hand/field           |
// | Trickstar Light Stage              | Spell Field  | Yes  | Yes   | None    | Search Trickstar; lock 1 backrow; +200 burn   | Main Phase search starter (Candina/Lycoris)   | Already face-up on field                    |
// | Trickstar Light Arena              | Spell Field  | Yes  | Yes   | None    | Revive Trickstar on Link Summon               | Main Phase extension                          | No Link Trickstar on field                  |
// | Trickstar Magical Laurel           | Spell Equip  | Yes  | Yes   | Target  | Special Summon 1 Trickstar from GY            | Target Trickstar in GY; Link climb            | GY empty                                    |
// | Trickstar Reincarnation            | Trap Normal  | Yes  | No    | Target  | Banish opp entire hand, draw same number; GY rev| Opponent hand has 3+ cards; Lycoris on field | Opponent hand empty                         |
// | Trickstar Candina                  | Monster L4   | No   | No    | None    | On NS: Search ANY Trickstar card; 200 burn    | Normal Summon (primary starter)               | Already have all required pieces            |
// | Trickstar Lycoris                  | Monster L3   | No   | No    | Bounce  | Quick: Bounce Trickstar -> SS; 200 burn on add| In hand; dodge removal or swarm field         | No Trickstar to bounce                      |
// | Trickstar Corobane                 | Monster L5   | Yes  | Yes   | Discard | Hand Honest: Double ATK on damage calc; free SS| In hand, Trickstar battles; or free Link mat  | No battle                                   |
// | Trickstar Lilybell                 | Monster L2   | Yes  | Yes   | None    | SS if added to hand; direct attack; recycle GY| Added to hand; direct attack to recover piece  | GY empty                                    |
// | Trickstar Aqua Angel               | Monster L4   | Yes  | Yes   | None    | SS if control Trickstar; reveal opp hand      | Control Trickstar; need Link material         | No Trickstar on field                       |
// | Trickstar Hoody                    | Monster L2   | Yes  | Yes   | None    | SS if control Link Trickstar; search S/T      | Control Link Trickstar; search Reincarnation  | No Link Trickstar on field                  |
// | Trickstar Mandrake                 | Monster L2   | Yes  | Yes   | None    | SS from GY if sent from hand to GY            | Discarded / sent to GY                        | Already used this turn                      |
// | Trickstar Bella Madonna            | Link-4       | Yes  | Yes   | None    | 2800 ATK Tower; unaffected; burn 500 per GY mon| Link 4 boss; burn opponent for game           | Points to friendly monsters (loses immune)  |
// | Trickstar Noble Angel              | Link-2       | Yes  | Yes   | None    | Search Trickstar S/T on Link; protect Trickstar| Link 2 step stone into Bella Madonna          | Already used this turn                      |
// | Trickstar Holly Angel              | Link-2       | No   | No    | None    | 200 burn on summon; +ATK                      | Link 2 mid-game beatdown                      | No Trickstar summons                        |
// | Trickstar Crimson Heart            | Link-2       | Yes  | Yes   | Discard | Draw 2 cards if LP higher than opp            | Main Phase draw engine                        | Cannot discard / low LP                     |
// | Trickstar Colchica                 | Link-1       | Yes  | Yes   | Tribute | Redirect damage on Trickstar battle           | Trickstar destroyed in battle                 | No battle                                   |
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
    [Deck("Anime_BlueAngel", "Anime_BlueAngel")]
    public class Anime_BlueAngelExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int TrickstarCandina = 61283655;
            public const int TrickstarLycoris = 35199656;
            public const int TrickstarCorobane = 98169343;
            public const int TrickstarLilybell = 98700941;
            public const int TrickstarAquaAngel = 37405032;
            public const int TrickstarHoody = 1410324;
            public const int TrickstarMandrake = 22219822;
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;

            // Spells
            public const int TrickstarLightStage = 35371948;
            public const int TrickstarLightArena = 63492244;
            public const int TrickstarMagicalLaurel = 22159429;
            public const int Terraforming = 73628505;
            public const int HarpiesFeatherDuster = 18144506;
            public const int CalledByTheGrave = 24224830;

            // Traps
            public const int TrickstarReincarnation = 21076084;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int TrickstarBellaMadonna = 41302052;
            public const int TrickstarHollyAngel = 32448765;
            public const int TrickstarNobleAngel = 37683441;
            public const int TrickstarCrimsonHeart = 51011872;
            public const int TrickstarBandDrumatis = 64804137;
            public const int TrickstarColchica = 298846;
            public const int TrickstarDelfiendium = 3792766;
            public const int TrickstarFoxgloveWitch = 86750474;
            public const int TrickstarBloom = 77307161;
        }

        public Anime_BlueAngelExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Candina Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Trickstar-Candina-Starter",
                RequiredCards = new List<int> { CardId.TrickstarCandina },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TrickstarCandina, ActionType = ExecutorType.Summon, Description = "Normal Summon Trickstar Candina" },
                    new() { CardId = CardId.TrickstarCandina, ActionType = ExecutorType.Activate, Description = "Candina search Light Stage or Reincarnation" }
                },
                FallbackLineName = "Trickstar-LightStage-Starter"
            });

            // ── Line 2: Light Stage Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Trickstar-LightStage-Starter",
                RequiredCards = new List<int> { CardId.TrickstarLightStage },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TrickstarLightStage, ActionType = ExecutorType.Activate, Description = "Activate Light Stage -> Search Candina" },
                    new() { CardId = CardId.TrickstarCandina, ActionType = ExecutorType.Summon, Description = "Normal Summon Candina" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK TRAPS, HANDTRAPS & COMBAT TRICKS
            // ═══════════════════════════════════════════════════════════════

            // Trickstar Reincarnation — Banish opponent entire hand & force redraw (burn with Lycoris!)
            AddExecutor(ExecutorType.Activate, CardId.TrickstarReincarnation, TrickstarReincarnationActivate);

            // Trickstar Corobane — Hand Honest (+2000 ATK in damage calc)
            AddExecutor(ExecutorType.Activate, CardId.TrickstarCorobane, TrickstarCorobaneActivate);

            // Trickstar Lycoris — Quick bounce friendly Trickstar to dodge removal & summon itself
            AddExecutor(ExecutorType.Activate, CardId.TrickstarLycoris, TrickstarLycorisActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE SPELLS & SEARCHERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarLightStage, TrickstarLightStageActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarLightArena, TrickstarLightArenaActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarMagicalLaurel, TrickstarMagicalLaurelActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMON STARTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.TrickstarCandina, TrickstarCandinaSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarCandina, TrickstarCandinaEffect);

            AddExecutor(ExecutorType.Summon, CardId.TrickstarLilybell, TrickstarLilybellSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarLilybell, TrickstarLilybellEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMON EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarCorobane, TrickstarCorobaneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarAquaAngel, TrickstarAquaAngelEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarHoody, TrickstarHoodyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarMandrake, TrickstarMandrakeEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.TrickstarLycoris, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrickstarAquaAngel, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK LINK PROGRESSION & BOSS BURN
            // ═══════════════════════════════════════════════════════════════

            // Link 4 Trickstar Bella Madonna (Tower boss: unaffected + burn 500 per Trickstar in GY)
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarBellaMadonna, BellaMadonnaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarBellaMadonna, BellaMadonnaBurnEffect);

            // Link 2 Trickstar Noble Angel (Search S/T)
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarNobleAngel, NobleAngelSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarNobleAngel, NobleAngelEffect);

            // Link 2 Trickstar Holly Angel (Burn on summon)
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarHollyAngel, HollyAngelSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarHollyAngel, HollyAngelEffect);

            // Link 2 Trickstar Crimson Heart (Draw 2)
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarCrimsonHeart, CrimsonHeartSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrickstarCrimsonHeart, CrimsonHeartEffect);

            // Link 1 Trickstar Colchica & Bloom
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarColchica, ColchicaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TrickstarBloom, BloomSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.TrickstarReincarnation, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & TRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool TrickstarReincarnationActivate()
        {
            // Hand wipe: Banish opponent's entire hand and force redraw
            if (Card.Location == CardLocation.SpellZone)
            {
                // In opponent's Draw Phase / Standby Phase / Main Phase when they hold cards
                return Enemy.Hand.Count >= 2;
            }

            // GY effect: Banish to Special Summon 1 Trickstar from GY
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Bot.Graveyard
                    .Where(c => c.IsMonster() && c.HasSetcode(0xfb))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TrickstarCorobaneActivate()
        {
            // Damage calc Honest effect: Send from hand to double Trickstar ATK
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.Damage)
            {
                ClientCard battling = Bot.BattlingMonster;
                ClientCard enemyMon = Enemy.BattlingMonster;
                if (battling != null && enemyMon != null && battling.HasSetcode(0xfb))
                {
                    if (battling.Attack <= enemyMon.Attack || enemyMon.Attack >= 2000)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TrickstarLycorisActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Target 1 face-up Trickstar monster on field to bounce
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.TrickstarCandina)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0xfb) && m.Id != CardId.TrickstarLycoris);

                if (target != null)
                {
                    AI.SelectCard(target);
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

        private bool DrollAndLockBirdActivate()
        {
            // Triggers when opponent adds card to hand
            return Duel.Player == 1;
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

        // ═══════════════════════════════════════════════════════════════
        //  SPELLS & DRAW/SEARCH ENGINE
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool TerraformingActivate()
        {
            AI.SelectCard(CardId.TrickstarLightStage);
            return true;
        }

        private bool TrickstarLightStageActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.TrickstarLightStage);
            }

            // Lock opponent backrow
            ClientCard setCard = Enemy.GetSpells().FirstOrDefault(s => s.IsFacedown());
            if (setCard != null)
            {
                AI.SelectCard(setCard);
                return true;
            }
            return true;
        }

        private bool TrickstarLightArenaActivate()
        {
            return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.TrickstarLightArena);
        }

        private bool TrickstarMagicalLaurelActivate()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && c.HasSetcode(0xfb))
                .OrderByDescending(c => c.Attack)
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

        private bool TrickstarCandinaSummon()
        {
            return true;
        }

        private bool TrickstarCandinaEffect()
        {
            // Search Light Stage, Lycoris, or Reincarnation
            if (!Bot.HasInSpellZone(CardId.TrickstarLightStage) && !Bot.HasInHand(CardId.TrickstarLightStage))
            {
                AI.SelectCard(CardId.TrickstarLightStage);
            }
            else if (!Bot.HasInHand(CardId.TrickstarLycoris))
            {
                AI.SelectCard(CardId.TrickstarLycoris);
            }
            else
            {
                AI.SelectCard(CardId.TrickstarReincarnation);
            }
            return true;
        }

        private bool TrickstarLilybellSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool TrickstarLilybellEffect()
        {
            // Direct attack damage: retrieve Trickstar from GY
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && c.HasSetcode(0xfb) && c.Id != CardId.TrickstarLilybell)
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool TrickstarCorobaneSpSummon()
        {
            return Bot.GetMonsters().All(m => m.HasSetcode(0xfb));
        }

        private bool TrickstarAquaAngelEffect()
        {
            return true;
        }

        private bool TrickstarHoodyEffect()
        {
            // Search Reincarnation or Light Stage
            AI.SelectCard(CardId.TrickstarReincarnation, CardId.TrickstarLightStage);
            return true;
        }

        private bool TrickstarMandrakeEffect()
        {
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  LINK PROGRESSIONS & BOSS EFFECT
        // ═══════════════════════════════════════════════════════════════

        private bool BellaMadonnaSpSummon()
        {
            return true;
        }

        private bool BellaMadonnaBurnEffect()
        {
            // Burn 500 per Trickstar in GY
            return true;
        }

        private bool NobleAngelSpSummon()
        {
            return true;
        }

        private bool NobleAngelEffect()
        {
            AI.SelectCard(CardId.TrickstarReincarnation, CardId.TrickstarLightStage);
            return true;
        }

        private bool HollyAngelSpSummon()
        {
            return true;
        }

        private bool HollyAngelEffect()
        {
            return true;
        }

        private bool CrimsonHeartSpSummon()
        {
            return true;
        }

        private bool CrimsonHeartEffect()
        {
            return Bot.Hand.Count > 1;
        }

        private bool ColchicaSpSummon()
        {
            return Bot.GetMonsters().Count(m => m.HasSetcode(0xfb)) >= 2;
        }

        private bool BloomSpSummon()
        {
            return Bot.GetMonsterCount() >= 2;
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
                var preferred = cards.Where(c => c.Id == CardId.TrickstarCandina ||
                                                c.Id == CardId.TrickstarLightStage ||
                                                c.Id == CardId.TrickstarReincarnation ||
                                                c.Id == CardId.TrickstarLycoris).ToList();
                if (preferred.Count >= min)
                {
                    return preferred.Take(max).ToList();
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
