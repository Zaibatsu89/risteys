using System;
using System.IO;

namespace KruispuntGroep6.Communication
{
	class JsonGenerator
	{
		int index;
		string[] jsons;
		Random random;

		public JsonGenerator()
		{
			random = new Random();
		}

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
					lane = random.Next(3, 6).ToString();
					break;
				case VehicleType.Default:
					lane = random.Next(8).ToString();
					break;
				case VehicleType.Godzilla:
					// hehe :P
					lane = "6";
					break;
				case VehicleType.Pedestrian:
					if (random.Next(2) < 1)
						lane = "0";
					else
						lane = "7";
					break;
				case VehicleType.Truck:
					lane = "3";
					break;
			}

			return direction + lane;
		}

		private string getRandomPositionTo(VehicleType vehicleType, string from)
		{
			string direction = string.Empty;
			string lane = string.Empty;

			int randomInt = random.Next(3);

			switch (vehicleType)
			{
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
				case VehicleType.Default:
					direction = getRandomPosition();
					lane = random.Next(8).ToString();
					break;
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
				case VehicleType.Pedestrian:
					direction = from.Substring(0, 1);

					if (from.Substring(1, 1) == "0")
						lane = "7";
					else
						lane = "0";
					break;
				case VehicleType.Truck:
					switch (from.Substring(0, 1))
					{
						case "N":
							direction = "W";
							break;
						case "E":
							direction = "N";
							break;
						case "S":
							direction = "E";
							break;
						case "W":
							direction = "S";
							break;
					}
					lane = "6";
					break;
			}

			return direction + lane;
		}
	}
}