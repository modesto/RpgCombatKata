using System;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Map;

namespace RpgCombatKata.Core.Business.Rules {
    public static class MapBasedExtensions
    {
        public static IObservable<TriedTo<Attack>> ApplyMapBasedRules(this IObservable<TriedTo<Attack>> observer, GameMap gameMap)
        {
            return observer.Where(attack => 
                gameMap.DistanceBetween(attack?.Event.From, attack?.Event.To).TotalMeters <=
                attack?.Event.AttackRange.Range.TotalMeters);
        }
    }
}