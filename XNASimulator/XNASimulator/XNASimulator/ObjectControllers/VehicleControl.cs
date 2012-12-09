using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.ObjectControllers
{
    class VehicleControl
    {
        private Lists lists;
        private GraphicsDevice graphics;
		private Random random;

        public VehicleControl(GraphicsDevice graphics, Lists lists)
        {
            this.lists = lists;
            this.graphics = graphics;

			random = new Random();
        }

        public void LoadVehicles()
        {
            string vehicleID = "";
            int i = 0;

            foreach (Lane lane in lists.Lanes)
            {
                if (lane.spawnTile != null)
                {
                    i += 1;
                    vehicleID = "V" + i;
                    Vehicle vehicle = LoadVehicle(lane.spawnTile, vehicleID);
                    lists.Vehicles.Add(vehicle);
                    lane.laneVehicles.Add(vehicle);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Vehicle vehicle in lists.Vehicles)
            {
                this.CheckAlive(vehicle);
                this.CheckCollission(vehicle);

                if (!vehicle.stopRedLight && !vehicle.stopCar)
                {
                    if (vehicle.rotation == RotationEnum.Right)
                    {
						vehicle.position += new Vector2(vehicle.speed, 0);
                        vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width / 2, vehicle.sprite.Height / 2);
                    }
                    else if (vehicle.rotation == RotationEnum.Down)
                    {
						vehicle.position += new Vector2(0, vehicle.speed);
                        vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width / 2, vehicle.sprite.Height / 2);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Vehicle vehicle in lists.Vehicles)
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
            Vehicle vehicle = new Vehicle(Textures.Car, vehicleID, random);
            vehicle.rotation = tile.Rotation;
            vehicle.position = tile.Position + tile.Origin;
            vehicle.spawntile = tile;
            return vehicle;
        }

        private void CheckAlive(Vehicle vehicle)
        {
            if (vehicle.alive)
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
            foreach (Tile tile in lists.Tiles)
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

                    if (tile.Texture.Equals(Textures.RedLight))
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