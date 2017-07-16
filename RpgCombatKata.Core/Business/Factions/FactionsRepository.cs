using System.Collections.Generic;

namespace RpgCombatKata.Core.Business.Factions
{
    public interface FactionsRepository
    {
        Faction GetFaction(FactionIdentity id);
        IEnumerable<Faction> GetFactions();

        void Join(Faction faction);
    }
}
