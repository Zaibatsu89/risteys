using System;
using System.IO;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.ObjectControllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace KruispuntGroep6.Simulator.Main
{
    //TODO: bugs: 
    //Ghost cars that don't move appear after reset or after stress

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
		private string address;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int TilesHor = 20;
        public static int TilesVer = 20;

        private LevelBuilder levelBuilder;
        private Lists lists;

		public Events.Communication Communication;

		private PathfinderLoader pathfinderLoader;
		private Pathfinder pathfinder;

		private VehicleControl vehicleControl;
        private TileControl tileControl;
        private LaneControl laneControl;

        private MouseState mouseStateCurrent;
        private MouseState mouseStatePrevious;
        private Vector2 mousePosition;

		private static void Main(string[] args)
		{
			using (MainGame game = new MainGame("lol"))
			{
				game.Run();
			}
		}

        public MainGame(string address)
        {
			this.address = address;
			this.Window.Title = "Kruispunt Groep 6: Simulator";

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				"SimulatorContent"));

            graphics.PreferredBackBufferHeight = TilesHor * 32;
            graphics.PreferredBackBufferWidth = TilesVer * 32;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Textures.Default = Content.Load<Texture2D>("Sprites\\Default32x32");

            lists = new Lists();

            levelBuilder = new LevelBuilder(lists);
			pathfinderLoader = new PathfinderLoader(lists);

			vehicleControl = new VehicleControl(this.GraphicsDevice, lists);
			tileControl = new TileControl(lists);
			laneControl = new LaneControl(lists);

			Communication = new Events.Communication(tileControl, vehicleControl);

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

            #region Textures
            Textures.RedLight = Content.Load<Texture2D>("Tiles/32p/RedLight32x32");
            Textures.GreenLight = Content.Load<Texture2D>("Tiles/32p/GreenLight32x32");
            Textures.YellowLight = Content.Load<Texture2D>("Tiles/32p/YellowLight32x32");
            Textures.BlinkLight = Content.Load<Texture2D>("Tiles/32p/BlinkLight32x32");

            Textures.Sidewalk2Red = Content.Load<Texture2D>("Tiles/32p/Sidewalk2Red32x32");
            Textures.Sidewalk2Green = Content.Load<Texture2D>("Tiles/32p/Sidewalk2Green32x32");
            Textures.SidewalkRightRed = Content.Load<Texture2D>("Tiles/32p/SidewalkRightRed32x32");
            Textures.SidewalkDownRed = Content.Load<Texture2D>("Tiles/32p/SidewalkDownRed32x32");

            Textures.Bikelane = Content.Load<Texture2D>("Tiles/32p/SortBike32x32");
            Textures.Buslane = Content.Load<Texture2D>("Tiles/32p/SortBus32x32");
            Textures.CarSortDown = Content.Load<Texture2D>("Tiles/32p/SortDown32x32");
            Textures.CarSortLeft = Content.Load<Texture2D>("Tiles/32p/SortLeft32x32");
            Textures.CarSortRight = Content.Load<Texture2D>("Tiles/32p/SortRight32x32");

            Textures.Road = Content.Load<Texture2D>("Tiles/32p/Road32x32");
            Textures.RoadCenter = Content.Load<Texture2D>("Tiles/32p/RoadCenter32x32");

            Textures.Crossing = Content.Load<Texture2D>("Tiles/32p/Crossing32x32");
            Textures.Grass = Content.Load<Texture2D>("Tiles/32p/Grass32x32");
            Textures.Sidewalk = Content.Load<Texture2D>("Tiles/32p/Sidewalk32x32");
            #endregion

            Textures.Car = Content.Load<Texture2D>("Sprites\\Car32x32");

			this.LoadCrossroad(Path.Combine(Content.RootDirectory,
				"Grids\\Crossroad.txt"));
			this.LoadPathfinding(Path.Combine(Content.RootDirectory,
				"Grids\\Pathfinding.txt"));

            tileControl.FillTileList();
            laneControl.LoadLanes();
			vehicleControl.SetPathfinder(pathfinder);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
		protected override void UnloadContent() { }

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

            //tileControl.Update(gameTime);
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

		private void LoadPathfinding(string filePath)
		{
			if (File.Exists(filePath))
				pathfinderLoader.LoadLevel(filePath);
			else throw new Exception("No Level Detected");

			Map map = new Map(pathfinderLoader.GetLayout());
			pathfinder = new Pathfinder(map);
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