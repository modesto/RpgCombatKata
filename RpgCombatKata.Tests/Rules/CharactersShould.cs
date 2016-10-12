using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RpgCombatKata.Core;
using RpgCombatKata.Core.Business.Characters;

namespace RpgCombatKata.Tests.Rules {
    public class CharactersShould : TestBase {
        [Test]
        public void be_able_to_join_the_game()
        {
            var character = Given.ALiveCharacter();
            var charactersRepository = Given.ACharactersRepository();
            Given.ANewGameEngine(charactersRepository: charactersRepository);
            When.TriedToJoinGame(character);
            charactersRepository.GetCharacter(character.Id).Id.Should().Be(character.Id);
        }
    }
}
