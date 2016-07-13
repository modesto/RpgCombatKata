using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgCombatKata.Core.Model
{
    public class CombatRules : GameRules
    {
        public Func<T, T> GetFilterFor<T>() {
            return new Func<T, T>(x => x);
        }

        public bool CanApplyTo<T>() {
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
