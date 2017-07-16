using System;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Core.Business.Rules {
    public static class CharactersExtensions {
        public static IObservable<TriedTo<T>> IfCharactersExist<T>(this IObservable<TriedTo<T>> observer, CharactersRepository charactersRepository)
            where T : GameEntityTargetedMessage {
            return observer
                .Where(message => charactersRepository.GetCharacter(message.Event.From) != null)
                .Where(message => charactersRepository.GetCharacter(message.Event.To) != null);
        }
    }
}