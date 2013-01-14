using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Bicycle : TrafficObject
	{
		public Bicycle(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 1f;
		}

		public Bicycle(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "bicycle";
		}
	}
}