﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgCombatKata.Core.Model
{
    public interface CharactersRepository {
        Character GetCharacter(string id);
    }
}
