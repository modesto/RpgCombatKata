using RpgCombatKata.Core.Model.Commands;

namespace RpgCombatKata.Core.Model {
    public class TriedTo<T> where T : GameMessage {
        public T Event { get; }

        public TriedTo(T gameEvent) {
            Event = gameEvent;
        }
    }
}