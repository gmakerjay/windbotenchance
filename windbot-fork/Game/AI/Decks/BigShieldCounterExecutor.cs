// ============================================================================
// CARD AUDIT — BigShieldCounter (Big Shield Gardna Stun & Defense OTK Fortress)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Big Shield Gardna           | Monster L4   | No   | No    | None    | 2600 DEF; auto-negates targeting Spells face-down | Ace Tank: Set face-down or summon in DEF     | Never attack without lethal                 |
// | Mid Shield Gardna           | Monster L4   | Yes  | No    | None    | 1800 DEF; negates targeting Spells; flips down| Flip face-down each turn to re-arm trap      | Attack position                             |
// | Fossil Dyna Pachycephalo    | Monster L4   | No   | No    | None    | Neither player can Special Summon; flips wipe | Floodgate SS / Set as flip board wipe        | When already control face-up Dyna           |
// | Stronghold Guardian         | Monster L4   | No   | No    | Discard | Damage Step: attacked DEF monster gets +1500  | Discard when Big Shield is attacked for +1500 | Not in Damage Step                          |
// | Lord of the Heavenly Prison | Monster L10  | Yes  | Yes   | Reveal  | Set cards cannot be destroyed by effects!     | Hand reveal in MP1; SS 3000/3000 & Set trap  | Already revealed this turn                  |
// | Ash Blossom & Joyous Spring | Monster L3   | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent searches or special summons from deck| Already used this turn                      |
// | Reinforcement of the Army   | Spell Normal | No   | No    | None    | Add 1 Level 4 or lower Warrior from deck      | Search Big Shield Gardna (Ace Card)          | Big Shield already saturated                |
// | Pot of Extravagance         | Spell Normal | Yes  | Yes   | Banish6 | Banish 6 Extra Deck face-down -> Draw 2 cards | Start of Main Phase 1 for draw 2              | Not start of MP1 or already drawn           |
// | Harpie's Feather Duster     | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls | Wipe opponent backrow before attacks/combos   | Opponent has 0 Spells/Traps                 |
// | Skill Drain                 | Trap Cont.   | No   | No    | 1000 LP | STUN: Negate all face-up monster effects!     | Shuts down opp; locks Big Shield in 2600 DEF!| Bot LP <= 1000 or already active            |
// | Summon Limit                | Trap Cont.   | No   | No    | None    | STUN: Max 2 Normal/Special Summons per turn   | Freeze opponent combo expansion               | Already active on field                     |
// | Anti-Spell Fragrance        | Trap Cont.   | No   | No    | None    | STUN: Spells must be Set for 1 turn to activate| Stop board breaker spells (Duster/Raigeki)   | Already active on field                     |
// | Gozen Match                 | Trap Cont.   | No   | No    | None    | STUN: Only 1 Attribute allowed (All ours EARTH)| Lock multi-attribute decks to 1 monster      | Already active on field                     |
// | Battle Mania                | Trap Normal  | No   | No    | None    | Opp Standby: All opp monsters to ATK & MUST atk| Force opponent to crash into Big Shield Gardna| Opponent controls 0 monsters or reflect mons|
// | Staunch Defender            | Trap Normal  | No   | No    | None    | When opp attacks: ALL opp monsters must hit Ace| Direct all attacks into Big Shield Gardna wall| No face-up Gardna on field                  |
// | Rise to Full Height         | Trap Normal  | No   | Yes   | None    | Double DEF (5200!) & opp can only attack target| Target Big Shield to double DEF & lock attacks| No valid target on field                    |
// | D2 Shield                   | Trap Normal  | No   | No    | None    | Target face-up DEF monster: DEF becomes double| Target Big Shield Gardna (2600 -> 5200 DEF!)  | No face-up DEF monsters on field            |
// | Cross Counter               | Trap Normal  | No   | No    | None    | If DEF higher than ATK: double battle damage! | Battle Step: double battle damage again (x4!) | DEF is not higher than attacker's ATK       |
// | Solemn Judgment             | Trap Counter | No   | No    | Half LP | Negate Summon or Spell/Trap card and destroy  | Protect backrow / Big Shield from wipes       | Trivial cards                               |
// | Solemn Strike               | Trap Counter | No   | No    | 1500 LP | Negate Monster effect or Special Summon & pop | Protect monsters from monster removal effects | Negligible effects                          |
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
    [Deck("BigShieldCounter", "BigShieldCounter")]
    public class BigShieldCounterExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int BigShieldGardna = 65240384;
            public const int MidShieldGardna = 75487237;
            public const int FossilDynaPachycephalo = 42009836;
            public const int StrongholdGuardian = 23535429;
            public const int LordOfTheHeavenlyPrison = 9822220;
            public const int AshBlossom = 14558128;

            // Spells
            public const int ReinforcementOfTheArmy = 32807846;
            public const int PotOfExtravagance = 49238328;
            public const int HarpiesFeatherDuster = 18144506;

            // Stun & Continuous Floodgates
            public const int SkillDrain = 82732705;
            public const int SummonLimit = 23516703;
            public const int AntiSpellFragrance = 58921041;
            public const int GozenMatch = 53334471;

            // Battle & Reflect Traps
            public const int BattleMania = 31245780;
            public const int StaunchDefender = 92854392;
            public const int RiseToFullHeight = 19254117;
            public const int D2Shield = 71249758;
            public const int CrossCounter = 37083210;
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 40605147;

            // Extra Deck
            public const int HeroicChampionExcalibur = 60645181;
            public const int Number39Utopia = 84013237;
            public const int UtopiaTheLightning = 56832966;
            public const int Bagooska = 90590303;
            public const int GagagaCowboy = 12014404;
            public const int TornadoDragon = 6983839;
            public const int AbyssDweller = 21044178;
            public const int SpLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.BigShieldGardna,
            CardId.MidShieldGardna,
            CardId.FossilDynaPachycephalo,
            CardId.LordOfTheHeavenlyPrison,
            CardId.UtopiaTheLightning,
            CardId.HeroicChampionExcalibur
        };

        private bool _lordRevealedThisTurn = false;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public override void OnNewTurn()
        {
            _lordRevealedThisTurn = false;
            base.OnNewTurn();
        }

        public BigShieldCounterExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Counter Traps & Handtraps (High Priority Protection) ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);

            // ── 2. Heavy Stun Floodgates (Paralyze Opponent Engine) ──
            // Skill Drain: Blanket on-field monster negate (locks Big Shield in 2600 DEF permanently!)
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            // Summon Limit: Caps opponent at 2 summons per turn
            AddExecutor(ExecutorType.Activate, CardId.SummonLimit, SummonLimitEffect);
            // Anti-Spell Fragrance: Freeze all opponent spells for 1 turn
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellFragranceEffect);
            // Gozen Match: Lock to 1 Attribute (all our monsters are EARTH!)
            AddExecutor(ExecutorType.Activate, CardId.GozenMatch, GozenMatchEffect);

            // ── 3. Innate Monster Protection / Hand Counters ──
            // Big Shield Gardna: Negate targeting Spell card when face-down!
            AddExecutor(ExecutorType.Activate, CardId.BigShieldGardna);

            // Stronghold Guardian: Damage Step Handtrap (+1500 DEF)
            AddExecutor(ExecutorType.Activate, CardId.StrongholdGuardian, StrongholdGuardianEffect);

            // ── 4. Draw & Board Wipes ──
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // ── 5. Lord of the Heavenly Prison (Hand reveal protects all Set cards & floodgates!) ──
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, LordOfTheHeavenlyPrisonEffect);

            // ── 6. Monster Flip Effect: Mid Shield Gardna ──
            AddExecutor(ExecutorType.Activate, CardId.MidShieldGardna, MidShieldGardnaEffect);

            // ── 7. Normal Summons & Sets ──
            // Fossil Dyna: Set face-down as a flip board wipe or Normal Summon for instant SS floodgate
            AddExecutor(ExecutorType.MonsterSet, CardId.FossilDynaPachycephalo, FossilDynaMonsterSet);
            AddExecutor(ExecutorType.Summon, CardId.FossilDynaPachycephalo, FossilDynaSummon);

            // Big Shield Gardna & Mid Shield Gardna: Premier face-down walls
            AddExecutor(ExecutorType.MonsterSet, CardId.BigShieldGardna);
            AddExecutor(ExecutorType.MonsterSet, CardId.MidShieldGardna);

            // Fallback Normal Summons if empty board
            AddExecutor(ExecutorType.Summon, CardId.BigShieldGardna, FallbackSummon);
            AddExecutor(ExecutorType.Summon, CardId.MidShieldGardna, FallbackSummon);

            // ── 8. Extra Deck Utilities & Finishers ──
            AddExecutor(ExecutorType.SpSummon, CardId.UtopiaTheLightning, UtopiaLightningSummon);
            AddExecutor(ExecutorType.Activate, CardId.UtopiaTheLightning, UtopiaLightningEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HeroicChampionExcalibur, ExcaliburSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeroicChampionExcalibur, ExcaliburEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Number39Utopia, UtopiaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number39Utopia);
            AddExecutor(ExecutorType.SpSummon, CardId.GagagaCowboy, CowboySummon);
            AddExecutor(ExecutorType.Activate, CardId.GagagaCowboy, CowboyEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Bagooska, BagooskaSummon);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpLittleKnight, SpLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixEffect);

            // ── 9. Battle Forcing & Reflect Traps (Battle Mania / Staunch Defender / D2 Shield / Cross Counter) ──
            AddExecutor(ExecutorType.Activate, CardId.BattleMania, BattleManiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.StaunchDefender, StaunchDefenderEffect);
            AddExecutor(ExecutorType.Activate, CardId.RiseToFullHeight, RiseToFullHeightEffect);
            AddExecutor(ExecutorType.Activate, CardId.D2Shield, D2ShieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossCounter, CrossCounterEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        //  OCGCore Callbacks & Hint Handling
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            const long HINTMSG_DISCARD = 501;
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_REMOVE = 503;
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_SET = 510;
            const long HINTMSG_FACEUP = 514;
            const long HINTMSG_TARGET = 551;

            // 1. Discard Cost / Damage Step Discard:
            if (hint == HINTMSG_DISCARD)
            {
                var stronghold = cards.FirstOrDefault(c => c.IsCode(CardId.StrongholdGuardian));
                if (stronghold != null) return new List<ClientCard> { stronghold };

                var rise = cards.FirstOrDefault(c => c.IsCode(CardId.RiseToFullHeight));
                if (rise != null) return new List<ClientCard> { rise };

                var duplicate = cards.GroupBy(c => c.Id).FirstOrDefault(g => g.Count() > 1)?.FirstOrDefault();
                if (duplicate != null) return new List<ClientCard> { duplicate };
            }

            // 2. Destroy Target (Tornado Dragon, Knightmare Phoenix):
            if (hint == HINTMSG_DESTROY)
            {
                var oppBackrow = cards.Where(c => c.Controller == 1 && (c.IsSpell() || c.IsTrap()))
                    .OrderByDescending(c => c.IsFaceup() ? 2 : 1).FirstOrDefault();
                if (oppBackrow != null) return new List<ClientCard> { oppBackrow };

                var oppMonster = cards.Where(c => c.Controller == 1 && c.IsMonster())
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppMonster != null) return new List<ClientCard> { oppMonster };
            }

            // 3. Banish Target (S:P Little Knight):
            if (hint == HINTMSG_REMOVE)
            {
                var oppDangerous = cards.Where(c => c.Controller == 1)
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppDangerous != null) return new List<ClientCard> { oppDangerous };
            }

            // 4. Search / Add to Hand (Reinforcement of the Army):
            // Priority: Big Shield Gardna (Ace) > Mid Shield Gardna
            if (hint == HINTMSG_ATOHAND)
            {
                var searchPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.BigShieldGardna) && !Bot.HasInHand(CardId.BigShieldGardna)) return 1;
                    if (c.IsCode(CardId.BigShieldGardna)) return 2;
                    if (c.IsCode(CardId.MidShieldGardna) && !Bot.HasInHand(CardId.MidShieldGardna)) return 3;
                    return 10;
                }).ToList();

                if (searchPicks.Count >= min)
                    return searchPicks.Take(min).ToList();
            }

            // 5. Set from Deck (Lord of the Heavenly Prison):
            // Stun & Reflect Priority: Skill Drain > Summon Limit > Battle Mania > Anti-Spell > Gozen > D2 Shield > Solemn Judgment
            if (hint == HINTMSG_SET)
            {
                var setPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.SkillDrain) && !Bot.HasInSpellZone(CardId.SkillDrain)) return 1;
                    if (c.IsCode(CardId.SummonLimit) && !Bot.HasInSpellZone(CardId.SummonLimit)) return 2;
                    if (c.IsCode(CardId.BattleMania) && !Bot.HasInSpellZone(CardId.BattleMania)) return 3;
                    if (c.IsCode(CardId.AntiSpellFragrance) && !Bot.HasInSpellZone(CardId.AntiSpellFragrance)) return 4;
                    if (c.IsCode(CardId.GozenMatch) && !Bot.HasInSpellZone(CardId.GozenMatch)) return 5;
                    if (c.IsCode(CardId.D2Shield) && !Bot.HasInSpellZone(CardId.D2Shield)) return 6;
                    if (c.IsCode(CardId.SolemnJudgment)) return 7;
                    if (c.IsCode(CardId.CrossCounter)) return 8;
                    if (c.IsCode(CardId.RiseToFullHeight)) return 9;
                    if (c.IsCode(CardId.SolemnStrike)) return 10;
                    return 20;
                }).ToList();

                if (setPicks.Count >= min)
                    return setPicks.Take(min).ToList();
            }

            // 6. Target Selection (D2 Shield / Rise to Full Height / Staunch Defender):
            // Always target Big Shield Gardna first!
            if (hint == HINTMSG_TARGET || hint == HINTMSG_FACEUP)
            {
                var gardnaTarget = cards.FirstOrDefault(c => c.Controller == 0 && c.IsCode(CardId.BigShieldGardna) && c.IsFaceup());
                if (gardnaTarget != null) return new List<ClientCard> { gardnaTarget };

                var otherDefTarget = cards.FirstOrDefault(c => c.Controller == 0 && c.IsDefense() && c.IsFaceup());
                if (otherDefTarget != null) return new List<ClientCard> { otherDefTarget };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Big Shield Gardna: ALWAYS Defense Position! (2600 DEF)
            if (cardId == CardId.BigShieldGardna)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Mid Shield Gardna & Fossil Dyna: Defense Position
            if (cardId == CardId.MidShieldGardna || cardId == CardId.FossilDynaPachycephalo)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Lord of the Heavenly Prison: 3000 DEF wall
            if (cardId == CardId.LordOfTheHeavenlyPrison)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Bagooska: ALWAYS Defense Position!
            if (cardId == CardId.Bagooska)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Offensive Beatsticks / Finishers: Attack Position
            if (cardId == CardId.UtopiaTheLightning || cardId == CardId.HeroicChampionExcalibur ||
                cardId == CardId.Number39Utopia || cardId == CardId.TornadoDragon)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Executor Action Handlers & Stun Strategic Logic
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            return true;
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            return true;
        }

        private bool SkillDrainEffect()
        {
            // Activate Skill Drain if we don't already control one and have LP > 1000
            if (Bot.LifePoints <= 1000) return false;
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
        }

        private bool SummonLimitEffect()
        {
            // Activate Summon Limit to choke opponent's summons to max 2 per turn
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SummonLimit));
        }

        private bool AntiSpellFragranceEffect()
        {
            // Freeze opponent spell cards (they must set them for a turn)
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AntiSpellFragrance));
        }

        private bool GozenMatchEffect()
        {
            // Lock players to 1 Attribute (all our monsters are EARTH)
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GozenMatch));
        }

        private bool StrongholdGuardianEffect()
        {
            // Damage Step Handtrap (+1500 DEF): Activates from HAND, so it completely ignores Skill Drain!
            if (Duel.Phase != DuelPhase.BattleStep && Duel.Phase != DuelPhase.Damage) return false;
            return Bot.BattlingMonster != null && Bot.BattlingMonster.IsDefense();
        }

        private bool HarpiesFeatherDusterEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool RotAEffect()
        {
            return true;
        }

        private bool LordOfTheHeavenlyPrisonEffect()
        {
            // Effect 1: Hand reveal in Main Phase 1 (protects all Set cards on field from destruction!)
            if (Card.Location == CardLocation.Hand && !_lordRevealedThisTurn)
            {
                if (Duel.Phase == DuelPhase.Main1 && Duel.Player == 0)
                {
                    _lordRevealedThisTurn = true;
                    return true;
                }
            }

            // Effect 2: When a Set Spell/Trap is activated: Special Summon itself and Set 1 Spell/Trap from Deck!
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }

            return false;
        }

        private bool MidShieldGardnaEffect()
        {
            // Flip itself face-down once per turn in Main Phase to reset its spell negation trap!
            return Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.IsDefense();
        }

        private bool FossilDynaMonsterSet()
        {
            // Set face-down as a surprise flip board wipe!
            return !Bot.HasInMonstersZone(CardId.FossilDynaPachycephalo);
        }

        private bool FossilDynaSummon()
        {
            // Normal Summon in Attack/Defense if opponent has Special Summoned monsters or we need immediate lock
            return Enemy.GetMonsterCount() > 0 && !Bot.HasInMonstersZone(CardId.FossilDynaPachycephalo);
        }

        private bool FallbackSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool UtopiaLightningSummon()
        {
            return Bot.HasInMonstersZone(CardId.Number39Utopia);
        }

        private bool UtopiaLightningEffect()
        {
            return Card.Attack < 5000 && Bot.BattlingMonster == Card;
        }

        private bool ExcaliburSummon()
        {
            int warriorCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.Warrior));
            return warriorCount >= 2 && Util.IsTurn1OrMain2() == false;
        }

        private bool ExcaliburEffect()
        {
            return Card.Attack < 4000;
        }

        private bool UtopiaSummon()
        {
            int level4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4);
            return level4Count >= 2 && Bot.HasInExtra(CardId.UtopiaTheLightning);
        }

        private bool CowboySummon()
        {
            if (Duel.Phase == DuelPhase.Main2 && Enemy.LifePoints <= 800) return true;
            return false;
        }

        private bool CowboyEffect()
        {
            return Duel.Phase == DuelPhase.Main2;
        }

        private bool BagooskaSummon()
        {
            int level4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4);
            return level4Count >= 2 && Enemy.GetMonsterCount() >= 2 && !Bot.HasInMonstersZone(CardId.Bagooska);
        }

        private bool TornadoDragonEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool AbyssDwellerEffect()
        {
            return Duel.Player == 1;
        }

        private bool KnightmarePhoenixEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool SpLittleKnightEffect()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool BattleManiaEffect()
        {
            // Do not force attack if enemy controls damage reflect monsters (Mikanko or Daigusto Sphreez)
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(29552709) || c.HasSetcode(0x18d))))
                return false;

            // Activate in opponent's Standby Phase if we control Big Shield Gardna or Defense walls
            bool haveGardna = Bot.GetMonsters().Any(c => c != null &&
                (c.IsCode(CardId.BigShieldGardna) || c.IsCode(CardId.MidShieldGardna) || c.IsFacedown()));
            return Duel.Player == 1 && Duel.Phase == DuelPhase.Standby && haveGardna && Enemy.GetMonsterCount() > 0;
        }

        private bool StaunchDefenderEffect()
        {
            if (Duel.Player != 1) return false;

            // Select face-up Big Shield Gardna (or high DEF wall)
            ClientCard gardna = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.BigShieldGardna) || c.IsCode(CardId.MidShieldGardna) || c.IsCode(CardId.LordOfTheHeavenlyPrison)) && c.IsDefense());
            if (gardna == null) return false;

            bool hasDefTrap = Bot.GetSpells().Any(c => c != null && c.IsFacedown() &&
                (c.IsCode(CardId.D2Shield) || c.IsCode(CardId.CrossCounter)));
            bool hasHandTrap = Bot.HasInHand(CardId.StrongholdGuardian);

            if (!hasDefTrap && !hasHandTrap && Enemy.GetMonsterCount() > 1) return false;

            AI.SelectCard(gardna);
            return true;
        }

        private bool RiseToFullHeightEffect()
        {
            // Field effect: double DEF of Big Shield Gardna (2600 -> 5200 DEF!)
            if (Card.Location == CardLocation.SpellZone)
            {
                ClientCard target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                    (c.IsCode(CardId.BigShieldGardna) || c.IsCode(CardId.MidShieldGardna)) && c.IsDefense());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }

            // GY effect: banish to lock opponent attacks strictly onto Big Shield Gardna!
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main1))
                {
                    ClientCard target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.BigShieldGardna));
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool D2ShieldEffect()
        {
            // Double DEF of Big Shield Gardna (2600 original -> 5200 DEF!)
            ClientCard gardna = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.BigShieldGardna) && c.IsDefense());
            if (gardna != null)
            {
                AI.SelectCard(gardna);
                return true;
            }

            // Secondary target: Mid Shield Gardna (1800 -> 3600 DEF)
            ClientCard other = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.MidShieldGardna) && c.IsDefense());
            if (other != null)
            {
                AI.SelectCard(other);
                return true;
            }

            return false;
        }

        private bool CrossCounterEffect()
        {
            // Battle step when opponent attacks our defense monster with higher DEF: double battle damage and pop attacker!
            if (Duel.Player != 1 || Enemy.BattlingMonster == null || Bot.BattlingMonster == null) return false;
            return Bot.BattlingMonster.IsDefense() && Bot.BattlingMonster.Defense > Enemy.BattlingMonster.Attack;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            // Big Shield Gardna: If it got changed to Attack Position (100 ATK) after being attacked,
            // IMMEDIATELY switch it back to Defense Position (2600 DEF)!
            if (Card.IsCode(CardId.BigShieldGardna))
            {
                if (Card.IsAttack() && Card.IsFaceup()) return true;
                return false;
            }

            // Mid Shield Gardna: NEVER stay in Attack Position!
            if (Card.IsCode(CardId.MidShieldGardna))
            {
                if (Card.IsAttack() && Card.IsFaceup()) return true;
                return false;
            }

            // Lord of the Heavenly Prison: Keep in DEF (3000 DEF wall) unless we have lethal push
            if (Card.IsCode(CardId.LordOfTheHeavenlyPrison))
            {
                if (Card.IsDefense() && Enemy.GetMonsterCount() > 0 && !Util.IsOneEnemyBetterThanValue(3000, false))
                    return false;
            }

            return DefaultMonsterRepos();
        }
    }
}
