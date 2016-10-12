using FluentAssertions;
using NUnit.Framework;

namespace RpgCombatKata.Tests.Rules
{
    [TestFixture]
    public class FactionCombatRulesShould : TestBase
    {
        [Test]
        public void allow_a_character_heal_an_ally()
        {
            var faction = Given.AFaction();
            var factionRepository = Given.AFactionRepository(faction);
            Given.ANewGameEngine(factionRepository);
            var initialHealth = 900;
            var pointsToHeal = 50;
            var character = Given.ALiveCharacter();
            var anAlly = Given.ALiveCharacter(healthPoints: initialHealth);
            When.TriedToJoinFaction(character.Id, faction.Id);
            When.TriedToJoinFaction(anAlly.Id, faction.Id);
            When.TriedToHeal(character.Id, anAlly.Id, pointsToHeal);
            anAlly.HealthCondition.CurrentHealth.Should().Be(initialHealth+pointsToHeal);
        }

        [Test]
        public void forbide_a_character_to_heal_an_enemy()
        {
            var initialHealth = 900;
            var pointsToHeal = 50;
            Given.ANewGameEngine();
            var healer = Given.ALiveCharacter();
            var enemy = Given.ALiveCharacter(healthPoints: initialHealth);
            When.TriedToHeal(healer.Id, enemy.Id, heal: pointsToHeal);
            enemy.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

        [Test]
        public void forbide_a_character_attack_an_ally()
        {
            var faction = Given.AFaction();
            var factionRepository = Given.AFactionRepository(faction);
            Given.ANewGameEngine(factionRepository);
            var attacker = Given.ALiveCharacter();
            var defender = Given.ALiveCharacter();
            var initialHealth = defender.HealthCondition.CurrentHealth;
            When.TriedToJoinFaction(attacker.Id, faction.Id);
            When.TriedToJoinFaction(defender.Id, faction.Id);
            var damage = 100;
            When.TriedToAttack(attacker.Id, defender.Id, damage: damage);
            defender.HealthCondition.CurrentHealth.Should().Be(initialHealth);
        }

    }
}
