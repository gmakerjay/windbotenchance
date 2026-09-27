// ============================================================================
// MadolcheExecutor.cs — Madolche Non-Targeting Shuffle & Vernusylph Engine
// Archetype: Madolche Queen Tiarafraise / Tiaramisu / Glassouffle / Sistart / Salon
// Standard: Decoupled Domain Plugin Architecture (Layer 3 in SKILL.md)
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
    [Deck("Madolche", "Madolche")]
    public class MadolcheExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Madolche Main Monsters
            public const int MadolcheMagileine = 11868731;
            public const int MadolchePetingcessoeur = 77848740;
            public const int MadolcheAnjelly = 34680482;
            public const int MadolcheHootcake = 91350799;
            public const int MadolcheMessengelato = 52404456;
            public const int MadolchePuddingcess = 74641045;

            // Vernusylph & Earth Monsters
            public const int VernusylphFlourishingHills = 9350312;
            public const int VernusylphMistingSeedlings = 81519836;
            public const int VernusylphAwakeningForests = 36745317;
            public const int MudoraTheSwordOracle = 99937011;

            // Handtraps & Defensive Staples
            public const int AshBlossom = 14558128;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int DominusPurge = 97045737;

            // Spells & Traps
            public const int MadolcheSalon = 71348837;
            public const int MadolcheChateau = 14001430;
            public const int MadolcheTicket = 60470713;
            public const int SmallWorld = 89558743;
            public const int MadolchePromenade = 68159562;

            // Extra Deck
            public const int MadolcheQueenTiarafraise = 49689480;
            public const int MadolcheQueenTiaramisu = 37164373;
            public const int MadolcheTeacherGlassouffle = 20343502;
            public const int MadolchePuddingcessChocolatALaMode = 44311445;
            public const int MadolcheFreshSistart = 96150936;
            public const int MadolcheMiniMeowcaroons = 38745241;
            public const int AbyssDweller = 21044178;
            public const int TornadoDragon = 6983839;
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
            public const int DivineArsenalAAZEUS = 90448279;
        }

        // Domain Plugin Coordinator (Layer 3)
        internal MadolchePlugin Plugin { get; private set; }

        public ClientCard CurrentLastChainCard => LastChainCard;

        public MadolcheExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new MadolchePlugin(this);

            // Register Strategic Assets
            ResourcePlan.RegisterAceCards(
                CardId.MadolcheQueenTiarafraise,
                CardId.MadolcheQueenTiaramisu,
                CardId.MadolcheTeacherGlassouffle,
                CardId.MadolcheFreshSistart,
                CardId.MadolchePuddingcessChocolatALaMode
            );

            BaitPlanner.RegisterComboStarters(
                CardId.MadolchePetingcessoeur,
                CardId.MadolcheMagileine,
                CardId.MadolcheAnjelly,
                CardId.SmallWorld,
                CardId.VernusylphFlourishingHills,
                CardId.VernusylphMistingSeedlings
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.MadolcheQueenTiarafraise,
                CardId.MadolcheQueenTiaramisu,
                CardId.MadolcheTeacherGlassouffle,
                CardId.MadolchePromenade
            );

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negations, Interventions & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheQueenTiarafraise, TiarafraiseQuickShuffle);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheTeacherGlassouffle, GlassouffleQuickProtect);
            AddExecutor(ExecutorType.Activate, CardId.MadolchePromenade, PromenadeActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeActivate);
            AddExecutor(ExecutorType.Activate, CardId.MudoraTheSwordOracle, MudoraActivate);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);

            // ── Tier 1: Board Clearing Non-Target Spin (Going 2nd Breakout) ──
            AddExecutor(ExecutorType.Activate, CardId.MadolcheQueenTiaramisu, TiaramisuBoardSpin);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUS, ZeusActivate);

            // ── Tier 2: Spells & Continuous Setup ──
            AddExecutor(ExecutorType.Activate, CardId.MadolcheSalon, SalonActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheChateau, ChateauActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheTicket, TicketActivate);
            AddExecutor(ExecutorType.Activate, CardId.SmallWorld, SmallWorldActivate);

            // ── Tier 3: Vernusylph Discard Extenders ──
            AddExecutor(ExecutorType.Activate, CardId.VernusylphFlourishingHills, VernusylphHillsActivate);
            AddExecutor(ExecutorType.Activate, CardId.VernusylphMistingSeedlings, VernusylphSeedlingsActivate);
            AddExecutor(ExecutorType.Activate, CardId.VernusylphAwakeningForests, VernusylphForestsActivate);

            // ── Tier 4: Madolche Core Engine (Special Summons & Searches) ──
            AddExecutor(ExecutorType.SpSummon, CardId.MadolchePetingcessoeur, PetingcessoeurSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolchePetingcessoeur, PetingcessoeurActivate);
            AddExecutor(ExecutorType.Summon, CardId.MadolcheMagileine, MagileineSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheMagileine, MagileineActivate);
            AddExecutor(ExecutorType.Summon, CardId.MadolcheAnjelly, AnjellySummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheAnjelly, AnjellyActivate);
            AddExecutor(ExecutorType.Summon, CardId.MadolcheHootcake, HootcakeSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheHootcake, HootcakeActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheMessengelato, MessengelatoActivate);

            // ── Tier 5: Extra Deck Links & Xyz Climbs ──
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheTeacherGlassouffle, GlassouffleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheQueenTiaramisu, TiaramisuSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheQueenTiarafraise, TiarafraiseSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolchePuddingcessChocolatALaMode, ChocolatSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolchePuddingcessChocolatALaMode, ChocolatActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheFreshSistart, SistartSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheMiniMeowcaroons, MeowcaroonsSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheMiniMeowcaroons, MeowcaroonsActivate);

            // ── Tier 6: Utility Extra Deck Monsters ──
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUS, ZeusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonSpSummon);

            // ── Tier 7: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.MadolchePromenade, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusPurge, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet);

            // ── Tier 8: Desperation MonsterSet ──
            AddExecutor(ExecutorType.MonsterSet, DesperationMonsterSet);

            // ── Tier 9: Smart Monster Repositioning ──
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION & COMBOS
        // ═══════════════════════════════════════════════════════════════

        private bool MaxxCActivate()
        {
            return Duel.Player == 1;
        }

        private bool AshBlossomActivate()
        {
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveActivate()
        {
            return DefaultCalledByTheGrave();
        }

        private bool CrossoutDesignatorActivate()
        {
            return DefaultCrossoutDesignator();
        }

        private bool TiarafraiseQuickShuffle()
        {
            // On opponent's turn, detach 1, shuffle up to 2 Madolche in GY to shuffle 2 opponent cards into deck!
            if (Card.Overlays.Count == 0) return false;
            bool hasGyMadolche = Bot.Graveyard.Any(c => c.HasSetcode(0x71));
            return hasGyMadolche && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool GlassouffleQuickProtect()
        {
            if (Card.Overlays.Count == 0) return false;
            // Protect Madolche from monster effects, or purge GY for Petingcessoeur
            return true;
        }

        private bool PromenadeActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: attach Madolche from Deck to Madolche Xyz
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x71) && m.HasType(CardType.Xyz));
            }
            // Field effect: Target 1 face-up card opponent controls, negate and bounce our Madolche
            ClientCard last = LastChainCard;
            if (last != null && last.Controller == 1 && !last.IsDisabled()) return true;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled()) || Enemy.GetSpells().Any(s => s.IsFaceup());
        }

        private bool DominusPurgeActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            if (last.IsDisabled()) return false;
            return true;
        }

        private bool MudoraActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Shuffle up to 3 cards from GY into deck
                return Enemy.Graveyard.Count >= 2 || (Bot.Graveyard.Count(c => c.IsMonster()) > 0 && Bot.HasInHand(CardId.MadolchePetingcessoeur));
            }
            return Bot.Hand.Any(c => c != Card && c.HasRace(CardRace.Fairy));
        }

        private bool TiaramisuBoardSpin()
        {
            if (Card.Overlays.Count == 0) return false;
            bool hasGyMadolche = Bot.Graveyard.Any(c => c.HasSetcode(0x71));
            return hasGyMadolche && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool SalonActivate()
        {
            return !Bot.GetSpells().Any(s => s.IsFaceup() && s.Id == CardId.MadolcheSalon);
        }

        private bool ChateauActivate()
        {
            return !Bot.GetSpells().Any(s => s.IsFaceup() && s.Id == CardId.MadolcheChateau);
        }

        private bool TicketActivate()
        {
            return !Bot.GetSpells().Any(s => s.IsFaceup() && s.Id == CardId.MadolcheTicket);
        }

        private bool SmallWorldActivate()
        {
            return true;
        }

        private bool VernusylphHillsActivate()
        {
            return Bot.Hand.Any(c => c != Card && c.IsMonster());
        }

        private bool VernusylphSeedlingsActivate()
        {
            return Bot.Hand.Any(c => c != Card && c.IsMonster());
        }

        private bool VernusylphForestsActivate()
        {
            return Bot.Hand.Any(c => c != Card && c.IsMonster());
        }

        private bool PetingcessoeurSpSummon()
        {
            // Requires NO MONSTERS in GY
            return Bot.Graveyard.Count(c => c.IsMonster()) == 0;
        }

        private bool PetingcessoeurActivate()
        {
            return true;
        }

        private bool MagileineSummon()
        {
            return true;
        }

        private bool MagileineActivate()
        {
            return true;
        }

        private bool AnjellySummon()
        {
            return true;
        }

        private bool AnjellyActivate()
        {
            return true;
        }

        private bool HootcakeSummon()
        {
            return true;
        }

        private bool HootcakeActivate()
        {
            return Bot.Graveyard.Any(c => c.IsMonster());
        }

        private bool MessengelatoActivate()
        {
            return true;
        }

        private bool GlassouffleSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.MadolcheTeacherGlassouffle)) return false;
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2;
        }

        private bool TiaramisuSpSummon()
        {
            var madolcheLv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && m.HasSetcode(0x71) && !IsAceCard(m)).ToList();
            return madolcheLv4s.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Duel.Turn > 1 || Bot.GetMonsters().Any(m => m.Id == CardId.MadolcheTeacherGlassouffle));
        }

        private bool TiarafraiseSpSummon()
        {
            // Overlay directly on top of Tiaramisu
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.MadolcheQueenTiaramisu);
        }

        private bool ChocolatSpSummon()
        {
            // Overlay on top of Rank 4 Madolche
            var target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Id == CardId.MadolcheTeacherGlassouffle) && m.Overlays.Count == 0);
            return target != null;
        }

        private bool ChocolatActivate()
        {
            return true;
        }

        private bool SistartSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.MadolcheFreshSistart)) return false;
            var madolcheMonsters = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasSetcode(0x71) && !IsAceCard(m)).ToList();
            return madolcheMonsters.Count >= 2;
        }

        private bool MeowcaroonsSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.MadolcheMiniMeowcaroons)) return false;
            var monsters = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            bool hasMadolche = monsters.Any(m => m.HasSetcode(0x71));
            return monsters.Count >= 2 && hasMadolche && Duel.Turn > 1;
        }

        private bool MeowcaroonsActivate()
        {
            return true;
        }

        private bool AbyssDwellerSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.AbyssDweller)) return false;
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2 && (Duel.Turn == 1 || Enemy.Graveyard.Count >= 2);
        }

        private bool AbyssDwellerActivate()
        {
            return Duel.Player == 1;
        }

        private bool ZeusSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Xyz) && m.Attacked);
        }

        private bool ZeusActivate()
        {
            if (Card.Overlays.Count < 2) return false;
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2 ||
                   (Enemy.GetMonsterCount() > 0 && Duel.Player == 1);
        }

        private bool TyphonSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 &&
                   Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500);
        }

        private bool TornadoDragonSpSummon()
        {
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2 && Enemy.GetSpellCount() > 0;
        }

        private bool TornadoDragonActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool SetTrapCondition()
        {
            return Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1;
        }

        private bool DesperationMonsterSet()
        {
            if (Bot.GetMonsterCount() > 0) return false;
            if (Duel.Turn <= 1) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsAttack() && m.Attack >= 1500);
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;
            return DefaultMonsterRepos();
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

                // Handtraps (0/1800) or 0 ATK -> 100% Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // High DEF / Wall -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // Boss / High ATK -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Search priority (hint 506 = HINTMSG_ATOHAND)
            if (hint == 506 && min <= 1 && 1 <= max)
            {
                var target = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (target != null) return new List<ClientCard> { target };
            }

            // Special summon priority (hint 509 = HINTMSG_SPSUMMON)
            if (hint == 509 && min <= 1 && 1 <= max)
            {
                var target = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null) return new List<ClientCard> { target };
            }

            // Tiaramisu / Tiarafraise opponent card shuffle (hint 507 = HINTMSG_TODECK)
            if (hint == 507)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                if (enemyCards.Count >= min) return enemyCards.Take(max).ToList();

                // If selecting our own GY cards to recycle back to deck:
                var myGyMadolche = cards.Where(c => c.Controller == 0 && c.HasSetcode(0x71)).ToList();
                if (myGyMadolche.Count >= min) return myGyMadolche.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
