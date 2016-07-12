using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using RpgCombatKata.Core.Model.Actions;

namespace RpgCombatKata.Core.Model
{
    public class Faction
    {
        private readonly List<string> members = new List<string>();
        public string Name { get; }
        public int TotalMembers => members.Count;

        public Faction(string name, IObservable<JoinFaction> joinFactionObservable, IObservable<LeaveFaction> leaveFactionObservable)
        {
            this.Name = name;
            joinFactionObservable.Where(faction => faction.FactionName == name).Subscribe(JoinFaction);
            leaveFactionObservable.Where(faction => faction.FactionName == name).Subscribe(LeaveFaction);
        }

        private void JoinFaction(JoinFaction action) {
            if (members.Contains(action.CharacterId)) return;
            members.Add(action.CharacterId);
        }

        private void LeaveFaction(LeaveFaction action) {
            if (members.Contains(action.CharacterId)) members.Remove(action.CharacterId);
        }
    }
}
