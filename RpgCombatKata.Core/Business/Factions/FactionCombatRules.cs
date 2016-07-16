﻿using System;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;

namespace RpgCombatKata.Core.Business.Factions
{
    public class FactionCombatRules : Rules.Rules
    {
        private readonly FactionsRepository factionsRepository;

        public FactionCombatRules(FactionsRepository factionsRepository)
        {
            this.factionsRepository = factionsRepository;
        }

        public Func<T, T> GetFilterFor<T>() where T : class
        {
            return ApplyFilter<T>;
        }

        private T ApplyFilter<T>(T gameEvent) where T : class
        {
            if (typeof(T) == typeof(TriedTo<Heal>)) {
                var heal = gameEvent as TriedTo<Heal>;
                if (AreEnemies(heal?.Event.From, heal?.Event.To)) return default(T);
                return (T)Convert.ChangeType(heal, typeof(T));
            }
            else if (typeof(T) == typeof(TriedTo<Attack>)) {
                var attack = gameEvent as TriedTo<Attack>;
                if (AreFriends(attack?.Event.From, attack?.Event.To)) return default(T);
                return (T)Convert.ChangeType(attack, typeof(T));
            }
            return gameEvent;
        }

        private bool AreFriends(GameEntityIdentity aCharacter, GameEntityIdentity anotherCharacter) {
            return !AreEnemies(aCharacter, anotherCharacter);
        }

        private bool AreEnemies(GameEntityIdentity aCharacter, GameEntityIdentity anotherCharacter) {
            if (aCharacter == anotherCharacter) return false;
            foreach (var faction in factionsRepository.GetFactions()) {
                if (faction.AreAllies(aCharacter, anotherCharacter)) return false;
            }
            return true;
        }

        public bool CanApplyTo<T>() where T : class
        {
            return typeof(T) == typeof(TriedTo<Heal>) || typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}
