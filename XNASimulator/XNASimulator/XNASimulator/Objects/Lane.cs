using System.Collections.Generic;

namespace KruispuntGroep6.Simulator.Objects
{
    class Lane
    {
        public static int LaneLength = 6;

        public List<Tile> laneTiles { get; set; }
        public List<Vehicle> laneVehicles { get; set; }

        public string laneID { get; private set; }
        public Tile trafficLight { get; set; }
        public Tile detectionFar { get; set; }
        public Tile detectionClose { get; set; }
        public Tile sidewalkCrossing { get; set; }
        public Tile spawnTile { get; set; }

        public Lane(string ID)
        {
            this.laneID = ID;
            this.laneTiles = new List<Tile>();
            this.laneVehicles = new List<Vehicle>();
        }
    }
}