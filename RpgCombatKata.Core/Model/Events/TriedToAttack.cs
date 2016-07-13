namespace RpgCombatKata.Core.Model.Events
{
    public class TriedToAttack : GameEvent
    {
        public Character Attacker { get; }
        public int Damage { get; }
        public Character Defender { get; }
        public AttackRange AttackRange { get; }

        public TriedToAttack(Character attacker, Character defender, int damage, AttackRange kind)
        {
            this.Attacker = attacker;
            this.Defender = defender;
            this.Damage = damage;
            this.AttackRange = kind;
        }

    }
}
