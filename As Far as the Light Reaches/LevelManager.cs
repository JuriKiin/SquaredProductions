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
//Level Manager
//This class is in charge of managing what level the player is on, and switches the 


namespace As_Far_as_the_Light_Reaches
{
    class LevelManager
    {      
        Texture2D curLevelTexture;

        // 2737 2965 dimensions for each underground piece 

        int curLevel = 0; // this int is the level we are on change this int to change the level. 

        public int CurLevel
        {
            get { return curLevel; }
            set { curLevel = value; }
        }

        public Texture2D CurLevelTexture
        {
            get { return curLevelTexture; }
            set { curLevelTexture = value; }

        }

        public LevelManager(ContentManager cont)
        {
            // int count = 1;


            //   levelBackgrounds.Add(cont.Load<Texture2D>(path + count.ToString()));
            //  count++;

        }


        public void LoadNextLevel()  //This Loads the next level
        {
            //  if(curLevel < levelBackgrounds.Count) curLevel++;
            //  curLevelTexture = levelBackgrounds[curLevel];
        }



    }
}
