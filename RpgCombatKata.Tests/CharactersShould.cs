using FluentAssertions;
using NUnit.Framework;
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

        [Test]
        public void not_be_healed_if_born_dead() {
            var aDeadCharacter = Given.ADeadCharacter();
            var heal = Given.AHealCharacterAction(to: aDeadCharacter.Id, heal: 50);
            When.Executed(heal);
            aDeadCharacter.Health.Should().Be(0);
        }

        [Test]
        public void not_be_healed_after_dead()
        {
            var aCharacter = Given.ACharacter(healthPoints: 50);
            var damage = Given.ADamageCharacterAction(to: aCharacter.Id, damage: 60);
            When.Executed(damage);
            var heal = Given.AHealCharacterAction(to: aCharacter.Id, heal: 50);
            When.Executed(heal);
            aCharacter.Health.Should().Be(0);
        }

        [Test]
        public void not_be_healed_over_max_life() {
            var aCharacter = Given.ACharacter();
            var expectedHealth = aCharacter.Health;
            var heal = Given.AHealCharacterAction(to: aCharacter.Id, heal: 50);
            When.Executed(heal);
            aCharacter.Health.Should().Be(expectedHealth);
        }

    }
}
