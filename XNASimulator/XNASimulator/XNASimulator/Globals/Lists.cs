using System.Collections.Generic;
using KruispuntGroep6.Simulator.Objects;
using System.Runtime.CompilerServices;

namespace KruispuntGroep6.Simulator.Globals
{
    class Lists
    {
        public List<Vehicle> Vehicles
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return Vehicles; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set { Vehicles = value; }
        }
        public List<Lane> Lanes 
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return Lanes; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set { Lanes = value; }
        }
        public Tile[,] Tiles
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return Tiles; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set { Tiles = value; }
        }

        public Lists()
        {
            Vehicles = new List<Vehicle>();
            Tiles = new Tile[0,0];
            Lanes = new List<Lane>();
        }
    }
}