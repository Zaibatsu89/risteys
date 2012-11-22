using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNASimulator.Enums;

namespace XNASimulator.Main
{
    class VehicleControl
    {
        public ContentManager Content { get; private set; }
        private Crossroad crossroad;

        public VehicleControl(IServiceProvider serviceProvider, Crossroad crossroad)
        {
            this.crossroad = crossroad;
            Content = new ContentManager(serviceProvider, "Content");
        }

        public void SpawnVehicles()
        {
            Texture2D spawnTexture = Content.Load<Texture2D>("Tiles/Spawn64x64");

            foreach (Tile tile in crossroad.tiles)
            {
                if (tile.Texture.Equals(spawnTexture))
                {

                }
            }
        }

        private Vehicle SpawnVehicle(RotationEnum tilerotation)
        {
            Vehicle vehicle = new Vehicle(Content.Load<Texture2D>("Sprites/RedCar64x64DU"), 0.0f);
            return vehicle; 
        }

        public void UpdateVehicles()
        {

        }

        public void DrawVehicles()
        {

        }
    }
}
