using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using RpgCombatKata.Core.Model.Events;

namespace RpgCombatKata.Core.Model {
    public class GameEngine {
        private readonly EventBus eventBus;
        private IDisposable joinRequestSubscriber;
        private List<Character> characters;
        private Factions factions;

        public GameEngine(EventBus eventBus, GameMap gameMap, Factions factions) {
            this.eventBus = eventBus;
            this.factions = factions;
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