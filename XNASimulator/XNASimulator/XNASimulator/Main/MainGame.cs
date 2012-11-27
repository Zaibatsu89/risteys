using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNASimulator.Main;

namespace XNASimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
		private Communication communication;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int tilesHor = 11;
        private int tilesVer = 11;

        private Crossroad crossroad;
        private VehicleControl vehicleControl;
        private Audio audio;

        private MouseState mouseStateCurrent;
        private MouseState mouseStatePrevious;
        private Vector2 mousePosition;

        public MainGame()
        {
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
			communication = new Communication();

            crossroad = new Crossroad(Services);
            vehicleControl = new VehicleControl(this.GraphicsDevice, crossroad);

            audio = new Audio(Services);

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

            this.LoadCrossroad("Content/Grids/TestGrid2.txt");
            vehicleControl.LoadVehicles();

            audio.PlayBackgroundMusic();

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
            mouseStateCurrent = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton != ButtonState.Pressed)
            {
                mousePosition = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                crossroad.CheckMouseCollision(mousePosition);
            }

            mouseStatePrevious = mouseStateCurrent;
            vehicleControl.UpdateVehicles();

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
            crossroad.Draw(gameTime, spriteBatch);
            vehicleControl.DrawVehicles(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadCrossroad(string path)
        {
            if (File.Exists(path))
                crossroad.LoadLevel(path);
            else throw new Exception("No Level Detected");
        }
    }
}
