using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Business;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Events;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Core.Business.Rules;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Tests.Fixtures {
    public class GivenFixtures : IDisposable {
        private readonly EventBus eventBus;
        public GivenFixtures(EventBus eventBus) {
            this.eventBus = eventBus;
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

        public Character ADeadCharacter() {
            return ALiveCharacter(healthPoints: 0);
        }

        public JoinToGameRequested AJoinToGameRequestedEvent(Character character) {
            return new JoinToGameRequested(character);
        }

        public void Dispose() {
            eventBus.Dispose();
        }



        public GameMap AGameMap() {
            return Substitute.For<GameMap>();
        }


        public Faction AFaction() {
            var factionId = Guid.NewGuid().ToString();
            return new Faction(factionId, eventBus.Subscriber<SuccessTo<JoinFaction>>(), eventBus.Subscriber<SuccessTo<LeaveFaction>>());
        }

        public RulesEngine ARulesEngine(Core.Business.Rules.Rules rules) {
            return ARulesEngine(new List<Core.Business.Rules.Rules>() {rules});
        }

        public RulesEngine ARulesEngine()
        {
            return ARulesEngine(new List<Core.Business.Rules.Rules>());
        }
        
        public RulesEngine ARulesEngine(List<Core.Business.Rules.Rules> rules)
        {
            return new RulesEngine(eventBus, rules);
        }

        public CombatRules ACombatRules() {
            return new CombatRules();
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

        public FactionCombatRules AFactionCombatRules(Faction aFaction)
        {
            var factionsRepository = Substitute.For<FactionsRepository>();
            factionsRepository.GetFaction(Arg.Is(aFaction.Id)).Returns(aFaction);
            factionsRepository.GetFactions().Returns(new List<Faction>() { aFaction });
            return new FactionCombatRules(factionsRepository);
        }
        public FactionCombatRules AFactionCombatRules()
        {
            var factionsRepository = Substitute.For<FactionsRepository>();
            return new FactionCombatRules(factionsRepository);
        }
    }
}