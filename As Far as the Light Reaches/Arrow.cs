using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

//Juri Kiin
//Arrow.cs
//This class contains the info each arrow has.
namespace As_Far_as_the_Light_Reaches
{
    class Arrow
    {
        private Texture2D curTexture;
        private Rectangle rec = new Rectangle(750,250,512,512);

        public Arrow(Texture2D tex)
        {
            curTexture = tex;
        }

        public Texture2D CurTexture
        {
            get { return curTexture; }
            set { curTexture = value; }
        }

        public Rectangle Rec
        {
            get { return rec; }
            set { rec = value; }
        }


    }
}
