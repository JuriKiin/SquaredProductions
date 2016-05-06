using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace As_Far_as_the_Light_Reaches
{
    class DialogueManager
    {
        List<string> lines = new List<string>();
        string got;

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

        public bool WriteLog(int lindex, int numlines)
        {
            return false;
        }
    }
}
