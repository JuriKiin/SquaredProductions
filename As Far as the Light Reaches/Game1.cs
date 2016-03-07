using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace As_Far_as_the_Light_Reaches
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Menu Attributes
        private bool pause = false;
        Texture2D startMenu;
        Texture2D statsMenu;
        Texture2D itemsMenu;
        Texture2D title;

        Vector2 vec;
        public Vector2 Barpos { get; set; }

        Texture2D protag;

        //State machine
        enum GameState { Menu, Walk, Combat, Over };
        GameState curState;

        //Keyboard States
        KeyboardState kbState;
        KeyboardState prevState = Keyboard.GetState();

        //Bool variables
        bool canMove = true;


        //Attributes to resize window
        int winX = 1000;
        int winY = 1000;

        //OBJECTS
            //Mouse object
        MouseState m = Mouse.GetState();
        Player player = new Player(20,20,4,12);


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
            curState = GameState.Menu;


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

            // Loading in the protagonist sprite
            protag = Content.Load<Texture2D>("Characters\\Protag\\Protag.png");

            // Loading in the start menu
            startMenu = Content.Load<Texture2D>("UI\\Start Menu.png");

            // Loading in the stats menu
            statsMenu = Content.Load<Texture2D>("UI\\Stats Menu.png");

            // Loading in the items menu
            itemsMenu = Content.Load<Texture2D>("UI\\Items Menu.png");

            //Loading in title screen
            title = Content.Load <Texture2D>("UI\\Title Screen.png");
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

            //Gets current state of mouse
            MouseState m = Mouse.GetState();

            //Switching between states
            switch (curState)
            {
                case GameState.Menu:

                    if (SingleKeyPress(Keys.Space)) curState = GameState.Walk;

                    break;

                case GameState.Walk:
                    //Update the player movement, and if the player pauses the game.
                    if (canMove) Move();
                    Pause();

                    break;

                case GameState.Combat:
                    break;

                case GameState.Over:
                    break;

                default: break;

            }


            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            // spawn in the menu
            spriteBatch.Begin();

            switch (curState)
            {
                case GameState.Menu:

                    spriteBatch.Draw(title, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    break;

                case GameState.Walk:

                    //set player texture
                    player.PlayerTexture = protag;
                    //set player rec
                    player.Width = 300;
                    player.Height = 300;

                    //Draw character
                    spriteBatch.Draw(player.PlayerTexture, player.PlayerRec, Color.White);


                    //If the player pauses the game1
                    if (pause)
                    {
                        canMove = false;    //Prevent the player from walking when the menu is up.
                        //Show the cursor
                        this.IsMouseVisible = true;


                        //Draw pause menu GUI
                        spriteBatch.Draw(startMenu, vec, Color.White);
                    }


                    break;

                case GameState.Combat:
                    break;

                case GameState.Over:
                    break;

                default: break;

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

        public bool SingleKeyPress(Keys key)
        {
            kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(key) && prevState.IsKeyUp(key))
            { prevState = kbState; return true; }
            else { prevState = kbState; return false; }
        }


        public void Move()
        {
            KeyboardState ks = Keyboard.GetState();

            //Make sprite move, and change sprite if the player looks differently
            if (ks.IsKeyDown(Keys.A)) player.X -= 3; //Move Left
            if (ks.IsKeyDown(Keys.D)) player.X += 3; //Move Right
            if (ks.IsKeyDown(Keys.W)) player.Y -= 3; //Move Up
            if (ks.IsKeyDown(Keys.S)) player.Y += 3; //Move Down
        }

        //Combat System Below.

    }
}
