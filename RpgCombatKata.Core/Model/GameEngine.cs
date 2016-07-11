using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using RpgCombatKata.Core.Model.Actions;
using RpgCombatKata.Core.Model.Events;

namespace RpgCombatKata.Core.Model {
    public class GameEngine {
        private readonly EventBus eventBus;
        private IDisposable joinRequestSubscriber;
        private List<Character> characters;
        private IDisposable tryToAttackSubscriber;

        public GameEngine(EventBus eventBus) {
            this.eventBus = eventBus;
            characters = new List<Character>();
            SubscribeToJoinRequests();
            SubscribeToCombatEvents();
        }

        private void SubscribeToCombatEvents() {
            tryToAttackSubscriber = eventBus.Subscriber<TriedToAttack>().Where(gameEvent => gameEvent.Attacker.Id != gameEvent.Defender.Id).Subscribe(TriedToAttack);
        }

        private void TriedToAttack(TriedToAttack gameEvent) {
            DamageCharacter damageCharacter = new DamageCharacter(gameEvent.Defender.Id, gameEvent.Damage);
            eventBus.Publish(damageCharacter);
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