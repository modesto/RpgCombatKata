using System;

namespace RpgCombatKata.Core.Model {
    public interface GameRules {
        Func<T,T> GetFilterFor<T>() where T : class;
        bool CanApplyTo<T>() where T : class;
    }
}