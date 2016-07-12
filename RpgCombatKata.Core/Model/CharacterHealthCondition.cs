using System;
using System.Reactive.Linq;

namespace RpgCombatKata.Core.Model {
    public class CharacterHealthCondition : HealthCondition {
        private IDisposable healsSubscriber;
        private readonly IObservable<SuccessTo<Heal>> healsObservable;

        public CharacterHealthCondition(string characterId, IObservable<SuccessTo<Attack>> attacksObservable, IObservable<SuccessTo<Heal>> healsObservable, int currentHealth = MaxHealth)
        {
            this.CurrentHealth = currentHealth;
            attacksObservable.Where(x => x.Event.To == characterId).Subscribe(x => ProcessAttack(x.Event));
            this.healsObservable = healsObservable.Where(x => x.Event.To == characterId);
            VerifyHealthStatus();
        }

        private void ProcessAttack(Attack attack) => CurrentHealth -= attack.Damage;

        private void ProcessHeal(Heal heal) => CurrentHealth += heal.HealingPoints;

        public int CurrentHealth { get; private set; }

        public const int MaxHealth = 1000;

        private void VerifyHealthStatus()
        {
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                healsSubscriber.TryToDispose();
            }
            else if (CurrentHealth == MaxHealth)
            {
                healsSubscriber.TryToDispose();
            }
            else if (healsSubscriber == null)
            {
                healsSubscriber = healsObservable.Subscribe(x => ProcessHeal(x.Event));
            }

        }

    }
}