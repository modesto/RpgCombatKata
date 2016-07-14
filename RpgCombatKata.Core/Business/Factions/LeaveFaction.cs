namespace RpgCombatKata.Core.Business.Factions
{
    public class LeaveFaction: GameAction, GameMessage
    {
        public string CharacterId { get; }
        public string FactionId { get; }

        public LeaveFaction(string characterId, string factionId)
        {
            CharacterId = characterId;
            FactionId = factionId;
        }
    }
}
