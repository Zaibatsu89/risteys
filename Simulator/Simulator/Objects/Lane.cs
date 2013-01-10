using System;
using System.Collections.Generic;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Objects.TrafficObjects;

namespace KruispuntGroep6.Simulator.Objects
{
    public class Lane
    {
        public static int LaneLength = 7;

        public List<Tile> laneTiles { get; set; }
        public List<TrafficObject> laneVehicles { get; set; }
		public Queue<TrafficObject> vehicleQueue { get; set; }

        public string laneID { get; private set; }
        public Tile trafficLight { get; set; }
        public Tile detectionFar { get; set; }
        public Tile detectionClose { get; set; }
        public Tile sidewalkCrossing { get; set; }
        public Tile spawnTile { get; set; }

        private Random random;

        public Lane(string ID)
        {
            this.laneID = ID;
            this.laneTiles = new List<Tile>();
			this.laneVehicles = new List<TrafficObject>();
			this.vehicleQueue = new Queue<TrafficObject>();
            this.random = new Random();
        }

        public TrafficObject SpawnVehicle(TrafficObject vehicle)
        {
			switch (vehicle.ToString())
			{
				case "bicycle":
					vehicle = new Bicycle(Textures.Bicycle, vehicle.ID, random);
					break;
				case "bus":
					vehicle = new Bus(Textures.Bus, vehicle.ID, random);
					break;
				case "car":
					vehicle = new Car(Textures.Car, vehicle.ID, random);
					break;
				case "godzilla":
					vehicle = new Godzilla(Textures.Godzilla, vehicle.ID, random);
					break;
				case "pedestrian":
					vehicle = new Pedestrian(Textures.Pedestrian, vehicle.ID, random);
					break;
				case "truck":
					vehicle = new Truck(Textures.Truck, vehicle.ID, random);
					break;
			}

			vehicle.rotation = this.spawnTile.Rotation;
			vehicle.position = this.spawnTile.Position;
			vehicle.drawposition = this.spawnTile.DrawPosition;

			//occupy tile
			vehicle.spawntile = this.spawnTile;
			vehicle.occupyingtile = this.spawnTile.GridCoordinates;
			this.spawnTile.isOccupied = true;
			this.spawnTile.OccupiedID = vehicle.ID;

			vehicle.currentLane = this;
			this.laneVehicles.Add(vehicle);

			return vehicle;
        }

        public string CreateDetectionPackage()
        {
            string light = this.laneID;
            //string type;
            return light;
        }
    }
}