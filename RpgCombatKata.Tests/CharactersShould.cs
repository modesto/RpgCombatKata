using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Model;
using Given = RpgCombatKata.Tests.TestFixtures; 

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
    }

    public static class TestFixtures {

        public static Character ACharacter() {
            return new Character();
        }
    }
}
