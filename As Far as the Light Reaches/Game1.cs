using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections;
using System;

namespace As_Far_as_the_Light_Reaches
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch mapBatch;
        Camera cam;

        ////TEXTURE VARIABLES

        // Menu Attributes
        private bool pause = false;
        Texture2D startMenu;
        Texture2D statsMenu;
        Texture2D itemsMenu;
        Texture2D title;
        Texture2D battleUI;

        //Arrow Key Textures
        Texture2D aR;   //Right arrow
        Texture2D aL;   //Left arrow
        Texture2D aU;   //Up arrow
        Texture2D aD;   //Down arrow

        Texture2D meter;    //This is the meter that te player will use to attack
        Rectangle meterRec; //This is the rectangle that will keep track of the meter's position.
        Texture2D meterObj; //This is the meter object that will be moving back and forth when the player is attacking.
        Rectangle meterObjRec;  //This is the rectangle that will keep track of the position of the meter obj;


        Texture2D protag;   //Protag Texture
        Texture2D antag;    //Antag texture

        // Vector attributes
        Vector2 pauseVec;
        Vector2 spawnVec = new Vector2(1000, 450);

        public Vector2 Barpos { get; set; }

        //Collection variables
        List<Enemy> enemies = new List<Enemy>();    //This list will be filled with all of the enemies in each level. Every time a level is loaded, the list will be emptied and loaded with new enemies.
        List<Texture2D> arrows = new List<Texture2D>(); //A list of all of the keys the user has to press (arrow keys and letter keys)
        List<Texture2D> curArrows = new List<Texture2D>();  //A list of the keys the player has to hit every time he faces an enemy. (Reset with every battle)

        Dictionary<int, Texture2D[]> levelManager = new Dictionary<int, Texture2D[]>();


        //State machine
        enum GameState { Menu, Walk, Combat, Over };
        GameState curState;

        //Keyboard States
        KeyboardState kbState;
        KeyboardState prevState = Keyboard.GetState();

        //Bool variables
        bool canMove = true;    //Make the player not move if the game is paused.
        bool attackState;       // True = attacking. False = blocking.

        //INT VARIABLES
        //Attributes to resize window
        int winX = 1000;
        int winY = 1000;

        int level;  // this variable tells us which data to load. (Switch statement)

        //OBJECTS
        //Mouse object
        MouseState m = Mouse.GetState();
        Player player = new Player(20, 20, 4, 12);
        Enemy curEnemy; //This object will be the enemy object that we fill with whatever enemy the player intersects with.
        Random rnd = new Random();



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
            pauseVec = new Vector2(-500, -100);
            curState = GameState.Menu;

            //camera object
            cam = new Camera(GraphicsDevice.Viewport);

            //Load the arrows into the list
            arrows.Add(aR);
            arrows.Add(aL);
            arrows.Add(aU);
            arrows.Add(aD);

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
            mapBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            // Loading in the protagonist sprite
            protag = Content.Load<Texture2D>("Characters\\Protag\\Protag.png");
            startMenu = Content.Load<Texture2D>("UI\\Start Menu.png");  // Loading in the start menu
            statsMenu = Content.Load<Texture2D>("UI\\Stats Menu.png");  // Loading in the stats menu
            itemsMenu = Content.Load<Texture2D>("UI\\Items Menu.png");  // Loading in the items menu
            title = Content.Load <Texture2D>("UI\\Title Screen.png");   // Loading in title screen
            battleUI = Content.Load<Texture2D>("UI\\Battle UI.png");   //Loading in the battle UI.

            //Arrow keys
            aR = Content.Load<Texture2D>("UI\\ArrowKey\\Right.png");
            aL = Content.Load<Texture2D>("UI\\ArrowKey\\Left.png");
            aU = Content.Load<Texture2D>("UI\\ArrowKey\\Up.png");
            aD = Content.Load<Texture2D>("UI\\ArrowKey\\Down.png");


            meter = Content.Load<Texture2D>("UI\\combatMeter.png");  //Loading in the combat meter
            meterObj = Content.Load<Texture2D>("UI\\combatMeterObj.png");   //Loading in the combat meter object


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

                    if (SingleKeyPress(Keys.Space))
                    {
                        curState = GameState.Walk;      //Change the gamestate to walk (normal gameplay)
                        level = 0;
                    }

                    break;

                case GameState.Walk:
                    //Update the player movement, and if the player pauses the game.
                    if (canMove) Move();    //Move the player around if the game isn't paused.
                    Pause();    //Update the game to check if the player pauses the game.

                    foreach (Enemy e in enemies)     //Check to see if the player position intersects with any of the enemies.
                    {
                        if (player.PlayerRec.Intersects(e.Pos))
                        {
                            curEnemy = e;   //Sets the enemy 
                            curState = GameState.Combat;    //Set the gamestate to combat
                        }

                    }

                    break;

                //When we are in combat, we need to get the number of arrows to spawn.
                case GameState.Combat:

                    int arr = SpawnArrow(); //Get the number of arrows

                    attackState = true; //Set the attack state to attacking.

                    //Player attack
                    if (attackState)
                    {
                        //Check for the position of the meter object to that of the meter, and call the damage method with that int.
                        meterObjRec.X -= 3;     //Move the arrows across the screen.


                        if (SingleKeyPress(Keys.Enter))
                        {
                            meterObjRec.X = +0;    //Stop the position of the meter object
                            curEnemy.PlayerAttack(player.Damage, meterObjRec.X);


                        }

                        attackState = false;    ///lastly set the attack phase to blocking.
                    }

                    if (!attackState)
                    {
                        //Reset the arrow list and then repopulate it with the right number of arrows.
                        if (arrows != null) arrows.Clear(); //Resets the list
                        for (int i = 0; i < arr; i++)
                        {
                            curArrows.Add(arrows[rnd.Next(0, arrows.Count - 1)]);   //Populate the current arrows with random arrows from the list.
                        }

                        attackState = true;     //Lastly set the attack phase to attacking.
                    }

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

                    //draw map
                    var viewMatrix = cam.GrabMatrix();
                    mapBatch.Begin(transformMatrix: viewMatrix);
                    //mapBatch.Draw(map, new Rectangle(0, 0, w, h)Color.White);
                    mapBatch.End();


                    //If the player pauses the game1
                    if (pause)
                    {
                        canMove = false;    //Prevent the player from walking when the menu is up.
                        //Show the cursor
                        this.IsMouseVisible = true; //Make the mouse visable
                        //Draw pause menu GUI
                        spriteBatch.Draw(startMenu, pauseVec, Color.White);
                    }


                    break;

                //Draw the GUI in combat
                case GameState.Combat:
                    spriteBatch.Draw(battleUI,new Rectangle(0,0,winX,winY),Color.White);    //Draw the battle UI

                    //Draw each Arrow
                    foreach (Texture2D arrow in curArrows)
                    {
                        spriteBatch.Draw(arrow,spawnVec,Color.White);
                    }

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
            if (ks.IsKeyDown(Keys.A)) cam.Position -= new Vector2(-3,0); //Move Left
            if (ks.IsKeyDown(Keys.D)) cam.Position -= new Vector2(3,0); //Move Right
            if (ks.IsKeyDown(Keys.W)) cam.Position -= new Vector2(0,3); //Move Up
            if (ks.IsKeyDown(Keys.S)) cam.Position -= new Vector2(0,-3); //Move Down
        }

        //Combat System Below.

        //This method takes how many arrows we should spawn. It takes the number from the enemy object that we are getting.
        public int SpawnArrow()
        {
            int numArrow;
            numArrow = curEnemy.NumArrow;   //Sets the number of arrows to the value of numArrow that the given enemy has.
            return numArrow;
        }



    }
}
