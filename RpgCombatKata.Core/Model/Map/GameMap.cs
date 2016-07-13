namespace RpgCombatKata.Core.Model.Map {
    public interface GameMap {
        Distance DistanceBetween(string aCharacter, string otherCharacter);
    }
}