namespace RpgCombatKata.Core.Model.Actions {
    public class HealCharacter : GameAction {
        public string To { get; }
        public int Heal { get; }

        public HealCharacter(string to, int heal) {
            To = to;
            Heal = heal;
        }
    }
}