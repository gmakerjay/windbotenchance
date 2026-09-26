// ============================================================================
// MadolcheExecutor.cs — Madolche Non-Targeting Shuffle & Vernusylph Engine
// Archetype: Madolche Queen Tiarafraise / Tiaramisu / Glassouffle / Vernusylph
// Standard: Decoupled Domain Plugin Architecture (Layer 3 in SKILL.md)
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
    [Deck("Madolche")]
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

            // Handtraps & Defensive Fairies
            public const int EffectVeiler = 97268402;
            public const int GhostOgreSnowRabbit = 59438930;
            public const int HeraldOfOrangeLight = 17266660;
            public const int HeraldOfGreenLight = 21074344;
            public const int DominusPurge = 97045737;

            // Spells & Traps
            public const int SmallWorld = 89558743;
            public const int MadolcheChateau = 14001430;
            public const int MadolcheTicket = 60470713;
            public const int MadolchePromenade = 68159562;

            // Extra Deck
            public const int MadolcheQueenTiaramisu = 37164373;
            public const int MadolcheTeacherGlassouffle = 20343502;
            public const int MadolcheQueenTiarafraise = 49689480;
            public const int MadolchePuddingcessChocolatALaMode = 44311445;
            public const int GagagaCowboy = 12014404;
            public const int BarometTheSacredSheepShrub = 62967433;
            public const int TornadoDragon = 6983839;
            public const int CastelTheSkyblasterMusketeer = 82633039;
            public const int DiamondDireWolf = 95169481;
            public const int Number106GiantHand = 63746411;
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
                CardId.MadolchePuddingcessChocolatALaMode
            );

            BaitPlanner.RegisterComboStarters(
                CardId.MadolchePetingcessoeur,
                CardId.MadolcheMagileine,
                CardId.MadolcheAnjelly,
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
            AddExecutor(ExecutorType.Activate, CardId.MadolcheQueenTiarafraise, TiarafraiseQuickShuffle);
            AddExecutor(ExecutorType.Activate, CardId.MadolcheTeacherGlassouffle, GlassouffleQuickProtect);
            AddExecutor(ExecutorType.Activate, CardId.MadolchePromenade, PromenadeActivate);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfOrangeLight, OrangeLightActivate);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfGreenLight, GreenLightActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgreSnowRabbit, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeActivate);
            AddExecutor(ExecutorType.Activate, CardId.MudoraTheSwordOracle, MudoraActivate);

            // ── Tier 1: Board Clearing Non-Target Spin (Going 2nd Breakout) ──
            AddExecutor(ExecutorType.Activate, CardId.MadolcheQueenTiaramisu, TiaramisuBoardSpin);

            // ── Tier 2: Spells & Continuous Setup ──
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

            // ── Tier 5: Extra Deck Xyz Climbs ──
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheTeacherGlassouffle, GlassouffleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheQueenTiaramisu, TiaramisuSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolcheQueenTiarafraise, TiarafraiseSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MadolchePuddingcessChocolatALaMode, ChocolatSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadolchePuddingcessChocolatALaMode, ChocolatActivate);

            // ── Tier 6: Utility Rank 4s ──
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number106GiantHand, GiantHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number106GiantHand, GiantHandActivate);

            // ── Tier 7: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.MadolchePromenade, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusPurge, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet);

            // ── Tier 8: Desperation MonsterSet (Face-down Defense Only) ──
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

        private bool OrangeLightActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (last.IsDisabled()) return false;
            // Requires another Fairy in hand
            return Bot.Hand.Any(c => c != Card && c.HasRace(CardRace.Fairy));
        }

        private bool GreenLightActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsSpell()) return false;
            if (last.IsDisabled()) return false;
            return Bot.Hand.Any(c => c != Card && c.HasRace(CardRace.Fairy));
        }

        private bool EffectVeilerActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (last.IsDisabled()) return false;
            return true;
        }

        private bool GhostOgreActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            return true;
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

        private bool TornadoDragonSpSummon()
        {
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2 && Enemy.GetSpellCount() > 0;
        }

        private bool TornadoDragonActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool GiantHandSpSummon()
        {
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2 && Duel.Turn == 1;
        }

        private bool GiantHandActivate()
        {
            return true;
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

                // 1. Handtraps (0/1800, Veiler 0/0) or 0 ATK -> 100% Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Wall (DEF > ATK and ATK < 1800) -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800) -> Attack
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
            if (hint == 506)
            {
                // Petingcessoeur if GY is clean
                if (Bot.Graveyard.Count(c => c.IsMonster()) == 0 && !Bot.HasInHand(CardId.MadolchePetingcessoeur))
                {
                    var peting = cards.FirstOrDefault(c => c.Id == CardId.MadolchePetingcessoeur);
                    if (peting != null) return new List<ClientCard> { peting };
                }

                // Anjelly > Magileine > Chateau > Promenade
                var anjelly = cards.FirstOrDefault(c => c.Id == CardId.MadolcheAnjelly);
                if (anjelly != null) return new List<ClientCard> { anjelly };

                var promenade = cards.FirstOrDefault(c => c.Id == CardId.MadolchePromenade);
                if (promenade != null && !Bot.HasInSpellZone(CardId.MadolchePromenade)) return new List<ClientCard> { promenade };

                var chateau = cards.FirstOrDefault(c => c.Id == CardId.MadolcheChateau);
                if (chateau != null && !Bot.HasInSpellZone(CardId.MadolcheChateau)) return new List<ClientCard> { chateau };
            }

            // Tiaramisu / Tiarafraise opponent card shuffle (hint 507 = HINTMSG_TODECK)
            if (hint == 507)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                if (enemyCards.Count >= min) return enemyCards.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: MadolchePlugin
    // ═══════════════════════════════════════════════════════════════
    internal class MadolchePlugin
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheStrategy Strategy { get; }
        public MadolcheGraveyardManager GraveyardManager { get; }
        public MadolcheMaterialScorer MaterialScorer { get; }
        public MadolcheBoardAssessor BoardAssessor { get; }

        public MadolchePlugin(MadolcheExecutor exec)
        {
            _exec = exec;
            Strategy = new MadolcheStrategy(exec);
            GraveyardManager = new MadolcheGraveyardManager(exec);
            MaterialScorer = new MadolcheMaterialScorer(exec);
            BoardAssessor = new MadolcheBoardAssessor(exec);
        }

        public void ResetTurnState() { }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MadolcheStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class MadolcheStrategy
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheStrategy(MadolcheExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MadolcheGraveyardManager
    // ═══════════════════════════════════════════════════════════════
    internal class MadolcheGraveyardManager
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheGraveyardManager(MadolcheExecutor exec)
        {
            _exec = exec;
        }

        public bool IsGyCleanForPetingcessoeur()
        {
            return _exec.Bot.Graveyard.Count(c => c.IsMonster()) == 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MadolcheMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class MadolcheMaterialScorer
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheMaterialScorer(MadolcheExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MadolcheBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class MadolcheBoardAssessor
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheBoardAssessor(MadolcheExecutor exec)
        {
            _exec = exec;
        }
    }
}
