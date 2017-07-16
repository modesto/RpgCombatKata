namespace RpgCombatKata.Core.Business {
    public class GameEntityTargetedMessage : GameMessage {
        public GameEntityIdentity From { get; }
        public GameEntityIdentity To { get; }

        protected GameEntityTargetedMessage(GameEntityIdentity from, GameEntityIdentity to) {
            From = from;
            To = to;
        }
    }
}