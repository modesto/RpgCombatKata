using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Core.Business.Rules
{
    public class GameEngine
    {
        private readonly EventBus eventBus;
        private readonly List<Rules> gameRules;
        private FactionsRepository factionsRepository;
        private GameMap gameMap;
        private CharactersRepository charactersRepository;

        public GameEngine(EventBus eventBus, List<Rules> gameRules) {
            this.eventBus = eventBus;
            this.gameRules = gameRules;
            SubscribeToTriedToAttack();
            //RegisterFiltersForTriedTo<Attack>();
            RegisterFiltersForTriedTo<Heal>();
            RegisterFiltersForTriedTo<JoinFaction>();
            RegisterFiltersForTriedTo<LeaveFaction>();
        }

        public GameEngine(EventBus eventBus, FactionsRepository factionsRepository, GameMap gameMap, CharactersRepository charactersRepository) {
            this.eventBus = eventBus;
            this.factionsRepository = factionsRepository;
            this.gameMap = gameMap;
            this.charactersRepository = charactersRepository;
            SubscribeToTriedToJoinGame();
            SubscribeToTriedToJoinFaction();
            SubscribeToTriedToAttack();
        }

        private void SubscribeToTriedToJoinGame() {
            var observer = eventBus.Observable<TriedTo<JoinGame>>();
            observer.Subscribe(x => charactersRepository.JoinCharacter(x.Event.Character));
        }

        private void SubscribeToTriedToJoinFaction() {
            var observer = eventBus.Observable<TriedTo<JoinFaction>>();
            observer.Subscribe(x => eventBus.Publish(new SuccessTo<JoinFaction>(x.Event)));
        }

        private void SubscribeToTriedToAttack() {
            var observer = eventBus.Observable<TriedTo<Attack>>();
            observer
                .Where(attack => charactersRepository.GetCharacter(attack.Event.From) != null)
                .Where(attack => charactersRepository.GetCharacter(attack.Event.To) != null)
                .Where(attack => attack.Event.From != attack.Event.To)
                .Where(attack => AreEnemies(attack.Event.From, attack.Event.To))
                .Where(attack => gameMap.DistanceBetween(attack?.Event.From, attack?.Event.To).TotalMeters <=
                attack?.Event.AttackRange.Range.TotalMeters)
                .Where(ApplyLevelBasedRules)
                .Subscribe(EvaluateFilterResult);
        }

        private bool ApplyLevelBasedRules(TriedTo<Attack> attack)
        {
            var attacker = charactersRepository.GetCharacter(attack?.Event.From);
            var defender = charactersRepository.GetCharacter(attack?.Event.To);

            if (attacker.Level >= defender.Level + 5)
            {
                attack?.Event.UpdateDamage(attack.Event.Damage.IncreaseIn(50.Percent()));
            }
            else if (attacker.Level <= defender.Level - 5)
            {
                attack?.Event.UpdateDamage(attack.Event.Damage.DecreaseIn(50.Percent()));
            }

            return true;
        }

        private bool AreEnemies(GameEntityIdentity aCharacter, GameEntityIdentity anotherCharacter)
        {
            if (aCharacter == anotherCharacter) return false;
            foreach (var faction in factionsRepository.GetFactions())
            {
                if (faction.AreAllies(aCharacter, anotherCharacter)) return false;
            }
            return true;
        }


        private void RegisterFiltersForTriedTo<T>() where T : GameMessage {
            var observer = eventBus.Observable<TriedTo<T>>();
            foreach(var rules in gameRules) {
                if (!rules.CanApplyTo<TriedTo<T>>()) continue;
                var filter = rules.GetFilterFor<TriedTo<T>>();
                observer = observer.Select(filter);
            }
            observer.Subscribe(EvaluateFilterResult);
        }

        private void EvaluateFilterResult<T>(TriedTo<T> x) where T : GameMessage {
            if (x == null) return;
            eventBus.Publish(new SuccessTo<T>(x.Event));
        }
    }
}
