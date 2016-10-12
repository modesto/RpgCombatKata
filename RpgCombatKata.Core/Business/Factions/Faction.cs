using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RpgCombatKata.Core.Business.Factions
{
    public class Faction
    {
        private readonly List<GameEntityIdentity> members = new List<GameEntityIdentity>();
        public FactionIdentity Id { get; }
        public int TotalMembers => members.Count;

		public Faction(FactionIdentity id, IObservable<SuccessTo<JoinFaction>> joinFactionObservable, IObservable<SuccessTo<LeaveFaction>> leaveFactionObservable ) {
            Id = id;
            joinFactionObservable.Where(gameEvent => gameEvent.Event.FactionId == id).Subscribe(JoinFaction);
            leaveFactionObservable.Where(gameEvent => gameEvent.Event.FactionId == id).Subscribe(LeaveFaction);
        }

        private void JoinFaction(SuccessTo<JoinFaction> action)
        {
            if (members.Contains(action.Event.CharacterId)) return;
            members.Add(action.Event.CharacterId);
        }

        private void LeaveFaction(SuccessTo<LeaveFaction> action)
        {
            if (members.Contains(action.Event.CharacterId)) members.Remove(action.Event.CharacterId);
        }

        public bool AreAllies(GameEntityIdentity aCharacterId, GameEntityIdentity anotherCharacterId) {
            return (members.Contains(aCharacterId) && members.Contains(anotherCharacterId));
        }
    }
}
