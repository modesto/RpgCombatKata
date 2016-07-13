namespace RpgCombatKata.Core.Model.Commands {
    public class Attack : GameMessage {
        public string From { get; }
        public string To { get; }
        public int Damage { get; private set; }
        public AttackRange AttackRange { get; }

        public Attack(string from, string to, int damage, AttackRange kind) {
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