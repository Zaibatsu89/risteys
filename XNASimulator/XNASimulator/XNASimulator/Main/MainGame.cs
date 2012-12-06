using System;
using System.IO;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.ObjectControllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KruispuntGroep6.Simulator.Main
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
		private string address;

		private KruispuntGroep6.Simulator.Events.Communication communication;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private int tilesHor = 11;
        private int tilesVer = 11;

        private LevelBuilder levelBuilder;
        private Audio audio;
        private Lists lists;

		private VehicleControl vehicleControl;
        private TileControl tileControl;
        private LaneControl laneControl;

        private MouseState mouseStateCurrent;
        private MouseState mouseStatePrevious;
        private Vector2 mousePosition;

        #region tileloading

        #endregion

        public MainGame(string address)
        {
			this.address = address;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = this.tilesHor * 64;
            graphics.PreferredBackBufferWidth = this.tilesVer * 64;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
			communication = new KruispuntGroep6.Simulator.Events.Communication(address);

            lists = new Lists();

            levelBuilder = new LevelBuilder(lists);

			vehicleControl = new VehicleControl(this.GraphicsDevice, lists);
            tileControl = new TileControl(lists);
            laneControl = new LaneControl(lists);

            //audio = new Audio(Services);

            this.IsMouseVisible = true;

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

            Textures.RedLight = Content.Load<Texture2D>("Tiles/LightsRed64x64");
            Textures.GreenLight = Content.Load<Texture2D>("Tiles/LightsGreen64x64");
            Textures.Crossing = Content.Load<Texture2D>("Tiles/Crossing64x64");
            Textures.Grass = Content.Load<Texture2D>("Tiles/Grass64x64");
            Textures.RedCar = Content.Load<Texture2D>("Sprites/RedCar64x64DU");
            Textures.Road = Content.Load<Texture2D>("Tiles/Road64x64");
            Textures.SideWalk = Content.Load<Texture2D>("Tiles/Sidewalk64x64");
            Textures.Spawn = Content.Load<Texture2D>("Tiles/Spawn64x64");

            this.LoadCrossroad("Content/Grids/TestGrid2.txt");
            vehicleControl.LoadVehicles();

            //audio.PlayBackgroundMusic();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            MouseButtonPress();

            tileControl.Update(gameTime);
            vehicleControl.Update(gameTime);
            laneControl.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            tileControl.Draw(gameTime, spriteBatch);
            vehicleControl.Draw(gameTime, spriteBatch);
            laneControl.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadCrossroad(string path)
        {
            if (File.Exists(path))
                levelBuilder.LoadLevel(path);
            else throw new Exception("No Level Detected");
        }

        private void MouseButtonPress()
        {
            mouseStateCurrent = Mouse.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton != ButtonState.Pressed)
            {
                mousePosition = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                tileControl.CheckMouseCollision(mousePosition);
            }

            mouseStatePrevious = mouseStateCurrent;
        }
    }
}