using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.ObjectControllers;

namespace KruispuntGroep6.Simulator.Events
{
    public class Communication
    {
		private TileControl tileControl;
		private VehicleControl vehicleControl;

		public Communication(TileControl tileControl, VehicleControl vehicleControl)
		{
			this.tileControl = tileControl;
			this.vehicleControl = vehicleControl;
		}

		/// <summary>
		/// This function is static, because Communication/Client needs to access it.
		/// </summary>
		/// <param name="message">String used to contain the message to decrypt.</param>
		public void Decrypter(string message)
		{
			string[] jsonArray = message.Split(']');

			foreach (string jsonObjectWithBracket in jsonArray)
			{
				if (!jsonObjectWithBracket.Equals(string.Empty))
				{
					string jsonObject = string.Empty;

					if (jsonObjectWithBracket.StartsWith(","))
					{
						jsonObject = jsonObjectWithBracket.Remove(0, 2);
					}
					else
					{
						jsonObject = jsonObjectWithBracket.Remove(0, 1);
					}

					string[] jsonParameters = jsonObject.Split(',');

					if (jsonParameters[0].Equals("input"))
					{
						if (!jsonParameters[2].Equals("pedestrian"))
						{
							// Spawn vehicle at 'from' and drive it to 'to'
							vehicleControl.Spawn(jsonParameters[2], jsonParameters[3], jsonParameters[4]);
						}
					}
					else if (jsonParameters[0].Equals("stoplight"))
					{
						LightsEnum lightsEnum = LightsEnum.Red;

						switch (jsonParameters[2])
						{
							case "blink":
								lightsEnum = LightsEnum.Blink;
								break;
							case "green":
								lightsEnum = LightsEnum.Green;
								break;
							case "red":
								lightsEnum = LightsEnum.Red;
								break;
							case "yellow":
								lightsEnum = LightsEnum.Yellow;
								break;
						}

						tileControl.ChangeLights(jsonParameters[1], lightsEnum);
					}
				}
			}
		}
    }
}