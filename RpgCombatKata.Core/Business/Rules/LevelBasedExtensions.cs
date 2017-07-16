using System;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;

namespace RpgCombatKata.Core.Business.Rules {
    public static class LevelBasedExtensions {
        public static IObservable<TriedTo<Attack>> ApplyLevelBasedRules(this IObservable<TriedTo<Attack>> observer, CharactersRepository charactersRepository)
        {
            return observer.Where(attack => {
                var attacker = charactersRepository.GetCharacter(attack?.Event.From);
                var defender = charactersRepository.GetCharacter(attack?.Event.To);
                if (attacker.Level >= defender.Level + 5)
                {
                    attack?.Event.UpdateDamage(NumericExtensionMethods.IncreaseIn(attack.Event.Damage, 50.Percent()));
                }
                else if (attacker.Level <= defender.Level - 5)
                {
                    attack?.Event.UpdateDamage(NumericExtensionMethods.DecreaseIn(attack.Event.Damage, 50.Percent()));
                }
                return true;
            });
        }
    }
}