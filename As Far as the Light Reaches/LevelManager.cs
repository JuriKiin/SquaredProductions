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
        int curLevel;   //Current level the player is on.
        List<Texture2D> levelBackgrounds = new List<Texture2D>();   //List of background textures for each level
        Texture2D curLevelTexture;
        string path = "Maps\\";
        // 2737 2965 dimensions for each underground piece 
        int[,] underground;
        

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
            int count = 1;
            curLevel = -1;
  
            levelBackgrounds.Add(cont.Load<Texture2D>(path + count.ToString()));
            count++;
          
        }


        public void LoadNextLevel()  //This Loads the next level
        {
            if(curLevel < levelBackgrounds.Count) curLevel++;
            curLevelTexture = levelBackgrounds[curLevel];
        }

    


    }
}
