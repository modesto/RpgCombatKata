namespace RpgCombatKata.Core.Model.Events {
    public class JoinToGameRequested : GameEvent {
        public Character Character { get; }

        public JoinToGameRequested(Character character) {
            Character = character;
        }
    }
}