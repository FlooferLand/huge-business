using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Project
{
    public class AnimatedDoor : BaseDoor
    {
        // Nodes
        Spatial model;
        Sprite3D modelFront;
        Sprite3D modelBack;
        Sprite3D modelFrontInterior;
        Sprite3D modelBackInterior;
        AudioStreamPlayer sound;

        // Export variables
        [Export] public bool flipModel = false;
        [Export] public Array<NodePath> HiddenOnEnter = new Array<NodePath>();
        [Export] public Array<NodePath> ShownOnEnter = new Array<NodePath>();

        // Variables
        public bool animating = false;
        public List<Spatial> disableOnEnter = new List<Spatial>();
        public List<Spatial> enableOnEnter = new List<Spatial>();

        public override void _Ready()
        {
            base._Ready();
            model = GetNode<Spatial>("Model");
            modelFront = model.GetNode<Sprite3D>("Front");
            modelBack = model.GetNode<Sprite3D>("Back");
            modelFrontInterior = GetNode<Sprite3D>("Interior/Front");
            modelBackInterior = GetNode<Sprite3D>("Interior/Back");
            sound = GetNode<AudioStreamPlayer>("Sound");
            modelBackInterior.Visible = false;
            modelFrontInterior.Visible = false;
            Global.blackoutHandler.Connect("end", this, nameof(_OnBlackoutEnd));
            foreach (var nodePath in HiddenOnEnter) {
                disableOnEnter.Add(GetNode<Spatial>(nodePath));
            }
            foreach (var nodePath in ShownOnEnter) {
                enableOnEnter.Add(GetNode<Spatial>(nodePath));
            }
            
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
            animating = true;
            currentSide = side;
            model.Rotation = Vector3.Zero;
            sound.Play();

            // Hiding the other side of the door model
            // As well as showing a dark door interior
            switch (currentSide?.id)
            {
                case DoorSide.SIDES.Front:
                    modelBack.Visible = false;
                    modelFrontInterior.Visible = true;
                    break;
                case DoorSide.SIDES.Back:
                    modelFront.Visible = false;
                    modelBackInterior.Visible = true;
                    break;
            }

            // Triggering blackout
            if (!locked)
            {
                Global.player.canMove = false;
                Global.blackoutHandler.TriggerBlackout(GetInstanceId().ToString());
            }
        }

        public void _OnBlackoutEnd(string reason)
        {
            // Not this door!
            if (currentSide == null || reason != GetInstanceId().ToString())
                return;

            // Performance stuff
            foreach (var node in disableOnEnter) {
                node.Visible = false;
            }
            foreach (var node in enableOnEnter) {
                node.Visible = true;
            }

            // Animation end
            Vector3 side = GlobalTransform.basis.x;
            if (RotationDegrees.y != 0 && Math.Abs(RotationDegrees.y - 180) > 0.5)
            {
                if (Math.Abs(RotationDegrees.y - 90) < 0.5 && Math.Abs(RotationDegrees.y - (-90)) < 0.5)
                {
                    side = GlobalTransform.basis.x;
                }
            }
            side *= (currentSide.id == DoorSide.SIDES.Front ? -1 : 1);

            animating = false;
            model.RotationDegrees = Vector3.Zero;
            currentSide = null;
            Global.player.canMove = true;
            modelFront.Visible = true;
            modelBack.Visible = true;
            modelBackInterior.Visible = false;
            modelFrontInterior.Visible = false;

            if (levelTransition == null)
                Global.player.Translation = (outOffset.GlobalPosition + side);
            else
                Global.gameMaster.ChangeMap(levelTransition);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            int angle = animating ? (locked ? 10 : 25) : 0;
            if (currentSide != null)
                angle = (currentSide.id == DoorSide.SIDES.Front && !flipModel ? -angle : angle);

            // TODO: flipModel doesn't always flip the door animation
            if (!locked)
            {
                model.Rotation = new Vector3(
                    0f,
                    Mathf.LerpAngle(
                        model.Rotation.y,
                        Mathf.Deg2Rad(angle),
                        5f * delta
                    ),
                    0f
                );
            } else
            {
                model.Rotation = new Vector3(
                    0f,
                    Mathf.LerpAngle(
                        model.Rotation.y,
                        Mathf.Deg2Rad(Mathf.Sin(angle) * 2f),
                        12f * delta
                    ),
                    0f
                );
            }

            // Polish TODO: Reimplement locked door animation
            // End locked animation
            /*if (locked && Mathf.CeilToInt(Mathf.Abs(model.RotationDegrees.y)) >= (angle / 2))
            {
                animating = false;
                model.RotationDegrees = Vector3.Zero;
                currentSide = null;
            }*/
        }
    }
}