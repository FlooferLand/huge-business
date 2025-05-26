using Godot;

namespace Project
{
    public class GameMaster : Spatial
    {
        // Nodes
        public Spatial loadedLevels;

        public override void _Ready()
        {
            Global.gameMaster = this;
            Global.player.camera.Current = true;

            // Getting the loaded levels spatial
            loadedLevels = GetNode<Spatial>("LoadedLevels");

            // Getting the default map
            Node map = loadedLevels.GetChild(0);

            // Fetching and using the player's spawn position
            Spatial spawn = (Spatial) map.GetNode("PlayerSpawn");
            if (spawn != null)
            {
                Global.player.Translation = spawn.Translation;
                Global.player.Rotation = spawn.Rotation;
            }
        }

        // Functions
        public void ChangeMap(PackedScene newScene)
        {
            // Deleting existing maps
            foreach (Node map in loadedLevels.GetChildren())
            {
                map.QueueFree();
            }

            // Adding new map
            Node scene = newScene.Instance();

            // Fetching and using the player's spawn position
            Spatial spawn = (Spatial) scene.GetNode("PlayerSpawn");
            if (spawn != null)
            {
                Global.player.Translation = spawn.Translation;
                Global.player.Rotation = spawn.Rotation;
            }

            loadedLevels.AddChild(scene);
            Global.player.camera.Current = true;
        }
    }
}