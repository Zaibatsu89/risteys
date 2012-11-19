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

namespace XNASimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int tilesHor = 9;
        private int tilesVer = 9;

        private Crossroad crossroad;
        private Audio audio;

        #region testcars
        /*
        private float minCarVelocity = 1.0f;
        private float maxCarVelocity = 2.0f;
        private float minCarDistance = 250.0f;
        private float maxCarDistance = 600.0f;
        Vehicle[] vehicles;
        Random random = new Random();
        */
        #endregion

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
            crossroad = new Crossroad(Services);
            audio = new Audio(Services);

            #region testcars
            /*
            vehicles = new Vehicle[6];
            for (int i = 0; i < 6; i++)
            {
                vehicles[i] = new Vehicle(Content.Load<Texture2D>("Sprites/RedCar64x64LR"), 0.0f);
            }
            */
            #endregion

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

            LoadCrossroad("Content/Grids/TestGrid2.txt");
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

            //UpdateCars();

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
            #region testcars
            /* foreach (Vehicle car in vehicles)
            {
                if (car.alive)
                {
                    spriteBatch.Draw(car.sprite, car.position,null, Color.White, car.rotation, car.center, 1.0f, SpriteEffects.None, 0f);
                }
            } */
            #endregion
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadCrossroad(string path)
        {
            if (File.Exists(path))
                crossroad.LoadLevel(path);
            else throw new Exception("No Level Detected");
        }

        /* UpdateCars
        private void UpdateCars()
        {
            foreach (Vehicle car in vehicles)
            {
                if (car.alive == true)
                {
                    car.position += car.velocity;
                    if (!GraphicsDevice.PresentationParameters.Bounds.Contains(new Point((int)car.position.X,
                                                        (int)car.position.Y)))
                    {
                        car.alive = false;
                    }
                }
                else
                {
                    car.alive = true;
                    car.position = new Vector2(64.0f, MathHelper.Lerp(minCarDistance,
                                                               maxCarDistance,
                                                               (float)random.NextDouble()));
                    car.velocity = new Vector2(MathHelper.Lerp(minCarVelocity,
                                                               maxCarVelocity,
                                                               (float)random.NextDouble()),
                                                               0);
                }
            }
        } */
    }
}
