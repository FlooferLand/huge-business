using Godot;
using Project;

public class PrisonDoor : SimpleDoor
{
    [Export] public NodePath BossFootstepAudioPath;
    AudioStreamPlayer3D bossFootstepAudio;
    bool used = false;

    public override void _Ready() {
        base._Ready();
        bossFootstepAudio = GetNode<AudioStreamPlayer3D>(BossFootstepAudioPath);
    }

    public override void Interact(DoorSide side) {
        if (used) return;

        if (!locked) {
            bossFootstepAudio.Play();
            used = true;
        }
        base.Interact(side);
    }
}
