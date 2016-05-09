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

        // Menu Attributes
        private bool pause = false;
        Texture2D startMenu;
        Texture2D statsMenu;
        Texture2D itemsMenu;
        Texture2D title;
        Texture2D basicUI;
        Texture2D battleUI;
        Texture2D hitbox;
        Texture2D classSelect;
        SpriteFont font;
        Texture2D ChoiceScreen;
        Texture2D DirectionsScreen;
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
        
        //Enums for general character motion 
        enum Facing {Right, Left, Up, Down };
        enum Motion {StandRight, StandLeft, StandUp, StandDown, WalkRight, WalkLeft, WalkUp, WalkDown };
        enum MoveState { Left, Right, Still};

        //Variables for movement state machines
        MoveState mState;
        Facing Direction;
        Motion Moving;

        //Frame data
        int frame;
        double timePerFrame = 200;
        double timePerFrame1 = 300;
        int frame1;
        int numFrames = 2;
        int framesElapsed;

        //Antag Textures 
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

        Rectangle meterRec; //This is the rectangle that will keep track of the meter's position.
        Texture2D meterObj; //This is the meter object that will be moving back and forth when the player is attacking.
        Rectangle meterObjRec = new Rectangle(100,425,25,100);  //This is the rectangle that will keep track of the position of the meter obj;

        Texture2D overScreen;  //Game over Screen. (Class, our names. etc.)
        Texture2D loadScreen;  //Loading screen between levels

        Texture2D protag;  //Protag Texture
        Texture2D antag;   //Antag texture

        //Maps 
        Texture2D Quarter;
        Texture2D underground;

        //Vector attributes
        Vector2 pauseVec;
        Vector2 spawnVec = new Vector2(500, 450);
        public Vector2 Barpos { get; set; }

        //Collection variables
        List<Enemy> enemies = new List<Enemy>();    //This list will be filled with all of the enemies in each level. Every time a level is loaded, the list will be emptied and loaded with new enemies.
        List<Arrow> arrows = new List<Arrow>(); //A list of all of the keys the user has to press (arrow keys and letter keys)
        List<Arrow> curArrows = new List<Arrow>();  //A list of the keys the player has to hit every time he faces an enemy. (Reset with every battle)
        List<Wall> walls = new List<Wall>();

        //Rectangle for the arrows spawning in the block state
        Rectangle arrowSpawnPoint = new Rectangle(500,500,256,256);

        //Game State machine
        enum GameState { Menu, Walk, Combat, Over, Pause, Item, Stats, Class, Directions, Choose};
        GameState curState;

        enum CombatState { Attack, Block }; //This enum toggles between the attack and block phases of the combat gameplay.
        CombatState cmbState;

        //Keyboard States
        KeyboardState kbState;
        KeyboardState prevState = Keyboard.GetState();

        //Attributes to resize window
        int winX = 1024;
        int winY = 768;

        int level;  // this variable tells us which data to load. (Switch statement)

        //OBJECTS
        MouseState m = Mouse.GetState();
        Player player;
        Enemy curEnemy; //This object will be the enemy object that we fill with whatever enemy the player intersects with.
        Random rnd = new Random();
        LevelManager manager;
        ArrowSpawn arrowSpawner = new ArrowSpawn();
        DialogueManager Story;

        Enemy TestGoon;
        Rectangle tunnel;
        int totalHits = 0;
        bool dialoguing;
        int logline;

        //does the level need to be genned?
        bool levelgenreq;

        //movement abilities
        bool canUp = true;
        bool canDown = true;
        bool canLeft = true;
        bool canRight = true;
        bool canMove = true;

        int moveSpeed = 5;

        //Vectors where dialogue lines will be placed
        Vector2 lineplace1;
        Vector2 lineplace2;
        Vector2 lineplace3;

        Rectangle startRec;

        //loading in pieces of underground map
        Texture2D one1;
        Texture2D one2;
        Texture2D one3;

        Texture2D two1;
        Texture2D two2;
        Texture2D two3;

        Texture2D three1;
        Texture2D three2;
        Texture2D three3;

        // load for other mapz 

        Texture2D upperground;
        Texture2D underTunnel;
        Texture2D palace;
        List<Texture2D> undergroundmaplist = new List<Texture2D>();

        // enemies 

        Texture2D cow;
        Texture2D horseboy;
        Texture2D unlucky;

        bool endGame = false;


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
            player.PlayerRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - (int)37.5, GraphicsDevice.Viewport.Height / 2 - 143, 75, 85);

            //define level manager
            manager = new LevelManager(Content);
            manager.LoadNextLevel();

            //define dialogue manager
            Story = new DialogueManager();
            lineplace1 = new Vector2(355, 605); //positions for
            lineplace2 = new Vector2(355, 625); //strings in
            lineplace3 = new Vector2(355, 645); //dialogue box (Y values need tweaking with new font)

            //Run Levelgen for level 1
            LevelGen();

            //camera object
            cam = new Camera(GraphicsDevice.Viewport);
           
            ReadFiles();    //Creates each enemy
            arrowSpawner.LoadArrow(Content); //This loads all of the arrow keys into arrows.

            // add underground map pieces to the list 
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\11.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\12.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\13.png"));

            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\21.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\22.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\23.png"));

            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\31.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\32.png"));
            undergroundmaplist.Add(Content.Load<Texture2D>("Maps\\33.png"));

            manager.CurLevel = 0;

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
            loadScreen = Content.Load<Texture2D>("UI\\LoadingScreen.png"); // Loading in load screen between levels
            overScreen = Content.Load<Texture2D>("UI\\GameOverScreen.png"); // Loading in Game Over screen
            hitbox = Content.Load<Texture2D>("UI\\ArrowHitbox.png"); // Loading in hitbox texture for block phase
            classSelect = Content.Load<Texture2D>("UI\\ClassSelectionUI.png"); //Loading in Class Select UI
            ChoiceScreen = Content.Load<Texture2D>("UI\\Choice.png"); //Loading in choice between protag and antag screen
            DirectionsScreen = Content.Load<Texture2D>("UI\\DirectionsScreen.png");

            //PLAYER SPRITE LOAD UP FOR PRO AND ANTAG 

            //Protag Textures 
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

            //Antag Textures 
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

            //Moving meter object for attacking in combat
            meterObj = Content.Load<Texture2D>("UI\\combatMeterObj.png");

            // MAPS 
            upperground = Content.Load<Texture2D>("Maps\\1.png");
            palace = Content.Load<Texture2D>("Maps\\palace.png");
            underTunnel = Content.Load<Texture2D>("Maps\\underTunnel.png");

            // enemies 

            cow = Content.Load<Texture2D>("Enemies\\Cow.png");
            horseboy = Content.Load<Texture2D>("Enemies\\HorseBoy.png");
            unlucky = Content.Load<Texture2D>("Enemies\\Unlucky.png");
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

            //Keyboard state for player movement 
            KeyboardState protag = Keyboard.GetState();

            //Gets current state of mouse
            MouseState m = Mouse.GetState();

            //Keyboard states
            prevState = kbState;
            kbState = Keyboard.GetState();

            //Frame Data
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame1);
            frame1 = framesElapsed % numFrames;

            //Setting player's rectangle = to player's position
            player.X = player.PlayerRec.X;
            player.Y = player.PlayerRec.Y;

            //Switching between states
            switch (curState)
            {
                //START MENU
                case GameState.Menu:

                    if (SingleKeyPress(Keys.Space))
                    {
                        curState = GameState.Directions;      //Change the gamestate to walk (normal gameplay)
                        manager.CurLevel = 0;
                        enemies.Clear();
                        walls.Clear();
                        LevelGen();
                        
                    }
                    break;

                case GameState.Directions:
                    
                    if(SingleKeyPress(Keys.Space))
                    {
                        curState = GameState.Class;
                    }
                    
                    break;
                
                //OVERWORLD STATE
                case GameState.Walk:

                    //if level needs to be generated, run LevelGen
                    if (levelgenreq == true) LevelGen();

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
                            //Story.WriteDialogue(e.startlines);// e.startlines (and endlines later) would be a new int for enemies that determines where their lines start. Would this need to be added to the ext. tool?
                            curEnemy = e;   //Sets the enemy
                                curState = GameState.Combat;    //Set the gamestate to combat
                                cmbState = CombatState.Attack;
                                mState = MoveState.Left;
                           
                        }
                    }

                    if (dialoguing) //Might get replaced.
                    {
                        canUp = false;
                        canDown = false;
                        canLeft = false;
                        canRight = false;
                    }

                    for(int i=0;i<walls.Count;i++)
                    {
                        if (walls[i].Pos.Intersects(player.PlayerRec))
                        {
                            //Could probably have some dialogue here. It might make being reset jarring but not confusing.
                            walls.Clear();
                            enemies.Clear();
                            LevelGen();
                            cam.Position -= cam.Position;
                        }
                    }

                    if (tunnel.Intersects(player.PlayerRec))
                    {                        
                        //LEVEL END dialogue
                        //Story.WriteDialogue(level); //This one works easily. The second you hit the tunnel, you're stopped, look through the dialogue, then transfer levels.
                        manager.LoadNextLevel(); //Prepare map for next level.

                        levelgenreq = true; //If levelgenreq is true, the next update will run the levelgen. 
                        walls.Clear();
                        enemies.Clear();
                        manager.CurLevel++;
                        LevelGen();
                        cam.Position -= cam.Position;
                        System.Threading.Thread.Sleep(200);
                    }
                    break;

                //FIGHTING ENEMIES STATE
                case GameState.Combat:  
                    
                    double attackPercentage = 0;    //This will be the modifier of damage we do to the enemy.
                    double meterLocation = 0;  //This will set the X value of the meterobj rectangle.

                    switch (cmbState)
                    {
                        //ATTACK PHASE OF COMBAT
                        case CombatState.Attack:

                            //MAKES COMBAT METER MOVE BACK AND FORTH ON SCREEN
                            switch(mState)
                            {
                                case MoveState.Left: meterObjRec = new Rectangle(meterObjRec.X + 15, meterObjRec.Y, 25, 150);
                                    if (meterObjRec.X >= 950) mState = MoveState.Right;
                                    break;
                                case MoveState.Right: meterObjRec = new Rectangle(meterObjRec.X - 15, meterObjRec.Y, 25, 150);
                                    if (meterObjRec.X <= 25) mState = MoveState.Left;
                                    break;
                                case MoveState.Still: meterObjRec = new Rectangle(meterObjRec.X,meterObjRec.Y,25,150);
                                    break;
                                default: mState = MoveState.Left;
                                    break;
                            }

                            //SERIES OF STATEMENTS TO DETERMINE DAMAGE 
                            if (SingleKeyPress(Keys.Space))
                            {
                                mState = MoveState.Still;   //Stop the meter    
                                meterLocation = meterObjRec.X;  //Store its location
                                attackPercentage = 100 * (meterLocation / 925);     //Set the attack percentage based on where it is on the meter.

                                if ((attackPercentage >= 0 && attackPercentage <= 15) || (attackPercentage >= 85 && attackPercentage <= 100))   //Set the attack modifier.
                                {
                                    curEnemy.CurrHealth -= (int)(player.Damage * 0.5);  //Poor hit.
                                }
                                else if ((attackPercentage >= 16 && attackPercentage <= 33) || (attackPercentage >= 67 && attackPercentage <= 84))
                                {
                                    curEnemy.CurrHealth -= (int)(player.Damage * 0.75); //Weak hit
                                }
                                else if ((attackPercentage >= 34 && attackPercentage <= 45) || (attackPercentage >= 55 && attackPercentage <= 66))
                                {
                                    curEnemy.CurrHealth -= player.Damage;   //Normal hit.
                                }
                                else if (attackPercentage >= 46 && attackPercentage <= 54)
                                {
                                    curEnemy.CurrHealth -= (int)(player.Damage * 1.5);   //Critical hit.
                                }
                                else
                                {
                                    curEnemy.CurrHealth -= 0;
                                }

                                System.Threading.Thread.Sleep(500);

                                if (curEnemy.CurrHealth <= 0)
                                {
                                    if (endGame)
                                    {
                                        curState = GameState.Over;
                                        return;
                                    }
                                    curState = GameState.Walk;
                                    enemies.Remove(curEnemy);   //Get rid of the enemy in the list of them.
                                }
                                else { cmbState = CombatState.Block; }

                            }

                            break;

                        //BLOCKING PHASE OF COMBAT
                        case CombatState.Block:

                            if (curArrows.Count == 0)  //If the arrow list is empty, fill it.
                            {
                                curArrows = arrowSpawner.GenerateArrows(curEnemy.numArrow, curEnemy.Directional);   //Populate the list of current arrows the player needs to hit.
                            }

                            foreach (Arrow a in curArrows)
                            {
                                a.Rec = new Rectangle(a.Rec.X - curEnemy.Speed, a.Rec.Y, 150, 150);  //Moves each one.
                            }

                            if (SingleKeyPress(curArrows[0].KeyValue))  //If we press the right key
                            {
                                if (curArrows[0].Rec.X >= 250 && curArrows[0].Rec.X <= 300)
                                {
                                    curArrows.RemoveAt(0);
                                }
                                else
                                {
                                    totalHits++;
                                    curArrows.RemoveAt(0);
                                }
                            }

                            if (curArrows.Count != 0)
                            {
                                if (curArrows[0].Rec.X < 120)
                                {
                                    totalHits++;
                                    curArrows.RemoveAt(0);
                                }
                            }

                            //CHANGES PHASE OF COMBAT OR LOADS GAME OVER 
                            if (curArrows.Count <= 0)
                            {
                                int damage = (int)((totalHits * curEnemy.Damage) * player.Armor);

                                player.CurHealth -= damage;  //Subtract the health 

                                if(player.CurHealth <= 0)
                                {
                                    curState = GameState.Over;
                                }

                                cmbState = CombatState.Attack;  //Set the combat state to attacking after a brief pause.
                                meterObjRec.X = 25;
                                mState = MoveState.Left;    //Reset the meter rectangle location and set its moving state.
                                attackPercentage = 0;
                                meterLocation = 0;
                                totalHits = 0;
                            }
                            break;

                        default: break;
                    }
                    break;

                //GAME PAUSED STATE
                case GameState.Pause:

                    IsMouseVisible = true; //Make the mouse visable

                    //SERIES TO CHECK FOR MENU SWITCHING
                    if (SingleKeyPress(Keys.I))     //If 'i' is pressed, switch the gamestate to Items
                    {
                        curState = GameState.Item;

                        //Checks if health potion is used 
                        if (SingleKeyPress(Keys.H) && potsAmount > 0)
                        {
                            player.CurHealth = player.MaxHealth;

                            potsAmount--;
                            if (potsAmount <= 0)
                            {
                                potsAmount = 0;
                            }
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

                    else if (SingleKeyPress(Keys.P))    //Go to normal pause screen.
                    {
                        curState = GameState.Pause;
                    }

                    else if (SingleKeyPress(Keys.Enter))    //Return to normal gameplay.
                    {
                        curState = GameState.Walk;
                    }

                    break;

                //CHOOSING CLASS STATE
                case GameState.Class:

                    bool canPlay = false;

                    if (SingleKeyPress(Keys.T))
                    {
                        player.MaxHealth = 26;
                        player.CurHealth = 26;
                        player.Damage = 3;
                        player.Armor = 0.5;
                        canPlay = true;
                    }

                    if (SingleKeyPress(Keys.A))
                    {
                        player.MaxHealth = 14;
                        player.CurHealth = 14;
                        player.Damage = 5;
                        player.Armor = 1.5;
                        canPlay = true;
                    }

                    if (SingleKeyPress(Keys.W))
                    {
                        player.MaxHealth = 18;
                        player.CurHealth = 18;
                        player.Damage = 7;
                        player.Armor = 0.75;
                        canPlay = true;
                    }

                    if (SingleKeyPress(Keys.B))
                    {
                        player.MaxHealth = 20;
                        player.CurHealth = 20;
                        player.Damage = 6;
                        player.Armor = 1;
                        canPlay = true;
                    }

                    if(canPlay)
                    {
                        curState = GameState.Walk;
                    }

                    break;
                
                //END GAME STATE
                case GameState.Over:
                    if(SingleKeyPress(Keys.Space))
                    {
                        curState = GameState.Menu;
                    }

                    break;



                case GameState.Choose:
                    if (SingleKeyPress(Keys.Left))
                    {
                        curEnemy = new Enemy((player.MaxHealth)*2,player.Damage,14,"Player",4,true,6);

                        endGame = true;
                        curState = GameState.Combat;
                        cam.Position -= cam.Position;
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        curEnemy = new Enemy((player.MaxHealth + 2)*2,player.Damage + 2, 16, "Boss", 3, true, 6);

                        endGame = true;
                        curState = GameState.Combat;
                        cam.Position -= cam.Position;
                    }


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
            
            spriteBatch.Begin();

            switch (curState)
            {
                case GameState.Menu:

                    spriteBatch.Draw(title, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.Directions:

                    spriteBatch.Draw(DirectionsScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.Class:

                    spriteBatch.Draw(classSelect, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.Walk:

                    //Draw HP in Walking UI
                    spriteBatch.DrawString(font, "HP: " + player.CurHealth, new Vector2(180, 900), Color.White);
                    
                    //Draw map
                    mapBatch.Begin(transformMatrix: cam.GrabMatrix());                 
                    // 2737 2965 dimensions for each underground piece 
                    switch (manager.CurLevel)
                    {
                        case 0:
                            mapBatch.Draw(upperground, new Rectangle(-4000, -7600, 5000, 8000), Color.White);




                            break;
                        case 1:
                            mapBatch.Draw(undergroundmaplist[0], new Rectangle(0, 0, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[1], new Rectangle(2737, 0, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[2], new Rectangle(5469, 0, 2737, 2965), Color.White);

                            mapBatch.Draw(undergroundmaplist[3], new Rectangle(0, 2965, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[4], new Rectangle(2750, 2965, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[5], new Rectangle(5469, 2965, 2737, 2965), Color.White);

                            mapBatch.Draw(undergroundmaplist[6], new Rectangle(0, 5930, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[7], new Rectangle(2750, 5930, 2737, 2965), Color.White);
                            mapBatch.Draw(undergroundmaplist[8], new Rectangle(5469, 5930, 2737, 2965), Color.White);


                            break;

                        case 2:
                            mapBatch.Draw(underTunnel, new Rectangle(195, -850, underTunnel.Width, underTunnel.Height), Color.White);
                            if (cam.Position.Y <= -1062)
                            {
                                System.Threading.Thread.Sleep(200);
                                manager.CurLevel = 3;
                                enemies.Clear();
                            }


                            break;

                        case 3:
                            Rectangle r = new Rectangle(450, -1030, (int)(antagDownStill.Width * 1.15), (int)(antagDownStill.Height * 1.15));

                            mapBatch.Draw(palace, new Rectangle(0, -1400, palace.Width/2, palace.Height/2), Color.White);
                            mapBatch.Draw(antagDownStill,r,Color.White);

                            if (cam.Position.Y <= -1020)
                            {
                                curState = GameState.Choose;
                            }

                            break;


                    }


                    //Draw each enemy
                    foreach (Enemy e in enemies)
                    {
                        spriteBatch.Draw(unlucky, new Rectangle(e.Pos.X, e.Pos.Y, 75, 85), Color.White);
                    }


                    mapBatch.End();


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

                case GameState.Combat:

                    switch (cmbState)
                    {
                        case CombatState.Attack:

                            //Draw the UI for combat
                            spriteBatch.Draw(battleUI, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                            //Draw the meter going back and forth on the screen.
                            spriteBatch.Draw(meterObj, meterObjRec, Color.White);   

                            //Draw the hp values of each fighter
                            spriteBatch.DrawString(font, "Enemy's Health: " + curEnemy.CurrHealth, new Vector2(10, 10), Color.White);
                            spriteBatch.DrawString(font, "Player Health: " + player.CurHealth, new Vector2(10, 60), Color.White);

                            break;

                        case CombatState.Block:

                            //Draw each arrow
                            foreach(Arrow a in curArrows)        
                            {
                                //Draw each arrow to the screen
                                spriteBatch.Draw(a.CurTexture,new Rectangle(a.Rec.X, a.Rec.Y, a.Rec.Width, a.Rec.Height),Color.White);
                            }

                            //Draws the hitbox for the arrows
                            spriteBatch.Draw(hitbox, new Rectangle(250, 300, 200, 150), Color.White);

                            //Draw the UI for combat
                            spriteBatch.Draw(battleUI, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                            //Draw the hp values of each fighter
                            spriteBatch.DrawString(font, "Player Health: " + player.CurHealth, new Vector2(10, 60), Color.White);
                            spriteBatch.DrawString(font, "Enemy's Health: " + curEnemy.CurrHealth, new Vector2(10, 10), Color.White);
                                                        
                            break;

                        default:
                            break;
                    }
                    break;

                case GameState.Over:

                    spriteBatch.Draw(overScreen,new Rectangle(0,0,winX,winY),Color.White);    //Draw the game over screen.
                    break;

                case GameState.Pause:

                    //Draw pause menu
                    spriteBatch.Draw(startMenu, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Item:

                    if (SingleKeyPress(Keys.H) && potsAmount > 0)
                    {
                        potsAmount--;
                        player.CurHealth = player.MaxHealth;
                    }

                    //Draw items menu
                    spriteBatch.Draw(itemsMenu, new Vector2(0, 0), Color.White); 
                    spriteBatch.DrawString(font, "x" + potsAmount, new Vector2(670, 275), Color.White);
                    break;

                case GameState.Stats:

                    //Draw the stats menu.
                    spriteBatch.Draw(statsMenu, new Vector2(0,0), Color.White);

                    //Prints out player stats in their own respective sections in the menu
                    spriteBatch.DrawString(font, "" + player.Armor, new Vector2(550, 195), Color.White);
                    spriteBatch.DrawString(font, "" + player.Damage, new Vector2(550, 375), Color.White);
                    spriteBatch.DrawString(font, "" + player.Boost, new Vector2(550, 550), Color.White);
                    break;


                case GameState.Choose:

                    spriteBatch.Draw(ChoiceScreen, new Vector2(0,0), Color.White);

                    break;

                default: break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        //METHOD TO INITIALIZE SERIES OF PAUSE MENUS
        public void Pause()
        {
            KeyboardState ks = Keyboard.GetState();
            //If P is pressed
            if (ks.IsKeyDown(Keys.P)) pause = true;
            //Player will hit the resume button to continue playing.
        }

        //METHOD TO CHECK FOR A SINGLE PRESS OF A KEY
        public bool SingleKeyPress(Keys key)
        {
            //kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(key) && prevState.IsKeyUp(key))
            { return true; }
            else { return false; }
        }

        //MAIN METHOD FOR MOVEMENT OF PLAYER
        public void Move()
        {
            KeyboardState ks = Keyboard.GetState();

            //Make sprite move, and change sprite if the player looks differently
            if (ks.IsKeyDown(Keys.A) && canLeft)//Move Left
            {
                cam.Position -= new Vector2(moveSpeed, 0);
                foreach(Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X + moveSpeed, e.Pos.Y, e.Pos.Width, e.Pos.Height);
                }
                foreach (Wall w in walls)
                {
                    w.Pos = new Rectangle(w.Pos.X + moveSpeed, w.Pos.Y, w.Pos.Width, w.Pos.Height);
                }
                tunnel = new Rectangle(tunnel.X + moveSpeed, tunnel.Y, tunnel.Width, tunnel.Height);

            }

            if (ks.IsKeyDown(Keys.D) && canRight)//Move Right
            {
                cam.Position -= new Vector2(-moveSpeed, 0);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X - moveSpeed, e.Pos.Y, e.Pos.Width, e.Pos.Height);
                }
                foreach (Wall w in walls)
                {
                    w.Pos = new Rectangle(w.Pos.X - moveSpeed, w.Pos.Y, w.Pos.Width, w.Pos.Height);
                }
                tunnel = new Rectangle(tunnel.X - moveSpeed, tunnel.Y, tunnel.Width, tunnel.Height);
            } 

            if (ks.IsKeyDown(Keys.W) && canUp)//Move Up
            {
                cam.Position -= new Vector2(0, moveSpeed);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X, e.Pos.Y + moveSpeed, e.Pos.Width, e.Pos.Height);
                }
                foreach (Wall w in walls)
                {
                    w.Pos = new Rectangle(w.Pos.X, w.Pos.Y + moveSpeed, w.Pos.Width, w.Pos.Height);
                }
                tunnel = new Rectangle(tunnel.X, tunnel.Y + moveSpeed, tunnel.Width, tunnel.Height);
            }

            if (ks.IsKeyDown(Keys.S) && canDown) //Move Down
            {
                cam.Position -= new Vector2(0, -moveSpeed);
                foreach (Enemy e in enemies)
                {
                    e.Pos = new Rectangle(e.Pos.X, e.Pos.Y - moveSpeed, e.Pos.Width, e.Pos.Height);
                }
                foreach (Wall w in walls)
                {
                    w.Pos = new Rectangle(w.Pos.X, w.Pos.Y - moveSpeed, w.Pos.Width, w.Pos.Height);
                }
                tunnel = new Rectangle(tunnel.X,tunnel.Y - moveSpeed, tunnel.Width,tunnel.Height);
            }

            if(SingleKeyPress(Keys.Space))
            {
                dialoguing = false;
            }

        }

        //THIS METHOD LOADS ENEMIES IN FROM THE ENEMY FILES
        public void ReadFiles()
        {
            string[] files = Directory.GetFiles(".");

            foreach (string file in files)
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
                    int speed = br.ReadInt32();

                    if (dir == 0) directional = true;
                    if (dir == 1) directional = false;

                    //Create the enemy and add it to the enemies list in game1
                    Enemy e = new Enemy(health, damage, numArrow, "Enemy", armor, directional, speed);
                    e.Pos = new Rectangle(rnd.Next(-200, 500), rnd.Next(-200, 590), 75, 85);
                    enemies.Add(e);

                    // close when we are done
                    br.Close();
                }
            }
        }

        //LEVEL GENERATING METHOD
        public void LevelGen()
        {
            //clear list of walls
            walls.Clear();
            switch (manager.CurLevel)
            {
                case 0:
                    //set player location and cam if possible
                    //set bounding box for level (?)

                    startRec = new Rectangle(-4000, -7600, 5000, 8000);

               //     walls.Add(new Wall(0, 0, 1000, 1, Wall.direction.up));
                    walls.Add(new Wall(0, 0, 1, 1800, Wall.direction.left));
                    walls.Add(new Wall(1000, 0, 1, 1800, Wall.direction.right));
                   // walls.Add(new Wall(0, 1800, 1000, 1, Wall.direction.down));

                    TestGoon = new Enemy(20, 1, 3, "Enemy0", 1, true,6);
                    TestGoon.Pos = new Rectangle(480, -1000, 75, 85);
                    enemies.Add(TestGoon);

                    Enemy E = new Enemy(18, 1, 6, "Enemy", 1, true,5);
                    E.Pos = new Rectangle(-450, -2300, 75, 85);
                    enemies.Add(E);

                    Enemy E2 = new Enemy(16, 1, 7, "Enemy2", 1, true, 5);
                    E2.Pos = new Rectangle(480, 0, 75, 85);
                    enemies.Add(E2);

                    Enemy E3 = new Enemy(15, 1, 9, "Enemy3", 1, true, 5);
                    E3.Pos = new Rectangle(-450, -4000, 75, 85);
                    enemies.Add(E3);
                     
                    Enemy E4 = new Enemy(16, 1, 12, "Enemy4", 1, true, 5);
                    E4.Pos = new Rectangle(-700, -3000, 75, 85);
                    enemies.Add(E4);

                    Enemy E5 = new Enemy(13, 2, 8, "Enemy5", 1, true, 5);
                    E5.Pos = new Rectangle(-300, -3500, 75, 85);
                    enemies.Add(E5);

                    Enemy E6 = new Enemy(10, 3, 5, "Enemy6", 1, true, 6);
                    E6.Pos = new Rectangle(-550, -5500, 75, 85);
                    enemies.Add(E6);

                    Enemy E7 = new Enemy(8, 2, 10, "Enemy7", 1, true, 6);
                    E7.Pos = new Rectangle(-800, -6000, 75, 85);
                    enemies.Add(E7);

                    //set tunnel, rectangle to transfer level on collide.
                    tunnel = new Rectangle(-4000, -7400,500,500);

                    break;
                case 1:
                    //set player location and camera position
                    //curleveltexture set LoadNextLevel
                    //set bounding box for level (?)
                    //set enemies, will eventually use file reading
                    //set box that will take the player to the next level


                    tunnel = new Rectangle(3640, 2697,500,500);

                    break;
                case 2:
                    //set player location
                    //set bounding box for level (?)
                    //set enemies, will eventually use file reading
                    //set tunnel, rectangle to transfer level on collide.
                    tunnel = new Rectangle(0,0,0,0);
                    enemies.Clear();
                    break;
                case 3:
                    //set player location
                    //set bounding box for level (?)
                    //set enemies, will eventually use file reading
                    //set tunnel, rectangle to transfer level on collide.
                    cam.Position -= cam.Position;

                    break;
            }
            levelgenreq = false;
        }

    }
}
