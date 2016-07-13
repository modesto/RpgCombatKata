using System;
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
            var gameEngine = Given.AGameEngine();
            var attacker = Given.ACharacter();
            var initialHealth = attacker.Health;
            var damage = 100;
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: attacker, damage: damage);
            When.Raised(tryToAttack);
            attacker.Health.Should().Be(initialHealth);
        }

        [Test]
        public void a_character_can_heal_to_himself() {
            var gameEngine = Given.AGameEngine();
            var character = Given.ACharacter(900);
            var heal = 50;
            var tryToHeal = Given.ATriedToHealEvent(source: character, target: character, heal: heal);
            When.Raised(tryToHeal);
            character.Health.Should().Be(950);
        }

        [Test]
        public void a_character_can_not_heal_to_an_enemy()
        {
            var gameEngine = Given.AGameEngine();
            var character = Given.ACharacter();
            var enemy = Given.ACharacter(900);
            var heal = 50;
            var tryToHeal = Given.ATriedToHealEvent(source: character, target: enemy, heal: heal);
            When.Raised(tryToHeal);
            enemy.Health.Should().Be(900);
        }

        [Test]
        public void attacks_do_more_damage_to_low_level_characters() {
            var gameEngine = Given.AGameEngine();
            var attacker = Given.ACharacter(level: 10);
            var defender = Given.ACharacter(level: 5);
            var initialHealth = defender.Health;
            var damage = 100;
            var expectedDamage = (int)(damage * 1.5);
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage);
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void attacks_do_less_damage_to_high_level_characters()
        {
            var gameEngine = Given.AGameEngine();
            var attacker = Given.ACharacter(level: 5);
            var defender = Given.ACharacter(level: 10);
            var initialHealth = defender.Health;
            var damage = 100;
            var expectedDamage = (int)(damage - (damage * 0.5));
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage);
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth - expectedDamage);
        }

        [Test]
        public void melee_attack_must_be_in_range()
        {
            var gameEngine = Given.AGameEngine();
            var gameMap = Given.AGameMap();
            var attacker = Given.ACharacter();
            var defender = Given.ACharacter();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(2));
            var initialHealth = defender.Health;
            var damage = 100;
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage);
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth - damage);
        }

        [Test]
        public void melee_attack_must_fail_if_is_not_in_range()
        {
            var gameMap = Given.AGameMap();
            var gameEngine = Given.AGameEngine(gameMap);
            var attacker = Given.ACharacter();
            var defender = Given.ACharacter();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(3));
            var initialHealth = defender.Health;
            var damage = 100;
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage);
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth);
        }

        [Test]
        public void range_attack_must_be_in_range()
        {
            var gameMap = Given.AGameMap();
            var gameEngine = Given.AGameEngine(gameMap);
            var attacker = Given.ACharacter();
            var defender = Given.ACharacter();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(20));
            var initialHealth = defender.Health;
            var damage = 100;
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage, kind: AttackRanges.Range());
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth - damage);
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
