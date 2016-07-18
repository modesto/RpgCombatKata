using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Factions
{
    public class LeaveFaction: GameMessage
    {
        public CharacterIdentity CharacterId { get; }
        public FactionIdentity FactionId { get; }

        public LeaveFaction(CharacterIdentity characterId, FactionIdentity factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
