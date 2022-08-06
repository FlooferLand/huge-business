using Godot;
using System;
using System.Collections.Generic;

namespace Project
{
    public class DAuthor
    {
        public int Id;
        public string Text;

        public DAuthor(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }

    public static class DialogueAuthors
    {
        public static DAuthor Player  = new DAuthor(0, "You");
        public static DAuthor Villain = new DAuthor(1, "Boss");

        public static DAuthor GetById(int id)
        {
            switch (id)
            {
                case 0:
                    return Player;
                case 1:
                    return Villain;

                default:
                    throw new Exception($"No character id of \"{id}\"!");
            }
        }
    }

    public class DSequenceEntry
    {
        public string Text;
        public DAuthor Author;

        public DSequenceEntry(string text, DAuthor author)
        {
            Text = text;
            Author = author; 
        }
    }

    public class DialogueHandler : Panel
    {
        // Nodes
        public Label author;
        public Label content;

        // Variables
        public List<DSequenceEntry> sequences = new List<DSequenceEntry>();
        public int currentSequenceId = 0;
        public DSequenceEntry currentSequence;

        public override void _Ready()
        {
            author = GetNode<Label>("Author");
            content = GetNode<Label>("Content");
            // characterAddTimer = GetNode<Timer>("CharacterAddTimer");
            // characterAddTimer.Connect("timeout", this, nameof(_AddCharacter));
            Visible = false;
            Global.dialogueHandler = this;
        }

        public override void _Process(float delta)
        {
            if (Visible && Input.IsActionJustPressed("skip"))
            {
                NextSequence();
            }
        }

        public void WriteSequence(params DSequenceEntry[] entries)
        {
            foreach (DSequenceEntry entry in entries)
            {
                sequences.Add(entry);
            }
            Visible = true;
            currentSequenceId = 0;
            currentSequence = sequences[0];
            content.Text = currentSequence.Text;
            author.Text = currentSequence.Author.Text;
            Global.player.canMove = false;
        }

        public void NextSequence()
        {
            // If there is a next sequence
            if (currentSequenceId + 1 < sequences.Count)
            {
                currentSequenceId += 1;

                DSequenceEntry current = sequences[currentSequenceId];
                content.Text = current.Text;
                author.Text = current.Author.Text;
            }
            else
            {
                sequences.Clear();
                currentSequenceId = 0;
                Visible = false;
                Global.player.canMove = true;
            }
        }
    }
}