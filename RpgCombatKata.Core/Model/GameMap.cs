namespace RpgCombatKata.Core.Model {
    public interface GameMap {
        Distance DistanceBetween(string aCharacter, string otherCharacter);
    }
}