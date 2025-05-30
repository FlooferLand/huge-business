using Godot;

namespace Project
{
    public class MainMenuButtons : VBoxContainer
    {
        // Nodes
        Button fullscreenToggle;
        Button startGameButton;
        Button quitGameButton;

        public override void _Ready()
        {
            // Getting buttons
            fullscreenToggle = GetNode<Button>("../..//FullscreenToggle");
            startGameButton = GetNode<Button>("Start game");
            quitGameButton = GetNode<Button>("Quit game");

            // Handling buttons
            fullscreenToggle.Connect("pressed", this, nameof(ToggleFullscreen));
            startGameButton.Connect("pressed", this, nameof(StartGame));
            quitGameButton.Connect("pressed", this, nameof(QuitGame));

            // Quit game button
            if (OS.HasFeature("web")) {
                quitGameButton.QueueFree();
            }
        }

        public void ToggleFullscreen()
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }

        public void StartGame()
        {
            GetTree().ChangeScene("res://Game Intro Video.tscn");
        }

        public void QuitGame()
        {
            GetTree().Quit();
        }
    }
}