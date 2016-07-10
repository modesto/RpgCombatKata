using System;

namespace RpgCombatKata.Core
{
    public static class IDisposableExtensionMethods
    {
        public static void TryToDispose(this IDisposable disposable) {
            disposable?.Dispose();
        }
    }
}
