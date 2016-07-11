using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class CombatEventsTests : TestBase
    {
        [Test]
        public void a_character_can_attack_another_character() {
            var gameEngine = Given.AGameEngine();
            var attacker = Given.ACharacter();
            var defender = Given.ACharacter();
            var initialHealth = defender.Health;
            var damage = 100;
            var tryToAttack = Given.ATriedToAttackEvent(attacker: attacker, defender: defender, damage: damage);
            When.Raised(tryToAttack);
            defender.Health.Should().Be(initialHealth-damage);
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

    }
}
