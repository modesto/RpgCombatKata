using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Model.Characters;


namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class LevelBasedCombatRulesShould : TestBase
    {
        [Test]
        public void attacks_do_more_damage_to_low_level_characters() {
            var attacker = Given.ALiveCharacter(level: 10);
            var defender = Given.ALiveCharacter(level: 5);
            List<Character> charactersStubData = new List<Character>() {attacker, defender};
            var combatRules = Given.ACombatRules();
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<Core.Model.Rules.Rules>() {combatRules, levelBasedCombatRules};
            var rulesEngine = Given.ARulesEngine(rules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = (int)(damage * 1.5);
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void make_attacks_do_less_damage_to_high_level_characters()
        {
            var attacker = Given.ALiveCharacter(level: 5);
            var defender = Given.ALiveCharacter(level: 10);
            List<Character> charactersStubData = new List<Character>() { attacker, defender };
            var combatRules = Given.ACombatRules();
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<Core.Model.Rules.Rules>() { combatRules, levelBasedCombatRules };
            var rulesEngine = Given.ARulesEngine(rules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = (int)(damage - (damage * 0.5));
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

    }
}
