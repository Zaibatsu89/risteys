﻿using System;
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
        private GraphicsDevice graphics;

        private Texture2D redLight;

        public VehicleControl(GraphicsDevice graphics, Crossroad crossroad)
        {
            this.crossroad = crossroad;
            this.vehicles = new List<Vehicle>();
            this.content = crossroad.Content;
            this.graphics = graphics;

            redLight = content.Load<Texture2D>("Tiles/LightsRed64x64");
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
                this.CheckAlive(vehicle);
                this.CheckCollission(vehicle);

                if (!vehicle.stopped)
                {
                    if (vehicle.rotation == RotationEnum.Right)
                    {
                        vehicle.position += new Vector2(1, 0);
                        vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width / 2, vehicle.sprite.Height / 2);
                    }
                    else if (vehicle.rotation == RotationEnum.Down)
                    {
                        vehicle.position += new Vector2(0, 1);
                        vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width / 2, vehicle.sprite.Height / 2);
                    }
                }
            }
        }

        public void DrawVehicles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.alive == true)
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
        }

        private Vehicle LoadVehicle(Tile tile, RotationEnum tilerotation)
        {
            Vehicle vehicle = new Vehicle(content.Load<Texture2D>("Sprites/RedCar64x64DU"), tilerotation);
            vehicle.position = tile.Position + tile.Origin;
            vehicle.spawntile = tile;
            return vehicle;
        }

        private void CheckAlive(Vehicle vehicle)
        {
            if (vehicle.alive == true)
            {
                if (!graphics.PresentationParameters.Bounds.Contains(new Point((int)vehicle.position.X,
                                                    (int)vehicle.position.Y)))
                {
                    vehicle.alive = false;
                }
            }
            else
            {
                vehicle.alive = true;
                vehicle.position = vehicle.spawntile.Position + vehicle.spawntile.Origin;
            }
        }

        private void CheckCollission(Vehicle vehicle)
        {
            foreach (Tile tile in crossroad.tiles)
            {
                if (vehicle.collission.Intersects(tile.CollisionRectangle))
                {
                    if (tile.Texture.Equals(redLight))
                    {
                        vehicle.stopped = true;
                    }
                    else
                    {
                        vehicle.stopped = false;
                    }
                }
            }
        }
    }
}
