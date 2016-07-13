﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgCombatKata.Core.Model
{
    public class LevelBasedCombatRules : GameRules
    {
        private CharactersRepository charactersRepository;

        public LevelBasedCombatRules(CharactersRepository charactersRepository)
        {
            this.charactersRepository = charactersRepository;
        }

        public Func<T, T> GetFilterFor<T>() where T : class {
            return new Func<T, T>(ApplyFilter<T>);
        }

        private T ApplyFilter<T>(T gameEvent) where T : class
        {
            TriedTo<Attack> attack = gameEvent as TriedTo<Attack>;
            var attacker = charactersRepository.GetCharacter(attack.Event.From);
            var defender = charactersRepository.GetCharacter(attack.Event.To);

            if (attacker.Level >= defender.Level + 5)
            {
                attack.Event.UpdateDamage((int)(attack.Event.Damage * 1.5));
            }
            else if (attacker.Level <= defender.Level - 5) {
                attack.Event.UpdateDamage((int) (attack.Event.Damage - (attack.Event.Damage * 0.5)));
            }

            return (T)Convert.ChangeType(attack, typeof(T));
        }


        public bool CanApplyTo<T>() where T : class {
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
