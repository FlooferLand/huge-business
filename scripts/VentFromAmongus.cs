using Godot;

namespace Project
{
    public class VentFromAmongus : StaticBody, IBaseInteractable
    {
        // Nodes
        AnimationPlayer animPlayer;

        public override void _Ready()
        {
            animPlayer = GetNode<AnimationPlayer>("AnimPlayer");
            Global.vent = this;
        }

        public void Interact()
        {
            animPlayer.Play("CloseVent");
        }
    }
}