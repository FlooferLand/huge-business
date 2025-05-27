using Godot;
using System;

public class ChaseSection : Spatial
{
    public override void _Ready() {
        Visible = false;
    }

    public override void _Process(float delta) {
        if (!Visible) return;

        var matrix = Transform;
        matrix.origin = new Vector3(
            matrix.origin.x + (Mathf.Sin(Time.GetTicksMsec() / 10f) * 0.1f),
            matrix.origin.y,
            matrix.origin.z
        );
        Transform = matrix;
    }
}
