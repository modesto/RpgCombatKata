using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgCombatKata.Core.Model.Events
{
    public class TriedToAttack : GameEvent
    {
        public Character Attacker { get; }
        public int Damage { get; }
        public Character Defender { get; }

        public TriedToAttack(Character attacker, Character defender, int damage)
        {
            this.Attacker = attacker;
            this.Defender = defender;
            this.Damage = damage;
        }
    }
}
