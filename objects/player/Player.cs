using Godot;
using System.Collections.Generic;

namespace Project
{
    public class Player : KinematicBody
    {
        // Nodes
        public  Spatial head = null;
        public  Camera camera = null;
        Spatial items = null;
        public  Spatial flashlight = null;
        AudioStreamPlayer footstepPlayer = null;
        AudioStreamPlayer voicePlayer = null;
        Timer footstepTimer = null;
        RayCast interactRay = null;
        Subtitle subtitle = null;

        const bool DEV_DEBUG_MODE = false;

        // Constants
        const float SPEED      = 2.5f;
        const float TURN_SPEED = 0.15f;  // 1.8f
        const float GRAVITY    = 5f;
        public const float FOOTSTEP_WALK_TIME = 0.5f;
        public const float FOOTSTEP_RUN_TIME  = 0.3f;
        public readonly Vector3 HEAD_POS_OFFSET = new Vector3(0f, 1.6f, 0f);
        public readonly Vector3 ITEMS_POS_OFFSET = new Vector3(0f, 1.5f, 0f);
        public float screenShake = 0f;
        public bool shakePunching = false;
        public bool canMove = true;

        // Variables
        Vector3 direction = Vector3.Zero;
        Vector3 velocity = Vector3.Zero;
        float time = 0f;
        public float moving = 0f;
        float currSpeed = 0f;
        public bool isRunning = false;
        bool hasLanded = false;
        public List<KeyObject> keys = new List<KeyObject>();

        public override void _Ready()
        {
            // Getting nodes
            //camera = GetParent().GetNode<Camera>("Camera");
            head = GetNode<Spatial>("Head");
            camera = GetNode<Camera>("Head/Camera");
            items = GetNode<Spatial>("Items");
            flashlight = GetNode<Spatial>("Items/Flashlight");
            footstepPlayer = GetNode<AudioStreamPlayer>("Footsteps/StreamPlayer");
            voicePlayer = GetNode<AudioStreamPlayer>("Voice");
            footstepTimer = GetNode<Timer>("Footsteps/Timer");
            interactRay = head.GetNode<RayCast>("InteractRay");
            subtitle = GetNode<Subtitle>("Head/Camera/UI/Subtitle");

            // Locking cursor
            Input.MouseMode = Input.MouseModeEnum.Captured;

            // Adding the player and the UI to the Global container
            Global.player = this;
            Global.ui = GetNode<UserInterface>("Head/Camera/UI");

            // Subtitle stuff!!!
            voicePlayer.Connect("finished", subtitle, nameof(subtitle.End));

            // Flashlight (and UI) go no
            flashlight.Visible = false;
            Global.interactionIndicator.Visible = false;

            // DEBUG
            if (isDebugging()) {
                GetViewport().DebugDraw = Viewport.DebugDrawEnum.Unshaded;
            }
        }

        public override void _Process(float delta)
        {
            // Locking/Unlocking cursor
            if (Input.IsActionJustPressed("ui_cancel"))
                Input.MouseMode = (2 - Input.MouseMode);

            // Fullscreen toggle
            if (Input.IsActionJustPressed("fullscreen_toggle"))
                OS.WindowFullscreen = !OS.WindowFullscreen;

            // Handling shake punch
            if (shakePunching && screenShake > 0.1f)
            {
                screenShake = Mathf.Lerp(screenShake, 0f, 0.1f);
            }
            else if (shakePunching && screenShake <= 0.1f)
            {
                shakePunching = false;
                screenShake = 0f;
            }

            // Player movement disable
            if (!canMove)
                return;

            #region --- Movable Input
            // Movement
            direction = Vector3.Zero;
            if (Input.IsActionPressed("move_forward"))
                direction -= Transform.basis.z;
            else if (Input.IsActionPressed("move_backward"))
                direction += Transform.basis.z;
            if (Input.IsActionPressed("move_left"))
                direction -= Transform.basis.x;
            else if (Input.IsActionPressed("move_right"))
                direction += Transform.basis.x;
            direction.y = 0f;

            // Sprinting
            if (Input.IsActionPressed("sprint"))
                isRunning = true;
            else
                isRunning = false;

            // Flashlight toggle
            if (Input.IsActionJustPressed("flashlight_toggle"))
                flashlight.Visible = !flashlight.Visible;
            #endregion

            // Animation and stuff
            const float headBobFrequency = 12f;
            float headBobAmount = (isRunning ? 0.03f : 0.01f);

            // If moving
            if (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) > 0 && moving <= 1f && !IsOnWall())
                moving = Mathf.Lerp(moving, 1f, 0.25f);
            else
                moving = Mathf.Lerp(moving, 0f, 0.1f);

            // Head tilt
            // TODO: Should be using delta time
            Vector3 headRot = head.Rotation;
            Vector3 headPos = head.Translation;
            headRot.z = (Mathf.Cos(time * headBobFrequency) * headBobAmount) * moving;
            headPos.y = (Mathf.Sin(time * headBobFrequency) * (headBobAmount * 5)) * moving;
            headPos.z = (Mathf.Cos(time * headBobFrequency) * (headBobAmount * 5)) * moving;

            // Item position/rotation based on the player's head
            items.Translation = headPos + ITEMS_POS_OFFSET;
            items.Rotation = new Vector3(
                Mathf.LerpAngle(items.Rotation.x, headRot.x, 0.2f),
                Mathf.LerpAngle(items.Rotation.y, headRot.y, 0.2f),
                Mathf.LerpAngle(items.Rotation.z, headRot.z, 0.2f)
            );

            // // Camera position/rotation smoothing
            // camera.Translation = Translation + headPos;
            // camera.Rotation = new Vector3(
            //     Mathf.LerpAngle(camera.Rotation.x, Rotation.x + headRot.x, 0.1f),
            //     Mathf.LerpAngle(camera.Rotation.y, Rotation.y + headRot.y, 0.25f),
            //     Mathf.LerpAngle(camera.Rotation.z, Rotation.z + headRot.z, 0.25f)
            // );

            // Footstep thumping
            if (moving > 0.5f)
            {
                if (footstepTimer.IsStopped())
                {
                    footstepPlayer.Stream = SoundEffects.footsteps[
                        Global.rng.RandiRange(0, SoundEffects.footsteps.Length - 1)
                    ];
                    footstepPlayer.Play();
                    footstepTimer.WaitTime = (isRunning ? FOOTSTEP_RUN_TIME : FOOTSTEP_WALK_TIME);
                    footstepTimer.Start();
                }
            }

            // Shakiness
            if (screenShake > 0f && !isDebugging())
            {
                Vector3 rotAppend = new Vector3(
                    Global.rng.RandfRange(-screenShake, screenShake) * 2f,
                    Global.rng.RandfRange(-screenShake, screenShake) * 1.5f,
                    Global.rng.RandfRange(-screenShake, screenShake) * 1.5f
                );

                // Safety switch (if the rotation is above a certain number)
                if ((camera.RotationDegrees + rotAppend).Abs() > (Vector3.One * 10))
                {
                    camera.RotationDegrees = new Vector3(
                        Mathf.Lerp(camera.RotationDegrees.x, 0f, delta * 25f),
                        Mathf.Lerp(camera.RotationDegrees.y, 0f, delta * 25f),
                        Mathf.Lerp(camera.RotationDegrees.z, 0f, delta * 25f)
                    );
                }
                else
                {
                    camera.RotationDegrees += rotAppend;
                }
            }
            else
            {
                camera.RotationDegrees = new Vector3(
                    Mathf.Lerp(camera.RotationDegrees.x, 0f, delta * 5f),
                    Mathf.Lerp(camera.RotationDegrees.y, 0f, delta * 5f),
                    Mathf.Lerp(camera.RotationDegrees.z, 0f, delta * 5f)
                );
            }

            head.Rotation = headRot;
            head.Translation = headPos + HEAD_POS_OFFSET;

            // Increasing the time
            time += delta;

            // Resetting the time every once in a while so it doesn't overflow max num limit
            if (time >= headBobFrequency*2)
                time = 0f;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (!canMove)
            {
                footstepPlayer.Stop();
                footstepTimer.Stop();
                return;
            }

            if (IsOnFloor() && !hasLanded)
            {
                // The frame the player hit the floor
                // footstepPlayer.Play();
                hasLanded = true;
                moving = 3f;
            }
            else if (!IsOnFloor())
            {
                hasLanded = false;
                direction *= 0.25f;
                velocity.y = -GRAVITY;
            }

            // Interacting with stuff
            if (interactRay.IsColliding()) {
                if (!(interactRay.GetCollider() is Spatial obj))
                    return;

                bool canInteract = true;
                if (obj is IMightNotInteract mightNotInteract) {
                    canInteract = mightNotInteract.CanInteract();
                }
                canInteract = canInteract && obj.Visible;

                bool pressingInteract = Input.IsActionJustPressed("interact");
                bool isObjectInteractable = obj.IsInGroup("interactable") && canInteract;
                if (isObjectInteractable)
                {
                    DoorSide doorSide = null;
                    if (obj is DoorSide side) {
                        doorSide = side;
                        obj = side.GetParent<Spatial>();
                    }
                    if (obj is IBaseUnlockable unlockable)
                    {
                        // Locked text
                        if (unlockable.IsLocked())
                        {
                            Global.interactionIndicator.Text = "Locked";
                            Global.interactionIndicator.Visible = true;
                        } else {
                            Global.interactionIndicator.Visible = false;
                        }

                        if (pressingInteract) {
                            if (unlockable.IsLocked()) {
                                KeyObject correctKey = null;
                                foreach (var key in keys) {
                                    if (key.CanUseOn(obj)) {
                                        correctKey = key;
                                        break;
                                    }
                                }
                                correctKey?.TryUse(obj);
                            }
                            doorSide?.Interact();
                        }
                    }

                    if (pressingInteract) {
                        if (!(obj is BaseDoor) && obj is IBaseInteractable interactable)
                            interactable.Interact();
                    }
                } else {
                    Global.interactionIndicator.Visible = false;
                }

                // Graphical stuff
                Global.interactionProgress.Visible = (isObjectInteractable && obj is IBaseProgressable);
                Global.ui.crosshairs[0].Visible = isObjectInteractable;
                Global.ui.crosshairs[1].Visible = false;
            }
            else
            {
                // Graphical stuff
                Global.interactionProgress.Visible = false;
                Global.ui.crosshairs[0].Visible = false;
                Global.interactionIndicator.Visible = false;
            }

            // Making it so movement speed won't increase when moving diagonaly
            if (direction != Vector3.Zero)
                direction = direction.Normalized();

            if (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) > 0 && IsOnFloor())
                currSpeed = Mathf.Lerp(currSpeed, (isRunning ? SPEED * 2.5f : SPEED), 0.02f);
            else
                currSpeed = Mathf.Lerp(currSpeed, 0f, 0.02f);
            velocity.x = direction.x * currSpeed;
            velocity.z = direction.z * currSpeed;

            // Moving player
            // velocity = velocity.LinearInterpolate(direction * currSpeed, ACCEL * delta);
            MoveAndSlide(velocity, Vector3.Up, true, 4, Mathf.Deg2Rad(25));
        }
        
        public override void _Input(InputEvent @event)
        {
            if (!canMove)
                return;
            
            // Head movement
            if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                // Magic i found on the Godot forums
                RotateY(Mathf.Deg2Rad(-mouseMotion.Relative.x * TURN_SPEED));
                head.RotateX(Mathf.Deg2Rad(-mouseMotion.Relative.y * TURN_SPEED));
                Vector3 rotDeg = head.RotationDegrees;
                rotDeg.x = Mathf.Clamp(rotDeg.x, -50f, 50f);
                rotDeg.y = Mathf.Clamp(rotDeg.y, -50f, 50f);
                head.RotationDegrees = rotDeg;
            }
        }

        public bool isDebugging() {
            return (DEV_DEBUG_MODE && OS.HasFeature("editor"));
        }

        public void Speak(AudioStream clip, string text)
        {
            subtitle.SetCharacter(DialogueAuthors.Player.Id);
            subtitle.Write(text);
            voicePlayer.Stream = clip;
            voicePlayer.Play();
        }

        public void ShakePunch(float intensity)
        {
            screenShake = intensity;
            shakePunching = true;
        }
    }
}