using System.Collections.Generic;

namespace RpgCombatKata.Core.Business.Factions
{
    public interface FactionsRepository
    {
        Faction GetFaction(string id);
        IEnumerable<Faction> GetFactions();
    }
}
