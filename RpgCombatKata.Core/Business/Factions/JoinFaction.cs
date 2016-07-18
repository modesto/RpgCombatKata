using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Factions
{
    public class JoinFaction : GameMessage
    {
		public CharacterIdentity CharacterId { get; }
        public FactionIdentity FactionId { get; }

		public JoinFaction(CharacterIdentity characterId, FactionIdentity factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
