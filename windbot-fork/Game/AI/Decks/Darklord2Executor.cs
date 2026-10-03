// ============================================================================
// CARD AUDIT — Darklord 2 (Darklords + Heralds + Fusion Engine)
// ============================================================================
// | Card Name                          | Level/Type     | Role                                        | Effect Summary & Tactics                                |
// |------------------------------------|----------------|---------------------------------------------|---------------------------------------------------------|
// | The First Darklord                 | L12 Fairy F    | Supreme Boss (4000/4000)                    | Morningstar used -> Board Wipe! All Fairies untargetable;|
// |                                    |                |                                             | Quick: Pay 1000 LP -> SS Fairy from hand/GY in DEF      |
// | Darklord Eveningstar               | L8 Fairy F     | Fusion Disrupter (3000/3000)                | On Fusion: Set 1 DL Spell + 1 DL Trap from Deck;        |
// |                                    |                |                                             | Quick: Pay 1000 LP -> Copy DL S/T in GY                 |
// | Darklord Morningstar               | L11 Fairy      | High Tribute Swarmer (3000/3000)            | Tribute Summon: SS Darklords from Deck = opp monsters;  |
// |                                    |                |                                             | Used as material for The First Darklord -> Board Wipe!  |
// | Darklord Ixchel                    | L10 Fairy      | Draw Engine + GY Copy                       | Discard self+DL -> Draw 2; Quick: Copy DL S/T in GY     |
// | Darklord Djehuty                   | L4 Fairy       | Primary Starter & GY Searcher               | NS/SS: SS Darklord from Deck; GY: Banish -> Search DL/QP|
// | Darklord Gulgolet                  | L4 Fairy       | Token Swarmer & GY Searcher                 | SS: Spawns 2 Tokens; Sent to GY: Search DL/Forbidden    |
// | Darklord Tezcatlipoca              | L9 Fairy       | Protection + GY Copy                        | Hand: Discard to protect DLs; Quick: Copy DL S/T in GY  |
// | Darklord Nasten                    | L7 Fairy       | Extender + GY Copy                          | Discard 2 DLs -> SS self; Quick: Copy DL S/T in GY      |
// | Herald of Orange Light             | L2 Fairy T     | Hand Monster Negate                         | Hand: Discard self+Fairy -> Negate & destroy monster    |
// | Banishment of the Darklords        | Normal Spell   | Universal Searcher                          | Search any Darklord card from Deck                      |
// | Darklord Dance                     | Normal Spell   | Fusion Spell (+1000 ATK)                    | Banish materials from hand/field -> Fusion DARK Fairy   |
// | Apex Polymerization                | Quick Spell    | Unrespondable Fusion                        | Pay 2000 LP -> Fusion Summon using field materials      |
// | Darklord Rebellion                 | Normal Spell   | Targeted Removal                            | Normal: Send DL -> Pop card; Copy: Pop with 0 cost!     |
// | The Sanctified Darklord            | Normal Trap    | Monster Negate + ATK Gain                   | Normal: Send DL -> Negate; Copy: Negate with 0 cost!    |
// | Condemned Darklord                 | Link-2 Fairy   | Link Bridge & Tribute Enabler               | Discard 1 -> Search/mill DL; Banish 2 from GY to tribute|
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Darklord 2", "Darklord 2")]
    [Deck("Darklord2", "Darklord 2")]
    public class Darklord2Executor : ModernExecutor
    {
        public class CardId
        {
            // Darklords
            public const int DarklordMorningstar = 25451652;
            public const int DarklordIxchel = 52840267;
            public const int DarklordGulgolet = 84031359;
            public const int DarklordDjehuty = 10426067;
            public const int DarklordTezcatlipoca = 88234365;
            public const int DarklordNasten = 25339070;
            public const int IndulgedDarklord = 82773292;
            public const int BanishmentOfTheDarklords = 87112784;
            public const int DarklordContact = 14517422;
            public const int DarklordDance = 99941223;
            public const int TheSanctifiedDarklord = 48152161;
            public const int DarklordRebellion = 50501121;
            public const int FallenAngelInDarkness = 93023136;

            // Heralds & Handtraps
            public const int HeraldOfOrangeLight = 17266660;
            public const int HeraldOfGreenLight = 21074344;
            public const int GhostOgre = 59438930;
            public const int GhostBelle = 73642296;
            public const int CalledByTheGrave = 24224830;

            // Spells
            public const int ApexPolymerization = 44886582;
            public const int UnleashedPowerPatronPortalTerminus = 25661743;
            public const int ForbiddenDroplet = 24299458;
            public const int ForbiddenCrown = 98829635;
            public const int ForbiddenChalice = 25789292;

            // Extra Deck
            public const int TheFirstDarklord = 4167084;
            public const int DarklordEveningstar = 10136446;
            public const int CondemnedDarklord = 35306215;
            public const int PredaplantDragostapelia = 69946549;
            public const int StarvingVenom = 41209827;
            public const int Garura = 11765832;
            public const int TheDukeOfDemise = 45445571;
            public const int AzaminaMoaRegina = 46174776;
            public const int SaintAzamina = 85065943;
            public const int Azamina = 17749468;
            public const int WarpGateTimelord = 67508932;
        }

        public Darklord2Plugin Plugin { get; }

        private bool _djehutySummonSpUsed = false;
        private bool _djehutyGySearchUsed = false;
        private bool _gulgoletTokenSpUsed = false;
        private bool _ixchelDrawUsed = false;
        private bool _firstDarklordSpUsed = false;
        private bool _eveningstarSetUsed = false;
        private bool _copiedRebellionThisTurn = false;
        private bool _copiedSanctifiedThisTurn = false;

        public Darklord2Executor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new Darklord2Plugin(this);
            DeckPlugin = Plugin;

            BaitPlanner.RegisterComboStarters(CardId.BanishmentOfTheDarklords, CardId.DarklordDjehuty, CardId.DarklordDance);
            BaitPlanner.RegisterBaitCards(CardId.DarklordIxchel, CardId.DarklordContact, CardId.ForbiddenChalice);

            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _djehutySummonSpUsed = false;
            _djehutyGySearchUsed = false;
            _gulgoletTokenSpUsed = false;
            _ixchelDrawUsed = false;
            _firstDarklordSpUsed = false;
            _eveningstarSetUsed = false;
            _copiedRebellionThisTurn = false;
            _copiedSanctifiedThisTurn = false;
            // Plugin reset is done by ModernExecutor.OnNewTurn() → DeckPlugin?.ResetTurnState()
        }

        private bool IsGoingFirstTurn()
        {
            return Duel.Turn == 1 && Duel.Player == 0;
        }

        private bool IsChainAlreadyNeutralized()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return false;
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Djehuty -> Gulgolet -> First Darklord Fusion ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Darklord2-Djehuty-Starter",
                RequiredCards = new List<int> { CardId.DarklordDjehuty },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.DarklordDjehuty, ActionType = ExecutorType.Summon, Description = "Normal Summon Djehuty" },
                    new() { CardId = CardId.DarklordDjehuty, ActionType = ExecutorType.Activate, Description = "Djehuty Special Summons Gulgolet from Deck" }
                }
            });

            // ── Line 2: Banishment Search Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Darklord2-Banishment-Search",
                RequiredCards = new List<int> { CardId.BanishmentOfTheDarklords },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BanishmentOfTheDarklords, ActionType = ExecutorType.Activate, Description = "Search Darklord Dance or Djehuty" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTERS, HANDTRAPS & QUICK DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfOrangeLight, HeraldOfOrangeLightActivate);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfGreenLight, HeraldOfGreenLightActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenChalice, ForbiddenChaliceActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: SEARCHERS, DRAW & GRAVEYARD SETUP
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.BanishmentOfTheDarklords, BanishmentActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarklordIxchel, IxchelHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusActivate);
            AddExecutor(ExecutorType.Activate, CardId.FallenAngelInDarkness, FallenAngelActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: FUSION SUMMONS & GRAVEYARD RECOVERY
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarklordDance, DarklordDanceActivate);
            AddExecutor(ExecutorType.Activate, CardId.ApexPolymerization, ApexPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarklordContact, DarklordContactActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarklordNasten, NastenHandEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: MONSTER SUMMONS & FIELD IGNITIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.DarklordDjehuty, DjehutySummon);
            AddExecutor(ExecutorType.Activate, CardId.DarklordDjehuty, DjehutyEffect);

            AddExecutor(ExecutorType.Activate, CardId.DarklordGulgolet, GulgoletEffect);

            AddExecutor(ExecutorType.Summon, CardId.IndulgedDarklord, IndulgedSummon);
            AddExecutor(ExecutorType.Activate, CardId.IndulgedDarklord, IndulgedEffect);

            AddExecutor(ExecutorType.Summon, CardId.DarklordMorningstar, MorningstarSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarklordMorningstar, MorningstarEffect);

            AddExecutor(ExecutorType.Summon, FallbackSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: DARKLORD GY COPY ENGINE (QUICK EFFECTS: ZERO COST COPIES!)
            // ═══════════════════════════════════════════════════════════════
            // Darklord Eveningstar / Ixchel / Tezcatlipoca / Nasten copying from GY:
            AddExecutor(ExecutorType.Activate, CardId.TheFirstDarklord, TheFirstDarklordEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordEveningstar, EveningstarEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordIxchel, IxchelFieldCopyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordTezcatlipoca, TezcatlipocaCopyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordNasten, NastenFieldCopyEffect);

            // Normal Activation of S/T from Hand/Field
            AddExecutor(ExecutorType.Activate, CardId.DarklordRebellion, DarklordRebellionActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheSanctifiedDarklord, SanctifiedDarklordActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: EXTRA DECK SUMMONS & LINKS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.CondemnedDarklord, CondemnedDarklordSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CondemnedDarklord, CondemnedDarklordEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TheFirstDarklord);
            AddExecutor(ExecutorType.SpSummon, CardId.DarklordEveningstar);
            AddExecutor(ExecutorType.SpSummon, CardId.PredaplantDragostapelia);
            AddExecutor(ExecutorType.SpSummon, CardId.StarvingVenom);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: SPELL SETS & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.TheSanctifiedDarklord, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.DarklordRebellion, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenChalice, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool HeraldOfOrangeLightActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster())
            {
                // Discard self + 1 Fairy (every Darklord is Fairy!)
                var fairyFodder = Bot.Hand.Where(c => c != Card && c.HasRace(CardRace.Fairy)).ToList();
                if (fairyFodder.Count > 0)
                {
                    var discard = Plugin.MaterialImpl.PickDiscardTarget(fairyFodder);
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HeraldOfGreenLightActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1 && lastCard.IsSpell())
            {
                var fairyFodder = Bot.Hand.Where(c => c != Card && c.HasRace(CardRace.Fairy)).ToList();
                if (fairyFodder.Count > 0)
                {
                    var discard = Plugin.MaterialImpl.PickDiscardTarget(fairyFodder);
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool GhostOgreActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool GhostBelleActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool CalledByTheGraveActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
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
            return false;
        }

        private bool ForbiddenDropletActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            var oppTargets = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget()).ToList();
            if (oppTargets.Count == 0) return false;

            var sendFodder = Bot.Hand.Where(c => c != Card && c.Id != CardId.TheFirstDarklord && c.Id != CardId.DarklordDance).Take(oppTargets.Count).ToList();
            if (sendFodder.Count > 0)
            {
                AI.SelectCard(sendFodder);
                AI.SelectNextCard(oppTargets);
                return true;
            }
            return false;
        }

        private bool ForbiddenChaliceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ForbiddenCrownActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
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
        //  SEARCHERS & DRAW ENGINE
        // ═══════════════════════════════════════════════════════════════

        private bool BanishmentActivate()
        {
            return true;
        }

        private bool IxchelHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard self + 1 Darklord card to draw 2!
                if (_ixchelDrawUsed) return false;
                var otherDL = Bot.Hand.Where(c => c != Card && (c.Id == CardId.DarklordGulgolet || c.Id == CardId.DarklordTezcatlipoca || c.Id == CardId.DarklordDjehuty || c.Id == CardId.DarklordMorningstar || c.Id == CardId.DarklordNasten)).FirstOrDefault();
                if (otherDL != null)
                {
                    _ixchelDrawUsed = true;
                    AI.SelectCard(otherDL);
                    return true;
                }
            }
            return false;
        }

        private bool TerminusActivate()
        {
            // Terminus prevents attacks except Power Patron monsters this turn.
            // On Turn 1 (Going First), it is pure advantage!
            if (IsGoingFirstTurn() || Duel.Turn == 1) return true;
            // On subsequent turns, only use if in Main Phase 2 or if we cannot declare lethal attack
            return Duel.Phase == DuelPhase.Main2 || Bot.GetMonsters().Count(m => m.IsFaceup() && m.Attack >= 2500) == 0;
        }

        private bool FallenAngelActivate()
        {
            var fairy = Bot.Hand.Concat(Bot.GetMonsters()).FirstOrDefault(c => c != Card && c.HasRace(CardRace.Fairy));
            if (fairy != null)
            {
                AI.SelectCard(fairy);
                AI.SelectNextCard(CardId.DarklordDjehuty, CardId.DarklordMorningstar, CardId.DarklordIxchel);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  FUSION SUMMONS & RECOVERY
        // ═══════════════════════════════════════════════════════════════

        private bool DarklordDanceActivate()
        {
            // Banish materials from hand/field/GY to Fusion summon DARK Fairy Fusion
            // Prioritize The First Darklord > Darklord Eveningstar
            AI.SelectCard(CardId.TheFirstDarklord, CardId.DarklordEveningstar, CardId.PredaplantDragostapelia);
            return true;
        }

        private bool ApexPolymerizationActivate()
        {
            if (Bot.LifePoints <= 2000) return false;
            // Apex Poly targets 1 face-up Effect monster on field to fuse with an Extra Deck summon
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Effect) && m.Level > 0);
        }

        private bool DarklordContactActivate()
        {
            ClientCard revTarget = Bot.Graveyard.Where(c => c.IsMonster() && c.HasRace(CardRace.Fairy))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (revTarget != null)
            {
                AI.SelectCard(revTarget);
                return true;
            }
            return false;
        }

        private bool NastenHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                var dlCards = Bot.Hand.Where(c => c != Card && c.Id != CardId.BanishmentOfTheDarklords && c.Id != CardId.DarklordDance).Take(2).ToList();
                if (dlCards.Count == 2)
                {
                    AI.SelectCard(dlCards);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS ON FIELD
        // ═══════════════════════════════════════════════════════════════

        private bool DjehutySummon()
        {
            return true;
        }

        private bool DjehutyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_djehutySummonSpUsed)
                {
                    _djehutySummonSpUsed = true;
                    // SS Gulgolet (spawns 2 tokens) or Ixchel
                    AI.SelectCard(CardId.DarklordGulgolet, CardId.DarklordIxchel, CardId.DarklordTezcatlipoca);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (!_djehutyGySearchUsed && Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Fusion) && m.HasRace(CardRace.Fairy)))
                {
                    _djehutyGySearchUsed = true;
                    AI.SelectCard(CardId.BanishmentOfTheDarklords, CardId.DarklordDance, CardId.DarklordContact);
                    return true;
                }
            }
            return true;
        }

        private bool GulgoletEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_gulgoletTokenSpUsed && Bot.GetMonsterCount() <= 3)
                {
                    _gulgoletTokenSpUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.BanishmentOfTheDarklords, CardId.DarklordDance, CardId.DarklordContact);
                return true;
            }
            return true;
        }

        private bool IndulgedSummon()
        {
            return true;
        }

        private bool IndulgedEffect()
        {
            return true;
        }

        private bool MorningstarSummon()
        {
            // Tribute summon when opponent has face-up effect monsters
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Effect)) && Bot.GetMonsterCount() >= 2;
        }

        private bool MorningstarEffect()
        {
            return true;
        }

        private bool FallbackSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  DARKLORD GY COPY ENGINE (PAY 1000 LP -> 0 COST COPY!)
        // ═══════════════════════════════════════════════════════════════

        private bool TheFirstDarklordEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Quick effect: Pay 1000 LP -> SS 1 Fairy from hand/GY in DEF
                if (!_firstDarklordSpUsed && Bot.LifePoints > 1000 && Bot.GetMonsterCount() < 5)
                {
                    ClientCard rev = Bot.Graveyard.Where(c => c.IsMonster() && c.HasRace(CardRace.Fairy) && c != Card)
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();
                    if (rev != null)
                    {
                        _firstDarklordSpUsed = true;
                        AI.SelectCard(rev);
                        return true;
                    }
                }
            }
            return true;
        }

        private bool EveningstarEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On Fusion summon: Set 1 DL Spell + 1 DL Trap simultaneously from Deck!
                if (!_eveningstarSetUsed)
                {
                    _eveningstarSetUsed = true;
                    // First pick Spell (Banishment, Contact, Dance)
                    AI.SelectCard(CardId.BanishmentOfTheDarklords, CardId.DarklordContact, CardId.DarklordDance);
                    // Second pick Trap (Sanctified, Rebellion)
                    AI.SelectNextCard(CardId.TheSanctifiedDarklord, CardId.DarklordRebellion);
                    return true;
                }

                // Quick effect: Copy DL S/T in GY
                return ExecuteDarklordGyCopy();
            }
            return true;
        }

        private bool IxchelFieldCopyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return ExecuteDarklordGyCopy();
            }
            return false;
        }

        private bool TezcatlipocaCopyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return ExecuteDarklordGyCopy();
            }
            return false;
        }

        private bool NastenFieldCopyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return ExecuteDarklordGyCopy();
            }
            return false;
        }

        private bool ExecuteDarklordGyCopy()
        {
            if (Bot.LifePoints <= 1000) return false;

            // Priority 1: Darklord Rebellion in GY -> Destroy 1 opponent card (WITHOUT monster cost!)
            if (!_copiedRebellionThisTurn && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0)
            {
                ClientCard rebellionInGy = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.DarklordRebellion);
                if (rebellionInGy != null)
                {
                    ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: false, canBeTarget: true);
                    if (oppTarget != null)
                    {
                        _copiedRebellionThisTurn = true;
                        AI.SelectCard(rebellionInGy);
                        AI.SelectNextCard(oppTarget);
                        return true;
                    }
                }
            }

            // Priority 2: The Sanctified Darklord in GY -> Negate 1 opponent monster + gain ATK (WITHOUT monster cost!)
            if (!_copiedSanctifiedThisTurn && Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget()))
            {
                ClientCard sanctifiedInGy = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                if (sanctifiedInGy != null)
                {
                    ClientCard oppMonster = Enemy.GetMonsters()
                        .Where(m => m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
                        .OrderByDescending(m => m.Attack)
                        .FirstOrDefault();
                    if (oppMonster != null)
                    {
                        _copiedSanctifiedThisTurn = true;
                        AI.SelectCard(sanctifiedInGy);
                        AI.SelectNextCard(oppMonster);
                        return true;
                    }
                }
            }

            // Priority 3: Darklord Contact in GY -> Special Summon Darklord from GY
            ClientCard contactInGy = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.DarklordContact);
            if (contactInGy != null && Bot.GetMonsterCount() < 5)
            {
                ClientCard revTarget = Bot.Graveyard.Where(c => c.IsMonster() && c.HasRace(CardRace.Fairy) && c != Card)
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (revTarget != null)
                {
                    AI.SelectCard(contactInGy);
                    AI.SelectNextCard(revTarget);
                    return true;
                }
            }

            return false;
        }

        private bool DarklordRebellionActivate()
        {
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() == 0) return false;
            var oppTarget = GetBestRemovalTarget(onlyFaceup: false, canBeTarget: true);
            if (oppTarget != null)
            {
                var sendCost = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Token))
                            ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet || c.Id == CardId.DarklordDjehuty);
                if (sendCost != null)
                {
                    AI.SelectCard(sendCost);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool SanctifiedDarklordActivate()
        {
            var oppTarget = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (oppTarget != null)
            {
                var sendCost = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Token))
                            ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet || c.Id == CardId.DarklordDjehuty);
                if (sendCost != null)
                {
                    AI.SelectCard(sendCost);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SUMMONS & LINKS
        // ═══════════════════════════════════════════════════════════════

        private bool CondemnedDarklordSpSummon()
        {
            return Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Fairy)) >= 2 && Bot.GetMonsterCount() <= 3;
        }

        private bool CondemnedDarklordEffect()
        {
            var discard = Plugin.MaterialImpl.PickDiscardTarget(Bot.Hand.ToList());
            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.DarklordMorningstar, CardId.DarklordDjehuty, CardId.DarklordIxchel);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS & SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2 || IsGoingFirstTurn();
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card == null) return false;
            if (Card.IsAttack() && Card.Attack == 0) return true;
            if (Card.IsDefense() && Card.Attack >= 2500 && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0) return true;
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.TheFirstDarklord || cardId == CardId.DarklordEveningstar || cardId == CardId.DarklordMorningstar)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // The First Darklord board wipe (when Morningstar used) -> YES!
            if (desc == Util.GetStringId(CardId.TheFirstDarklord, 0)) return true;
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                    if (searchTarget != null) return new List<ClientCard> { searchTarget };
                }

                if (hint == 501 || (cards.All(c => c.Location == CardLocation.Hand) && !cancelable))
                {
                    var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (discardTarget != null) return new List<ClientCard> { discardTarget };
                }

                if (hint == 502 || hint == 503 || hint == 504)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).OrderByDescending(c => Scorer.ThreatScore(c)).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.Take(Math.Min(max, enemyTargets.Count)).ToList();
                    }
                }

                if (hint == 509)
                {
                    var spTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                    if (spTarget != null) return new List<ClientCard> { spTarget };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
