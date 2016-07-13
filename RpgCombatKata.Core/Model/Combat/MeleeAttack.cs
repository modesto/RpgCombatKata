using RpgCombatKata.Core.Model.Map;

namespace RpgCombatKata.Core.Model.Combat {
    public class MeleeAttack : AttackRange {
        public Distance Range => Distance.FromMeters(2);
    }
}