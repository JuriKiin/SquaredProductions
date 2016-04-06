using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


//Juri Kiin
//ArrowSpawn.cs
//This class loads each arrow key sprite into the game, and then contains a method that will determine which keys to spawn, and how many.
//The draw method in Game1.cs will handle the actual drawing of the keys.
namespace As_Far_as_the_Light_Reaches
{
    class ArrowSpawn
    {

        List<Texture2D> loadArrows = new List<Texture2D>();
        string path = "UI\\ArrowKey\\";     //gives us a path to the folder where the arrows sprites are kept.

        public ArrowSpawn() {}  //Constructor. Empty for now.


        //This method will get the arrow keys and then add them into the arrow key list.

        public void LoadArrow(ContentManager cont)
        {
            int count = 1;
            while (true)
            {
                try
                {
                    loadArrows.Add(cont.Load<Texture2D>(path + count.ToString()));
                    count++;
                }
                catch { break; }
            }

        }


        public List<Arrow> GenerateArrows(int numberOfArrows, bool dir)       //This method generates the curArrow list of arrows that should be spawned per given enemy.
        {
            Random rnd = new Random();  //Create new Random object

            List<Arrow> cArrow = new List<Arrow>();     //List that generates a custom set of arrow keys every time the method is called.

            for(int i = 0; i<=numberOfArrows;i++)   //Loop through this list until no more arrows should be spawned.
            {
                int j = rnd.Next(0, loadArrows.Count - 1);
                Arrow arr = new Arrow(loadArrows[j]);
                cArrow.Add(arr);   //Adds the random key to the list.
            }

            return cArrow;
        }



    }
}
