using RpgCombatKata.Core.Business.Combat;

namespace RpgCombatKata.Tests.Fixtures {
    public static class AttackRanges {
        public static AttackRange Melee() {
            return new MeleeAttack();
        }

        public static AttackRange Range() {
            return new RangedAttack();
        }
    }
}