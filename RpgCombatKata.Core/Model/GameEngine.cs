using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgCombatKata.Core.Model {
    public class GameEngine {
        private readonly EventBus eventBus;
        private IDisposable joinRequestSubscriber;
        private List<Character> characters;

        public GameEngine(EventBus eventBus) {
            this.eventBus = eventBus;
            characters = new List<Character>();
            SubscribeToJoinRequests();
        }

        private void SubscribeToJoinRequests() {
            joinRequestSubscriber = eventBus.Subscriber<JoinToGameRequested>().Where(gameEvent => !characters.Any(character => character.Id == gameEvent.Character.Id)).Subscribe(x => JoinToGame(x.Character));
        }

        private void JoinToGame(Character character) {
            characters.Add(character);
        }

        public bool IsPlaying(string characterId) {
            return characters.Any(x => x.Id == characterId);
        }

        public int CountTotalPlayers() {
            return characters.Count;
        }
    }
}