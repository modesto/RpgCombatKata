using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgCombatKata.Core.Model.Actions
{
    public class LeaveFaction: GameAction
    {
        public string CharacterId { get; }
        public string FactionName { get; }

        public LeaveFaction(string characterId, string factionName)
        {
            this.CharacterId = characterId;
            this.FactionName = factionName;
        }
    }
}
