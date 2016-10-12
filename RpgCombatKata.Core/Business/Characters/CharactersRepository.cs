namespace RpgCombatKata.Core.Business.Characters
{
    public interface CharactersRepository {
        Character GetCharacter(GameEntityIdentity id);

        void JoinCharacter(Character character);
    }
}
