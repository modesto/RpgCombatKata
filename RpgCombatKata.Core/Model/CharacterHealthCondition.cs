using System;
using System.Reactive.Linq;

namespace RpgCombatKata.Core.Model {
    public class CharacterHealthCondition : HealthCondition {
        private readonly IObservable<SuccessTo<Heal>> healsObservable;

        public CharacterHealthCondition(string characterId, IObservable<SuccessTo<Attack>> attacksObservable, IObservable<SuccessTo<Heal>> healsObservable )
        {
            this.healsObservable = healsObservable;
            this.CurrentHealth = MaxHealth;
            attacksObservable.Where(x => x.Event.To == characterId).Subscribe(x => ProcessAttack(x.Event));
            healsObservable.Where(x => x.Event.To == characterId).Subscribe(x => ProcessHeal(x.Event));
        }

        private void ProcessHeal(Heal heal) {
            this.CurrentHealth += heal.HealingPoints;
        }

        public CharacterHealthCondition(string characterId, IObservable<SuccessTo<Attack>> attacksObservable, IObservable<SuccessTo<Heal>> healsObservable, int currentHealth) : this(characterId, attacksObservable, healsObservable)
        {
            this.CurrentHealth = currentHealth;
        }

        private void ProcessAttack(Attack attack) {
            CurrentHealth -= attack.Damage;
        }

        public int CurrentHealth { get; private set; }

        public int MaxHealth => 1000;
    }
}