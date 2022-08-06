using Godot;

namespace Project
{
    public class MainMenu : Control
    {
        // Nodes
        private RichTextLabel credit;

        public override void _Ready()
        {
            // Getting nodes
            credit = GetNode<RichTextLabel>("Credit");

            // Doing stuff with nodes
            credit.Connect("meta_clicked", this, nameof(_OnLinkClicked));

            // Min res so UI doesn't scale weirdly
            OS.MinWindowSize = new Vector2(1200, 650);

            // For when the game resets
            Input.SetMouseMode(Input.MouseMode.Visible);

            // Maximizing window because yes
            OS.WindowMaximized = true;
        }

        public void _OnLinkClicked(string meta)
        {
            OS.ShellOpen(meta);
        }
    }
}