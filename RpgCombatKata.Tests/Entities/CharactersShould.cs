using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Entities
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
            When.ASuccessAttack(to: aCharacter.Id, damage: 100);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(900);
        }

        [Test]
        public void no_receive_damage_to_other_targets()
        {
            var aCharacter = Given.ALiveCharacter();
            var anotherCharacter = Given.ALiveCharacter();
            When.ASuccessAttack(to: aCharacter.Id, damage: 100);
            anotherCharacter.HealthCondition.CurrentHealth.Should().Be(1000);
        }

        [Test]
        public void be_healed() {
            var aCharacter = Given.ALiveCharacter(healthPoints : 900);
            When.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(950);
        }

        [Test]
        public void not_be_healed_if_is_not_the_target()
        {
            var aCharacter = Given.ALiveCharacter(healthPoints: 900);
            var anotherCharacter = Given.ALiveCharacter(healthPoints: 900);
            When.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            anotherCharacter.HealthCondition.CurrentHealth.Should().Be(900);
        }

        [Test]
        public void not_be_healed_if_born_dead() {
            var aDeadCharacter = Given.ADeadCharacter();
            When.ASuccessHeal(to: aDeadCharacter.Id, healingPoints: 50);
            aDeadCharacter.HealthCondition.CurrentHealth.Should().Be(0);
        }

        [Test]
        public void not_be_healed_after_dead()
        {
            var aCharacter = Given.ALiveCharacter(healthPoints: 50);
            When.ASuccessAttack(to: aCharacter.Id, damage: 60);
            When.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(0);
        }

        [Test]
        public void not_be_healed_over_max_life() {
            var aCharacter = Given.ALiveCharacter();
            var expectedHealth = aCharacter.HealthCondition.CurrentHealth;
            When.ASuccessHeal(to: aCharacter.Id, healingPoints: 50);
            aCharacter.HealthCondition.CurrentHealth.Should().Be(expectedHealth);
        }


        [Test]
        public void allow_a_character_to_heal_himself()
        {
            var rulesPipeline = Given.ARulesPipeline();
            var healer = Given.ALiveCharacter(healthPoints: 900);
            var initialHealth = healer.HealthCondition.CurrentHealth;
            var pointsToHeal = 50;
            When.TriedToHeal(healer.Id, healer.Id, heal: pointsToHeal);
            healer.HealthCondition.CurrentHealth.Should().Be(initialHealth + pointsToHeal);
        }

    }
}
