using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Model.Characters;
using RpgCombatKata.Core.Model.Combat;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class CombatRulesShould : TestBase
    {
        [Test]
        public void allow_a_character_attack_another_character() {
            var combatRules = Given.ACombatRules();
            var rulesEngine = Given.ARulesEngine(combatRules);
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
            var rulesEngine = Given.ARulesEngine(combatRules);
            var attacker = Given.ALiveCharacter();
            var initialHealth = attacker.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, attacker.Id, damage: damage);
            attacker.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }


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

    public static class AttackRanges {
        public static AttackRange Melee() {
            return new MeleeAttack();
        }

        public static AttackRange Range() {
            return new RangedAttack();
        }
    }
}
