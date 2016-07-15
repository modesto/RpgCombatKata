using System;
using RpgCombatKata.Core.Business.Map;

namespace RpgCombatKata.Core
{
    public static class NumericExtensionMethods
    {
        public static int IncreaseIn(this int value, Func<int, int> calculation) => value + calculation(value);
        public static int DecreaseIn(this int value, Func<int, int> calculation) => value - calculation(value);
        public static Func<int, int> Percent(this int percent) => value => (int)(value * ((decimal)percent / 100));
    }
}
