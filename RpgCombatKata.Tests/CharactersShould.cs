﻿using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core.Model;
using Given = RpgCombatKata.Tests.TestFixtures;
using When = RpgCombatKata.Tests.TestFixtures;

namespace RpgCombatKata.Tests
{
    [TestFixture]
    public class CharactersShould : TestBase
    {
        [Test]
        public void start_with_1000_health_points() {
            var character = Given.ALiveCharacter();
            character.HealthCondition.CurrentHealth.Should().Be(1000);
        }

        [Test]
        public void receive_damage() {
            var aCharacter = Given.ALiveCharacter();
            var attack = Given.ASuccessAttack(to: aCharacter.Id, damage: 100);
            When.Raised(attack);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(900);
        }

        [Test]
        public void receive_only_damage_which_is_target()
        {
            var aCharacter = Given.ALiveCharacter();
            var anotherCharacter = Given.ALiveCharacter();
            var attack = Given.ASuccessAttack(to: aCharacter.Id, damage: 100);
            When.Raised(attack);
            anotherCharacter.HealthCondition.CurrentHealth.Should().Be(1000);
        }

        [Test]
        public void be_healed() {
            var aCharacter = Given.ALiveCharacter(healthPoints : 900);
            var heal = Given.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            When.Raised(heal);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(950);
        }

        [Test]
        public void be_healed_only_if_is_target()
        {
            var aCharacter = Given.ALiveCharacter(healthPoints: 900);
            var anotherCharacter = Given.ALiveCharacter(healthPoints: 900);
            var heal = Given.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            When.Raised(heal);
            anotherCharacter.HealthCondition.CurrentHealth.Should().Be(900);
        }

        [Test]
        public void not_be_healed_if_born_dead() {
            var aDeadCharacter = Given.ADeadCharacter();
            var heal = Given.ASuccessHeal(to: aDeadCharacter.Id, healingPoints: 50);
            When.Raised(heal);
            aDeadCharacter.HealthCondition.CurrentHealth.Should().Be(0);
        }

        [Test]
        public void not_be_healed_after_dead()
        {
            var aCharacter = Given.ALiveCharacter(healthPoints: 50);
            var attack = Given.ASuccessAttack(to: aCharacter.Id, damage: 60);
            When.Raised(attack);
            var heal = Given.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            When.Raised(heal);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(0);
        }

        [Test]
        public void not_be_healed_over_max_life() {
            var aCharacter = Given.ALiveCharacter();
            var expectedHealth = aCharacter.HealthCondition.CurrentHealth;
            var heal = Given.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            When.Raised(heal);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(expectedHealth);
        }

    }
}
