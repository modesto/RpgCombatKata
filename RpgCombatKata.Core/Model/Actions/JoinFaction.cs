namespace RpgCombatKata.Core.Model.Actions
{
    public class JoinFaction : GameAction
    {
        public string CharacterId { get; }
        public string FactionName { get; }

        public JoinFaction(string characterId, string factionName)
        {
            this.CharacterId = characterId;
            this.FactionName = factionName;
        }
    }
}
