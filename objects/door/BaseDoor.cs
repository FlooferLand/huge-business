using Godot;

namespace Project
{
    public abstract class BaseDoor : Spatial, IBaseUnlockable
    {
        protected Spatial outOffset;

        // Export Variables
        [Export] public bool locked = false;
        [Export] public PackedScene levelTransition;

        // Variables
        public DoorSide currentSide = null;

        public abstract void Interact(DoorSide side);

        public override void _Ready() {
            base._Ready();
            outOffset = GetNode<Spatial>("OutOffset");
        }

        public void Unlock()
        {
            locked = false;
        }

        public bool IsLocked() {
            return locked;
        }
    }
}
