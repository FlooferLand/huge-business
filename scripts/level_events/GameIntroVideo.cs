using Godot;
using System;

namespace Project
{
    public class GameIntroVideo : Control
    {
        // Nodes
        public VideoPlayer video;
        public Timer startTimer;

        public override void _Ready()
        {
            video = GetNode<VideoPlayer>("VideoPlayer");
            video.Connect("finished", this, nameof(_OnVideoFinished));
            Input.MouseMode = Input.MouseModeEnum.Captured;

            startTimer = GetNode<Timer>("StartTimer");
            startTimer.Connect("timeout", this, nameof(_StartVideo));
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("skip"))
                _OnVideoFinished();
        }

        public void _StartVideo()
        {
            video.Play();
        }

        public void _OnVideoFinished()
        {
            GetTree().ChangeScene("res://Game Master.tscn");
        }
    }
}