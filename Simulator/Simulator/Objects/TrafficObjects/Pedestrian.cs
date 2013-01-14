using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Pedestrian : TrafficObject
	{
		public Pedestrian(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 0.25f;
		}

		public Pedestrian(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "pedestrian";
		}
	}
}