using Godot;
using System;

namespace Project
{
    public class SimpleDoor : BaseDoor
    {
        // Nodes
        private AudioStreamPlayer sound;

        public override void _Ready()
        {
            sound = GetNode<AudioStreamPlayer>("Sound");
            Global.blackoutHandler.Connect("end", this, nameof(_OnBlackoutEnd));
        }

        public override void Interact(DoorSide side)
        {
            if (locked)
            {
                sound.Play();
                return;
            }

            // Triggering blackout
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

            currentSide = null;
            Global.player.canMove = true;

            if (levelTransition == null)
                Global.player.Translation = (GlobalTransform.origin + side) - (Vector3.Up * 0.95f);
            else
                Global.gameMaster.ChangeMap(levelTransition);
        }
    }
}