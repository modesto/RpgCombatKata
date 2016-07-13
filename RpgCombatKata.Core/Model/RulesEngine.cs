using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RpgCombatKata.Core.Model
{
    public class RulesEngine
    {
        private readonly EventBus eventBus;
        private readonly List<GameRules> gameRules;

        public RulesEngine(EventBus eventBus, List<GameRules> gameRules) {
            this.eventBus = eventBus;
            this.gameRules = gameRules;
            RegisterFiltersForTriedTo<Attack>();
            RegisterFiltersForTriedTo<Heal>();
        }

        private void RegisterFiltersForTriedTo<T>() where T : GameMessage {
            var observer = eventBus.Subscriber<TriedTo<T>>();
            foreach(var rules in gameRules) {
                if (!rules.CanApplyTo<TriedTo<T>>()) continue;
                var filter = rules.GetFilterFor<TriedTo<T>>();
                observer = observer.Select(filter);
            }
            observer.Subscribe(EvaluateFilterResult);
        }

        private void EvaluateFilterResult<T>(TriedTo<T> x) where T : GameMessage {
            if (x == null) return;
            eventBus.Publish(new SuccessTo<T>(x.Event));
        }
    }
}
