using Godot;
using System;

namespace Project
{
    public class Subtitle : Label
    {
        public DAuthor currentCharacter;

        public void SetCharacter(int id)
        {
            currentCharacter = DialogueAuthors.GetById(id);
        }

        public void Write(string text)
        {
            Text = currentCharacter.Text + ":  " + text;
        }

        public void End()
        {
            Text = "";
        }
    }
}