using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RpgCombatKata.Core.Model.Characters;
using RpgCombatKata.Core.Model.Combat;
using RpgCombatKata.Core.Model.Map;
using RpgCombatKata.Core.Model.Rules;

namespace RpgCombatKata.Tests
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
        public void allow_a_character_to_heal_himself() {
            var rulesEngine = Given.ARulesEngine();
            var healer = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = healer.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            When.TriedToHeal(healer.Id, healer.Id, heal: pointsToHeal);
            healer.HealthCondition.CurrentHealth.Should().Be(initialHealth + pointsToHeal);
        }

        [Test]
        public void attacks_do_more_damage_to_low_level_characters() {
            var attacker = Given.ALiveCharacter(level: 10);
            var defender = Given.ALiveCharacter(level: 5);
            List<Character> charactersStubData = new List<Character>() {attacker, defender};
            var combatRules = Given.ACombatRules();
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<Rules>() {combatRules, levelBasedCombatRules};
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
            var rules = new List<Rules>() { combatRules, levelBasedCombatRules };
            var rulesEngine = Given.ARulesEngine(rules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = (int)(damage - (damage * 0.5));
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void allow_melee_attacks_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(2));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - damage);
        }

        [Test]
        public void forbide_melee_attacks_out_of_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(3));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Melee());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void allow_range_attacks_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(20));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth- damage);
        }

        [Test]
        public void forbide_range_attacks_out_of_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(21));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void forbide_a_character_to_heal_an_enemy()
        {
            var factionCombatRules = Given.AFactionCombatRules();
            var rulesEngine = Given.ARulesEngine(factionCombatRules);
            var healer = Given.ALiveCharacter();
            var enemy = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = enemy.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            When.TriedToHeal(healer.Id, enemy.Id, heal: pointsToHeal);
            enemy.HealthCondition.CurrentHealth.Should().Be(initialHealth);
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
