using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class CombatRulesShould : TestBase
    {
        [Test]
        public void allow_a_character_attack_another_character() {
            var combatRules = Given.ACombatRules();
            Given.ARulesPipeline(combatRules);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth-damage);
        }

        [Test]
        public void forbide_a_character_attack_to_himself()
        {
            var combatRules = Given.ACombatRules();
            Given.ARulesPipeline(combatRules);
            var attacker = Given.ALiveCharacter();
            var initialHealth = attacker.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, attacker.Id, damage: damage);
            attacker.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }
    }
}
