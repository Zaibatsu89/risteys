using System;
using KruispuntGroep6.Simulator.Main;
using XNASimulator.Globals;
using KruispuntGroep6.Simulator.Json;

namespace KruispuntGroep6.Simulator.Communication
{
    public class Communication
    {
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private MainGame game;

		public Communication(MainGame game)
		{
			this.game = game;
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

					if (jsonParameters[0].Equals("INPUT"))
					{
						// Spawn vehicle at 'from' and drive it to 'to'
						game.Spawn(jsonParameters[2], jsonParameters[3], jsonParameters[4]);
					}
					else if (jsonParameters[0].Contains("STOPLIGHT"))
					{
						LightsEnum lightsEnum = LightsEnum.Blink;

						switch (jsonParameters[2])
						{
							case "BLINK":
								lightsEnum = LightsEnum.Blink;
								break;
							case "GREEN":
								lightsEnum = LightsEnum.Green;
								break;
							case "RED":
								lightsEnum = LightsEnum.Red;
								break;
							case "YELLOW":
								lightsEnum = LightsEnum.Yellow;
								break;
						}

						game.ChangeLights(jsonParameters[1], lightsEnum);
					}
				}
			}
		}
    }
}