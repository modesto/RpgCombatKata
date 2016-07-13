using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class FactionRulesShould : TestBase
    {
        [Test]
        public void allow_a_character_to_join_a_faction() {
            var character = Given.ALiveCharacter();
            var aFaction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            var joinFaction = Given.ATriedToJoinFaction(character.Id, aFaction.Id);
            When.Raised(joinFaction);
            aFaction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void a_character_can_not_join_a_faction_twice()
        {
            var character = Given.ALiveCharacter();
            var aFaction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            var joinFaction = Given.ATriedToJoinFaction(character.Id, aFaction.Id);
            When.Raised(joinFaction);
            When.Raised(joinFaction);
            aFaction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void a_character_can_leave_a_faction() {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            var joinFaction = Given.ATriedToJoinFaction(character.Id, faction.Id);
            var leaveFaction = Given.ATriedToLeaveFactionAction(character.Id, faction.Id);
            When.Raised(joinFaction);
            faction.TotalMembers.Should().Be(1);
            When.Raised(leaveFaction);
            faction.TotalMembers.Should().Be(0);
        }

        [Test]
        public void a_character_can_heal_an_ally()
        {
            var character = Given.ALiveCharacter();
            var anAlly = Given.ALiveCharacter(healthPoints: 900);
            var faction = Given.AFaction();
            var factionRules = Given.AFactionCombatRules(faction);
            var rulesEngine = Given.ARulesEngine(factionRules);
            var joinFaction = Given.ATriedToJoinFaction(character.Id, faction.Id);
            var allyJoinFaction = Given.ATriedToJoinFaction(anAlly.Id, faction.Id);
            var triedToHeal = Given.ATriedToHealEvent(character.Id, anAlly.Id, 50);
            When.Raised(joinFaction);
            When.Raised(allyJoinFaction);
            When.Raised(triedToHeal);
            anAlly.HealthCondition.CurrentHealth.Should().Be(950);
        }
    }
}
