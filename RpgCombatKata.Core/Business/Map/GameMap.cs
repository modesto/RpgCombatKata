namespace RpgCombatKata.Core.Business.Map {
    public interface GameMap {
        Distance DistanceBetween(GameEntityIdentity aCharacter, GameEntityIdentity otherCharacter);
    }
}