using Godot;
using System;

namespace Project
{
    public class PlagueDoctor : Spatial
    {
        // Nodes
        public AnimationPlayer animPlayer;

        // Variables
        public bool canStartAttacking = false;
        public int  timesAttacked = 0;

        public override void _Ready()
        {
            Global.plagueDoctor = this;
            animPlayer = GetParent().GetNode<AnimationPlayer>("AnimPlayer");
            animPlayer.Connect("animation_finished", this, nameof(_OnAnimFinished));
        }

        public void _OnAnimFinished(string animName)
        {
            if (animName == "RESET")
                return;

            if (animName == "Attack")
            {
                Global.facilityEvents.Jumpscare();
            }
        }

        public void StartAttacking()
        {
            if (!canStartAttacking)
                return;
            animPlayer.Play("Attack", 1f, (timesAttacked == 0 ? 0.7f : 0.85f));
            timesAttacked += 1;
            if (timesAttacked > 20)
            {
                OS.Alert("bruh, you be taking your sweet time?");
                timesAttacked = 0;
            }
        }

        public void BlockAttack()
        {
            animPlayer.Stop(true);
            StartAttacking();
        }
    }
}