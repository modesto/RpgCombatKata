namespace RpgCombatKata.Core.Model.Events {
    public class TriedToHeal : GameEvent
    {
        public int Heal { get; }
        public Character Source { get; }
        public Character Target { get; }

        public TriedToHeal(Character source, Character target, int heal)
        {
            Source = source;
            Target = target;
            Heal = heal;
        }
    }
}