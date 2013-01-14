using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Default : TrafficObject
	{
		public Default(Texture2D texture, string ID)
			: base(texture, ID)
        {

		}

		public Default(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "default";
		}
	}
}