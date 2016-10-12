using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NSubstitute;
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

        public FactionsRepository AFactionRepository(Faction aFaction)
        {
            var factionsRepository = new FactionsRepositoryInMemory();
            factionsRepository.Join(aFaction);
            return factionsRepository;
        }

        public Structure AStructure(int durability) {
            var structureId = new GameEntityIdentity(Guid.NewGuid().ToString());
            var durabilityCondition = GivenTheDurabilityConditionOf(structureId, currentDurability: durability);
            return new Structure(structureId, durabilityCondition);
        }

    }

    public class FactionsRepositoryInMemory : FactionsRepository {
        readonly Dictionary<FactionIdentity, Faction> factions = new Dictionary<FactionIdentity, Faction>();
        public Faction GetFaction(FactionIdentity id) {
            return factions[id];
        }

        public IEnumerable<Faction> GetFactions() {
            return new ReadOnlyCollection<Faction>(factions.Values.ToList());
        }

        public void Join(Faction faction) {
            factions.Add(faction.Id, faction);
        }
    }

    public class CharactersRepositoryInMemory : CharactersRepository {
        private readonly Dictionary<GameEntityIdentity, Character> characters = new Dictionary<GameEntityIdentity, Character>();
        public Character GetCharacter(GameEntityIdentity id) {
            return characters[id];
        }

        public void JoinCharacter(Character character) {
            characters.Add(character.Id, character);
        }
    }
}