using System;

namespace RpgCombatKata.Core.Model.Factions
{
    // Can I do this?amerules
    //public class FactionRules : Rules.Rules
    //{
    //    private readonly FactionsRepository factionsRepository;

    //    public FactionRules(FactionsRepository factionsRepository) {
    //        this.factionsRepository = factionsRepository;
    //    }

    //    public Func<T, T> GetFilterFor<T>() where T : class {
    //        return ApplyFilter<T>;
    //    }

    //    private T ApplyFilter<T>(T gameEvent) where T : class {
    //        var joinRequest = gameEvent as TriedTo<JoinFaction>;
    //        return (T) Convert.ChangeType(joinRequest, typeof(T));
    //    }

    //    public bool CanApplyTo<T>() where T : class {
    //        return typeof(T) == typeof(TriedTo<JoinFaction>);
    //    }
    //}
}
