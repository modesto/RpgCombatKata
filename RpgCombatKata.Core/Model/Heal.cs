namespace RpgCombatKata.Core.Model {
    public class Heal : GameMessage {
        public string To { get; }
        public int HealingPoints { get; }

        public Heal(string to, int healingPoints) {
            To = to;
            HealingPoints = healingPoints;
        }
    }
}