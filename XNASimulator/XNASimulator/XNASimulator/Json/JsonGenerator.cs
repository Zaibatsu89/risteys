using System;
using System.Collections;
using System.IO;
using KruispuntGroep6.Simulator;

namespace KruispuntGroep6.Simulator.Json
{
	/// <summary>
	/// Class used to generate random detector JSONs, input JSONs or stoplight JSONs.
	/// </summary>
	class JsonGenerator
	{
		private int index;		// Integer used to determine which jsons index needs to be used.
		private string[] jsons;	// String array used to store all generated JSONs.
		private int nextCount;	// Integer used to store the next counter in the GenerateJson function.
		private int previousCount;	// Integer used to store the previous counter in the GenerateJson function.
		private string previousJson;	// String used to store the previous JSON  in the GenerateJson function.
		private Random random;	// Random used to generate random numbers.
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Constructor.
		/// </summary>
		public JsonGenerator()
		{
			random = new Random();
		}

		/// <summary>
		/// Gets random detector light.
		/// </summary>
		/// <returns>String used to contain the detector light value.</returns>
		private string GetRandomDetectorLight()
		{
			string direction = GetRandomDirection(null);
			string lane = GetRandomLane(null);

			return direction + lane;
		}

		/// <summary>
		/// Gets random detector to.
		/// </summary>
		/// <returns>String used to contain the detector to value.</returns>
		private string GetRandomDetectorTo(string ln, string dir)
		{
			string direction = GetRandomDirection(new ArrayList() { ln });
			string lane = string.Empty;

			switch (dir)
			{
				case "0":
					lane = "7";
					break;
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
					lane = "6";
					break;
				case "7":
					lane = "0";
					break;
				default:
					throw new Exception(string.Format("Direction int {0} wordt niet herkend!", dir));
			}

			return direction + lane;
		}

		/// <summary>
		/// Gets random detector type.
		/// </summary>
		/// <returns>String used to contain the detector type value (bicycle, bus, car, pedestrian).</returns>
		private string GetRandomDetectorType(string lane)
		{
			string vehicleType = string.Empty;

			switch (lane)
			{
				case "0":
					vehicleType = "pedestrian";
					break;
				case "1":
					vehicleType = "bicycle";
					break;
				case "2":
					vehicleType = "bus";
					break;
				case "3":
				case "4":
				case "5":
					if (random.Next(2) < 1)
					{
						vehicleType = "car";
					}
					else
					{
						vehicleType = "truck";
					}
					break;
				case "6":
					vehicleType = "godzilla";
					break;
				case "7":
					vehicleType = "pedestrian";
					break;
				default:
					throw new Exception(string.Format("Lane {0} wordt niet herkend!", lane));
			}

			return vehicleType;
		}

		/// <summary>
		/// Gets random lane.
		/// </summary>
		/// <param name="exclude">Integer array used to contain the lane(s) that needs to be ignored.</param>
		/// <returns>String used to contain an integer (mostly between zero and seven)</returns>
		private string GetRandomLane(ArrayList exclude)
		{
			int intRandom = -1;
			string strRandom;

			if (exclude != null)
			{
				while (exclude.Contains(intRandom) || intRandom < 0)
				{
					intRandom = random.Next(8);
				}
			}
			else
			{
				intRandom = random.Next(8);
			}

			if (intRandom.Equals(6))
			{
				// Chance at getting Godzilla is 12 times less than getting other vehicles.
				if (random.Next(12) > 0)
				{
					strRandom = GetRandomLane(exclude);
				}
			}

			strRandom = intRandom.ToString();

			return strRandom;
		}

		/// <summary>
		/// Gets random direction.
		/// </summary>
		/// <returns>String used to contain a direction (E, N, S or W).</returns>
		private string GetRandomDirection(ArrayList exclude)
		{
			int intRandom = -1;
			string strRandom = string.Empty;

			if (exclude != null)
			{
				while (exclude.Contains(strRandom) || intRandom < 0)
				{
					intRandom = random.Next(4);

					switch (intRandom)
					{
						case 0:
							strRandom = "E";
							break;
						case 1:
							strRandom = "N";
							break;
						case 2:
							strRandom = "S";
							break;
						case 3:
							strRandom = "W";
							break;
						default:
							throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
					}
				}
			}
			else
			{
				intRandom = random.Next(4);

				switch (intRandom)
				{
					case 0:
						strRandom = "E";
						break;
					case 1:
						strRandom = "N";
						break;
					case 2:
						strRandom = "S";
						break;
					case 3:
						strRandom = "W";
						break;
					default:
						throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
				}
			}

			return strRandom;
		}

		/// <summary>
		/// Gets random direction: from, not to.
		/// </summary>
		/// <param name="vehicleType">VehicleType used to contain the vehicle type value.</param>
		/// <returns>String used to contain the position in two chars: first is direction, second is lane number.</returns>
		private string GetRandomDirectionFrom(string vehicleType)
		{
			string direction = GetRandomDirection(null);
			string lane = string.Empty;

			switch (vehicleType)
			{
				case "bicycle":
					lane = "1";
					break;
				case "bus":
					lane = "2";
					break;
				case "car":
					// Car can come from lane 3, 4 or 5.
					lane = random.Next(3, 6).ToString();
					break;
				case "godzilla":
					// Godzilla comes from the output lane, hehe :P.
					lane = "6";
					break;
				case "pedestrian":
					// Pedestrian has equal change to come from lane 0 or 7.
					if (random.Next(2) < 1)
					{
						lane = "0";
					}
					else
					{
						lane = "7";
					}
					break;
				case "truck":
					// Truck can come from lane 3, 4 or 5.
					lane = random.Next(3, 6).ToString();
					break;
				default:
					// Unknown vehicle can come from every lane, except 6.
					lane = GetRandomLane(new ArrayList() { 6 });
					break;
			}

			return direction + lane;
		}

		/// <summary>
		/// Gets random direction: to, not from.
		/// </summary>
		/// <param name="vehicleType">VehicleType used to contain the vehicle type value.</param>
		/// <param name="from">String used to contain the position from where the vehicle comes from.</param>
		/// <returns>String used to contain the position in two chars: first is direction, second is lane number.</returns>
		private string GetRandomDirectionTo(string vehicleType, string from)
		{
			string direction = string.Empty;
			string lane = string.Empty;

			int intRandom = random.Next(3);

			switch (vehicleType)
			{
				// bicycle can go from every direction and lane 1 and go to every direction and lane 6.
				case "bicycle":
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (intRandom)
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
							switch (intRandom)
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
							switch (intRandom)
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
							switch (intRandom)
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
				case "bus":
					switch (from.Substring(0, 1))
					{
						case "N":
							switch (intRandom)
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
							switch (intRandom)
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
							switch (intRandom)
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
							switch (intRandom)
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
				case "car":
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
				// Godzilla can go from every direction and lane 6 and go to the opposite direction and lane 6.
				case "godzilla":
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
				case "pedestrian":
					direction = from[0].ToString();

					if (from[1].ToString().Equals("0"))
					{
						lane = "7";
					}
					else
					{
						lane = "0";
					}
					break;
				// Truck can go from every direction and lane 3, 4 or 5 and go to every direction and lane 6.
				case "truck":
					switch (from[0].ToString())
					{
						case "N":
							switch (from[1].ToString())
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
							switch (from[1].ToString())
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
							switch (from[1].ToString())
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
							switch (from[1].ToString())
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
				// Unknown vehicle can go to every direction and every lane, except 1, 2, 3, 4 and 5.
				default:
					direction = GetRandomDirection(null);
					lane = GetRandomLane(new ArrayList() { 1, 2, 3, 4, 5 });
					break;
			}

			return direction + lane;
		}

		/// <summary>
		/// Gets random empty.
		/// </summary>
		/// <returns>String used to contain the empty value (true or false).</returns>
		private string GetRandomEmpty()
		{
			int intRandom = random.Next(2);
			string strRandom = string.Empty;

			switch (intRandom)
			{
				case 0:
					strRandom = "true";
					break;
				case 1:
					strRandom = "false";
					break;
				default:
					throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
			}

			return strRandom;
		}

		/// <summary>
		/// Gets random loop.
		/// </summary>
		/// <returns>String used to contain the loop value (far, close or null).</returns>
		private string GetRandomLoop(string vehicleType)
		{
			int intRandom = random.Next(3);

			if (vehicleType.Equals("pedestrian"))
			{
				intRandom = random.Next(1, 3);
			}

			string strRandom = string.Empty;

			switch (intRandom)
			{
				case 0:
					strRandom = "far";
					break;
				case 1:
					strRandom = "close";
					break;
				case 2:
					strRandom = "null";
					break;
				default:
					throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
			}

			return strRandom;
		}

		/// <summary>
		/// Gets random stoplight light (direction and lane).
		/// </summary>
		/// <returns>String used to contain the stoplight light value.</returns>
		private string GetRandomStoplightLight()
		{
			string direction = GetRandomDirection(null);
			ArrayList exclude = new ArrayList() { 6 };
			string lane = GetRandomLane(exclude);

			return direction + lane;
		}

		/// <summary>
		/// Gets random state (green, red, yellow, blink, off).
		/// </summary>
		/// <returns>String used to contain the state value.</returns>
		private string GetRandomState()
		{
			int intRandom = random.Next(5);
			string strRandom = string.Empty;

			switch (intRandom)
			{
				case 0:
					strRandom = "green";
					break;
				case 1:
					strRandom = "red";
					break;
				case 2:
					strRandom = "yellow";
					break;
				case 3:
					strRandom = "blink";
					break;
				case 4:
					strRandom = "off";
					break;
				default:
					throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
			}

			return strRandom;
		}

		/// <summary>
		/// Gets random vehicle type (bicycle, bus, car, godzilla, pedestrian or truck).
		/// </summary>
		/// <returns>VehicleType used to contain the vehicle type value.</returns>
		private string GetRandomVehicleType(ArrayList exclude)
		{
			int intRandom = -1;
			string strRandom = string.Empty;

			if (exclude != null)
			{
				while (exclude.Contains(strRandom) || intRandom < 0)
				{
					intRandom = random.Next(6);

					switch (intRandom)
					{
						case 0:
							strRandom = "bicycle";
							break;
						case 1:
							strRandom = "bus";
							break;
						case 2:
							strRandom = "car";
							break;
						case 3:
							// Chance at getting Godzilla is 12 times less than getting other vehicles.
							if (random.Next(12) < 1)
							{
								strRandom = "godzilla";
							}
							else
							{
								strRandom = GetRandomVehicleType(exclude);
							}
							break;
						case 4:
							strRandom = "pedestrian";
							break;
						case 5:
							strRandom = "truck";
							break;
						default:
							throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
					}
				}
			}
			else
			{
				intRandom = random.Next(6);

				switch (intRandom)
				{
					case 0:
						strRandom = "bicycle";
						break;
					case 1:
						strRandom = "bus";
						break;
					case 2:
						strRandom = "car";
						break;
					case 3:
						// Chance at getting Godzilla is 12 times less than getting other vehicles.
						if (random.Next(12) < 1)
						{
							strRandom = "godzilla";
						}
						else
						{
							strRandom = GetRandomVehicleType(exclude);
						}
						break;
					case 4:
						strRandom = "pedestrian";
						break;
					case 5:
						strRandom = "truck";
						break;
					default:
						throw new Exception(string.Format("Random int {0} wordt niet herkend!", intRandom));
				}
			}

			return strRandom;
		}

		/// <summary>
		/// Generates JSON.
		/// </summary>
		/// <param name="count">Integer used to count the current JSON time.</param>
		/// <param name="total">Integer used to determine what the total number of JSONs is.</param>
		/// <returns>String used to contain JSON.</returns>
		public string GenerateJSON(string type, int count, int total)
		{
			if (count.Equals(0))
			{
				index = 0;
				jsons = new string[total];
				previousCount = 0;
				previousJson = string.Empty;
			}

			previousCount = count;

			if (type.Equals("inputs"))
			{
				float floatTrafficDensity = ((float)count / (float)total) * 3f + 1;
				int intTrafficDensity = (int)Math.Round(floatTrafficDensity, 0);
				int randomInt = random.Next(intTrafficDensity);

				if (randomInt > 0)
				{
					count -= index++;
				}
				else if (index > 0)
				{
					count -= index - 1;
				}
				else
				{
					index++;
				}
			}

			string json = previousJson;

			if (previousCount.Equals(count) || count > nextCount)
			{
				json = @"[{";

				nextCount = count;
			}
			else
			{
				json = json.Remove(json.Length - 1);
				json += @", {";
			}

			switch (type)
			{
				case "inputs":
					string vehicleType = GetRandomVehicleType(null);
					string positionFrom = GetRandomDirectionFrom(vehicleType);
					string positionTo = GetRandomDirectionTo(vehicleType, positionFrom);

					json += @"""time"":""";
					json += count.ToString();
					json += @""", ""type"":""";
					json += vehicleType;
					json += @""", ""from"":""";
					json += positionFrom;
					json += @""", ""to"":""";
					json += positionTo;
					break;
				case "stoplights":
					string stoplightLight = GetRandomStoplightLight();
					string state = GetRandomState();

					json += @"""light"":""";
					json += stoplightLight;
					json += @""", ""state"":""";
					json += state;
					break;
				case "detectors":
					string detectorLight = GetRandomDetectorLight();
					string detectorType = GetRandomDetectorType(
						detectorLight[1].ToString());
					string loop = GetRandomLoop(
						detectorType);
					string empty = GetRandomEmpty();
					string detectorTo = GetRandomDetectorTo(
						detectorLight[0].ToString(),
						detectorLight[1].ToString());

					json += @"""light"":""";
					json += detectorLight;
					json += @""", ""type"":""";
					json += detectorType;
					json += @""", ""loop"":""";
					json += loop;
					json += @""", ""empty"":""";
					json += empty;
					json += @""", ""to"":""";
					json += detectorTo;
					break;
			}

			json += @"""}]";

			if (type.Equals("inputs"))
			{
				jsons[index + count - (previousCount - count) - 1] = json;
			}
			else
			{
				jsons[index] = json;
				index++;
			}

			previousJson = json;

			return json;
		}

		/// <summary>
		/// Saves JSON string array in JSON file.
		/// </summary>
		/// <param name="jsonType">String used to determine the type of the JSON string array.</param>
		/// <returns>Boolean used to determine if the saving is succesfull.</returns>
		public bool SaveJSONFile(string jsonType)
		{
			bool succes = true;
			string message = string.Empty;

			// Remove empty JSONs
			int index = 0;

			foreach (string json in jsons)
			{
				if (json != null)
				{
					index++;
				}
			}

			string[] tempJsons = new string[index];

			index = 0;

			foreach (string json in jsons)
			{
				if (json != null)
				{
					tempJsons[index] = json;
					index++;
				}
			}

			jsons = tempJsons;

			try
			{
				// Write JSON file in main folder of KruispuntGroep6 called XNASimulator
				File.WriteAllLines(jsonType + strings.JsonFileExtension, jsons);
			}
			catch (Exception)
			{
				succes = false;
			}

			return succes;
		}
	}
}