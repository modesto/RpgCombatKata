using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RpgCombatKata.Core.Model;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class CombatEventsTests : TestBase
    {
        [Test]
        public void a_character_can_attack_another_character() {
            var combatRules = Given.ACombatRules();
            var rulesEngine = Given.ARulesEngine(combatRules);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage);
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth-damage);
        }

        [Test]
        public void a_character_can_not_attack_to_himself()
        {
            var combatRules = Given.ACombatRules();
            var rulesEngine = Given.ARulesEngine(combatRules);
            var attacker = Given.ALiveCharacter();
            var initialHealth = attacker.HealthCondition.CurrentHealth;
            var damage = 100;
            var attack = Given.ATriedToAttackEvent(attacker.Id, attacker.Id, damage: damage);
            When.Raised(attack);
            attacker.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void a_character_can_heal_to_himself() {
            var healingRules = Given.AHealingRules();
            var rulesEngine = Given.ARulesEngine(healingRules);
            var healer = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = healer.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            var heal = Given.ATriedToHealEvent(healer.Id, healer.Id, heal: pointsToHeal);
            When.Raised(heal);
            healer.HealthCondition.CurrentHealth.Should().Be(initialHealth + pointsToHeal);
        }

        [Test]
        public void a_character_can_not_heal_to_an_enemy()
        {
            var healingRules = Given.AHealingRules();
            var rulesEngine = Given.ARulesEngine(healingRules);
            var healer = Given.ALiveCharacter();
            var enemy = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = enemy.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            var heal = Given.ATriedToHealEvent(healer.Id, enemy.Id, heal: pointsToHeal);
            When.Raised(heal);
            enemy.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void attacks_do_more_damage_to_low_level_characters() {
            var attacker = Given.ALiveCharacter(level: 10);
            var defender = Given.ALiveCharacter(level: 5);
            List<Character> charactersStubData = new List<Character>() {attacker, defender};
            var combatRules = Given.ACombatRules();
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<GameRules>() {combatRules, levelBasedCombatRules};
            var rulesEngine = Given.ARulesEngine(rules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = (int)(damage * 1.5);
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage);
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void attacks_do_less_damage_to_high_level_characters()
        {
            var attacker = Given.ALiveCharacter(level: 5);
            var defender = Given.ALiveCharacter(level: 10);
            List<Character> charactersStubData = new List<Character>() { attacker, defender };
            var combatRules = Given.ACombatRules();
            var levelBasedCombatRules = Given.ALevelBasedCombatRules(charactersStubData);
            var rules = new List<GameRules>() { combatRules, levelBasedCombatRules };
            var rulesEngine = Given.ARulesEngine(rules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var expectedDamage = (int)(damage - (damage * 0.5));
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage);
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void melee_attack_must_be_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(2));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage);
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - damage);
        }

        [Test]
        public void melee_attack_must_fail_if_is_not_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(3));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Melee());
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void range_attack_must_be_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(20));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesEngine = Given.ARulesEngine(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            var attack = Given.ATriedToAttackEvent(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            When.Raised(attack);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth- damage);
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
