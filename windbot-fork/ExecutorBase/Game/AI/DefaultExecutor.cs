using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    public abstract class DefaultExecutor : Executor
    {
        protected class _CardId
        {
            public const int JizukirutheStarDestroyingKaiju = 63941210;
            public const int ThunderKingtheLightningstrikeKaiju = 48770333;
            public const int DogorantheMadFlameKaiju = 93332803;
            public const int RadiantheMultidimensionalKaiju = 28674152;
            public const int GadarlatheMysteryDustKaiju = 36956512;
            public const int KumongoustheStickyStringKaiju = 29726552;
            public const int GamecieltheSeaTurtleKaiju = 55063751;
            public const int SuperAntiKaijuWarMachineMechaDogoran = 84769941;

            public const int UltimateConductorTytanno = 18940556;
            public const int ElShaddollConstruct = 20366274;
            public const int AllyOfJusticeCatastor = 26593852;

            public const int DupeFrog = 46239604;
            public const int MaraudingCaptain = 2460565;

            public const int BlackRoseDragon = 73580471;
            public const int JudgmentDragon = 57774843;
            public const int TopologicTrisbaena = 72529749;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int HarpiesFeatherDuster = 18144506;
            public const int DarkMagicAttack = 2314238;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int CosmicCyclone = 8267140;
            public const int ChickenGame = 67616300;

            public const int SantaClaws = 46565218;

            public const int CastelTheSkyblasterMusketeer = 82633039;
            public const int CrystalWingSynchroDragon = 50954680;
            public const int NumberS39UtopiaTheLightning = 56832966;
            public const int Number39Utopia = 84013237;
            public const int UltimayaTzolkin = 1686814;
            public const int MekkKnightCrusadiaAstram = 21887175;
            public const int HamonLordofStrikingThunder = 32491822;

            public const int MoonMirrorShield = 19508728;
            public const int MirrorBarrier = 95915457;
            public const int PhantomKnightsFogBlade = 25542642;

            public const int VampireFraeulein = 6039967;
            public const int InjectionFairyLily = 79575620;

            public const int BlueEyesChaosMAXDragon = 55410871;

            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int LockBird = 94145021;
            public const int GhostOgreAndSnowRabbit = 59438930;
            public const int GhostBelle = 73642296;
            public const int EffectVeiler = 97268402;
            public const int ArtifactLancea = 34267821;
            public const int MulcharmyPurulia = 84192580;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyNyalus = 87126721;

            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681982;
            public const int InfiniteImpermanence = 10045474;
            public const int SolemnJudgment = 41420027;
            public const int SolemnWarning = 84749824;
            public const int SolemnStrike = 40605147;
            public const int SolemnReport = 78114463;
            public const int GalaxySoldier = 46659709;
            public const int MacroCosmos = 30241314;
            public const int UpstartGoblin = 70368879;
            public const int CyberEmergency = 60600126;
            public const int MistakenArrest = 4149689;

            public const int ThunderKingRaiOh = 71564252;
            public const int ThunderDragonColossus = 15291624;
            public const int DeckLockdown = 34507039;
            public const int Mistake = 10833828;
            public const int DoomZDestruction = 28546905;
            public const int Number41BagooskatheTerriblyTiredTapir = 90590303;
            public const int Number41BagooskatheTerriblyTiredTapirAlt = 90590304;
            public const int LightningStorm = 14532163;
            public const int DiabellzeOfTheOriginalSin = 43262273;
            public const int PotOfExtravagance = 49238328;

            public const int EaterOfMillions = 63845230;

            public const int InvokedPurgatrio = 12307878;
            public const int ChaosAncientGearGiant = 51788412;
            public const int UltimateAncientGearGolem = 12652643;

            public const int RedDragonArchfiend = 70902743;

            public const int ImperialOrder = 61740673;
            public const int NaturiaBeast = 33198837;
            public const int AntiSpellFragrance = 58921041;
        }

        int HonestEffectCount = 0;
        protected Dictionary<int, int> calledbytheGraveIdCountMap = new Dictionary<int, int>();
        protected List<int> crossoutDesignatorIdList = new List<int>();
        protected List<int> resolvedEffectIdList = new List<int>();
        protected List<int> enemyResolvedEffectIdList = new List<int>();
        protected List<int> infiniteImpermanenceNegatedColumns = new List<int>();
        protected int mistakenArrestAffectedCount = 0;
        protected int lightningStormOption = -1;

        protected DefaultExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            AddExecutor(ExecutorType.Activate, _CardId.ChickenGame, DefaultChickenGame);
            AddExecutor(ExecutorType.Activate, _CardId.SantaClaws);
        }

        /// <summary>
        /// Decide which card should the attacker attack.
        /// </summary>
        /// <param name="attacker">Card that attack.</param>
        /// <param name="defenders">Cards that defend.</param>
        /// <returns>BattlePhaseAction including the target, or null (in this situation, GameAI will check the next attacker)</returns>
        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            // Prioritize defenders: Threat/Negators first, then highest attackable monsters
            var sortedDefenders = defenders
                .Where(d => d != null)
                .OrderByDescending(d => {
                    int score = 0;
                    if (CardIntelligence.IsKnownNegator(d.Id)) score += 10000;
                    if (CardIntelligence.IsFloodgateMonster(d.Id)) score += 9000;
                    if (d.IsExtraCard()) score += 5000;
                    score += d.Attack;
                    return score;
                })
                .ToList();

            // 1. Lethal Direct Attack Priority: If attacker can attack directly and its ATK deals lethal damage to enemy LP, GO FOR GAME!
            if (attacker.CanDirectAttack && attacker.Attack >= Enemy.LifePoints)
            {
                return AI.Attack(attacker, null);
            }

            // 2. Direct Attacker Priority (e.g. Sky Striker Hayate triggers search on direct attack, Toons under Toon Kingdom)
            // If enemy has no urgent floodgates or negators, prioritize direct attack!
            if (attacker.CanDirectAttack && (attacker.Id == 8491308 || attacker.Id == 25862681 || attacker.HasSetcode(0x62)))
            {
                if (!sortedDefenders.Any(d => CardIntelligence.IsFloodgateMonster(d.Id) || CardIntelligence.IsKnownNegator(d.Id)))
                {
                    return AI.Attack(attacker, null);
                }
            }

            foreach (ClientCard defender in sortedDefenders)
            {
                attacker.RealPower = attacker.Attack;
                defender.RealPower = defender.GetDefensePower();

                if (defender.RealPower < 0)
                {
                    defender.RealPower = 3000; // Assume 3000 ATK/DEF for safety if ? or unknown
                }

                if (!OnPreBattleBetween(attacker, defender))
                    continue;

                if (attacker.RealPower > defender.RealPower || (attacker.RealPower >= defender.RealPower && attacker.IsLastAttacker && defender.IsAttack()))
                    return AI.Attack(attacker, defender);
            }

            if (attacker.CanDirectAttack)
                return AI.Attack(attacker, null);

            return null;
        }

        /// <summary>
        /// Decide whether to declare attack between attacker and defender.
        /// Can be overrided to update the RealPower of attacker for cards like Honest.
        /// </summary>
        /// <param name="attacker">Card that attack.</param>
        /// <param name="defender">Card that defend.</param>
        /// <returns>false if the attack shouldn't be done.</returns>
        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker.RealPower <= 0)
                return false;

            // Universal Safeguard: Never attack dangerous reflection/damage monsters
            if (CardIntelligence.IsDangerousBattleTarget(defender, attacker))
                return false;

            if (!attacker.IsMonsterHasPreventActivationEffectInBattle())
            {
                if (defender.IsMonsterInvincible() && defender.IsDefense())
                    return false;

                if (defender.IsMonsterDangerous())
                {
                    bool canIgnoreIt = !attacker.IsDisabled() && (
                        attacker.IsCode(_CardId.UltimateConductorTytanno) && defender.IsDefense() ||
                        attacker.IsCode(_CardId.ElShaddollConstruct) && defender.IsSpecialSummoned ||
                        attacker.IsCode(_CardId.AllyOfJusticeCatastor) && !defender.HasAttribute(CardAttribute.Dark));
                    if (!canIgnoreIt)
                        return false;
                }

                foreach (ClientCard equip in defender.EquipCards)
                {
                    if (equip.IsCode(new[] { _CardId.MoonMirrorShield, _CardId.MirrorBarrier }) && !equip.IsDisabled())
                    {
                        return false;
                    }
                }

                if (!defender.IsDisabled())
                {
                    if (defender.IsCode(_CardId.MekkKnightCrusadiaAstram) && defender.IsAttack() && attacker.IsSpecialSummoned)
                        return false;

                    if (defender.IsCode(_CardId.CrystalWingSynchroDragon) && defender.IsAttack() && attacker.Level >= 5)
                        return false;

                    if (defender.IsCode(_CardId.AllyOfJusticeCatastor) && !attacker.HasAttribute(CardAttribute.Dark))
                        return false;

                    if (defender.IsCode(_CardId.NumberS39UtopiaTheLightning) && defender.IsAttack() && defender.HasXyzMaterial(2, _CardId.Number39Utopia))
                        defender.RealPower = 5000;

                    if (defender.IsCode(_CardId.VampireFraeulein))
                        defender.RealPower += (Enemy.LifePoints > 3000) ? 3000 : (Enemy.LifePoints - 100);

                    if (defender.IsCode(_CardId.InjectionFairyLily) && Enemy.LifePoints > 2000)
                        defender.RealPower += 3000;
                }
            }

            if (!defender.IsMonsterHasPreventActivationEffectInBattle())
            {
                if (attacker.IsCode(_CardId.NumberS39UtopiaTheLightning) && !attacker.IsDisabled() && attacker.HasXyzMaterial(2, _CardId.Number39Utopia))
                    attacker.RealPower = 5000;

                foreach (ClientCard equip in attacker.EquipCards)
                {
                    if (equip.IsCode(_CardId.MoonMirrorShield) && !equip.IsDisabled())
                    {
                        attacker.RealPower = defender.RealPower + 100;
                    }
                }
            }

            if (Enemy.HasInMonstersZone(_CardId.MekkKnightCrusadiaAstram, true) && !(defender).IsCode(_CardId.MekkKnightCrusadiaAstram))
                return false;

            if (Enemy.HasInMonstersZone(_CardId.DupeFrog, true) && !(defender).IsCode(_CardId.DupeFrog))
                return false;

            if (Enemy.HasInMonstersZone(_CardId.MaraudingCaptain, true) && !defender.IsCode(_CardId.MaraudingCaptain) && defender.Race == (int)CardRace.Warrior)
                return false;

            if (defender.IsCode(_CardId.UltimayaTzolkin) && !defender.IsDisabled() && Enemy.GetMonsters().Any(monster => !monster.Equals(defender) && monster.HasType(CardType.Synchro)))
                return false;

            if (Enemy.GetMonsters().Any(monster => !monster.Equals(defender) && monster.IsCode(_CardId.HamonLordofStrikingThunder) && !monster.IsDisabled() && monster.IsDefense()))
                return false;

            if (defender.OwnTargets.Any(card => card.IsCode(_CardId.PhantomKnightsFogBlade) && !card.IsDisabled()))
                return false;

            return true;
        }

        /// <summary>
        /// Called when the AI has to select a card position.
        /// </summary>
        /// <param name="cardId">Id of the card to position on the field.</param>
        /// <param name="positions">List of available positions.</param>
        /// <returns>Selected position, or 0 if no position is set for this card.</returns>
        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            YGOSharp.OCGWrapper.NamedCard cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.Attack == 0 && positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override bool OnSelectBattleReplay()
        {
            if (Bot.BattlingMonster == null)
                return false;
            List<ClientCard> defenders = new List<ClientCard>(Duel.Fields[1].GetMonsters());
            defenders.Sort(CardContainer.CompareDefensePower);
            defenders.Reverse();
            BattlePhaseAction result = OnSelectAttackTarget(Bot.BattlingMonster, defenders);
            if (result != null && result.Action == BattlePhaseAction.BattleAction.Attack)
            {
                return true;
            }
            return false;
        }

        public override void OnHintZone(int player, int zone)
        {
            base.OnHintZone(player, zone);
            ChainInfo currentChainInfo = Duel.GetCurrentSolvingChainInfo();
            if (currentChainInfo != null)
            {
                if (currentChainInfo.IsActivateCode(_CardId.InfiniteImpermanence))
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        if ((zone & (0x100 << i)) == 0)
                            continue;
                        if (currentChainInfo.ActivatePlayer == 0)
                            infiniteImpermanenceNegatedColumns.Add(i);
                        else
                            infiniteImpermanenceNegatedColumns.Add(4 - i);
                    }
                }
            }
        }

        public override void OnChainSolved(int chainIndex)
        {
            ChainInfo currentChain = Duel.GetCurrentSolvingChainInfo();
            if (currentChain != null && !Duel.IsCurrentSolvingChainNegated())
            {
                if (currentChain.ActivatePlayer == 0)
                {
                    if (currentChain.ActivateId > 0)
                        resolvedEffectIdList.Add(currentChain.ActivateId);
                    if (currentChain.ActivateAlias > 0)
                        resolvedEffectIdList.Add(currentChain.ActivateAlias);
                    if (currentChain.RelatedCard != null)
                        resolvedEffectIdList.Add(currentChain.RelatedCard.GetNonAltartCode());
                }
                if (currentChain.IsActivateCode(_CardId.LockBird))
                {
                    resolvedEffectIdList.Add(_CardId.LockBird);
                }
                if (currentChain.ActivatePlayer == 1)
                {
                    if (currentChain.IsActivateCode(_CardId.MaxxC))
                        enemyResolvedEffectIdList.Add(_CardId.MaxxC);
                    if (currentChain.IsActivateCode(_CardId.MulcharmyPurulia))
                        enemyResolvedEffectIdList.Add(_CardId.MulcharmyPurulia);
                    if (currentChain.IsActivateCode(_CardId.MulcharmyFuwalos))
                        enemyResolvedEffectIdList.Add(_CardId.MulcharmyFuwalos);
                    if (currentChain.IsActivateCode(_CardId.MulcharmyNyalus))
                        enemyResolvedEffectIdList.Add(_CardId.MulcharmyNyalus);
                    if (currentChain.IsActivateCode(_CardId.MistakenArrest))
                    {
                        if (Duel.Player == 1)
                            mistakenArrestAffectedCount = Math.Max(mistakenArrestAffectedCount, 3);
                        else
                            mistakenArrestAffectedCount = Math.Max(mistakenArrestAffectedCount, 2);
                    }
                }
            }
            base.OnChainSolved(chainIndex);
        }

        public override void OnNewTurn()
        {
            HonestEffectCount = 0;
            infiniteImpermanenceNegatedColumns.Clear();
            resolvedEffectIdList.Clear();
            enemyResolvedEffectIdList.Clear();
            if (Duel.Turn <= 1)
            {
                calledbytheGraveIdCountMap.Clear();
                mistakenArrestAffectedCount = 0;
            }
            mistakenArrestAffectedCount = Math.Max(mistakenArrestAffectedCount - 1, 0);
            List<int> keyList = calledbytheGraveIdCountMap.Keys.ToList();
            foreach (int dic in keyList)
            {
                if (calledbytheGraveIdCountMap[dic] > 0)
                {
                    calledbytheGraveIdCountMap[dic] -= 1;
                }
            }
            crossoutDesignatorIdList.Clear();
            base.OnNewTurn();
        }

        public override void OnNewPhase()
        {
        }

        public override void OnChaining(int player, ClientCard card)
        {
        }

        public override void OnChainEnd()
        {
            lightningStormOption = -1;
            Brain?.OnChainEnd();
            base.OnChainEnd();
        }

        public override void OnMove(ClientCard card, int previousControler, int previousLocation, int currentControler, int currentLocation)
        {
            if (card != null)
            {
                ChainInfo currentSolvingChain = Duel.GetCurrentSolvingChainInfo();
                if (currentSolvingChain != null && currentLocation == (int)CardLocation.Removed)
                {
                    int originId = card.Id;
                    if (card.Data != null)
                    {
                        if (card.Data.Alias > 0) originId = card.Data.Alias;
                        else originId = card.Id;
                    }
                    if (currentSolvingChain.IsActivateCode(_CardId.CalledByTheGrave))
                    {
                        calledbytheGraveIdCountMap[originId] = 2;
                    }
                    if (currentSolvingChain.IsActivateCode(_CardId.CrossoutDesignator))
                    {
                        crossoutDesignatorIdList.Add(originId);
                    }
                }
            }
            base.OnMove(card, previousControler, previousLocation, currentControler, currentLocation);
        }

        /// <summary>
        /// Destroy face-down cards first, in our turn.
        /// </summary>
        protected bool DefaultMysticalSpaceTyphoon()
        {
            if (Duel.CurrentChain.Any(card => card.IsCode(_CardId.MysticalSpaceTyphoon)))
            {
                return false;
            }

            List<ClientCard> spells = Enemy.GetSpells();
            if (spells.Count == 0)
                return false;

            ClientCard selected = Enemy.SpellZone.GetFloodgate();

            if (selected == null)
            {
                if (Duel.Player == 0)
                    selected = spells.FirstOrDefault(card => card.IsFacedown());
                if (Duel.Player == 1)
                    selected = spells.FirstOrDefault(card => card.HasType(CardType.Continuous) || card.HasType(CardType.Equip) || card.HasType(CardType.Field));
            }

            if (selected == null)
                return false;
            AI.SelectCard(selected);
            return true;
        }

        /// <summary>
        /// Destroy face-down cards first, in our turn.
        /// </summary>
        protected bool DefaultCosmicCyclone()
        {
            foreach (ClientCard card in Duel.CurrentChain)
                if (card.IsCode(_CardId.CosmicCyclone))
                    return false;
            return (Bot.LifePoints > 1000) && DefaultMysticalSpaceTyphoon();
        }

        /// <summary>
        /// Activate if avail.
        /// </summary>
        protected bool DefaultGalaxyCyclone()
        {
            List<ClientCard> spells = Enemy.GetSpells();
            if (spells.Count == 0)
                return false;

            ClientCard selected = null;

            if (Card.Location == CardLocation.Grave)
            {
                selected = Util.GetBestEnemySpell(true);
            }
            else
            {
                selected = spells.FirstOrDefault(card => card.IsFacedown());
            }

            if (selected == null)
                return false;

            AI.SelectCard(selected);
            return true;
        }

        /// <summary>
        /// Set the highest ATK level 4+ effect enemy monster.
        /// </summary>
        protected bool DefaultBookOfMoon()
        {
            if (Util.IsAllEnemyBetter(true))
            {
                ClientCard monster = Enemy.GetMonsters().GetHighestAttackMonster(true);
                if (monster != null && monster.HasType(CardType.Effect) && !monster.HasType(CardType.Link) && (monster.HasType(CardType.Xyz) || monster.Level > 4))
                {
                    AI.SelectCard(monster);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return problematic monster, and if this card become target, return any enemy monster.
        /// </summary>
        protected bool DefaultCompulsoryEvacuationDevice()
        {
            ClientCard target = Util.GetProblematicEnemyMonster(0, true);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            if (Util.IsChainTarget(Card))
            {
                ClientCard monster = Util.GetBestEnemyMonster(false, true);
                if (monster != null)
                {
                    AI.SelectCard(monster);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Revive the best monster when we don't have better one in field.
        /// </summary>
        protected bool DefaultCallOfTheHaunted()
        {
            if (!Util.IsAllEnemyBetter(true))
                return false;
            ClientCard selected = Bot.Graveyard.GetMatchingCards(card => card.IsCanRevive()).OrderByDescending(card => card.Attack).FirstOrDefault();
            AI.SelectCard(selected);
            return true;
        }

        /// <summary>
        /// Default Scapegoat effect
        /// </summary>
        protected bool DefaultScapegoat()
        {
            if (DefaultSpellWillBeNegated()) return false;
            if (Duel.Player == 0) return false;
            if (Duel.Phase == DuelPhase.End) return true;
            if (DefaultOnBecomeTarget()) return true;
            if (Duel.Phase > DuelPhase.Main1 && Duel.Phase < DuelPhase.Main2)
            {
                if (Enemy.HasInMonstersZone(new[]
                {
                    _CardId.UltimateConductorTytanno,
                    _CardId.InvokedPurgatrio,
                    _CardId.ChaosAncientGearGiant,
                    _CardId.UltimateAncientGearGolem,
                    _CardId.RedDragonArchfiend
                }, true)) return false;
                if (Util.GetTotalAttackingMonsterAttack(1) >= Bot.LifePoints) return true;
            }
            return false;
        }
        /// <summary>
        /// Always active in opponent's turn.
        /// </summary>
        protected bool DefaultMaxxC()
        {
            if (Duel.Player != 1) return false;
            // Prevent activating Maxx "C" if Droll & Lock Bird is active or Maxx "C" was already resolved by us this turn
            if (resolvedEffectIdList.Contains(_CardId.MaxxC) || resolvedEffectIdList.Contains(_CardId.LockBird))
                return false;
            // Prevent chaining duplicate Maxx "C" in the same chain
            if (Util.ChainContainsCard(_CardId.MaxxC))
                return false;
            return true;
        }
        /// <summary>
        /// Standard Droll & Lock Bird response
        /// </summary>
        protected bool DefaultDrollAndLockBird()
        {
            if (Duel.Player == 0) return false;
            if (resolvedEffectIdList.Contains(_CardId.LockBird)) return false;
            if (Util.ChainContainsCard(_CardId.LockBird)) return false;
            return Duel.LastChainPlayer == 1;
        }
        /// <summary>
        /// Always disable opponent's effect except some cards like UpstartGoblin
        /// </summary>
        protected bool DefaultAshBlossomAndJoyousSpring()
        {
            int[] ignoreList = {
                _CardId.MacroCosmos,
                _CardId.UpstartGoblin,
                _CardId.CyberEmergency
            };
            if (Util.GetLastChainCard().IsCode(ignoreList))
                return false;
            if (Util.GetLastChainCard().HasSetcode(0x11e) && Util.GetLastChainCard().Location == CardLocation.Hand) // Danger! archtype hand effect
                return false;
            return Duel.LastChainPlayer == 1;
        }
        /// <summary>
        /// Always activate unless the activating card is disabled
        /// </summary>
        protected bool DefaultGhostOgreAndSnowRabbit()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsDisabled())
                return false;
            return DefaultTrap();
        }
        /// <summary>
        /// Always disable opponent's effect
        /// </summary>
        protected bool DefaultGhostBelleAndHauntedMansion()
        {
            return DefaultTrap();
        }
        /// <summary>
        /// Same as DefaultBreakthroughSkill
        /// </summary>
        protected bool DefaultEffectVeiler()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsCode(_CardId.GalaxySoldier) && Enemy.Hand.Count >= 3) return false;
            if (Util.ChainContainsCard(_CardId.EffectVeiler))
                return false;
            return DefaultBreakthroughSkill();
        }
        /// <summary>
        /// Chain common hand traps and GY monsters
        /// </summary>
        protected bool DefaultCalledByTheGrave()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard == null) return false;

            int targetCode = lastCard.GetNonAltartCode();
            bool isHandtrapOrThreat = CardIntelligence.IsHandtrap(lastCard.Id) || CardIntelligence.IsHandtrap(targetCode)
                || CardIntelligence.IsKnownNegator(lastCard.Id) || CardIntelligence.IsHighThreatChokepoint(lastCard.Id);

            if (isHandtrapOrThreat)
            {
                if (Enemy.Graveyard.Any(gy => gy != null && (gy.IsCode(targetCode) || gy.IsCode(lastCard.Id))))
                {
                    AI.SelectCard(lastCard.Id);
                    return UniqueFaceupSpell();
                }
            }

            return false;
        }

        /// <summary>
        /// Default Crossout Designator effect:
        /// Declares the last chain card if it is in our remaining deck to negate it.
        /// </summary>
        protected bool DefaultCrossoutDesignator()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard == null) return false;

            int targetCode = lastCard.GetNonAltartCode();
            if (GetRemainingCount(targetCode) > 0)
            {
                AI.SelectAnnounceID(targetCode);
                return UniqueFaceupSpell();
            }

            return false;
        }
        /// <summary>
        /// Default InfiniteImpermanence effect
        /// </summary>
        protected bool DefaultInfiniteImpermanence()
        {
            // TODO: disable s & t
            if (!DefaultUniqueTrap())
                return false;
            return DefaultDisableMonster();
        }
        /// <summary>
        /// Chain the enemy monster, or disable monster like Rescue Rabbit.
        /// </summary>
        protected bool DefaultBreakthroughSkill()
        {
            if (!DefaultUniqueTrap())
                return false;
            return DefaultDisableMonster();
        }
        /// <summary>
        /// Chain the enemy monster, or disable monster like Rescue Rabbit.
        /// </summary>
        protected bool DefaultDisableMonster()
        {
            // Do not chain another negator if our chain already contains an active negation responding to this chain
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && (c.IsCode(10045474) || c.IsCode(97268402) || c.IsCode(24224830) || CardIntelligence.IsKnownNegator(c.Id))))
                return false;

            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetShouldBeDisabledBeforeItUseEffectMonster();
                if (target != null && !target.IsDisabled())
                {
                    bool canTarget = true;
                    if (Card != null)
                    {
                        if (Card.IsMonster() && target.IsShouldNotBeMonsterTarget())
                            canTarget = false;
                        else if ((Card.IsSpell() || Card.IsTrap()) && target.IsShouldNotBeSpellTrapTarget())
                            canTarget = false;
                    }
                    if (canTarget)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            ClientCard LastChainCard = Util.GetLastChainCard();

            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget())
            {
                bool canTarget = true;
                if (Card != null)
                {
                    if (Card.IsMonster() && LastChainCard.IsShouldNotBeMonsterTarget())
                        canTarget = false;
                    else if ((Card.IsSpell() || Card.IsTrap()) && LastChainCard.IsShouldNotBeSpellTrapTarget())
                        canTarget = false;
                }
                else
                {
                    if (LastChainCard.IsShouldNotBeSpellTrapTarget())
                        canTarget = false;
                }

                if (canTarget)
                {
                    AI.SelectCard(LastChainCard);
                    return true;
                }
            }

            if (Bot.BattlingMonster != null && Enemy.BattlingMonster != null)
            {
                if (!Enemy.BattlingMonster.IsDisabled() && Enemy.BattlingMonster.IsCode(_CardId.EaterOfMillions))
                {
                    AI.SelectCard(Enemy.BattlingMonster);
                    return true;
                }
            }

            if (Duel.Phase == DuelPhase.BattleStart && Duel.Player == 1 &&
                Enemy.HasInMonstersZone(_CardId.NumberS39UtopiaTheLightning, true))
            {
                AI.SelectCard(_CardId.NumberS39UtopiaTheLightning);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Activate Solemn Judgment to negate opponent's S/T activation or summon.
        /// Pays half LP. Prevents self-chain, duplicate chain in same chain, and paying LP redundantly.
        /// </summary>
        protected bool DefaultSolemnJudgment()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && (lastCard.Controller == 0 || lastCard.IsDisabled())) return false;
            return !Util.IsChainTargetOnly(Card) && !(Duel.Player == 0 && Duel.LastChainPlayer == -1) && DefaultTrap();
        }

        /// <summary>
        /// Activate Solemn Warning to negate opponent's summon or SS effect.
        /// Costs 2000 LP. Prevents self-chain, duplicate chain in same chain, and paying LP redundantly.
        /// </summary>
        protected bool DefaultSolemnWarning()
        {
            if (Bot.LifePoints <= 2000) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && (lastCard.Controller == 0 || lastCard.IsDisabled())) return false;
            return !(Duel.Player == 0 && Duel.LastChainPlayer == -1) && DefaultTrap();
        }

        /// <summary>
        /// Activate Solemn Strike to negate opponent's special summon or monster effect.
        /// Costs 1500 LP. Prevents self-chain, duplicate chain in same chain, and paying LP redundantly.
        /// </summary>
        protected bool DefaultSolemnStrike()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && (lastCard.Controller == 0 || lastCard.IsDisabled())) return false;
            return !(Duel.Player == 0 && Duel.LastChainPlayer == -1) && DefaultTrap();
        }

        /// <summary>
        /// Activate Solemn Report to negate opponent's Spell/Trap Card activation.
        /// Option 0: Pay 1500 LP to negate and lock name for the rest of turn.
        /// Prevents self-chain, duplicate chain in same chain, and paying LP redundantly.
        /// </summary>
        protected bool DefaultSolemnReport()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer != 1) return false;

            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller == 0 || lastCard.IsDisabled()) return false;
            if (!lastCard.HasType(CardType.Spell) && !lastCard.HasType(CardType.Trap)) return false;

            AI.SelectOption(0);
            return true;
        }

        /// <summary>
        /// Activate when all enemy monsters have better ATK.
        /// </summary>
        protected bool DefaultTorrentialTribute()
        {
            return !Util.HasChainedTrap(0) && Util.IsAllEnemyBetter(true);
        }

        /// <summary>
        /// Activate enemy have more S&T.
        /// </summary>
        protected bool DefaultHeavyStorm()
        {
            return Bot.GetSpellCount() < Enemy.GetSpellCount();
        }

        /// <summary>
        /// Activate before other winds, if enemy have more than 2 S&T.
        /// </summary>
        protected bool DefaultHarpiesFeatherDusterFirst()
        {
            return Enemy.GetSpellCount() >= 2;
        }

        /// <summary>
        /// Activate when one enemy monsters have better ATK.
        /// </summary>
        protected bool DefaultHammerShot()
        {
            return Util.IsOneEnemyBetter(true);
        }

        /// <summary>
        /// Activate when one enemy monsters have better ATK or DEF.
        /// </summary>
        protected bool DefaultDarkHole()
        {
            return Util.IsOneEnemyBetter();
        }

        /// <summary>
        /// Activate when one enemy monsters have better ATK or DEF.
        /// </summary>
        protected bool DefaultRaigeki()
        {
            return Util.IsOneEnemyBetter();
        }

        /// <summary>
        /// Activate when one enemy monsters have better ATK or DEF.
        /// </summary>
        protected bool DefaultSmashingGround()
        {
            return Util.IsOneEnemyBetter();
        }

        /// <summary>
        /// Activate when we have more than 15 cards in deck.
        /// </summary>
        protected bool DefaultPotOfDesires()
        {
            return Bot.Deck.Count > 15;
        }

        /// <summary>
        /// Set traps only and avoid block the activation of other cards.
        /// Protects handtraps from being set face-down in MP1.
        /// </summary>
        protected bool DefaultSpellSet()
        {
            if (Card == null) return false;

            // Universal Safeguard: Never set handtraps face-down (especially in MP1)
            if (CardIntelligence.IsHandtrap(Card.Id) || CardIntelligence.IsHandtrap(Card.GetNonAltartCode()))
                return false;

            // Cards with empty-field hand activation conditions must stay in hand
            if (Card.IsCode(10045474, 40366667, 6325660, 89264428, 23002292))
                return false;

            return (Card.IsTrap() || Card.HasType(CardType.QuickPlay)) && Bot.GetSpellCountWithoutField() < 4;
        }

        /// <summary>
        /// Summon with tributes ATK lower.
        /// </summary>
        protected bool DefaultTributeSummon()
        {
            if (!UniqueFaceupMonster())
                return false;
            int tributecount = (int)Math.Ceiling((Card.Level - 4.0d) / 2.0d);
            for (int j = 0; j < 7; ++j)
            {
                ClientCard tributeCard = Bot.MonsterZone[j];
                if (tributeCard == null) continue;
                if (tributeCard.GetDefensePower() < Card.Attack)
                    tributecount--;
            }
            return tributecount <= 0;
        }

        /// <summary>
        /// Activate when we have no field.
        /// </summary>
        protected bool DefaultField()
        {
            return Bot.SpellZone[5] == null;
        }

        /// <summary>
        /// Turn if all enemy is better.
        /// </summary>
        protected bool DefaultMonsterRepos()
        {
            if (Card.IsFaceup() && Card.IsDefense() && Card.Attack == 0)
                return false;

            if (Enemy.HasInMonstersZone(_CardId.BlueEyesChaosMAXDragon, true) &&
                Card.IsAttack() && (4000 - Card.Defense) * 2 > (4000 - Card.Attack))
                return false;
            if (Enemy.HasInMonstersZone(_CardId.BlueEyesChaosMAXDragon, true) &&
                Card.IsDefense() && Card.IsFaceup() &&
                (4000 - Card.Defense) * 2 > (4000 - Card.Attack))
                return true;

            bool enemyBetter = Util.IsAllEnemyBetter(true);
            if (Card.IsAttack() && enemyBetter)
                return true;
            if (Card.IsDefense() && !enemyBetter && Card.Attack >= Card.Defense)
                return true;

            return false;
        }

        /// <summary>
        /// If spell will be negated
        /// </summary>
        protected bool DefaultSpellWillBeNegated()
        {
            return Bot.HasInSpellZone(_CardId.ImperialOrder, true, true) || Enemy.HasInSpellZone(_CardId.ImperialOrder, true) || Enemy.HasInMonstersZone(_CardId.NaturiaBeast, true);
        }

        /// <summary>
        /// If spell must set first to activate
        /// </summary>
        protected bool DefaultSpellMustSetFirst()
        {
            ClientCard card = null;
            foreach (ClientCard check in Bot.GetSpells())
            {
                if (check.IsCode(_CardId.AntiSpellFragrance) && !check.IsDisabled())
                    card = check;
            }
            if (card != null && card.IsFaceup())
                return true;
            return Bot.HasInSpellZone(_CardId.AntiSpellFragrance, true, true) ||  Enemy.HasInSpellZone(_CardId.AntiSpellFragrance, true);
        }

        /// <summary>
        /// if spell/trap is the target or enermy activate HarpiesFeatherDuster
        /// </summary>
        protected bool DefaultOnBecomeTarget()
        {
            if (Util.IsChainTarget(Card)) return true;
            int[] destroyAllList =
            {
                _CardId.EvilswarmExcitonKnight,
                _CardId.BlackRoseDragon,
                _CardId.JudgmentDragon,
                _CardId.TopologicTrisbaena
            };
            int[] destroyAllOpponentList =
            {
                _CardId.HarpiesFeatherDuster,
                _CardId.DarkMagicAttack
            };

            if (Util.ChainContainsCard(destroyAllList)) return true;
            if (Enemy.HasInSpellZone(destroyAllOpponentList, true)) return true;
            // TODO: ChainContainsCard(id, player)
            return false;
        }
        /// <summary>
        /// Chain enemy activation or summon.
        /// </summary>
        protected bool DefaultTrap()
        {
            return (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer != 0) || Duel.LastChainPlayer == 1;
        }

        /// <summary>
        /// Activate when avail and no other our trap card in this chain or face-up.
        /// </summary>
        protected bool DefaultUniqueTrap()
        {
            if (Util.HasChainedTrap(0))
                return false;

            return UniqueFaceupSpell();
        }

        /// <summary>
        /// Check no other our spell or trap card with same name face-up.
        /// </summary>
        protected bool UniqueFaceupSpell()
        {
            return !Bot.GetSpells().Any(card => card.IsCode(Card.Id) && card.IsFaceup());
        }

        /// <summary>
        /// Check no other our monster card with same name face-up.
        /// </summary>
        protected bool UniqueFaceupMonster()
        {
            return !Bot.GetMonsters().Any(card => card.IsCode(Card.Id) && card.IsFaceup());
        }

        /// <summary>
        /// Avoid chaining in mess. Also prevents double-chaining when identical
        /// face-down cards are both triggered simultaneously (e.g. 2x Infinite Impermanence flipped at once).
        /// </summary>
        protected bool DefaultDontChainMyself()
        {
            if (Executors.Any(exec => exec.Type == Type && exec.CardId == Card.Id))
                return false;
            // BUG FIX: ตรวจว่า chain ปัจจุบันมีการ์ด ID เดียวกันอยู่แล้วหรือไม่
            // ป้องกัน double-chain เมื่อการ์ดใบเดียวกัน 2 ใบถูก activate พร้อมกัน
            if (Card != null && Util.ChainContainsCard(Card.Id))
                return false;
            return Duel.LastChainPlayer != 0;
        }

        /// <summary>
        /// Draw when we have lower LP, or destroy it. Can be overrided.
        /// </summary>
        protected bool DefaultChickenGame()
        {
            if (Executors.Count(exec => exec.Type == Type && exec.CardId == Card.Id) > 1)
                return false;
            if (Bot.LifePoints <= 1000)
                return false;
            if (Bot.LifePoints <= Enemy.LifePoints && ActivateDescription == Util.GetStringId(_CardId.ChickenGame, 0))
                return true;
            if (Bot.LifePoints > Enemy.LifePoints && ActivateDescription == Util.GetStringId(_CardId.ChickenGame, 1))
                return true;
            return false;
        }

        /// <summary>
        /// Draw when we have Dark monster in hand,and banish random one. Can be overrided.
        /// </summary>
        protected bool DefaultAllureofDarkness()
        {
            ClientCard target = Bot.Hand.FirstOrDefault(card => card.HasAttribute(CardAttribute.Dark));
            return target != null;
        }

        /// <summary>
        /// Clever enough.
        /// </summary>
        protected bool DefaultDimensionalBarrier()
        {
            const int RITUAL = 0;
            const int FUSION = 1;
            const int SYNCHRO = 2;
            const int XYZ = 3;
            const int PENDULUM = 4;
            if (Duel.Player != 0)
            {
                List<ClientCard> monsters = Enemy.GetMonsters();
                int[] levels = new int[13];
                bool tuner = false;
                bool nontuner = false;
                foreach (ClientCard monster in monsters)
                {
                    if (monster.HasType(CardType.Tuner))
                        tuner = true;
                    else if (!monster.HasType(CardType.Xyz) && !monster.HasType(CardType.Link))
                    {
                        nontuner = true;
                        levels[monster.Level] = levels[monster.Level] + 1;
                    }

                    if (monster.IsOneForXyz())
                    {
                        AI.SelectOption(XYZ);
                        return true;
                    }
                }
                if (tuner && nontuner)
                {
                    AI.SelectOption(SYNCHRO);
                    return true;
                }
                for (int i=1; i<=12; i++)
                {
                    if (levels[i]>1)
                    {
                        AI.SelectOption(XYZ);
                        return true;
                    }
                }
                ClientCard l = Enemy.SpellZone[6];
                ClientCard r = Enemy.SpellZone[7];
                if (l != null && r != null && l.LScale != r.RScale)
                {
                    AI.SelectOption(PENDULUM);
                    return true;
                }
            }
            ClientCard lastchaincard = Util.GetLastChainCard();
            if (Duel.LastChainPlayer == 1 && lastchaincard != null && !lastchaincard.IsDisabled())
            {
                if (lastchaincard.HasType(CardType.Ritual))
                {
                    AI.SelectOption(RITUAL);
                    return true;
                }
                if (lastchaincard.HasType(CardType.Fusion))
                {
                    AI.SelectOption(FUSION);
                    return true;
                }
                if (lastchaincard.HasType(CardType.Synchro))
                {
                    AI.SelectOption(SYNCHRO);
                    return true;
                }
                if (lastchaincard.HasType(CardType.Xyz))
                {
                    AI.SelectOption(XYZ);
                    return true;
                }
                if (lastchaincard.IsFusionSpell())
                {
                    AI.SelectOption(FUSION);
                    return true;
                }
            }
            if (Util.IsChainTarget(Card))
            {
                AI.SelectOption(XYZ);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clever enough
        /// </summary>
        protected bool DefaultInterruptedKaijuSlumber()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(
                    _CardId.GamecieltheSeaTurtleKaiju,
                    _CardId.KumongoustheStickyStringKaiju,
                    _CardId.GadarlatheMysteryDustKaiju,
                    _CardId.RadiantheMultidimensionalKaiju,
                    _CardId.DogorantheMadFlameKaiju,
                    _CardId.ThunderKingtheLightningstrikeKaiju,
                    _CardId.JizukirutheStarDestroyingKaiju
                    );
                return true;
            }

            if (DefaultDarkHole())
            {
                AI.SelectCard(
                    _CardId.JizukirutheStarDestroyingKaiju,
                    _CardId.ThunderKingtheLightningstrikeKaiju,
                    _CardId.DogorantheMadFlameKaiju,
                    _CardId.RadiantheMultidimensionalKaiju,
                    _CardId.GadarlatheMysteryDustKaiju,
                    _CardId.KumongoustheStickyStringKaiju,
                    _CardId.GamecieltheSeaTurtleKaiju
                    );
                AI.SelectNextCard(
                    _CardId.SuperAntiKaijuWarMachineMechaDogoran,
                    _CardId.GamecieltheSeaTurtleKaiju,
                    _CardId.KumongoustheStickyStringKaiju,
                    _CardId.GadarlatheMysteryDustKaiju,
                    _CardId.RadiantheMultidimensionalKaiju,
                    _CardId.DogorantheMadFlameKaiju,
                    _CardId.ThunderKingtheLightningstrikeKaiju
                    );
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clever enough.
        /// </summary>
        protected bool DefaultKaijuSpsummon()
        {
            IList<int> kaijus = new[] {
                _CardId.JizukirutheStarDestroyingKaiju,
                _CardId.GadarlatheMysteryDustKaiju,
                _CardId.GamecieltheSeaTurtleKaiju,
                _CardId.RadiantheMultidimensionalKaiju,
                _CardId.KumongoustheStickyStringKaiju,
                _CardId.ThunderKingtheLightningstrikeKaiju,
                _CardId.DogorantheMadFlameKaiju,
                _CardId.SuperAntiKaijuWarMachineMechaDogoran
            };
            foreach (ClientCard monster in Enemy.GetMonsters())
            {
                if (monster.IsCode(kaijus))
                    return Card.GetDefensePower() > monster.GetDefensePower();
            }

            ClientCard target = Enemy.MonsterZone.GetFloodgate();
            if (target == null)
            {
                target = Enemy.MonsterZone.GetDangerousMonster();
            }
            if (target == null)
            {
                target = Util.GetOneEnemyBetterThanValue(Card.GetDefensePower());
            }

            if (target != null)
            {
                // Safety Check: Avoid giving opponent a Kaiju we cannot handle.
                // We only do it if the target is a critical threat/negator, or we have a way to handle it.
                bool isCriticalThreat = target.IsCode(
                    42009023, 7902349, 15397015, 19261966, 78193831, 67922702, 96015934, // Floodgates
                    84815190, 4280258, 57793869, 86066372, 17330115, 63767246, 94977269, 33198886 // Negators/Bosses
                );

                int kaijuPower = Card.Attack;
                bool canHandleKaiju = false;

                // 1. Can we run it over by battle with our current field?
                if (Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsAttack() && !m.Attacked && m.Attack > kaijuPower))
                {
                    canHandleKaiju = true;
                }
                // 2. Can we summon a stronger monster from hand?
                else if (Bot.Hand.Any(c => c != null && c.IsMonster() && c.Attack > kaijuPower && (c.Level <= 4 || Bot.GetMonsterCount() >= 1)))
                {
                    canHandleKaiju = true;
                }
                // 3. Do we have removal options in hand or set on field?
                else if (Bot.Hand.Any(c => c != null && (c.Id == 12580477 || c.Id == 53129443 || c.Id == 24299458 || c.Id == 6430623 || c.Id == 37520316 || c.Id == 25311006)))
                {
                    canHandleKaiju = true;
                }
                // 4. Contact fusion material (Cyber Dragon + Machine Kaiju)
                else if (Card.Id == _CardId.JizukirutheStarDestroyingKaiju &&
                         Bot.ExtraDeck.Any(c => c != null && c.Id == 21060005) && // Chimeratech Fortress Dragon
                         (Bot.HasInHand(70095154) || Bot.HasInMonstersZone(70095154)))
                {
                    canHandleKaiju = true;
                }

                // CRITICAL SAFEGUARD: Avoid giving opponent a Kaiju we cannot handle.
                // If we give them a 3300 ATK Kaiju and cannot remove or beat over it,
                // the opponent will attack and destroy us with our own Kaiju!
                if (!canHandleKaiju)
                {
                    // Only acceptable if target is a hard floodgate AND Kaiju is low ATK (<= 2400)
                    // AND Bot has enough defense/LP to survive
                    if (isCriticalThreat && kaijuPower <= 2400 &&
                        (Bot.LifePoints > kaijuPower || Bot.GetMonsters().Any(m => m != null && m.IsDefense())))
                    {
                        AI.SelectCard(target);
                        return true;
                    }

                    return false;
                }

                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Intelligent Nibiru activation:
        /// 1. Opponent's turn only, Main Phase only.
        /// 2. Protects our own Ace/Boss monsters unless opponent has lethal ATK.
        /// 3. Ensures opponent has committed meaningful board presence.
        /// </summary>
        protected virtual bool DefaultNibiru()
        {
            // 1. Opponent's turn, Main Phase only
            if (Duel.Player != 1 || (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2))
                return false;

            // 2. Chain check: don't chain to our own cards
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 0)
                return false;

            // 3. Boss protection: NEVER wipe our own field if we control Ace/Boss monsters unless opponent has lethal!
            int ourBossCount = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && (IsAceCard(m) || m.Attack >= 2500));
            int enemyTotalAtk = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);

            if (ourBossCount > 0 && enemyTotalAtk < Bot.LifePoints && Enemy.GetMonsterCount() <= 2)
            {
                // Our boss board is winning, don't throw it away!
                return false;
            }

            // 4. Opponent must have committed at least 2 monsters, or total ATK >= 2500, or a critical threat
            if (Enemy.GetMonsterCount() >= 2 || enemyTotalAtk >= 2500 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && CardIntelligence.IsHighThreatChokepoint(m.Id)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Summon when we don't have monster attack higher than enemy's.
        /// </summary>
        protected bool DefaultNumberS39UtopiaTheLightningSummon()
        {
            int bestBotAttack = Util.GetBestAttack(Bot);
            return Util.IsOneEnemyBetterThanValue(bestBotAttack, false);
        }

        /// <summary>
        /// Activate if the card is attack pos, and its attack is below 5000, when the enemy monster is attack pos or not useless faceup defense pos
        /// </summary>
        protected bool DefaultNumberS39UtopiaTheLightningEffect()
        {
            return Card.IsAttack() && Card.Attack < 5000 && (Enemy.BattlingMonster.IsAttack() || Enemy.BattlingMonster.IsFacedown() || Enemy.BattlingMonster.GetDefensePower() >= Card.Attack);
        }

        /// <summary>
        /// Summon when it can and should use effect.
        /// </summary>
        protected bool DefaultEvilswarmExcitonKnightSummon()
        {
            int selfCount = Bot.GetMonsterCount() + Bot.GetSpellCount() + Bot.GetHandCount();
            int oppoCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount() + Enemy.GetHandCount();
            return (selfCount - 1 < oppoCount) && DefaultEvilswarmExcitonKnightEffect();
        }

        /// <summary>
        /// Activate when we have less cards than enemy's, or the atk sum of we is lower than enemy's.
        /// </summary>
        protected bool DefaultEvilswarmExcitonKnightEffect()
        {
            int selfCount = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int oppoCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();

            if (selfCount < oppoCount)
                return true;

            int selfAttack = Bot.GetMonsters().Sum(monster => (int?)monster.GetDefensePower()) ?? 0;
            int oppoAttack = Enemy.GetMonsters().Sum(monster => (int?)monster.GetDefensePower()) ?? 0;

            return selfAttack < oppoAttack;
        }

        /// <summary>
        /// Summon in main2, or when the attack of we is lower than enemy's, but not when enemy have monster higher than 2500.
        /// </summary>
        protected bool DefaultStardustDragonSummon()
        {
            int selfBestAttack = Util.GetBestAttack(Bot);
            int oppoBestAttack = Util.GetBestPower(Enemy);
            return (selfBestAttack <= oppoBestAttack && oppoBestAttack <= 2500) || Util.IsTurn1OrMain2();
        }

        /// <summary>
        /// Negate enemy's destroy effect, and revive from grave.
        /// </summary>
        protected bool DefaultStardustDragonEffect()
        {
            return (Card.Location == CardLocation.Grave) || Duel.LastChainPlayer == 1;
        }

        /// <summary>
        /// Summon when enemy have card which we must solve.
        /// </summary>
        protected bool DefaultCastelTheSkyblasterMusketeerSummon()
        {
            return Util.GetProblematicEnemyCard() != null;
        }

        /// <summary>
        /// Bounce the problematic enemy card. Ignore the 1st effect.
        /// </summary>
        protected bool DefaultCastelTheSkyblasterMusketeerEffect()
        {
            if (ActivateDescription == Util.GetStringId(_CardId.CastelTheSkyblasterMusketeer, 0))
                return false;
            ClientCard target = Util.GetProblematicEnemyCard();
            if (target != null)
            {
                AI.SelectCard(0);
                AI.SelectNextCard(target);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Summon when it should use effect, or when the attack of we is lower than enemy's, but not when enemy have monster higher than 3000.
        /// </summary>
        protected bool DefaultScarlightRedDragonArchfiendSummon()
        {
            int selfBestAttack = Util.GetBestAttack(Bot);
            int oppoBestAttack = Util.GetBestPower(Enemy);
            return (selfBestAttack <= oppoBestAttack && oppoBestAttack <= 3000) || DefaultScarlightRedDragonArchfiendEffect();
        }

        /// <summary>
        /// Activate when we have less monsters than enemy, or when enemy have more than 3 monsters.
        /// </summary>
        protected bool DefaultScarlightRedDragonArchfiendEffect()
        {
            int selfCount = Bot.GetMonsters().Count(monster => !monster.Equals(Card) && monster.IsSpecialSummoned && monster.HasType(CardType.Effect) && monster.Attack <= Card.Attack);
            int oppoCount = Enemy.GetMonsters().Count(monster => monster.IsSpecialSummoned && monster.HasType(CardType.Effect) && monster.Attack <= Card.Attack);
            return selfCount <= oppoCount && oppoCount > 0 || oppoCount >= 3;
        }

        /// <summary>
        /// Clever enough.
        /// </summary>
        protected bool DefaultHonestEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.BattlingMonster.IsAttack() &&
                    (((Bot.BattlingMonster.Attack < Enemy.BattlingMonster.Attack) || Bot.BattlingMonster.Attack >= Enemy.LifePoints)
                    || ((Bot.BattlingMonster.Attack < Enemy.BattlingMonster.Defense) && (Bot.BattlingMonster.Attack + Enemy.BattlingMonster.Attack > Enemy.BattlingMonster.Defense)));
            }

            if (Util.IsTurn1OrMain2() && HonestEffectCount <= 5)
            {
                HonestEffectCount++;
                return true;
            }

            return false;
        }

        protected bool IsMain1SearchDeferred()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;

            // Check if we can enter the Battle Phase
            bool canGoToBattle = Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase;
            if (!canGoToBattle) return false;

            // Check if we already have a face-up high-ATK/strong monster on our field
            bool hasStrongMonster = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                (c.Attack >= 2500 || 
                 (c.Attack >= 2000 && (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) || c.HasType(CardType.Ritual)))));

            return hasStrongMonster;
        }

        /// <summary>
        /// Default: prioritize expendable materials (low ATK, low priority) to protect ace cards.
        /// Step 1: Try using ONLY non-ace cards as material.
        /// Step 2: If not enough non-ace, reluctantly include ace cards (least valuable first).
        /// </summary>
        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SelectMaterialPreferNonAce(cards, min, max)
                ?? base.OnSelectLinkMaterial(cards, min, max);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SelectMaterialPreferNonAce(cards, min, max)
                ?? base.OnSelectXyzMaterial(cards, min, max);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            return SelectMaterialPreferNonAce(cards, min, max)
                ?? base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SelectMaterialPreferNonAce(cards, min, max)
                ?? base.OnSelectFusionMaterial(cards, min, max);
        }

        /// <summary>
        /// Shared material selection logic: prefer non-ace cards as material.
        /// Only uses ace cards when there aren't enough non-ace alternatives.
        /// When forced to use aces, picks the least valuable ones first.
        /// </summary>
        private IList<ClientCard> SelectMaterialPreferNonAce(IList<ClientCard> cards, int min, int max)
        {
            var valid = cards.Where(c => c != null).ToList();
            if (valid.Count < min) return null;

            // Split into non-ace and ace pools
            var nonAce = valid.Where(c => !IsAceCard(c))
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();

            // If we have enough non-ace cards, use only those
            if (nonAce.Count >= max)
                return nonAce.Take(max).ToList();

            if (nonAce.Count >= min)
            {
                // We have enough non-ace to meet minimum, but not max.
                // Use all non-ace first, then fill remaining slots with least-valuable aces.
                int needed = max - nonAce.Count;
                var acePool = valid.Where(c => IsAceCard(c))
                    .OrderBy(c => GetMaterialPriority(c))  // least valuable aces first
                    .Take(needed)
                    .ToList();
                nonAce.AddRange(acePool);
                return nonAce;
            }

            // Not enough non-ace at all — forced to use aces. Sort everything by priority.
            var allSorted = valid.OrderBy(c => GetMaterialPriority(c)).ToList();
            return allSorted.Take(max).ToList();
        }

        /// <summary>
        /// Return the enemy monster that should be disabled by the default logic.
        /// </summary>
        protected ClientCard DefaultGetDisableMonsterTarget()
        {
            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetShouldBeDisabledBeforeItUseEffectMonster();
                if (target != null)
                    return target;
            }

            ClientCard LastChainCard = Util.GetLastChainCard();

            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget() && !LastChainCard.IsShouldNotBeSpellTrapTarget())
            {
                return LastChainCard;
            }

            if (Bot.BattlingMonster != null && Enemy.BattlingMonster != null)
            {
                if (!Enemy.BattlingMonster.IsDisabled() && Enemy.BattlingMonster.IsCode(_CardId.EaterOfMillions))
                {
                    return Enemy.BattlingMonster;
                }
            }

            if (Duel.Phase == DuelPhase.BattleStart && Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetFirstMatchingCard(card =>
                    card.IsCode(_CardId.NumberS39UtopiaTheLightning) && !card.IsDisabled());
                if (target != null)
                    return target;
            }

            return null;
        }

        protected bool DefaultCheckWhetherCardIsNegated(ClientCard card)
        {
            if (card == null) return true;
            if (card.Data == null) return card.IsDisabled();
            int originId = card.Data.Alias;
            if (originId == 0) originId = card.Data.Id;
            return crossoutDesignatorIdList.Contains(originId)
                || (calledbytheGraveIdCountMap.ContainsKey(originId) && calledbytheGraveIdCountMap[originId] > 0)
                || (card.IsDisabled() && ((int)card.Location & (int)CardLocation.Onfield) > 0);
        }

        protected bool DefaultCheckWhetherCardIdIsNegated(int cardId)
        {
            return crossoutDesignatorIdList.Contains(cardId)
                || (calledbytheGraveIdCountMap.ContainsKey(cardId) && calledbytheGraveIdCountMap[cardId] > 0);
        }

        protected int GetCalledbytheGraveIdCount(int cardId)
        {
            if (!calledbytheGraveIdCountMap.ContainsKey(cardId)) return 0;
            return calledbytheGraveIdCountMap[cardId];
        }

        protected bool DefaultCheckWhetherNumber41IsActive()
        {
            return Bot.MonsterZone.Concat(Enemy.MonsterZone).Any(card =>
                card != null && card.IsFaceup() && (card.IsCode(_CardId.Number41BagooskatheTerriblyTiredTapir, _CardId.Number41BagooskatheTerriblyTiredTapirAlt) || card.Id == 26273196 || card.Id == 85359414)
                && card.IsDefense() && !card.IsDisabled());
        }

        /// <summary>
        /// Check whether all available spell columns are negated.
        /// </summary>
        protected bool DefaultCheckAllAvailableSpellColumnNegated()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Bot.SpellZone[i] != null)
                    continue;
                if (infiniteImpermanenceNegatedColumns.Contains(i))
                    continue;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether the spells will be negated.
        /// </summary>
        protected bool DefaultCheckWhetherSpellActivateWillBeNegated(ClientCard card)
        {
            if (card == null) return false;
            if (card.Location == CardLocation.SpellZone)
            {
                return infiniteImpermanenceNegatedColumns.Contains(card.Sequence);
            }
            return DefaultCheckAllAvailableSpellColumnNegated();
        }

        /// <summary>
        /// Check whether bot can search cards from deck.
        /// </summary>
        protected bool DefaultCheckWhetherBotCanSearch()
        {
            if (resolvedEffectIdList.Contains(_CardId.LockBird))
                return false;
            if (mistakenArrestAffectedCount > 0)
                return false;
            if (Bot.HasInMonstersZone(_CardId.ThunderKingRaiOh, notDisabled: true, faceUp: true)
                || Enemy.HasInMonstersZone(_CardId.ThunderKingRaiOh, notDisabled: true, faceUp: true))
                return false;
            if (Enemy.HasInMonstersZone(_CardId.ThunderDragonColossus, notDisabled: true, faceUp: true))
                return false;
            if (Bot.HasInSpellZone(_CardId.DeckLockdown, notDisabled: true, faceUp: true)
                || Enemy.HasInSpellZone(_CardId.DeckLockdown, notDisabled: true, faceUp: true)
                || Bot.HasInSpellZone(_CardId.Mistake, notDisabled: true, faceUp: true)
                || Enemy.HasInSpellZone(_CardId.Mistake, notDisabled: true, faceUp: true))
                return false;
            if (Enemy.HasInSpellZone(_CardId.DoomZDestruction, notDisabled: true, faceUp: true))
                return false;
            return true;
        }

        /// <summary>
        /// Check whether bot can draw cards.
        /// </summary>
        protected bool DefaultCheckWhetherBotCanDraw()
        {
            if (resolvedEffectIdList.Contains(_CardId.LockBird))
                return false;
            return true;
        }

        /// <summary>
        /// Check whether enemy can draw cards.
        /// </summary>
        protected bool DefaultCheckWhetherEnemyCanDraw()
        {
            if (resolvedEffectIdList.Contains(_CardId.LockBird))
                return false;
            return true;
        }

        /// <summary>
        /// Baseline fallback idle command for legacy bots.
        /// </summary>
        public override MainPhaseAction OnFallbackIdleCmd(MainPhase main)
        {
            if (main == null) return null;
            if (main.CanBattlePhase && Bot.HasAttackingMonster())
                return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
            return null;
        }
    }
}
