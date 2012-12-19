using System;
using System.Collections.Generic;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Main;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.ObjectControllers
{
    public class VehicleControl
    {
        private Lists lists;
        private GraphicsDevice graphics;
		private Pathfinder pathfinder;
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
            foreach (Vehicle vehicle in lists.Vehicles)
            {
                if (!vehicle.ID.Equals(string.Empty))
                {
                    this.CheckAlive(vehicle);
                    this.CheckNextTile(vehicle);
                    this.TempPathfinding(vehicle);

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

        private void CheckAlive(Vehicle vehicle)
        {
            if (vehicle.alive)
            {
                if (!graphics.PresentationParameters.Bounds.Contains(new Point((int)vehicle.position.X,
                    (int)vehicle.position.Y)) )
                {
                    vehicle.alive = false;

                    //Free up the last tile it was on
                    lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].OccupiedID = string.Empty;
                    lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y].isOccupied = false;

                    //Reset the vehicle for future use
                    lists.Vehicles[int.Parse(vehicle.ID.Substring(1,vehicle.ID.Length - 1))] = new Vehicle(string.Empty);
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

        public void Spawn(string type, string from, string to)
        {
            foreach (Lane lane in lists.Lanes)
            {
                if(lane.laneID.Equals(from))
                {
                    for (int i = 0; i < lists.Vehicles.Length; i++)
                    {
                        lists.Vehicles[i].destinationLaneID = to;
                        Vehicle newVehicle = lists.Vehicles[i];
                        string vehicleType = string.Empty;

                        if (newVehicle.ID.Equals(string.Empty))
                        {

                            switch (type)
                            {
                                case "bike":
                                    vehicleType = "b";
                                    break;
                                case "bus":
                                    vehicleType = "B";
                                    break;
                                case "car":
                                    vehicleType = "c";
                                    break;
                                case "godzilla":
                                    vehicleType = "g";
                                    break;
                                case "truck":
                                    vehicleType = "t";
                                    break;
								default:
									vehicleType = "?";
									break;
                            }

                            if (!lane.spawnTile.isOccupied)
                            {
                                string vehicleID = vehicleType + i;
                                lists.Vehicles[i] = lane.SpawnVehicle(vehicleID);
                                break;

                            }
                            else
                            {
								if (lane.vehicleQueue.Count > 512)
								{
									lane.vehicleQueue.Dequeue();
								}

                                lane.vehicleQueue.Enqueue(newVehicle);
                            }
                        }
                    }
                }             
            }

			// Spawn car
			RotationEnum directionFrom = RotationEnum.North;
			
			switch (from[0])
			{
				case 'N':
					// spawn car from north
					directionFrom = RotationEnum.North;
					break;
				case 'E':
					// spawn car from east
					directionFrom = RotationEnum.East;
					break;
				case 'S':
					// spawn car from south
					directionFrom = RotationEnum.South;
					break;
				case 'W':
					// spawn car from west
					directionFrom = RotationEnum.West;
					break;
			}

			// Drive car
			RotationEnum directionTo = RotationEnum.North;

            switch (to[0])
            {
                case 'N':
                    // drive car to north
					directionTo = RotationEnum.North;
                    break;
                case 'E':
                    // drive car to east
					directionTo = RotationEnum.East;
                    break;
                case 'S':
                    // drive car to south
					directionTo = RotationEnum.South;
                    break;
                case 'W':
                    // drive car to west
					directionTo = RotationEnum.West;
                    break;
            }

			int intTo = int.Parse(to[1].ToString());

			Path(directionFrom, directionTo, intTo);
        }

		private void Path(RotationEnum from, RotationEnum to, int intTo)
		{
			for (int i = 0; i < lists.Vehicles.Length; i++)
			{
				Point pntDeparture = new Point();

				if (lists.Vehicles[i].spawntile != null)
				{
					if (lists.Vehicles[i].spawntile.GridCoordinates != null)
					{
						int departureX = (int)lists.Vehicles[i].spawntile.GridCoordinates.X;
						int departureY = (int)lists.Vehicles[i].spawntile.GridCoordinates.Y;

						switch (from)
						{
							case RotationEnum.North:
								// from north
								departureY += 7;
								break;
							case RotationEnum.East:
								// from east
								departureX -= 7;
								break;
							case RotationEnum.South:
								// from south
								departureY -= 7;
								break;
							case RotationEnum.West:
								// from west
								departureX += 7;
								break;
							default:
								throw new Exception(string.Format("Direction from {0} wordt niet herkend!", from));
						}

						if (departureX > -1 && departureY > -1)
						{
							pntDeparture = new Point(departureX, departureY);
						}
						else
						{
							// Pech gehad
						}

						Point pntArrival = new Point();

						int arrivalX = -1;
						int arrivalY = -1;

						foreach (Lane lane in lists.Lanes)
						{
							switch (to)
							{
								case RotationEnum.North:
									// to north
									if (lane.laneID.Equals("N6"))
									{
										arrivalX = (int)lane.spawnTile.GridCoordinates.X;
										arrivalY = (int)lane.spawnTile.GridCoordinates.Y;
									}
									break;
								case RotationEnum.East:
									// to east
									if (lane.laneID.Equals("E6"))
									{
										arrivalX = (int)lane.spawnTile.GridCoordinates.X;
										arrivalY = (int)lane.spawnTile.GridCoordinates.Y;
									}
									break;
								case RotationEnum.South:
									// to south
									if (lane.laneID.Equals("S6"))
									{
										arrivalX = (int)lane.spawnTile.GridCoordinates.X;
										arrivalY = (int)lane.spawnTile.GridCoordinates.Y;
									}
									break;
								case RotationEnum.West:
									// to west
									if (lane.laneID.Equals("W6"))
									{
										arrivalX = (int)lane.spawnTile.GridCoordinates.X;
										arrivalY = (int)lane.spawnTile.GridCoordinates.Y;
									}
									break;
								default:
									throw new Exception(string.Format("Direction to {0} wordt niet herkend!", to));
							}
						}

						if (arrivalX > -1 && arrivalY > -1)
						{
							pntArrival = new Point(arrivalX, arrivalY);
						}
						else
						{
							pntArrival = pntDeparture;
						}

						if (!pntArrival.Equals(pntDeparture))
						{
							Tuple<string, List<Vector2>> path = new Tuple<string, List<Vector2>>(lists.Vehicles[i].ID, new List<Vector2>());

							try
							{
								path = new Tuple<string, List<Vector2>>(lists.Vehicles[i].ID, pathfinder.FindPath(pntDeparture, pntArrival));
							}
							catch (IndexOutOfRangeException)
							{
								
							}

							bool alreadyInPaths = false;

							foreach (Tuple<string, List<Vector2>> path2 in paths)
							{
								if (path2.Item1.Equals(lists.Vehicles[i].ID))
								{
									alreadyInPaths = true;
								}
							}

							if (!alreadyInPaths)
							{
								paths.Add(path);
							}
						}
					}
				}
			}
		}

		public List<Tuple<string, List<Vector2>>> GetPaths()
		{
			return paths;
		}

		public void SetPathfinder(Pathfinder pathfinder)
		{
			this.pathfinder = pathfinder;
		}



        private void TempPathfinding(Vehicle vehicle)
        {
            Tile occupyingTile = lists.Tiles[(int)vehicle.occupyingtile.X, (int)vehicle.occupyingtile.Y];

            //origin
            switch (vehicle.spawntile.Rotation)
            {
                case RotationEnum.North:
                    //destination
                    switch (vehicle.destinationLaneID)
                    {
                        case "N6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.East;
                            break;
                        case "E6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.East;
                            break;
                        case "W6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.East;
                            break;
                    }
                    break;
                case RotationEnum.South:
                    switch (vehicle.destinationLaneID)
                    {
                        case "E6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.West;
                            break;
                        case "S6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.West;
                            break;
                        case "W6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.West;
                            break;
                    }
                    break;
                case RotationEnum.East:
                    switch (vehicle.destinationLaneID)
                    {
                        case "N6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.South;
                            break;
                        case "S6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.South;
                            break;
                        case "E6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.South;
                            break;
                    }
                    break;
                case RotationEnum.West:
                    switch (vehicle.destinationLaneID)
                    {
                        case "N6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.North;
                            break;
                        case "S6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.North;
                            break;
                        case "W6":
                            if (occupyingTile.Texture.Equals(Textures.Crossing))
                                vehicle.rotation = RotationEnum.North;
                            break;
                    }
                    break;
            }
        }
	}
}