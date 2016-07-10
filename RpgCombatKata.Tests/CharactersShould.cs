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
    }

    public static class TestFixtures {
        private static readonly EventBus eventBus = new EventBus();

        public static Character ACharacter() {
            var characterUid = Guid.NewGuid().ToString();
            var damagesObservable = eventBus.Subscriber<DamageCharacter>().Where(x => x.To == characterUid);
            return new Character(characterUid, damagesObservable);
        }

        public static DamageCharacter ADamageCharacterAction(string to, int damage) {
            return new DamageCharacter(to, damage);
        }

        public static void Executed(DamageCharacter action) {
            eventBus.Publish(action);
        }
    }
}
