namespace RpgCombatKata.Core.Model {
    public class MeleeAttack : AttackRange {
        public Distance Range => Distance.FromMeters(2);
    }
}