// ============================================================================
// CARD AUDIT — AztecRock (Stone Statue of the Aztecs Defense OTK / Rock Stun Fortress)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Stone Statue of the Aztecs  | Monster L4   | No   | No    | None    | 2000 DEF; double battle damage when attacked  | Set face-down or summon in DEF as OTK anvil   | Attack position                             |
// | Fossil Dyna Pachycephalo    | Monster L4   | No   | No    | None    | Neither player can Special Summon; flips wipe | Normal summon / Ties target to floodgate opp  | When we need to special summon              |
// | Koa'ki Meiru Guardian       | Monster L4   | No   | No    | Tribute | Tribute self: negate monster effect & destroy | Disrupt opponent monster effect               | Opponent activates negligible effect        |
// | Koa'ki Meiru Wall           | Monster L4   | No   | No    | Tribute | Tribute self: negate Spell card & destroy     | Disrupt opponent Spell card (Duster/Raigeki)  | Trivial spell                               |
// | Block Dragon                | Monster L8   | No   | Yes   | Banish3 | Rocks cannot be destroyed by card effects!    | SS from hand/GY to grant full board immunity  | Not enough EARTH monsters in GY             |
// | Lord of the Heavenly Prison | Monster L10  | Yes  | Yes   | Reveal  | Set cards cannot be destroyed by effects!     | Hand reveal in MP1; SS 3000/3000 & Set trap  | Already revealed this turn                  |
// | Stronghold Guardian         | Monster L4   | No   | No    | Discard | Damage Step: attacked DEF monster gets +1500  | Discard when Aztec is attacked for +1500 DEF  | Not in Damage Step                          |
// | Ash Blossom & Joyous Spring | Monster L3   | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent searches or special summons from deck| Already used this turn                      |
// | Ties of the Brethren        | Spell Normal | Yes  | Yes   | 2000 LP | Target Lv4 Rock -> SS 2 Lv4 Rocks from deck   | Target Lv4 Rock to bring Aztec + Guardian/Dyna| Bot LP <= 2000 or no valid targets          |
// | Canyon                      | Spell Field  | No   | No    | None    | Double battle damage when Defense Rock attacked| Activate to stack x2 damage on top of Aztec  | Already active on field                     |
// | Terraforming                | Spell Normal | No   | No    | None    | Add 1 Field Spell from deck to hand           | Search Canyon                                 | Canyon already in hand or on field          |
// | Harpie's Feather Duster     | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls | Wipe opponent backrow before attacks/combos   | Opponent has 0 Spells/Traps                 |
// | Pot of Extravagance         | Spell Normal | Yes  | Yes   | Banish6 | Banish 6 Extra Deck face-down -> Draw 2 cards | Activate at start of Main Phase 1 for draw 2  | Not start of MP1 or already drawn           |
// | D2 Shield                   | Trap Normal  | No   | No    | None    | Target face-up DEF monster: DEF becomes double| Target Aztec when attacked (DEF becomes 4000!)| No face-up DEF monsters on field            |
// | Cross Counter               | Trap Normal  | No   | No    | None    | If DEF higher than ATK: double battle damage! | Battle Step: double battle damage again (x4!) | DEF is not higher than attacker's ATK       |
// | Battle Mania                | Trap Normal  | No   | No    | None    | Opp Standby: All opp monsters to ATK & MUST atk| Force opponent to attack into Aztec fortress  | Opponent controls 0 monsters                |
// | Rise to Full Height         | Trap Normal  | No   | Yes   | None    | Double DEF & opp can only attack target mon   | Target Aztec to double DEF and lock attacks   | Aztec not on field                          |
// | Solemn Judgment             | Trap Counter | No   | No    | Half LP | Negate Summon or Spell/Trap card and destroy  | Protect backrow / Aztec / Canyon from wipes   | Trivial cards                               |
// | Solemn Strike               | Trap Counter | No   | No    | 1500 LP | Negate Monster effect or Special Summon & pop | Protect monsters from destruction effects     | Negligible effects                          |
// | Gallant Granite (Extra)     | Xyz Rank 4   | Yes  | Yes   | Detach1 | Add 1 Rock monster from Deck to hand          | Search Aztec / Lord of Heavenly / Block Dragon| Already searched this turn                  |
// | Tornado Dragon (Extra)      | Xyz Rank 4   | Yes  | No    | Detach1 | Quick: Target 1 Spell/Trap and destroy it      | Destroy opponent backrow / Field Spells       | Opponent has 0 Spells/Traps                 |
// | Abyss Dweller (Extra)       | Xyz Rank 4   | Yes  | No    | Detach1 | Quick: Opponent cannot activate effects in GY  | Shut down GY-heavy decks (Mathmech, Tear, etc)| Opponent has no GY engine                   |
// | Bagooska (Extra)            | Xyz Rank 4   | No   | No    | Detach1 | All monsters in DEF; negate effects in DEF    | Emergency stall if combo disrupted            | When we are ready to OTK with Aztec         |
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
    [Deck("AztecRock", "AztecRock")]
    public class AztecRockExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int StoneStatueOfTheAztecs = 31812496;
            public const int FossilDynaPachycephalo = 42009836;
            public const int KoakiMeiruGuardian = 45041488;
            public const int KoakiMeiruWall = 66816282;
            public const int BlockDragon = 94689206;
            public const int LordOfTheHeavenlyPrison = 9822220;
            public const int StrongholdGuardian = 23535429;
            public const int AshBlossom = 14558128;

            // Spells
            public const int TiesOfTheBrethren = 40450317;
            public const int Canyon = 28120197;
            public const int Terraforming = 73628505;
            public const int HarpiesFeatherDuster = 18144506;
            public const int PotOfExtravagance = 49238328;

            // Traps
            public const int D2Shield = 71249758;
            public const int BattleMania = 31245780;
            public const int CrossCounter = 37083210;
            public const int RiseToFullHeight = 19254117;
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 40605147;

            // Extra Deck
            public const int GallantGranite = 32530043;
            public const int Bagooska = 90590303;
            public const int AbyssDweller = 21044178;
            public const int TornadoDragon = 6983839;
            public const int SpLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.StoneStatueOfTheAztecs,
            CardId.BlockDragon,
            CardId.LordOfTheHeavenlyPrison,
            CardId.FossilDynaPachycephalo,
            CardId.GallantGranite
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

        public AztecRockExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Counter Traps & High Priority Negates (Protect Board & Backrow) ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.KoakiMeiruGuardian, KoakiGuardianEffect);
            AddExecutor(ExecutorType.Activate, CardId.KoakiMeiruWall, KoakiWallEffect);

            // ── 2. Damage Step Handtrap Defense Booster ──
            AddExecutor(ExecutorType.Activate, CardId.StrongholdGuardian, StrongholdGuardianEffect);

            // ── 3. Backrow Destruction & Draw Power ──
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming);
            AddExecutor(ExecutorType.Activate, CardId.Canyon, CanyonEffect);

            // ── 4. Lord of the Heavenly Prison (Protects all Set cards from destruction!) ──
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, LordOfTheHeavenlyPrisonEffect);

            // ── 5. Block Dragon (Grants Full Destruction Immunity to All Rocks!) ──
            AddExecutor(ExecutorType.SpSummon, CardId.BlockDragon, BlockDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlockDragon);

            // ── 6. Normal Summons & Sets ──
            // If we have Ties of the Brethren: Normal Summon a Level 4 Rock
            AddExecutor(ExecutorType.Summon, CardId.KoakiMeiruGuardian, RockSummon);
            AddExecutor(ExecutorType.Summon, CardId.KoakiMeiruWall, RockSummon);
            AddExecutor(ExecutorType.Summon, CardId.FossilDynaPachycephalo, FossilDynaSummon);

            // Stone Statue of the Aztecs: Prefer MonsterSet face-down unless we have Ties of the Brethren!
            AddExecutor(ExecutorType.MonsterSet, CardId.StoneStatueOfTheAztecs, AztecMonsterSet);
            AddExecutor(ExecutorType.Summon, CardId.StoneStatueOfTheAztecs, AztecSummon);

            // ── 7. Ties of the Brethren (Calls Aztec + Fossil Dyna + Guardian from Deck) ──
            AddExecutor(ExecutorType.Activate, CardId.TiesOfTheBrethren, TiesEffect);

            // ── 8. Extra Deck Tools (Gallant Granite, Tornado Dragon, Abyss Dweller) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GallantGranite, GallantGraniteSummon);
            AddExecutor(ExecutorType.Activate, CardId.GallantGranite, GallantGraniteEffect);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpLittleKnight, SpLittleKnightEffect);

            // ── 9. Battle Force & Reflect Traps (Battle Mania / D2 Shield / Cross Counter) ──
            AddExecutor(ExecutorType.Activate, CardId.BattleMania, BattleManiaEffect);
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

            const long HINTMSG_RELEASE = 500;
            const long HINTMSG_DISCARD = 501;
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_REMOVE = 503;
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_SPSUMMON = 509;
            const long HINTMSG_SET = 510;
            const long HINTMSG_FACEUP = 514;
            const long HINTMSG_REMOVEXYZ = 519;
            const long HINTMSG_TARGET = 551;

            // 1. Tribute Cost (Koa'ki Meiru monsters): Tribute self
            if (hint == HINTMSG_RELEASE)
            {
                var koaki = cards.FirstOrDefault(c => c.Controller == 0 &&
                    (c.IsCode(CardId.KoakiMeiruGuardian) || c.IsCode(CardId.KoakiMeiruWall)));
                if (koaki != null) return new List<ClientCard> { koaki };
            }

            // 2. Discard Cost / Damage Step Discard: Stronghold Guardian
            if (hint == HINTMSG_DISCARD)
            {
                var stronghold = cards.FirstOrDefault(c => c.IsCode(CardId.StrongholdGuardian));
                if (stronghold != null) return new List<ClientCard> { stronghold };
            }

            // 3. Destroy Target (Tornado Dragon, Knightmare Phoenix):
            // Destroy opponent backrow / continuous spells/traps
            if (hint == HINTMSG_DESTROY)
            {
                var oppBackrow = cards.Where(c => c.Controller == 1 && (c.IsSpell() || c.IsTrap()))
                    .OrderByDescending(c => c.IsFaceup() ? 2 : 1).FirstOrDefault();
                if (oppBackrow != null) return new List<ClientCard> { oppBackrow };

                var oppMonster = cards.Where(c => c.Controller == 1 && c.IsMonster())
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppMonster != null) return new List<ClientCard> { oppMonster };
            }

            // 4. Banish Cost (Block Dragon): Banish 3 EARTH monsters from GY/Hand
            if (hint == HINTMSG_REMOVE && cards.All(c => c.Location == CardLocation.Grave || c.Location == CardLocation.Hand))
            {
                var earthMonsters = cards.Where(c => c.HasAttribute(CardAttribute.Earth))
                    .OrderBy(c => c.Location == CardLocation.Grave ? 1 : 2) // GY first
                    .ThenBy(c => IsAceCard(c) ? 9 : 1)
                    .ToList();
                if (earthMonsters.Count >= min)
                    return earthMonsters.Take(min).ToList();
            }

            // 5. Search / Add to Hand (Gallant Granite / Block Dragon):
            // Priority: Stone Statue of the Aztecs > Lord of the Heavenly Prison > Block Dragon > Fossil Dyna
            if (hint == HINTMSG_ATOHAND)
            {
                var searchPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.StoneStatueOfTheAztecs) && !Bot.HasInHand(CardId.StoneStatueOfTheAztecs) && !Bot.HasInMonstersZone(CardId.StoneStatueOfTheAztecs)) return 1;
                    if (c.IsCode(CardId.LordOfTheHeavenlyPrison) && !Bot.HasInHand(CardId.LordOfTheHeavenlyPrison) && !Bot.HasInMonstersZone(CardId.LordOfTheHeavenlyPrison)) return 2;
                    if (c.IsCode(CardId.BlockDragon) && !Bot.HasInHand(CardId.BlockDragon) && !Bot.HasInMonstersZone(CardId.BlockDragon)) return 3;
                    if (c.IsCode(CardId.FossilDynaPachycephalo) && !Bot.HasInMonstersZone(CardId.FossilDynaPachycephalo)) return 4;
                    if (c.IsCode(CardId.KoakiMeiruGuardian)) return 5;
                    return 10;
                }).ToList();

                if (searchPicks.Count >= min)
                    return searchPicks.Take(min).ToList();
            }

            // 6. Special Summon (Ties of the Brethren):
            // Call Aztec + Fossil Dyna + Guardian from deck!
            if (hint == HINTMSG_SPSUMMON)
            {
                var tiesPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.StoneStatueOfTheAztecs)) return 1;
                    if (c.IsCode(CardId.FossilDynaPachycephalo)) return 2;
                    if (c.IsCode(CardId.KoakiMeiruGuardian)) return 3;
                    if (c.IsCode(CardId.KoakiMeiruWall)) return 4;
                    return 10;
                }).ToList();

                if (tiesPicks.Count >= min)
                    return tiesPicks.Take(min).ToList();
            }

            // 7. Set Trap/Spell from Deck (Lord of the Heavenly Prison):
            // Priority: Canyon (if not active) > Battle Mania > D2 Shield > Solemn Judgment > Solemn Strike
            if (hint == HINTMSG_SET)
            {
                var setPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.Canyon) && !Bot.HasInSpellZone(CardId.Canyon)) return 1;
                    if (c.IsCode(CardId.BattleMania) && !Bot.HasInSpellZone(CardId.BattleMania) && !Bot.HasInHand(CardId.BattleMania)) return 2;
                    if (c.IsCode(CardId.D2Shield) && !Bot.HasInSpellZone(CardId.D2Shield) && !Bot.HasInHand(CardId.D2Shield)) return 3;
                    if (c.IsCode(CardId.SolemnJudgment)) return 4;
                    if (c.IsCode(CardId.SolemnStrike)) return 5;
                    if (c.IsCode(CardId.CrossCounter)) return 6;
                    return 10;
                }).ToList();

                if (setPicks.Count >= min)
                    return setPicks.Take(min).ToList();
            }

            // 8. Target Selection for D2 Shield / Rise to Full Height / S:P Little Knight:
            if (hint == HINTMSG_TARGET || hint == HINTMSG_FACEUP)
            {
                // Target Stone Statue of the Aztecs for DEF doubling
                var aztec = cards.FirstOrDefault(c => c.Controller == 0 && c.IsCode(CardId.StoneStatueOfTheAztecs));
                if (aztec != null) return new List<ClientCard> { aztec };

                // Target opponent threat for removal / banish
                var oppThreat = cards.Where(c => c.Controller == 1)
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppThreat != null) return new List<ClientCard> { oppThreat };
            }

            // 9. Detach Xyz Material:
            if (hint == HINTMSG_REMOVEXYZ)
            {
                var mat = cards.OrderBy(c => IsAceCard(c) ? 9 : 1).FirstOrDefault();
                if (mat != null) return new List<ClientCard> { mat };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Stone Statue of the Aztecs: ALWAYS Defense Position!
            if (cardId == CardId.StoneStatueOfTheAztecs)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Fossil Dyna: Defense if possible
            if (cardId == CardId.FossilDynaPachycephalo)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            // Lord of the Heavenly Prison: 3000 DEF wall or 3000 ATK
            if (cardId == CardId.LordOfTheHeavenlyPrison)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Beatsticks / Xyz: Attack Position
            if (cardId == CardId.BlockDragon || cardId == CardId.GallantGranite ||
                cardId == CardId.KoakiMeiruGuardian || cardId == CardId.TornadoDragon)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller == 0) return false;
            var last = Util.GetLastChainCard();
            // Protect our backrow / Canyon / Block Dragon / Aztec against board breakers and mass wipes
            return last.IsSpell() || last.IsTrap() || last.Id == 72989439 || last.IsMonster();
        }

        private bool SolemnStrikeEffect()
        {
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller == 0) return false;
            var last = Util.GetLastChainCard();
            // Negate monster effect (especially destruction effects targeting our board) or Special Summon
            return last.IsMonster();
        }

        private bool KoakiGuardianEffect()
        {
            return Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1 && Util.GetLastChainCard().IsMonster();
        }

        private bool KoakiWallEffect()
        {
            // Negate opponent Spell cards (Raigeki, Feather Duster, Lightning Storm, Dark Ruler, Super Poly)
            return Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1 && Util.GetLastChainCard().IsSpell();
        }

        private bool StrongholdGuardianEffect()
        {
            // Damage Step: activate when our Defense monster (especially Aztec) is attacked!
            if (Duel.Phase != DuelPhase.BattleStep && Duel.Phase != DuelPhase.Damage) return false;
            return Bot.BattlingMonster != null && Bot.BattlingMonster.IsDefense();
        }

        private bool HarpiesFeatherDusterEffect()
        {
            // Destroy all opponent backrow if they control any Spells/Traps
            return Enemy.GetSpellCount() > 0;
        }

        private bool CanyonEffect()
        {
            // If Set on field (e.g. by Lord of the Heavenly Prison), flip it face-up!
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown()) return true;
            // If in hand, activate if we don't already control a face-up Canyon
            return !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Canyon));
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

        private bool BlockDragonSummon()
        {
            int earthCount = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Earth)) +
                             Bot.Hand.Count(c => c != null && c.HasAttribute(CardAttribute.Earth) && c.Id != CardId.BlockDragon);
            return earthCount >= 3 && !Bot.HasInMonstersZone(CardId.BlockDragon);
        }

        private bool TiesEffect()
        {
            if (Bot.LifePoints <= 2000) return false;
            // Target a face-up Level 4 Rock
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.Rock));
        }

        private bool RockSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool FossilDynaSummon()
        {
            return !Bot.HasInMonstersZone(CardId.FossilDynaPachycephalo);
        }

        private bool AztecMonsterSet()
        {
            // If we have Ties of the Brethren in hand, do NOT set face-down! Normal Summon in DEF instead so Ties can activate!
            if (Bot.HasInHand(CardId.TiesOfTheBrethren) && Bot.LifePoints > 2000) return false;
            return true;
        }

        private bool AztecSummon()
        {
            // Normal Summon Aztec (which OnSelectPosition will put in DEF) if we have Ties of the Brethren!
            return Bot.HasInHand(CardId.TiesOfTheBrethren) && Bot.LifePoints > 2000;
        }

        private bool GallantGraniteSummon()
        {
            // Xyz summon Gallant Granite if we need to search Aztec, Lord of Heavenly Prison, or Block Dragon
            return !Bot.HasInHand(CardId.StoneStatueOfTheAztecs) && !Bot.HasInMonstersZone(CardId.StoneStatueOfTheAztecs) ||
                   !Bot.HasInHand(CardId.LordOfTheHeavenlyPrison);
        }

        private bool GallantGraniteEffect()
        {
            return true;
        }

        private bool TornadoDragonEffect()
        {
            // Quick effect: target and destroy 1 opponent Spell/Trap card
            return Enemy.GetSpellCount() > 0;
        }

        private bool AbyssDwellerEffect()
        {
            // Quick effect: prevent opponent from activating GY effects
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
            // Activate only during opponent's Standby Phase
            if (Duel.Player != 1 || Duel.Phase != DuelPhase.Standby)
                return false;

            if (Enemy.GetMonsterCount() == 0)
                return false;

            // Do not force attack if enemy controls damage reflect monsters (Mikanko, Daigusto Sphreez, Yubel, Amazoness Swords Woman)
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.IsCode(29552709) || c.HasSetcode(0x18d) || c.IsCode(73915051) ||
                 c.IsCode(78371393) || c.IsCode(4779091) || c.IsCode(31764782))))
            {
                return false;
            }

            // Do not activate if we control face-up Bagooska in Defense
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsDefense() && c.IsCode(CardId.Bagooska)))
                return false;

            var myMonsters = Bot.GetMonsters().Where(c => c != null).ToList();
            if (myMonsters.Count == 0)
                return false;

            // Guard against vulnerable monsters
            bool hasAttackRedirect = Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.RiseToFullHeight));
            bool hasVulnerableAttack = myMonsters.Any(c => c.IsFaceup() && c.IsAttack() && c.Attack < 2000);
            if (hasVulnerableAttack && !hasAttackRedirect)
                return false;

            bool hasZeroDef = myMonsters.Any(c => c.IsFaceup() && c.IsDefense() && c.Defense <= 0);
            if (hasZeroDef && !hasAttackRedirect)
                return false;

            // Find best defense wall
            int bestDef = 0;
            ClientCard bestWall = null;
            foreach (var m in myMonsters)
            {
                if (m.IsFacedown())
                {
                    int def = m.Data?.Defense ?? 0;
                    if (def > bestDef)
                    {
                        bestDef = def;
                        bestWall = m;
                    }
                }
                else if (m.IsDefense())
                {
                    if (m.Defense > 0 && m.Defense > bestDef)
                    {
                        bestDef = m.Defense;
                        bestWall = m;
                    }
                }
            }

            if (bestWall == null || bestDef < 1800)
                return false;

            bool hasD2 = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.D2Shield));
            bool hasStronghold = Bot.HasInHand(CardId.StrongholdGuardian);
            bool hasRise = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.RiseToFullHeight));

            int maxPotentialDef = bestDef;
            if (hasD2 || hasRise) maxPotentialDef = Math.Max(maxPotentialDef, bestDef * 2);
            if (hasStronghold) maxPotentialDef += 1500;

            int oppMaxAtk = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .Select(c => c.Attack)
                .DefaultIfEmpty(0)
                .Max();

            if (oppMaxAtk == 0 && Enemy.GetMonsterCount() > 0)
                oppMaxAtk = 1500;

            if (oppMaxAtk >= maxPotentialDef)
                return false;

            return true;
        }

        private bool RiseToFullHeightEffect()
        {
            // Field effect: only activate in opponent's Battle Step when our defense monster is attacked!
            if (Card.Location == CardLocation.SpellZone)
            {
                if (Duel.Player != 1 || (Duel.Phase != DuelPhase.BattleStep && Duel.Phase != DuelPhase.Damage))
                    return false;

                ClientCard target = Bot.BattlingMonster;
                if (target == null || !target.IsFaceup() || !target.IsDefense() || target.Defense <= 0)
                    return false;

                if (!target.IsCode(CardId.StoneStatueOfTheAztecs) && !target.IsCode(CardId.LordOfTheHeavenlyPrison))
                    return false;

                ClientCard attacker = Enemy.BattlingMonster;
                if (attacker != null && target.Defense * 2 > attacker.Attack)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }

            // GY effect: lock attacks to our high-DEF wall
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main1))
                {
                    ClientCard target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsDefense() && c.Defense >= 1800 &&
                        (c.IsCode(CardId.StoneStatueOfTheAztecs) || c.IsCode(CardId.LordOfTheHeavenlyPrison)));
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
            if (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage ||
                (Duel.Phase == DuelPhase.Standby && Bot.HasInSpellZone(CardId.BattleMania)))
            {
                // Double DEF of Stone Statue of the Aztecs (2000 -> 4000 DEF!)
                ClientCard aztec = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.StoneStatueOfTheAztecs) && c.IsDefense());
                if (aztec != null)
                {
                    AI.SelectCard(aztec);
                    return true;
                }
            }

            return false;
        }

        private bool CrossCounterEffect()
        {
            // Battle step when opponent attacks our defense monster with higher DEF: double battle damage!
            if (Duel.Player != 1 || Enemy.BattlingMonster == null || Bot.BattlingMonster == null) return false;
            return Bot.BattlingMonster.IsDefense() && Bot.BattlingMonster.Defense > Enemy.BattlingMonster.Attack;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            // Stone Statue of the Aztecs: NEVER switch to Attack! Always keep in DEF!
            if (Card.IsCode(CardId.StoneStatueOfTheAztecs))
            {
                if (Card.IsAttack() && Card.IsFaceup()) return true;
                return false;
            }

            // Lord of the Heavenly Prison: Keep in DEF (3000 DEF wall) unless we have a clear lethal push
            if (Card.IsCode(CardId.LordOfTheHeavenlyPrison))
            {
                if (Card.IsDefense() && Enemy.GetMonsterCount() > 0 && !Util.IsOneEnemyBetterThanValue(3000, false))
                    return false;
            }

            return DefaultMonsterRepos();
        }
    }
}
