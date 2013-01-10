using System.Collections.Generic;
using KruispuntGroep6.Simulator.Main;
using KruispuntGroep6.Simulator.Objects;
using KruispuntGroep6.Simulator.Objects.TrafficObjects;

namespace KruispuntGroep6.Simulator.Globals
{
    public class Lists
    {
        public TrafficObject[] Vehicles { get; set; }
        public List<Lane> Lanes {get; set;}
        public Tile[,] Tiles { get; set; }

        public Lists()
        {
            Vehicles = new TrafficObject[MainGame.TilesHor * MainGame.TilesVer];

			for (int i = 0; i < Vehicles.Length; i++)
			{
				Vehicles[i] = new Default(string.Empty);
			}

            Tiles = new Tile[0,0];
            Lanes = new List<Lane>();
        }
    }
}