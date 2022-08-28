using Godot;
using System;

namespace Project
{
    public class UserInterface : Control
    {
        public Control[] crosshairs;

        public override void _Ready()
        {
            // Fetching crosshairs
            crosshairs = new Control[] {
                GetNode("Crosshairs").GetChild<Control>(0), // Active
                GetNode("Crosshairs").GetChild<Control>(1)  // Grab
            };

            // Getting some UI nodes..
            Global.interactionProgress = GetNode<InteractionProgress>("InteractionProgress");
        }
    }
}