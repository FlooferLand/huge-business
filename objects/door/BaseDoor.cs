using Godot;

namespace Project
{
    public abstract class BaseDoor : Spatial, IBaseUnlockable
    {
        // Export Variables
        [Export] public bool locked = false;
        [Export] public PackedScene levelTransition;

        // Variables
        public DoorSide currentSide = null;

        public abstract void Interact(DoorSide side);

        public void Unlock()
        {
            locked = false;
            Interact(currentSide);
        }
    }
}
