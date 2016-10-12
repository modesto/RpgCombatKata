namespace RpgCombatKata.Core.Business {
    public class GameEntityTargetedMessage : GameMessage {
        public GameEntityIdentity To { get; }

        protected GameEntityTargetedMessage(GameEntityIdentity to) {
            To = to;
        }
    }
}