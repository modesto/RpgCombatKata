using System;
using System.Reactive.Linq;
using FluentAssertions;
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

        public Character ALiveCharacter(int? healthPoints = default(int?), int level = 1)
        {
            var characterUid = Guid.NewGuid().ToString();
            var damagesObservable = eventBus.Subscriber<DamageCharacter>().Where(x => x.To == characterUid);
            var healsObservable = eventBus.Subscriber<HealCharacter>().Where(x => x.To == characterUid);
            var healthCondition = GivenTheHealthConditionOf(characterUid, currentHealth: healthPoints);
            return new Character(characterUid, damagesObservable, healsObservable, healthCondition);
        }

        private CharacterHealthCondition GivenTheHealthConditionOf(string characterId, int? currentHealth) {
            var attacksObservable = eventBus.Subscriber<SuccessTo<Attack>>();
            return currentHealth.HasValue ? new CharacterHealthCondition(characterId, attacksObservable, currentHealth.Value) 
                                          : new CharacterHealthCondition(characterId, attacksObservable);
        }


        internal JoinFaction AJoinFactionAction(string characterId, string factionName)
        {
            return new JoinFaction(characterId, factionName);
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

        public GameEngine AGameEngine(GameMap gameMap = null, Factions factions = null) {
            if (gameMap == null) {
                gameMap = Substitute.For<GameMap>();
                gameMap.DistanceBetween(Arg.Any<string>(), Arg.Any<string>()).Returns(Distance.FromMeters(0));
            }

            if (factions == null) {
                factions = Substitute.For<Factions>();
                factions.AreAlly(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            }
            return new GameEngine(eventBus, gameMap, factions);
        }

        public JoinToGameRequested AJoinToGameRequestedEvent(Character character) {
            return new JoinToGameRequested(character);
        }

        public void Raised<T>(T gameEvent) {
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

        public JoinFaction AJoinFactionAction() {
            throw new NotImplementedException();
        }

        public Faction AFaction(string factionName) {
            return new Faction(factionName, eventBus.Subscriber<JoinFaction>(), eventBus.Subscriber<LeaveFaction>());
        }

        public LeaveFaction ALeaveFactionAction(string characterId, string factionName) {
            return new LeaveFaction(characterId, factionName);
        }

        public Factions AFactionsService() {
            return new FactionsService();
        }

        public SuccessTo<Attack> ASuccessAttack(string to, int damage) {
            return new SuccessTo<Attack>(new Attack(to, damage));
        }
    }

    public class CharacterHealthCondition : HealthCondition {
        public CharacterHealthCondition(string characterId, IObservable<SuccessTo<Attack>> attacksObservable)
        {
            this.CurrentHealth = MaxHealth;
            attacksObservable.Where(x => x.Event.To == characterId).Subscribe(x => ProcessAttack(x.Event));
        }
        
        public CharacterHealthCondition(string characterId, IObservable<SuccessTo<Attack>> attacksObservable, int currentHealth) : this(characterId, attacksObservable)
        {
            this.CurrentHealth = currentHealth;
        }

        private void ProcessAttack(Attack attack) {
            CurrentHealth -= attack.Damage;
        }

        public int CurrentHealth { get; private set; }

        public int MaxHealth => 1000;
    }

    public class Attack : GameMessage {
        public string To { get; }
        public int Damage { get; }

        public Attack(string to, int damage) {
            To = to;
            Damage = damage;
        }
    }

    public interface GameMessage {}

    public class SuccessTo<T> where T : GameMessage {
        public T Event { get; }

        public SuccessTo(T gameEvent) {
            this.Event = gameEvent;
        }
    }
}