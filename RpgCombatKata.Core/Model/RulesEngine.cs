using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgCombatKata.Core.Model.Events;

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
        }

        private void RegisterFiltersForTriedTo<T>() where T : GameMessage {
            var observer = eventBus.Subscriber<TriedTo<T>>();
            var rules = gameRules.First();
            if (rules.CanApplyTo<TriedTo<T>>()) {
                var filter = rules.GetFilterFor<TriedTo<T>>();
                observer = observer.Select(filter);
            }
            observer.Subscribe(x => eventBus.Publish(new SuccessTo<T>(x.Event)));
        }
    }
}
