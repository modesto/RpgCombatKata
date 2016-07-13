using System.Collections.Generic;

namespace RpgCombatKata.Core.Model.Factions
{
    public interface FactionsRepository
    {
        Faction GetFaction(string id);
        IEnumerable<Faction> GetFactions();
    }
}
