namespace RpgCombatKata.Core.Business {
    public class GameEntityIdentity {
        private readonly string id;

        public GameEntityIdentity(string id) {
            this.id = id;
        }

        public override bool Equals(object obj) {
            var otherCharacter = obj as GameEntityIdentity;
            return otherCharacter?.id == id;
        }

        protected bool Equals(GameEntityIdentity other) {
            return string.Equals(id, other.id);
        }

        public override int GetHashCode() {
            return id?.GetHashCode() ?? 0;
        }

        public static bool operator ==(GameEntityIdentity a, GameEntityIdentity b) {
            return a?.id == b?.id;
        }

        public static bool operator !=(GameEntityIdentity a, GameEntityIdentity b) {
            return !(a == b);
        }
    }


    public class NoGameEntityIdentity : GameEntityIdentity {
        public NoGameEntityIdentity() : base(string.Empty) {}
    }
}