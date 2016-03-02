using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace As_Far
{
    public class Button 
    {
        //Attributes for button properties
        private int buttonX; 
        private int buttonY;
        private string name;
        private Color color;
        private Texture2D texture;
        MouseState m = Mouse.GetState();
        

        // public property for button's X coord.
        public int ButtonX
        {
            get
            {
                return buttonX;
            }
        }

        // public property for button's Y coord.
        public int ButtonY
        {
            get
            {
                return buttonY;
            }
        }

        //public property for button name
        public string Name
        {
            get
            {
                return name;
            }
        }

        //public property for button color
        public Color Color
        {
            get
            {
                return color;
            }
        }

        //public property for button's texture
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }
         
        //constructor with the 4 parameters of the buttons we will make
        public Button(string name, Color color, Texture2D texture, int buttonX, int buttonY)
        {
            this.name = name;
            this.color = color;
            this.texture = texture;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
        }

        //method for checking if mouse cursor is inside button
        //changes color of button to white 
        public bool mouseEnter()
        {
            if (m.Position.X < buttonX + Texture.Width && m.Position.X > buttonX && m.Position.Y < buttonY + Texture.Height && m.Position.Y > buttonY)
            {
                color = Color.White;
                return true;
                
            }
            else
            {
                return false;
            }
        }



    }
}
