using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: LabrynthPlugin
    //  Mastery of Normal Traps, Archetype Resource Engine & Dynamic Disruption Sequencing
    // ═══════════════════════════════════════════════════════════════════════════════════
    public class LabrynthPlugin : DeckPluginBase
    {
        public LabrynthExecutor Exec { get; }

        public override string DeckName => "Labrynth";

        public LabrynthStrategy StrategyImpl { get; }
        public LabrynthMaterialEvaluator MaterialImpl { get; }
        public LabrynthThreatEvaluator ThreatImpl { get; }
        public LabrynthTrapManager TrapManager { get; }
        public LabrynthBoardAssessor BoardAssessor { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public LabrynthPlugin(LabrynthExecutor exec)
        {
            Exec = exec;
            StrategyImpl = new LabrynthStrategy(exec);
            MaterialImpl = new LabrynthMaterialEvaluator(exec);
            ThreatImpl = new LabrynthThreatEvaluator(exec);
            TrapManager = new LabrynthTrapManager(exec);
            BoardAssessor = new LabrynthBoardAssessor(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class LabrynthStrategy : IDeckStrategy
    {
        private readonly LabrynthExecutor _exec;

        public LabrynthStrategy(LabrynthExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Search priority (Arianna / Pot of Duality / etc.):
            // 1. Big Welcome Labrynth (Core engine starter & recursion)
            if (!_exec.Bot.HasInHandOrInSpellZone(LabrynthExecutor.CardId.BigWelcomeLabrynth))
            {
                var bigWelcome = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth);
                if (bigWelcome != null) return bigWelcome;
            }

            // 2. Welcome Labrynth (Backup engine)
            if (!_exec.Bot.HasInHandOrInSpellZone(LabrynthExecutor.CardId.WelcomeLabrynth) &&
                _exec.Bot.HasInHandOrInSpellZone(LabrynthExecutor.CardId.BigWelcomeLabrynth))
            {
                var welcome = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
                if (welcome != null) return welcome;
            }

            // 3. Arianna the Labrynth Servant (Normal Summon searcher if not used)
            if (!_exec.AriannaUsed && !_exec.Bot.HasInHand(LabrynthExecutor.CardId.AriannaTheLabrynthServant))
            {
                var arianna = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant);
                if (arianna != null) return arianna;
            }

            // 4. Lady Labrynth of the Silver Castle (3000 ATK Boss & Trap Searcher)
            if (!_exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle) &&
                !_exec.Bot.HasInHand(LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle))
            {
                var lady = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle);
                if (lady != null) return lady;
            }

            // 5. Lovely Labrynth of the Silver Castle (2900 ATK Pop & Recycle Boss)
            if (!_exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle) &&
                !_exec.Bot.HasInHand(LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle))
            {
                var lovely = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle);
                if (lovely != null) return lovely;
            }

            // 6. Labrynth Cooclock (Instant Trap resolution enable)
            if (!_exec.Bot.HasInHand(LabrynthExecutor.CardId.LabrynthCooclock) &&
                _exec.Bot.GetSpells().Any(s => s != null && s.IsFacedown() && _exec.IsNormalTrap(s)))
            {
                var cooclock = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthCooclock);
                if (cooclock != null) return cooclock;
            }

            // 7. Labrynth Stovie Torbie / Chandraglier (Furniture engine to set traps)
            var stovie = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthStovieTorbie);
            if (stovie != null && !_exec.Bot.HasInHand(LabrynthExecutor.CardId.LabrynthStovieTorbie)) return stovie;

            var chandra = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthChandraglier);
            if (chandra != null && !_exec.Bot.HasInHand(LabrynthExecutor.CardId.LabrynthChandraglier)) return chandra;

            // 8. Arias the Labrynth Butler
            var arias = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriasTheLabrynthButler);
            if (arias != null) return arias;

            // 9. Destructive Daruma Karma Cannon / IDP / Punishment
            var karma = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon);
            if (karma != null) return karma;

            var punishment = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DogmatikaPunishment);
            if (punishment != null) return punishment;

            return candidates[0];
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            bool isBigWelcome = (_exec.CurrentExecutingCard != null && _exec.CurrentExecutingCard.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth)
                || (_exec.CurrentLastChainCard != null && _exec.CurrentLastChainCard.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth)
                || (_exec.Duel.CurrentChain != null && _exec.Duel.CurrentChain.Any(c => c != null && c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth && c.Location != CardLocation.Grave));
            bool hasFodderOnField = _exec.Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !_exec.IsAceCard(c));

            // Big Welcome with NO fodder monsters on field:
            // Big Welcome mandates returning 1 monster we control to hand AFTER summoning.
            // If we have no fodder (e.g. only Lady on field, or empty field), summoning Lovely/Lady will force
            // us to bounce that boss monster right back to hand!
            // Therefore, summon Arianna (to get search trigger) or Cooclock (to enable traps this turn!) or Arias/Stovie:
            if (isBigWelcome && !hasFodderOnField)
            {
                // If we haven't searched with Arianna this turn, summon Arianna -> trigger search -> bounce Arianna to hand!
                if (!_exec.AriannaUsed)
                {
                    var arianna = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant);
                    if (arianna != null) return arianna;
                }

                // If we have Set Traps that need immediate activation, summon Cooclock -> bounce Cooclock -> discard Cooclock!
                var cooclock = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthCooclock);
                if (cooclock != null) return cooclock;

                var ariannaFallback = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant);
                if (ariannaFallback != null) return ariannaFallback;

                var arias = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriasTheLabrynthButler);
                if (arias != null) return arias;

                var stovie = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthStovieTorbie);
                if (stovie != null) return stovie;

                var chandra = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthChandraglier);
                if (chandra != null) return chandra;
            }

            // Normal Big Welcome (we already control fodder to bounce) OR Welcome Labrynth:
            // 1. Lovely Labrynth of the Silver Castle (2900 ATK, Pop + Recycle, floodgates monster response)
            if (!_exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle))
            {
                var lovely = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle);
                if (lovely != null) return lovely;
            }

            // 2. Lady Labrynth of the Silver Castle (3000 ATK, Untargetable, Set Trap from Deck)
            if (!_exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle))
            {
                var lady = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle);
                if (lady != null) return lady;
            }

            // 3. Arianna the Labrynth Servant (Searcher)
            var ariannaExt = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant);
            if (ariannaExt != null) return ariannaExt;

            // 4. Arias the Labrynth Butler
            var ariasExt = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriasTheLabrynthButler);
            if (ariasExt != null) return ariasExt;

            return candidates[0];
        }

        public ClientCard PickTrapToSetFromDeck(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 0. Special Furniture Case: Candidates are ONLY Labrynth Spell/Traps (Big Welcome & Welcome)
            bool isFurnitureSet = candidates.All(c => c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth || c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
            if (isFurnitureSet)
            {
                // If Bot has no monsters on field and no monsters in hand, Big Welcome will bounce the summoned monster!
                // Welcome Labrynth brings out Lovely directly to the field without bouncing, enabling Eradicator, Punishment, Lovely protection!
                bool canKeepBossOnField = _exec.Bot.GetMonsterCount() > 0 || _exec.Bot.Hand.Any(c => c.IsMonster() && c.Level <= 4);
                if (!canKeepBossOnField && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.WelcomeLabrynth))
                {
                    var welcomeFirst = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
                    if (welcomeFirst != null) return welcomeFirst;
                }

                // If already have Big Welcome, set Welcome
                if (_exec.Bot.HasInHandOrInSpellZone(LabrynthExecutor.CardId.BigWelcomeLabrynth))
                {
                    var welcomeSecond = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
                    if (welcomeSecond != null) return welcomeSecond;
                }

                // Otherwise prioritize Big Welcome
                var bigWelcomeFirst = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth);
                if (bigWelcomeFirst != null) return bigWelcomeFirst;

                var welcomeFallback = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
                if (welcomeFallback != null) return welcomeFallback;
            }

            int oppMonsterCount = _exec.Enemy.GetMonsterCount();
            var oppCards = _exec.Enemy.GetMonsters().Concat(_exec.Enemy.Graveyard).Concat(_exec.Enemy.GetSpells()).ToList();
            bool isAltergeist = oppCards.Any(c => c.Id == 25533642 || c.Id == 53143898 || c.Id == 89538537 || c.Id == 42790071 || c.Id == 1508649 || c.Id == 27541563);
            bool isDarkMagician = oppCards.Any(c => c.Id == 46986414 || c.Id == 47222536 || c.Id == 48680970 || c.Id == 7084129 || c.Id == 1784686 || c.Id == 30603688 || c.Id == 7922915 || c.Id == 41721210);
            bool isABC = oppCards.Any(c => c.Id == 1561110 || c.Id == 66399653 || c.Id == 77411244 || c.Id == 30012506 || c.Id == 3405259 || c.Id == 99249638 || c.Id == 12524259);
            bool isBlueEyes = oppCards.Any(c => c.Id == 89631139 || c.Id == 8240199 || c.Id == 71039903 || c.Id == 79814787 || c.Id == 38517737 || c.Id == 41620959 || c.Id == 39701395 || c.Id == 48800175 || c.Id == 6853254);
            bool isTrapDeck = isAltergeist || oppCards.Count(c => c.IsTrap()) >= 2;
            bool isSpellDeck = isDarkMagician || oppCards.Count(c => c.IsSpell()) >= 2;
            bool oppHasEDNonLink = isDarkMagician || isABC || isBlueEyes || oppCards.Any(c => c.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Ritual | CardType.Pendulum));

            // Lady Labrynth Set Trap Priority based on Board State & Matchup:
            // 1. Virus Auto-Win (Eradicator Epidemic Virus when we control Lovely or Lady)
            if ((isTrapDeck || isSpellDeck) && (_exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle) ||
                                                _exec.Bot.HasInMonstersZone(LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle)))
            {
                var virus = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.EradicatorEpidemicVirus);
                if (virus != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.EradicatorEpidemicVirus)) return virus;
            }

            // 2. Anti-Dragon / BlueEyes Counter:
            // Blue-Eyes swarms 3000 ATK dragons from hand and GY (Alternative, Return of the Dragon Lords, Silver's Cry)
            // Daruma Karma Cannon flips them all face-down (ruining attacks, bypasses Return destruction protection)!
            // Ice Dragon's Prison banishes 2 dragons non-targeting (bypassing Return destruction protection)!
            if (isBlueEyes)
            {
                if (!_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon))
                {
                    var daruma = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon);
                    if (daruma != null) return daruma;
                }

                if (_exec.Enemy.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Dragon)) &&
                    !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.IceDragonsPrison))
                {
                    var idp = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.IceDragonsPrison);
                    if (idp != null) return idp;
                }
            }

            // 2.5 Anti-ABC Counter: D-Barrier on Fusion prevents ABC-Dragon Buster summon and shuts it down!
            if (isABC && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DimensionalBarrier))
            {
                var dbarrier = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DimensionalBarrier);
                if (dbarrier != null) return dbarrier;
            }

            // 3. Link / Swarm Counter: Destructive Daruma Karma Cannon (Sends Link monsters to GY without targeting/destroying!)
            if ((oppMonsterCount >= 2 || isAltergeist || oppCards.Any(c => c.HasType(CardType.Link))) &&
                !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon))
            {
                var daruma = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon);
                if (daruma != null) return daruma;
            }

            // 4. Matchup Counter: Dimensional Barrier (Instant shutdown vs Fusion/Synchro/Xyz)
            if (oppHasEDNonLink && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DimensionalBarrier))
            {
                var dbarrier = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DimensionalBarrier);
                if (dbarrier != null) return dbarrier;
            }

            // 4. Core Engine Extender: Big Welcome Labrynth (If not already set or in hand)
            if (!_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.BigWelcomeLabrynth) &&
                !_exec.Bot.HasInHand(LabrynthExecutor.CardId.BigWelcomeLabrynth))
            {
                var bigWelcome = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth);
                if (bigWelcome != null) return bigWelcome;
            }

            // 5. Universal Turn 1 / Blind Disruption: Destructive Daruma Karma Cannon
            if (!_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon))
            {
                var daruma = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon);
                if (daruma != null) return daruma;
            }

            // 6. Universal Extra Deck Lock: Dimensional Barrier
            if (!_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DimensionalBarrier))
            {
                var dbarrier = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DimensionalBarrier);
                if (dbarrier != null) return dbarrier;
            }

            // 7. Target Destruction / Extra Mill: Dogmatika Punishment
            if (oppMonsterCount >= 1 && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DogmatikaPunishment))
            {
                var punishment = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DogmatikaPunishment);
                if (punishment != null) return punishment;
            }

            // 8. Graveyard / Field Non-Target Banish: Ice Dragon's Prison
            if (_exec.Enemy.Graveyard.Any(c => c.IsMonster()) &&
                !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.IceDragonsPrison))
            {
                var idp = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.IceDragonsPrison);
                if (idp != null) return idp;
            }

            // 9. Single-target Bounce: Compulsory Evacuation Device
            var ced = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.CompulsoryEvacuationDevice);
            if (ced != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.CompulsoryEvacuationDevice)) return ced;

            // 10. Non-Destructive Spin: Terrors of the Overroot (Only if opponent has cards in GY!)
            if (_exec.Enemy.Graveyard.Count > 0)
            {
                var overroot = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.TerrorsOfTheOverroot);
                if (overroot != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.TerrorsOfTheOverroot)) return overroot;
            }

            // Fallback: Welcome Labrynth
            var welcome = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.WelcomeLabrynth);
            if (welcome != null) return welcome;

            return candidates[0];
        }

        public ClientCard PickTrapToRecycleFromGY(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Lovely Labrynth GY Set:
            // 1. Dimensional Barrier (Instant win vs Extra Deck)
            var dbarrier = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DimensionalBarrier);
            if (dbarrier != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DimensionalBarrier)) return dbarrier;

            // 2. Destructive Daruma Karma Cannon
            var daruma = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon);
            if (daruma != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon)) return daruma;

            // 3. Dogmatika Punishment
            var punishment = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.DogmatikaPunishment);
            if (punishment != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.DogmatikaPunishment)) return punishment;

            // 4. Ice Dragon's Prison
            var idp = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.IceDragonsPrison);
            if (idp != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.IceDragonsPrison)) return idp;

            // 5. Big Welcome Labrynth
            var bigWelcome = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.BigWelcomeLabrynth);
            if (bigWelcome != null && !_exec.Bot.HasInSpellZone(LabrynthExecutor.CardId.BigWelcomeLabrynth)) return bigWelcome;

            // 6. Terrors of the Overroot
            var overroot = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.TerrorsOfTheOverroot);
            if (overroot != null) return overroot;

            return candidates.FirstOrDefault(c => c != null && _exec.IsNormalTrap(c)) ?? candidates[0];
        }

        public ClientCard PickBounceSelfTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Big Welcome return to hand priority (return utility cards to replay / reuse):
            // 1. Arianna (Normal summon search again next turn!)
            var arianna = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant);
            if (arianna != null) return arianna;

            // 2. Cooclock (Replay hand effect!)
            var cooclock = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthCooclock);
            if (cooclock != null) return cooclock;

            // 3. Furniture (Stovie / Chandraglier to reuse hand discard)
            var stovie = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthStovieTorbie);
            if (stovie != null) return stovie;

            var chandra = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LabrynthChandraglier);
            if (chandra != null) return chandra;

            // 4. Arias
            var arias = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.AriasTheLabrynthButler);
            if (arias != null) return arias;

            // Never bounce Lovely or Lady unless absolutely no choice
            var nonAce = candidates.FirstOrDefault(c => !_exec.IsAceCard(c));
            if (nonAce != null) return nonAce;

            // If forced to choose between Lovely and Lady:
            // 1. Bounce disabled monster first
            var disabled = candidates.FirstOrDefault(c => c.IsDisabled());
            if (disabled != null) return disabled;

            // 2. Keep Lady if we control Set traps (Untargetable 3000 ATK tower!)
            bool hasSetTraps = _exec.Bot.GetSpells().Any(s => s != null && s.IsFacedown());
            if (hasSetTraps)
            {
                var lovely = candidates.FirstOrDefault(c => c.Id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle);
                if (lovely != null) return lovely;
            }

            return candidates.OrderBy(c => c.Attack).FirstOrDefault() ?? candidates[0];
        }
    }

    public class LabrynthMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly LabrynthExecutor _exec;

        public LabrynthMaterialEvaluator(LabrynthExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;
            // Lady and Lovely are core aces; never sacrifice or Link climb away unless lethal
            if (card.Id == LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle) return 50000;
            if (card.Id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle) return 50000;
            if (card.Id == LabrynthExecutor.CardId.ChaosAngel) return 40000;
            if (card.Id == LabrynthExecutor.CardId.SuperStarslayerTYPHON) return 30000;
            if (card.Id == LabrynthExecutor.CardId.AriannaTheLabrynthServant) return 2000;
            if (card.Id == LabrynthExecutor.CardId.AriasTheLabrynthButler) return 1000;
            if (card.Id == LabrynthExecutor.CardId.LabrynthStovieTorbie) return 100;
            if (card.Id == LabrynthExecutor.CardId.LabrynthChandraglier) return 100;
            if (card.Id == LabrynthExecutor.CardId.LabrynthCooclock) return 50;
            return 200;
        }

        public int GetDiscardCost(ClientCard card)
        {
            if (card == null) return 999999;
            int id = card.Id;

            // 1. Transaction Rollback (Thrives in Graveyard, top discard priority)
            if (id == LabrynthExecutor.CardId.TransactionRollback) return -10000;

            // 2. Cooclock (GY trigger to return to hand or SS)
            if (id == LabrynthExecutor.CardId.LabrynthCooclock) return -5000;

            // 3. Duplicate Furniture (Stovie / Chandraglier if we have > 1 in hand)
            if ((id == LabrynthExecutor.CardId.LabrynthStovieTorbie || id == LabrynthExecutor.CardId.LabrynthChandraglier) &&
                _exec.Bot.Hand.Count(c => c.Id == id) > 1) return -2000;

            // 4. Stovie / Chandraglier (single copy when discarding to each other)
            if (id == LabrynthExecutor.CardId.LabrynthStovieTorbie || id == LabrynthExecutor.CardId.LabrynthChandraglier) return -1000;

            // 5. Duplicate Traps in hand
            if (_exec.IsNormalTrap(card) && _exec.Bot.Hand.Count(h => h.Id == id) > 1) return 500;

            // 6. Lower impact single-target removal
            if (id == LabrynthExecutor.CardId.TerrorsOfTheOverroot) return 2000;
            if (id == LabrynthExecutor.CardId.CompulsoryEvacuationDevice) return 2500;

            // 7. Core Traps with GY utility
            if (id == LabrynthExecutor.CardId.BigWelcomeLabrynth) return 4000;
            if (id == LabrynthExecutor.CardId.WelcomeLabrynth) return 5000;

            // 8. Reactive Removal Traps
            if (id == LabrynthExecutor.CardId.IceDragonsPrison) return 6000;
            if (id == LabrynthExecutor.CardId.DogmatikaPunishment) return 7000;

            // 9. Key Utility Monsters
            if (id == LabrynthExecutor.CardId.AriasTheLabrynthButler) return 10000;
            // 🔒 CRITICAL: Protect Arianna if we can still Normal Summon her this turn!
            if (id == LabrynthExecutor.CardId.AriannaTheLabrynthServant)
            {
                if (!_exec.AriannaUsed && _exec.Duel.Player == 0) return 40000; // MUST Normal Summon!
                return 12000;
            }

            // 10. Handtraps (preserve for disruption)
            if (id == LabrynthExecutor.CardId.InfiniteImpermanence) return 14000;
            if (id == LabrynthExecutor.CardId.AshBlossom) return 16000;

            // 11. High-Impact Floodgates & Turn-Enders (NEVER casually discard!)
            if (id == LabrynthExecutor.CardId.DestructiveDarumaKarmaCannon) return 25000;
            if (id == LabrynthExecutor.CardId.DimensionalBarrier) return 30000;
            if (id == LabrynthExecutor.CardId.EradicatorEpidemicVirus) return 30000;
            if (id == LabrynthExecutor.CardId.TheBlackGoatLaughs) return 20000;

            // 12. Aces (Lady / Lovely)
            if (id == LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle || id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle) return 50000;

            return 1000;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetDiscardCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            // Never pop Lovely or Lady
            var fodder = candidates.FirstOrDefault(c => !_exec.IsAceCard(c));
            if (fodder != null) return fodder;
            return candidates[0];
        }
    }

    public class LabrynthThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly LabrynthExecutor _exec;

        public LabrynthThreatEvaluator(LabrynthExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int id = card.Id;
            // Extreme Backrow Wipe Threat
            if (id == 18144506 || id == 43898403 || id == 57728570 || id == 23002292) return 15000;

            // Active Negators & Floodgates
            if (card.IsMonster() && card.IsFaceup() && !card.IsDisabled())
            {
                if (CardIntelligence.GetCardThreatScore(card) >= 9000) return 9000;
                if (card.Attack >= 3000) return 8000;
                if (card.IsExtraCard()) return 6000;
                return 4000;
            }

            if (card.IsSpell() || card.IsTrap())
            {
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)) return 5000;
                return 2000;
            }

            return 1000;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            int id = card.Id;
            return id == 18144506 || id == 43898403 || id == 57728570 || id == 23002292 || (card.IsMonster() && card.Attack >= 3000);
        }
    }

    public class LabrynthTrapManager
    {
        private readonly LabrynthExecutor _exec;

        public LabrynthTrapManager(LabrynthExecutor exec) => _exec = exec;

        public bool HasActiveDisruption()
        {
            return _exec.Bot.GetSpells().Any(s => s != null && s.IsFacedown() && _exec.IsNormalTrap(s));
        }

        public int CountNormalTrapsInDeck()
        {
            return _exec.Bot.Deck.Count(c => c != null && _exec.IsNormalTrap(c));
        }
    }

    public class LabrynthBoardAssessor
    {
        private readonly LabrynthExecutor _exec;

        public LabrynthBoardAssessor(LabrynthExecutor exec) => _exec = exec;

        public bool HasLovelyProtection()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle);
        }

        public bool HasLadyActive()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle);
        }

        public bool IsLabrynthDominating()
        {
            bool hasBoss = HasLovelyProtection() || HasLadyActive();
            int setTraps = _exec.Bot.GetSpells().Count(s => s != null && s.IsFacedown());
            return hasBoss && setTraps >= 2;
        }
    }
}
