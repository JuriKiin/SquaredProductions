using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace As_Far_as_the_Light_Reaches
{
    class Player
    {
        //MAYBE THINK ABOUT ADDING A CLASS ATTRIBUTE TO THE MAIN PLAYER CLASS INSTEAD OF AHAVING A SEPERATE CLASS FOR CLASS SYSTEM

        //Draw attributes
        private Texture2D playerTexture;    //Player Texture
        private Rectangle playerRec;        //Player Rectangle
        private int x;              //X position of Rec
        private int y;              //Y position of Rec
        private int width;
        private int height;

        //Character attributes
        private int maxHealth;
        private int curHealth;
        private int damage;
        private int armor;
        private int boost;

        //Properties
        public int X
        {
            get { return playerRec.X; }
            set { playerRec.X = value; }
        }
        public int Y
        {
            get { return playerRec.Y; }
            set { playerRec.Y = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int CurHealth
        {
            get { return curHealth; }
            set { curHealth = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public int Armor
        {
            get { return armor; }
            set { armor = value; }

        }

        public int Boost
        {
            get { return boost; }
            set { boost = value; }
        }
        public Rectangle PlayerRec
        {
            get { return playerRec; }
            set { playerRec = value; }

        }
        public Texture2D PlayerTexture
        {
            get { return playerTexture; }
            set { playerTexture = value; }
        }

        public int Width
        {
            get { return playerRec.Width; }
            set { playerRec.Width = value; }
        }

        public int Height
        {
            get { return playerRec.Height; }
            set { playerRec.Height = value; }
        }


        //Constructor
        public Player(int h, int mh, int d, int a, int b)
        {
            //set default stats
            curHealth = h;
            maxHealth = mh;
            damage = d;
            armor = a;
            boost = b;
        }


    }
}
