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
            string vehicleID = "";
            int i = 0;

            foreach (Tile tile in crossroad.tiles)
            {
				if (tile.Texture.Equals(spawnTexture))
				{
					i += 1;
					vehicleID = "V" + i;
					vehicles.Add(LoadVehicle(tile, vehicleID));
				}
            }
        }

        public void UpdateVehicles()
        {
            foreach (Tile tile in crossroad.tiles)
            {
                tile.UpdateOccupied(vehicles);
            }

            foreach (Vehicle vehicle in vehicles)
            {
                this.CheckAlive(vehicle);
                this.CheckCollission(vehicle);

                if (!vehicle.stopRedLight && !vehicle.stopCar)
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
                if (vehicle.alive)
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

        private Vehicle LoadVehicle(Tile tile, string vehicleID)
        {
            Vehicle vehicle = new Vehicle(content.Load<Texture2D>("Sprites/RedCar64x64DU"), vehicleID);
            vehicle.rotation = tile.Rotation;
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
                    if (tile.isOccupied && !string.Equals(tile.OccupiedID, vehicle.ID))
                    {
                        vehicle.stopCar = true;
                    }
                    else
                    {
                        vehicle.stopCar = false;
                    }

                    if (tile.Texture.Equals(redLight))
                    {
                        vehicle.stopRedLight = true;
                    }
                    else
                    {
                        vehicle.stopRedLight = false;
                    }
                }
            }
        }

		public static void Spawn(string from)
		{
			//TODO: spawn car

			switch (from[0])
			{
				case 'N':
					// spawn car at north, facing south

					break;
				case 'E':
					// spawn car at east, facing west

					break;
				case 'S':
					// spawn car at south, facing north

					break;
				case 'W':
					// spawn car at west, facing east

					break;
			}
		}

		public static void Drive(string to)
		{
			//TODO: drive car

			switch (to[0])
			{
				case 'N':
					// drive car to north

					break;
				case 'E':
					// drive car to east

					break;
				case 'S':
					// drive car to south

					break;
				case 'W':
					// drive car to west

					break;
			}
		}
	}
}
