using Godot;

namespace Project
{
    public class PaperPickup : StaticBody, IBaseInteractable
    {
        // Export Variables
        [Export(PropertyHint.MultilineText)] public string text = "None";
        [Export] public bool disappearsOnPickup = false;

        public override void _Ready()
        {
            RotationDegrees = new Vector3(
                RotationDegrees.x,
                RotationDegrees.y,
                Global.rng.RandfRange(-15f, 15f)
            );
        }

        public void Interact()
        {
            Global.paperHandler.Write(text);
            if (disappearsOnPickup)
                QueueFree();
        }
    }
}