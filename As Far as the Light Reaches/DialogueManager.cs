using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace As_Far_as_the_Light_Reaches
{
    class DialogueManager
    {
        //attributes
        private List<string> lines = new List<string>();
        private string got;
        private bool speaking = false;
        private int current;

        //keyboard stuff needed for singlekeypress()
        KeyboardState curKB;
        KeyboardState prevKB;

        public bool Speaking //property in case Game1 needs to know dialogue is being played.
        {
            get { return speaking; }
        }

        public DialogueManager()
        {
            //Load in dialogue lines
            StreamReader Read = new StreamReader("Dialogue.txt"); //pull up text file.
            while ((got = Read.ReadLine()) != "KILLREADER") //until the document ends...
            {
                lines.Add(got); //add the strings into the list
            }
            Read.Close();
        }

        //SingleKeyPress for progressing through dialog.
        public bool SingleKeyPress(Keys key)
        {
            prevKB = curKB;
            curKB = Keyboard.GetState();
            if (curKB.IsKeyDown(key) && prevKB.IsKeyUp(key))
            { return true; }
            else { return false; }
        }
        

        //five possible triggers for dialogue:
        //LEVEL START: Write method should be placed IMMEDIATELY AFTER LevelGen(). Set lindex based on current level.**
        //LEVEL END: Write method should be placed IMMEDIATELY BEFORE LevelGen(). Set lindex based on current level.
        //BEFORE COMBAT: Write method should be placed IMMEDIATELY BEFORE switching to combat game state. Set lindex based on current enemy.
        //AFTER COMBAT: Write method should be placed IMMEDIATELY AFTER switching to walk game state from combat. Set lindex based on current enemy.**
        //ON RECTANGLE COLLISION: One dialogue section is supposed to trigger about halfway down the QM, but this would be another rectangle packed into the already silly Move(). I'll probably rewrite it to be at level start.

        //Level End and Before Combat are easy. The second the character resumes, they'll be transferred to a different state/level and the adventure continues.

        //**Issues**
        //Level Start needs to be called as the game begins, but LevelGen() for the first level is called before Draw(). You'd be caught on a black screen with only dialogue for a bit. Not a disaster, but very unpleasant.
        //After Combat, similarly, can only trigger after the map has been redrawn, not just immediately 
        //^These two might be solved by a method similar to LevelGen(), where we use a bool to run it on only a single Update() iteration, thus letting us run it whenever is needed. Kinda hamfisted though.

        //Write: Targets a spot in the list that is the start of a dialogue section, freezes the player, displays 3 lines, waits for a spacebar press, then continues giving messages or breaks.
        public void WriteDialogue(int lindex)//Oh, because of the wait, one method should work for all the triggers! Whatever happens after the dialogue can happen actually after the method. Yay for only one method.
        {
            /*
            speaking = true;
            ability to move = false; //Can stay inside this function, or we can just disallow movement while speaking.

            //Issue with drawing: The way that this functions, . The solution: while Speaking is true, draw writes 3 strings determined by the manager, likely won't work.
            //Would it be healthy to draw them through this function? I know there's a way to do it, but I think drawing outside of Draw() can cause issues. It works perfectly if it doesn't create issues.
            draw lines[lindex];       
            draw lines[lindex+1];     
            draw lines[lindex+2];     
            //draw a spacebar icon next to the dialogue box? Keeps it nice and clear, and it shouldn't take long at all.
            
            while(!SingleKeyPress(Keys.space)) //until we hit spacebar to progress...
            {
                //Wait? Somehow...? My first idea was just an empty while you had to escape from, but that would run without getting to the very Draw() that should show us the strings.
                //I don't know what in the world will keep us from doing things such as switching state or running LevelGen(), but still allow Draw() to run. 
                //If the drawing goes straight through this method, this isn't an issue. The empty while is fine. If not that, we could try adding different conditionals to those statements.
            }
            if(lines[lindex+3] = "-") //If the division is a dash, there's still more dialogue to go in this section. Need to make sure we don't have any spare spaces on lines with divisions.
            {
                WriteLvlStart(lindex+4); //Recurses to continue showing dialogue.
            }
            else if(lines[lindex+3] = "~") //If the division is a tilde, we've reached the end of this dialogue section. Need to make sure we don't have any spare spaces on lines with divisions.
            {
                speaking = false;
                ability to move = true; //if this isn't already determined by speaking. (With Before Combat this might immediately turned back off, but still.)
            }
            */
        }
    }
}
