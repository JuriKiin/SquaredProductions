using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

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
        Texture2D basicUI;
        Texture2D battleUI;
        SpriteFont font;
        int potsAmount = 10;

        //Player Rectangle
        Rectangle playerRec;

        // Protag Textures 
        Texture2D protagDownStill;
        Texture2D protagDownWalk1;
        Texture2D protagDownWalk2;

        Texture2D protagLeftWalk;
        Texture2D protagLeftStill;

        Texture2D protagRightWalk;
        Texture2D protagRightStill;

        Texture2D protagUpStill;
        Texture2D protagUpWalk1;
        Texture2D protagUpWalk2;

        //protag movng enum 
        enum Facing {Right, Left, Up, Down };
        enum Motion {StandRight, StandLeft, StandUp, StandDown, WalkRight, WalkLeft, WalkUp, WalkDown };

        Facing Direction;
        Motion Moving;

        int frame;
        double timePerFrame = 200;
        double timePerFrame1 = 300;
        int frame1;
        int numFrames = 2;
        int framesElapsed;

        // Antag Textures 

        Texture2D curTex;

        Texture2D antagDownStill;
        Texture2D antagDownWalk1;
        Texture2D antagDownWalk2;

        Texture2D antagLeftWalk;
        Texture2D antagLeftStill;

        Texture2D antagRightWalk;
        Texture2D antagRightStill;

        Texture2D antagUpStill;
        Texture2D antagUpWalk1;
        Texture2D antagUpWalk2;

        Texture2D meter;    //This is the meter that te player will use to attack
        Rectangle meterRec; //This is the rectangle that will keep track of the meter's position.
        Texture2D meterObj; //This is the meter object that will be moving back and forth when the player is attacking.
        Rectangle meterObjRec;  //This is the rectangle that will keep track of the position of the meter obj;

        Texture2D overScreen;   //Game over Screen. (Class, our names. etc.)

        Texture2D protag;   //Protag Texture
        Texture2D antag;    //Antag texture

        // Maps 
        Texture2D Quarter;

        // Vector attributes
        Vector2 pauseVec;
        Vector2 spawnVec = new Vector2(500, 450);

        public Vector2 Barpos { get; set; }

        //Collection variables
        List<Enemy> enemies = new List<Enemy>();    //This list will be filled with all of the enemies in each level. Every time a level is loaded, the list will be emptied and loaded with new enemies.
        List<Arrow> arrows = new List<Arrow>(); //A list of all of the keys the user has to press (arrow keys and letter keys)
        List<Arrow> curArrows = new List<Arrow>();  //A list of the keys the player has to hit every time he faces an enemy. (Reset with every battle)

        Rectangle arrowSpawnPoint = new Rectangle(500,500,256,256);

        bool hits = false;
        //Game State machine
        enum GameState { Menu, Walk, Combat, Over, Pause, Item, Stats};
        GameState curState;

        enum CombatState { Attack, Block }; //This enum toggles between the attack and block phases of the combat gameplay.
        CombatState cmbState;

        //Keyboard States
        KeyboardState kbState;
        KeyboardState prevState = Keyboard.GetState();

        //INT VARIABLES
        //Attributes to resize window
        int winX = 1000;
        int winY = 1000;

        int level;  // this variable tells us which data to load. (Switch statement)

        //OBJECTS
        MouseState m = Mouse.GetState();
        Player player;
        int arrCount = 0;
        Enemy curEnemy; //This object will be the enemy object that we fill with whatever enemy the player intersects with.
        Random rnd = new Random();
        LevelManager manager;
        ArrowSpawn arrowSpawner = new ArrowSpawn();
        Enemy TestGoon;

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
            Direction = Facing.Down;
            Moving = Motion.StandDown;
            
            //define player object and position
            player = new Player(20, 20, 4, 12, 0);
            player.PlayerRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 150, GraphicsDevice.Viewport.Height / 2 - 300, 300, 300);


            //define test goon
            TestGoon = new Enemy(10, 2, 5, "Dumb Goon", 1, true);
            TestGoon.Pos = new Rectangle(0, 0, 75, 85);
            enemies.Add(TestGoon);            

            //camera object
            cam = new Camera(GraphicsDevice.Viewport);
           
            ReadFiles();    //Creates each enemy
            arrowSpawner.LoadArrow(Content);   //This should load all of the arrow keys into arrows.

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

            startMenu = Content.Load<Texture2D>("UI\\Start Menu.png");  // Loading in the start menu
            statsMenu = Content.Load<Texture2D>("UI\\Stats Menu.png");  // Loading in the stats menu
            itemsMenu = Content.Load<Texture2D>("UI\\Items Menu.png");  // Loading in the items menu
            title = Content.Load <Texture2D>("UI\\Title Screen.png");   // Loading in title screen
            basicUI = Content.Load<Texture2D>("UI\\Basic UI.png");      // Loading in basic UI
            battleUI = Content.Load<Texture2D>("UI\\Battle UI.png");    // Loading in the battle UI
            font = Content.Load<SpriteFont>("UI\\Font1");               // Loading in font for stats



            // ACTUAL PLAYER SPRITE LOAD UP FOR PRO AND ANTAG 

            //

            // Protag Textures 
            protagDownStill = Content.Load<Texture2D>("Characters\\Protag\\ProtagDownStill.png");
            protagDownWalk1 = Content.Load<Texture2D>("Characters\\Protag\\ProtagDownWalk1.png");
            protagDownWalk2 = Content.Load<Texture2D>("Characters\\Protag\\ProtagDownWalk2.png");
            protagLeftWalk = Content.Load<Texture2D>("Characters\\Protag\\ProtagLeftWalk.png");
            protagLeftStill = Content.Load<Texture2D>("Characters\\Protag\\ProtagLeftStill.png");

            protagRightWalk = Content.Load<Texture2D>("Characters\\Protag\\ProtagRightWalk.png");
            protagRightStill = Content.Load<Texture2D>("Characters\\Protag\\ProtagRightStill.png");

            protagUpStill = Content.Load<Texture2D>("Characters\\Protag\\ProtagUpStill.png");
            protagUpWalk1 = Content.Load<Texture2D>("Characters\\Protag\\ProtagUpWalk1.png");
            protagUpWalk2 = Content.Load<Texture2D>("Characters\\Protag\\ProtagUpWalk2.png");

            // Antag Textures 
            antagDownStill = Content.Load<Texture2D>("Characters\\Antag\\AntagDownStill.png");
            antagDownWalk1 = Content.Load<Texture2D>("Characters\\Antag\\AntagDownWalk1.png");
            antagDownWalk2 = Content.Load<Texture2D>("Characters\\Antag\\AntagDownWalk2.png");

            antagLeftWalk = Content.Load<Texture2D>("Characters\\Antag\\AntagLeftWalk.png");
            antagLeftStill = Content.Load<Texture2D>("Characters\\Antag\\AntagLeftStill.png");

            antagRightWalk = Content.Load<Texture2D>("Characters\\Antag\\AntagRightWalk.png");
            antagRightStill = Content.Load<Texture2D>("Characters\\Antag\\AntagRightStill.png");

            antagUpStill = Content.Load<Texture2D>("Characters\\Antag\\AntagUpStill.png");
            antagUpWalk1 = Content.Load<Texture2D>("Characters\\Antag\\AntagUpWalk1.png");
            antagUpWalk2 = Content.Load<Texture2D>("Characters\\Antag\\AntagUpWalk2.png");

            manager = new LevelManager(Content);
            manager.LoadNextLevel();


            //overScreen = Content.Load<Texture2D>("UI\\overScreen.png");   //Loading in the game voer screen.
            //meter = Content.Load<Texture2D>("UI\\combatMeter.png");  //Loading in the combat meter
            //meterObj = Content.Load<Texture2D>("UI\\combatMeterObj.png");   //Loading in the combat meter object


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

            

            // keybaard state for player movement 
            KeyboardState protag = Keyboard.GetState();

            //Gets current state of mouse
            MouseState m = Mouse.GetState();

            //Keyboard states
            prevState = kbState;
            kbState = Keyboard.GetState();

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame1);
            frame1 = framesElapsed % numFrames;

            player.X = player.PlayerRec.X;
            player.Y = player.PlayerRec.Y;

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

                    //If P is hit, pause the game.
                    if (SingleKeyPress(Keys.P)) curState = GameState.Pause;

                    //moves the camera, therefore the map and map elements.
                    Move();

                    if(!protag.IsKeyDown(Keys.W) && !protag.IsKeyDown(Keys.S) && !protag.IsKeyDown(Keys.A) && !protag.IsKeyDown(Keys.D))
                    {
                        if(Direction == Facing.Right)
                        {
                            Moving = Motion.StandRight;
                        }
                        if (Direction == Facing.Left)
                        {
                            Moving = Motion.StandLeft;
                        }
                        if (Direction == Facing.Up)
                        {
                            Moving = Motion.StandUp;
                        }
                        if (Direction == Facing.Down)
                        {
                            Moving = Motion.StandDown;
                        }
                    }
                    else if (protag.IsKeyDown(Keys.W))
                    {
                        Direction = Facing.Up;
                        Moving = Motion.WalkUp;
                    }
                    else if (protag.IsKeyDown(Keys.S))
                    {
                        Direction = Facing.Down;
                        Moving = Motion.WalkDown;
                    }
                    else if (protag.IsKeyDown(Keys.A))
                    {
                        Direction = Facing.Left;
                        Moving = Motion.WalkLeft;
                    }
                    else if (protag.IsKeyDown(Keys.D))
                    {
                        Direction = Facing.Right;
                        Moving = Motion.WalkRight;
                    }

                   foreach (Enemy e in enemies)     //Check to see if the player position intersects with any of the enemies.
                    {
                       if (e.Pos.Intersects(player.PlayerRec))
                       {
                            curEnemy = e;   //Sets the enemy
                            curState = GameState.Combat;    //Set the gamestate to combat
                       }
                    }
                    break;

                case GameState.Combat:  //Begin combat

                    int totalHits = 0;   //This int keeps track of the number of times the player tries to tap the correct button. If there are no more arrows, then reset the list and then go to attacking.
                    
                    switch (cmbState)
                    {
                        case CombatState.Attack:

                            //Put meter-based code here for correct timing and damage

                            if (curEnemy.CurrHealth > 0)
                            {
                                cmbState = CombatState.Block;     //Set the attack phase to blocking.
                            }
                            else
                            {
                                curState = GameState.Walk;        //Says the battle is over
                            }
                            
                            break;

                        case CombatState.Block:
                            if (arrCount == 0)
                            {
                                curArrows = arrowSpawner.GenerateArrows(curEnemy.numArrow, curEnemy.Directional);   //Populate the list of current arrows the player needs to hit.
                                arrCount = curArrows.Count;
                                //for(int i = 0; i<= curArrows.Count - 1; i++)      //Move the arrows across the screen.
                                //{
                                //   curArrows[i].Rec = new Rectangle(curArrows[i].Rec.X - i  * 50, curArrows[i].Rec.Y, 512, 512);
                                //}
                            }
                            //for(int i = 0; i<= curArrows.Count - 1; i++)      //Move the arrows across the screen.
                            //{
                            //   curArrows[i].Rec = new Rectangle(curArrows[i].Rec.X - 7, curArrows[i].Rec.Y, 512, 512);
                            //}

                            curArrows[0].Rec = new Rectangle(curArrows[0].Rec.X - 7, curArrows[0].Rec.Y, 150, 150);
                            for (int a = 0; a < curArrows.Count; a++)  //Check if the player presses the correct key when the key sprite gets to the hitbox.
                                {
                                    if (75 < (curArrows[a].Rec.X) && 350 > (curArrows[a].Rec.X) )   //Checks for collision
                                        {
                                            hits = true;
                                            if (SingleKeyPress(curArrows[a].KeyValue))
                                            {
                                                totalHits++;
                                                
                                                curArrows.RemoveAt(a);
                                                a--;
                                            }
                                        }
                                    else if (curArrows[a].Rec.X < 0 - 200 - curArrows[a].Rec.Width)
                                        {
                                             
                                             curArrows.RemoveAt(a);
                                    hits = false;
                                             a--;
                                        } 
                                }
                                
                            //--    WHAT TO DO IF WE WANT TO CHANGE THE PHASE THE COMBAT IS IN  --//

                            if (curArrows.Count <= 0)
                            {
                                player.CurHealth -= (arrCount - totalHits) * curEnemy.Damage;  //Subtract the health 
                                arrCount = 0;
                                
                                cmbState = CombatState.Attack;  //Set the combat state to attacking after a brief pause.
                            }
                            else
                            {

                            }

                            
                            break;

                        default: break;
                    }
                    break;

                case GameState.Pause:
                    this.IsMouseVisible = true; //Make the mouse visable

                    if (SingleKeyPress(Keys.I))     //If 'i' is pressed, switch the gamestate to Items
                    {
                        curState = GameState.Item;

                        if (SingleKeyPress(Keys.H))
                        {
                            player.CurHealth += 10;
                        }
                    }
                    else if (SingleKeyPress(Keys.U))    //If 'u' is pressed, switch the gamState to Stats
                    {
                        curState = GameState.Stats;
                    }
                    else if (SingleKeyPress(Keys.Enter))     //Return to normal gameplay.
                    {
                        curState = GameState.Walk;
                    }
                    break;

                case GameState.Stats:
                    if (SingleKeyPress(Keys.I)) //Switch to item menu
                    {
                        curState = GameState.Item;
                    }
                    else if (SingleKeyPress(Keys.P))       //Switch to normal pause method.
                    {
                        curState = GameState.Pause;
                    }
                    else if (SingleKeyPress(Keys.Enter))    //Return to normal gameplay.
                    {
                        curState = GameState.Walk;
                    }
                    break;

                case GameState.Item:
                    if (SingleKeyPress(Keys.U))     //Go to Stats menu
                    {
                        curState = GameState.Stats;
                    }
                    else if (SingleKeyPress(Keys.P))    //To go normal pause screen.
                    {
                        curState = GameState.Pause;
                    }
                    else if (SingleKeyPress(Keys.Enter))    //Return to normal gameplay.
                    {
                        curState = GameState.Walk;
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

                    //Draw HP in Walking UI
                    spriteBatch.DrawString(font, "HP: " + player.CurHealth, new Vector2(180, 900), Color.White);
                    
                    //draw map
                    var viewMatrix = cam.GrabMatrix();
                    mapBatch.Begin(transformMatrix: viewMatrix);
                    mapBatch.Draw(manager.CurLevelTexture, new Rectangle(0, 0, 1000, 1800), Color.White);   //Draws the level background
                    mapBatch.End();

                    //Draw each enemy
                    foreach (Enemy e in enemies)
                    {
                        spriteBatch.Draw(protagDownStill, new Rectangle(e.Pos.X, e.Pos.Y, 75, 85), Color.White);
                    }

                    //Draw basic UI
                    spriteBatch.Draw(basicUI, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    switch (Moving)
                    {
                        case Motion.StandDown: spriteBatch.Draw(protagDownStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White); break;
                        case Motion.StandUp: spriteBatch.Draw(protagUpStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White); break;
                        case Motion.StandLeft: spriteBatch.Draw(protagLeftStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White); break;
                        case Motion.StandRight: spriteBatch.Draw(protagRightStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White); break;

                        case Motion.WalkDown:
                            if (frame1 == 1)
                            {
                                spriteBatch.Draw(protagDownWalk1, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(protagDownWalk2, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            break;
                        case Motion.WalkUp:
                            if (frame1 == 1)
                            {
                                spriteBatch.Draw(protagUpWalk2, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(protagUpWalk1, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            break;
                        case Motion.WalkLeft:
                            if (frame == 1)
                            {
                               
                                spriteBatch.Draw(protagRightWalk, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                            }
                            else
                            {
                                spriteBatch.Draw(protagRightStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                            }

                            break;
                        case Motion.WalkRight:
                            if (frame == 1)
                            {
                                spriteBatch.Draw(protagRightWalk, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(protagRightStill, new Rectangle(player.PlayerRec.X, player.PlayerRec.Y, 75, 85), Color.White);
                            }
                            break;
                    }
                    break;

                //Draw the GUI in combat
                case GameState.Combat:

                    switch (cmbState)
                    {
                        case CombatState.Attack:
                            
                            break;

                        case CombatState.Block:
                            spriteBatch.DrawString(font, ":" + hits, new Vector2(0, 0), Color.White);
                            int iteration = 0;
                            foreach(Arrow a in curArrows)        //Draw each Arrow
                            {
                                iteration++;

                                spriteBatch.Draw(a.CurTexture,new Rectangle(a.Rec.X + (iteration * a.Rec.Width + 50), a.Rec.Y, a.Rec.Width, a.Rec.Height),Color.White);
                            }
                            
                            break;
                        default: break;

                    }
                    spriteBatch.Draw(battleUI, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.Over:

 //                   spriteBatch.Draw(overScreen,new Rectangle(0,0,winX,winY),Color.White);    //Draw the game over screen.

                    break;

                case GameState.Pause:
                    //Draw pause menu GUI
                    spriteBatch.Draw(startMenu, pauseVec, Color.White); //Draw pause menu
                    break;

                case GameState.Item:
                    spriteBatch.Draw(itemsMenu, pauseVec, Color.White); //Draw items menu
                    if (SingleKeyPress(Keys.H))
                    {
                        potsAmount--;
                        if (potsAmount <= 0)
                        {
                            potsAmount = 0;
                        }
                    }
                    spriteBatch.DrawString(font, "x" + potsAmount, new Vector2(800, 300), Color.White);
                    break;

                case GameState.Stats:
                    spriteBatch.Draw(statsMenu, pauseVec, Color.White); //Draw the stats menu.
                    spriteBatch.DrawString(font, "" + player.Armor, new Vector2(580, 165), Color.White);
                    spriteBatch.DrawString(font, "" + player.Damage, new Vector2(580, 400), Color.White);
                    spriteBatch.DrawString(font, "" + player.Boost, new Vector2(580, 635), Color.White);
                    spriteBatch.DrawString(font, "", new Vector2(580, 235), Color.White);
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
            //kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(key) && prevState.IsKeyUp(key))
            { return true; }
            else { return false; }
        }


        public void Move()
        {
            KeyboardState ks = Keyboard.GetState();

            //Make sprite move, and change sprite if the player looks differently
            if (ks.IsKeyDown(Keys.A))
            {
                cam.Position -= new Vector2(3, 0);
                foreach(Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X + 3, e.Pos.Y, e.Pos.Width, e.Pos.Height);
                }

            }//Move Left
            if (ks.IsKeyDown(Keys.D))
            {
                cam.Position -= new Vector2(-3, 0);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X - 3, e.Pos.Y, e.Pos.Width, e.Pos.Height);
                }

            } //Move Right
            if (ks.IsKeyDown(Keys.W))
            {
                cam.Position -= new Vector2(0, 3);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X, e.Pos.Y + 3, e.Pos.Width, e.Pos.Height);
                }
            }
            //Move Up
            if (ks.IsKeyDown(Keys.S))
            {
                cam.Position -= new Vector2(0, -3);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X, e.Pos.Y - 3, e.Pos.Width, e.Pos.Height);
                }
            } //Move Down
        }

        //Combat System Below.

        //THIS METHOD LOADS ENEMIES IN FROM THE ENEMY FILES
        public void ReadFiles()
        {
            string[] files = Directory.GetFiles(".");

//            for(int i=0;i<files.Length-1;i++)
            foreach(string file in files)
            {
                if (file.Contains("$_$"))
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(file));

                    bool directional = true;

                    // need to follow the file format to get the data
                    int numArrow = br.ReadInt32();
                    int health = br.ReadInt32();
                    int damage = br.ReadInt32();
                    int armor = br.ReadInt32();
                    int dir = br.ReadInt32();

                    if (dir == 0) directional = true;
                    if (dir == 1) directional = false;
                    //Create the enemy and add it to the enemies list in game1
                    Enemy e = new Enemy(health, damage, numArrow, "Enemy", armor, directional);
                    e.Pos = new Rectangle(rnd.Next(-200, 500), rnd.Next(-200, 590), 200, 200);
                    e.EnemyTexture = protag;
                    enemies.Add(e);
                    // close when we are done
                    br.Close();
                }
            }
        }


    }
}
