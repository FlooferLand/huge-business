using System;
using Godot;

namespace Project
{
    public class SimpleDoor : BaseDoor
    {
        // Nodes
        AudioStreamPlayer sound;

        public override void _Ready()
        {
            base._Ready();
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
            Global.blackoutHandler.TriggerBlackout(GetInstanceId().ToString());
            sound.Play();
        }

        public void _OnBlackoutEnd(string reason)
        {
            // Not this door!
            if (currentSide == null || reason != GetInstanceId().ToString())
                return;

            // Animation end
            Vector3 side = GlobalTransform.basis.x;
            if (RotationDegrees.y != 0 && Math.Abs(RotationDegrees.y - 180) > 1.0)
            {
                if (Math.Abs(RotationDegrees.y - 90) < 1.0 && Math.Abs(RotationDegrees.y - (-90)) < 1.0)
                {
                    side = GlobalTransform.basis.x;
                }
            }
            side *= (currentSide.id == DoorSide.SIDES.Front ? -1 : 1);

            currentSide = null;
            Global.player.canMove = true;

            if (levelTransition == null)
                Global.player.Translation = (outOffset.GlobalPosition + side);
            else
                Global.gameMaster.ChangeMap(levelTransition);
        }
    }
}