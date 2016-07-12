namespace RpgCombatKata.Core.Model {
    public class RangedAttack : AttackRange {
        public Distance Range => Distance.FromMeters(20);
    }
}