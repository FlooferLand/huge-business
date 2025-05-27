using Godot;

namespace Project
{
    public class WorkplaceCutsceneEvents : Spatial
    {
        // Nodes
        public AnimationPlayer animPlayer;
        public AnimationPlayer hordeBobPlayer;
        public Spatial coreProps;
        public Spatial chaseProps;
        public Spatial chaseLights;
        public AudioStreamPlayer chaseMusic;
        public AudioStreamPlayer3D employeeHordeFootsteps;
        public Camera jumpscareCamera;
        public Camera cutsceneCamera;

        public override void _Ready()
        {
            hordeBobPlayer = GetNode<AnimationPlayer>("Chase/EmployeeHorde/BobPlayer");
            employeeHordeFootsteps = GetNode<AudioStreamPlayer3D>("Chase/EmployeeHorde/Footsteps");
            animPlayer = GetNode<AnimationPlayer>("AnimPlayer");
            animPlayer.Connect("animation_finished", this, nameof(_AnimFinished));
            jumpscareCamera = GetNode<Camera>("Chase/EmployeeHorde/JumpscareCamera");
            cutsceneCamera = GetNode<Camera>("ChaseIntro/CutscenePlayer/Camera");

            // Getting stuff for _AnimFinished
            coreProps  = GetParent().GetNode<Spatial>("CoreSection/Props");
            chaseProps  = GetParent().GetNode<Spatial>("ChaseSection/Props");
            chaseLights = GetParent().GetNode<Spatial>("ChaseSection/Lights");
            chaseMusic = GetNode<AudioStreamPlayer>("ChaseMusic");
        }

        // Setting up stuff when cutscenes are finished
        public void _AnimFinished(string animName)
        {
            if (animName == "RESET")
                return;

            switch (animName)
            {
                case "ChaseIntro":
                    // Stopping the user from going the wrong way
                    StaticBody barricade = chaseProps.GetNode<StaticBody>("ExitBarricade");
                    barricade.Translate(Vector3.Up * 5);

                    // Unlocking the door
                    BaseDoor door = GetParent().GetNode<BaseDoor>("TransitionSection/Chase P1 Entrance");
                    door.locked = false;

                    // Hiding the "Hide" guide
                    Sprite3D hideHelper = coreProps.GetNode<Sprite3D>("Hide Helper");
                    hideHelper.Visible = false;

                    // Helping guide the user by showing the door
                    Light light = chaseLights.GetNode<Light>("Chase P2 Entrance Light");
                    light.Visible = true;

                    // BAM BAM BAM BAM BUM BUM BRAPAPAPAPA
                    chaseMusic.Play();

                    // Playing the chase animation
                    animPlayer.Play("Chase");
                    hordeBobPlayer.PlaybackSpeed = 1.5f;
                    hordeBobPlayer.Play("Bobbing");
                    employeeHordeFootsteps.Play();

                    // Resetting cameras because Godot
                    jumpscareCamera.ClearCurrent(false);
                    cutsceneCamera.ClearCurrent(false);
                    Global.player.camera.Current = true;
                    break;
            }
        }

        public void ShakeScreen()
        {
            Global.player.ShakePunch(1.5f);
        }
    }
}