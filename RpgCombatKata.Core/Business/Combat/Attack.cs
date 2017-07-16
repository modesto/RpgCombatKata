namespace RpgCombatKata.Core.Business.Combat {
    public class Attack : GameEntityTargetedMessage {
        public GameEntityIdentity From { get; }
        public int Damage { get; private set; }
        public AttackRange AttackRange { get; }

        public Attack(GameEntityIdentity from, GameEntityIdentity to, int damage, AttackRange kind) : base(from, to) {
            From = from;
            Damage = damage;
            AttackRange = kind;
        }

        public void UpdateDamage(int newValue) {
            Damage = newValue;
        }
    }
}