using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Truck : TrafficObject
	{
		public Truck(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 2f;
		}

		public Truck(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "truck";
		}
	}
}