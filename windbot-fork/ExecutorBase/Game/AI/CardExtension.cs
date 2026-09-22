using System;
using System.Linq;
using WindBot.Game.AI.Enums;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    public static class CardExtension
    {
        /// <summary>
        /// Is this monster is invincible to battle?
        /// </summary>
        public static bool IsMonsterInvincible(this ClientCard card)
        {
            if (card == null || card.IsDisabled()) return false;
            return CardIntelligence.IsInvincibleBattle(card.Id) || Enum.IsDefined(typeof(InvincibleMonster), card.Id);
        }

        /// <summary>
        /// Is this monster is dangerous to attack?
        /// </summary>
        public static bool IsMonsterDangerous(this ClientCard card)
        {
            if (card == null || card.IsDisabled()) return false;
            return CardIntelligence.IsDangerousBattleTarget(card, null)
                || Enum.IsDefined(typeof(DangerousMonster), card.Id)
                || (card.HasSetcode(0x18d) && (card.HasType(CardType.Ritual) || (card.EquipCards != null && card.EquipCards.Count > 0)));
        }

        /// <summary>
        /// Do this monster prevents activation of opponent's effect monsters in battle?
        /// </summary>
        public static bool IsMonsterHasPreventActivationEffectInBattle(this ClientCard card)
        {
            if (card == null || card.IsDisabled()) return false;
            return Enum.IsDefined(typeof(PreventActivationEffectInBattle), card.Id);
        }

        /// <summary>
        /// Can this monster attack while it is in face-up Defense Position?
        /// </summary>
        public static bool IsMonsterAttackWhileInDefPos(this ClientCard card)
        {
            if (card == null) return false;
            return card.IsFaceup() && card.IsDefense() && !card.IsDisabled()
                && (card.HasSetcode(0x9a) || Enum.IsDefined(typeof(DefenseAttackMonster), card.Id));
        }

        /// <summary>
        /// Get the power this monster uses to attack in its current position.
        /// </summary>
        public static int GetAttackPower(this ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsMonsterAttackWhileInDefPos()
                && !Enum.IsDefined(typeof(DefenseAttackWithAttackValueMonster), card.Id))
                return card.Defense;
            return card.Attack;
        }

        /// <summary>
        /// Is this card shouldn't be tried to be selected as target?
        /// </summary>
        public static bool IsShouldNotBeTarget(this ClientCard card)
        {
            if (card == null) return false;
            if (card.IsDisabled()) return false;
            if (card.EquipCards != null && card.EquipCards.Any(c => c != null && (c.IsCode(30012506, 15622650) || c.IsCode(77411244) || c.IsCode(3405259, 34050266) || c.IsCode(89812483)) && !c.IsDisabled()))
                return true;
            if (card.Overlays != null && card.Overlays.Any(code => code == 91025875))
                return true;
            if (CardIntelligence.IsTargetImmune(card.Id))
                return true;
            return (!card.HasType(CardType.Normal) && Enum.IsDefined(typeof(ShouldNotBeTarget), card.Id));
        }

        /// <summary>
        /// Is this card shouldn't be tried to be selected as target of monster?
        /// </summary>
        public static bool IsShouldNotBeMonsterTarget(this ClientCard card)
        {
            if (card == null) return false;
            if (card.IsDisabled()) return false;
            if (card.EquipCards != null && card.EquipCards.Any(c => c != null && (c.IsCode(30012506, 15622650) || c.IsCode(89812483)) && !c.IsDisabled()))
                return true;
            return Enum.IsDefined(typeof(ShouldNotBeMonsterTarget), card.Id);
        }

        /// <summary>
        /// Is this card shouldn't be tried to be selected as target of spell & trap?
        /// </summary>
        public static bool IsShouldNotBeSpellTrapTarget(this ClientCard card)
        {
            if (card == null) return false;
            if (card.IsDisabled()) return false;
            if (card.EquipCards != null && card.EquipCards.Any(c => c != null && (c.IsCode(77411244) || c.IsCode(3405259, 34050266) || c.IsCode(89812483)) && !c.IsDisabled()))
                return true;
            return Enum.IsDefined(typeof(ShouldNotBeSpellTrapTarget), card.Id);
        }

        /// <summary>
        /// Is this monster should be disabled (with Breakthrough Skill) before it use effect and release or banish itself?
        /// </summary>
        public static bool IsMonsterShouldBeDisabledBeforeItUseEffect(this ClientCard card)
        {
            if (card == null || card.IsDisabled()) return false;
            return Enum.IsDefined(typeof(ShouldBeDisabledBeforeItUseEffectMonster), card.Id);
        }

        public static bool IsFloodgate(this ClientCard card)
        {
            if (card == null) return false;
            return CardIntelligence.IsFloodgate(card.Id) || Enum.IsDefined(typeof(Floodgate), card.Id);
        }

        public static bool IsOneForXyz(this ClientCard card)
        {
            if (card == null) return false;
            return Enum.IsDefined(typeof(OneForXyz), card.Id);
        }

        public static bool IsFusionSpell(this ClientCard card)
        {
            if (card == null) return false;
            if (CardIntelligence.IsFusionSpell(card.Id) || Enum.IsDefined(typeof(FusionSpell), card.Id))
                return true;
            if (card.HasType(CardType.Spell))
            {
                string desc = card.Data?.Description;
                if (!string.IsNullOrEmpty(desc))
                {
                    if (desc.Contains("Fusion Summon") || desc.Contains("ฟิวชั่น"))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is this monster not be synchro material?
        /// </summary>
        public static bool IsMonsterNotBeSynchroMaterial(this ClientCard card)
        {
            if (card == null) return false;
            return Enum.IsDefined(typeof(NotBeSynchroMaterialMonster), card.Id);
        }

        /// <summary>
        /// Is this monster not be xyz material?
        /// </summary>
        public static bool IsMonsterNotBeXyzMaterial(this ClientCard card)
        {
            if (card == null) return false;
            return Enum.IsDefined(typeof(NotBeXyzMaterialMonster), card.Id);
        }

        public static bool IsMonsterNotBeSummonTribute(this ClientCard card)
        {
            if (card == null) return false;
            return Enum.IsDefined(typeof(NotBeSummonTributeMonster), card.Id);
        }
    }
}