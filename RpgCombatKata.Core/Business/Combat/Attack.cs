using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Combat {
    public class Attack : GameMessage {
        public GameEntityIdentity From { get; }
        public GameEntityIdentity To { get; }
        public int Damage { get; private set; }
        public AttackRange AttackRange { get; }

        public Attack(GameEntityIdentity from, GameEntityIdentity to, int damage, AttackRange kind) {
            From = from;
            To = to;
            Damage = damage;
            AttackRange = kind;
        }

        public void UpdateDamage(int newValue) {
            Damage = newValue;
        }
    }
}