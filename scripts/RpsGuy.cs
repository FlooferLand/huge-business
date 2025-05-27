using Godot;
using System;
using Project;

public class RpsGuy : Spatial
{
    AnimationPlayer animPlayer;
    int watchCounter = 0;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimPlayer");
        animPlayer.Connect("animation_finished", this, nameof(_AnimFinished));

        var startTrigger = GetNode<Area>("StartTrigger");
        startTrigger.Connect("body_entered", this, nameof(_BodyEntered));
        startTrigger.Connect("body_exited", this, nameof(_BodyExited));
    }

    void _BodyEntered(Node body) {
        if (!(body is Player player)) return;

        if (watchCounter == 0) {
            animPlayer.Play("Silly");
        }
    }

    void _BodyExited(Node body) {
        if (!(body is Player player)) return;

        if (watchCounter == 1) {
            animPlayer.Play("Jumpscare");
        }
    }

    void _AnimFinished(string animName) {
        if (animName == "Silly" || animName == "Jumpscare") {
            watchCounter += 1;
        }
    }
}
