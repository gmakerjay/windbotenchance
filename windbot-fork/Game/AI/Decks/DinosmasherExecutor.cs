// ============================================================================
// CARD AUDIT — Dinosmasher (Modernized STR38 Dinosmasher's Fury - Token Clog & Target-Lock)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|
// | Ultimate Conductor Tyranno(18940556)| Monster L10 | Yes  | No    | Destroy | Book of Eclipse all opp monsters; 1000 burn/mon| Main Phase / Battle Phase                    |
// | Souleating Oviraptor (44335251)    | Monster L4   | Yes  | Yes   | Target  | Search/Dump Dino; destroy token to revive Dino| Main Phase starter & combo extension         |
// | Miscellaneousaurus (38572779)      | Monster L4   | Yes  | Yes   | Discard | Dinos unaffected during MP; GY banish SS Dino | Start of Main Phase 1 protection              |
// | Babycerasaurus (36042004)          | Monster L2   | No   | No    | None    | When destroyed: SS Level 4- Dino from Deck   | Destroyed by card effect                     |
// | Petiteranodon (82946847)           | Monster L2   | No   | No    | None    | When destroyed: SS Level 4+ Dino from Deck   | Destroyed by card effect                     |
// | Animadorned Archosaur (98022050)   | Monster L1   | Yes  | Yes   | Destroy | Destroy Dino to search Double Evolution Pill  | On Normal/Special summon                     |
// | Lost World (17228908)              | Spell Field  | Yes  | No    | None    | SS Jurraegg Token to opp; target-lock; deck pop| Before summoning Dinosaurs                   |
// | Double Evolution Pill (38179121)   | Spell Normal | Yes  | Yes   | Banish  | Banish 1 Dino + 1 Non-Dino -> SS UCT from deck| Have 1 Dino + 1 Non-Dino in hand/GY          |
// | Ojama Trio (29843091)              | Trap Normal  | No   | No    | None    | SS 3 Ojama Tokens to opp field (clog zones)   | Opponent turn: clog their monster zones      |
// | Survival's End (44612603)          | Trap Normal  | No   | No    | None    | Destroy Normal mons/Tokens -> SS Dino from deck| Opp has Jurraegg/Ojama tokens                |
// | Evolzar Lars (35103106)            | Xyz Rank 6   | Yes  | Yes   | Detach  | Quick: Negate any face-up card effect (2x/trn)| Opponent activates card effect               |
// | Evolzar Dolkka (42752141)          | Xyz Rank 4   | No   | No    | Detach  | Quick: Negate monster effect & destroy (2x)   | Opponent monster effect                      |
// | Evolzar Laggia (74294676)          | Xyz Rank 4   | Yes  | No    | Detach 2| Quick: Omni-negate Spell/Trap/Summon          | Opponent dangerous play                      |
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
    [Deck("Dinosmasher", "Dinosmasher")]
    public class DinosmasherExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int UltimateConductorTyranno = 18940556;
            public const int SouleatingOviraptor = 44335251;
            public const int Babycerasaurus = 36042004;
            public const int Petiteranodon = 82946847;
            public const int Miscellaneousaurus = 38572779;
            public const int AnimadornedArchosaur = 98022050;
            public const int XenoMeteorus = 5852388;
            public const int Frostosaurus = 6631034;
            public const int MegalosmasherX = 81823360;
            public const int GiantRex = 80280944;
            public const int Pankratops = 82385847;
            public const int Dogoran = 93332803;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;

            // Spells & Traps
            public const int GroundXeno = 67523044;
            public const int LostWorld = 17228908;
            public const int JurraeggToken = 17228909;
            public const int FossilDig = 47325505;
            public const int Terraforming = 73628505;
            public const int DoubleEvolutionPill = 38179121;
            public const int DarkRulerNoMore = 54693926;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;
            public const int HarpieFeatherDuster = 18144506;
            public const int InfiniteImpermanence = 10045474;
            public const int OjamaTrio = 29843091;
            public const int OjamaToken = 29843092;
            public const int SurvivalEnd = 46009906;

            // Extra Deck
            public const int EvolzarLars = 35103106;
            public const int EvolzarDolkka = 42752141;
            public const int EvolzarLaggia = 74294676;
            public const int AbyssDweller = 21044178;
            public const int Number41Bagooska = 90590303;
            public const int Dugares = 66011101;
            public const int Linkuriboh = 41999284;
            public const int SecureGardna = 2220237;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int SPLittleKnight = 29301450;
            public const int AccesscodeTalker = 86066372;
            public const int TornadoDragon = 6983839;
        }

        internal DinosmasherPlugin Plugin { get; private set; }
        private bool _lostWorldSubUsedThisTurn = false;
        private bool _pillUsedThisTurn = false;
        private bool _potOfProsperityUsedThisTurn = false;
        private bool _tttUsedThisTurn = false;
        private bool _survivalEndGYUsedThisTurn = false;
        private bool _archosaurUsedThisTurn = false;
        private bool _miscHandUsedThisTurn = false;
        private bool _miscGYUsedThisTurn = false;
        private bool _oviraptorSummonUsedThisTurn = false;
        private bool _oviraptorReviveUsedThisTurn = false;
        private bool _uctEclipseUsedThisTurn = false;

        public DinosmasherExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new DinosmasherPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _lostWorldSubUsedThisTurn = false;
            _pillUsedThisTurn = false;
            _potOfProsperityUsedThisTurn = false;
            _tttUsedThisTurn = false;
            _survivalEndGYUsedThisTurn = false;
            _archosaurUsedThisTurn = false;
            _miscHandUsedThisTurn = false;
            _miscGYUsedThisTurn = false;
            _oviraptorSummonUsedThisTurn = false;
            _oviraptorReviveUsedThisTurn = false;
            _uctEclipseUsedThisTurn = false;
            Plugin?.ResetTurnState();
        }

        private void RegisterHelperModules()
        {
            ResourcePlan.RegisterAceCards(
                CardId.UltimateConductorTyranno,
                CardId.EvolzarLars,
                CardId.EvolzarDolkka,
                CardId.EvolzarLaggia
            );
            HeuristicGuard.RegisterAceCards(
                CardId.UltimateConductorTyranno,
                CardId.EvolzarLars,
                CardId.EvolzarDolkka,
                CardId.EvolzarLaggia
            );
            BaitPlanner.RegisterComboStarters(
                CardId.LostWorld,
                CardId.FossilDig,
                CardId.SouleatingOviraptor
            );
            ChainAdvisor.RegisterHighValueTargets(
                CardId.UltimateConductorTyranno,
                CardId.EvolzarLars,
                CardId.EvolzarDolkka,
                CardId.EvolzarLaggia,
                CardId.SouleatingOviraptor
            );
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: HAND PROTECTION, INTERRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            // Miscellaneousaurus: Activate immediately at start of Main 1 to make all Dinos unaffected!
            AddExecutor(ExecutorType.Activate, CardId.Miscellaneousaurus, MiscHandActivate);

            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, () => DefaultCalledByTheGrave());
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, () => DefaultInfiniteImpermanence());
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, () => DefaultEffectVeiler());

            // Evolzar Boss Negations (Priority Tier 0)
            AddExecutor(ExecutorType.Activate, CardId.EvolzarLars, EvolzarLarsActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarDolkka, EvolzarDolkkaActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarLaggia, EvolzarLaggiaActivate);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);
            AddExecutor(ExecutorType.Activate, CardId.Number41Bagooska);

            // Ultimate Conductor Tyranno Quick Effect: Book of Eclipse on opponent's monsters
            AddExecutor(ExecutorType.Activate, CardId.UltimateConductorTyranno, UCTActivate);

            // Pankratops Quick Spot Removal
            AddExecutor(ExecutorType.Activate, CardId.Pankratops, PankratopsActivate);

            // Dogoran Kaiju Special Summon over opponent's boss (Break towers before activating effects!)
            AddExecutor(ExecutorType.SpSummon, CardId.Dogoran, DogoranSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: FIELD SPELL, SEARCHERS & CONSISTENCY
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, () => Enemy.GetSpellCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreActivate);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, () => true);
            AddExecutor(ExecutorType.Activate, CardId.LostWorld, LostWorldActivate);
            AddExecutor(ExecutorType.Activate, CardId.GroundXeno, GroundXenoActivate);
            AddExecutor(ExecutorType.Activate, CardId.FossilDig, FossilDigActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: STARTERS, COMBO EXTENDERS & FLOATERS
            // ═══════════════════════════════════════════════════════════════
            // Souleating Oviraptor Normal Summon & Effects
            AddExecutor(ExecutorType.Summon, CardId.SouleatingOviraptor);
            AddExecutor(ExecutorType.Activate, CardId.SouleatingOviraptor, OviraptorActivate);

            // Baby floaters (Automatically resolve when popped)
            AddExecutor(ExecutorType.Activate, CardId.Babycerasaurus, BabycerasaurusActivate);
            AddExecutor(ExecutorType.Activate, CardId.Petiteranodon, PetiteranodonActivate);

            // Animadorned Archosaur
            AddExecutor(ExecutorType.Summon, CardId.AnimadornedArchosaur, ArchosaurSummon);
            AddExecutor(ExecutorType.Activate, CardId.AnimadornedArchosaur, ArchosaurActivate);

            // Miscellaneousaurus GY revival effect (Banish 1 for Archosaur!)
            AddExecutor(ExecutorType.Activate, CardId.Miscellaneousaurus, MiscGYActivate);

            // Giant Rex revival on banish
            AddExecutor(ExecutorType.Activate, CardId.GiantRex);

            // Xeno Meteorus special summon & level climb
            AddExecutor(ExecutorType.Activate, CardId.XenoMeteorus, XenoMeteorusActivate);

            // Double Evolution Pill -> SS Ultimate Conductor Tyranno
            AddExecutor(ExecutorType.Activate, CardId.DoubleEvolutionPill, DoubleEvolutionPillActivate);

            // Ultimate Conductor Tyranno Special Summon from Hand (Banish 2 Dinos)
            AddExecutor(ExecutorType.SpSummon, CardId.UltimateConductorTyranno, UCTSpSummon);

            // Pankratops Special Summon going second
            AddExecutor(ExecutorType.SpSummon, CardId.Pankratops, () => Enemy.GetMonsterCount() > Bot.GetMonsterCount());

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: EXTRA DECK SUMMONS (Evolzar Xyz & Link Climbing)
            // ═══════════════════════════════════════════════════════════════
            // Archosaur (Level 1) -> Linkuriboh -> Secure Gardna (Guarantees Non-Dino in GY for Pill!)
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SecureGardna, SecureGardnaSpSummon);

            // Evolzar Lars (Rank 6 with Xeno Meteorus + Frostosaurus)
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarLars, EvolzarLarsSpSummon);

            // Evolzar Dolkka / Laggia (Rank 4 with Oviraptor + Giant Rex)
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarDolkka, EvolzarDolkkaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarLaggia, EvolzarLaggiaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, BagooskaSpSummon);

            // Utility Xyz & Links
            AddExecutor(ExecutorType.SpSummon, CardId.Dugares, () => Bot.GetMonsterCount() >= 2 && Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.Activate, CardId.Dugares, DugaresActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, () => Bot.GetMonsterCount() >= 3 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, () =>
            {
                int lv4Dinos = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.Dinosaur));
                return lv4Dinos < 2 && Bot.GetMonsterCount() >= 2 && Enemy.GetSpellCount() > 0;
            });
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, () => Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() >= 2);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: NORMAL SUMMONS & SETS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.Babycerasaurus, () => Bot.GetMonsterCount() == 0 && Bot.HasInHand(CardId.LostWorld));

            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        private bool IsCurrentCardNegated()
        {
            return Card != null && DefaultCheckWhetherCardIsNegated(Card);
        }

        private bool MiscHandActivate()
        {
            if (_miscHandUsedThisTurn) return false;
            // Protection shield: Discard Misc at start of Main 1 to make all Dinos completely unaffected!
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
            {
                _miscHandUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MiscGYActivate()
        {
            if (_miscGYUsedThisTurn) return false;
            if (Card.Location == CardLocation.Grave)
            {
                int dinoCount = Bot.Graveyard.Count(c => c.HasRace(CardRace.Dinosaur));
                // Banish 1 (Misc itself) -> Special Summon Archosaur to search Pill!
                if (dinoCount >= 1 && !Bot.HasInMonstersZone(CardId.AnimadornedArchosaur) && !Bot.HasInHand(CardId.DoubleEvolutionPill))
                {
                    _miscGYUsedThisTurn = true;
                    AI.SelectCard(CardId.AnimadornedArchosaur);
                    return true;
                }
                // Banish 2 -> Special Summon Babycerasaurus
                if (dinoCount >= 2 && !Bot.HasInMonstersZone(CardId.Babycerasaurus))
                {
                    _miscGYUsedThisTurn = true;
                    AI.SelectCard(CardId.Babycerasaurus);
                    return true;
                }
                // Banish 4 -> Special Summon Oviraptor from Deck!
                if (dinoCount >= 4 && !Bot.HasInMonstersZone(CardId.SouleatingOviraptor))
                {
                    _miscGYUsedThisTurn = true;
                    AI.SelectCard(CardId.SouleatingOviraptor);
                    return true;
                }
            }
            return false;
        }

        private bool LostWorldActivate()
        {
            return !Bot.HasInSpellZone(CardId.LostWorld);
        }

        private bool FossilDigActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // ROTA for Dinosaur: Search Oviraptor > Misc > Babycerasaurus
            AI.SelectCard(CardId.SouleatingOviraptor, CardId.Miscellaneousaurus, CardId.Babycerasaurus);
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            if (_potOfProsperityUsedThisTurn || IsCurrentCardNegated()) return false;
            var banishList = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.EvolzarLars &&
                c.Id != CardId.EvolzarDolkka &&
                c.Id != CardId.EvolzarLaggia &&
                c.Id != CardId.Linkuriboh &&
                c.Id != CardId.SecureGardna
            ).Take(6).ToList();

            if (banishList.Count >= 3)
            {
                _potOfProsperityUsedThisTurn = true;
                AI.SelectCard(banishList);
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (_tttUsedThisTurn || IsCurrentCardNegated()) return false;
            _tttUsedThisTurn = true;
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500))
                AI.SelectOption(1); // Steal
            else
                AI.SelectOption(0); // Draw 2
            return true;
        }

        private bool CrossoutActivate()
        {
            if (IsCurrentCardNegated()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.Id == CardId.AshBlossom ||
                    LastChainCard.Id == CardId.InfiniteImpermanence)
                {
                    AI.SelectAnnounceID(LastChainCard.Id);
                    return true;
                }
            }
            return false;
        }

        private bool OviraptorActivate()
        {
            if (IsCurrentCardNegated()) return false;

            // Trigger 1: On-Summon Search or Dump (HOPT)
            if (Card.Location == CardLocation.MonsterZone && (Duel.LastChainPlayer == 0 || Duel.CurrentChain.Count == 0))
            {
                if (!_oviraptorSummonUsedThisTurn)
                {
                    _oviraptorSummonUsedThisTurn = true;
                    AI.SelectCard(CardId.Babycerasaurus, CardId.Miscellaneousaurus, CardId.UltimateConductorTyranno);
                    return true;
                }
            }

            // Trigger 2: On-Field Ignition Destruction & Revival (HOPT)
            if (_oviraptorReviveUsedThisTurn) return false;

            // CRITICAL GUARD: MUST have at least 1 Dinosaur in GY to revive!
            bool hasDinoInGY = Bot.Graveyard.Any(c => c.HasRace(CardRace.Dinosaur));
            if (!hasDinoInGY) return false;

            // Target Priority:
            // 1. Jurraegg Token on opponent's field (Lost World will pop Baby from Deck instead!)
            // 2. Babycerasaurus or Petiteranodon on our field (they float into new monsters!)
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.Id == CardId.JurraeggToken || m.HasType(CardType.Token))
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.Babycerasaurus || m.Id == CardId.Petiteranodon);

            if (target != null)
            {
                _oviraptorReviveUsedThisTurn = true;
                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        private bool BabycerasaurusActivate()
        {
            // SS Level 4 or lower Dinosaur from Deck on destruction
            AI.SelectCard(CardId.SouleatingOviraptor, CardId.AnimadornedArchosaur, CardId.GiantRex);
            return true;
        }

        private bool PetiteranodonActivate()
        {
            // SS Level 4 or higher Dinosaur from Deck on destruction
            AI.SelectCard(CardId.XenoMeteorus, CardId.Pankratops, CardId.SouleatingOviraptor);
            return true;
        }

        private bool ArchosaurSummon()
        {
            // Summon Archosaur if we have Baby in hand or field to destroy
            return Bot.Hand.Any(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon) ||
                   Bot.GetMonsters().Any(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);
        }

        private bool ArchosaurActivate()
        {
            if (_archosaurUsedThisTurn || IsCurrentCardNegated()) return false;
            // Destroy Babycerasaurus or Petiteranodon in hand/field to search Double Evolution Pill
            ClientCard popTarget = Bot.GetMonsters().Concat(Bot.Hand).FirstOrDefault(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);
            if (popTarget == null && !Bot.HasInHand(CardId.DoubleEvolutionPill))
            {
                // Can pop another Dino on field if not Boss
                popTarget = Bot.GetMonsters().FirstOrDefault(c => c.HasRace(CardRace.Dinosaur) &&
                    c.Id != CardId.UltimateConductorTyranno &&
                    c.Id != CardId.EvolzarLars &&
                    c.Id != CardId.EvolzarDolkka &&
                    c.Id != CardId.EvolzarLaggia);
            }
            if (popTarget != null)
            {
                _archosaurUsedThisTurn = true;
                AI.SelectCard(popTarget);
                return true;
            }
            return false;
        }

        private bool XenoMeteorusActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // SS Frostosaurus (Level 6) to overlay into Rank 6 Evolzar Lars!
            AI.SelectCard(CardId.Frostosaurus, CardId.MegalosmasherX);
            return true;
        }

        private bool DoubleEvolutionPillActivate()
        {
            if (_pillUsedThisTurn || IsCurrentCardNegated()) return false;

            // Check Banish Cost: Must have 1 Dinosaur and 1 Non-Dinosaur in Hand/GY!
            var allDinos = Bot.Hand.Concat(Bot.Graveyard).Where(c => c.HasRace(CardRace.Dinosaur)).ToList();
            var allNonDinos = Bot.Hand.Concat(Bot.Graveyard).Where(c => !c.HasRace(CardRace.Dinosaur) && c.IsMonster()).ToList();

            if (allDinos.Count >= 1 && allNonDinos.Count >= 1)
            {
                _pillUsedThisTurn = true;
                AI.SelectCard(CardId.UltimateConductorTyranno, CardId.Pankratops);
                return true;
            }

            return false;
        }

        private bool UCTSpSummon()
        {
            // Banish 2 Dinosaurs from GY to summon 3500 ATK UCT
            return Bot.Graveyard.Count(c => c.HasRace(CardRace.Dinosaur)) >= 2;
        }

        private bool UCTActivate()
        {
            if (_uctEclipseUsedThisTurn || IsCurrentCardNegated()) return false;

            // ═══ ANTI-SELF-HARM AUDIT: BOOK OF ECLIPSE VALIDITY CHECK ═══
            // Only activate if opponent controls at least 1 face-up monster that CAN be flipped to Defense!
            // (Link Monsters have no Defense and CANNOT be flipped; already face-down or Defense monsters are unaffected)
            bool oppHasFlippableMonster = Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.HasType(CardType.Link) && m.IsAttack());
            if (!oppHasFlippableMonster) return false;

            // Pop Target Evaluation:
            // STRICT RULE: ONLY pop Babycerasaurus/Petiteranodon (which float into new monsters),
            // or an Archosaur whose effect has already resolved, or a Token!
            // NEVER pop Evolzar Lars, Dolkka, Laggia, or our only Oviraptor!
            var popCandidates = Bot.Hand.Concat(Bot.GetMonsters()).ToList();
            ClientCard popTarget = Plugin.MaterialImpl.PickUCTPopTarget(popCandidates);

            if (popTarget != null)
            {
                _uctEclipseUsedThisTurn = true;
                AI.SelectCard(popTarget);
                return true;
            }

            return false;
        }

        private bool EvolzarLarsSpSummon()
        {
            int lv6 = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 6);
            return lv6 >= 2;
        }

        private bool EvolzarLarsActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Quick effect: Detach 2 to negate any face-up card effect on the field (twice per turn!)
            return true;
        }

        private bool EvolzarDolkkaSpSummon()
        {
            int lv4Dinos = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.Dinosaur));
            return lv4Dinos >= 2;
        }

        private bool EvolzarDolkkaActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Quick effect: Detach 1 to negate monster effect activation and destroy it!
            return true;
        }

        private bool EvolzarLaggiaSpSummon()
        {
            int lv4Dinos = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.Dinosaur));
            return lv4Dinos >= 2;
        }

        private bool EvolzarLaggiaActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Quick effect: Detach 2 for omni-negate (Summon, Spell, Trap)
            return true;
        }

        private bool PankratopsActivate()
        {
            // Tribute self to destroy 1 card opponent controls (Prioritize highest threat like Eternal Soul, Skill Drain!)
            bool dragonKnightOnField = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == 41721210);
            var targets = Enemy.GetMonsters().Where(m => m != null && !IsTargetImmune(m)).AsEnumerable();
            if (!dragonKnightOnField)
                targets = targets.Concat(Enemy.GetSpells().Where(s => s != null && !IsTargetImmune(s)));

            var oppTarget = targets.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                   .ThenByDescending(c => c.Attack)
                                   .FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool DarkRulerNoMoreActivate()
        {
            if (IsCurrentCardNegated()) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && (CardIntelligence.IsKnownNegator(m.Id) || m.Attack >= 2500 || Plugin.ThreatImpl.EvaluateThreatScore(m) >= 40));
        }

        private bool DogoranSpSummon()
        {
            var oppMons = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (oppMons.Count == 0) return false;

            // Prioritize highest threat: Dragon Knight (lockdown), Crystal Wing, ABC Buster, 2000+ ATK
            var highThreat = oppMons.OrderByDescending(m => Plugin.ThreatImpl.EvaluateThreatScore(m))
                                    .ThenByDescending(m => m.Attack)
                                    .FirstOrDefault();
            if (highThreat != null && (Plugin.ThreatImpl.EvaluateThreatScore(highThreat) >= 40 || highThreat.Attack >= 2000 || oppMons.Count == 1))
            {
                AI.SelectCard(highThreat);
                return true;
            }
            return false;
        }

        private bool GroundXenoActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Activate Ground Xeno if we have at least 1 card in hand to pop (Baby/Petite is best)
            return Bot.Hand.Count >= 2;
        }

        private bool AbyssDwellerSpSummon()
        {
            int lv4 = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4);
            return lv4 >= 2 && (Util.IsTurn1OrMain2() || Enemy.Graveyard.Count >= 2);
        }

        private bool AbyssDwellerActivate()
        {
            if (IsCurrentCardNegated()) return false;
            // Quick effect: Detach 1 to prevent opponent from activating GY effects this turn!
            return Duel.Player == 1 || Enemy.Graveyard.Any(c => c.IsMonster());
        }

        private bool BagooskaSpSummon()
        {
            int lv4 = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4);
            return lv4 >= 2 && Util.IsTurn1OrMain2() && !Bot.HasInMonstersZone(CardId.UltimateConductorTyranno);
        }

        private bool LinkuribohSpSummon()
        {
            // Convert Level 1 Archosaur to Linkuriboh to put Non-Dino in GY for Pill!
            return Bot.GetMonsters().Any(m => m.Id == CardId.AnimadornedArchosaur) &&
                   !Bot.Graveyard.Any(c => !c.HasRace(CardRace.Dinosaur) && c.IsMonster());
        }

        private bool SecureGardnaSpSummon()
        {
            // Link climb Linkuriboh into Secure Gardna to guarantee Non-Dino in GY for Pill!
            return Bot.GetMonsters().Any(m => m.Id == CardId.Linkuriboh);
        }

        private bool DugaresActivate()
        {
            // Option 0: Draw 2, discard 1
            AI.SelectOption(0);
            return true;
        }

        private bool AccesscodeTalkerSpSummon()
        {
            int linkRating = Bot.GetMonsters().Where(c => c.IsFaceup()).Sum(c => c.HasType(CardType.Link) ? c.LinkMarker : 1);
            return linkRating >= 4 && (Duel.Turn >= 2 || Enemy.GetMonsterCount() > 0);
        }

        private bool SPLittleKnightActivate()
        {
            var oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                         ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool SpellSetStrategy()
        {
            if (Card == null) return false;

            // STACK-AWARE GUARD: Never set duplicate Spells/Traps if one is already set/active in SpellZone
            // Setting duplicate Ojama Trio or Survival's End wastes valuable backrow slots!
            if (Bot.SpellZone.Any(c => c != null && c.Id == Card.Id))
                return false;

            // STACK-AWARE GUARD: Preserve at least 1-2 free Spell/Trap zones for Field Spells & Normal Spells
            if (Bot.GetSpellCount() >= 4)
                return false;

            // Ojama Trio requires 3 empty monster zones to activate: do not set if opp already has 3+ monsters
            if (Card.Id == CardId.OjamaTrio && Enemy.GetMonsterCount() > 2)
                return false;

            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay)) return Duel.Phase == DuelPhase.Main2;
            return false;
        }

        public bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            if (Card.HasType(CardType.Link)) return false;

            if (Card.IsAttack() && (Card.Attack == 0 || CardIntelligence.IsHandtrap(Card.Id)))
                return true;

            if (Card.IsAttack() && Card.Defense > Card.Attack && Card.Attack < 1800)
            {
                if (Duel.Phase == DuelPhase.Main1 && ShouldRushAttack) return false;
                return true;
            }

            if (Card.IsDefense() && Card.Attack >= 1800 && Card.Attack >= Card.Defense)
            {
                if (!Util.IsAllEnemyBetter(true)) return true;
            }

            return DefaultMonsterRepos();
        }

        #region Multi-Dimensional Token & Battle Phase Attack Tactics

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker == null || defenders == null || defenders.Count == 0)
                return base.OnSelectAttackTarget(attacker, defenders);

            // ═══════════════════════════════════════════════════════════════════════
            // DIMENSION 1: ULTIMATE CONDUCTOR TYRANNO (UCT) BATTLE TACTICS
            // ═══════════════════════════════════════════════════════════════════════
            // UCT can attack ALL monsters once each.
            // At start of Damage Step vs Defense Position monster: 1000 burn damage + sends to GY!
            // UCT SHOULD attack all tokens and defense monsters to deal 1000 burn per token!
            if (attacker.Id == CardId.UltimateConductorTyranno)
            {
                // Prefer defense monsters / tokens for guaranteed 1000 burn without destruction float
                var defTarget = defenders.FirstOrDefault(d => d.IsDefense());
                if (defTarget != null)
                {
                    return AI.Attack(attacker, defTarget);
                }
                var atkTarget = defenders.OrderBy(d => d.Attack).FirstOrDefault();
                if (atkTarget != null && attacker.Attack > atkTarget.Attack)
                {
                    return AI.Attack(attacker, atkTarget);
                }
            }

            // ═══════════════════════════════════════════════════════════════════════
            // DIMENSION 2: NON-UCT MONSTERS VS OPPONENT'S TOKENS
            // ═══════════════════════════════════════════════════════════════════════
            // "ตีไปจะเป็นการเปิดช่องให้เค้าลงไหม ?" (Does attacking free a zone for opponent?)
            // "ตีแล้วได้อะไร" (What do we gain from attacking it?)
            // 1. Jurraegg Token under Lost World:
            //    - As long as Jurraegg Token is on opponent's field, opponent CANNOT target our monsters!
            //    - If Lost World substitution is AVAILABLE: Attacking the token lets us destroy Baby from Deck
            //      to summon a Dinosaur, while the Token SURVIVES (Target-Lock remains active!).
            //    - If Lost World substitution was ALREADY USED: Attacking the token DESTROYS IT,
            //      FREEING A ZONE for opponent and REMOVING OUR TARGET-LOCK SHIELD!
            // 2. Ojama Tokens:
            //    - Clogs 3 opponent zones. Attacking with small monsters merely frees up zones!
            bool hasLostWorld = Bot.HasInSpellZone(CardId.LostWorld);
            bool hasBabyInDeck = Bot.Deck.Any(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);

            // Separate Real Monsters from Tokens
            var realMonsters = defenders.Where(d => !d.HasType(CardType.Token) && d.Id != CardId.JurraeggToken && d.Id != CardId.OjamaToken).ToList();
            var tokens = defenders.Where(d => d.HasType(CardType.Token) || d.Id == CardId.JurraeggToken || d.Id == CardId.OjamaToken).ToList();

            // Priority: Attack real enemy monsters first!
            if (realMonsters.Count > 0)
            {
                var killable = realMonsters.Where(d => (d.IsAttack() && attacker.Attack > d.Attack) || (d.IsDefense() && attacker.Attack > d.Defense))
                                           .OrderByDescending(d => Plugin.ThreatImpl.EvaluateThreatScore(d))
                                           .FirstOrDefault();
                if (killable != null)
                {
                    return AI.Attack(attacker, killable);
                }
            }

            // If only Tokens remain:
            if (tokens.Count > 0)
            {
                // Check if we have confirmed Lethal on board this turn (Kill now, don't worry about future zones)
                int totalBotAttack = Bot.GetMonsters().Where(m => m.IsAttack()).Sum(m => m.Attack);
                bool hasLethalOnBoard = totalBotAttack >= Enemy.LifePoints;

                // Case A: Lost World substitution is READY and we have Baby in Deck!
                // Attacking Jurraegg Token triggers Lost World to pop Baby from Deck -> SS new Dino,
                // AND the token survives on opponent's field!
                if (hasLostWorld && !_lostWorldSubUsedThisTurn && hasBabyInDeck)
                {
                    var jurraegg = tokens.FirstOrDefault(t => t.Id == CardId.JurraeggToken || t.HasRace(CardRace.Dinosaur));
                    if (jurraegg != null)
                    {
                        _lostWorldSubUsedThisTurn = true;
                        return AI.Attack(attacker, jurraegg);
                    }
                }

                // Case B: Lethal Confirmed -> Destroy tokens to clear field and push for game!
                if (hasLethalOnBoard)
                {
                    return AI.Attack(attacker, tokens.First());
                }

                // Case C: Non-lethal and Lost World substitution already used:
                // DO NOT attack the tokens! Keep opponent's zones clogged and keep Target-Lock active!
                return null;
            }

            return base.OnSelectAttackTarget(attacker, defenders);
        }

        #endregion

        #region Card Selection & Trigger Handlers

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Lost World Destruction Substitute (Hint 502 / Deck Pop)
            // STRICT RULE: Only pop Babycerasaurus or Petiteranodon from DECK!
            if (hint == 502 && cards.All(c => c.Location == CardLocation.Deck || c.Location == CardLocation.Hand))
            {
                var babySub = Plugin.MaterialImpl.PickDestructionSubstitute(cards, min);
                if (babySub != null && cards.Contains(babySub))
                {
                    return new List<ClientCard> { babySub };
                }
            }

            // 2. Double Evolution Pill Banish Selection (1 Dino + 1 Non-Dino)
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Id == CardId.DoubleEvolutionPill && cards.Any(c => c.Location == CardLocation.Grave || c.Location == CardLocation.Hand))
            {
                var pillMaterials = Plugin.MaterialImpl.PickDoubleEvolutionPillBanish(cards);
                if (pillMaterials.Count >= min)
                {
                    return pillMaterials.Take(max).ToList();
                }
            }

            // 2.3 Kaiju / Dogoran Tribute Target (Hint 500 / Release Opponent Monster)
            if (hint == 500 && cards.Any(c => c.Controller == 1))
            {
                var oppTarget = cards.Where(c => c.Controller == 1)
                                     .OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                     .ThenByDescending(c => c.Attack)
                                     .FirstOrDefault();
                if (oppTarget != null) return new List<ClientCard> { oppTarget };
            }

            // 2.5 Ground Xeno Hand Pop Target
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Id == CardId.GroundXeno && cards.All(c => c.Location == CardLocation.Hand))
            {
                var baby = cards.FirstOrDefault(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);
                if (baby != null) return new List<ClientCard> { baby };
                var rex = cards.FirstOrDefault(c => c.Id == CardId.GiantRex);
                if (rex != null) return new List<ClientCard> { rex };
                var arch = cards.FirstOrDefault(c => c.Id == CardId.AnimadornedArchosaur);
                if (arch != null) return new List<ClientCard> { arch };
                var misc = cards.FirstOrDefault(c => c.Id == CardId.Miscellaneousaurus);
                if (misc != null) return new List<ClientCard> { misc };
                var normal = cards.FirstOrDefault(c => c.Id == CardId.Frostosaurus || c.Id == CardId.MegalosmasherX);
                if (normal != null) return new List<ClientCard> { normal };
            }

            // 3. Removal / Banish / Destroy (Hints: 502, 503, 505) -> Target Opponent Only!
            if (hint == 502 || hint == 503 || hint == 505)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    return oppCards.OrderByDescending(c => Plugin.ThreatImpl.EvaluateThreatScore(c))
                                   .ThenByDescending(c => c.Attack)
                                   .Take(Math.Min(max, oppCards.Count))
                                   .ToList();
                }

                // If hint 502 (Destroy) on our own cards (e.g. Xeno Meteorus, Oviraptor pop):
                // STRICTLY use PickDestructionSubstitute to pop Babycerasaurus, Petiteranodon, or JurraeggToken!
                if (hint == 502)
                {
                    var popTarget = Plugin.MaterialImpl.PickDestructionSubstitute(cards, min);
                    if (popTarget != null && cards.Contains(popTarget))
                    {
                        var result = new List<ClientCard> { popTarget };
                        result.AddRange(cards.Where(c => c != popTarget).Take(max - 1));
                        if (result.Count >= min) return result.Take(max).ToList();
                    }
                }
            }

            // 4. Discard Target (Hint 501 or Discard Prompt) -> Prioritize Misc / Giant Rex
            if (hint == 501 || cards.All(c => c.Location == CardLocation.Hand))
            {
                var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null && cards.Contains(discardTarget))
                {
                    var result = new List<ClientCard> { discardTarget };
                    result.AddRange(cards.Where(c => c != discardTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            // 5. Special Summon Target (Hint 509)
            if (hint == 509)
            {
                var ssTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (ssTarget != null && cards.Contains(ssTarget))
                {
                    var result = new List<ClientCard> { ssTarget };
                    result.AddRange(cards.Where(c => c != ssTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            // 6. Search Target (Hint 506)
            if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
            {
                var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (searchTarget != null && cards.Contains(searchTarget))
                {
                    var result = new List<ClientCard> { searchTarget };
                    result.AddRange(cards.Where(c => c != searchTarget).Take(max - 1));
                    if (result.Count >= min) return result.Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // Number 41: Bagooska MUST ALWAYS be in Defense position (Rule 12)
            if (cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            // Attack Bosses: UCT, Evolzar Lars, Dolkka, Laggia, Pankratops, Accesscode
            int[] attackers = {
                CardId.UltimateConductorTyranno,
                CardId.EvolzarLars,
                CardId.EvolzarDolkka,
                CardId.EvolzarLaggia,
                CardId.Pankratops,
                CardId.AccesscodeTalker,
                CardId.SouleatingOviraptor,
                CardId.Frostosaurus,
                CardId.MegalosmasherX,
                CardId.GiantRex
            };
            if (attackers.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;

            // Walls & Floaters: Babycerasaurus, Petiteranodon, Archosaur, Handtraps
            if (cardId == CardId.Babycerasaurus || cardId == CardId.Petiteranodon ||
                cardId == CardId.AnimadornedArchosaur || cardId == CardId.AshBlossom)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;

            // Reject opponent effects by default
            if (card.Controller == 1) return false;

            // Always accept Lost World Jurraegg token generation & protection substitution!
            if (card.Id == CardId.LostWorld) return true;

            // Always accept Babycerasaurus & Petiteranodon float triggers!
            if (card.Id == CardId.Babycerasaurus || card.Id == CardId.Petiteranodon) return true;

            // Always accept Giant Rex revival on banish
            if (card.Id == CardId.GiantRex) return true;

            // Always accept Xeno Meteorus special summon on destruction
            if (card.Id == CardId.XenoMeteorus) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override bool OnSelectHand() => true;

        #endregion
    }
}
