using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Bus : TrafficObject
	{
		public Bus(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 3f;
		}

		public Bus(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "bus";
		}
	}
}