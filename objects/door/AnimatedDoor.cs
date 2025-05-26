using Godot;

namespace Project
{
    public class AnimatedDoor : BaseDoor
    {
        // Nodes
        Spatial model;
        AudioStreamPlayer sound;

        // Export variables
        [Export] public bool flipModel = false;

        // Variables
        public bool animating = false;

        public override void _Ready()
        {
            model = GetNode<Spatial>("Model");
            sound = GetNode<AudioStreamPlayer>("Sound");
            Global.blackoutHandler.Connect("end", this, nameof(_OnBlackoutEnd));
            
            // Flipping model
            if (flipModel)
            {
                foreach (Spatial child in model.GetChildren())
                {
                    child.Scale = new Vector3(
                        child.Scale.x,
                        child.Scale.y,
                        -child.Scale.z
                    );
                    child.Translation += Vector3.Back;
                }
                model.Translation = new Vector3(
                    0f, 0f, -model.Translation.z
                );
            }
        }

        public override void Interact(DoorSide side)
        {
            if (locked)
            {
                // Polish TODO: Reimplement locked door animation
                //animating = true;
                sound.Play();
                return;
            }

            // Triggering blackout
            animating = true;
            currentSide = side;
            Global.player.canMove = false;
            Global.blackoutHandler.TriggerBlackout(Name);
            sound.Play();
        }

        public void _OnBlackoutEnd(string reason)
        {
            // Return if currentSide returns null
            // idk why that happens XD
            if (currentSide == null || reason != Name)
                return;

            // Animation end
            Vector3 side = GlobalTransform.basis.x;
            if (RotationDegrees.y != 0 && RotationDegrees.y != 180)
            {
                if (RotationDegrees.y == 90 && RotationDegrees.y == -90)
                {
                    side = GlobalTransform.basis.x;
                }
            }
            side = side * (currentSide.id == DoorSide.SIDES.Front ? -1 : 1);

            animating = false;
            model.RotationDegrees = Vector3.Zero;
            currentSide = null;
            Global.player.canMove = true;

            if (levelTransition == null)
                Global.player.Translation = (GlobalTransform.origin + side) - (Vector3.Up * 0.95f);
            else
                Global.gameMaster.ChangeMap(levelTransition);
        }

        public override void _Process(float delta)
        {
            if (!animating)
                return;
            int angle = (locked ? 8 : 30);
            angle = (currentSide.id == DoorSide.SIDES.Front && !flipModel ? angle : -angle);

            // TODO: flipModel doesn't always flip the door animation
            model.Rotation = new Vector3(
                0f,
                Mathf.LerpAngle(
                    model.Rotation.y,
                    Mathf.Deg2Rad(angle),
                    5f * delta
                ),
                0f
            );

            // Polish TODO: Reimplement locked door animation
            // // End locked animation
            // if (locked && Mathf.CeilToInt(Mathf.Abs(model.RotationDegrees.y)) >= (angle / 2))
            // {
            //     animating = false;
            //     model.RotationDegrees = Vector3.Zero;
            //     currentSide = null;
            // }
        }
    }
}