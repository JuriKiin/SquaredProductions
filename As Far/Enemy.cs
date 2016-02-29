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
namespace As_Far
{
    class Enemy
    {
        //Attributes
        private int health;
        private int damage;
        private int numArrow;

        public Enemy(int h, int d, int n)
        {
            health = h;
            damage = d;
            numArrow = n;
        }
    }
}
