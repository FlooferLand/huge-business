using Godot;

namespace Project
{
    public class CutsceneSkipper : Label
    {
        public override void _Ready()
        {
            Visible = false;
            Global.cutsceneSkipper = this;
        }

        public void Start(AnimationPlayer player)
        {
            player.PlaybackSpeed = 24;
            Visible = true;
        }

        public void Stop(AnimationPlayer player)
        {
            player.PlaybackSpeed = 1;
            Visible = false;
        }
    }
}