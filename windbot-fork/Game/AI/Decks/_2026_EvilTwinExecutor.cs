// ============================================================
// CARD AUDIT — 2026_EvilTwin
// ============================================================
// | Card Name                | Type       | OPT? | HOPT? | Cost | Effect Summary           | Activate When                        | NEVER Activate When                          | Can Be Negated By |
// |--------------------------|------------|------|-------|------|--------------------------|--------------------------------------|----------------------------------------------|-------------------|
// | Live☆Twin Ki-sikil       | Monster    | Yes  | Yes   | None | SS Lil-la from Deck/Hand | Control no other monsters            | Already control another monster              | Ash, Veiler, Imperm|
// | Live☆Twin Lil-la         | Monster    | Yes  | Yes   | None | SS Ki-sikil from Deck/Hand| Control no other monsters            | Already control another monster              | Ash, Veiler, Imperm|
// | Evil★Twin Ki-sikil       | Link 2     | Yes  | Yes   | None | Draw 1 / Revive Lil-la   | Need to draw / no Lil-la on field    | Self-negating or no revivable target in GY   | Ash, Veiler, Imperm|
// | Evil★Twin Lil-la         | Link 2     | Yes  | Yes   | None | Destroy 1 / Revive Ki-s  | Control Ki-s / no Ki-s on field      | Self-negating or no revivable target in GY   | Ash, Veiler, Imperm|
// | Trouble Sunny            | Link 4     | Yes  | Yes   | None | GY Send / Tribute SS 2   | Opponent turn disruption / extend    | No revivable Ki-sikil + Lil-la in GY         | Ash, Veiler, Imperm|
// | Sunny's Snitch           | Spell      | Yes  | Yes   | None | Search Live☆Twin         | Main Phase search target exists      | Snitch already used / no targets in deck     | Ash Blossom       |
// | Fiendsmith Engraver      | Monster    | Yes  | Yes   | None | Search Tract / SS / Send | Need search / Fiendsmith setup       | No LIGHT Fiend in GY to shuffle              | Ash, Veiler, Imperm|
// | Fiendsmith's Tract       | Spell      | Yes  | Yes   | None | Search Engraver / Fusion | Need Engraver / Fusion summon setup  | No targets in deck / hand empty              | Ash Blossom       |
// | Fiendsmith's Requiem     | Link 1     | Yes  | Yes   | None | Search Engraver          | Have LIGHT Fiend on field            | No Engraver in deck                          | Ash Blossom       |
// | Fiendsmith's Sequence    | Link 2     | Yes  | Yes   | None | Fusion summon            | Have materials on field/GY           | No targets to summon                         | Veiler, Imperm    |
// | Fiendsmith's Lacrima     | Fusion     | Yes  | Yes   | None | Send Fiendsmith to GY    | Special Summoned                     | No Fiendsmith cards left in deck/extra       | Ash Blossom       |
// | Fiendsmith's Desirae     | Fusion     | Yes  | Yes   | None | Negate monster effect    | Opponent turn disruption             | No monsters on opponent field                | Veiler, Imperm    |
// | D/D/D Wave High King     | Xyz R6     | Yes  | Yes   | Det  | Negate Special Summon    | Opponent Special Summon / Effect     | Out of Xyz materials                         | Veiler, Imperm    |
// | Chaos Angel              | Synchro 10 | Yes  | Yes   | None | Banish 1 card            | Special Summoned                     | No targets on field                          | Veiler, Imperm    |
// | A Bao A Qu               | Link 4     | Yes  | Yes   | Disc | Quick destroy / SS GY    | Main Phase disruption / SS standby   | Hand empty / no targets                      | Veiler, Imperm    |
// | Number 65: Djinn Buster  | Xyz R2     | Yes  | Yes   | Det  | Negate monster effect    | Opponent monster effect activates    | Out of Xyz materials                         | Veiler, Imperm    |
// ============================================================
// ACE CARDS:
//   Primary  : Evil★Twin's Trouble Sunny — Quick Effect board split and high ATK (3300)
//   Secondary: Fiendsmith's Desirae — Boss monster negation
//   Tertiary : D/D/D Wave High King Caesar — Double Special Summon negation
//   Quaternary: Chaos Angel & A Bao A Qu — Board breaking and spot removal
// Priority: Win > Negate/Disrupt > Combo > Setup
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_EvilTwin", "2026_EvilTwin")]
    public class _2026_EvilTwinExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int LiveTwinKiSikil = 36326160;
            public const int LiveTwinKiSikilFrost = 54257392;
            public const int LiveTwinLilla = 73810864;
            public const int LiveTwinLillaSweet = 82699999;
            public const int FiendsmithEngraver = 60764609;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;
            
            // Hand Traps & Techs
            public const int MaxxC = 23434538;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int Nibiru = 27204311;
            public const int EffectVeiler = 97268402;
            public const int GhostMourner = 52038441;
            public const int GhostBelle = 73642296;

            // Spells & Traps
            public const int LiveTwinSunnysSnitch = 37582948;
            public const int FiendsmithsTract = 98567237;
            public const int FiendsmithInParadise = 99989863;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int EvilTwinKiSikilDeal = 6636319;
            public const int EvilTwinKiSikil = 9205573;
            public const int EvilTwinLilla = 36609518;
            public const int EvilTwinsTroubleSunny = 93672138;
            public const int FiendsmithsRequiem = 2463794;
            public const int FiendsmithsSequence = 49867899;
            public const int FiendsmithsAgnumday = 32991300;
            public const int FiendsmithsDesirae = 82135803;
            public const int FiendsmithsLacrima = 46640168;
            public const int NecroquipPrincess = 93860227;
            public const int DDDWaveHighKingCaesar = 79559912;
            public const int Number65DjinnBuster = 3790062;
            public const int ChaosAngel = 22850702;
            public const int ABaoAQu = 4731783;
            public const int MoonOfTheClosedHeaven = 71818935;
        }

        private bool _snitchUsed = false;
        private bool _tractUsed = false;
        private bool _engraverUsed = false;
        private bool _troubleSunnyUsed = false;
        private bool _kisikilSummonEffectUsed = false;
        private bool _lillaSummonEffectUsed = false;
        private bool _kisikilLinkEffectUsed = false;
        private bool _lillaLinkEffectUsed = false;
        private bool _frostUsed = false;
        private bool _requiemUsed = false;
        private bool _sequenceUsed = false;
        private bool _lacrimaSendUsed = false;
        private bool _desiraeUsed = false;
        private bool _necroquipUsed = false;
        private bool _caesarsUsed = false;
        private bool _abaoaquUsed = false;
        private bool _djinnBusterUsed = false;
        private bool _engraverSpSummonUsed = false;

        private static readonly int[] AceCardIds = {
            CardId.EvilTwinsTroubleSunny,
            CardId.FiendsmithsDesirae,
            CardId.DDDWaveHighKingCaesar,
            CardId.ChaosAngel,
            CardId.ABaoAQu,
            CardId.Number65DjinnBuster
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Any(id => card.IsCode(id));
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.FabledLurrie)) return 50;
            if (c.IsCode(CardId.FiendsmithsRequiem) || c.IsCode(CardId.MoonOfTheClosedHeaven)) return 80;
            if (c.IsCode(CardId.LacrimaTheCrimsonTears)) return 100;
            if (c.IsCode(CardId.LiveTwinKiSikilFrost) || c.IsCode(CardId.LiveTwinLillaSweet)) return 250;
            if (c.IsCode(CardId.LiveTwinKiSikil) || c.IsCode(CardId.LiveTwinLilla)) return 300;
            return base.GetMaterialPriority(c);
        }

        private bool IsFiendsmithCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.FiendsmithEngraver ||
                   card.Id == CardId.LacrimaTheCrimsonTears ||
                   card.Id == CardId.FiendsmithsTract ||
                   card.Id == CardId.FiendsmithInParadise ||
                   card.Id == CardId.FiendsmithsRequiem ||
                   card.Id == CardId.FiendsmithsSequence ||
                   card.Id == CardId.FiendsmithsAgnumday ||
                   card.Id == CardId.FiendsmithsDesirae ||
                   card.Id == CardId.FiendsmithsLacrima;
        }

        public _2026_EvilTwinExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.LiveTwinKiSikil, CardId.LiveTwinLilla },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.LiveTwinKiSikil, ActionType = ExecutorType.Activate, Description = "Play CardId.LiveTwinKiSikil" },
                    new() { CardId = CardId.LiveTwinLilla, ActionType = ExecutorType.Activate, Description = "Extend with CardId.LiveTwinLilla" }
                },
                EndBoardScore = 80
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.LiveTwinKiSikil, CardId.LiveTwinKiSikilFrost, CardId.FiendsmithsTract, CardId.LiveTwinSunnysSnitch);
            BaitPlanner.RegisterBaitCards(CardId.LiveTwinKiSikilFrost, CardId.FiendsmithsTract, CardId.LiveTwinSunnysSnitch);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.LiveTwinKiSikil, CardId.LiveTwinKiSikilFrost, CardId.InfiniteImpermanence, CardId.EvilTwinsTroubleSunny, CardId.FiendsmithEngraver);

            // 1. Hand Traps & Negation (opponent-reactive, highest priority)
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, () => SmartHandTrapChain() && DefaultGhostBelleAndHauntedMansion());
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, () => SmartHandTrapChain() && DefaultEffectVeiler());
            AddExecutor(ExecutorType.Activate, CardId.GhostMourner, GhostMournerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);

            // 2. Main Phase Searches & Setup
            AddExecutor(ExecutorType.Activate, CardId.LiveTwinSunnysSnitch, SunnysSnitchEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverActivate);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaTheCrimsonTearsEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie);

            // 3. Normal / Special Summoning (Live☆Twin / Fiendsmith)
            AddExecutor(ExecutorType.Summon, CardId.LiveTwinKiSikil, LiveTwinSummon);
            AddExecutor(ExecutorType.Summon, CardId.LiveTwinLilla, LiveTwinSummon);
            AddExecutor(ExecutorType.Summon, CardId.LiveTwinKiSikilFrost, LiveTwinSummon);
            AddExecutor(ExecutorType.Summon, CardId.LiveTwinLillaSweet, LiveTwinSummon);
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, FiendsmithSummon);
            AddExecutor(ExecutorType.Summon, CardId.FabledLurrie, FiendsmithSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LiveTwinKiSikilFrost, LiveTwinFrostSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LiveTwinLillaSweet, LiveTwinSweetSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithEngraver, EngraverSpSummon);

            // 4. Live☆Twin / Evil★Twin Core Effect Activations
            AddExecutor(ExecutorType.Activate, CardId.LiveTwinKiSikil, LiveTwinKiSikilEffect);
            AddExecutor(ExecutorType.Activate, CardId.LiveTwinLilla, LiveTwinLillaEffect);
            AddExecutor(ExecutorType.Activate, CardId.LiveTwinKiSikilFrost, LiveTwinFrostEffect);
            AddExecutor(ExecutorType.Activate, CardId.LiveTwinLillaSweet, LiveTwinSweetEffect);
            
            // 5. Link/Xyz/Synchro Summons (Extra Deck)
            AddExecutor(ExecutorType.SpSummon, CardId.EvilTwinsTroubleSunny, TroubleSunnySummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilTwinKiSikil, EvilTwinKiSikilSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilTwinKiSikilDeal, EvilTwinKiSikilSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilTwinLilla, EvilTwinLillaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, DDDWaveHighKingCaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ABaoAQu, ABaoAQuSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number65DjinnBuster, DjinnBusterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, FiendsmithSequenceSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsAgnumday, FiendsmithsAgnumdaySummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonOfTheClosedHeavenSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, FiendsmithRequiemSummon);

            // 6. Evil★Twin Link Monster GY Revivals & Trouble Sunny Play
            AddExecutor(ExecutorType.Activate, CardId.EvilTwinKiSikil, EvilTwinKiSikilEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilTwinKiSikilDeal, EvilTwinKiSikilEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilTwinLilla, EvilTwinLillaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilTwinsTroubleSunny, TroubleSunnyEffect);

            // 7. Fiendsmith Extra Deck plays (Requiem, Sequence, Fusion/XYZ)
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsLacrima, FiendsmithsLacrimaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithsLacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsDesirae, FiendsmithsDesiraeSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsDesirae, DesiraeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DDDWaveHighKingCaesar, CaesarEffect);
            AddExecutor(ExecutorType.Activate, CardId.Number65DjinnBuster, DjinnBusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChaosAngel, ChaosAngelEffect);
            AddExecutor(ExecutorType.Activate, CardId.ABaoAQu, ABaoAQuEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.NecroquipPrincess, NecroquipPrincessSummon);
            AddExecutor(ExecutorType.Activate, CardId.NecroquipPrincess, NecroquipPrincessEffect);

            // 8. Spell/Trap Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.FiendsmithInParadise, SetTrapCondition);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithInParadise, FiendsmithInParadiseEffect);
            
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Evil Twin / Fiendsmith control — prefer going first to set up Trouble Sunny & High King Caesar
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _snitchUsed = false;
            _tractUsed = false;
            _engraverUsed = false;
            _troubleSunnyUsed = false;
            _kisikilSummonEffectUsed = false;
            _lillaSummonEffectUsed = false;
            _kisikilLinkEffectUsed = false;
            _lillaLinkEffectUsed = false;
            _frostUsed = false;
            _requiemUsed = false;
            _sequenceUsed = false;
            _lacrimaSendUsed = false;
            _desiraeUsed = false;
            _necroquipUsed = false;
            _caesarsUsed = false;
            _abaoaquUsed = false;
            _djinnBusterUsed = false;
            _engraverSpSummonUsed = false;

            if (ShouldGoBreakBoard)
            {
                _troubleSunnyUsed = false;
                _desiraeUsed = false;
            }
        }

        private bool DrollAndLockBirdEffect()
        {
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private IList<ClientCard> GetBestSequenceFusionMaterials(IList<ClientCard> cards, int min, int max)
        {
            int kisikilInGrave = Bot.Graveyard.Count(c => c != null && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
            int lillaInGrave = Bot.Graveyard.Count(c => c != null && c.Id == CardId.EvilTwinLilla);

            var prioritized = cards.OrderBy(c => {
                if (c == null) return 100;
                if (c.Id == CardId.FabledLurrie) return 1;
                if (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.MoonOfTheClosedHeaven) return 2;
                if (c.Id == CardId.LacrimaTheCrimsonTears) return 3;
                if (c.Id == CardId.FiendsmithEngraver) return 4;
                
                bool isKiSikil = c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal;
                bool isLilla = c.Id == CardId.EvilTwinLilla;
                bool isTroubleSunny = c.Id == CardId.EvilTwinsTroubleSunny;

                if (isTroubleSunny) return 50; // Protect Trouble Sunny in GY!
                if (isKiSikil) return kisikilInGrave > 1 ? 5 : 40;
                if (isLilla) return lillaInGrave > 1 ? 5 : 40;

                return 6;
            }).ToList();

            return prioritized.Take(max).ToList();
        }

        private IList<ClientCard> GetBestDiscardTargets(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();
            
            // 1. Fabled Lurrie (Summons itself if discarded)
            var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
            if (lurrie != null) result.Add(lurrie);
            
            // 2. Fiendsmith Engraver (Can revive from GY)
            var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver);
            if (engraver != null) result.Add(engraver);
            
            // 3. Lacrima the Crimson Tears
            var lacrima = cards.FirstOrDefault(c => c != null && c.Id == CardId.LacrimaTheCrimsonTears);
            if (lacrima != null) result.Add(lacrima);

            // 4. LiveTwin Frost / Sweet (GY trigger / SS)
            var frost = cards.FirstOrDefault(c => c != null && c.Id == CardId.LiveTwinKiSikilFrost && !result.Contains(c));
            if (frost != null) result.Add(frost);
            var sweet = cards.FirstOrDefault(c => c != null && c.Id == CardId.LiveTwinLillaSweet && !result.Contains(c));
            if (sweet != null) result.Add(sweet);

            // 5. Our turn: hand traps are less useful — discard them
            if (Duel.Player == 0)
            {
                var handTraps = cards.Where(c => c != null && !result.Contains(c) &&
                    (c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia ||
                     c.Id == CardId.DrollAndLockBird || c.Id == CardId.EffectVeiler ||
                     c.Id == CardId.GhostMourner || c.Id == CardId.GhostBelle ||
                     c.Id == CardId.Nibiru)).ToList();
                foreach (var ht in handTraps)
                    if (!result.Contains(ht)) result.Add(ht);
            }

            // 6. Duplicate hand traps / cards
            var duplicates = cards.Where(c => c != null && cards.Count(x => x != null && x.Id == c.Id) > 1 
                                         && c.Id != CardId.LiveTwinKiSikil && c.Id != CardId.LiveTwinLilla).ToList();
            foreach (var dup in duplicates)
            {
                if (!result.Contains(dup)) result.Add(dup);
            }

            // 7. General low priority cards
            var lowPriority = cards.Where(c => c != null && 
                c.Id != CardId.LiveTwinKiSikil && 
                c.Id != CardId.LiveTwinLilla && 
                c.Id != CardId.LiveTwinSunnysSnitch && 
                c.Id != CardId.FiendsmithsTract && 
                c.Id != CardId.CalledByTheGrave && 
                c.Id != CardId.CrossoutDesignator).ToList();
            foreach (var lp in lowPriority)
            {
                if (!result.Contains(lp)) result.Add(lp);
            }
            
            // Fallback
            foreach (var c in cards)
            {
                if (c != null && !result.Contains(c)) result.Add(c);
            }
            
            return result.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            var activeCard = Card ?? LastChainCard;
            if (activeCard != null)
            {
                if (activeCard.Id == CardId.FiendsmithsSequence)
                {
                    return GetBestSequenceFusionMaterials(cards, min, max);
                }

                // Link-2 Ki-sikil GY Revive of Lil-la
                if ((activeCard.Id == CardId.EvilTwinKiSikil || activeCard.Id == CardId.EvilTwinKiSikilDeal) && activeCard.Location == CardLocation.MonsterZone)
                {
                    var revivable = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && 
                        (c.Id == CardId.EvilTwinLilla || c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet));
                    if (revivable != null) return new List<ClientCard> { revivable };
                }

                // Link-2 Lil-la GY Revive of Ki-sikil
                if (activeCard.Id == CardId.EvilTwinLilla && activeCard.Location == CardLocation.MonsterZone)
                {
                    var revivable = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && 
                        (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal || c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost));
                    if (revivable != null) return new List<ClientCard> { revivable };
                }

                // Trouble Sunny: On field Tribute -> Revive 1 Ki-sikil AND 1 Lil-la
                if (activeCard.Id == CardId.EvilTwinsTroubleSunny)
                {
                    var kisikil = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal))
                               ?? cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost));

                    var lilla = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && c.Id == CardId.EvilTwinLilla)
                             ?? cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.IsCanRevive() && (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet));

                    if (max >= 2 && kisikil != null && lilla != null)
                    {
                        return new List<ClientCard> { kisikil, lilla };
                    }
                    if (kisikil != null && cards.Contains(kisikil))
                    {
                        return new List<ClientCard> { kisikil };
                    }
                    if (lilla != null && cards.Contains(lilla))
                    {
                        return new List<ClientCard> { lilla };
                    }

                    // Trouble Sunny GY effect: Send 1 Evil Twin monster (from Deck/Hand/Field) to GY
                    var deckTwin = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Deck && 
                        (c.Id == CardId.EvilTwinKiSikilDeal || c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla));
                    if (deckTwin != null) return new List<ClientCard> { deckTwin };

                    var handTwin = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Hand && 
                        (c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinLillaSweet || c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla));
                    if (handTwin != null) return new List<ClientCard> { handTwin };
                }

                if (activeCard.Id == CardId.FiendsmithsTract)
                {
                    var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver);
                    if (engraver != null) return new List<ClientCard> { engraver };
                    var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                    if (lurrie != null) return new List<ClientCard> { lurrie };
                }

                // Fiendsmith's Lacrima (Fusion): Send Fiendsmith to GY
                if (activeCard.Id == CardId.FiendsmithsLacrima && activeCard.Location == CardLocation.MonsterZone)
                {
                    var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver && !Bot.Graveyard.Any(g => g != null && g.Id == CardId.FiendsmithEngraver));
                    if (engraver != null) return new List<ClientCard> { engraver };
                    var extraFiend = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Extra && IsFiendsmithCard(c));
                    if (extraFiend != null) return new List<ClientCard> { extraFiend };
                }

                // Lacrima the Crimson Tears: Send Fiendsmith to GY
                if (activeCard.Id == CardId.LacrimaTheCrimsonTears && activeCard.Location == CardLocation.MonsterZone)
                {
                    var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver && !Bot.Graveyard.Any(g => g != null && g.Id == CardId.FiendsmithEngraver));
                    if (engraver != null) return new List<ClientCard> { engraver };
                    var tract = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithsTract);
                    if (tract != null) return new List<ClientCard> { tract };
                    var paradise = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithInParadise);
                    if (paradise != null) return new List<ClientCard> { paradise };
                }

                // Desirae: Face-up monster negation
                if (activeCard.Id == CardId.FiendsmithsDesirae)
                {
                    var enemyFaceup = cards.Where(c => c != null && c.Controller != 0 && c.IsFaceup() && !c.IsDisabled()).ToList();
                    if (enemyFaceup.Count > 0)
                    {
                        var bestThreats = enemyFaceup.OrderByDescending(c => {
                            if (c == Util.GetProblematicEnemyMonster() || c == Util.GetProblematicEnemyCard()) return 100;
                            if (c.IsMonster() && c.Attack >= 2000) return 80;
                            return 50;
                        }).ToList();
                        return bestThreats.Take(max).ToList();
                    }
                }

                // Chaos Angel: Banish 1 card
                if (activeCard.Id == CardId.ChaosAngel)
                {
                    var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                                 Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                                 Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                    if (target != null && cards.Contains(target))
                    {
                        return new List<ClientCard> { target };
                    }
                }

                // A Bao A Qu: Discard cost / Destroy target / Revive target / Return to deck bottom
                if (activeCard.Id == CardId.ABaoAQu)
                {
                    if (hint == 501) return GetBestDiscardTargets(cards, min, max);
                    if (hint == 507 || cards.All(c => c != null && c.Location == CardLocation.Hand))
                    {
                        return GetBestDiscardTargets(cards, min, max);
                    }
                    var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ??
                                 cards.FirstOrDefault(c => c != null && c.Controller != 0);
                    if (target != null && cards.Contains(target)) return new List<ClientCard> { target };

                    var reviveTarget = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && (c.Id == CardId.EvilTwinsTroubleSunny || c.Id == CardId.DDDWaveHighKingCaesar || c.Id == CardId.FiendsmithsDesirae));
                    if (reviveTarget != null && cards.Contains(reviveTarget)) return new List<ClientCard> { reviveTarget };
                }
            }

            // Protect our face-up Ace cards from being used as generic materials/tributes
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                {
                    return safeCards.Take(max).ToList();
                }
            }

            // Hand discard selection (e.g. Fiendsmith's Tract / A Bao A Qu)
            if (hint == 501) // HINTMSG_DISCARD
            {
                return GetBestDiscardTargets(cards, min, max);
            }

            // Return to deck (e.g. A Bao A Qu Standby effect)
            if (hint == 507 && cards.All(c => c != null && c.Location == CardLocation.Hand))
            {
                return GetBestDiscardTargets(cards, min, max);
            }

            // Target destroy selection (e.g. Evil★Twin Lil-la destroy trigger)
            if (hint == 502 || hint == 504) // HINTMSG_DESTROY / HINTMSG_TOGRAVE
            {
                var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target != null && cards.Contains(target))
                {
                    return new List<ClientCard> { target };
                }
                var enemyCard = cards.FirstOrDefault(c => c != null && c.Controller != 0);
                if (enemyCard != null)
                {
                    return new List<ClientCard> { enemyCard };
                }
            }

            // Link material selection (hint == 533: HINTMSG_LMATERIAL)
            if (hint == 533)
            {
                var nonNullCards = cards.Where(c => c != null).ToList();
                var ordered = nonNullCards.OrderBy(c => {
                    if (c.Id == CardId.FabledLurrie) return 1;
                    if (c.Id == CardId.MoonOfTheClosedHeaven) return 2;
                    if (c.Id == CardId.FiendsmithsRequiem) return 3;
                    if (c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinLillaSweet) return 4;
                    if (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla) return 5;
                    if (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal || c.Id == CardId.EvilTwinLilla) return 6;
                    if (IsAceCard(c)) return 100;
                    return 10;
                }).ToList();

                var safeList = ordered.Where(c => !IsAceCard(c)).ToList();
                if (safeList.Count >= min)
                {
                    return safeList.Take(max).ToList();
                }
                return ordered.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        protected override bool CanDealLethal()
        {
            var ourAttackers = Bot.MonsterZone
                .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                .OrderByDescending(c => c.Attack)
                .ToList();

            if (ourAttackers.Count == 0) return false;

            var enemyMonsters = Enemy.MonsterZone
                .Where(c => c != null)
                .ToList();

            if (enemyMonsters.Count == 0)
            {
                int directDamage = ourAttackers.Sum(c => c.Attack);
                return directDamage >= Enemy.LifePoints;
            }

            int totalDamage = 0;
            var unusedAttackers = new List<ClientCard>(ourAttackers);

            var blockers = enemyMonsters.Select(c => new {
                Card = c,
                DefenseValue = c.IsFacedown() ? 1000 : (c.IsAttack() ? c.Attack : c.Defense),
                IsAttack = c.IsFaceup() && c.IsAttack()
            }).OrderByDescending(b => b.DefenseValue).ToList();

            foreach (var blocker in blockers)
            {
                ClientCard match = null;
                if (blocker.IsAttack)
                {
                    match = unusedAttackers.FirstOrDefault(a => a.Attack >= blocker.DefenseValue);
                }
                else
                {
                    match = unusedAttackers.FirstOrDefault(a => a.Attack > blocker.DefenseValue);
                }

                if (match == null)
                {
                    return false;
                }

                unusedAttackers.Remove(match);
                if (blocker.IsAttack && match.Attack > blocker.DefenseValue)
                {
                    totalDamage += (match.Attack - blocker.DefenseValue);
                }
            }

            totalDamage += unusedAttackers.Sum(a => a.Attack);
            return totalDamage >= Enemy.LifePoints;
        }

        protected override bool ShouldSkipCombo()
        {
            if (SkipComboSearch) return true;
            if (Duel.Phase == DuelPhase.Main1 && Scorer != null && Scorer.HasLethal()) return true;
            if (Duel.Phase == DuelPhase.Main1 && CanDealLethal()) return true;
            return false;
        }

        protected override bool ShouldStopExtending()
        {
            bool hasTroubleSunny = Bot.HasInMonstersZone(CardId.EvilTwinsTroubleSunny);
            bool hasCaesar = Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar);
            bool hasDesirae = Bot.HasInMonstersZone(CardId.FiendsmithsDesirae);

            if (hasTroubleSunny && (hasCaesar || hasDesirae)) return true;

            return base.ShouldStopExtending();
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.EvilTwinsTroubleSunny) || 
                Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar) || 
                Bot.HasInMonstersZone(CardId.FiendsmithsDesirae))
            {
                return true;
            }
            return base.IsBoardStrongEnough();
        }

        protected override bool HasNegateOnField()
        {
            if (base.HasNegateOnField()) return true;
            foreach (var m in Bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
                if (m.Id == CardId.DDDWaveHighKingCaesar || 
                    m.Id == CardId.FiendsmithsDesirae || 
                    m.Id == CardId.Number65DjinnBuster)
                {
                    return true;
                }
            }
            return false;
        }

        protected override int CountDisruptions()
        {
            int count = base.CountDisruptions();
            foreach (var m in Bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
                if (m.Id == CardId.EvilTwinsTroubleSunny) count += 2;
                else if (m.Id == CardId.DDDWaveHighKingCaesar) count += 2;
                else if (m.Id == CardId.FiendsmithsDesirae) count += 1;
                else if (m.Id == CardId.ABaoAQu) count += 1;
                else if (m.Id == CardId.Number65DjinnBuster) count += 1;
            }
            return count;
        }

        private bool FiendsmithSummon()
        {
            if (ShouldSkipCombo()) return false;
            bool hasTwinStarter = Bot.HasInHand(CardId.LiveTwinKiSikil) || Bot.HasInHand(CardId.LiveTwinLilla) || Bot.HasInHand(CardId.LiveTwinSunnysSnitch);
            return !hasTwinStarter && Bot.GetMonsterCount() == 0;
        }

        private bool SetTrapCondition()
        {
            return Util.IsTurn1OrMain2();
        }

        private bool MulcharmyEffect()
        {
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack >= attacker.Attack) return false;
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        private bool MonsterRepos()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || IsAceCard(monster)) continue;

                bool enemyEmpty = Enemy.GetMonsterCount() == 0;

                if (monster.IsAttack())
                {
                    if (!enemyEmpty && !IsSafeToAttack(monster) && IsSafeToDefend(monster))
                        return true;
                }
                else
                {
                    if (enemyEmpty || IsSafeToAttack(monster))
                        return true;
                }
            }
            return false;
        }

        private ClientCard GetRevivableLilla()
        {
            foreach (ClientCard card in Bot.Graveyard)
            {
                if (card == null) continue;
                if (card.IsMonster() && card.IsCanRevive() && card.Id == CardId.EvilTwinLilla)
                {
                    return card;
                }
            }
            foreach (ClientCard card in Bot.Graveyard)
            {
                if (card == null) continue;
                if (card.IsMonster() && card.IsCanRevive() && 
                    (card.Id == CardId.LiveTwinLilla || card.Id == CardId.LiveTwinLillaSweet))
                {
                    return card;
                }
            }
            return null;
        }

        private ClientCard GetRevivableKisikil()
        {
            foreach (ClientCard card in Bot.Graveyard)
            {
                if (card == null) continue;
                if (card.IsMonster() && card.IsCanRevive() && 
                    (card.Id == CardId.EvilTwinKiSikil || card.Id == CardId.EvilTwinKiSikilDeal))
                {
                    return card;
                }
            }
            foreach (ClientCard card in Bot.Graveyard)
            {
                if (card == null) continue;
                if (card.IsMonster() && card.IsCanRevive() && 
                    (card.Id == CardId.LiveTwinKiSikil || card.Id == CardId.LiveTwinKiSikilFrost))
                {
                    return card;
                }
            }
            return null;
        }

        private bool AshBlossomEffect()
        {
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CrossoutDesignatorEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;
            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultCalledByTheGrave();
        }

        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649,  // Altergeist Hexstia
                59822133  // Blue-Eyes Spirit Dragon
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool SunnysSnitchEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_snitchUsed) return false;
            if (Bot.HasInSpellZone(CardId.LiveTwinSunnysSnitch)) return false;
            if (GetRemainingCount(CardId.LiveTwinKiSikil) == 0 && 
                GetRemainingCount(CardId.LiveTwinLilla) == 0 &&
                GetRemainingCount(CardId.LiveTwinKiSikilFrost) == 0 &&
                GetRemainingCount(CardId.LiveTwinLillaSweet) == 0) return false;
            
            bool hasKis = Bot.HasInHand(CardId.LiveTwinKiSikil);
            bool hasLilla = Bot.HasInHand(CardId.LiveTwinLilla);
            bool hasFrost = Bot.HasInHand(CardId.LiveTwinKiSikilFrost);
            bool hasSweet = Bot.HasInHand(CardId.LiveTwinLillaSweet);

            if (!hasKis && !hasLilla)
                AI.SelectCard(CardId.LiveTwinKiSikil);
            else if (hasKis && !hasLilla && !hasSweet)
                AI.SelectCard(CardId.LiveTwinLillaSweet, CardId.LiveTwinLilla);
            else if (hasLilla && !hasKis && !hasFrost)
                AI.SelectCard(CardId.LiveTwinKiSikilFrost, CardId.LiveTwinKiSikil);
            else
                AI.SelectCard(CardId.LiveTwinKiSikilFrost, CardId.LiveTwinLillaSweet, CardId.LiveTwinKiSikil);
            _snitchUsed = true;
            return true;
        }

        private bool TractEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_tractUsed) return false;
            if (GetRemainingCount(CardId.FiendsmithEngraver) == 0 &&
                GetRemainingCount(CardId.FabledLurrie) == 0) return false;
            AI.SelectCard(CardId.FiendsmithEngraver, CardId.FabledLurrie);
            _tractUsed = true;
            return true;
        }

        private bool EngraverActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                if (!_engraverUsed && GetRemainingCount(CardId.FiendsmithsTract) > 0)
                {
                    AI.SelectCard(CardId.FiendsmithsTract);
                    _engraverUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                var enemyTarget = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup());
                if (enemyTarget != null)
                {
                    var equipCard = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsMonster() && c.EquipTarget != null);
                    if (equipCard != null)
                    {
                        AI.SelectCard(equipCard);
                        AI.SelectNextCard(enemyTarget);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (ShouldSkipCombo()) return false;
                if (_engraverSpSummonUsed) return false;
                
                int kisikilInGrave = Bot.Graveyard.Count(c => c != null && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
                
                // Get eligible LIGHT Fiends from GY to shuffle back
                var lightFiends = Bot.Graveyard.Where(c => c != null && c != Card && 
                    c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend)).ToList();

                // Exclude Trouble Sunny and only Ki-sikil
                var safeTargets = lightFiends.Where(c => 
                    c.Id != CardId.EvilTwinsTroubleSunny && 
                    ((c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal) || kisikilInGrave > 1)
                ).ToList();

                if (safeTargets.Count == 0) return false;

                var target = safeTargets.OrderBy(c => {
                    if (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.MoonOfTheClosedHeaven) return 1;
                    if (c.Id == CardId.FabledLurrie || c.Id == CardId.LacrimaTheCrimsonTears) return 2;
                    if (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal) return 3;
                    return 5;
                }).FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _engraverSpSummonUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LacrimaTheCrimsonTearsEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                return Bot.Hand.Count > 1;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (GetRemainingCount(CardId.FiendsmithEngraver) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.FiendsmithEngraver))
                {
                    AI.SelectCard(CardId.FiendsmithEngraver);
                }
                else if (GetRemainingCount(CardId.FiendsmithsTract) > 0)
                {
                    AI.SelectCard(CardId.FiendsmithsTract);
                }
                else if (GetRemainingCount(CardId.FiendsmithInParadise) > 0)
                {
                    AI.SelectCard(CardId.FiendsmithInParadise);
                }
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c != Card && IsFiendsmithCard(c) && c.Id != CardId.EvilTwinsTroubleSunny);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldDeferNormalSummon()
        {
            if (Util.OpponentHasNegation())
            {
                int kisikilInGrave = Bot.Graveyard.Count(c => c != null && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
                bool canSpSummonEngraverGrave = !_engraverSpSummonUsed &&
                                                Bot.Graveyard.Any(c => c != null && c.Id == CardId.FiendsmithEngraver && c.IsCanRevive()) &&
                                                Bot.Graveyard.Any(c => c != null && c.Id != CardId.FiendsmithEngraver && c.Id != CardId.EvilTwinsTroubleSunny && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend) && ((c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal) || kisikilInGrave > 1));
                bool canSpSummonEngraverHand = !_engraverSpSummonUsed &&
                                               Bot.Hand.Any(c => c != null && c.Id == CardId.FiendsmithEngraver) &&
                                               (Bot.Hand.Any(c => c != null && c.Id != CardId.FiendsmithEngraver && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend)) ||
                                                Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend)));
                bool hasBaitSpells = Bot.Hand.Any(c => c != null && (
                                        (c.Id == CardId.LiveTwinSunnysSnitch && !_snitchUsed) ||
                                        (c.Id == CardId.FiendsmithsTract && !_tractUsed)
                                     ));

                if (canSpSummonEngraverGrave || canSpSummonEngraverHand || hasBaitSpells)
                {
                    return true;
                }
            }
            return false;
        }

        private bool LiveTwinSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (ShouldDeferNormalSummon()) return false;

            bool alreadyHaveSame = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == Card.Id);
            if (alreadyHaveSame) return false;

            if (Bot.GetMonsterCount() == 0) return true;

            bool hasComplementary = false;
            if (Card.Id == CardId.LiveTwinKiSikil || Card.Id == CardId.LiveTwinKiSikilFrost)
            {
                hasComplementary = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                    (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet || c.Id == CardId.EvilTwinLilla));
            }
            else if (Card.Id == CardId.LiveTwinLilla || Card.Id == CardId.LiveTwinLillaSweet)
            {
                hasComplementary = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                    (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
            }

            if (hasComplementary) return true;

            bool hasAnyTwinOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla ||
                 c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinLillaSweet ||
                 c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinLilla ||
                 c.Id == CardId.EvilTwinKiSikilDeal));

            return !hasAnyTwinOnField;
        }

        private bool LiveTwinFrostSpSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet || c.Id == CardId.EvilTwinLilla));
        }

        private bool LiveTwinSweetSpSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
        }

        private bool EngraverSpSummon()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                if (_engraverSpSummonUsed) return false;
                bool hasLightFiend = Bot.Hand.Any(c => c != null && c != Card && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend)) ||
                                     Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
                if (hasLightFiend)
                {
                    _engraverSpSummonUsed = true;
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (ShouldSkipCombo()) return false;
                if (_engraverSpSummonUsed) return false;

                int kisikilInGrave = Bot.Graveyard.Count(c => c != null && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));

                var lightFiends = Bot.Graveyard.Where(c => c != null && c != Card && 
                    c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend)).ToList();

                var safeTargets = lightFiends.Where(c => 
                    c.Id != CardId.EvilTwinsTroubleSunny && 
                    ((c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal) || kisikilInGrave > 1)
                ).ToList();

                if (safeTargets.Count == 0) return false;

                var target = safeTargets.OrderBy(c => {
                    if (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.MoonOfTheClosedHeaven) return 1;
                    if (c.Id == CardId.FabledLurrie || c.Id == CardId.LacrimaTheCrimsonTears) return 2;
                    if (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal) return 3;
                    return 5;
                }).FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _engraverSpSummonUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LiveTwinKiSikilEffect()
        {
            if (_kisikilSummonEffectUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.GetMonsterCount() == 1)
                {
                    AI.SelectCard(CardId.LiveTwinLilla, CardId.LiveTwinLillaSweet);
                    _kisikilSummonEffectUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LiveTwinLillaEffect()
        {
            if (_lillaSummonEffectUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.GetMonsterCount() == 1)
                {
                    AI.SelectCard(CardId.LiveTwinKiSikil, CardId.LiveTwinKiSikilFrost);
                    _lillaSummonEffectUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LiveTwinFrostEffect()
        {
            if (_frostUsed) return false;
            _frostUsed = true;
            return true;
        }

        private bool LiveTwinSweetEffect()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack > 0);
        }

        private bool EvilTwinKiSikilSummon()
        {
            if (ShouldSkipCombo()) return false;
            
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal)))
                return false;

            bool hasKisikil = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost));
            int materials = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c));
            return hasKisikil && materials >= 2;
        }

        private bool EvilTwinLillaSummon()
        {
            if (ShouldSkipCombo()) return false;
            
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.EvilTwinLilla))
                return false;

            bool hasLilla = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet));
            int materials = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c));
            return hasLilla && materials >= 2;
        }

        private bool FiendsmithRequiemSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend) && 
                c.Id != CardId.FiendsmithsSequence && 
                c.Id != CardId.FiendsmithsLacrima && 
                c.Id != CardId.FiendsmithsDesirae && 
                c.Id != CardId.EvilTwinKiSikil &&
                c.Id != CardId.EvilTwinKiSikilDeal &&
                !IsAceCard(c));
        }

        private bool FiendsmithSequenceSummon()
        {
            if (ShouldSkipCombo()) return false;

            bool hasKisikil = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost || 
                 c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
                  
            bool hasLilla = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet || 
                 c.Id == CardId.EvilTwinLilla));
            
            bool canMakeTroubleSunny = hasKisikil && hasLilla && GetRemainingCount(CardId.EvilTwinsTroubleSunny) > 0;
            if (canMakeTroubleSunny)
            {
                int extraMaterials = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c) && 
                    c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal && c.Id != CardId.EvilTwinLilla);
                if (extraMaterials < 2)
                    return false;
            }

            int materials = Bot.GetMonsters().Count(c => c != null && c.HasRace(CardRace.Fiend) && !IsAceCard(c) &&
                c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal && c.Id != CardId.EvilTwinLilla);
            return materials >= 2;
        }

        private bool TroubleSunnySummon()
        {
            if (ShouldSkipCombo()) return false;
            
            bool hasKisikil = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost || 
                 c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
                 
            bool hasLilla = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet || 
                 c.Id == CardId.EvilTwinLilla));
                 
            int totalMonsters = Bot.GetMonsterCount();
            return hasKisikil && hasLilla && totalMonsters >= 2;
        }

        private bool EvilTwinKiSikilEffect()
        {
            if (_kisikilLinkEffectUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool controlNoLilla = !Bot.GetMonsters().Any(c => c != null && 
                    (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet || c.Id == CardId.EvilTwinLilla));
                if (controlNoLilla)
                {
                    var lilla = GetRevivableLilla();
                    if (lilla != null)
                    {
                        AI.SelectCard(lilla);
                        _kisikilLinkEffectUsed = true;
                        return true;
                    }
                }
                else
                {
                    _kisikilLinkEffectUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool EvilTwinLillaEffect()
        {
            if (_lillaLinkEffectUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool controlNoKisikil = !Bot.GetMonsters().Any(c => c != null && 
                    (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal));
                if (controlNoKisikil)
                {
                    var kisikil = GetRevivableKisikil();
                    if (kisikil != null)
                    {
                        AI.SelectCard(kisikil);
                        _lillaLinkEffectUsed = true;
                        return true;
                    }
                }
                else
                {
                    var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                                 Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                                 Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        _lillaLinkEffectUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TroubleSunnyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard != null && LastChainCard.Controller == 0) return false;

                var kisikil = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal))
                           ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinKiSikilFrost));
                var lilla = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && c.Id == CardId.EvilTwinLilla)
                         ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && (c.Id == CardId.LiveTwinLilla || c.Id == CardId.LiveTwinLillaSweet));
                
                if (kisikil == null || lilla == null) return false;

                // Opponent's turn: activate to disrupt
                if (Duel.Player == 1)
                {
                    AI.SelectCard(new[] { kisikil, lilla });
                    return true;
                }

                // Our turn:
                // Dodging target
                if (LastChainCard != null && LastChainCard.Controller == 1 && Util.IsChainTarget(Card))
                {
                    AI.SelectCard(new[] { kisikil, lilla });
                    return true;
                }

                // Opponent targeting our Evil★Twin monsters
                if (LastChainCard != null && LastChainCard.Controller == 1)
                {
                    bool twinTargeted = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                        Util.IsChainTarget(c) &&
                        (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal || 
                         c.Id == CardId.EvilTwinLilla || c.Id == CardId.LiveTwinKiSikil || 
                         c.Id == CardId.LiveTwinLilla));
                    if (twinTargeted)
                    {
                        AI.SelectCard(new[] { kisikil, lilla });
                        return true;
                    }
                }

                // Battle Phase extension
                bool inBattlePhase = Duel.Phase > DuelPhase.Main1 && Duel.Phase < DuelPhase.Main2;
                if (inBattlePhase && Card.Attacked)
                {
                    var otherAttackers = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && c != Card && !c.Attacked);
                    int remainingDamage = otherAttackers.Sum(c => c.Attack) + kisikil.Attack + lilla.Attack;
                    if (remainingDamage >= Enemy.LifePoints)
                    {
                        AI.SelectCard(new[] { kisikil, lilla });
                        return true;
                    }
                }

                return false;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_troubleSunnyUsed) return false;
                var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target != null)
                {
                    // Cost can be from Deck, Hand, or Field
                    var deckCost = Bot.Deck.FirstOrDefault(c => c != null && 
                        (c.Id == CardId.EvilTwinKiSikilDeal || c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla));
                    var fieldCost = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                        (c.Id == CardId.EvilTwinKiSikil || c.Id == CardId.EvilTwinKiSikilDeal || c.Id == CardId.EvilTwinLilla));

                    if (deckCost != null || fieldCost != null || Bot.Hand.Any(c => c != null && (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla)))
                    {
                        AI.SelectCard(target);
                        _troubleSunnyUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool RequiemEffect()
        {
            if (_requiemUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (GetRemainingCount(CardId.FiendsmithEngraver) > 0)
                {
                    AI.SelectCard(CardId.FiendsmithEngraver);
                    _requiemUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool SequenceEffect()
        {
            if (_sequenceUsed) return false;
            _sequenceUsed = true;
            return true;
        }

        private bool DesiraeEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_desiraeUsed) return false;
                var target = Scorer?.GetHighestThreat() ?? Util.GetProblematicEnemyMonster() ?? 
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    _desiraeUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1 && LastChainCard != null && LastChainCard.Controller == 1)
                    return false;

                var lightFiends = Bot.Graveyard.Where(c => c != null && c != Card && 
                    c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend) &&
                    c.Id != CardId.EvilTwinsTroubleSunny).ToList();

                var target = lightFiends.OrderBy(c => {
                    if (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.MoonOfTheClosedHeaven) return 1;
                    if (c.Id == CardId.FabledLurrie || c.Id == CardId.LacrimaTheCrimsonTears) return 2;
                    return 5;
                }).FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool CaesarEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        private bool DjinnBusterEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        private bool ChaosAngelEffect()
        {
            var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                         Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FiendsmithsLacrimaSpSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => c != null && c != Card && IsFiendsmithCard(c) && c.Id != CardId.EvilTwinsTroubleSunny);
            }
            return true;
        }

        private bool FiendsmithsLacrimaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_lacrimaSendUsed) return false;
                if (GetRemainingCount(CardId.FiendsmithEngraver) > 0)
                {
                    AI.SelectCard(CardId.FiendsmithEngraver);
                    _lacrimaSendUsed = true;
                    return true;
                }
                _lacrimaSendUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1 && LastChainCard != null && LastChainCard.Controller == 1)
                    return false;

                var targets = Bot.Graveyard.Where(c => c != null && c != Card && IsFiendsmithCard(c) && c.Id != CardId.EvilTwinsTroubleSunny).ToList();
                var target = targets.OrderBy(c => {
                    if (c.Id == CardId.FiendsmithsRequiem) return 1;
                    if (c.Id == CardId.FiendsmithsTract) return 2;
                    return 5;
                }).FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool FiendsmithsDesiraeSummon()
        {
            if (ShouldSkipCombo()) return false;
            bool hasFusion = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.FiendsmithsLacrima && !IsAceCard(c)) ||
                             Bot.Graveyard.Any(c => c != null && c.Id == CardId.FiendsmithsLacrima);
            bool hasLink = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link) && !IsAceCard(c)) ||
                           Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Link) && c.Id != CardId.EvilTwinsTroubleSunny);
            return hasFusion && hasLink;
        }

        private bool DDDWaveHighKingCaesarSummon()
        {
            if (ShouldSkipCombo()) return false;
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            return lv6Fiends >= 2;
        }

        private bool ChaosAngelSummon()
        {
            if (ShouldSkipCombo()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level > 0 && !IsAceCard(c)).ToList();
            int sum = monsters.Sum(c => c.Level);
            return sum == 10 && monsters.Count >= 2;
        }

        private bool MoonOfTheClosedHeavenSummon()
        {
            if (ShouldSkipCombo()) return false;
            int materials = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c) &&
                c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal && c.Id != CardId.EvilTwinLilla);
            return materials >= 2;
        }

        private bool DjinnBusterSummon()
        {
            if (ShouldSkipCombo()) return false;
            int lv2Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 2 && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            return lv2Fiends >= 2;
        }

        private bool ABaoAQuSummon()
        {
            if (ShouldSkipCombo()) return false;
            int materials = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c));
            return materials >= 3;
        }

        private bool ABaoAQuEffect()
        {
            if (Duel.Phase == DuelPhase.Standby)
            {
                // Standby Phase effect: Draw cards equal to Fiends/Zombies in GY, then place that many cards on bottom of Deck.
                // Free card filtering! Always activate in Standby phase.
                return true;
            }

            if (_abaoaquUsed) return false;
            // Main Phase quick effect: Discard 1 card to destroy 1 card on the field
            var target = Scorer?.GetBestRemovalTarget() ?? Util.GetProblematicEnemyCard() ?? 
                         Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null && Bot.Hand.Count > 0)
            {
                AI.SelectCard(target);
                _abaoaquUsed = true;
                return true;
            }
            return false;
        }

        private bool FiendsmithsAgnumdaySummon()
        {
            if (ShouldSkipCombo()) return false;
            bool hasFiendsmith = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsFiendsmithCard(c) && !IsAceCard(c));
            int totalMonsters = Bot.GetMonsters().Count(c => c != null && !IsAceCard(c) &&
                c.Id != CardId.EvilTwinKiSikil && c.Id != CardId.EvilTwinKiSikilDeal && c.Id != CardId.EvilTwinLilla);
            return hasFiendsmith && totalMonsters >= 2;
        }

        private bool NibiruEffect()
        {
            if (Duel.Player == 0) return false;
            return true;
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var nonNullCards = cards.Where(c => c != null).ToList();
            
            var preferred = nonNullCards.OrderBy(c => {
                if (c.Id == CardId.FabledLurrie) return 1;
                if (c.Id == CardId.LiveTwinKiSikilFrost || c.Id == CardId.LiveTwinLillaSweet) return 2;
                if (c.Id == CardId.LiveTwinKiSikil || c.Id == CardId.LiveTwinLilla) return 3;
                if (c.Id == CardId.LacrimaTheCrimsonTears) return 4;
                if (c.Id == CardId.FiendsmithsLacrima) return 5;
                if (IsAceCard(c)) return 100;
                return 10;
            }).ToList();

            var result = preferred.Take(max).ToList();
            if (result.Count < min)
            {
                foreach (var c in nonNullCards)
                {
                    if (!result.Contains(c)) result.Add(c);
                    if (result.Count >= min) break;
                }
            }
            return result;
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;
            if (Card == null) return base.OnSelectOption(options);

            if (Card.IsCode(CardId.NecroquipPrincess))
            {
                if (options.Count > 1) return 1; // Prefer drawing
                return 0;
            }

            return base.OnSelectOption(options);
        }

        private bool NecroquipPrincessSummon()
        {
            if (ShouldSkipCombo()) return false;
            
            bool hasEquippedMonster = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !IsAceCard(c) &&
                c.EquipCards != null && c.EquipCards.Any(eq => eq.IsMonster()));
            
            int fiendsCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            int totalMaterials = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            
            return hasEquippedMonster && fiendsCount >= 1 && totalMaterials >= 2;
        }

        private bool NecroquipPrincessEffect()
        {
            if (_necroquipUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                _necroquipUsed = true;
                return true;
            }
            return false;
        }

        private bool GhostMournerEffect()
        {
            if (Duel.Player == 0) return false;
            return true;
        }

        private bool FiendsmithInParadiseEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsFiendsmithCard(c));
        }
    }

    [Deck("Expert_2026_EvilTwin", "2026_EvilTwin")]
    public class ExpertEvilTwinExecutor : _2026_EvilTwinExecutor
    {
        private string _duelId;
        public ExpertEvilTwinExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            return base.OnSelectIdleCmd(main);
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            return base.OnBattle(attackers, defenders);
        }
    }
}
