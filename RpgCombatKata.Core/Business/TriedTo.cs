namespace RpgCombatKata.Core.Business {
    public class TriedTo<T> where T : GameMessage {
        public T Event { get; }

        public TriedTo(T gameEvent) {
            Event = gameEvent;
        }
    }
}