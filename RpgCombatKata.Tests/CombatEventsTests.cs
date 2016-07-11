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
    }
}
