using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Entities
{
    [TestFixture]
    public class StrusturesShould : TestBase
    {
        [Test]
        public void start_with_given_health_points() {
            var structure = Given.AStructure(durability: 2000);
            structure.DurabilityCondition.CurrentDurability.Should().Be(2000);
        }

        [Test]
        public void receive_damage() {
            var structure = Given.AStructure(durability: 2000);
            When.ASuccessAttack(to: structure.Id, damage: 100);
            structure.DurabilityCondition.CurrentDurability.Should().Be(1900);
        }

        [Test]
        public void not_be_healed()
        {
            var structure = Given.AStructure(durability: 900);
            When.ASuccessHeal(to: structure.Id, healingPoints: 50);
            structure.DurabilityCondition.CurrentDurability.Should().Be(900);
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
