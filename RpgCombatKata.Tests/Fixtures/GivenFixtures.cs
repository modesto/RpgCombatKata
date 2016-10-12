using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using RpgCombatKata.Core.Business;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Core.Business.Rules;
using RpgCombatKata.Core.Business.Structures;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Tests.Fixtures {
    public class GivenFixtures {
        private readonly EventBus eventBus;
        public GivenFixtures(EventBus eventBus) {
            this.eventBus = eventBus;
        }

        public Character ALiveCharacter(int? healthPoints = default(int?), int level = 1)
        {
            var characterUid = new CharacterIdentity(Guid.NewGuid().ToString());
            var healthCondition = GivenTheHealthConditionOf(characterUid, currentHealth: healthPoints);
            var character = new Character(characterUid, healthCondition, level);
            eventBus.Publish(new TriedTo<JoinGame>(new JoinGame(character)));
            return character;
        }

        private CharacterHealthCondition GivenTheHealthConditionOf(GameEntityIdentity characterId, int? currentHealth) {
            var attacksObservable = eventBus.Observable<SuccessTo<Attack>>();
            var healsObservable = eventBus.Observable<SuccessTo<Heal>>();
            return currentHealth.HasValue ? new CharacterHealthCondition(characterId, attacksObservable, healsObservable, currentHealth.Value) 
                                          : new CharacterHealthCondition(characterId, attacksObservable, healsObservable);
        }

        private DurabilityCondition GivenTheDurabilityConditionOf(GameEntityIdentity structureId, int currentDurability)
        {
            var attacksObservable = eventBus.Observable<SuccessTo<Attack>>();
            return new DurabilityCondition(structureId, attacksObservable, currentDurability);
        }

        public Character ADeadCharacter() {
            return ALiveCharacter(healthPoints: 0);
        }
 
        public GameMap AGameMap() {
            return Substitute.For<GameMap>();
        }

        public Faction AFaction() {
			var factionId = new FactionIdentity(Guid.NewGuid().ToString());
            return new Faction(factionId, eventBus.Observable<SuccessTo<JoinFaction>>(), eventBus.Observable<SuccessTo<LeaveFaction>>());
        }

        public GameEngine AGameEngine(Core.Business.Rules.Rules rules) {
            return AGameEngine(new List<Core.Business.Rules.Rules> {rules});
        }

        public GameEngine AGameEngine()
        {
            return AGameEngine(new List<Core.Business.Rules.Rules>());
        }

        public GameEngine ANewGameEngine(FactionsRepository factionRepository = null, GameMap gameMap = null, CharactersRepository charactersRepository = null) {

            return new GameEngine(eventBus,
                factionRepository ?? AFactionRepository(AFaction()),
                gameMap ?? AGameMap(),
                charactersRepository ?? ACharactersRepository());
        }

        public CharactersRepository ACharactersRepository() {
            return new CharactersRepositoryInMemory();
        }

        public CharactersRepository ACharactersRepository(List<Character> charactersStubData)
        {
            var charactersRepository = Substitute.For<CharactersRepository>();
            charactersRepository.GetCharacter(Arg.Any<GameEntityIdentity>())
                .Returns(x => charactersStubData.First(y => y.Id == x.ArgAt<GameEntityIdentity>(0)));

            return charactersRepository;
        }


        public GameEngine AGameEngine(List<Core.Business.Rules.Rules> rules)
        {
            return new GameEngine(eventBus, rules);
        }

        public CombatRules ACombatRules() {
            return new CombatRules();
        }

        public LevelBasedCombatRules ALevelBasedCombatRules(List<Character> charactersStubData) {
            var charactersRepository = Substitute.For<CharactersRepository>();
            charactersRepository.GetCharacter(Arg.Any<GameEntityIdentity>())
                .Returns(x => charactersStubData.First(y => y.Id == x.ArgAt<GameEntityIdentity>(0)));
            return new LevelBasedCombatRules(charactersRepository);
        }

        public MapBasedCombatRules AMapBasedCombatRules(GameMap gameMap) {
            return new MapBasedCombatRules(gameMap);
        }

        public FactionCombatRules AFactionCombatRules(Faction aFaction)
        {
            var factionsRepository = Substitute.For<FactionsRepository>();
            factionsRepository.GetFaction(Arg.Is(aFaction.Id)).Returns(aFaction);
            factionsRepository.GetFactions().Returns(new List<Faction> { aFaction });
            return new FactionCombatRules(factionsRepository);
        }

        public FactionsRepository AFactionRepository(Faction aFaction)
        {
            var factionsRepository = Substitute.For<FactionsRepository>();
            factionsRepository.GetFaction(Arg.Is(aFaction.Id)).Returns(aFaction);
            factionsRepository.GetFactions().Returns(new List<Faction> { aFaction });
            return factionsRepository;
        }

        public FactionCombatRules AFactionCombatRules()
        {
            var factionsRepository = Substitute.For<FactionsRepository>();
            return new FactionCombatRules(factionsRepository);
        }

        public Structure AStructure(int durability) {
            var structureId = new GameEntityIdentity(Guid.NewGuid().ToString());
            var durabilityCondition = GivenTheDurabilityConditionOf(structureId, currentDurability: durability);
            return new Structure(structureId, durabilityCondition);
        }

    }

    public class CharactersRepositoryInMemory : CharactersRepository {
        private Dictionary<GameEntityIdentity, Character> characters = new Dictionary<GameEntityIdentity, Character>();
        public Character GetCharacter(GameEntityIdentity id) {
            return characters[id];
        }

        public void JoinCharacter(Character character) {
            characters.Add(character.Id, character);
        }
    }
}