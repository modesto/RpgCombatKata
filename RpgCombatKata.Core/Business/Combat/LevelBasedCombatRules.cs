using System;
using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Combat
{
    public class LevelBasedCombatRules : Rules.Rules
    {
        private readonly CharactersRepository charactersRepository;

        public LevelBasedCombatRules(CharactersRepository charactersRepository)
        {
            this.charactersRepository = charactersRepository;
        }

        public Func<T, T> GetFilterFor<T>() where T : class {
            return ApplyFilter<T>;
        }

        private T ApplyFilter<T>(T gameEvent) where T : class
        {
            var attack = gameEvent as TriedTo<Attack>;
            var attacker = charactersRepository.GetCharacter(attack?.Event.From);
            var defender = charactersRepository.GetCharacter(attack?.Event.To);

            if (attacker.Level >= defender.Level + 5)
            {
                attack?.Event.UpdateDamage(attack.Event.Damage.IncreaseIn(50.Percent()));
            }
            else if (attacker.Level <= defender.Level - 5) {
                attack?.Event.UpdateDamage(attack.Event.Damage.DecreaseIn(50.Percent()));
            }
            return (T)Convert.ChangeType(attack, typeof(T));
        }


        public bool CanApplyTo<T>() where T : class {
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
