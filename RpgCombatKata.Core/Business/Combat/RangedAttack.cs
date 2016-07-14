using RpgCombatKata.Core.Business.Map;

namespace RpgCombatKata.Core.Business.Combat {
    public class RangedAttack : AttackRange {
        public Distance Range => Distance.FromMeters(20);
    }
}