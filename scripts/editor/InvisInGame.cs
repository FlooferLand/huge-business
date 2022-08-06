using Godot;
using System;

namespace Project
{
    public class InvisInGame : Spatial
    {
        public override void _Ready()
        {
            Visible = false;
        }
    }
}