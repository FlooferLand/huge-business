using Godot;

namespace Project
{
    // Assumes the node has an AnimationPlayer parent
    public class CutsceneHandler : Area
    {
        // Nodes
        public AnimationPlayer parent;
        public Spatial cutsceneRoot;

        // Export Variables
        [Export] public string animationName = "EMPTY";

        public override void _Ready()
        {
            parent = GetParent<AnimationPlayer>();
            if (parent.GetParent().HasNode(animationName))
            {
                cutsceneRoot = parent.GetParent().GetNode<Spatial>(animationName);
            }
            Connect("body_entered", this, nameof(_BodyEntered));
            parent.Connect("animation_finished", this, nameof(_AnimFinished));
        }

        public void _BodyEntered(Node body)
        {
            if (body is Player)
            {
                parent.Play(animationName);
                Global.player.canMove = false;
                Global.player.Visible = false;
                return;
            }
        }

        public override void _Process(float delta)
        {
            if (parent.IsPlaying() && Input.IsActionJustPressed("skip"))
            {
                Global.cutsceneSkipper.Start(parent);
            }
        }

        public void _AnimFinished(string animName)
        {
            Global.cutsceneSkipper.Stop(parent);
            if (animName == "RESET")
                return;

            if (cutsceneRoot != null)
            {
                Spatial playerPos = cutsceneRoot.GetNode<Spatial>("CutsceneEndPlayerPos");
                if (playerPos != null)
                {
                    Global.player.Translation = playerPos.Translation;
                    Global.player.Rotation = playerPos.Rotation;
                }
                else
                {
                    GD.Print($"[Anim] No player pos for cutscene \"{animName}.\"!");
                }
                cutsceneRoot.QueueFree();
            }
            Global.player.canMove = true;
            Global.player.Visible = true;
            Global.player.camera.Current = true;
            QueueFree();
        }
    }
}