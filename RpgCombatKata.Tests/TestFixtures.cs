using System;
using System.Reactive.Linq;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Model;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Tests {
    public static class TestFixtures {
        private static readonly EventBus eventBus = new EventBus();

        public static Character ACharacter(int? healthPoints = default(int?)) {
            var characterUid = Guid.NewGuid().ToString();
            var damagesObservable = eventBus.Subscriber<DamageCharacter>().Where(x => x.To == characterUid);
            var healsObservable = eventBus.Subscriber<HealCharacter>().Where(x => x.To == characterUid);
            return new Character(characterUid, damagesObservable, healsObservable, healthPoints);
        }

        public static DamageCharacter ADamageCharacterAction(string to, int damage) {
            return new DamageCharacter(to, damage);
        }

        public static void Executed<T>(T action) where T: GameAction {
            eventBus.Publish(action);
        }

        public static HealCharacter AHealCharacterAction(string to, int heal) {
            return new HealCharacter(to, heal);
        }

        public static Character ADeadCharacter() {
            return ACharacter(healthPoints: 0);
        }
    }
}