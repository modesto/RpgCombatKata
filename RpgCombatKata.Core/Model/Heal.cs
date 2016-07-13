namespace RpgCombatKata.Core.Model {
    public class Heal : GameMessage {
        public string From { get; }
        public string To { get; }
        public int HealingPoints { get; }

        public Heal(string from, string to, int healingPoints) {
            From = from;
            To = to;
            HealingPoints = healingPoints;
        }
    }
}