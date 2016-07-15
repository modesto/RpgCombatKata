using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Entities
{
    [TestFixture]
    public class StrusturesShould : TestBase
    {
        [Test]
        public void start_with_given_health_points() {
            var expectedDurability = 2000;
            var structure = Given.AStructure(durability: expectedDurability);
            structure.DurabilityCondition.CurrentDurability.Should().Be(expectedDurability);
        }

        [Test]
        public void receive_damage() {
            var structure = Given.AStructure(durability: 2000);
            var initialDurability = 2000;
            var attackDamage = 100;
            When.ASuccessAttack(to: structure.Id, damage: attackDamage);
            structure.DurabilityCondition.CurrentDurability.Should().Be(initialDurability-attackDamage);
        }

        [Test]
        public void not_be_healed()
        {
            var expectedDurability = 900;
            var structure = Given.AStructure(durability: expectedDurability);
            When.ASuccessHeal(to: structure.Id, healingPoints: 50);
            structure.DurabilityCondition.CurrentDurability.Should().Be(expectedDurability);
        }

        [Test]
        public void not_receive_damage_under_minimum()
        {
            var structure = Given.AStructure(durability: 30);
            When.ASuccessAttack(to: structure.Id, damage: 100);
            structure.DurabilityCondition.CurrentDurability.Should().Be(0);
        }
    }
}
