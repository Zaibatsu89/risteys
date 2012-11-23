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
        private ContentManager content; 
        private Crossroad crossroad;
        private List<Vehicle> vehicles;

        public VehicleControl(Crossroad crossroad)
        {
            this.crossroad = crossroad;
            this.vehicles = new List<Vehicle>();
            this.content = crossroad.Content;
        }

        public void LoadVehicles()
        {
            Texture2D spawnTexture = content.Load<Texture2D>("Tiles/Spawn64x64");

            foreach (Tile tile in crossroad.tiles)
            {
                if (tile.Texture.Equals(spawnTexture))
                {
                    vehicles.Add(LoadVehicle(tile, tile.Rotation));
                }
            }
        }

        public void UpdateVehicles()
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.rotation == RotationEnum.Right)
                    vehicle.position += new Vector2(1, 0);
                else if (vehicle.rotation == RotationEnum.Down)
                    vehicle.position += new Vector2(0, 1);
            }
        }

        public void DrawVehicles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                spriteBatch.Draw(vehicle.sprite,
                                        vehicle.position,
                                        null,
                                        Color.White,
                                        Rotation.getRotation(vehicle.rotation),
                                        vehicle.origin,
                                        1.0f,
                                        SpriteEffects.None,
                                        0.0f);
            }
        }

        private Vehicle LoadVehicle(Tile tile, RotationEnum tilerotation)
        {
            Vehicle vehicle = new Vehicle(content.Load<Texture2D>("Sprites/RedCar64x64DU"), tilerotation);
            vehicle.position = tile.Position + tile.Origin;
            return vehicle;
        }
    }
}
