using System;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Combat;

namespace RpgCombatKata.Core.Business.Structures {
    public class DurabilityCondition
    {
        public int CurrentDurability { get; private set; }

        public DurabilityCondition(string structureId, IObservable<SuccessTo<Attack>> attacksObservable, int currentDurability)
        {
            CurrentDurability = currentDurability;

            attacksObservable.Where(x => x.Event.To == structureId).Subscribe(x => ProcessAttack(x.Event));
            VerifyHealthStatus();
        }
        private void ProcessAttack(Attack attack)
        {
            CurrentDurability -= attack.Damage;
            VerifyHealthStatus();
        }

        private void VerifyHealthStatus() {
            if (CurrentDurability <= 0) CurrentDurability = 0;
        }
    }
}