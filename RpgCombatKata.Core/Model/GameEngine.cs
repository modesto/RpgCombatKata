using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using RpgCombatKata.Core.Model.Actions;
using RpgCombatKata.Core.Model.Events;

namespace RpgCombatKata.Core.Model {
    public class GameEngine {
        private readonly EventBus eventBus;
        private readonly GameMap gameMap;
        private IDisposable joinRequestSubscriber;
        private List<Character> characters;
        private IDisposable tryToAttackSubscriber;
        private IDisposable tryToHealSubscriber;
        private Factions factions;

        public GameEngine(EventBus eventBus, GameMap gameMap, Factions factions) {
            this.eventBus = eventBus;
            this.gameMap = gameMap;
            this.factions = factions;
            characters = new List<Character>();
            SubscribeToJoinRequests();
            SubscribeToCombatEvents();
        }

        private void SubscribeToCombatEvents() {
            tryToAttackSubscriber = eventBus.Subscriber<TriedToAttack>()
                .Where(gameEvent => gameEvent.Attacker.Id != gameEvent.Defender.Id)
                .Where(gameEvent => gameMap.DistanceBetween(gameEvent.Attacker.Id, gameEvent.Defender.Id).TotalMeters <= gameEvent.AttackRange.Range.TotalMeters)
                .Subscribe(TriedToAttack);
            tryToHealSubscriber = eventBus.Subscriber<TriedToHeal>().Where(gameEvent => AreAllies(gameEvent)).Subscribe(TriedToHeal);
        }

        private static bool AreAllies(TriedToHeal gameEvent) {
            return gameEvent.Source.Id == gameEvent.Target.Id;
        }

        private void TriedToAttack(TriedToAttack gameEvent) {
            var calculatedDamage = gameEvent.Damage;
            if (gameEvent.Attacker.Level >= gameEvent.Defender.Level + 5) {
                calculatedDamage = (int)(calculatedDamage*1.5);
            } else if (gameEvent.Attacker.Level <= gameEvent.Defender.Level - 5) {
                calculatedDamage = (int) (calculatedDamage - (calculatedDamage*0.5));
            }
            DamageCharacter damageCharacter = new DamageCharacter(gameEvent.Defender.Id, calculatedDamage);
            eventBus.Publish(damageCharacter);
        }

        private void TriedToHeal(TriedToHeal gameEvent) {
            HealCharacter healCharacter = new HealCharacter(gameEvent.Target.Id, gameEvent.Heal);
            eventBus.Publish(healCharacter);
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