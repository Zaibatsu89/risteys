using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Godzilla : TrafficObject
	{
		public Godzilla(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 0.5f;
		}

		public Godzilla(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "godzilla";
		}
	}
}