using System;

namespace RpgCombatKata.Core.Model {
    public interface GameRules {
        Func<T,T> GetFilterFor<T>();
        bool CanApplyTo<T>();
    }
}