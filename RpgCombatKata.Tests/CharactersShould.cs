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
    }

    public static class TestFixtures {

        public static Character ACharacter() {
            return new Character();
        }
    }
}
