using System;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Core.Model {
    public class Character {
        private IDisposable healingSubscriber;
        private const int MaxHealth = 1000;

        public Character(string uid, IObservable<DamageCharacter> damagesObservable, IObservable<HealCharacter> healsObservable, int? healthPoints = default(int?)) {
            Health = healthPoints ?? MaxHealth;
            Id = uid;
            damagesObservable.Subscribe(x => ReceiveDamage(x.Damage));
            SubscribeToHealing(healsObservable);
        }

        private void SubscribeToHealing(IObservable<HealCharacter> healsObservable) {
            if (Health > 0) {
                healingSubscriber = healsObservable.Subscribe(x => ReceiveHeal(x.Heal));
            }
        }

        private void ReceiveHeal(int heal) {
            Health += heal;
        }

        private void ReceiveDamage(int damage) {
            Health -= damage;
            VerifyHealthStatus();
        }

        private void VerifyHealthStatus() {
            if (Health <= 0) {
                Health = 0;
                healingSubscriber.Dispose();
            }
        }

        public int Health { get; private set; }
        public string Id { get; }
    }
}