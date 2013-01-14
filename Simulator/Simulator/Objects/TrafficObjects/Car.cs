using Microsoft.Xna.Framework.Graphics;
using System;

namespace KruispuntGroep6.Simulator.Objects.TrafficObjects
{
	public class Car : TrafficObject
	{
		public Car(Texture2D texture, string ID)
			: base(texture, ID)
        {
			speed = 4f;
		}
		
		public Car(string ID)
			: base(ID)
		{
			
		}

		public override string ToString()
		{
			return "car";
		}
	}
}