using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business {
    public class JoinGame : GameMessage {
        public JoinGame(Character character) {
            this.Character = character;
        }

        public Character Character { get;}
    }
}