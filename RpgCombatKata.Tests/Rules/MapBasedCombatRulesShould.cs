using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RpgCombatKata.Core.Business.Map;
using RpgCombatKata.Tests.Fixtures;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class MapBasedCombatRulesShould : TestBase
    {

        [Test]
        public void allow_melee_attacks_in_range()
        {
            var gameMap = Given.AGameMap();
            Given.ANewGameEngine(gameMap: gameMap);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(2.Meters());
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - damage);
        }

        [Test]
        public void forbide_melee_attacks_out_of_range()
        {
            var gameMap = Given.AGameMap();
            Given.ANewGameEngine(gameMap: gameMap);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(3.Meters());
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Melee());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void allow_range_attacks_in_range()
        {
            var gameMap = Given.AGameMap();
            Given.ANewGameEngine(gameMap: gameMap);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(20.Meters());
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - damage);
        }

        [Test]
        public void forbide_range_attacks_out_of_range()
        {
            var gameMap = Given.AGameMap();
            Given.ANewGameEngine(gameMap: gameMap);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(21.Meters());
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }
    }
}
