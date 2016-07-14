namespace RpgCombatKata.Core.Business.Structures {
    public class Structure
    {
        public DurabilityCondition DurabilityCondition { get; }
        public string Id { get; }

        public Structure(string structureId, DurabilityCondition durabilityCondition)
        {
            Id = structureId;
            DurabilityCondition = durabilityCondition;
        }
    }
}