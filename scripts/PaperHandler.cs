using Godot;
using System;

namespace Project
{
    public class PaperHandler : TextureRect
    {
        // Nodes
        private Label label;

        public override void _Ready()
        {
            label = GetNode<Label>("Text");
            Visible = false;
            Global.paperHandler = this;
        }

        public override void _Process(float delta)
        {
            if (Visible && Input.IsActionJustPressed("skip"))
            {
                Global.player.canMove = true;
                Visible = false;
            }
        }

        public void Write(string text)
        {
            label.Text = text;
            Global.player.canMove = false;
            Visible = true;

            // Randomization so it looks poggers
            FlipH = (Global.rng.RandiRange(0, 2) == 1);
            FlipV = (Global.rng.RandiRange(0, 2) == 1);
        }
    }
}