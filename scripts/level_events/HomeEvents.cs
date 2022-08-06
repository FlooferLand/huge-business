using Godot;
using System;
using System.Collections.Generic;

namespace Project
{
    public class HomeEvents : Spatial
    {
        public override void _Ready()
        {
            Global.dialogueHandler.WriteSequence(
                new DSequenceEntry(
                    "* yawns *",
                    DialogueAuthors.Player
                ),
                new DSequenceEntry(
                    "What a good mornin, let's see what's on my phone.",
                    DialogueAuthors.Player
                ),
                new DSequenceEntry(
                    "Oh, dang it. Those damn NFTs aren't selling well",
                    DialogueAuthors.Player
                ),
                new DSequenceEntry(
                    "It must be the fault of my employees, not the ever-changing market and evergrowing hate for NFTs!",
                    DialogueAuthors.Player
                ),
                new DSequenceEntry(
                    "I'm gonna fire all 'dem bastards!",
                    DialogueAuthors.Player
                )
            );
        }
    }
}