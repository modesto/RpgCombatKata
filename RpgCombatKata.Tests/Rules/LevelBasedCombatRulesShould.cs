using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Business.Characters;


namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class LevelBasedCombatRulesShould : TestBase
    {
        [Test]
        public void attacks_do_more_damage_to_low_level_characters() {
            var attacker = Given.ALiveCharacter(level: 10);
            var defender = Given.ALiveCharacter(level: 5);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = damage.IncreaseIn(50.Percent());
            var charactersStubData = new List<Character>() {attacker, defender};
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var combatRules = Given.ACombatRules();
            var rules = new List<Core.Business.Rules.Rules>() {combatRules, levelBasedCombatRules};
            Given.ARulesPipeline(rules);
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void make_attacks_do_less_damage_to_high_level_characters()
        {
            var attacker = Given.ALiveCharacter(level: 5);
            var defender = Given.ALiveCharacter(level: 10);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = damage.DecreaseIn(50.Percent());
            var combatRules = Given.ACombatRules();
            var charactersStubData = new List<Character>() { attacker, defender };
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<Core.Business.Rules.Rules>() { combatRules, levelBasedCombatRules };
            Given.ARulesPipeline(rules);
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }
    }
}
