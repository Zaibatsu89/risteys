using System;
using System.Collections.Generic;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Main;
using KruispuntGroep6.Simulator.Objects;
using KruispuntGroep6.Simulator.Objects.TrafficObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.ObjectControllers
{
    public class VehicleControl
    {
        private Lists lists;
        private GraphicsDevice graphics;
		private List<Tuple<string, List<Vector2>>> paths;
		private Random random;

        public VehicleControl(GraphicsDevice graphics, Lists lists)
        {
            this.lists = lists;
            this.graphics = graphics;

			paths = new List<Tuple<string, List<Vector2>>>();
			random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            foreach (TrafficObject vehicle in lists.Vehicles)
            {
				if (!vehicle.ToString().Equals("default"))
				{
					this.CheckAlive(vehicle);
					this.CheckNextTile(vehicle);
					this.Pathfinding(vehicle);

					if (!vehicle.stopRedLight && !vehicle.stopCar)
					{
						switch (vehicle.rotation)
						{
							case RotationEnum.North:
								vehicle.position -= new Vector2(0, vehicle.speed);
								vehicle.drawposition -= new Vector2(0, vehicle.speed);
								break;
							case RotationEnum.East:
								vehicle.position += new Vector2(vehicle.speed, 0);
								vehicle.drawposition += new Vector2(vehicle.speed, 0);
								break;
							case RotationEnum.West:
								vehicle.position -= new Vector2(vehicle.speed, 0);
								vehicle.drawposition -= new Vector2(vehicle.speed, 0);
								break;
							case RotationEnum.South:
								vehicle.position += new Vector2(0, vehicle.speed);
								vehicle.drawposition += new Vector2(0, vehicle.speed);
								break;
						}

						vehicle.collission = new Rectangle((int)vehicle.position.X, (int)vehicle.position.Y, vehicle.sprite.Width, vehicle.sprite.Height);
					}
				}
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (TrafficObject vehicle in lists.Vehicles)
            {
				if (vehicle.alive)
				{
					spriteBatch.Draw
					(
						vehicle.sprite,
						vehicle.drawposition,
						null,
						Color.White,
						Rotation.getRotation(vehicle.rotation),
						vehicle.origin,
						1.0f,
						SpriteEffects.None,
						0.0f
					);
				}
            }
        }

        private void CheckAlive(TrafficObject vehicle)
        {
            if (vehicle.alive)
            {
                if (!graphics.PresentationParameters.Bounds.Contains(new Point((int)vehicle.position.X, (int)vehicle.position.Y)))
                {
                    vehicle.alive = false;

                    //Free up the last tile it was on
                    lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].OccupiedID = string.Empty;
                    lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].isOccupied = false;

					//Reset the vehicle for future use
					lists.Vehicles[int.Parse(vehicle.ID.Substring(1, vehicle.ID.Length - 1))] = new Default(string.Empty);
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

        private void CheckNextTile(TrafficObject vehicle)
        {
            Tile nextTile;
			Tile currentTile = lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y];

            switch (vehicle.rotation)
            {
                case RotationEnum.North:
                    //check if there is a tile north of the one the vehicle is occupying
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.North.ToString()))
                    {
						nextTile = currentTile.adjacentTiles[RotationEnum.North.ToString()];
                        
                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
							switch (vehicle.ToString())
							{
								case "bus":
									if (nextTile.adjacentTiles.ContainsKey(RotationEnum.North.ToString()))
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.North.ToString()];
									}
									break;
								case "car":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.North.ToString()) &&
										!nextTile.Texture.Equals(Textures.Car) &&
										nextTile.Texture.Equals(Textures.Truck)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.North.ToString()];
									}
									break;
								case "truck":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.North.ToString()) &&
										(
											nextTile.adjacentTiles[RotationEnum.North.ToString()].Texture.Equals(Textures.Car) ||
											nextTile.adjacentTiles[RotationEnum.North.ToString()].Texture.Equals(Textures.Truck)
										)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.North.ToString()];
									}
									break;
							}

							CheckTileOccupation(vehicle, nextTile, currentTile.adjacentTiles[RotationEnum.North.ToString()].GridCoordinates);
                        }
                        else
                        {
							CheckTileOccupation(vehicle, currentTile, currentTile.GridCoordinates);
                        }
                    }
                    break;
                case RotationEnum.East:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.East.ToString()))
                    {
						nextTile = currentTile.adjacentTiles[RotationEnum.East.ToString()];

                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
							switch (vehicle.ToString())
							{
								case "bus":
									if (nextTile.adjacentTiles.ContainsKey(RotationEnum.East.ToString()))
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.East.ToString()];
									}
									break;
								case "car":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.East.ToString()) &&
										!nextTile.Texture.Equals(Textures.Car) &&
										nextTile.Texture.Equals(Textures.Truck)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.East.ToString()];
									}
									break;
								case "truck":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.East.ToString()) &&
										(
											nextTile.adjacentTiles[RotationEnum.East.ToString()].Texture.Equals(Textures.Car) ||
											nextTile.adjacentTiles[RotationEnum.East.ToString()].Texture.Equals(Textures.Truck)
										)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.East.ToString()];
									}
									break;
							}

							CheckTileOccupation(vehicle, nextTile, currentTile.adjacentTiles[RotationEnum.East.ToString()].GridCoordinates);
                        }
                        else
                        {
							CheckTileOccupation(vehicle, currentTile, currentTile.GridCoordinates);
                        }
                    }
                    break;
                case RotationEnum.South:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.South.ToString()))
                    {
						nextTile = currentTile.adjacentTiles[RotationEnum.South.ToString()];

                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
							switch (vehicle.ToString())
							{
								case "bus":
									if (nextTile.adjacentTiles.ContainsKey(RotationEnum.South.ToString()))
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.South.ToString()];
									}
									break;
								case "car":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.South.ToString()) &&
										!nextTile.Texture.Equals(Textures.Car) &&
										nextTile.Texture.Equals(Textures.Truck)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.South.ToString()];
									}
									break;
								case "truck":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.South.ToString()) &&
										(
											nextTile.adjacentTiles[RotationEnum.South.ToString()].Texture.Equals(Textures.Car) ||
											nextTile.adjacentTiles[RotationEnum.South.ToString()].Texture.Equals(Textures.Truck)
										)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.South.ToString()];
									}
									break;
							}

							CheckTileOccupation(vehicle, nextTile, currentTile.adjacentTiles[RotationEnum.South.ToString()].GridCoordinates);
                        }
                        else
                        {
							CheckTileOccupation(vehicle, currentTile, currentTile.GridCoordinates);
                        }
                    }
                    break;
                case RotationEnum.West:
                    if (currentTile.adjacentTiles.ContainsKey(RotationEnum.West.ToString()))
                    {
						nextTile = currentTile.adjacentTiles[RotationEnum.West.ToString()];

                        if (vehicle.collission.Intersects(nextTile.CollisionRectangle))
                        {
							switch (vehicle.ToString())
							{
								case "bus":
									if (nextTile.adjacentTiles.ContainsKey(RotationEnum.West.ToString()))
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.West.ToString()];
									}
									break;
								case "car":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.West.ToString()) &&
										!nextTile.Texture.Equals(Textures.Car) &&
										nextTile.Texture.Equals(Textures.Truck)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.West.ToString()];
									}
									break;
								case "truck":
									if
									(
										nextTile.adjacentTiles.ContainsKey(RotationEnum.West.ToString()) &&
										(
											nextTile.adjacentTiles[RotationEnum.West.ToString()].Texture.Equals(Textures.Car) ||
											nextTile.adjacentTiles[RotationEnum.West.ToString()].Texture.Equals(Textures.Truck)
										)
									)
									{
										nextTile = nextTile.adjacentTiles[RotationEnum.West.ToString()];
									}
									break;
							}

							CheckTileOccupation(vehicle, nextTile, currentTile.adjacentTiles[RotationEnum.West.ToString()].GridCoordinates);
                        }
                        else
                        {
                            CheckTileOccupation(vehicle, currentTile, currentTile.GridCoordinates);
                        }
                    }
                    break;
            }
        }

        private void CheckTileOccupation(TrafficObject vehicle, Tile tile, Vector2 gridCoordinates)
        {
            //check if occupied
            if (tile.isOccupied)
            {
                //is it occupied by this vehicle?
                if (tile.GridCoordinates.Equals(vehicle.occupyingtile))
                {
                    //yes, so go..
                    vehicle.stopCar = false;

                    if (tile.Equals(vehicle.currentLane.detectionClose))
                    {

                    }
                    else if (tile.Equals(vehicle.currentLane.detectionFar))
                    {

                    }
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

                //let the detection know
                if (tile.Equals(vehicle.currentLane.detectionClose))
                {

                }
                else if (tile.Equals(vehicle.currentLane.detectionFar))
                {

                }

                //set the new tile
				vehicle.occupyingtile = gridCoordinates;
            }

			if
			(
				tile.Texture.Equals(Textures.BlinkLight) ||
				tile.Texture.Equals(Textures.RedLight) ||
				(
					tile.Texture.Equals(Textures.Sidewalk2Green) &&
					!vehicle.ToString().Equals("pedestrian")
				) ||
				(
					tile.Texture.Equals(Textures.Sidewalk2Red) &&
					vehicle.ToString().Equals("pedestrian")
				)
			)
            {
                vehicle.stopRedLight = true;
            }
			else if (tile.Texture.Equals(Textures.SidewalkDownRed) || (tile.Texture.Equals(Textures.SidewalkRightRed)))
			{
				if (tile.Texture.Equals(Textures.SidewalkDownRed))
				{
					switch (vehicle.rotation)
					{
						case RotationEnum.East:
							if (vehicle.currentLane.laneID.Equals("N0") || (vehicle.currentLane.laneID.Equals("W7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.North:
							if (vehicle.currentLane.laneID.Equals("W0") || (vehicle.currentLane.laneID.Equals("S7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.South:
							if (vehicle.currentLane.laneID.Equals("E0") || (vehicle.currentLane.laneID.Equals("N7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.West:
							if (vehicle.currentLane.laneID.Equals("S0") || (vehicle.currentLane.laneID.Equals("E7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
					}
				}
				else
				{
					switch (vehicle.rotation)
					{
						case RotationEnum.East:
							if (vehicle.currentLane.laneID.Equals("W0") || (vehicle.currentLane.laneID.Equals("S7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.North:
							if (vehicle.currentLane.laneID.Equals("S0") || (vehicle.currentLane.laneID.Equals("E7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.South:
							if (vehicle.currentLane.laneID.Equals("N0") || (vehicle.currentLane.laneID.Equals("W7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
						case RotationEnum.West:
							if (vehicle.currentLane.laneID.Equals("E0") || (vehicle.currentLane.laneID.Equals("N7")))
							{
								vehicle.stopRedLight = false;
							}
							else
							{
								vehicle.stopRedLight = true;
							}
							break;
					}
				}
			}
			else
			{
				vehicle.stopRedLight = false;
			}
        }

        private void Pathfinding(TrafficObject vehicle)
        {
			if (!vehicle.destinationLaneID.Equals(string.Empty))
			{
				switch (vehicle.pathfindingStep)
				{
					case 0:
						// Is the vehicle going to cross?
						if (!vehicle.ToString().Equals("pedestrian"))
						{
							vehicle.pathfindingStep = 2;
						}

						switch (vehicle.destinationLaneID)
						{
							case "N6":
								if (vehicle.currentLane.laneID[0].Equals('S'))
								{
									// Vehicle is going to cross
									vehicle.pathfindingStep = 1;
								}
								break;
							case "E6":
								if (vehicle.currentLane.laneID[0].Equals('W'))
								{
									vehicle.pathfindingStep = 1;
								}
								break;
							case "S6":
								if (vehicle.currentLane.laneID[0].Equals('N'))
								{
									vehicle.pathfindingStep = 1;
								}
								break;
							case "W6":
								if (vehicle.currentLane.laneID[0].Equals('E'))
								{
									vehicle.pathfindingStep = 1;
								}
								break;
						}

						break;
					case 1:
						// Vehicle is going to cross
						switch (vehicle.rotation)
						{
							case RotationEnum.East:
								if (vehicle.occupyingtile.X > 9)
								{
									// Vehicle is in the center of the intersection, so turn
									vehicle.rotation = RotationEnum.South;
								}
								break;
							case RotationEnum.North:
								if (vehicle.occupyingtile.Y < 10)
								{
									vehicle.rotation = RotationEnum.East;
								}
								break;
							case RotationEnum.South:
								if (vehicle.occupyingtile.Y > 9)
								{
									vehicle.rotation = RotationEnum.West;
								}
								break;
							case RotationEnum.West:
								if (vehicle.occupyingtile.X < 10)
								{
									vehicle.rotation = RotationEnum.North;
								}
								break;
						}

						vehicle.pathfindingStep = 2;

						break;
					case 2:
						// Get destination tile position from destinationLaneID
						Vector2 destination = new Vector2();

						switch (vehicle.destinationLaneID[0])
						{
							case 'E':
								destination = new Vector2(20, 12);
								break;
							case 'N':
								destination = new Vector2(12, 0);
								break;
							case 'S':
								destination = new Vector2(7, 20);
								break;
							case 'W':
								destination = new Vector2(0, 7);
								break;
						}

						// Pathfinding to destination direction
						switch (vehicle.rotation)
						{
							case RotationEnum.East:
								// Is the vehicle parallel with his destination?
								if (vehicle.occupyingtile.X > destination.X)
								{
									// Is the vehicle going to north or to south?
									if (vehicle.destinationLaneID[0].Equals('N'))
									{
										// Turn to north
										vehicle.rotation = RotationEnum.North;
									}
									else
									{
										// Turn to south
										vehicle.rotation = RotationEnum.South;
									}
								}
								break;
							case RotationEnum.North:
								if (vehicle.occupyingtile.Y < destination.Y)
								{
									if (vehicle.destinationLaneID[0].Equals('W'))
									{
										vehicle.rotation = RotationEnum.West;
									}
									else
									{
										vehicle.rotation = RotationEnum.East;
									}
								}
								break;
							case RotationEnum.South:
								if (vehicle.occupyingtile.Y > destination.Y)
								{
									if (vehicle.destinationLaneID[0].Equals('E'))
									{
										vehicle.rotation = RotationEnum.East;
									}
									else
									{
										vehicle.rotation = RotationEnum.West;
									}
								}
								break;
							case RotationEnum.West:
								if (vehicle.occupyingtile.X < destination.X)
								{
									if (vehicle.destinationLaneID[0].Equals('S'))
									{
										vehicle.rotation = RotationEnum.South;
									}
									else
									{
										vehicle.rotation = RotationEnum.North;
									}
								}
								break;
						}

						break;
				}
			}
        }

		public void Spawn(string type, string from, string to)
		{
			foreach (Lane lane in lists.Lanes)
			{
				if (lane.laneID.Equals(from))
				{
					for (int i = 0; i < lists.Vehicles.Length; i++)
					{
						TrafficObject newVehicle = lists.Vehicles[i];
						string vehicleType = string.Empty;

						if (newVehicle.ID.Equals(string.Empty))
						{
							switch (type)
							{
								case "bicycle":
									newVehicle = new Bicycle(string.Empty);
									vehicleType = "b";
									break;
								case "bus":
									newVehicle = new Bus(string.Empty);
									vehicleType = "B";
									break;
								case "car":
									newVehicle = new Car(string.Empty);
									vehicleType = "c";
									break;
								case "godzilla":
									newVehicle = new Godzilla(string.Empty);
									vehicleType = "g";
									break;
								case "pedestrian":
									newVehicle = new Pedestrian(string.Empty);
									vehicleType = "p";
									break;
								case "truck":
									newVehicle = new Truck(string.Empty);
									vehicleType = "t";
									break;
								default:
									newVehicle = new Default(string.Empty);
									vehicleType = "?";
									break;
							}

							if (!lane.spawnTile.isOccupied)
							{
								newVehicle.ID = vehicleType + i;
								newVehicle.destinationLaneID = to;
								lists.Vehicles[i] = lane.SpawnVehicle(newVehicle);

								// Super important break, because without this break, the vehicleQueue gets full
								break;
							}
							else
							{
								lane.vehicleQueue.Enqueue(newVehicle);
							}
						}
					}
				}
			}
		}
	}
}