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
                    //lists.Vehicles.Add(vehicle);
                    lane.laneVehicles.Add(vehicle);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Vehicle vehicle in lists.Vehicles)
            {
                if (!vehicle.ID.Equals(string.Empty))
                {
                    this.CheckAlive(vehicle);
                    this.CheckCollission(vehicle);

                    if (!vehicle.stopRedLight && !vehicle.stopCar)
                    {
                        switch (vehicle.rotation)
                        {
                            case RotationEnum.Up:
                                vehicle.position -= new Vector2(0, vehicle.speed);
                                vehicle.drawposition -= new Vector2(0, vehicle.speed);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height - 32);
                                break;
                            case RotationEnum.Right:
                                vehicle.position += new Vector2(vehicle.speed, 0);
                                vehicle.drawposition += new Vector2(vehicle.speed, 0);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
                                break;
                            case RotationEnum.Left:
                                vehicle.position -= new Vector2(vehicle.speed, 0);
                                vehicle.drawposition -= new Vector2(vehicle.speed, 0);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width - 32, vehicle.sprite.Height);
                                break;
                            case RotationEnum.Down:
                                vehicle.position += new Vector2(0, vehicle.speed);
                                vehicle.drawposition += new Vector2(0, vehicle.speed);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
                                break;
                        }
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
                                            vehicle.drawposition,
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
            vehicle.position = tile.Position;
            vehicle.drawposition = tile.DrawPosition;
            vehicle.spawntile = tile;
            return vehicle;
        }

        private void CheckAlive(Vehicle vehicle)
        {
            if (vehicle.alive)
            {
                if (!graphics.PresentationParameters.Bounds.Contains(new Point((int)vehicle.position.X,
                    (int)vehicle.position.Y)) )
                {
                    vehicle.alive = false;
                    //lists.Vehicles.Remove(vehicle);
                    lists.Vehicles[vehicle.ID[1]].ID = string.Empty;
                }
            }
            else
            {
                if (!vehicle.ID.Equals(string.Empty))
                    vehicle.alive = true;
                //vehicle.position = vehicle.spawntile.Position;
                //vehicle.drawposition = vehicle.spawntile.DrawPosition;
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

        public void Spawn(string from)
        {
            foreach (Lane lane in lists.Lanes)
            {
                if(lane.laneID.Equals(from))
                {
                    //this.vehicleIDCounter += 1;
                    //Vehicle vehicle = LoadVehicle(lane.spawnTile, "V" + vehicleIDCounter);
                    //lists.Vehicles.Add(vehicle);
                    for (int i = 0; i < lists.Vehicles.Length; i++)
                    {
                        if (!lane.spawnTile.isOccupied)
                        {
                            Vehicle newVehicle = lists.Vehicles[i];
                            if (newVehicle.ID.Equals(string.Empty))
                            {
                                newVehicle = LoadVehicle(lane.spawnTile, "V" + i);
                                lists.Vehicles[i] = newVehicle;
                                break;
                            }
                        }
                        else
                        {
                            //put vehicle in queue
                        }
                    }
                    //lane.laneVehicles.Add(vehicle);
                }             
            }
        }

        public void Drive(string to)
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