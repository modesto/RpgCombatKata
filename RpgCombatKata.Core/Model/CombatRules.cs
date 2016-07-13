using System;

namespace RpgCombatKata.Core.Model
{
    public class CombatRules : GameRules
    {
        public Func<T, T> GetFilterFor<T>() where T : class {
            return new Func<T, T>(ApplyFilter<T>);
        }

        private T ApplyFilter<T>(T gameEvent) where T : class {
            TriedTo<Attack> attack = gameEvent as TriedTo<Attack>;
            if (attack?.Event?.From == attack?.Event?.To) return default(T);
            return (T)Convert.ChangeType(attack, typeof(T));
        }

        public bool CanApplyTo<T>() where T : class{
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
