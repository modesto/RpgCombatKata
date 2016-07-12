using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class FactionRules : TestBase
    {
        [Test]
        public void a_character_can_join_a_faction() {
            var character = Given.ACharacter();
            var gameEngine = Given.AGameEngine();
            var faction = Given.AFaction("AnyFaction");
            var joinFaction = Given.AJoinFactionAction(character.Id, faction.Name);
            When.Executed(joinFaction);
            faction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void a_character_can_not_join_a_faction_twice()
        {
            var character = Given.ACharacter();
            var gameEngine = Given.AGameEngine();
            var faction = Given.AFaction("AnyFaction");
            var joinFaction = Given.AJoinFactionAction(character.Id, faction.Name);
            When.Executed(joinFaction);
            When.Executed(joinFaction);
            faction.TotalMembers.Should().Be(1);
        }
    }
}
