using RpgCombatKata.Core.Business.Map;

namespace RpgCombatKata.Core.Business.Combat {
    public class MeleeAttack : AttackRange {
        public Distance Range => Distance.FromMeters(2);
    }
}