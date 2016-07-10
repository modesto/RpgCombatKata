using System;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Model;
using RpgCombatKata.Core.Model.Actions;
using Given = RpgCombatKata.Tests.TestFixtures;
using When = RpgCombatKata.Tests.TestFixtures;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class CharactersShould
    {
        [Test]
        public void start_with_1000_health_points() {
            var character = Given.ACharacter();
            character.Health.Should().Be(1000);
        }

        [Test]
        public void have_an_unique_identifier() {
            var firstCharacter = Given.ACharacter();
            var secondCharacter = Given.ACharacter();
            firstCharacter.Id.Should().NotBeNullOrWhiteSpace();
            firstCharacter.Id.Should().NotBe(secondCharacter.Id);
        }

        [Test]
        public void receive_damage() {
            var aCharacter = Given.ACharacter();
            var damage = Given.ADamageCharacterAction(to: aCharacter.Id, damage: 100);
            When.Executed(damage);
            aCharacter.Health.Should().Be(900);
        }

        [Test]
        public void be_healed() {
            var aCharacter = Given.ACharacter(healthPoints : 900);
            var heal = Given.AHealCharacterAction(to: aCharacter.Id, heal: 50);
            When.Executed(heal);
            aCharacter.Health.Should().Be(950);
        }
    }

    public static class TestFixtures {
        private static readonly EventBus eventBus = new EventBus();

        public static Character ACharacter(int? healthPoints = default(int?)) {
            var characterUid = Guid.NewGuid().ToString();
            var damagesObservable = eventBus.Subscriber<DamageCharacter>().Where(x => x.To == characterUid);
            var healsObservable = eventBus.Subscriber<HealCharacter>().Where(x => x.To == characterUid);
            return new Character(characterUid, damagesObservable, healsObservable, healthPoints);
        }

        public static DamageCharacter ADamageCharacterAction(string to, int damage) {
            return new DamageCharacter(to, damage);
        }

        public static void Executed<T>(T action) where T: GameAction {
            eventBus.Publish(action);
        }

        public static HealCharacter AHealCharacterAction(string to, int heal) {
            return new HealCharacter(to, heal);
        }
    }
}
