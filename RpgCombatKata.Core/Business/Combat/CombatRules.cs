using System;

namespace RpgCombatKata.Core.Business.Combat
{
    public class CombatRules : Rules.Rules
    {
        public Func<T, T> GetFilterFor<T>() where T : class {
            return ApplyFilter<T>;
        }

        private T ApplyFilter<T>(T gameEvent) where T : class {
            var attack = gameEvent as TriedTo<Attack>;
            if (attack?.Event?.From == attack?.Event?.To) return default(T);
            return (T)Convert.ChangeType(attack, typeof(T));
        }

        public bool CanApplyTo<T>() where T : class{
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
