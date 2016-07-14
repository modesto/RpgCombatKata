﻿using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RpgCombatKata.Core.Business.Map;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class MapBasedCombatRulesShould : TestBase
    {

        [Test]
        public void allow_melee_attacks_in_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(2));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesPipeline = Given.ARulesPipeline(mapRules);
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
            var rulesPipeline = Given.ARulesPipeline(mapRules);
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
            var rulesPipeline = Given.ARulesPipeline(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth - damage);
        }

        [Test]
        public void forbide_range_attacks_out_of_range()
        {
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var gameMap = Given.AGameMap();
            gameMap.DistanceBetween(attacker.Id, defender.Id).Returns(Distance.FromMeters(21));
            var mapRules = Given.AMapBasedCombatRules(gameMap);
            var rulesPipeline = Given.ARulesPipeline(mapRules);
            var initialHealth = defender.HealthCondition.CurrentHealth;
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage, kind: AttackRanges.Range());
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }
    }
}