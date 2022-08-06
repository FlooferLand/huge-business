using Godot;

namespace Project
{
    public class InteractionProgress : ProgressBar
    {
        public override void _Ready()
        {
            Global.interactionProgress = this;
        }

        public void AddProgress(float amount)
        {
            Value += amount;
        }

        public void SetProgress(float amount)
        {
            Value = amount;
        }
    }
}
