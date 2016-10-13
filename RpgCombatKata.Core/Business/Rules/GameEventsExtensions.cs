using System;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules {
    public static class GameEventsExtensions {
        public static EventBus SubscribeToOutOfTheGameEvents(this EventBus eventBus,
            CharactersRepository charactersRepository) {
            var observer = eventBus.Observable<TriedTo<JoinGame>>();
            observer.Subscribe(x => charactersRepository.JoinCharacter(x.Event.Character));
            return eventBus;
        }
    }
}