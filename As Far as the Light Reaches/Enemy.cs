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
        private int currHealth;
        private int health;
        private int damage;
        public int numArrow;
        private int armor;
        private int speed;
        private bool directional;
        private Rectangle pos;
        private Texture2D enemyTex;

        Game1 game = new Game1();

        public Enemy(int hp, int dam, int numArr, string nm, int arm, bool dir, int spd)
        {
            name = nm;
            currHealth = hp;
            health = hp;
            damage = dam;
            numArrow = numArr;
            armor = arm;
            directional = dir;
            speed = spd;
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
        public int Armor
        {
            get { return armor; }
        }
        public Texture2D EnemyTexture
        {
            get { return enemyTex; }
            set { enemyTex = value; }
        }
        public bool Directional
        {
            get { return directional; }
        }
        public int CurrHealth
        {
            get { return currHealth; }
            set { currHealth = value; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
    }
}