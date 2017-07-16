using System;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules {
    public static class CombatEventsExtensions
    {
        public static EventBus SubscribeToCombatEvents(this EventBus eventBus,
            CharactersRepository charactersRepository, GameMap gameMap, FactionsRepository factionsRepository) {
            SubscribeToAttack(eventBus, charactersRepository, gameMap, factionsRepository);
            SubscribeToHeal(eventBus, charactersRepository, factionsRepository);
            return eventBus;
        }

        private static void SubscribeToHeal(EventBus eventBus, CharactersRepository charactersRepository,
            FactionsRepository factionsRepository)
        {
            var observer = eventBus.Observable<TriedTo<Heal>>();
            observer
                .IfCharactersExist(charactersRepository)
                .AreFriends(factionsRepository)
                .Subscribe(x => eventBus.Publish(new SuccessTo<Heal>(x.Event)));
        }

        private static void SubscribeToAttack(EventBus eventBus, CharactersRepository charactersRepository, GameMap gameMap,
            FactionsRepository factionsRepository) {
            var observer = eventBus.Observable<TriedTo<Attack>>();
            observer
                .IfCharactersExist(charactersRepository)
                .AreEnemies(factionsRepository)
                .ApplyMapBasedRules(gameMap)
                .ApplyLevelBasedRules(charactersRepository)
                .Subscribe(x => eventBus.Publish(new SuccessTo<Attack>(x.Event)));
        }
    }
}