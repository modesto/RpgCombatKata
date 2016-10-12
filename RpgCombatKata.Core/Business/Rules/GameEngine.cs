using System;
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
        private readonly FactionsRepository factionsRepository;
        private readonly GameMap gameMap;
        private readonly CharactersRepository charactersRepository;

        public GameEngine(EventBus eventBus, FactionsRepository factionsRepository, GameMap gameMap, CharactersRepository charactersRepository) {
            this.eventBus = eventBus;
            this.factionsRepository = factionsRepository;
            this.gameMap = gameMap;
            this.charactersRepository = charactersRepository;
            SubscribeToTriedToJoinGame();
            SubscribeToTriedToJoinFaction();
            SubscribeToTriedToLeaveFaction();
            SubscribeToTriedToAttack();
            SubscribeToTriedToHeal();
        }

        private void SubscribeToTriedToLeaveFaction() {
            var observer = eventBus.Observable<TriedTo<LeaveFaction>>();
            observer.Subscribe(x => eventBus.Publish(new SuccessTo<LeaveFaction>(x.Event)));
        }

        private void SubscribeToTriedToHeal() {
            var observer = eventBus.Observable<TriedTo<Heal>>();
            observer
                .Where(heal => charactersRepository.GetCharacter(heal.Event.From) != null)
                .Where(heal => charactersRepository.GetCharacter(heal.Event.To) != null)
                .Where(heal => AreFriends(heal.Event.From, heal.Event.To))
                .Subscribe(x => eventBus.Publish(new SuccessTo<Heal>(x.Event)));
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
                .Subscribe(x => eventBus.Publish(new SuccessTo<Attack>(x.Event)));
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

        private bool AreFriends(GameEntityIdentity aCharacter, GameEntityIdentity anotherCharacter) {
            return !AreEnemies(aCharacter, anotherCharacter);
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
    }
}
