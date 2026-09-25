// ============================================================================
// CARD AUDIT — GOAT_RedEyes (Reasoning Dragon & Metamorphosis)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Red-Eyes Darkness Dragon    | Monster L9   | No   | No    | Trib RE | SS by tributing Red-Eyes B. Dragon; +300/GY Dr| Summon when Red-Eyes is on field              | Opponent has lethal attack and no defense   |
// | Armed Dragon LV7            | Monster L7   | No   | No    | Discard | SS by LV5; discard monster to board wipe mon  | Clear opponent board                          | Hand has 0 monsters                         |
// | Mirage Dragon               | Monster L4   | No   | No    | None    | Opponent cannot activate Traps in Battle Phase| Summon before attacking to shut down traps    | Never                                       |
// | Red-Eyes B. Dragon          | Monster L7   | No   | No    | None    | 2400/2000 Normal dragon; tribute/Inferno target| SS via Chick/Flute/Reasoning                  | Tribute summon without fodder               |
// | Armed Dragon LV5            | Monster L5   | No   | No    | Discard | Discard monster: pop face-up mon; evolve LV7  | Destroy monster or evolve during End Phase    | Hand has 0 monsters                         |
// | Lord of D.                  | Monster L4   | No   | No    | None    | Dragons cannot be targeted; enables Flute     | Normal summon when having Flute + Dragons     | No dragons in hand/field                    |
// | Masked Dragon               | Monster L3   | No   | No    | None    | Battle destroyed: SS Dragon <=1500 ATK from DK| Set as floater; tutor Lv3 / Masked Dragon     | Deck has 0 valid dragons                    |
// | Element Dragon              | Monster L4   | No   | No    | None    | +500 with FIRE; double attack with WIND       | Normal summon beater                          | Never                                       |
// | Cyber-Stein                 | Monster L2   | No   | No    | 5000 LP | Pay 5000 LP: SS Fusion from Extra Deck        | Bring King Dragun / Restrict / Black Skull    | Bot LP <= 5000                              |
// | Black Dragon's Chick        | Monster L1   | No   | No    | Tribute | Send self to GY: SS Red-Eyes B. Dragon from HD| Have Red-Eyes B. Dragon in hand               | Red-Eyes B. Dragon not in hand              |
// | Armed Dragon LV3            | Monster L3   | No   | No    | None    | Standby Phase: evolve into Armed Dragon LV5   | Standby Phase when LV5 is in deck/hand        | Armed Dragon LV5 not in deck/hand           |
// | Spirit Ryu                  | Monster L4   | No   | No    | Discard | Battle Step: discard Dragon for +1000 ATK     | Beat over enemy monster or lethal damage      | Hand has 0 dragons                          |
// | Luster Dragon               | Monster L4   | No   | No    | None    | 1900 ATK Normal dragon beatstick              | Normal summon beatstick                       | Never                                       |
// | My Body as a Shield         | Spell Quick  | No   | No    | 1500 LP | Negate monster destruction effect & destroy it| Opp activates Mirror Force / Torrential / Pop | Bot LP <= 1500                              |
// | The Dragon's Bead           | Trap Cont    | No   | No    | Discard | Discard Dragon: negate Trap targeting Dragon  | Opponent targets Dragon with Trap             | Hand has 0 dragons                          |
// | Different Dimension Capsule | Spell Normal | No   | No    | None    | Banish card from deck; add to hand 2 turns    | Search Darkness Dragon / Dimension Fusion     | Late game when 2 turns is too slow          |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate                               | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3 cards, then discard 2 cards            | Always activate; pitch Dragons for Darkness   | Hand is empty                               |
// | Super Rejuvenation          | Spell Quick  | No   | No    | None    | End Phase: draw cards = Dragons discarded/trib| End Phase after discarding/tributing Dragons  | Not End Phase or 0 dragons discarded        |
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with strong board            | Weak single attack                          |
// | Lightning Vortex            | Spell Normal | No   | No    | Discard | Destroy all face-up monsters opponent controls | Opponent has 2+ face-up monsters              | Hand is empty                               |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent has 0 face-up monsters             |
// | Card Destruction            | Spell Normal | No   | No    | None    | Both players discard hand and draw same amount| Hand has 2+ Dragons to dump to GY             | Hand has high value non-dump spells         |
// | Dragon's Rage               | Trap Cont    | No   | No    | None    | All Dragons inflict piercing battle damage    | Activate to push piercing damage through walls| Already active                              |
// | Inferno Fire Blast          | Spell Normal | No   | No    | None    | Target Red-Eyes B. Dragon: burn opp for 2400  | Red-Eyes B. Dragon on field; burn for 2400    | Red-Eyes B. Dragon not on field             |
// | Giant Trunade               | Spell Normal | No   | No    | None    | Return all Spells/Traps to hand               | Clear backrow before big dragon attacks       | Opponent has 0 backrow                      |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Clear opponent backrow                        | We control King Dragun / face-up equips     |
// | Soul Release                | Spell Normal | No   | No    | None    | Banish up to 5 cards from any GY              | Banish opp GY threats or setup Dimension Fusion| Graveyards are empty                        |
// | Reasoning                   | Spell Normal | No   | No    | None    | Opp calls Level; excavate until monster to SS | Always activate; deck has Lv1,2,3,4,5,7       | Monster zones are full                      |
// | Metamorphosis               | Spell Normal | No   | No    | Tribute | Tribute monster: SS Fusion with same Level    | Lv1->TER, Lv5->Fiend Skull, Lv7->King Dragun  | No valid tribute on field                   |
// | The Flute of Summoning Dragon|Spell Normal | No   | No    | None    | Lord of D. on field: SS up to 2 Dragons from HD| Lord of D. on field and Dragons in hand      | Lord of D. not on field or 0 dragons in hand|
// | Dimension Fusion            | Spell Normal | No   | No    | 2000 LP | Pay 2000 LP: SS all banished monsters         | We have 2+ strong banished Dragons            | Bot LP <= 2000 or no banished monsters      |
// | King Dragun (Extra)         | Fusion L7    | Yes  | No    | None    | Dragons untargetable; SS Dragon from hand/turn| SS via Metamorphosis (Lv7) or Cyber-Stein     | Monster zones full                          |
// | Thousand-Eyes Restrict (Ext)| Fusion L1    | Yes  | No    | None    | Lock attacks; suck 1 opp monster into equip   | SS via Metamorphosis (Lv1) or Cyber-Stein     | Already on field                            |
// | Black Skull Dragon (Extra)  | Fusion L9    | No   | No    | None    | 3200/2500 Dragon boss                         | SS via Metamorphosis (Lv9) or Cyber-Stein OTK| Overkill when Restrict/King is better       |
// | Fiend Skull Dragon (Extra)  | Fusion L5    | No   | No    | None    | 2000/1200; Negates Flip effects; Trap immune  | SS via Metamorphosis (Lv5 Armed Dragon)       | Overkill                                    |
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
    [Deck("GOAT_RedEyes", "GOAT_RedEyes")]
    public class GOAT_RedEyesExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int RedEyesDarknessDragon = 96561011;
            public const int ArmedDragonLv7 = 73879377;
            public const int MirageDragon = 15960641;
            public const int RedEyesBlackDragon = 74677422;
            public const int ArmedDragonLv5 = 46384672;
            public const int LordOfD = 17985575;
            public const int MaskedDragon = 39191307;
            public const int ElementDragon = 30314994;
            public const int CyberStein = 69015963;
            public const int BlackDragonsChick = 36262024;
            public const int ArmedDragonLv3 = 980973;
            public const int SpiritRyu = 67957315;
            public const int LusterDragon = 11091375;

            // Spells & Traps
            public const int MyBodyAsAShield = 69279219;
            public const int TheDragonsBead = 92408984;
            public const int DifferentDimensionCapsule = 11961740;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int SuperRejuvenation = 27770341;
            public const int MirrorForce = 44095762;
            public const int LightningVortex = 69162969;
            public const int SnatchSteal = 45986603;
            public const int CardDestruction = 72892473;
            public const int DragonsRage = 54178050;
            public const int InfernoFireBlast = 52684508;
            public const int GiantTrunade = 42703248;
            public const int HeavyStorm = 19613556;
            public const int SoulRelease = 5758500;
            public const int Reasoning = 58577036;
            public const int Metamorphosis = 46411259;
            public const int TheFluteOfSummoningDragon = 43973174;
            public const int DimensionFusion = 23557835;

            // Extra Deck Fusions
            public const int DarkfireDragon = 17881964;
            public const int FiendSkullDragon = 66235877;
            public const int KingDragun = 13756293;
            public const int GaiaTheDragonChampion = 66889139;
            public const int BlackSkullDragon = 11901678;
            public const int ThousandEyesRestrict = 63519819;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.RedEyesDarknessDragon,
            CardId.ArmedDragonLv7,
            CardId.KingDragun,
            CardId.BlackSkullDragon,
            CardId.ThousandEyesRestrict
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_RedEyesExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DifferentDimensionCapsule, DifferentDimensionCapsuleEffect);

            // ── 2. Backrow Removal & Board Protection ──
            AddExecutor(ExecutorType.Activate, CardId.MyBodyAsAShield, MyBodyAsAShieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonsBead);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);

            // ── 3. Reasoning (Cheat Summons) ──
            AddExecutor(ExecutorType.Activate, CardId.Reasoning);

            // ── 4. Inferno Fire Blast (Burn for 2400 with Red-Eyes) ──
            AddExecutor(ExecutorType.Activate, CardId.InfernoFireBlast, InfernoFireBlastEffect);

            // ── 5. Special Summons & Tributes ──
            // Chick -> Red-Eyes B. Dragon
            AddExecutor(ExecutorType.Activate, CardId.BlackDragonsChick, BlackDragonsChickEffect);
            // Red-Eyes B. Dragon -> Red-Eyes Darkness Dragon
            AddExecutor(ExecutorType.SpSummon, CardId.RedEyesDarknessDragon, RedEyesDarknessDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarknessDragon);

            // ── 6. Metamorphosis & Cyber-Stein ──
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);
            AddExecutor(ExecutorType.Activate, CardId.CyberStein, CyberSteinEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionFusion, DimensionFusionEffect);

            // ── 7. Flute of Summoning Dragon ──
            AddExecutor(ExecutorType.Activate, CardId.TheFluteOfSummoningDragon, TheFluteOfSummoningDragonEffect);

            // ── 8. Monster Effects ──
            AddExecutor(ExecutorType.Activate, CardId.KingDragun, KingDragunEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThousandEyesRestrict, ThousandEyesRestrictEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv3, ArmedDragonLv3Effect);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv5, ArmedDragonLv5Effect);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv7, ArmedDragonLv7Effect);
            AddExecutor(ExecutorType.Activate, CardId.SpiritRyu, SpiritRyuEffect);

            // ── 9. Spells & Discard Engine ──
            AddExecutor(ExecutorType.Activate, CardId.LightningVortex, LightningVortexEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.CardDestruction, CardDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoulRelease, SoulReleaseEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperRejuvenation, SuperRejuvenationEffect);
            AddExecutor(ExecutorType.Activate, CardId.DragonsRage);

            // ── 10. Normal Summons ──
            // Mirage Dragon shuts down traps in battle!
            AddExecutor(ExecutorType.Summon, CardId.MirageDragon);
            AddExecutor(ExecutorType.Summon, CardId.LusterDragon);
            AddExecutor(ExecutorType.Summon, CardId.LordOfD, LordOfDSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElementDragon);
            AddExecutor(ExecutorType.Summon, CardId.BlackDragonsChick, BlackDragonsChickSummon);
            AddExecutor(ExecutorType.Summon, CardId.CyberStein, CyberSteinSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArmedDragonLv3);

            // Defensive Set / Floaters
            AddExecutor(ExecutorType.MonsterSet, CardId.MaskedDragon);
            AddExecutor(ExecutorType.MonsterSet, CardId.ArmedDragonLv3);
            AddExecutor(ExecutorType.Summon, CardId.MaskedDragon);

            // Floater Activation
            AddExecutor(ExecutorType.Activate, CardId.MaskedDragon, MaskedDragonEffect);

            // ── 11. Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);

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
            const long HINTMSG_SPSUMMON = 509;

            // 1. Tribute Selection: Chick (for Red-Eyes) > Red-Eyes (for Darkness) > Metamorphosis targets
            if (hint == HINTMSG_RELEASE)
            {
                var tributes = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.BlackDragonsChick)) return 1;
                    if (c.IsCode(CardId.RedEyesBlackDragon)) return 2;
                    if (c.IsCode(CardId.ArmedDragonLv3)) return 3;
                    if (c.IsCode(CardId.MaskedDragon)) return 4;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (tributes.Count >= min)
                    return tributes.Take(min).ToList();
            }

            // 2. Discard Selection (Armed Dragon / Spirit Ryu / Lightning Vortex / Dragon's Bead):
            if (hint == HINTMSG_DISCARD)
            {
                var discards = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    // Discard normal dragons first to boost Darkness Dragon
                    if (c.IsCode(CardId.RedEyesBlackDragon)) return 1;
                    if (c.IsCode(CardId.LusterDragon)) return 2;
                    if (c.IsCode(CardId.ElementDragon)) return 3;
                    if (c.IsCode(CardId.MaskedDragon)) return 4;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (discards.Count >= min)
                    return discards.Take(min).ToList();
            }

            // 3. Special Summon:
            // Cyber-Stein / Metamorphosis Extra Deck Fusions:
            // Priority: King Dragun (if dragons in hand) > Restrict (if opp has monster) > Black Skull (high ATK)
            if (hint == HINTMSG_SPSUMMON)
            {
                var extraFusions = cards.Where(c => c.Location == CardLocation.Extra).OrderBy(c =>
                {
                    if (c.IsCode(CardId.KingDragun) && Bot.Hand.Any(h => h != null && h.HasRace(CardRace.Dragon))) return 1;
                    if (c.IsCode(CardId.ThousandEyesRestrict) && Enemy.GetMonsterCount() > 0 && !Bot.HasInMonstersZone(CardId.ThousandEyesRestrict)) return 2;
                    if (c.IsCode(CardId.BlackSkullDragon)) return 3;
                    if (c.IsCode(CardId.KingDragun)) return 4;
                    if (c.IsCode(CardId.FiendSkullDragon)) return 5;
                    return 10;
                }).ToList();

                if (extraFusions.Count >= min)
                    return extraFusions.Take(min).ToList();

                // Masked Dragon / Flute of Summoning Dragon / King Dragun effect:
                var dragonSummons = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.RedEyesBlackDragon)) return 1;
                    if (c.IsCode(CardId.ArmedDragonLv5)) return 2;
                    if (c.IsCode(CardId.LusterDragon)) return 3;
                    if (c.IsCode(CardId.MirageDragon)) return 4;
                    if (c.IsCode(CardId.MaskedDragon)) return 5;
                    if (c.IsCode(CardId.ArmedDragonLv3)) return 6;
                    return 10;
                }).ToList();

                if (dragonSummons.Count >= min)
                    return dragonSummons.Take(min).ToList();
            }

            // 4. Target Destruction (Armed Dragon LV5 / LV7):
            if (hint == HINTMSG_DESTROY)
            {
                var threats = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (threats.Count >= min)
                    return threats.Take(min).ToList();
            }

            // 5. Soul Release banish: Banish opponent's key GY cards
            if (hint == HINTMSG_REMOVE)
            {
                var oppGy = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.Grave)
                    .OrderByDescending(c => c.Attack).ToList();
                if (oppGy.Count >= min)
                    return oppGy.Take(Math.Min(max, oppGy.Count)).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.RedEyesDarknessDragon || cardId == CardId.BlackSkullDragon ||
                cardId == CardId.KingDragun || cardId == CardId.ArmedDragonLv7 ||
                cardId == CardId.ArmedDragonLv5 || cardId == CardId.LusterDragon ||
                cardId == CardId.MirageDragon)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            }

            if (cardId == CardId.MaskedDragon || cardId == CardId.ArmedDragonLv3 || cardId == CardId.BlackDragonsChick)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool MyBodyAsAShieldEffect()
        {
            return Bot.LifePoints > 1500 && Duel.LastChainPlayer == 1;
        }

        private bool DifferentDimensionCapsuleEffect()
        {
            AI.SelectCard(CardId.RedEyesDarknessDragon, CardId.DimensionFusion, CardId.Reasoning);
            return true;
        }

        private bool GiantTrunadeEffect()
        {
            return Enemy.GetSpellCount() >= 1;
        }

        private bool HeavyStormEffect()
        {
            // Don't destroy our King Dragun or face-up equipped Restrict
            if (Bot.HasInMonstersZone(CardId.KingDragun) || Bot.HasInMonstersZone(CardId.ThousandEyesRestrict))
            {
                return Enemy.GetSpellCount() >= 3;
            }
            return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && Bot.GetSpellCount() == 0);
        }

        private bool InfernoFireBlastEffect()
        {
            // Inflicts 2400 damage to opponent!
            return Bot.HasInMonstersZone(CardId.RedEyesBlackDragon);
        }

        private bool BlackDragonsChickSummon()
        {
            return Bot.Hand.Any(c => c != null && c.IsCode(CardId.RedEyesBlackDragon));
        }

        private bool BlackDragonsChickEffect()
        {
            return Bot.Hand.Any(c => c != null && c.IsCode(CardId.RedEyesBlackDragon));
        }

        private bool RedEyesDarknessDragonSummon()
        {
            // Tribute 1 Red-Eyes B. Dragon on field
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RedEyesBlackDragon));
        }

        private bool CyberSteinSummon()
        {
            return Bot.LifePoints > 5000 && Bot.GetMonsterCount() < 5;
        }

        private bool CyberSteinEffect()
        {
            if (Bot.LifePoints <= 5000 || Bot.GetMonsterCount() >= 5) return false;

            if (Enemy.GetMonsterCount() > 0 && !Bot.HasInMonstersZone(CardId.ThousandEyesRestrict))
            {
                AI.SelectCard(CardId.ThousandEyesRestrict);
                return true;
            }
            if (Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Dragon)))
            {
                AI.SelectCard(CardId.KingDragun);
                return true;
            }
            AI.SelectCard(CardId.BlackSkullDragon);
            return true;
        }

        private bool DimensionFusionEffect()
        {
            if (Bot.LifePoints <= 2000) return false;
            int ourBanishedDragons = Bot.Banished.Count(c => c != null && c.IsMonster() && c.HasRace(CardRace.Dragon));
            return ourBanishedDragons >= 2 || (ourBanishedDragons >= 1 && Enemy.LifePoints <= 2400);
        }

        private bool MetamorphosisEffect()
        {
            // Tribute Level 1 (Chick) -> Thousand-Eyes Restrict
            // Tribute Level 5 (Armed Dragon LV5) -> Fiend Skull Dragon
            // Tribute Level 7 (Red-Eyes B. Dragon) -> King Dragun
            // Tribute Level 9 (Red-Eyes Darkness Dragon) -> Black Skull Dragon
            var validMonsters = Bot.MonsterZone.GetMonsters().Where(c => c != null && c.IsFaceup() &&
                (c.Level == 1 || c.Level == 5 || c.Level == 7 || c.Level == 9)).ToList();

            if (validMonsters.Count == 0) return false;

            var target = validMonsters.OrderBy(c =>
            {
                if (c.Level == 1 && !Bot.HasInMonstersZone(CardId.ThousandEyesRestrict)) return 1;
                if (c.Level == 7 && Bot.Hand.Any(h => h != null && h.HasRace(CardRace.Dragon))) return 2;
                if (c.Level == 5) return 3;
                if (c.Level == 7) return 4;
                return 10;
            }).FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                if (target.Level == 1) AI.SelectNextCard(CardId.ThousandEyesRestrict);
                else if (target.Level == 5) AI.SelectNextCard(CardId.FiendSkullDragon);
                else if (target.Level == 7) AI.SelectNextCard(CardId.KingDragun);
                else if (target.Level == 9) AI.SelectNextCard(CardId.BlackSkullDragon);
                return true;
            }
            return false;
        }

        private bool LordOfDSummon()
        {
            return Bot.HasInHand(CardId.TheFluteOfSummoningDragon) && Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Dragon));
        }

        private bool TheFluteOfSummoningDragonEffect()
        {
            return Bot.HasInMonstersZone(CardId.LordOfD) && Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Dragon));
        }

        private bool KingDragunEffect()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            var dragon = Bot.Hand.FirstOrDefault(c => c != null && c.HasRace(CardRace.Dragon));
            if (dragon != null)
            {
                AI.SelectCard(dragon);
                return true;
            }
            return false;
        }

        private bool ThousandEyesRestrictEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.EquipCards.Count == 0)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ArmedDragonLv3Effect()
        {
            if (Duel.Phase == DuelPhase.Standby && Duel.Player == 0)
            {
                return Bot.GetRemainingCount(CardId.ArmedDragonLv5, 1) > 0;
            }
            return false;
        }

        private bool ArmedDragonLv5Effect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Standby && Duel.Player == 0)
                {
                    return Bot.GetRemainingCount(CardId.ArmedDragonLv7, 1) > 0;
                }
                // Discard monster to pop enemy monster with <= ATK
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Id != Card.Id && !IsAceCard(c));
                if (discard != null)
                {
                    var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack <= discard.Attack && !IsTargetImmune(c));
                    if (target != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ArmedDragonLv7Effect()
        {
            if (Enemy.GetMonsterCount() >= 2 && Bot.Hand.Any(c => c != null && c.IsMonster() && !IsAceCard(c)))
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && !IsAceCard(c));
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
            }
            return false;
        }

        private bool SpiritRyuEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Bot.BattlingMonster != Card) return false;
            var dragons = Bot.Hand.Where(c => c != null && c.HasRace(CardRace.Dragon)).ToList();
            if (dragons.Count == 0) return false;

            ClientCard defender = Enemy.BattlingMonster;
            if (defender == null)
            {
                // Direct: pump only if lethal
                if (Card.Attack + 1000 >= Enemy.LifePoints)
                {
                    AI.SelectCard(dragons.First());
                    return true;
                }
            }
            else
            {
                int defPower = defender.GetDefensePower();
                if (Card.Attack < defPower && Card.Attack + 1000 >= defPower)
                {
                    AI.SelectCard(dragons.First());
                    return true;
                }
            }
            return false;
        }

        private bool LightningVortexEffect()
        {
            if (DefaultSpellWillBeNegated() || Bot.Hand.Count == 0) return false;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsTargetImmune(c)) >= 2;
        }

        private bool SnatchStealEffect()
        {
            if (DefaultSpellWillBeNegated()) return false;
            ClientCard target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null && target.Attack >= 1500)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CardDestructionEffect()
        {
            int dragonCount = Bot.Hand.Count(c => c != null && c.HasRace(CardRace.Dragon));
            return dragonCount >= 2 || Bot.Hand.Count >= 5;
        }

        private bool SoulReleaseEffect()
        {
            return Enemy.Graveyard.Count >= 2;
        }

        private bool SuperRejuvenationEffect()
        {
            return Duel.Phase == DuelPhase.End;
        }

        private bool MaskedDragonEffect()
        {
            return true;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            if (Card.IsCode(CardId.ThousandEyesRestrict))
            {
                // TER locks all attacks anyway
                return false;
            }
            if (Card.IsCode(CardId.MaskedDragon))
            {
                if (Card.IsFaceup() && Card.IsAttack()) return true; // Switch to DEF to float
                return false;
            }
            return DefaultMonsterRepos();
        }
    }
}
