using System;
using System.IO;

namespace KruispuntGroep6.Communication.Json
{
	/// <summary>
	/// Class to generate random JSON.
	/// </summary>
	class JsonGenerator
	{
		int index;		// Integer used to determine which jsons index needs to be used.
		string[] jsons;	// String array used to store all generated JSONs.
		Random random;	// Random used to generate random numbers.

		/// <summary>
		/// Constructor.
		/// </summary>
		public JsonGenerator()
		{
			random = new Random();
		}

		/// <summary>
		/// Generates JSON.
		/// </summary>
		/// <param name="count">Integer used to count the current JSON time.</param>
		/// <param name="total">Integer used to determine what the total number of JSONs is.</param>
		/// <returns>String used to contain JSON.</returns>
		public string GenerateJSON(int count, int total)
		{
			if (count == 0)
			{
				index = 0;
				jsons = new string[total];
			}

			float floatTrafficDensity = ((float)count / (float)total) * 4f + 2;
			int intTrafficDensity = (int)Math.Round(floatTrafficDensity, 0);
			int randomInt = random.Next(intTrafficDensity);
			if (randomInt > 0)
				count -= index++;
			else if (index > 0)
				count -= index - 1;

			string json = @"{";
			VehicleType vehicleType = getRandomVehicleType();
			string positionFrom = getRandomPositionFrom(vehicleType);
			string positionTo = getRandomPositionTo(vehicleType, positionFrom);

			json += @"""time"":";
			json += count.ToString();
			json += @", ""type"":""";
			json += vehicleType.ToString().ToLower();
			json += @""", ""from"":""";
			json += positionFrom;
			json += @""", ""to"":""";
			json += positionTo;
			json += @"""}";

			if (index > 0)
				jsons[index + count - 1] = json;
			else
				jsons[0] = json;

			return json;
		}

		/// <summary>
		/// Saves jsons string array in input.json file.
		/// </summary>
		/// <returns>Boolean used to determine if the saving is succesfull.</returns>
		public bool SaveJSONFile()
		{
			bool succes = true;
			string message = string.Empty;

			try
			{
				// Write input.json in main folder KruispuntGroep6
				#if DEBUG
				File.WriteAllLines(@"..\..\..\input.json", jsons);
				#endif
			}
			catch (Exception e)
			{
				message = e.Message;
				succes = false;
			}

			return succes;
		}

		/// <summary>
		/// Gets random vehicle type (bike, bus, car, godzilla, pedestrian or truck).
		/// </summary>
		/// <returns>VehicleType used to contain the vehicle type value.</returns>
		private VehicleType getRandomVehicleType()
		{
			int randomInt = random.Next(6);
			VehicleType randomType = VehicleType.Default;

			switch (randomInt)
			{
				case 0:
					randomType = VehicleType.Bike;
					break;
				case 1:
					randomType = VehicleType.Bus;
					break;
				case 2:
					randomType = VehicleType.Car;
					break;
				case 3:
					// Chance at getting Godzilla is 12 times less than getting other vehicles.
					if (random.Next(12) < 1)
						randomType = VehicleType.Godzilla;
					else
						randomType = getRandomVehicleType();
					break;
				case 4:
					randomType = VehicleType.Pedestrian;
					break;
				case 5:
					randomType = VehicleType.Truck;
					break;
			}

			return randomType;
		}

		/// <summary>
		/// Gets random position.
		/// </summary>
		/// <returns>String used to contain direction (E, N, S or W).</returns>
		private string getRandomPosition()
		{
			int intDirection = random.Next(4);
			string stringDirection = string.Empty;

			switch (intDirection)
			{
				case 0:
					stringDirection = "E";
					break;
				case 1:
					stringDirection = "N";
					break;
				case 2:
					stringDirection = "S";
					break;
				case 3:
					stringDirection = "W";
					break;
			}
			return stringDirection;
		}

		/// <summary>
		/// Gets random position: from, not to.
		/// </summary>
		/// <param name="vehicleType">VehicleType used to contain the vehicle type value.</param>
		/// <returns>String used to contain the position in two chars: first is direction, second is lane number.</returns>
		private string getRandomPositionFrom(VehicleType vehicleType)
		{
			string direction = getRandomPosition();
			string lane = string.Empty;

			switch (vehicleType)
			{
				case VehicleType.Bike:
					lane = "1";
					break;
				case VehicleType.Bus:
					lane = "2";
					break;
				case VehicleType.Car:
					// Car can come from lane 3, 4 or 5.
					lane = random.Next(3, 6).ToString();
					break;
				case VehicleType.Default:
					// Unknown vehicle can come from every lane, except 6.
					int defaultLane = random.Next(8);
					while (defaultLane != 6)
					{
						lane = defaultLane.ToString();
						defaultLane = random.Next(8);
					}
					break;
				case VehicleType.Godzilla:
					// Godzilla comes from the output lane, hehe :P.
					lane = "6";
					break;
				case VehicleType.Pedestrian:
					// Pedestrian has equal change to come from lane 0 or 7.
					if (random.Next(2) < 1)
						lane = "0";
					else
						lane = "7";
					break;
				case VehicleType.Truck:
					// Truck can come from lane 3, 4 or 5.
					lane = random.Next(3, 6).ToString();
					break;
			}

			return direction + lane;
		}

		/// <summary>
		/// Gets random position: to, not from.
		/// </summary>
		/// <param name="vehicleType">VehicleType used to contain the vehicle type value.</param>
		/// <param name="from">String used to contain the position from where the vehicle comes from.</param>
		/// <returns>String used to contain the position in two chars: first is direction, second is lane number.</returns>
		private string getRandomPositionTo(VehicleType vehicleType, string from)
		{
			string direction = string.Empty;
			string lane = string.Empty;

			int randomInt = random.Next(3);

			switch (vehicleType)
			{
				// Bike can go from every direction and lane 1 and go to every direction and lane 6.
				case VehicleType.Bike:
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (randomInt)
							{
								case 0:
									direction = "E";
									break;
								case 1:
									direction = "S";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "E":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "S";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "S":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "E";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "W":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "E";
									break;
								case 2:
									direction = "S";
									break;
							}
							break;
					}
					lane = "6";
					break;
				// Bus can go from every direction and lane 2 and go to every direction and lane 6.
				case VehicleType.Bus:
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (randomInt)
							{
								case 0:
									direction = "E";
									break;
								case 1:
									direction = "S";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "E":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "S";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "S":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "E";
									break;
								case 2:
									direction = "W";
									break;
							}
							break;
						case "W":
							switch (randomInt)
							{
								case 0:
									direction = "N";
									break;
								case 1:
									direction = "E";
									break;
								case 2:
									direction = "S";
									break;
							}
							break;
					}
					lane = "6";
					break;
				// Car can go from every direction and lane 3, 4 or 5 and go to every direction and lane 6.
				case VehicleType.Car:
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "W";
									break;
								case "4":
									direction = "S";
									break;
								case "5":
									direction = "E";
									break;
							}
							break;
						case "E":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "N";
									break;
								case "4":
									direction = "W";
									break;
								case "5":
									direction = "S";
									break;
							}
							break;
						case "S":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "E";
									break;
								case "4":
									direction = "N";
									break;
								case "5":
									direction = "W";
									break;
							}
							break;
						case "W":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "S";
									break;
								case "4":
									direction = "E";
									break;
								case "5":
									direction = "N";
									break;
							}
							break;
					}
					lane = "6";
					break;
				// Unknown vehicle can go from every direction and every lane and go to every direction and every lane.
				case VehicleType.Default:
					direction = getRandomPosition();
					lane = random.Next(8).ToString();
					break;
				// Godzilla can go from every direction and lane 6 and go to the opposite direction and lane 6.
				case VehicleType.Godzilla:
					switch (from.Substring(0, 1))
					{
						case "N":
							direction = "S";
							break;
						case "E":
							direction = "W";
							break;
						case "S":
							direction = "N";
							break;
						case "W":
							direction = "E";
							break;
					}
					lane = "6";
					break;
				// Pedestrian can go from every direction and lane 0 or 7 and go to every direction and the other lane.
				case VehicleType.Pedestrian:
					direction = from.Substring(0, 1);

					if (from.Substring(1, 1) == "0")
						lane = "7";
					else
						lane = "0";
					break;
				// Truck can go from every direction and lane 3, 4 or 5 and go to every direction and lane 6.
				case VehicleType.Truck:
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "W";
									break;
								case "4":
									direction = "S";
									break;
								case "5":
									direction = "E";
									break;
							}
							break;
						case "E":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "N";
									break;
								case "4":
									direction = "W";
									break;
								case "5":
									direction = "S";
									break;
							}
							break;
						case "S":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "E";
									break;
								case "4":
									direction = "N";
									break;
								case "5":
									direction = "W";
									break;
							}
							break;
						case "W":
							switch (from.Substring(1, 1))
							{
								case "3":
									direction = "S";
									break;
								case "4":
									direction = "E";
									break;
								case "5":
									direction = "N";
									break;
							}
							break;
					}
					lane = "6";
					break;
			}

			return direction + lane;
		}
	}
}