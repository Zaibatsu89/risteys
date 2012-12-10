using System.Collections.Generic;
using KruispuntGroep6.Simulator.Objects;

namespace KruispuntGroep6.Simulator.Globals
{
    class Lists
    {
        public List<Vehicle> Vehicles {get; set;}
        public List<Lane> Lanes { get; set; }
        public Tile[,] Tiles { get; set; }

        public Lists()
        {
            Vehicles = new List<Vehicle>();
            Tiles = new Tile[0,0];
            Lanes = new List<Lane>();
        }
    }
}