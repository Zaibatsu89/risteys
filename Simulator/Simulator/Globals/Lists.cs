using System.Collections.Generic;
using KruispuntGroep6.Simulator.Main;
using KruispuntGroep6.Simulator.Objects;

namespace KruispuntGroep6.Simulator.Globals
{
    public class Lists
    {
        public Vehicle[] Vehicles { get; set; }
        public List<Lane> Lanes {get; set;}
        public Tile[,] Tiles { get; set; }

        public Lists()
        {
            Vehicles = new Vehicle[MainGame.TilesHor * MainGame.TilesVer];
            for (int i = 0; i < Vehicles.Length; i++)
            {
                Vehicles[i] = new Vehicle(string.Empty);
            }

            Tiles = new Tile[0,0];
            Lanes = new List<Lane>();
        }
    }
}