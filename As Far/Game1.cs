using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace As_Far
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // additional attributes
        private bool pause = false;
        Texture2D sm;
        Vector2 vec;

        //Attributes to resize window
        int winX = 1000;
        int winY = 1000;

        //Player Attributes
        Texture2D protag;
        Vector2 playerPos;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //Resize Window to attributes.
            graphics.PreferredBackBufferWidth = winX;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = winY;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            vec = new Vector2(-500, -100);

            //Load all of the protag images into an array

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            protag = Content.Load<Texture2D>("Characters\\Protag\\Protag.png");



            // Loading in the start menu
            sm = Content.Load<Texture2D>("UI\\StartMenu.png");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // (*TEMPORARY*) Making the menu move so you can see the whole thing

            Pause();
            Move();

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);

            // TODO: Add your drawing code here

            // spawn in the menu
            spriteBatch.Begin();

            //Draw character
            spriteBatch.Draw(protag, playerPos, Color.White);

            //If the player pauses the game
            if (pause)
            {
                //Draw pause menu GUI
                spriteBatch.Draw(sm, vec, Color.White);
                //Show the cursor
                this.IsMouseVisible = true;
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Method to initialize the pause menu
        public void Pause()
        {
            KeyboardState ks = Keyboard.GetState();
            //If P is pressed
            if (ks.IsKeyDown(Keys.P)) pause = true;
            //Player will hit the resume button to continue playing.
        }

        public void Move()
        {
            KeyboardState ks = Keyboard.GetState();

            //Make sprite move, and change sprite if the player looks differently
            if (ks.IsKeyDown(Keys.A)) playerPos.X -= 3; //Move Left
            if (ks.IsKeyDown(Keys.D)) playerPos.X += 3; //Move Right
            if (ks.IsKeyDown(Keys.W)) playerPos.Y -= 3; //Move Up
            if (ks.IsKeyDown(Keys.S)) playerPos.Y += 3; //Move Down

        }

    }
}
