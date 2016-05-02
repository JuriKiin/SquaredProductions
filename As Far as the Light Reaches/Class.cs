using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Juri Kiin
//This will assign the player proper attributes based on the class they choose at the beginning.
namespace As_Far_as_the_Light_Reaches
{
    class Class
    {
        Player p = new Player(0,0,0,0,0);

        public void Tank()
        {
            p.MaxHealth = 26;
            p.Damage = 3;
            p.Armor = 6;
        }

        public void Assassin()
        {
            p.MaxHealth = 14;
            p.Damage = 5;
            p.Armor = 2;
        }

        public void Warrior()
        {
            p.MaxHealth = 18;
            p.Damage = 7;
            p.Armor = 4;
        }

        public void Barbarian()
        {
            p.MaxHealth = 20;
            p.Damage = 6;
            p.Armor = 0;
        }
    }
}
