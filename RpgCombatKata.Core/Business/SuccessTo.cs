namespace RpgCombatKata.Core.Business {
    public class SuccessTo<T> where T : GameMessage {
        public T Event { get; }

        public SuccessTo(T gameEvent) {
            Event = gameEvent;
        }
    }
}