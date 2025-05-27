using Godot;

namespace Project
{
    public class EmergencyLight : Spatial
    {
        // Nodes
        public Light light;
        public Timer switchTimer;

        // Export Variables
        [Export] public float lightIntensity = 2f;

        // Variables
        public bool animState = true;
        public float initialWaitTime;

        public override void _Ready()
        {
            light = GetNode<Light>("Light");
            light.LightEnergy = 0f;

            switchTimer = GetNode<Timer>("SwitchTimer");
            switchTimer.Connect("timeout", this, nameof(_SwitchState));
            initialWaitTime = switchTimer.WaitTime;
            switchTimer.WaitTime = Global.rng.RandfRange(
                initialWaitTime - 0.7f,
                initialWaitTime + 0.7f
            );
        }

        public override void _Process(float delta)
        {
            light.LightEnergy = Mathf.Lerp(
                light.LightEnergy,
                (animState) ? 1f : 0f,
                10f * delta
            );
        }

        public void _SwitchState()
        {
            animState = !animState;
        }
    }
}