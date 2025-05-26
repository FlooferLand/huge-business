using Godot;

namespace Project
{
    public class JumpscareOnCollision : Area
    {
        // Nodes
        AnimationPlayer animPlayer;

        public override void _Ready()
        {
            animPlayer = GetNode<AnimationPlayer>("AnimPlayer");
            Connect("body_entered", this, nameof(_BodyEntered));
            animPlayer.Connect("animation_finished", this, nameof(_OnAnimFinished));
        }

        public void _BodyEntered(Node body)
        {
            if (!(body is Player))
                return;
            animPlayer.Play("Jumpscare");
        }

        public void _OnAnimFinished(string animName)
        {
            if (animName == "RESET")
                return;

            if (animName != "Jumpscare")
                return;

            GetTree().ReloadCurrentScene();
        }
    }
}