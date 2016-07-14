namespace RpgCombatKata.Core.Business.Map {
    public interface GameMap {
        Distance DistanceBetween(string aCharacter, string otherCharacter);
    }
}