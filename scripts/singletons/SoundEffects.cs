using System;
using Godot;

namespace Project
{
    public static class SoundEffects
    {
        public static AudioStream[] footsteps =
        {
            GD.Load<AudioStream>("res://audio/footsteps/footstep_0.wav"),
            GD.Load<AudioStream>("res://audio/footsteps/footstep_1.wav"),
            GD.Load<AudioStream>("res://audio/footsteps/footstep_2.wav"),
            GD.Load<AudioStream>("res://audio/footsteps/footstep_3.wav")
        };

        public static AudioStream[] playerVoiceLines =
        {
            GD.Load<AudioStream>("res://audio/voicelines/other/player/facility_intro.wav")
        };
    }
}
