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

        [Test]
        public void a_character_can_leave_a_faction() {
            var factions = Given.AFactionsService();
            var character = Given.ACharacter();
            var gameEngine = Given.AGameEngine(factions: factions);
            var faction = Given.AFaction("AnyFaction");
            var joinFaction = Given.AJoinFactionAction(character.Id, faction.Name);
            var leaveFaction = Given.ALeaveFactionAction(character.Id, faction.Name);
            When.Executed(joinFaction);
            When.Executed(leaveFaction);
            faction.TotalMembers.Should().Be(0);
        }

        //[Test]
        //public void a_character_can_heal_an_ally()
        //{
        //    var character = Given.ACharacter();
        //    var anAlly = Given.ACharacter(healthPoints: 900);
        //    var gameEngine = Given.AGameEngine();
        //    var faction = Given.AFaction("AnyFaction");
        //    var joinFaction = Given.AJoinFactionAction(character.Id, faction.Name);
        //    var allyJoinFaction = Given.AJoinFactionAction(anAlly.Id, faction.Name);
        //    var triedToHeal = Given.ATriedToHealEvent(character, anAlly, 50);
        //    When.Executed(joinFaction);
        //    When.Executed(allyJoinFaction);
        //    When.Raised(triedToHeal);
        //    anAlly.Health.Should().Be(950);
        //}

    }
}
