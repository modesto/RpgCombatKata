namespace RpgCombatKata.Core.Model {
    public class Attack : GameMessage {
        public string From { get; }
        public string To { get; }
        public int Damage { get; private set; }

        public Attack(string from, string to, int damage) {
            From = from;
            To = to;
            Damage = damage;
        }

        public void UpdateDamage(int newValue) {
            Damage = newValue;
        }
    }
}