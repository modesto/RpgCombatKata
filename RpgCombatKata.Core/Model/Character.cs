using System;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Core.Model {
    public class Character : LivingBeing {
        public Character(string uid, HealthCondition healthCondition, int level = 1)
        {
            HealthCondition = healthCondition;
            Id = uid;
            Level = level;
        }
        public int Level { get; }

        public string Id { get; }
        public HealthCondition HealthCondition { get; }
    }
}