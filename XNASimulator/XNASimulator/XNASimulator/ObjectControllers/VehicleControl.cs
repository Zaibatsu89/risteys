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

        public void Update(GameTime gameTime)
        {
            foreach (Vehicle vehicle in lists.Vehicles)
            {
                if (!vehicle.ID.Equals(string.Empty))
                {
                    this.CheckAlive(vehicle);
                    this.CheckNextTile(vehicle);

                    if (!vehicle.stopRedLight && !vehicle.stopCar)
                    {
                        switch (vehicle.rotation)
                        {
                            case RotationEnum.North:
                                vehicle.position -= new Vector2(0, vehicle.speed);
                                vehicle.drawposition -= new Vector2(0, vehicle.speed);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
                                break;
                            case RotationEnum.East:
                                vehicle.position += new Vector2(vehicle.speed, 0);
                                vehicle.drawposition += new Vector2(vehicle.speed, 0);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
                                break;
                            case RotationEnum.West:
                                vehicle.position -= new Vector2(vehicle.speed, 0);
                                vehicle.drawposition -= new Vector2(vehicle.speed, 0);
                                vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
                                break;
                            case RotationEnum.South:
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

            //occupy tile
            vehicle.spawntile = tile;
            vehicle.occupyingtile = tile.GridCoordinates;
            tile.isOccupied = true;
            tile.OccupiedID = vehicle.ID;
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

                    //Reset the vehicle for future use
                    lists.Vehicles[vehicle.ID[1]] = new Vehicle(string.Empty);
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

        private void CheckNextTile(Vehicle vehicle)
        {
            Tile nextTile;
            Tile currentTile = lists.Tiles[(int)vehicle.occupyingtile.X,(int)vehicle.occupyingtile.Y];

            switch (vehicle.rotation)
            {
                case RotationEnum.North:
                    //check if there is a tile north of the one the vehicle is occupying
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.North.ToString()))
                    {
                        nextTile = currentTile.adjacentTiles[RotationEnum.North.ToString()];
                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
                            CheckTileOccupation(vehicle, nextTile);
                        }
                        else
                        {
                            CheckTileOccupation(vehicle, currentTile);
                        }
                    }
                    break;
                case RotationEnum.East:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.East.ToString()))
                    {
                        nextTile = currentTile.adjacentTiles[RotationEnum.East.ToString()];
                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
                            CheckTileOccupation(vehicle, nextTile);
                        }
                        else
                        {
                            CheckTileOccupation(vehicle, currentTile);
                        }
                    }
                    break;
                case RotationEnum.South:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.South.ToString()))
                    {
                        nextTile = currentTile.adjacentTiles[RotationEnum.South.ToString()];
                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
                            CheckTileOccupation(vehicle, nextTile);
                        }
                        else
                        {
                            CheckTileOccupation(vehicle, currentTile);
                        }
                    }
                    break;
                case RotationEnum.West:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.West.ToString()))
                    {
                        nextTile = currentTile.adjacentTiles[RotationEnum.West.ToString()];
                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
                            CheckTileOccupation(vehicle, nextTile);
                        }
                        else
                        {
                            CheckTileOccupation(vehicle, currentTile);
                        }
                    }
                    break;
            }
        }

        private void CheckTileOccupation(Vehicle vehicle, Tile tile)
        {
            //check if occupied
            if (tile.isOccupied)
            {
                //is it occupied by this vehicle?
                if (tile.GridCoordinates.Equals(vehicle.occupyingtile))
                {
                    //yes, so go..
                    vehicle.stopCar = false;
                }
                else
                {
                    //no, so wait..
                    vehicle.stopCar = true;
                }
            }                    
            else //not occupied
            {
                vehicle.stopCar = false;

                //claim it 
                tile.isOccupied = true;
                tile.OccupiedID = vehicle.ID;

                //release previous tile
                lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].isOccupied = false;
                lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].OccupiedID = string.Empty;

                //set the new tile
                vehicle.occupyingtile = tile.GridCoordinates;              
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
        

        public void Spawn(string from)
        {
            foreach (Lane lane in lists.Lanes)
            {
                if(lane.laneID.Equals(from))
                {
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