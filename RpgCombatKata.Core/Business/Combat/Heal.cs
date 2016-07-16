using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Combat {
    public class Heal : GameMessage {
        public GameEntityIdentity From { get; }
        public GameEntityIdentity To { get; }
        public int HealingPoints { get; }

        public Heal(GameEntityIdentity from, GameEntityIdentity to, int healingPoints) {
            From = from;
            To = to;
            HealingPoints = healingPoints;
        }
    }
}