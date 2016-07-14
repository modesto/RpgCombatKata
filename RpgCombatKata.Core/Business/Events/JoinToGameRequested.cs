using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Events {
    public class JoinToGameRequested : GameEvent {
        public Character Character { get; }

        public JoinToGameRequested(Character character) {
            Character = character;
        }
    }
}