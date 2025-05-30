using Godot;

namespace Project
{
    public class TheFacilityEvents : Node
    {
        // Nodes
        public Spatial events;
        public AnimationPlayer animPlayer;
        public StaticBody escapeDoorBlockage;
        public Spatial lockpickHint;
        public Spatial ventAttentionLight;
        public Camera dummyPlayerCamera;
        public Camera jumpscareCamera;

        public override void _Ready()
        {
            events = GetNode<Spatial>("Events");
            animPlayer = GetNode<AnimationPlayer>("Building/Enemies/AnimPlayer");
            animPlayer.Connect("animation_finished", this, nameof(_OnAnimFinished));
            escapeDoorBlockage = GetNode<StaticBody>("Building/Props/EscapeDoorBlockage");
            lockpickHint = GetNode<Spatial>("Building/Props/LockpickHint");
            ventAttentionLight = GetNode<Spatial>("Building/Lights/VentAttentionLight");
            dummyPlayerCamera = GetNode<Camera>("Building/Enemies/DummyPlayer/Camera");
            jumpscareCamera = GetNode<Camera>("Building/Enemies/Jumpscare/Camera");
            Global.facilityEvents = this;

            // Something i have to do because Godot is annoying and makes it so when it detects there's no camera set as "current" it forcefully makes a random camera current.
            dummyPlayerCamera.ClearCurrent(false);
            jumpscareCamera.ClearCurrent(false);

            // signal signal signal signal signal signal signal signal signal signal signal signal
            escapeDoorBlockage.Connect(nameof(EscapeDoorBlockage.on_interact), this, nameof(_OnEscapeDoorBlockageInteract));

            // Intro events go brr
            Global.player.Speak(SoundEffects.playerVoiceLines[0], "Thank Huge Business, i am safe now.");
        }

        public void _OnAnimFinished(string animName)
        {
            if (animName == "RESET")
                return;

            switch (animName)
            {
                case "Jumpscare":
                    GetTree().ReloadCurrentScene();
                    break;
                case "Intro":
                    escapeDoorBlockage.Translation = new Vector3(
                        escapeDoorBlockage.Translation.x,
                        2,
                        escapeDoorBlockage.Translation.z
                    );
                    GetNode<BaseDoor>("Building/Props/EndSequenceDoor").locked = true;
                    lockpickHint.Visible = true;
                    ventAttentionLight.Visible = true;

                    Global.plagueDoctor.canStartAttacking = true;
                    Global.plagueDoctor.StartAttacking();
                    break;
                default:
                    break;
            }
            dummyPlayerCamera.ClearCurrent(false);
            jumpscareCamera.ClearCurrent(false);
            Global.player.camera.Current = true;
        }

        public void Jumpscare()
        {
            jumpscareCamera.Current = true;
            animPlayer.Play("Jumpscare");
        }

        public void _OnEscapeDoorBlockageInteract()
        {
            lockpickHint.Visible = false;
        }
    }
}