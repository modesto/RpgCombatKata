namespace RpgCombatKata.Core.Business.Structures {
    public class Structure
    {
        public DurabilityCondition DurabilityCondition { get; }
        public GameEntityIdentity Id { get; }

        public Structure(GameEntityIdentity structureId, DurabilityCondition durabilityCondition)
        {
            Id = structureId;
            DurabilityCondition = durabilityCondition;
        }
    }
}