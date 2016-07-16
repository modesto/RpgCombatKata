using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Factions
{
    public class LeaveFaction: GameMessage
    {
        public GameEntityIdentity CharacterId { get; }
        public string FactionId { get; }

        public LeaveFaction(GameEntityIdentity characterId, string factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
