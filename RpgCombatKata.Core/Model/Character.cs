using System;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Core.Model {
    public class Character {
        private const int MaxHealth = 1000;

        public Character(string uid, IObservable<DamageCharacter> damagesObservable, IObservable<HealCharacter> healsObservable, int? healthPoints = default(int?)) {
            Health = healthPoints ?? MaxHealth;
            Id = uid;
            damagesObservable.Subscribe(x => ReceiveDamage(x.Damage));
            healsObservable.Subscribe(x => ReceiveHeal(x.Heal));
        }

        private void ReceiveHeal(int heal) {
            Health += heal;
        }

        private void ReceiveDamage(int damage) {
            Health -= damage;
        }

        public int Health { get; private set; }
        public string Id { get; }
    }
}