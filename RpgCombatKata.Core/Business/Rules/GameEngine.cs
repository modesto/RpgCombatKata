using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules
{
    public class GameEngine
    {
        public GameEngine(EventBus eventBus, FactionsRepository factionsRepository, GameMap gameMap,
            CharactersRepository charactersRepository) {
            eventBus
                .SubscribeToFactionEvents()
                .SubscribeToOutOfTheGameEvents(charactersRepository)
                .SubscribeToCombatEvents(charactersRepository, gameMap, factionsRepository);
        }
    }
}
