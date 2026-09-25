// ============================================================================
// CARD AUDIT — Mikanko (Reflect & Force-Attack OTK)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Hu-Li the Jewel Mikanko     | Monster L3   | Yes  | Yes   | None    | 0 ATK; reflect dmg; with equip: untargetable! | Summon & equip to protect entire Mikanko brd | Already have Hu-Li equipped on field        |
// | Ha-Re the Sword Mikanko     | Monster L3   | Yes  | Yes   | None    | 0 ATK; reflect dmg; with equip: search equip  | Normal Summon / SS via Arabesque to search   | Already searched this turn                  |
// | Ni-Ni the Mirror Mikanko    | Monster L3   | Yes  | Yes   | None    | 0 ATK; reflect dmg; opp turn quick snatch mon | Summon / equip to disrupt opponent turn      | No valid target on opponent field           |
// | Ohime the Manifested Mikanko| Ritual L6    | Yes  | Yes   | Discard1| Hand: search Mikanko & discard; Quick: equip  | Search combo piece in hand; Quick equip in GY | Already searched this turn                  |
// | Arahime the Manifested Mikan| Ritual L9    | Yes  | Yes   | None    | GY: equip self to monster on field; bounce mon| In GY: equip to enemy or own Mikanko          | No monsters on field                        |
// | Diviner of the Herald       | Monster L2   | Yes  | Yes   | Send EX | On NS/SS: send Herald from Extra to search Ohime| Normal Summon starter to search Ohime        | No Heralds left in Extra Deck               |
// | Jizukiru, the Star Destroying| Monster L10  | No   | No    | Trib opp| SS to opp field by tributing 1 monster (3300) | Tribute opp monster to setup 10k reflect OTK  | Opponent has 0 monsters                     |
// | Dogoran, the Mad Flame Kaiju| Monster L8   | No   | No    | Trib opp| SS to opp field by tributing 1 monster (3000) | Tribute opp monster to setup reflect OTK      | Opponent has 0 monsters                     |
// | Lava Golem                  | Monster L8   | No   | No    | Trib2opp| SS to opp field by tributing 2 monsters (3000)| Tribute 2 opp monsters; 1000 burn in Standby  | Opponent controls < 2 monsters              |
// | Ash Blossom & Joyous Spring | Monster L3   | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent searches or special summons from deck| Already used this turn                      |
// | Heavenly Gate of the Mikanko| Spell Field  | No   | No    | None    | Opp MUST attack equip mons; locks cards in bat| Activate to force opponent attacks & 2nd hit  | Already active on field                     |
// | Mikanko Water Arabesque     | Spell Equip  | Yes  | Yes   | None    | Bounce equipped monster -> SS Mikanko & equip | Equip to opp monster to bounce & tutor Mikanko| No Mikanko in hand or deck                  |
// | Double-Edged Sword          | Spell Equip  | No   | No    | None    | +2000 ATK; both take dmg; Mikanko reflects x2 | Equip to Kaiju or Mikanko for 10k OTK reflect!| No battle targets                           |
// | Mikanko Fire Dance          | Spell Equip  | Yes  | Yes   | None    | SS Mikanko from HD/GY; SS 1 from opp GY to opp| Revive Mikanko and give opp a monster to hit  | No Mikanko in hand/GY                       |
// | Mikanko Dance-Mayowashidori | Spell Equip  | Yes  | Yes   | None    | Bounce battling monster at damage step; revive| Equip for removal and GY revival              | No monsters on field                        |
// | Mikanko Reflection Rondo    | Spell Equip  | Yes  | Yes   | None    | Take control of equipped monster while Mikanko| Steal opponent boss monster                   | We control 0 Mikanko monsters               |
// | Mikanko Purification Dance  | Spell Equip  | Yes  | Yes   | None    | Bounce 1 card when monster Special Summoned   | Equip for reactive disruption                 | No valid targets                            |
// | The Great Mikanko Ceremony  | Spell Quick  | Yes  | Yes   | None    | SS Mikanko from hand; GY: dump Mikanko from DK| SS Ohime/Hu-Li from hand; dump equip to GY    | No Mikanko in hand                          |
// | Mikanko Kagura              | Spell Ritual | Yes  | Yes   | Tribute | Ritual Summon Mikanko & pop cards/burn 1000 ea| Ritual Summon Ohime / Arahime                 | Not enough tributes                         |
// | Preparation of Rites        | Spell Normal | No   | No    | None    | Add Level 7 or lower Ritual monster from deck | Search Ohime the Manifested Mikanko           | No Ritual monsters in deck                  |
// | Mikanko Rivalry             | Trap Quick   | Yes  | Yes   | None    | Quick: Equip 1 Equip Spell from Deck or GY    | Surprise equip Double-Edged Sword or Rondo    | No valid monsters on field                  |
// | Mikanko Spiritwalk          | Trap Normal  | Yes  | Yes   | None    | Target opp face-up monster; equip to Mikanko  | Disrupt opponent boss on their turn           | We control 0 Mikanko monsters               |
// | Mikanko Promise             | Trap Normal  | Yes  | Yes   | None    | SS Mikanko from hand/deck & equip from HD/GY  | Opponent turn summon Hu-Li for untarget lock  | No Mikanko in hand or deck                  |
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
    [Deck("Mikanko", "Mikanko")]
    public class MikankoExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int HuLiTheJewelMikanko = 6327734;
            public const int HaReTheSwordMikanko = 18377261;
            public const int NiNiTheMirrorMikanko = 54862960;
            public const int OhimeTheManifestedMikanko = 81260679;
            public const int ArahimeTheManifestedMikanko = 75771170;
            public const int DivinerOfTheHerald = 92919429;
            public const int JizukiruTheStarDestroyingKaiju = 63941210;
            public const int DogoranTheMadFlameKaiju = 93332803;
            public const int LavaGolem = 102380;
            public const int AshBlossom = 14558128;

            // Spells
            public const int HeavenlyGateOfTheMikanko = 17255673;
            public const int MikankoWaterArabesque = 43527730;
            public const int DoubleEdgedSword = 41927278;
            public const int MikankoFireDance = 80044027;
            public const int MikankoDanceMayowashidori = 57736667;
            public const int MikankoReflectionRondo = 79912449;
            public const int MikankoPurificationDance = 16433136;
            public const int TheGreatMikankoCeremony = 44649322;
            public const int MikankoKagura = 16310544;
            public const int PreparationOfRites = 96729612;

            // Traps
            public const int MikankoRivalry = 78199891;
            public const int MikankoSpiritwalk = 53174748;
            public const int MikankoPromise = 42705243;

            // Extra Deck
            public const int HeraldOfTheArcLight = 79606837;
            public const int SpLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.OhimeTheManifestedMikanko,
            CardId.HuLiTheJewelMikanko,
            CardId.HaReTheSwordMikanko,
            CardId.NiNiTheMirrorMikanko
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public MikankoExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);

            // ── 2. Kaiju & Lava Golem (Removal + OTK punching bag) ──
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JizukiruTheStarDestroyingKaiju, KaijuSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DogoranTheMadFlameKaiju, KaijuSummon);

            // ── 3. Search & Consistency Engine ──
            AddExecutor(ExecutorType.Activate, CardId.PreparationOfRites, PreparationOfRitesEffect);
            AddExecutor(ExecutorType.Summon, CardId.DivinerOfTheHerald, DivinerSummon);
            AddExecutor(ExecutorType.Activate, CardId.DivinerOfTheHerald, DivinerEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight, HeraldEffect);

            // Ohime search effect in hand
            AddExecutor(ExecutorType.Activate, CardId.OhimeTheManifestedMikanko, OhimeHandEffect);

            // ── 4. Field Spell (Heavenly Gate of the Mikanko - Force Attack Lock) ──
            AddExecutor(ExecutorType.Activate, CardId.HeavenlyGateOfTheMikanko, HeavenlyGateEffect);

            // ── 5. Special Summons & Ceremonies ──
            AddExecutor(ExecutorType.Activate, CardId.TheGreatMikankoCeremony, CeremonyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoKagura, KaguraEffect);

            // ── 6. Equip Spells (Arabesque / Fire Dance / Double-Edged Sword) ──
            // Water Arabesque: Equip to opponent's monster to bounce it and summon Mikanko from deck!
            AddExecutor(ExecutorType.Activate, CardId.MikankoWaterArabesque, ArabesqueEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoubleEdgedSword, DoubleEdgedSwordEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoFireDance, FireDanceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoReflectionRondo, ReflectionRondoEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoDanceMayowashidori, MayowashidoriEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoPurificationDance);

            // ── 7. Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.HaReTheSwordMikanko, MikankoMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.HuLiTheJewelMikanko, MikankoMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.NiNiTheMirrorMikanko, MikankoMonsterSummon);

            // ── 8. Monster Trigger Effects on Equip ──
            AddExecutor(ExecutorType.Activate, CardId.HuLiTheJewelMikanko);
            AddExecutor(ExecutorType.Activate, CardId.HaReTheSwordMikanko);
            AddExecutor(ExecutorType.Activate, CardId.NiNiTheMirrorMikanko, NiNiEffect);
            AddExecutor(ExecutorType.Activate, CardId.OhimeTheManifestedMikanko, OhimeFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArahimeTheManifestedMikanko);

            // ── 9. Traps (Quick-Play Equips / Snatch / Protection) ──
            AddExecutor(ExecutorType.Activate, CardId.MikankoSpiritwalk, SpiritwalkEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoRivalry, RivalryEffect);
            AddExecutor(ExecutorType.Activate, CardId.MikankoPromise, PromiseEffect);

            // ── 10. Spell Sets & Repositions ──
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
            const long HINTMSG_TOGRAVE = 504;
            const long HINTMSG_RTOHAND = 505;
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_SPSUMMON = 509;
            const long HINTMSG_EQUIP = 518;

            // 1. Lava Golem / Kaiju Tribute: Pick highest ATK / threat opponent monsters
            if (hint == HINTMSG_RELEASE && cards.Any(c => c.Controller == 1))
            {
                var oppThreats = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                if (oppThreats.Count >= min)
                    return oppThreats.Take(min).ToList();
            }

            // 2. Discard Cost (Ohime the Manifested Mikanko):
            // Pitch Equip spells (Ohime can equip them directly from GY!) or redundant cards
            if (hint == HINTMSG_DISCARD)
            {
                var discards = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.MikankoFireDance)) return 1;
                    if (c.IsCode(CardId.DoubleEdgedSword)) return 2;
                    if (c.IsCode(CardId.MikankoDanceMayowashidori)) return 3;
                    if (c.IsCode(CardId.ArahimeTheManifestedMikanko)) return 4;
                    if (c.HasType(CardType.Equip)) return 5;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (discards.Count >= min)
                    return discards.Take(min).ToList();
            }

            // 3. Diviner dump: Send Herald of the Arc Light from Extra Deck
            if (hint == HINTMSG_TOGRAVE)
            {
                var herald = cards.FirstOrDefault(c => c.IsCode(CardId.HeraldOfTheArcLight));
                if (herald != null) return new List<ClientCard> { herald };
            }

            // 4. Search / Add to Hand (Preparation of Rites / Herald / Ohime / Ha-Re / Hu-Li):
            if (hint == HINTMSG_ATOHAND)
            {
                var searchPicks = cards.OrderBy(c =>
                {
                    // If searching Ritual monster: Ohime
                    if (c.IsCode(CardId.OhimeTheManifestedMikanko)) return 1;
                    // If searching starter: Arabesque
                    if (c.IsCode(CardId.MikankoWaterArabesque)) return 2;
                    // If searching monster: Hu-Li (untargetable lock)
                    if (c.IsCode(CardId.HuLiTheJewelMikanko) && !Bot.HasInMonstersZone(CardId.HuLiTheJewelMikanko)) return 3;
                    // If searching OTK card: Double-Edged Sword
                    if (c.IsCode(CardId.DoubleEdgedSword)) return 4;
                    // If searching trap: Mikanko Rivalry / Spiritwalk
                    if (c.IsCode(CardId.MikankoSpiritwalk)) return 5;
                    if (c.IsCode(CardId.MikankoRivalry)) return 6;
                    return 10;
                }).ToList();

                if (searchPicks.Count >= min)
                    return searchPicks.Take(min).ToList();
            }

            // 5. Special Summon (Arabesque / Fire Dance / Ceremony / Promise):
            if (hint == HINTMSG_SPSUMMON)
            {
                var spTargets = cards.OrderBy(c =>
                {
                    // Prioritize Hu-Li for the untargetable shield
                    if (c.IsCode(CardId.HuLiTheJewelMikanko) && !Bot.HasInMonstersZone(CardId.HuLiTheJewelMikanko)) return 1;
                    if (c.IsCode(CardId.HaReTheSwordMikanko)) return 2;
                    if (c.IsCode(CardId.NiNiTheMirrorMikanko)) return 3;
                    if (c.IsCode(CardId.OhimeTheManifestedMikanko)) return 4;
                    return 10;
                }).ToList();

                if (spTargets.Count >= min)
                    return spTargets.Take(min).ToList();
            }

            // 6. Equip Target Selection:
            if (hint == HINTMSG_EQUIP)
            {
                // Double-Edged Sword: Equip to opponent's highest ATK monster (e.g. Kaiju) for 10k reflect OTK!
                if (Card != null && Card.IsCode(CardId.DoubleEdgedSword))
                {
                    var oppHighAtk = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone)
                        .OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (oppHighAtk != null) return new List<ClientCard> { oppHighAtk };
                }

                // Water Arabesque: Equip to opponent's monster to bounce it!
                if (Card != null && Card.IsCode(CardId.MikankoWaterArabesque))
                {
                    var oppTarget = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone)
                        .OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (oppTarget != null) return new List<ClientCard> { oppTarget };
                }

                // Generic Equips: Equip to Hu-Li first (to trigger untargetable lock)
                var huli = cards.FirstOrDefault(c => c.Controller == 0 && c.IsCode(CardId.HuLiTheJewelMikanko));
                if (huli != null) return new List<ClientCard> { huli };

                var anyMikanko = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.MonsterZone)
                    .OrderBy(c => c.EquipCards.Count).FirstOrDefault();
                if (anyMikanko != null) return new List<ClientCard> { anyMikanko };
            }

            // 7. Bounce target (Water Arabesque): Bounce opponent monster
            if (hint == HINTMSG_RTOHAND)
            {
                var oppMon = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppMon != null) return new List<ClientCard> { oppMon };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Mikanko monsters should always be in Attack Position (they take 0 damage and reflect it to opp!)
            if (cardId == CardId.HuLiTheJewelMikanko || cardId == CardId.HaReTheSwordMikanko ||
                cardId == CardId.NiNiTheMirrorMikanko || cardId == CardId.OhimeTheManifestedMikanko)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            if (cardId == CardId.DivinerOfTheHerald)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool LavaGolemSummon()
        {
            return Enemy.GetMonsterCount() >= 2;
        }

        private bool KaijuSummon()
        {
            return Enemy.GetMonsterCount() >= 1;
        }

        private bool PreparationOfRitesEffect()
        {
            return Bot.GetRemainingCount(CardId.OhimeTheManifestedMikanko, 1) > 0;
        }

        private bool DivinerSummon()
        {
            return Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.HeraldOfTheArcLight));
        }

        private bool DivinerEffect()
        {
            return Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.HeraldOfTheArcLight));
        }

        private bool HeraldEffect()
        {
            return Bot.GetRemainingCount(CardId.OhimeTheManifestedMikanko, 1) > 0;
        }

        private bool OhimeHandEffect()
        {
            // Activate Ohime in hand: search any Mikanko card, then discard 1 card
            return Card.Location == CardLocation.Hand;
        }

        private bool OhimeFieldEffect()
        {
            // Quick effect: target 1 equip spell in GY, equip to appropriate monster on field
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Equip)) &&
                       Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
            }
            return false;
        }

        private bool HeavenlyGateEffect()
        {
            return !Bot.HasInSpellZone(CardId.HeavenlyGateOfTheMikanko);
        }

        private bool CeremonyEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Special Summon Mikanko monster from hand
                return Bot.Hand.Any(c => c != null && (c.IsCode(CardId.OhimeTheManifestedMikanko) ||
                                                       c.IsCode(CardId.HuLiTheJewelMikanko) ||
                                                       c.IsCode(CardId.HaReTheSwordMikanko)));
            }
            // GY effect: dump Mikanko card to GY
            return true;
        }

        private bool KaguraEffect()
        {
            return Bot.Hand.Any(c => c != null && c.IsCode(CardId.OhimeTheManifestedMikanko));
        }

        private bool ArabesqueEffect()
        {
            // Best play: Equip to opponent's monster, bounce it, and summon Mikanko from deck!
            if (Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            // Otherwise equip to own Mikanko to trigger search
            ClientCard own = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.EquipCards.Count == 0);
            if (own != null)
            {
                AI.SelectCard(own);
                return true;
            }
            return false;
        }

        private bool DoubleEdgedSwordEffect()
        {
            // If opponent controls a monster: equip to their monster (increases battle damage to 10k+)
            if (Enemy.GetMonsterCount() > 0 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup()))
            {
                ClientCard oppTarget = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            // Or equip to our Mikanko
            ClientCard ownMikanko = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (ownMikanko != null)
            {
                AI.SelectCard(ownMikanko);
                return true;
            }
            return false;
        }

        private bool FireDanceEffect()
        {
            // Revive Mikanko from GY or hand
            return Bot.Graveyard.Any(c => c != null && IsAceCard(c)) ||
                   Bot.Hand.Any(c => c != null && IsAceCard(c));
        }

        private bool ReflectionRondoEffect()
        {
            // Steal opponent face-up monster while controlling a Mikanko
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) && Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool MayowashidoriEffect()
        {
            ClientCard own = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (own != null)
            {
                AI.SelectCard(own);
                return true;
            }
            return false;
        }

        private bool MikankoMonsterSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool NiNiEffect()
        {
            // Opponent turn quick effect: take control of 1 face-up monster while equipped
            if (Duel.Player == 1 && Card.EquipCards.Count > 0 && Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SpiritwalkEffect()
        {
            if (Enemy.GetMonsterCount() > 0 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup()))
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool RivalryEffect()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool PromiseEffect()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            // Always keep Mikanko in Attack Position to reflect damage
            if (IsAceCard(Card))
            {
                if (Card.IsDefense() && Card.IsFaceup()) return true;
                return false;
            }
            return DefaultMonsterRepos();
        }
    }
}
