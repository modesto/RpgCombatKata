using System;
using System.Collections;
using System.Collections.Generic;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Core.Model {
    public class Character : LivingBeing {
        private IDisposable healingSubscriber;
        private IDisposable damageSubscriber;
        private IObservable<HealCharacter> healsObservable;
        private const int MaxHealth = 1000;

        public Character(string id, HealthCondition healthCondition) {
            Id = id;
            HealthCondition = healthCondition;
        }
        public Character(string uid, IObservable<DamageCharacter> damagesObservable, IObservable<HealCharacter> healsObservable, int? healthPoints = default(int?), int level = 1) {
            this.healsObservable = healsObservable;
            Health = healthPoints ?? MaxHealth;
            Id = uid;
            Level = level;
            SubscribeToDamage(damagesObservable);
            VerifyHealthStatus();
        }

        public Character(string uid, IObservable<DamageCharacter> damagesObservable, IObservable<HealCharacter> healsObservable, HealthCondition healthCondition)
        {
            this.healsObservable = healsObservable;
            HealthCondition = healthCondition;
            Id = uid;
            SubscribeToDamage(damagesObservable);
            VerifyHealthStatus();
        }
        public int Level { get; }

        private void SubscribeToDamage(IObservable<DamageCharacter> damagesObservable) {
            damageSubscriber = damagesObservable.Subscribe(x => ReceiveDamage(x.Damage));
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
                healingSubscriber.TryToDispose();
            } else if (Health == MaxHealth) {
                healingSubscriber.TryToDispose();
            }
            else if (healingSubscriber == null) {
                healingSubscriber = healsObservable.Subscribe(x => ReceiveHeal(x.Heal));
            }

        }

        public int Health { get; private set; }
        public string Id { get; }
        public HealthCondition HealthCondition { get; }
    }
}