using System;

namespace RpgCombatKata.Core.Model.Combat
{
    public class HealingRules : Rules.Rules
    {
        public Func<T, T> GetFilterFor<T>() where T : class {
            return ApplyFilter<T>;
        }

        private T ApplyFilter<T>(T gameEvent) where T : class {
            var heal = gameEvent as TriedTo<Heal>;
            if (heal?.Event?.From != heal?.Event?.To) return default(T);
            return (T)Convert.ChangeType(heal, typeof(T));
        }

        public bool CanApplyTo<T>() where T : class{
            return typeof(T) == typeof(TriedTo<Heal>);
        }
    }
}
