using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Factions
{
    public class JoinFaction : GameMessage
    {
        public GameEntityIdentity CharacterId { get; }
        public string FactionId { get; }

        public JoinFaction(GameEntityIdentity characterId, string factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
