namespace RpgCombatKata.Core.Model {
    public class Attack : GameMessage {
        public string To { get; }
        public int Damage { get; }

        public Attack(string to, int damage) {
            To = to;
            Damage = damage;
        }
    }
}