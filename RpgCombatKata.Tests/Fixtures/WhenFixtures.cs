using RpgCombatKata.Core.Business;
using RpgCombatKata.Core.Business.Characters;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Infrastructure;

namespace RpgCombatKata.Tests.Fixtures {
    public class WhenFixtures {
        private readonly EventBus eventBus;

        public WhenFixtures(EventBus eventBus) {
            this.eventBus = eventBus;
        }

        public void Raise<T>(T gameEvent) {
            eventBus.Publish(gameEvent);
        }

        public void TriedToHeal(GameEntityIdentity source, GameEntityIdentity target, int heal)
        {
            Raise(new TriedTo<Heal>(new Heal(source, target, heal)));
        }

        public void TriedToAttack(GameEntityIdentity from, GameEntityIdentity to, int damage, AttackRange kind = null)
        {
            kind = kind ?? new MeleeAttack();
            Raise(new TriedTo<Attack>(new Attack(from, to, damage, kind)));
        }

        public void TriedToJoinFaction(CharacterIdentity characerId, FactionIdentity factionId)
        {
            Raise(new TriedTo<JoinFaction>(new JoinFaction(characerId, factionId)));
        }

        public void TriedToLeaveFaction(CharacterIdentity characterId, FactionIdentity factionId)
        {
            Raise(new TriedTo<LeaveFaction>(new LeaveFaction(characterId, factionId)));
        }

        public void ASuccessAttack(GameEntityIdentity to, int damage) {
            ASuccessAttack(new NoGameEntityIdentity(), to, damage);
        }
        public void ASuccessAttack(GameEntityIdentity from, GameEntityIdentity to, int damage)
        {
            Raise(new SuccessTo<Attack>(new Attack(from, to, damage, AttackRanges.Melee())));
        }

        public void ASuccessHeal(GameEntityIdentity to, int healingPoints) {
            Raise(new SuccessTo<Heal>(new Heal(new NoGameEntityIdentity(), to, healingPoints)));
        }

        public void TriedToJoinGame(Character character) {
            Raise(new TriedTo<JoinGame>(new JoinGame(character)));
        }
    }
}