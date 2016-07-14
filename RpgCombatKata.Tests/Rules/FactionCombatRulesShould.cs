using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class FactionCombatRulesShould : TestBase
    {

        [Test]
        public void allow_a_character_heal_an_ally()
        {
            var character = Given.ALiveCharacter();
            var anAlly = Given.ALiveCharacter(healthPoints: 900);
            var faction = Given.AFaction();
            var factionRules = Given.AFactionCombatRules(faction);
            Given.ARulesPipeline(factionRules);
            When.TriedToJoinFaction(character.Id, faction.Id);
            When.TriedToJoinFaction(anAlly.Id, faction.Id);
            When.TriedToHeal(character.Id, anAlly.Id, 50);
            anAlly.HealthCondition.CurrentHealth.Should().Be(950);
        }

        [Test]
        public void forbide_a_character_to_heal_an_enemy()
        {
            var factionCombatRules = Given.AFactionCombatRules();
            Given.ARulesPipeline(factionCombatRules);
            var healer = Given.ALiveCharacter();
            var enemy = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = enemy.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            When.TriedToHeal(healer.Id, enemy.Id, heal: pointsToHeal);
            enemy.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void forbide_a_character_attack_an_ally()
        {
            var faction = Given.AFaction();
            var factionRules = Given.AFactionCombatRules(faction);
            Given.ARulesPipeline(factionRules);
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
