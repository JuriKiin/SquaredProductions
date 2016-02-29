using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace As_Far
    //written by Dustin Knowlton
    // 2/29/2015
    //class generally defines the matrix of transformation that will be applied to draw method
{
    public class Camera
    {
        private readonly Viewport VP;
        
        // used properties
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }

        //constructing camera
        public Camera(Viewport vp)
        {
            VP=vp;
            Rotation = 0f;
            Zoom = 1f;
            Origin = new Vector2(vp.Width / 2f, vp.Height / 2f);
        }
        
        //gets viewpoint matrix
        public Matrix GrabMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0f));
        }
    }
}
