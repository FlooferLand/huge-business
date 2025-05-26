using Godot;

namespace Project
{
    public class GameEnd : Spatial
    {
        // Nodes
        public VideoPlayer videoPlayer;
        public Timer videoPlayTimer;

        public override void _Ready()
        {
            Global.player.canMove = false;
            videoPlayer = GetNode<VideoPlayer>("UI/VideoPlayer");
            videoPlayTimer = GetNode<Timer>("VideoPlayTimer");

            videoPlayTimer.Connect("timeout", this, nameof(PlayVideo));
            videoPlayer.Connect("finished", this, nameof(_OnVideoEnd));
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
        }

        public void _OnVideoEnd()
        {
            GetTree().ChangeScene("res://MainMenu.tscn");
        }
    }
}