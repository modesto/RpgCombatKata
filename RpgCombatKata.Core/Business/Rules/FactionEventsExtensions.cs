using System;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules {
    public static class FactionEventsExtensions {
        public static EventBus SubscribeToFactionEvents(this EventBus eventBus) {
            SubscribeToLeaveFaction(eventBus);
            SubscribeToJoinFaction(eventBus);
            return eventBus;
        }

        private static void SubscribeToJoinFaction(EventBus eventBus) {
            var observer = eventBus.Observable<TriedTo<JoinFaction>>();
            observer.Subscribe(x => eventBus.Publish(new SuccessTo<JoinFaction>(x.Event)));
        }

        private static void SubscribeToLeaveFaction(EventBus eventBus) {
            var observer = eventBus.Observable<TriedTo<LeaveFaction>>();
            observer.Subscribe(x => eventBus.Publish(new SuccessTo<LeaveFaction>(x.Event)));
        }
    }
}