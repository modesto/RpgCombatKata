using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules
{
    public class RulesEngine
    {
        private readonly EventBus eventBus;
        private readonly List<Rules> gameRules;

        public RulesEngine(EventBus eventBus, List<Rules> gameRules) {
            this.eventBus = eventBus;
            this.gameRules = gameRules;
            RegisterFiltersForTriedTo<Attack>();
            RegisterFiltersForTriedTo<Heal>();
            RegisterFiltersForTriedTo<JoinFaction>();
            RegisterFiltersForTriedTo<LeaveFaction>();
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
