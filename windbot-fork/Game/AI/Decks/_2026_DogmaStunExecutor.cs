// ============================================================
// CARD AUDIT — 2026_DogmaStun (Enhanced High-IQ Strategic Stun)
// ============================================================
// | Card Name                     | Type       | OPT? | HOPT? | Cost | Effect Summary                              | Activate When                               | NEVER Activate When                           |
// |-------------------------------|------------|------|-------|------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Inspector Boarder             | Monster L4 | No   | No    | None | Lock monster effect activations             | Turn 1 Normal Summon priority #1            | Already have Boarder on field                 |
// | Dogmatika Ecclesia            | Monster L4 | Yes  | Yes   | None | NS/SS: Search Dogmatika Punishment          | In Hand / On Summon                         | Already searched Punishment                   |
// | Enneacraft - Asta.PIXEA       | Monster L9 | Yes  | Yes   | None | 3000 ATK beatstick / discard fodder         | Hand / Discard fodder                       | Need normal summon                            |
// | Nadir Servant                 | Spell      | Yes  | Yes   | Send | Send ED monster -> Add Ecclesia from Dk/GY  | Main Phase starter                          | Already have all pieces                       |
// | Decisive Battle of Golgonda   | ContSpell  | Yes  | No    | Send | Protects cards by dumping Albaz ED          | On field / When card would be destroyed     | Already have face-up Golgonda                 |
// | The Fallen & The Virtuous     | QuickSp    | Yes  | Yes   | Send | Send Albaz ED -> Destroy 1 face-up card     | Opponent has face-up threat                 | Opponent has 0 cards (guard self-harm)       |
// | Necrovalley                   | FieldSpell | No   | No    | None | Lock GY moves & banishes                    | Main Phase 1 early                          | Already active Necrovalley                    |
// | Terraforming                  | Spell      | No   | No    | None | Search Necrovalley                          | Main Phase 1 early                          | Already have Necrovalley                      |
// | Moon Mirror Shield            | EquipSpell | No   | No    | None | Win any battle (+100 over opponent)         | Equip to Boarder or Ecclesia                | No monsters on field                          |
// | Card of Demise                | Spell      | Yes  | Yes   | None | Draw until 3 cards in hand                  | After setting all Spells/Traps              | Hand > 3 or before setting traps             |
// | Pot of Prosperity             | Spell      | Yes  | Yes   | Banish| Excavate 3/6, add 1, cannot draw rest turn | Main Phase 1 early                          | After Demise                                  |
// | Super Polymerization          | QuickSp    | No   | No    | Discard| Fuse opponent monsters (unrespondable)     | Opponent has 2+ valid fusion materials      | Hand < 2 or no materials                     |
// | Called by the Grave           | QuickSp    | Yes  | Yes   | None | Banish & negate monster in GY (Handtrap)    | Opponent activates handtrap or GY effect    | Bot's own turn without threats                |
// | Evenly Matched                | NormalTrap | No   | No    | None | Banish opponent cards face-down             | End of Battle Phase, opponent field > bot   | Bot has more cards                            |
// | Crackdown                     | ContTrap   | No   | No    | None | Take control of 1 face-up opponent monster  | Opponent summons high ATK/threat monster    | Opponent has no monsters                      |
// | Solemn Strike                 | CounterTr  | No   | No    | 1500 | Pay 1500 LP -> Negate SS or monster eff     | Opponent summons boss or activates effect   | LP <= 1500                                    |
// | Solemn Judgment               | CounterTr  | No   | No    | LP/2 | Pay half LP -> Negate S/T or Summon         | Opponent activates board breaker / boss SS  | LP < 1000                                     |
// | Skill Drain                   | ContTrap   | No   | No    | 1000 | Pay 1000 LP -> Negate monster effects       | Opponent activates monster or on standby    | LP <= 1000 or already active                  |
// | Rivalry of Warlords           | ContTrap   | No   | No    | None | Lock players to 1 Type of monster           | Opponent controls multiple Types            | Already active                                |
// | Gozen Match                   | ContTrap   | No   | No    | None | Lock players to 1 Attribute of monster      | Opponent controls multiple Attributes       | Already active                                |
// | Dogmatika Punishment          | NormalTrap | Yes  | Yes   | Send | Send ED monster with >= ATK -> Pop monster  | Opponent summons threat monster             | No valid ED target with >= ATK                |
// | Elder Entity N'tss            | Fusion L4  | Yes  | No    | None | Sent to GY -> Destroy 1 card on field       | Sent as cost by Punishment / Nadir          | Field has no targets                          |
// | Garura                        | Fusion L6  | Yes  | Yes   | None | Sent to GY -> Draw 1 card                   | Sent as cost by Nadir / Punishment          | Deck empty                                    |
// | The Dragon that Devours Dogma | Fusion L8  | Yes  | Yes   | None | 3000 ATK send cost; End Phase search Dogma  | Sent as cost by Punishment / The Fallen     | Already in GY                                 |
// | Titaniklad                    | Fusion L8  | Yes  | Yes   | None | 2500 ATK send cost; End Phase search Dogma  | Sent as cost by Punishment / Nadir          | Already in GY                                 |
// | Albion the Branded Dragon     | Fusion L8  | Yes  | Yes   | None | 2500 ATK send cost; End Phase search Branded| Sent as cost by Punishment / The Fallen     | Already in GY                                 |
// | Starving Venom Fusion Dragon  | Fusion L8  | Yes  | No    | None | Super Poly target (2 DARK)                  | Super Poly resolution                       | -                                             |
// | Mudragon of the Swamp         | Fusion L4  | Yes  | No    | None | Super Poly target (Same Attr, diff Type)    | Super Poly resolution                       | -                                             |
// | S:P Little Knight             | Link 2     | Yes  | Yes   | None | Banish card on summon, dodge interruption   | Main Phase 2 or when Extra Deck open        | Extra Deck locked by Dogmatika                |
// | TY-PHON Sky Crisis            | Xyz R12    | Yes  | Yes   | None | Lock monsters with 2900+ ATK & bounce       | Opponent SS 2+ from Extra Deck              | Extra Deck locked by Dogmatika                |
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
    [Deck("2026_DogmaStun", "2026_DogmaStun")]
    public class _2026_DogmaStunExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Monsters
            public const int DogmatikaEcclesia = 60303688;
            public const int InspectorBoarder = 15397015;
            public const int EnneacraftAstaPIXEA = 28454232;

            // Spells
            public const int DecisiveBattleOfGolgonda = 70485614;
            public const int NadirServant = 1984618;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int Necrovalley = 47355498;
            public const int MoonMirrorShield = 19508728;
            public const int CardOfDemise = 59750328;
            public const int SuperPolymerization = 48130397;
            public const int PotOfProsperity = 84211599;
            public const int CalledByTheGrave = 24224830;
            public const int Terraforming = 73628505;

            // Traps
            public const int EvenlyMatched = 15693423;
            public const int Crackdown = 36975314;
            public const int SolemnStrike = 40605147;
            public const int SolemnJudgment = 41420027;
            public const int RivalryOfWarlords = 90846359;
            public const int GozenMatch = 53334471;
            public const int SkillDrain = 82732705;
            public const int DogmatikaPunishment = 82956214;

            // Extra Deck
            public const int AlbionTheBrandedDragon = 87746184;
            public const int TitanikladTheAshDragon = 41373230;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int ElderEntityNtss = 80532587;
            public const int Garura = 11765832;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int MudragonOfTheSwamp = 54757758;
            public const int SPLittleKnight = 29301450;
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_BANISH = 511;

        // Turn-scoped activation trackers
        private bool _nadirServantUsed = false;
        private bool _ecclesiaSummonUsed = false;
        private bool _ecclesiaEffectUsed = false;
        private bool _fallenVirtuousUsed = false;
        private bool _superPolyUsed = false;
        private bool _prosperityUsed = false;
        private bool _demiseUsed = false;

        public _2026_DogmaStunExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register strategic assets
            ResourcePlan.RegisterAceCards(
                CardId.InspectorBoarder,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.StarvingVenomFusionDragon,
                CardId.SPLittleKnight,
                CardId.SuperStarslayerTYPHONSkyCrisis
            );

            BaitPlanner.RegisterComboStarters(
                CardId.PotOfProsperity,
                CardId.Terraforming,
                CardId.NadirServant
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.InspectorBoarder,
                CardId.SkillDrain,
                CardId.Necrovalley
            );

            RegisterOptionalFieldRemovalCards(CardId.TheFallenAndTheVirtuous);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority Execution)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Counter Traps, Unrespondable Spells & Hard Negates ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            // ── Continuous Floodgates Activation ──
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, RivalryOfWarlordsEffect);
            AddExecutor(ExecutorType.Activate, CardId.GozenMatch, GozenMatchEffect);
            AddExecutor(ExecutorType.Activate, CardId.Crackdown, CrackdownEffect);

            // ── Reactive Removal Traps & Spells ──
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaPunishment, DogmatikaPunishmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // ── Extra Deck GY Trigger Effects ──
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssEffect);
            AddExecutor(ExecutorType.Activate, CardId.Garura, GaruraEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, DogmaDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TitanikladTheAshDragon, TitanikladEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionEffect);

            // ── Tier 1: Searchers, Draw & Setup Spells ──
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.DecisiveBattleOfGolgonda, GolgondaEffect);
            AddExecutor(ExecutorType.Activate, CardId.NadirServant, NadirServantEffect);
            AddExecutor(ExecutorType.Activate, CardId.MoonMirrorShield, MoonMirrorShieldEffect);

            // ── Tier 2: Monster Summons (Hierarchy: Boarder > Ecclesia) ──
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DogmatikaEcclesia, EcclesiaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, InspectorBoarderSummon);
            AddExecutor(ExecutorType.Summon, CardId.DogmatikaEcclesia, EcclesiaSummon);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaEcclesia, EcclesiaEffect);

            // ── Tier 3: Card of Demise (Play ONLY after clearing hand / setting traps) ──
            AddExecutor(ExecutorType.Activate, CardId.CardOfDemise, CardOfDemiseEffect);

            // ── Tier 4: Spell & Trap Sets (Fill Backrow before End Phase) ──
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DogmatikaPunishment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.Crackdown, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.RivalryOfWarlords, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.GozenMatch, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.EvenlyMatched, EvenlyMatchedSet);

            // ── Tier 5: Repositioning ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _nadirServantUsed = false;
            _ecclesiaSummonUsed = false;
            _ecclesiaEffectUsed = false;
            _fallenVirtuousUsed = false;
            _superPolyUsed = false;
            _prosperityUsed = false;
            _demiseUsed = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.InspectorBoarder
                || card.Id == CardId.TheDragonThatDevoursTheDogma
                || card.Id == CardId.StarvingVenomFusionDragon
                || card.Id == CardId.SPLittleKnight
                || card.Id == CardId.SuperStarslayerTYPHONSkyCrisis;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: HIGH-IQ COUNTER TRAPS & NEGATES
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;
            if (Bot.LifePoints <= 500) return false;

            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && (lastCard.Controller == 0 || lastCard.IsDisabled())) return false;

            // Prioritize board breakers that would destroy backrow / monsters
            if (lastCard != null)
            {
                int id = lastCard.GetNonAltartCode();
                if (id == 18144507  // Harpie's Feather Duster
                    || id == 14532163  // Lightning Storm
                    || id == 12580477  // Raigeki
                    || id == 15693423  // Evenly Matched
                    || id == 5318639   // Mystical Space Typhoon
                    || id == 98338152  // Cosmic Cyclone
                    || id == 43898403  // Twin Twisters
                    || id == 44362883  // Branded Fusion
                    || CardIntelligence.IsHighThreatChokepoint(id))
                {
                    return true;
                }

                // Inherent high-threat boss summons
                if (lastCard.IsExtraCard() || lastCard.Attack >= 2500 || CardIntelligence.IsKnownNegator(id))
                {
                    return true;
                }
            }

            return DefaultSolemnJudgment();
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;

            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && (lastCard.Controller == 0 || lastCard.IsDisabled())) return false;

            // Inherent Summon negation
            if (Duel.LastChainPlayer == -1 && lastCard != null)
            {
                if (lastCard.IsExtraCard() || lastCard.Attack >= 1800 || CardIntelligence.IsKnownNegator(lastCard.Id))
                    return true;
            }

            // Monster effect activation negation
            if (lastCard != null && lastCard.IsMonster())
            {
                int id = lastCard.GetNonAltartCode();
                if (CardIntelligence.IsKnownNegator(id)
                    || CardIntelligence.IsHighThreatChokepoint(id)
                    || CardIntelligence.IsHandtrap(id)
                    || lastCard.Attack >= 1500
                    || lastCard.Location == CardLocation.MonsterZone)
                {
                    return true;
                }
            }

            return DefaultSolemnStrike();
        }

        private bool CalledByTheGraveEffect()
        {
            return DefaultCalledByTheGrave();
        }

        // ═══════════════════════════════════════════════════════════════
        //  REMOVAL & DISRUPTION HANDLERS
        // ═══════════════════════════════════════════════════════════════

        private bool DogmatikaPunishmentEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DogmatikaPunishment)) return false;

            var oppMonsters = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !CardIntelligence.IsTargetImmune(c))
                .ToList();

            if (oppMonsters.Count == 0) return false;

            // SMART TIMING:
            // Opponent Turn:
            // 1. Monster activates effect on field -> chain Punishment!
            // 2. Opponent controls an Extra Deck monster or Known Negator / Chokepoint
            // 3. Opponent is in Battle Phase or attacking
            // 4. Opponent has monster >= 1800 ATK
            // 5. Opponent is ending their turn (clear board before next turn)
            bool shouldTrigger = false;
            if (Duel.Player == 1)
            {
                ClientCard lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.Location == CardLocation.MonsterZone)
                    shouldTrigger = true;
                else if (oppMonsters.Any(c => c.IsExtraCard() || CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id)))
                    shouldTrigger = true;
                else if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.End)
                    shouldTrigger = true;
                else if (oppMonsters.Any(c => c.Attack >= 1800))
                    shouldTrigger = true;
            }
            else
            {
                shouldTrigger = Duel.Phase == DuelPhase.Main2 || oppMonsters.Any(c => CardIntelligence.IsKnownNegator(c.Id));
            }

            if (!shouldTrigger) return false;

            // Select highest threat target
            var oppMonster = oppMonsters.OrderByDescending(c => {
                int score = c.Attack;
                if (CardIntelligence.IsKnownNegator(c.Id)) score += 5000;
                if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 3000;
                if (c.IsExtraCard()) score += 2000;
                return score;
            }).FirstOrDefault();

            if (oppMonster == null) return false;

            int targetAtk = oppMonster.Attack;
            ClientCard edTarget = null;

            // Priority Extra Deck Dump:
            // 1. N'tss (2500 ATK) -> pops a 2nd card! (if target <= 2500 and Enemy has another card on field)
            if (targetAtk <= 2500 && Enemy.GetFieldCount() > 1 && Bot.ExtraDeck.Any(c => c.Id == CardId.ElderEntityNtss))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.ElderEntityNtss);
            }
            // 2. Garura (1500 ATK) -> draws 1 card
            else if (targetAtk <= 1500 && Bot.ExtraDeck.Any(c => c.Id == CardId.Garura))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.Garura);
            }
            // 3. The Dragon that Devours Dogma (3000 ATK) -> searches Dogmatika in End Phase
            else if (targetAtk <= 3000 && Bot.ExtraDeck.Any(c => c.Id == CardId.TheDragonThatDevoursTheDogma))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.TheDragonThatDevoursTheDogma);
            }
            // 4. Titaniklad (2500 ATK) -> searches Ecclesia in End Phase
            else if (targetAtk <= 2500 && Bot.ExtraDeck.Any(c => c.Id == CardId.TitanikladTheAshDragon))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.TitanikladTheAshDragon);
            }
            // 5. Albion (2500 ATK) -> sets The Fallen & The Virtuous in End Phase
            else if (targetAtk <= 2500 && Bot.ExtraDeck.Any(c => c.Id == CardId.AlbionTheBrandedDragon))
            {
                edTarget = Bot.ExtraDeck.First(c => c.Id == CardId.AlbionTheBrandedDragon);
            }
            else
            {
                edTarget = Bot.ExtraDeck.OrderBy(c => c.Attack).FirstOrDefault(c => c.Attack >= targetAtk);
            }

            if (edTarget != null)
            {
                AI.SelectCard(oppMonster);
                AI.SelectNextCard(edTarget);
                return true;
            }
            return false;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (_fallenVirtuousUsed) return false;

            // Mode 1: Send Albaz Fusion from Extra Deck -> Destroy 1 face-up card on field
            // STRICT SAFEGUARD: Only activate if opponent has face-up cards to avoid self-harm!
            var oppTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (CardIntelligence.IsFloodgateSpellTrap(c.Id) || c.HasType(CardType.Continuous) || c.HasType(CardType.Field)))
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsKnownNegator(c.Id))
                         ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            if (oppTarget != null && Bot.ExtraDeck.Any(c => c.Id == CardId.TheDragonThatDevoursTheDogma || c.Id == CardId.TitanikladTheAshDragon || c.Id == CardId.AlbionTheBrandedDragon))
            {
                _fallenVirtuousUsed = true;
                AI.SelectOption(0);
                AI.SelectCard(CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon, CardId.AlbionTheBrandedDragon);
                AI.SelectNextCard(oppTarget);
                return true;
            }

            // Mode 2: If Ecclesia is on field or GY, Special Summon 1 monster from either GY
            bool ecclesiaAvailable = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.DogmatikaEcclesia)
                                  || Bot.Graveyard.Any(c => c.Id == CardId.DogmatikaEcclesia);

            if (ecclesiaAvailable && Bot.GetMonsterCount() < 5)
            {
                var oppGyMonster = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
                var botGyMonster = Bot.Graveyard.Where(c => c != null && c.IsMonster() && c.Id != CardId.DogmatikaEcclesia).OrderByDescending(c => c.Attack).FirstOrDefault();
                var gyTarget = oppGyMonster ?? botGyMonster;

                if (gyTarget != null && gyTarget.Attack >= 1500)
                {
                    _fallenVirtuousUsed = true;
                    AI.SelectOption(1);
                    AI.SelectCard(gyTarget);
                    return true;
                }
            }

            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (_superPolyUsed) return false;
            if (Bot.Hand.Count < 2 && Card.Location == CardLocation.Hand) return false;
            if (Bot.Hand.Count < 1 && Card.Location == CardLocation.SpellZone) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SuperPolymerization)) return false;

            var allMonsters = new List<ClientCard>();
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var botMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();

            allMonsters.AddRange(oppMonsters);
            allMonsters.AddRange(botMonsters);

            if (allMonsters.Count < 2) return false;

            // Timing check:
            // Opponent turn:
            // - Chain to monster activation or when opp has 2+ monsters
            // Bot turn:
            // - Clear opponent field in Main 1 or push for lethal in Battle Phase
            bool shouldActivate = false;
            if (Duel.Player == 1)
            {
                ClientCard last = Util.GetLastChainCard();
                if (last != null && last.Controller == 1 && last.Location == CardLocation.MonsterZone)
                    shouldActivate = true;
                else if (oppMonsters.Count >= 2)
                    shouldActivate = true;
                else if (oppMonsters.Count >= 1 && botMonsters.Count >= 1)
                    shouldActivate = true;
            }
            else
            {
                shouldActivate = oppMonsters.Count > 0;
            }

            if (!shouldActivate) return false;

            // 1. Starving Venom: 2 DARK monsters on field (tokens excluded)
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.StarvingVenomFusionDragon))
            {
                var darks = allMonsters.Where(c => c.HasAttribute(CardAttribute.Dark) && !c.HasType(CardType.Token)).ToList();
                if (darks.Count >= 2 && darks.Any(c => c.Controller == 1))
                {
                    _superPolyUsed = true;
                    return true;
                }
            }

            // 2. Mudragon of the Swamp: 2 monsters with Same Attribute, Different Type
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.MudragonOfTheSwamp))
            {
                for (int i = 0; i < allMonsters.Count; i++)
                {
                    for (int j = i + 1; j < allMonsters.Count; j++)
                    {
                        if (allMonsters[i].Attribute == allMonsters[j].Attribute
                            && allMonsters[i].Race != allMonsters[j].Race
                            && (allMonsters[i].Controller == 1 || allMonsters[j].Controller == 1))
                        {
                            _superPolyUsed = true;
                            return true;
                        }
                    }
                }
            }

            // 3. Garura: 2 monsters with Same Attribute, Same Type, Different Names
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.Garura))
            {
                for (int i = 0; i < allMonsters.Count; i++)
                {
                    for (int j = i + 1; j < allMonsters.Count; j++)
                    {
                        if (allMonsters[i].Attribute == allMonsters[j].Attribute
                            && allMonsters[i].Race == allMonsters[j].Race
                            && allMonsters[i].Id != allMonsters[j].Id
                            && (allMonsters[i].Controller == 1 || allMonsters[j].Controller == 1))
                        {
                            _superPolyUsed = true;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool EvenlyMatchedEffect()
        {
            return Enemy.GetFieldCount() > Bot.GetFieldCount() + 1;
        }

        private bool CrackdownEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.Crackdown)) return false;
            if (Bot.GetMonsterCount() >= 5) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !CardIntelligence.IsTargetImmune(c)).ToList();
            if (targets.Count == 0) return false;

            // SMART TIMING:
            // Opponent Turn:
            // - Intercept Extra Deck monster summon or on-field effect activation
            // - Break pair before Link/Xyz climb
            // - Steal attacker in Battle Phase
            // Bot Turn:
            // - Steal in Main 1 to remove blocker or use as Link material for S:P
            bool trigger = false;
            if (Duel.Player == 1)
            {
                ClientCard lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.Location == CardLocation.MonsterZone)
                    trigger = true;
                else if (targets.Any(c => c.IsExtraCard() || CardIntelligence.IsKnownNegator(c.Id) || c.Attack >= 1800))
                    trigger = true;
                else if (targets.Count >= 2)
                    trigger = true;
                else if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                    trigger = true;
            }
            else
            {
                trigger = (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
            }

            if (!trigger) return false;

            var target = targets.OrderByDescending(c => {
                int score = c.Attack;
                if (CardIntelligence.IsKnownNegator(c.Id)) score += 5000;
                if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 3000;
                if (c.IsExtraCard()) score += 2000;
                return score;
            }).FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  FLOODGATE CONTINUOUS TRAPS & SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool SkillDrainEffect()
        {
            if (Card.IsFaceup()) return false;
            if (Bot.LifePoints <= 1000) return false;
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SkillDrain)) return false;

            // Activate in response to opponent monster effect, or Draw/Standby Phase
            if (Duel.Player == 1)
            {
                ClientCard lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster()) return true;
                if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby) return true;
            }
            return Enemy.GetMonsterCount() > 0;
        }

        private bool RivalryOfWarlordsEffect()
        {
            if (Card.IsFaceup()) return false;
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.RivalryOfWarlords)) return false;
            return Duel.Player == 1 || Enemy.GetMonsterCount() >= 2;
        }

        private bool GozenMatchEffect()
        {
            if (Card.IsFaceup()) return false;
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.GozenMatch)) return false;
            return Duel.Player == 1 || Enemy.GetMonsterCount() >= 2;
        }

        private bool NecrovalleyEffect()
        {
            if (Card.IsFaceup()) return false;
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.Necrovalley);
        }

        private bool GolgondaEffect()
        {
            // Continuous Spell activation from hand
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.DecisiveBattleOfGolgonda);
            }
            // Destruction substitution: send Albaz Fusion to GY
            AI.SelectCard(CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon, CardId.AlbionTheBrandedDragon);
            return true;
        }

        private bool MoonMirrorShieldEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.GetMonsterCount() == 0) return false;
                var equipTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.InspectorBoarder)
                               ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.DogmatikaEcclesia)
                               ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                if (equipTarget != null)
                {
                    AI.SelectCard(equipTarget);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true; // Return to top/bottom of deck
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCHERS & DRAW SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool TerraformingEffect()
        {
            AI.SelectCard(CardId.Necrovalley);
            return true;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsed || _demiseUsed) return false;
            _prosperityUsed = true;
            return true;
        }

        private bool NadirServantEffect()
        {
            if (_nadirServantUsed) return false;
            _nadirServantUsed = true;

            // Send N'tss (if opponent controls cards to pop), or Garura (draw 1), or Dogma Dragon (3000 ATK, EP search)
            if (Enemy.GetFieldCount() > 0 && Bot.ExtraDeck.Any(c => c.Id == CardId.ElderEntityNtss))
            {
                AI.SelectCard(CardId.ElderEntityNtss);
            }
            else if (Bot.ExtraDeck.Any(c => c.Id == CardId.Garura))
            {
                AI.SelectCard(CardId.Garura);
            }
            else if (Bot.ExtraDeck.Any(c => c.Id == CardId.TheDragonThatDevoursTheDogma))
            {
                AI.SelectCard(CardId.TheDragonThatDevoursTheDogma);
            }
            else
            {
                AI.SelectCard(CardId.TitanikladTheAshDragon, CardId.AlbionTheBrandedDragon);
            }

            // Add Ecclesia from Deck or GY
            AI.SelectNextCard(CardId.DogmatikaEcclesia);
            return true;
        }

        private bool CardOfDemiseEffect()
        {
            if (_demiseUsed || _prosperityUsed) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            // Delay Demise if hand has settable traps or field spells that should be deployed first
            int settable = Bot.Hand.Count(c => c != null && c.Id != CardId.CardOfDemise && (c.IsTrap() || c.HasType(CardType.QuickPlay) || c.Id == CardId.DecisiveBattleOfGolgonda || c.Id == CardId.Necrovalley));
            if (settable > 0 && Bot.GetSpellCount() < 5) return false;

            // Also summon monsters before Demise (Demise blocks SS after resolution)
            if (Bot.GetMonsterCount() == 0 && (Bot.HasInHand(CardId.InspectorBoarder) || Bot.HasInHand(CardId.DogmatikaEcclesia)))
                return false;

            int handCount = Bot.Hand.Count;
            if (handCount <= 3)
            {
                _demiseUsed = true;
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SENT TO GY TRIGGERS
        // ═══════════════════════════════════════════════════════════════

        private bool NtssEffect()
        {
            var oppTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsFloodgateSpellTrap(c.Id))
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsKnownNegator(c.Id))
                         ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup())
                         ?? Enemy.GetMonsters().FirstOrDefault()
                         ?? Enemy.GetSpells().FirstOrDefault();

            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool GaruraEffect() => true;

        private bool DogmaDragonEffect()
        {
            AI.SelectCard(CardId.DogmatikaEcclesia, CardId.DogmatikaPunishment);
            return true;
        }

        private bool TitanikladEffect()
        {
            AI.SelectCard(CardId.DogmatikaEcclesia);
            return true;
        }

        private bool AlbionEffect()
        {
            AI.SelectCard(CardId.TheFallenAndTheVirtuous);
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTER SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool InspectorBoarderSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool EcclesiaSpSummon()
        {
            if (_ecclesiaSummonUsed) return false;
            bool edOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsExtraCard())
                          || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsExtraCard());

            if (edOnField)
            {
                _ecclesiaSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool EcclesiaSummon()
        {
            if (Bot.HasInHand(CardId.InspectorBoarder) && Bot.GetMonsterCount() == 0) return false;
            return true;
        }

        private bool EcclesiaEffect()
        {
            if (_ecclesiaEffectUsed) return false;
            _ecclesiaEffectUsed = true;
            AI.SelectCard(CardId.DogmatikaPunishment);
            return true;
        }

        private bool SPLittleKnightSummon()
        {
            // Only summon if opponent has high-threat card and we have disposable materials
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return materials.Count >= 2 && Enemy.GetFieldCount() > 0;
        }

        private bool SPLittleKnightEffect()
        {
            // On summon: banish 1 card on field or in either GY
            var oppThreat = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsFloodgateSpellTrap(c.Id))
                         ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsKnownNegator(c.Id))
                         ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            if (oppThreat != null)
            {
                AI.SelectCard(oppThreat);
                return true;
            }
            return true;
        }

        private bool EvenlyMatchedSet()
        {
            return Duel.Turn == 1 || Duel.Player == 0;
        }

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            // Always keep Boarder and Ecclesia in Attack position if safe
            if (Card.Id == CardId.InspectorBoarder && Card.IsAttack()) return false;
            return DefaultMonsterRepos();
        }

        // ═══════════════════════════════════════════════════════════════
        //  ONSELECTCARD INTELLIGENCE
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Pot of Prosperity Extra Deck Banish
            if (cards.All(c => c.Location == CardLocation.Extra))
            {
                var banishPriority = new[] {
                    CardId.SeaMonsterOfTheseus,
                    CardId.MudragonOfTheSwamp,
                    CardId.StarvingVenomFusionDragon,
                    CardId.AlbionTheBrandedDragon,
                    CardId.TitanikladTheAshDragon,
                    CardId.Garura,
                    CardId.SPLittleKnight
                };
                var result = new List<ClientCard>();
                foreach (int id in banishPriority)
                {
                    foreach (var c in cards.Where(c2 => c2.Id == id && !result.Contains(c2)))
                    {
                        if (result.Count < max) result.Add(c);
                    }
                }
                foreach (var c in cards.Where(c2 => !result.Contains(c2)))
                {
                    if (result.Count < max) result.Add(c);
                }
                if (result.Count >= min) return result.Take(max).ToList();
            }

            // 2. Pot of Prosperity Excavated Cards Selection (hint == HINT_SELECT_TOHAND)
            if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity && hint == HINT_SELECT_TOHAND)
            {
                var preferred = new[] {
                    CardId.InspectorBoarder,
                    CardId.SkillDrain,
                    CardId.Necrovalley,
                    CardId.SolemnJudgment,
                    CardId.SolemnStrike,
                    CardId.DogmatikaPunishment,
                    CardId.DogmatikaEcclesia,
                    CardId.NadirServant,
                    CardId.TheFallenAndTheVirtuous,
                    CardId.Crackdown,
                    CardId.GozenMatch,
                    CardId.RivalryOfWarlords,
                    CardId.SuperPolymerization,
                    CardId.EvenlyMatched
                };
                foreach (int id in preferred)
                {
                    var pick = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (pick != null) return new List<ClientCard> { pick };
                }
            }

            // 3. Super Polymerization Discard Cost (hint == HINT_SELECT_DISCARD)
            if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization && hint == HINT_SELECT_DISCARD)
            {
                var safeDiscards = cards.OrderBy(c => {
                    if (c.Id == CardId.EnneacraftAstaPIXEA) return 1;
                    if (c.Id == CardId.DecisiveBattleOfGolgonda && Bot.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == CardId.DecisiveBattleOfGolgonda)) return 2;
                    if (c.Id == CardId.Necrovalley && Bot.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == CardId.Necrovalley)) return 3;
                    if (IsAceCard(c)) return 1000;
                    return 500 - c.Attack;
                }).Take(max).ToList();
                return safeDiscards;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
