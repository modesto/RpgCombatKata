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
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void a_character_can_not_join_a_faction_twice()
        {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
        }

        [Test]
        public void a_character_can_leave_a_faction() {
            var character = Given.ALiveCharacter();
            var faction = Given.AFaction();
            var rulesEngine = Given.ARulesEngine();
            When.TriedToJoinFaction(character.Id, faction.Id);
            faction.TotalMembers.Should().Be(1);
            When.TriedToLeaveFaction(character.Id, faction.Id);
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
            When.TriedToJoinFaction(character.Id, faction.Id);
            When.TriedToJoinFaction(anAlly.Id, faction.Id);
            When.TriedToHeal(character.Id, anAlly.Id, 50);
            anAlly.HealthCondition.CurrentHealth.Should().Be(950);
        }

        [Test]
        public void a_character_can_not_attack_an_ally()
        {
            var faction = Given.AFaction();
            var factionRules = Given.AFactionCombatRules(faction);
            var rulesEngine = Given.ARulesEngine(factionRules);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            When.TriedToJoinFaction(attacker.Id, faction.Id);
            When.TriedToJoinFaction(defender.Id, faction.Id);
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }
    }
}
