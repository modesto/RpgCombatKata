using System;

namespace RpgCombatKata.Core.Model {
    public class MapBasedCombatRules : GameRules {
        private readonly GameMap gameMap;

        public MapBasedCombatRules(GameMap gameMap) {
            this.gameMap = gameMap;
        }

        public Func<T, T> GetFilterFor<T>() where T : class {
            return ApplyFilter<T>;
        }

        private T ApplyFilter<T>(T gameEvent) where T : class
        {
            TriedTo<Attack> attack = gameEvent as TriedTo<Attack>;
            if (gameMap.DistanceBetween(attack.Event.From, attack.Event.To).TotalMeters <=
                attack.Event.AttackRange.Range.TotalMeters) {
                return (T)Convert.ChangeType(attack, typeof(T));
                
            }
            return default(T);
        }

        public bool CanApplyTo<T>() where T : class {
            return typeof(T) == typeof(TriedTo<Attack>);
        }
    }
}