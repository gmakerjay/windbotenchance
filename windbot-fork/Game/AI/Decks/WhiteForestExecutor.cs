// ============================================================================
// CARD AUDIT — WhiteForest (Modern White Forest & Toy Box Synchro Engine)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Diabellstar Vengeance              | Monster L8   | Yes  | Yes   | Send GY | Hand/Field Quick: Negate & banish opp monster | Opponent activates monster effect            | Target already negated or invalid           |
// | Nibiru, the Primal Being           | Monster L11  | Yes  | Yes   | Tribute | Quick: Tribute all field monsters, SS + Token | Opponent 5+ summons and has high threat      | Bot controls established winning board      |
// | Ghost Ogre & Snow Rabbit           | Monster L3 T | Yes  | Yes   | Send GY | Handtrap: destroy face-up card activating eff | Opponent activates card/eff on field         | Bot controls no hand or target is immune    |
// | Effect Veiler                      | Monster L1 T | Yes  | Yes   | Send GY | Handtrap: Negate 1 opponent face-up monster   | Opponent Main Phase, unnegated monster       | Target already disabled                     |
// | D.D. Crow                          | Monster L1   | No   | No    | Discard | Quick: Banish 1 card in opponent Graveyard    | Opponent targets GY or key target in GY      | Opponent GY empty                           |
// | Radian, the Multidimensional Kaiju | Monster L7   | No   | No    | Tribute | Special Summon to opp field by tributing 1    | Opponent controls dangerous threat/negator   | Opponent controls no monsters               |
// | The Iris Swordsoul                 | Monster L8   | Yes  | Yes   | None    | SS if negated monster on field; pop/draw      | Main Phase, negated monster on field         | Already Special Summoned this turn          |
// | Diabellze the White Witch          | Monster L8   | Yes  | Yes   | None    | SS if control Diabellstar/Sinful; pop S/T     | Control Diabellstar/Sinful; opp S/T to pop    | No valid targets                            |
// | Astellar of the White Forest       | Monster L2   | Yes  | Yes   | Send ST | Send S/T: SS LIGHT Spellcaster Tuner; GY SS   | Main Phase primary starter; GY trigger on ST | Already used this turn                      |
// | Elzette of the White Forest        | Monster L2   | Yes  | Yes   | Send ST | Send S/T: SS self + search White Forest; GY add| Hand starter; opponent turn recycle          | Already used this turn                      |
// | Silvy of the White Forest          | Monster L4 T | Yes  | Yes   | None    | On NS/SS: Search White Forest S/T; GY bounce  | Normal/Special Summoned; GY bounce Synchro   | Target Synchro on FIELD (destroys own boss) |
// | Rucia of the White Forest          | Monster L4 T | Yes  | Yes   | Send ST | SS if control White Forest; send S/T draw 1   | Control White Forest; GY bounce Synchro      | Target Synchro on FIELD (destroys own boss) |
// | Toy Soldier                        | Monster L4   | Yes  | Yes   | None    | Set self as Spell; SS when sent from S/T; srch| Set in S/T, send as cost to SS & search      | Already used this turn                      |
// | Toy Tank                           | Monster L4   | Yes  | Yes   | None    | Set self as Spell; SS when sent from S/T; trib| Set in S/T, send as cost to SS & revive      | Already used this turn                      |
// | Toy Box                            | Spell Cont   | Yes  | No    | None    | Set up to 2 Toy monsters in S/T; pop attacker | Main Phase setup; battle phase protection    | S/T zone full                               |
// | Tales of the White Forest          | Spell Normal | Yes  | Yes   | None    | Search White Forest; re-set when sent as cost | Control Spellcaster/Illusion; auto re-sets   | Already used this turn                      |
// | Scourge of the White Forest        | Spell Quick  | Yes  | Yes   | Tribute | Tribute Synchro to negate 1 face-up; re-set   | Opponent activates or target face-up threat  | No Synchro to tribute                       |
// | Filia Diabell                      | Spell Normal | Yes  | Yes   | None    | Search Level 8+ Diabell monster               | Main Phase search Diabellstar Vengeance      | Already used this turn                      |
// | Forbidden Chalice                  | Spell Quick  | No   | No    | None    | Negate 1 face-up monster on field (+400 ATK)  | Opponent activates monster eff or on threat  | Target already negated                      |
// | Mystical Space Typhoon             | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on the field             | Opponent controls dangerous backrow/floodgate | Opponent controls 0 Spells/Traps            |
// | Woes of the White Forest           | Trap Normal  | Yes  | Yes   | None    | SS White Forest + immediate Synchro; re-set   | Opponent turn or end of Main Phase; re-sets   | Already used this turn                      |
// | Curse of Diabell                   | Trap Normal  | Yes  | Yes   | Send mon| Wipe all opp face-up cards if 2+ Diabell      | Control 2+ Diabell monsters                  | Opponent controls 0 face-up cards           |
// | Diabell, Queen of the White Forest | Synchro L8   | Yes  | Yes   | Send ST | Quick: SS L7 or lower Synchro Tuner from ED/GY| Opponent activates card/eff; retrieve S/T    | No S/T to send or no Synchro Tuner in ED/GY |
// | Silvera, Wolf Tamer of White Forest| Synchro L6 T | Yes  | Yes   | None    | On SS: Turn all opp face-up monsters face-down| On Special Summon (via Synchro or Diabell)   | Opponent controls 0 face-up monsters        |
// | Rciela, Sinister Soul White Forest | Synchro L6 T | Yes  | Yes   | Send ST | On SS: Send S/T, search White Forest / Light  | On Special Summon (ladder step to Diabell)   | No S/T to send                              |
// | Zalen the Shackled Dragon          | Synchro L7 T | Yes  | Yes   | None    | Quick: Negate 1st or 2nd effect in chain      | ChainLink >= 2 with opponent effect          | Already used this turn                      |
// | Draco Berserker of the Tenyi       | Synchro L8   | Yes  | Yes   | None    | Quick: Banish opponent monster that activates | Opponent activates monster effect            | Target unaffected                           |
// | Crimson Blader                     | Synchro L8   | No   | No    | None    | Destroys monster: opp cannot summon L5+ next  | Battle Phase against Level 5+ meta decks     | Opponent has no monsters                    |
// | Stardust Dragon                    | Synchro L8   | No   | No    | Tribute | Quick: Negate effect that destroys cards      | Opponent activates destruction effect        | Already tributed this turn                  |
// | Snake-Eyes Doomed Dragon           | Fusion L8    | Yes  | Yes   | Send ST | Send 2 S/T monster cards: place enemy in S/T  | Opponent controls monster; have 2 S/T monsters| Enemy monster zone empty                    |
// | Tornado Dragon                     | Xyz R4       | Yes  | No    | Detach  | Quick: Detach 1, destroy 1 Spell/Trap         | Opponent activates S/T or has backrow         | Opponent controls 0 S/T                     |
// | Evilswarm Exciton Knight           | Xyz R4       | No   | No    | Detach  | Quick: Wipe all other cards on field if behind| Opponent has more total cards than bot       | Bot has equal or more cards                 |
// | Bujintei Tsukuyomi                 | Xyz R4       | Yes  | No    | Detach  | Detach 1, send hand to GY, draw 2 cards       | Hand has 0-1 cards, need fresh resources     | Hand has high value cards                   |
// | Dingirsu, Orcust of Evening Star   | Xyz R8       | Yes  | No    | None    | Non-targeting send 1 opp card to GY; protect  | Special Summoned; board breaking push         | Opponent controls 0 cards                   |
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
    [Deck("WhiteForest", "WhiteForest")]
    public class WhiteForestExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int ElzetteOfTheWhiteForest = 61980241;
            public const int AstellarOfTheWhiteForest = 25592142;
            public const int RuciaOfTheWhiteForest = 24779554;
            public const int SilvyOfTheWhiteForest = 98385955;
            public const int DiabellzeTheWhiteWitch = 60145298;
            public const int DiabellstarVengeance = 23151193;
            public const int TheIrisSwordsoul = 62849088;
            public const int ToySoldier = 65504487;
            public const int ToyTank = 69925461;
            public const int RadianTheMultidimensionalKaiju = 28674152;
            public const int NibiruThePrimalBeing = 27204311;
            public const int GhostOgreAndSnowRabbit = 59438930;
            public const int EffectVeiler = 97268402;
            public const int DDCrow = 24508238;

            // Spells
            public const int TalesOfTheWhiteForest = 99289828;
            public const int ScourgeOfTheWhiteForest = 93723936;
            public const int FiliaDiabell = 78293584;
            public const int ToyBox = 24878656;
            public const int ForbiddenChalice = 25789292;
            public const int MysticalSpaceTyphoon = 5318639;

            // Traps
            public const int WoesOfTheWhiteForest = 62995268;
            public const int CurseOfDiabell = 64998567;

            // Extra Deck
            public const int DiabellQueenOfTheWhiteForest = 14307929;
            public const int RcielaSinisterSoulOfTheWhiteForest = 77313225;
            public const int SilveraWolfTamerOfTheWhiteForest = 41924516;
            public const int PoplarOfTheWhiteForest = 5800323;
            public const int SnakeEyesVengeanceDragon = 79415624;
            public const int StardustDragon = 44508094;
            public const int ZalenTheShackledDragon = 4891376;
            public const int DracoBerserkerOfTheTenyi = 5041348;
            public const int CrimsonBlader = 80321197;
            public const int VisasAmritara = 821049;
            public const int SnakeEyesDoomedDragon = 58071334;
            public const int TornadoDragon = 6983839;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int BujinteiTsukuyomi = 73289035;
            public const int DingirsuTheOrcustOfTheEveningStar = 93854893;
        }

        public WhiteForestExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Quick Disruption (Enemy / Chain)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DiabellstarVengeance, DiabellstarVengeanceActivate);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgreAndSnowRabbit, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.DDCrow, DDCrowActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenChalice, ForbiddenChaliceActivate);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoonActivate);
            AddExecutor(ExecutorType.Activate, CardId.ZalenTheShackledDragon, ZalenActivate);
            AddExecutor(ExecutorType.Activate, CardId.DracoBerserkerOfTheTenyi, DracoBerserkerActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragon, StardustDragonActivate);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonKnightActivate);

            // -------------------------------------------------------------
            // 2. Boss Quick Interaction: Diabell, Queen of the White Forest
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DiabellQueenOfTheWhiteForest, DiabellQueenActivate);
            AddExecutor(ExecutorType.Activate, CardId.SilveraWolfTamerOfTheWhiteForest, SilveraActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheIrisSwordsoul, TheIrisSwordsoulActivate);

            // -------------------------------------------------------------
            // 3. Board Breaker: Kaiju & Removal
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.RadianTheMultidimensionalKaiju, RadianKaijuSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScourgeOfTheWhiteForest, ScourgeActivate);
            AddExecutor(ExecutorType.Activate, CardId.CurseOfDiabell, CurseOfDiabellActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SnakeEyesDoomedDragon, SnakeEyesDoomedDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesDoomedDragon, SnakeEyesDoomedDragonActivate);

            // -------------------------------------------------------------
            // 4. Toy Box Engine Setup
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ToyBox, ToyBoxActivate);
            AddExecutor(ExecutorType.SpellSet, CardId.ToySoldier, ToyMonsterSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ToyTank, ToyMonsterSpellSet);

            // -------------------------------------------------------------
            // 5. Toy Floating Triggers (When sent from S/T to GY)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ToySoldier, ToySoldierEffect);
            AddExecutor(ExecutorType.Activate, CardId.ToyTank, ToyTankEffect);

            // -------------------------------------------------------------
            // 6. Search & Field Extenders
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.FiliaDiabell, FiliaDiabellActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellzeTheWhiteWitch, DiabellzeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DiabellzeTheWhiteWitch, DiabellzeActivate);
            AddExecutor(ExecutorType.Activate, CardId.TalesOfTheWhiteForest, TalesActivate);
            AddExecutor(ExecutorType.Activate, CardId.WoesOfTheWhiteForest, WoesActivate);

            // -------------------------------------------------------------
            // 7. White Forest Starters & Extenders
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ElzetteOfTheWhiteForest, ElzetteActivate);
            AddExecutor(ExecutorType.Summon, CardId.AstellarOfTheWhiteForest, AstellarSummon);
            AddExecutor(ExecutorType.Activate, CardId.AstellarOfTheWhiteForest, AstellarActivate);
            AddExecutor(ExecutorType.Summon, CardId.SilvyOfTheWhiteForest, SilvySummon);
            AddExecutor(ExecutorType.Activate, CardId.SilvyOfTheWhiteForest, SilvyActivate);
            AddExecutor(ExecutorType.Activate, CardId.RuciaOfTheWhiteForest, RuciaActivate);
            AddExecutor(ExecutorType.Summon, CardId.RuciaOfTheWhiteForest, RuciaSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElzetteOfTheWhiteForest, ElzetteSummon);
            AddExecutor(ExecutorType.Summon, CardId.ToySoldier, ToySoldierSummon);

            // -------------------------------------------------------------
            // 8. Extra Deck Climbing & Boss Summons
            // -------------------------------------------------------------
            // Step 1: Synchro Tuner Level 6 (Ladder Engine)
            AddExecutor(ExecutorType.SpSummon, CardId.RcielaSinisterSoulOfTheWhiteForest, RcielaSummon);
            AddExecutor(ExecutorType.Activate, CardId.RcielaSinisterSoulOfTheWhiteForest, RcielaActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SilveraWolfTamerOfTheWhiteForest, SilveraSummon);

            // Step 2: Level 8 Boss (Diabell Queen / Draco Berserker / Crimson Blader / Stardust / Visas)
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellQueenOfTheWhiteForest, DiabellQueenSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DracoBerserkerOfTheTenyi, DracoBerserkerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonBlader, CrimsonBladerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustDragon, StardustDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.VisasAmritara, VisasAmritaraSummon);
            AddExecutor(ExecutorType.Activate, CardId.VisasAmritara, VisasAmritaraActivate);

            // Step 3: Level 7 Disruption Tuner
            AddExecutor(ExecutorType.SpSummon, CardId.ZalenTheShackledDragon, ZalenSummon);

            // Step 4: Level 4 Mini Tuner
            AddExecutor(ExecutorType.SpSummon, CardId.PoplarOfTheWhiteForest, PoplarSummon);
            AddExecutor(ExecutorType.Activate, CardId.PoplarOfTheWhiteForest, PoplarActivate);

            // Step 5: Rank 4 / Rank 8 Xyz Summons
            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BujinteiTsukuyomi, BujinteiTsukuyomiSummon);
            AddExecutor(ExecutorType.Activate, CardId.BujinteiTsukuyomi, BujinteiTsukuyomiActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DingirsuTheOrcustOfTheEveningStar, DingirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.DingirsuTheOrcustOfTheEveningStar, DingirsuActivate);

            // -------------------------------------------------------------
            // 9. Backrow Set & Reposition Strategy
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =====================================================================
        // Boss Protection Helper
        // =====================================================================
        private bool IsWhiteForestBoss(ClientCard c)
        {
            if (c == null) return false;
            if (c.Id == CardId.DiabellQueenOfTheWhiteForest ||
                c.Id == CardId.DiabellzeTheWhiteWitch ||
                c.Id == CardId.DracoBerserkerOfTheTenyi ||
                c.Id == CardId.CrimsonBlader ||
                c.Id == CardId.StardustDragon ||
                c.Id == CardId.DingirsuTheOrcustOfTheEveningStar ||
                c.Id == CardId.TheIrisSwordsoul ||
                c.Id == CardId.SnakeEyesDoomedDragon)
                return true;
            if (c.IsFaceup() && c.Attack >= 2400)
                return true;
            return false;
        }

        // =====================================================================
        // Handtraps & Quick Disruption Callbacks
        // =====================================================================
        private bool DiabellstarVengeanceActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = LastChainCard;
                if (target != null && (target.Location == CardLocation.MonsterZone || target.Location == CardLocation.Hand || target.Location == CardLocation.Grave))
                {
                    return true;
                }
            }
            return false;
        }

        private bool GhostOgreActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = LastChainCard;
                if (target != null && (target.Location == CardLocation.MonsterZone || target.Location == CardLocation.SpellZone))
                {
                    return true;
                }
            }
            return false;
        }

        private bool EffectVeilerActivate()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && m.Attack >= 1500);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool DDCrowActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = Enemy.Graveyard.OrderByDescending(c => c.Attack).FirstOrDefault(c => c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool NibiruActivate()
        {
            // Protect our established board: NEVER wipe if we control our key bosses!
            if (Bot.HasInMonstersZone(CardId.DiabellQueenOfTheWhiteForest) || Bot.HasInMonstersZone(CardId.DiabellzeTheWhiteWitch))
                return false;

            return DefaultNibiru();
        }

        private bool ForbiddenChaliceActivate()
        {
            // 1. Negate opponent activated monster effect in chain
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = LastChainCard;
                if (target != null && target.Controller == 1 && target.Location == CardLocation.MonsterZone && !target.IsDisabled())
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            // 2. Negate dangerous monster during Main Phase (Floodgate / Negator)
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                ClientCard threat = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() &&
                    (CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id) || CardIntelligence.IsHighThreatChokepoint(m.Id)));
                if (threat != null)
                {
                    AI.SelectCard(threat);
                    return true;
                }
            }

            // 3. Battle Phase: NEVER buff an enemy monster (e.g. Nibiru or opposing battler)!
            // Only buff OUR battling monster if that +400 ATK is what turns a loss/draw into a win!
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                ClientCard ourBattler = Bot.BattlingMonster;
                ClientCard oppBattler = Enemy.BattlingMonster;
                if (ourBattler != null && oppBattler != null && ourBattler.IsFaceup())
                {
                    if (ourBattler.Attack <= oppBattler.Attack && ourBattler.Attack + 400 > oppBattler.Attack)
                    {
                        AI.SelectCard(ourBattler);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MysticalSpaceTyphoonActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = LastChainCard;
                // Only chain to cards that MUST remain on the field to resolve!
                // MST DOES NOT NEGATE Normal Spells, Normal Traps, or Quick-Play Spells!
                if (target != null && target.Controller == 1 && target.Location == CardLocation.SpellZone)
                {
                    bool requiresFieldPresence = target.HasType(CardType.Continuous) ||
                                                 target.HasType(CardType.Field) ||
                                                 target.HasType(CardType.Equip) ||
                                                 (target.Type & (int)CardType.Pendulum) != 0 ||
                                                 CardIntelligence.IsFloodgate(target.Id);
                    if (requiresFieldPresence)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            // Outside chain: destroy active face-up continuous/field/floodgate S/T
            ClientCard faceupThreat = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup() &&
                (s.HasType(CardType.Continuous) || s.HasType(CardType.Field) || s.HasType(CardType.Equip) || CardIntelligence.IsFloodgate(s.Id)));
            if (faceupThreat != null)
            {
                AI.SelectCard(faceupThreat);
                return true;
            }

            // End Phase: snipe opponent's set backrow
            if (Duel.Phase == DuelPhase.End)
            {
                ClientCard setCard = Enemy.GetSpells().FirstOrDefault(s => s.IsFacedown());
                if (setCard != null)
                {
                    AI.SelectCard(setCard);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldSkipForLethal()
        {
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && CanDealLethal())
                return true;
            return false;
        }

        private bool ZalenActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool DracoBerserkerActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = LastChainCard;
                if (target != null && target.Controller == 1 && target.Location == CardLocation.MonsterZone)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool StardustDragonActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool TornadoDragonActivate()
        {
            ClientCard target = Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ExcitonKnightActivate()
        {
            int botTotal = Bot.GetMonsterCount() + Bot.GetSpellCount() + Bot.Hand.Count;
            int oppTotal = Enemy.GetMonsterCount() + Enemy.GetSpellCount() + Enemy.Hand.Count;
            return oppTotal > botTotal;
        }

        // =====================================================================
        // Diabell Queen & Silvera Callbacks
        // =====================================================================
        private bool DiabellQueenActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // 1. Quick effect: When opponent activates card or effect, send 1 S/T to SS Level 7 or lower Synchro Tuner!
                if (Duel.LastChainPlayer == 1)
                {
                    ClientCard stCost = GetExpendableSpellTrapCost();
                    if (stCost != null)
                    {
                        AI.SelectCard(stCost);
                        // Priority target: Silvera (Book of Eclipse opp board) > Rciela > Zalen
                        AI.SelectNextCard(new[] {
                            CardId.SilveraWolfTamerOfTheWhiteForest,
                            CardId.RcielaSinisterSoulOfTheWhiteForest,
                            CardId.ZalenTheShackledDragon
                        });
                        return true;
                    }
                }

                // 2. On-Summon trigger (when summoned using a Synchro Tuner): Target 1 S/T in GY, add to hand!
                if (Bot.Graveyard.Any(c => c.IsSpell() || c.IsTrap()))
                {
                    ClientCard keyST = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TalesOfTheWhiteForest || c.Id == CardId.WoesOfTheWhiteForest) ??
                                       Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.ForbiddenChalice || c.Id == CardId.MysticalSpaceTyphoon || c.Id == CardId.ToyBox) ??
                                       Bot.Graveyard.FirstOrDefault(c => c.IsSpell() || c.IsTrap());
                    if (keyST != null)
                    {
                        AI.SelectCard(keyST);
                        return true;
                    }
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool SilveraActivate()
        {
            // On SS: Change all opponent face-up monsters to face-down defense position!
            return Enemy.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool TheIrisSwordsoulActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Main Phase: SS if monster with negated effect is on field
                bool hasNegated = Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsDisabled()) ||
                                  Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsDisabled());
                return hasNegated;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            return false;
        }

        // =====================================================================
        // Board Breakers & Kaiju Callbacks
        // =====================================================================
        private bool RadianKaijuSummon()
        {
            // Tribute opponent highest threat / negator
            ClientCard oppBoss = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (oppBoss != null && (oppBoss.Attack >= 2500 || oppBoss.IsDisabled() == false))
            {
                AI.SelectCard(oppBoss);
                return true;
            }
            return false;
        }

        private bool ScourgeActivate()
        {
            // Tribute 1 Synchro to negate 1 face-up card on field
            ClientCard oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            if (oppTarget == null) oppTarget = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup());

            ClientCard synchroTribute = Bot.GetMonsters().FirstOrDefault(m => m.HasType(CardType.Synchro) && !IsWhiteForestBoss(m));

            if (oppTarget != null && synchroTribute != null)
            {
                AI.SelectCard(synchroTribute);
                AI.SelectNextCard(oppTarget);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool CurseOfDiabellActivate()
        {
            int diabellCount = Bot.GetMonsters().Count(m => m.IsFaceup() && (m.Id == CardId.DiabellstarVengeance || m.Id == CardId.DiabellzeTheWhiteWitch || m.Id == CardId.DiabellQueenOfTheWhiteForest));
            if (diabellCount >= 2 && Enemy.GetMonsters().Any(m => m.IsFaceup()))
            {
                ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => !IsWhiteForestBoss(m));
                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            return false;
        }

        private bool SnakeEyesDoomedDragonSummon()
        {
            // SS by sending 2 face-up monster cards from S/T zone
            var stMonsters = Bot.GetSpells().Where(s => s.IsFaceup() && (s.Id == CardId.ToySoldier || s.Id == CardId.ToyTank)).ToList();
            if (stMonsters.Count >= 2)
            {
                AI.SelectCard(stMonsters.Take(2).ToList());
                return true;
            }
            return false;
        }

        private bool SnakeEyesDoomedDragonActivate()
        {
            // On SS: Place 1 opponent monster into their Spell & Trap Zone
            ClientCard target = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // =====================================================================
        // Toy Engine Callbacks
        // =====================================================================
        private bool ToyBoxActivate()
        {
            if (ShouldSkipForLethal()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.ToyBox);
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                // Set up to 2 Toy monsters from deck/hand into S/T zone
                int freeST = 5 - Bot.GetSpellCount();
                if (freeST >= 2)
                {
                    AI.SelectOption(0); // Option 0: Set toys
                    AI.SelectCard(new[] {
                        CardId.ToySoldier,
                        CardId.ToyTank
                    });
                    return true;
                }
            }
            return false;
        }

        private bool ToyMonsterSpellSet()
        {
            // Place Toy Soldier / Toy Tank as Spell in S/T zone from hand
            return Bot.GetSpellCount() < 5;
        }

        private bool ToySoldierEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // When sent from S/T to GY: SS self!
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipForLethal()) return false;
                // On NS/SS: Add Toy Box from deck to hand (or Level 4 LIGHT)
                AI.SelectCard(CardId.ToyBox);
                return true;
            }
            return false;
        }

        private bool ToyTankEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // When sent from S/T to GY: SS self!
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipForLethal()) return false;
                // Tribute self to SS Level 6 or lower monster from GY
                ClientCard reviveTarget = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.RcielaSinisterSoulOfTheWhiteForest || c.Id == CardId.SilveraWolfTamerOfTheWhiteForest) ??
                                         Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.SilvyOfTheWhiteForest || c.Id == CardId.AstellarOfTheWhiteForest);
                if (reviveTarget != null)
                {
                    AI.SelectCard(reviveTarget);
                    return true;
                }
            }
            return false;
        }

        // =====================================================================
        // White Forest Starters & Extenders Callbacks
        // =====================================================================
        private bool FiliaDiabellActivate()
        {
            // Add Level 8+ Diabell monster from deck
            AI.SelectCard(new[] {
                CardId.DiabellstarVengeance,
                CardId.DiabellzeTheWhiteWitch
            });
            return true;
        }

        private bool DiabellzeSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.Id == CardId.DiabellstarVengeance || m.Id == CardId.DiabellQueenOfTheWhiteForest);
        }

        private bool DiabellzeActivate()
        {
            ClientCard target = Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TalesActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (ShouldSkipForLethal()) return false;
                bool hasSpellcasterOrIllusion = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasRace(CardRace.SpellCaster) || m.Race == 0x2000000));
                if (hasSpellcasterOrIllusion)
                {
                    int target = !Bot.HasInHand(CardId.AstellarOfTheWhiteForest) ? CardId.AstellarOfTheWhiteForest :
                                 !Bot.HasInHand(CardId.SilvyOfTheWhiteForest) ? CardId.SilvyOfTheWhiteForest :
                                 !Bot.HasInHand(CardId.ElzetteOfTheWhiteForest) ? CardId.ElzetteOfTheWhiteForest :
                                 CardId.RuciaOfTheWhiteForest;
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Re-set self when sent to GY for monster effect!
                return true;
            }
            return false;
        }

        private bool WoesActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (ShouldSkipForLethal()) return false;
                // SS White Forest from hand/deck and immediate Synchro
                AI.SelectCard(new[] {
                    CardId.SilvyOfTheWhiteForest,
                    CardId.RuciaOfTheWhiteForest,
                    CardId.AstellarOfTheWhiteForest
                });
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Re-set self!
                return true;
            }
            return false;
        }

        private bool ElzetteActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipForLethal()) return false;
                // Send 1 S/T to SS self + search White Forest monster
                ClientCard stCost = GetExpendableSpellTrapCost();
                if (stCost != null)
                {
                    AI.SelectCard(stCost);
                    int searchTarget = !Bot.HasInHand(CardId.AstellarOfTheWhiteForest) ? CardId.AstellarOfTheWhiteForest :
                                       !Bot.HasInHand(CardId.SilvyOfTheWhiteForest) ? CardId.SilvyOfTheWhiteForest :
                                       CardId.RuciaOfTheWhiteForest;
                    AI.SelectNextCard(searchTarget);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // In opp turn, if LIGHT Spellcaster Tuner is SS: add self to hand
                return Duel.Player == 1;
            }
            return false;
        }

        private bool ElzetteSummon()
        {
            if (ShouldSkipForLethal()) return false;
            // Unbrick: Normal summon if field empty OR we have Tuners to tune with Level 2 into Level 6/8
            if (Bot.GetMonsterCount() == 0) return true;
            bool hasTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner() && !IsWhiteForestBoss(m));
            if (hasTuner && !Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && !IsWhiteForestBoss(m)))
                return true;
            if (GetExpendableSpellTrapCost() != null && Bot.GetMonsterCount() < 4)
                return true;
            return false;
        }

        private bool AstellarSummon()
        {
            if (ShouldSkipForLethal()) return false;
            // Primary Normal Summon starter!
            return true;
        }

        private bool AstellarActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipForLethal()) return false;
                // Send 1 S/T from hand/field to GY: SS LIGHT Spellcaster Tuner from Deck!
                ClientCard stCost = GetExpendableSpellTrapCost();
                if (stCost != null)
                {
                    AI.SelectCard(stCost);
                    AI.SelectNextCard(new[] {
                        CardId.SilvyOfTheWhiteForest,
                        CardId.RuciaOfTheWhiteForest,
                        CardId.EffectVeiler
                    });
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // If S/T sent to GY to activate monster effect: SS self from GY!
                return true;
            }
            return false;
        }

        private bool SilvySummon()
        {
            if (ShouldSkipForLethal()) return false;
            return true;
        }

        private bool SilvyActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipForLethal()) return false;
                // On NS/SS: Search White Forest S/T
                AI.SelectCard(new[] {
                    CardId.TalesOfTheWhiteForest,
                    CardId.WoesOfTheWhiteForest,
                    CardId.ScourgeOfTheWhiteForest
                });
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // STRICT FIELD BOSS PROTECTION:
                // Target White Forest Synchro in GRAVEYARD (NOT ON FIELD!)
                ClientCard synchroTarget = Bot.Graveyard.FirstOrDefault(c =>
                    c.Id == CardId.RcielaSinisterSoulOfTheWhiteForest ||
                    c.Id == CardId.SilveraWolfTamerOfTheWhiteForest ||
                    c.Id == CardId.PoplarOfTheWhiteForest);
                if (synchroTarget != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(synchroTarget);
                    return true;
                }
            }
            return false;
        }

        private bool RuciaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipForLethal()) return false;
                // Hand effect: SS if controlling White Forest monster
                bool hasWf = Bot.GetMonsters().Any(m => m.IsFaceup() && (
                    m.Id == CardId.AstellarOfTheWhiteForest ||
                    m.Id == CardId.ElzetteOfTheWhiteForest ||
                    m.Id == CardId.SilvyOfTheWhiteForest ||
                    m.Id == CardId.RcielaSinisterSoulOfTheWhiteForest ||
                    m.Id == CardId.SilveraWolfTamerOfTheWhiteForest ||
                    m.Id == CardId.DiabellQueenOfTheWhiteForest));
                return hasWf && Bot.GetMonsterCount() < 5;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipForLethal()) return false;
                // Send 1 S/T to draw 1
                ClientCard stCost = GetExpendableSpellTrapCost();
                if (stCost != null)
                {
                    AI.SelectCard(stCost);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // STRICT FIELD BOSS PROTECTION:
                // Target White Forest Synchro in GRAVEYARD (NOT ON FIELD!)
                ClientCard synchroTarget = Bot.Graveyard.FirstOrDefault(c =>
                    c.Id == CardId.RcielaSinisterSoulOfTheWhiteForest ||
                    c.Id == CardId.SilveraWolfTamerOfTheWhiteForest ||
                    c.Id == CardId.PoplarOfTheWhiteForest);
                if (synchroTarget != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(synchroTarget);
                    return true;
                }
            }
            return false;
        }

        private bool RuciaSummon()
        {
            if (ShouldSkipForLethal()) return false;
            if (Bot.GetMonsterCount() == 0) return true;
            bool hasNonTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && !IsWhiteForestBoss(m));
            if (hasNonTuner && !Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner()))
                return true;
            return false;
        }

        private bool ToySoldierSummon()
        {
            if (ShouldSkipForLethal()) return false;
            if (Bot.GetMonsterCount() == 0) return true;
            bool hasLevel4 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 4 && !IsWhiteForestBoss(m));
            if (hasLevel4 && Bot.GetMonsterCount() < 4) return true;
            return false;
        }

        // =====================================================================
        // Extra Deck Summons Callbacks
        // =====================================================================
        private bool RcielaSummon()
        {
            // Level 6 Synchro Tuner — 1 Tuner + 1+ Non-Tuner (e.g. Silvy Lv4 + Astellar/Elzette Lv2)
            // NEVER sacrifice healthy Level 8 Bosses!
            int nonBossTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner() && !IsWhiteForestBoss(m));
            int nonBossNonTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner() && !IsWhiteForestBoss(m));
            return nonBossTuners >= 1 && nonBossNonTuners >= 1;
        }

        private bool RcielaActivate()
        {
            // On SS: Send 1 S/T from hand/field to GY; add 1 White Forest card or LIGHT Spellcaster from deck!
            ClientCard stCost = GetExpendableSpellTrapCost();
            if (stCost != null)
            {
                AI.SelectCard(stCost);
                AI.SelectNextCard(new[] {
                    CardId.TheIrisSwordsoul,
                    CardId.TalesOfTheWhiteForest,
                    CardId.WoesOfTheWhiteForest,
                    CardId.AstellarOfTheWhiteForest,
                    CardId.SilvyOfTheWhiteForest,
                    CardId.ElzetteOfTheWhiteForest,
                    CardId.RuciaOfTheWhiteForest,
                    CardId.EffectVeiler
                });
                return true;
            }
            return false;
        }

        private bool SilveraSummon()
        {
            // Level 6 Synchro Tuner — Book of Eclipse on legs!
            if (Enemy.GetMonsters().Any(m => m.IsFaceup()))
            {
                int nonBossTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner() && !IsWhiteForestBoss(m));
                int nonBossNonTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner() && !IsWhiteForestBoss(m));
                if (nonBossTuners >= 1 && nonBossNonTuners >= 1) return true;
            }
            // Fallback if Rciela already summoned
            if (Bot.HasInMonstersZone(CardId.RcielaSinisterSoulOfTheWhiteForest))
            {
                int nonBossTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner() && !IsWhiteForestBoss(m) && m.Id != CardId.RcielaSinisterSoulOfTheWhiteForest);
                int nonBossNonTuners = Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner() && !IsWhiteForestBoss(m));
                if (nonBossTuners >= 1 && nonBossNonTuners >= 1) return true;
            }
            return false;
        }

        private bool DiabellQueenSummon()
        {
            // Level 8 Core Boss (2500 ATK)
            // Only 1 Diabell Queen needed on field!
            if (Bot.HasInMonstersZone(CardId.DiabellQueenOfTheWhiteForest)) return false;

            // Must not sacrifice another Level 8 Boss (Draco Berserker, Crimson Blader, etc.)
            bool hasSynchroTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.RcielaSinisterSoulOfTheWhiteForest || m.Id == CardId.SilveraWolfTamerOfTheWhiteForest));
            bool hasLv2 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 2 && !IsWhiteForestBoss(m));
            if (hasSynchroTuner && hasLv2) return true;

            bool hasLv4Tuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 4 && m.IsTuner() && !IsWhiteForestBoss(m));
            bool hasLv4NonTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 4 && !m.IsTuner() && !IsWhiteForestBoss(m));
            if (hasLv4Tuner && hasLv4NonTuner) return true;

            return false;
        }

        private bool DracoBerserkerSummon()
        {
            // Level 8 Boss (3000 ATK, Banish disruption)
            if (Bot.HasInMonstersZone(CardId.DracoBerserkerOfTheTenyi)) return false;

            // Do NOT sacrifice Diabell Queen
            bool hasSynchroTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.RcielaSinisterSoulOfTheWhiteForest || m.Id == CardId.SilveraWolfTamerOfTheWhiteForest));
            bool hasLv2 = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 2 && !IsWhiteForestBoss(m));
            if (hasSynchroTuner && hasLv2) return true;

            bool hasLv4Tuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 4 && m.IsTuner() && !IsWhiteForestBoss(m));
            bool hasLv4NonTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 4 && !m.IsTuner() && !IsWhiteForestBoss(m));
            if (hasLv4Tuner && hasLv4NonTuner) return true;

            return false;
        }

        private bool CrimsonBladerSummon()
        {
            if (Bot.HasInMonstersZone(CardId.CrimsonBlader)) return false;
            if (!Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Level >= 5)) return false;
            if (Bot.HasInMonstersZone(CardId.DiabellQueenOfTheWhiteForest) && Bot.HasInMonstersZone(CardId.DracoBerserkerOfTheTenyi)) return false;
            return true;
        }

        private bool StardustDragonSummon()
        {
            if (Bot.HasInMonstersZone(CardId.StardustDragon)) return false;
            if (Bot.HasInMonstersZone(CardId.DiabellQueenOfTheWhiteForest) && Bot.HasInMonstersZone(CardId.DracoBerserkerOfTheTenyi)) return false;
            return true;
        }

        private bool VisasAmritaraSummon()
        {
            if (Bot.HasInMonstersZone(CardId.VisasAmritara)) return false;
            return true;
        }

        private bool VisasAmritaraActivate()
        {
            return true;
        }

        private bool ZalenSummon()
        {
            if (Bot.HasInMonstersZone(CardId.ZalenTheShackledDragon)) return false;
            return true;
        }

        private bool PoplarSummon()
        {
            // Level 4 Synchro Tuner (700 ATK)
            // NEVER summon if we already have Level 6 or Level 8 bosses!
            if (Bot.GetMonsters().Any(m => IsWhiteForestBoss(m) || m.Id == CardId.RcielaSinisterSoulOfTheWhiteForest || m.Id == CardId.SilveraWolfTamerOfTheWhiteForest))
                return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 2 && m.IsTuner()) &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && m.Level == 2 && !m.IsTuner());
        }

        private bool PoplarActivate()
        {
            AI.SelectCard(new[] {
                CardId.TalesOfTheWhiteForest,
                CardId.WoesOfTheWhiteForest
            });
            return true;
        }

        private bool ExcitonKnightSummon()
        {
            int botTotal = Bot.GetMonsterCount() + Bot.GetSpellCount() + Bot.Hand.Count;
            int oppTotal = Enemy.GetMonsterCount() + Enemy.GetSpellCount() + Enemy.Hand.Count;
            if (oppTotal <= botTotal) return false;
            int lv4Count = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 4 && !IsWhiteForestBoss(m));
            return lv4Count >= 2;
        }

        private bool TornadoDragonSummon()
        {
            if (Enemy.GetSpellCount() == 0) return false;
            int lv4Count = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 4 && !IsWhiteForestBoss(m));
            return lv4Count >= 2;
        }

        private bool BujinteiTsukuyomiSummon()
        {
            if (Bot.Hand.Count > 1) return false;
            int lv4LightCount = Bot.GetMonsters().Count(m => m.IsFaceup() && m.Level == 4 && m.HasAttribute(CardAttribute.Light) && !IsWhiteForestBoss(m));
            return lv4LightCount >= 2;
        }

        private bool BujinteiTsukuyomiActivate()
        {
            return Bot.Hand.Count <= 1;
        }

        private bool DingirsuSummon()
        {
            // NEVER summon Dingirsu if opponent has 0 monsters (don't waste bosses on backrow!)
            if (Enemy.GetMonsterCount() == 0) return false;

            // NEVER sacrifice Diabell Queen or Diabellze! Only use expendable Level 8s
            var expendableLv8 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 8 &&
                m.Id != CardId.DiabellQueenOfTheWhiteForest &&
                m.Id != CardId.DiabellzeTheWhiteWitch &&
                !IsWhiteForestBoss(m)).ToList();

            if (expendableLv8.Count < 2) return false;

            return Enemy.GetMonsters().Any(m => m.IsFaceup() && (m.Attack >= 2500 || CardIntelligence.IsTargetImmune(m) || CardIntelligence.IsHighThreatChokepoint(m.Id)));
        }

        private bool DingirsuActivate()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target == null) target = Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // =====================================================================
        // S/T Cost Resolver: Identifies cards that float or re-set
        // =====================================================================
        private ClientCard GetExpendableSpellTrapCost()
        {
            // 1. Toy Soldier / Toy Tank in S/T zone (they SS themselves when sent to GY!)
            ClientCard toyInST = Bot.GetSpells().FirstOrDefault(s => s.IsFaceup() && (s.Id == CardId.ToySoldier || s.Id == CardId.ToyTank));
            if (toyInST != null) return toyInST;

            // 2. White Forest Spells/Traps on field (they re-set themselves to field from GY!)
            ClientCard wfSpellOnField = Bot.GetSpells().FirstOrDefault(s => s.Id == CardId.TalesOfTheWhiteForest ||
                                                                           s.Id == CardId.WoesOfTheWhiteForest ||
                                                                           s.Id == CardId.ScourgeOfTheWhiteForest);
            if (wfSpellOnField != null) return wfSpellOnField;

            // 3. White Forest Spells/Traps in hand (they re-set themselves to field from GY!)
            ClientCard wfSpellInHand = Bot.Hand.FirstOrDefault(c => c.Id == CardId.TalesOfTheWhiteForest ||
                                                                   c.Id == CardId.WoesOfTheWhiteForest ||
                                                                   c.Id == CardId.ScourgeOfTheWhiteForest);
            if (wfSpellInHand != null) return wfSpellInHand;

            // 4. Toy Soldier / Tank in hand
            ClientCard toyInHand = Bot.Hand.FirstOrDefault(c => c.Id == CardId.ToySoldier || c.Id == CardId.ToyTank);
            if (toyInHand != null) return toyInHand;

            // 5. Any other non-essential Spell/Trap on field (strictly EXCLUDING Toy Box if it is our only Toy Box)
            ClientCard otherST = Bot.GetSpells().FirstOrDefault(s => s.Id != CardId.ToyBox);
            if (otherST != null) return otherST;

            // 6. Non-essential Spell/Trap in hand (e.g. duplicate Chalice/MST)
            ClientCard expendableInHand = Bot.Hand.FirstOrDefault(c => (c.IsSpell() || c.IsTrap()) && c.Id != CardId.ToyBox);
            if (expendableInHand != null) return expendableInHand;

            return null;
        }

        // =====================================================================
        // Backrow & Reposition
        // =====================================================================
        private bool SpellSetStrategy()
        {
            // Strictly NO handtraps in MP1
            if (Card.Id == CardId.GhostOgreAndSnowRabbit || Card.Id == CardId.EffectVeiler || Card.Id == CardId.DDCrow)
                return false;

            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            // Switch 0 ATK monsters to Defense
            if (Card.IsAttack() && (Card.Attack <= 1000 || Card.Id == CardId.AstellarOfTheWhiteForest || Card.Id == CardId.ElzetteOfTheWhiteForest))
                return true;
            // Switch high ATK monsters to Attack
            if (Card.IsDefense() && Card.Attack >= 1900)
                return true;
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Primal Being Token (27204312) given to opponent must ALWAYS be FaceUpDefence!
            if (cardId == 27204312 && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            // When summoning 0 ATK starters, choose FaceUpDefence if not attacking
            if (cardId == CardId.AstellarOfTheWhiteForest || cardId == CardId.ElzetteOfTheWhiteForest || cardId == CardId.PoplarOfTheWhiteForest)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        // =====================================================================
        // Safe Selection Overrides
        // =====================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 🚫 Rule 1: Hint 502 (Destroy), 503/504 (Remove/Banish), 508 (ToGrave removal):
            // MUST target ENEMY cards (Controller == 1) if available!
            if (hint == 502 || hint == 503 || hint == 504 || hint == 508)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    return enemyCards.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            // Hint 506 (HINTMSG_ATOHAND): Search priority
            if (hint == 506)
            {
                int[] searchPriority = new[]
                {
                    CardId.AstellarOfTheWhiteForest,
                    CardId.SilvyOfTheWhiteForest,
                    CardId.ElzetteOfTheWhiteForest,
                    CardId.TheIrisSwordsoul,
                    CardId.RuciaOfTheWhiteForest,
                    CardId.TalesOfTheWhiteForest,
                    CardId.WoesOfTheWhiteForest,
                    CardId.ToyBox,
                    CardId.ScourgeOfTheWhiteForest
                };

                var ordered = cards.OrderBy(c =>
                {
                    int idx = Array.IndexOf(searchPriority, c.Id);
                    return idx >= 0 ? idx : 100;
                }).ToList();

                return ordered.Take(max).ToList();
            }

            // Hint 500 (Release / Tribute): Never tribute Diabell Queen or Diabellze
            if (hint == 500)
            {
                var safeFodder = cards.Where(c => !IsWhiteForestBoss(c)).ToList();
                if (safeFodder.Count >= min)
                {
                    return safeFodder.OrderBy(c => c.Attack).Take(min).ToList();
                }
            }

            // Hint 513 / 519 (Xyz Material): Strictly protect Diabell Queen & Diabellze!
            if (hint == 513 || hint == 519)
            {
                var safeFodder = cards.Where(c => c.Id != CardId.DiabellQueenOfTheWhiteForest && c.Id != CardId.DiabellzeTheWhiteWitch && !IsWhiteForestBoss(c)).ToList();
                if (safeFodder.Count >= min)
                {
                    return safeFodder.OrderBy(c => c.Attack).Take(min).ToList();
                }
                var nonQueen = cards.Where(c => c.Id != CardId.DiabellQueenOfTheWhiteForest && c.Id != CardId.DiabellzeTheWhiteWitch).ToList();
                if (nonQueen.Count >= min)
                {
                    return nonQueen.OrderBy(c => c.Attack).Take(min).ToList();
                }
            }

            // Hint 512 (Synchro Material) / Hint 507 (General Material) / Hint 0:
            // Protect Bosses! Lowest stats / Tuners / non-bosses first!
            if (hint == 512 || hint == 507 || hint == 0)
            {
                var sortedMaterials = cards.OrderBy(c =>
                {
                    if (IsWhiteForestBoss(c)) return 999;
                    if (c.Id == CardId.RcielaSinisterSoulOfTheWhiteForest || c.Id == CardId.SilveraWolfTamerOfTheWhiteForest)
                        return 500; // Level 6 Synchros can be used to ladder into Diabell Queen
                    return c.Attack; // Smallest attack first (Astellar 0, Elzette 0, Rucia 800, Silvy 1500)
                }).ToList();

                return sortedMaterials.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
