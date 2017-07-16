namespace RpgCombatKata.Core.Business.Combat {
    public class Heal : GameEntityTargetedMessage {
        public int HealingPoints { get; }

        public Heal(GameEntityIdentity from, GameEntityIdentity to, int healingPoints) : base(from, to) {
            HealingPoints = healingPoints;
        }
    }
}