namespace RpgCombatKata.Core.Business.Factions
{
    public class JoinFaction : GameAction, GameMessage
    {
        public string CharacterId { get; }
        public string FactionId { get; }

        public JoinFaction(string characterId, string factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
