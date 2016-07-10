using System;
using Microsoft.SqlServer.Server;

namespace RpgCombatKata.Core.Model {
    public class Character {
        private const int MaxHealth = 1000;

        public Character() {
            Health = MaxHealth;
            Id = Guid.NewGuid().ToString();
        }
        public int Health { get; }
        public string Id { get; }
    }
}