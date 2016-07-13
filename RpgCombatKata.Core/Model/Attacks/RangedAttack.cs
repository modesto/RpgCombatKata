using RpgCombatKata.Core.Model.Map;

namespace RpgCombatKata.Core.Model.Attacks {
    public class RangedAttack : AttackRange {
        public Distance Range => Distance.FromMeters(20);
    }
}