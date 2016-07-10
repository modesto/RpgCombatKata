namespace RpgCombatKata.Core.Model {
    public class Character {
        private const int MaxHealth = 1000;

        public Character() {
            Health = MaxHealth;
        }
        public int Health { get; }
    }
}