using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;


//Juri Kiin
//Enemy.cs
//This will represent the enemies in the game. They will have their own health, and damage.
namespace As_Far_as_the_Light_Reaches
{
    class Enemy
    {
        //Attributes
        private string name;
        private int currhealth;
        private int health;
        private int damage;
        private int numArrow;
        private int boxwidth;
        private Rectangle pos;

        Game1 game = new Game1();

        public Enemy(int h, int d, int na, string nm)
        {
            name = nm;
            currhealth = h;
            health = h;
            damage = d;
            numArrow = na;
        }

        //Properties
        public Rectangle Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public int NumArrow
        {
            get { return numArrow; }
        }



        public void PlayerAttack (int dmg) //pass in player's damage, enemy defense if needed. This will be run when the bar is stopped with spacebar.
        {
            Vector2 bar = game.Barpos;
            int mult = 0;
            if (bar.X > 180 || bar.X < 120) //sample values assume the bar is 100 units to the right and 100 units long, and that the window is 300 units long. Obviously all wrong, but this is just example stuff right now.
            {
                mult = 0;
            }
            else if (bar.X > 165 || bar.X < 135)
            {
                mult = 1;
            }
            else if (bar.X > 155||bar.X <125)
            {
                mult = 2;
            }
            else if (bar.X > 125||bar.X<115)
            {
                mult = 3;
            }
            int hit = dmg * mult;
            currhealth -= hit;
        }

        public void PlayerDefend(int dmg)
        { }
    }
}
