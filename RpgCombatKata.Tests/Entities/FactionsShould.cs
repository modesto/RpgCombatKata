using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class FactionsShould : TestBase
    {
        [Test]
        public void allow_a_character_join_a_faction() {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void forbide_a_character_join_a_faction_twice()
        {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void allow_a_character_leave_a_faction() {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
            When.TriedToLeaveFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(0);
        }

    }
}
