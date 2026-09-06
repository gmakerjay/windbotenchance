using System;

namespace YgoAiPlatform.Core
{
    public class Move
    {
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// Typed action enum — ใช้แทน magic strings
        /// </summary>
        public ActionType ActionType { get; set; } = ActionType.Pass;

        /// <summary>
        /// String accessor สำหรับ backward-compatibility (อ่าน/เขียน ActionType โดยอัตโนมัติ)
        /// </summary>
        public string Action
        {
            get => ActionType.ToActionString();
            set => ActionType = ActionTypeExtensions.ParseAction(value);
        }
        
        public double BaseScore { get; set; }
        public string TargetLocation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Card metadata for 32-feature neural evaluator (v2.0)
        /// <summary>Card database ID (passcode). 0 if unknown.</summary>
        public int CardId { get; set; }
        /// <summary>Card ATK value (0 if not applicable, e.g. spells/traps)</summary>
        public int CardAtk { get; set; }
        /// <summary>Card DEF value (0 if not applicable)</summary>
        public int CardDef { get; set; }
        /// <summary>Card level/rank (0 if not applicable, e.g. Link monsters)</summary>
        public int CardLevel { get; set; }
        /// <summary>Card type string, e.g. "Monster", "Spell", "Trap", "Fusion", "Link", "Xyz", "Synchro"</summary>
        public string CardType { get; set; } = string.Empty;
        /// <summary>Card type bitmask (from OCG core: Monster=0x1, Spell=0x2, Trap=0x4, Fusion=0x40, Synchro=0x2000, Xyz=0x800000, Link=0x4000000)</summary>
        public int CardTypeFlags { get; set; }

        // สำหรับเช็คความเท่ากันของ Move ใน ReplayValidator
        public bool IsEqualTo(Move other)
        {
            if (other == null) return false;
            return CardName.Equals(other.CardName, StringComparison.OrdinalIgnoreCase) &&
                   ActionType == other.ActionType &&
                   TargetLocation.Equals(other.TargetLocation, StringComparison.OrdinalIgnoreCase);
        }
    }
}
