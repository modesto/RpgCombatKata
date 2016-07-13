namespace RpgCombatKata.Core.Model {
    public class SuccessTo<T> where T : GameMessage {
        public T Event { get; }

        public SuccessTo(T gameEvent) {
            Event = gameEvent;
        }
    }
}