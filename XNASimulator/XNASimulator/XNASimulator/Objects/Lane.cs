using System.Collections.Generic;

namespace KruispuntGroep6.Simulator.Objects
{
    class Lane
    {
        public Tile[] laneTiles { get; set; }
        public List<Vehicle> laneVehicles { get; set; }

        public string laneID { get; private set; }
        public Tile trafficLight { get; private set; }
        public Tile detectionFar { get; private set; }
        public Tile detectionClose { get; private set; }
        public Tile sidewalkCrossing { get; private set; }

        public Lane(string ID)
        {
            this.laneID = ID;
        }
    }
}