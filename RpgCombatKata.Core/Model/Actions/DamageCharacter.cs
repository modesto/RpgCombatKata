namespace RpgCombatKata.Core.Model.Actions {
    public class DamageCharacter {
        public int Damage { get; }
        public string To { get; }

        public DamageCharacter(string to, int damage)
        {
            To = to;
            Damage = damage;
        }
    }
}