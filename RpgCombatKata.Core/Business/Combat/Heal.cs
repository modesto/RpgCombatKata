namespace RpgCombatKata.Core.Business.Combat {
    public class Heal : GameEntityTargetedMessage {
        public GameEntityIdentity From { get; }
        public int HealingPoints { get; }

        public Heal(GameEntityIdentity from, GameEntityIdentity to, int healingPoints) : base(to) {
            From = from;
            HealingPoints = healingPoints;
        }
    }
}