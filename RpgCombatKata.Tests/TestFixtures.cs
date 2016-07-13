using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NSubstitute;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Model;
using RpgCombatKata.Core.Model.Actions;
using RpgCombatKata.Core.Model.Attacks;
using RpgCombatKata.Core.Model.Characters;
using RpgCombatKata.Core.Model.Commands;
using RpgCombatKata.Core.Model.Events;
using RpgCombatKata.Core.Model.Map;
using RpgCombatKata.Core.Model.Rules;

namespace RpgCombatKata.Tests {
    public class TestFixtures : IDisposable {
        private readonly EventBus eventBus = new EventBus();

        public TriedTo<Attack> ATriedToAttackEvent(string from, string to, int damage, AttackRange kind = null)
        {
            if (kind == null)
            {
                kind = new MeleeAttack();
            }
            return new TriedTo<Attack>(new Attack(from, to, damage, kind));
        }

        public Character ALiveCharacter(int? healthPoints = default(int?), int level = 1)
        {
            var characterUid = Guid.NewGuid().ToString();
            var healthCondition = GivenTheHealthConditionOf(characterUid, currentHealth: healthPoints);
            return new Character(characterUid, healthCondition, level);
        }

        private CharacterHealthCondition GivenTheHealthConditionOf(string characterId, int? currentHealth) {
            var attacksObservable = eventBus.Subscriber<SuccessTo<Attack>>();
            var healsObservable = eventBus.Subscriber<SuccessTo<Heal>>();
            return currentHealth.HasValue ? new CharacterHealthCondition(characterId, attacksObservable, healsObservable, currentHealth.Value) 
                                          : new CharacterHealthCondition(characterId, attacksObservable, healsObservable);
        }


        internal JoinFaction AJoinFactionAction(string characterId, string factionName)
        {
            return new JoinFaction(characterId, factionName);
        }

        public void Executed<T>(T action) where T: GameAction {
            eventBus.Publish(action);
        }

        public Character ADeadCharacter() {
            return ALiveCharacter(healthPoints: 0);
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

        public TriedTo<Heal> ATriedToHealEvent(string source, string target, int heal)
        {
            return new TriedTo<Heal>(new Heal(source, target, heal));
        }


        public GameMap AGameMap() {
            return Substitute.For<GameMap>();
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

        public SuccessTo<Attack> ASuccessAttack(string from, string to, int damage) {
            return new SuccessTo<Attack>(new Attack(from, to, damage,AttackRanges.Melee()));
        }

        public SuccessTo<Attack> ASuccessAttack(string to, int damage) {
            return ASuccessAttack("", to, damage);
        }
        public SuccessTo<Heal> ASuccessHeal(string to, int healingPoints) {
            return new SuccessTo<Heal>(new Heal("", to, healingPoints));
        }

        public RulesEngine ARulesEngine(Rules rules) {
            return ARulesEngine(new List<Rules>() {rules});
        }

        public RulesEngine ARulesEngine(List<Rules> rules)
        {
            return new RulesEngine(eventBus, rules);
        }

        public CombatRules ACombatRules() {
            return new CombatRules();
        }

        public HealingRules AHealingRules() {
            return new HealingRules();
        }

        public LevelBasedCombatRules ALevelBasedCombatRules(List<Character> charactersStubData) {
            var charactersRepository = Substitute.For<CharactersRepository>();
            charactersRepository.GetCharacter(Arg.Any<string>())
                .Returns(x => charactersStubData.First(y => y.Id == x.ArgAt<string>(0)));
            return new LevelBasedCombatRules(charactersRepository);
        }

        public MapBasedCombatRules AMapBasedCombatRules(GameMap gameMap) {
            return new MapBasedCombatRules(gameMap);
        }
    }
}