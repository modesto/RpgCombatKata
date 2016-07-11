using System;
using FluentAssertions;
using NUnit.Framework;
using Given = RpgCombatKata.Tests.TestFixtures;
using When = RpgCombatKata.Tests.TestFixtures;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class GameEngineShould : TestBase
    {
        [Test]
        public void allow_a_character_to_join_game() {
            var aCharacter = Given.ACharacter();
            var aGameEngine = Given.AGameEngine();
            var joinToGameRequested = Given.AJoinToGameRequestedEvent(aCharacter);
            When.Raised(joinToGameRequested);
            aGameEngine.IsPlaying(aCharacter.Id).Should().BeTrue();
        }

        [Test]
        public void allow_characters_to_join_game()
        {
            var aGameEngine = Given.AGameEngine();
            var firstCharacter = Given.ACharacter();
            var secondCharacter = Given.ACharacter();
            var firstJoinToGameRequested = Given.AJoinToGameRequestedEvent(firstCharacter);
            var secondJoinToGameRequested = Given.AJoinToGameRequestedEvent(secondCharacter);
            When.Raised(firstJoinToGameRequested);
            When.Raised(secondJoinToGameRequested);
            aGameEngine.CountTotalPlayers().Should().Be(2);
        }


        [Test]
        public void aoid_a_character_to_join_game_twice()
        {
            var aCharacter = Given.ACharacter();
            var aGameEngine = Given.AGameEngine();
            var joinToGameRequested = Given.AJoinToGameRequestedEvent(aCharacter);
            When.Raised(joinToGameRequested);
            When.Raised(joinToGameRequested);
            aGameEngine.CountTotalPlayers().Should().Be(1);
        }

    }
}
