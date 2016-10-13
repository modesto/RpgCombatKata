using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Factions;

namespace RpgCombatKata.Core.Business.Rules {
    public static class FactionBasedExtensions {
        public static IObservable<TriedTo<T>> AreEnemies<T>(this IObservable<TriedTo<T>> observer,
            FactionsRepository factionsRepository) where T : GameEntityTargetedMessage
        {
            return observer.Where(attack => {
                var factions = factionsRepository.GetFactions();
                return AreEnemies(attack.Event.From, attack.Event.To, factions);
            });
        }

        public static IObservable<TriedTo<T>> AreFriends<T>(this IObservable<TriedTo<T>> observer,
            FactionsRepository factionsRepository) where T : GameEntityTargetedMessage
        {
            return observer.Where(attack => {
                var factions = factionsRepository.GetFactions();
                return !AreEnemies(attack.Event.From, attack.Event.To, factions);
            });
        }

        private static bool AreEnemies(GameEntityIdentity from, GameEntityIdentity to, IEnumerable<Faction> factions)
        {
            return from != to && factions.All(faction => !faction.AreAllies(from, to));
        }
    }
}