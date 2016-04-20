using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace As_Far_as_the_Light_Reaches
{
    class Wall
    {
        private Rectangle pos;
        public enum direction { left, right, up, down };
        private direction blocks;

        public Wall(int x, int y, int w, int h, direction block)
        {
            pos = new Rectangle(x, y, w, h);
            blocks = block;
        }
        public Rectangle Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public direction Blocks
        {
            get { return blocks; }
        }
    }
}
