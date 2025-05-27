using Godot;

namespace Project
{
    public class EscapeDoorBlockage : StaticBody, IBaseInteractable, IBaseProgressable
    {
        // Signals
        [Signal] public delegate void on_interact();

        // Nodes
        AudioStreamPlayer3D streamPlayer;
        Particles particles;

        // Variables
        public int progress = 0;

        public override void _Ready()
        {
            streamPlayer = GetNode<AudioStreamPlayer3D>("Audio");
            particles = GetNode<Particles>("Particles");
        }

        public override void _Process(float delta)
        {
            if (progress < 100)
                return;

            Global.interactionProgress.SetProgress(0);
            streamPlayer.UnitDb = 0;
            streamPlayer.Play();
            QueueFree();
        }

        public void Interact()
        {
            progress += 1;
            Global.interactionProgress.AddProgress(1);
            streamPlayer.PitchScale = Global.rng.RandfRange(0.95f, 1.05f);
            streamPlayer.Play();
            particles.Emitting = true;
            EmitSignal(nameof(on_interact));
        }
    }
}