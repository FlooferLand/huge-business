using Godot;
using System;

namespace Project
{
    public class TheCurrentPropertyIsCringe : Camera
    {
        public override void _Ready()
        {
            ClearCurrent(false);
        }
    }
}