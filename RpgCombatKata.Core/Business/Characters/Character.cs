namespace RpgCombatKata.Core.Business.Characters {
    public class Character : LivingBeing {
        public Character(GameEntityIdentity uid, HealthCondition healthCondition, int level = 1)
        {
            HealthCondition = healthCondition;
            Id = uid;
            Level = level;
        }
        public int Level { get; }

        public GameEntityIdentity Id { get; }
        public HealthCondition HealthCondition { get; }
    }
}