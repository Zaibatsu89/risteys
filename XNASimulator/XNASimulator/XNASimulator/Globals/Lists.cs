using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNASimulator.Objects;

namespace XNASimulator.Globals
{
    class Lists
    {
        public List<Vehicle> Vehicles {get; set;}
        public Tile[,] Tiles { get; set; }
        public List<Lane> Lanes { get; set; }

        public Lists()
        {
            Vehicles = new List<Vehicle>();
            Tiles = new Tile[0,0];
            Lanes = new List<Lane>();
        }
    }
}
