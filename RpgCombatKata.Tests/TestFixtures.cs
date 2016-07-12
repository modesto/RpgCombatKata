using System;
using System.Reactive.Linq;
using NSubstitute;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Model;
using RpgCombatKata.Core.Model.Actions;
using RpgCombatKata.Core.Model.Events;

namespace RpgCombatKata.Tests {
    public class TestFixtures : IDisposable {
        private readonly EventBus eventBus = new EventBus();

        public Character ACharacter(int? healthPoints = default(int?), int level = 1) {
            var characterUid = Guid.NewGuid().ToString();
            var damagesObservable = eventBus.Subscriber<DamageCharacter>().Where(x => x.To == characterUid);
            var healsObservable = eventBus.Subscriber<HealCharacter>().Where(x => x.To == characterUid);
            return new Character(characterUid, damagesObservable, healsObservable, healthPoints, level);
        }

        public DamageCharacter ADamageCharacterAction(string to, int damage) {
            return new DamageCharacter(to, damage);
        }

        public void Executed<T>(T action) where T: GameAction {
            eventBus.Publish(action);
        }

        public HealCharacter AHealCharacterAction(string to, int heal) {
            return new HealCharacter(to, heal);
        }

        public Character ADeadCharacter() {
            return ACharacter(healthPoints: 0);
        }

        public GameEngine AGameEngine(GameMap gameMap = null) {
            if (gameMap == null) {
                gameMap = Substitute.For<GameMap>();
                gameMap.DistanceBetween(Arg.Any<string>(), Arg.Any<string>()).Returns(Distance.FromMeters(0));
            }

            return new GameEngine(eventBus, gameMap);
        }

        public JoinToGameRequested AJoinToGameRequestedEvent(Character character) {
            return new JoinToGameRequested(character);
        }

        public void Raised<T>(T gameEvent) where T : GameEvent {
            eventBus.Publish(gameEvent);
        }

        public void Dispose() {
            eventBus.Dispose();
        }

        public TriedToAttack ATriedToAttackEvent(Character attacker, Character defender, int damage, AttackRange kind = null) {
            if (kind == null) {
                kind = new MeleeAttack();
            }
            return new TriedToAttack(attacker, defender, damage, kind);
        }

        public TriedToHeal ATriedToHealEvent(Character source, Character target, int heal) {
            return new TriedToHeal(source, target, heal);
        }

        public GameMap AGameMap() {
            return Substitute.For<GameMap>();
        }
    }
}