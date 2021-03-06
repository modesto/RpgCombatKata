using System;
using RpgCombatKata.Core.Business.Combat;

namespace RpgCombatKata.Core.Business.Characters {
    public class CharacterHealthCondition : HealthCondition {
        private IDisposable healsSubscriber;
        private readonly IObservable<SuccessTo<Heal>> healsObservable;

        public CharacterHealthCondition(GameEntityIdentity characterId, IObservable<SuccessTo<Attack>> attacksObservable,
            IObservable<SuccessTo<Heal>> healsObservable, int currentHealth = MaxHealth) {
            CurrentHealth = currentHealth;
            attacksObservable.TargettingTo(characterId).Subscribe(x => ProcessAttack(x.Event));
            this.healsObservable = healsObservable.TargettingTo(characterId);
            VerifyHealthStatus();
        }

        private void ProcessAttack(Attack attack) {
            CurrentHealth -= attack.Damage;
            VerifyHealthStatus();
        }

        private void ProcessHeal(Heal heal) {
            CurrentHealth += heal.HealingPoints;
            VerifyHealthStatus();
        }

        public int CurrentHealth { get; private set; }

        public const int MaxHealth = 1000;

        private void VerifyHealthStatus() {
            if (CurrentHealth <= 0) {
                CurrentHealth = 0;
                healsSubscriber?.Dispose();
            }
            else if (CurrentHealth == MaxHealth) {
                healsSubscriber?.Dispose();
            }
            else if (healsSubscriber == null) {
                healsSubscriber = healsObservable.Subscribe(x => ProcessHeal(x.Event));
            }

        }

    }
}