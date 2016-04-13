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
        private Rectangle rec = new Rectangle(750, 300, 200, 200);
        private Keys keyValue;

        public Keys KeyValue
        {
            get{ return keyValue; }
            set{ keyValue = value; }
        }

        public Arrow(Texture2D tex, Keys key)
        {
            curTexture = tex;
            keyValue = key;
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
