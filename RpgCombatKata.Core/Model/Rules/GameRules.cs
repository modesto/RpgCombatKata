using System;

namespace RpgCombatKata.Core.Model.Rules {
    public interface GameRules {
        Func<T,T> GetFilterFor<T>() where T : class;
        bool CanApplyTo<T>() where T : class;
    }
}