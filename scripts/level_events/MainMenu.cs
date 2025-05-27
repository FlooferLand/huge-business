using Godot;

namespace Project
{
	public class MainMenu : Control
	{
		// Nodes
		RichTextLabel credit;
		AnimationPlayer backgroundMovement;

		public override void _Ready()
		{
			// Getting nodes
			credit = GetNode<RichTextLabel>("Credit");
			backgroundMovement = GetNode<AnimationPlayer>("BackgroundMovement");

			// Doing stuff with nodes
			credit.Connect("meta_clicked", this, nameof(_OnLinkClicked));

			// For when the game resets
			Input.MouseMode = Input.MouseModeEnum.Visible;

			// Animation player go brr
			backgroundMovement.Play("BackgroundMovement");
		}

		public void _OnLinkClicked(string meta)
		{
			OS.ShellOpen(meta);
		}
	}
}
