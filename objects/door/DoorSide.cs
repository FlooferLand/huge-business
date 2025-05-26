using Godot;

namespace Project
{
    public class DoorSide : StaticBody, IBaseInteractable
    {
        // Nodes
        BaseDoor parent;

        // Variables
        public enum SIDES { Front, Back }
        [Export(PropertyHint.Enum)] public SIDES id;

        public override void _Ready()
        {
            parent = GetParent<BaseDoor>();
        }

        public void Interact()
        {
            parent.Interact(this);
        }
    }
}