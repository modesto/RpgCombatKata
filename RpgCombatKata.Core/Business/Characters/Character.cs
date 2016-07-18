namespace RpgCombatKata.Core.Business.Characters {
    public class Character : LivingBeing {
        public Character(CharacterIdentity uid, HealthCondition healthCondition, int level = 1)
        {
            HealthCondition = healthCondition;
            Id = uid;
            Level = level;
        }
        public int Level { get; }

        public CharacterIdentity Id { get; }
        public HealthCondition HealthCondition { get; }
    }
}