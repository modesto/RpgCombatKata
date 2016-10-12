using System;
using System.Reactive.Linq;

namespace RpgCombatKata.Core.Business {
    public static class IObservableExtensions {
        public static IObservable<SuccessTo<T>> TargettingTo<T>(this IObservable<SuccessTo<T>> observable,
            GameEntityIdentity expectedTarget) where T : GameEntityTargetedMessage {
            return observable.Where(x => x.Event.To == expectedTarget);
        }
    }
}