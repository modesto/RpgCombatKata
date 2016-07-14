using RpgCombatKata.Core;
using RpgCombatKata.Core.Business;
using RpgCombatKata.Core.Business.Combat;
using RpgCombatKata.Core.Business.Factions;
using RpgCombatKata.Core.Infrastructure;
using RpgCombatKata.Tests.Rules;

namespace RpgCombatKata.Tests.Fixtures {
    public class WhenFixtures {
        private readonly EventBus eventBus;

        public WhenFixtures(EventBus eventBus) {
            this.eventBus = eventBus;
        }

        public void Raise<T>(T gameEvent) {
            eventBus.Publish(gameEvent);
        }

        public void TriedToHeal(string source, string target, int heal)
        {
            Raise(new TriedTo<Heal>(new Heal(source, target, heal)));
        }

        public void TriedToAttack(string from, string to, int damage, AttackRange kind = null)
        {
            kind = kind ?? new MeleeAttack();
            Raise(new TriedTo<Attack>(new Attack(from, to, damage, kind)));
        }

        public void TriedToJoinFaction(string characerId, string factionId)
        {
            Raise(new TriedTo<JoinFaction>(new JoinFaction(characerId, factionId)));
        }

        public void TriedToLeaveFaction(string characterId, string factionId)
        {
            Raise(new TriedTo<LeaveFaction>(new LeaveFaction(characterId, factionId)));
        }

        public void ASuccessAttack(string to, int damage) {
            ASuccessAttack("", to, damage);
        }
        public void ASuccessAttack(string from, string to, int damage)
        {
            Raise(new SuccessTo<Attack>(new Attack(from, to, damage, AttackRanges.Melee())));
        }

        public void ASuccessHeal(string to, int healingPoints) {
            Raise(new SuccessTo<Heal>(new Heal("", to, healingPoints)));
        }
    }
}