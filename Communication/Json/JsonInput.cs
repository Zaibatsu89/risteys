using System;

namespace KruispuntGroep6.Communication.Json
{
	/// <summary>
	/// Public class used to distinguish between the dynamic input JSON's various random types.
	/// </summary>
    public class JsonInput
    {
		/// <summary>
		/// Gets dynamic input JSON of a random type.
		/// </summary>
		/// <returns></returns>
		public dynamic getRandomJSON()
		{
			Random random = new Random();
			int randomInt = random.Next(4);
			string randomString = String.Empty;

			switch (randomInt)
			{
				case 0:
					// Type: Input
					randomString = @"{""time"":0, ""type"":""car"", ""from"":""N4"", ""to"":""S6""}";
					break;
				case 1:
					// Type: Stoplight
					randomString = @"{""light"":""N1"", ""state"":""red""}";
					break;
				case 2:
					// Type: Detector
					randomString = @"{""light"":""N1"", ""type"":""bus"", ""loop"":""far"", ""empty"":true, ""to"":""E6""}";
					break;
				case 3:
					// Type: Start
					randomString = @"{""starttime"":""23:00""}";
					break;
				case 4:
					// Type: Multiplier
					randomString = @"{""multiplier"":1}";
					break;
			}

			// Parse the string to dynamic JSON.
			return DynamicJson.Parse(randomString);
		}

		/// <summary>
		/// Gets value of dynamic JSON.
		/// </summary>
		/// <param name="json">Dynamic object used to contain JSON.</param>
		/// <returns>String used to contain value.</returns>
		public string getValue(dynamic json)
		{
			JsonType jsonType = getType(json);
			string waarde = string.Empty;

			if (jsonType == JsonType.Input)
				waarde = json.type.ToString();
			else if (jsonType == JsonType.Stoplight)
				waarde = json.state.ToString();
			else if (jsonType == JsonType.Detector)
				waarde = json.loop.ToString();
			else if (jsonType == JsonType.Start)
				waarde = json.starttime.ToString();
			else if (jsonType == JsonType.Multiplier)
				waarde = json.multiplier.ToString();
			else
				waarde = "Unknown JSON";

			return waarde;
		}

		/// <summary>
		/// Gets type of dynamic JSON.
		/// </summary>
		/// <param name="json">Dynamic object used to contain JSON.</param>
		/// <returns>String used to contain value.</returns>
        public JsonType getType(dynamic json)
        {
			JsonType jsonType = JsonType.Default;

            if (json.IsDefined("time"))
                jsonType = JsonType.Input;
            else if (json.IsDefined("state"))
                jsonType = JsonType.Stoplight;
            else if (json.IsDefined("loop"))
                jsonType = JsonType.Detector;
            else if (json.IsDefined("starttime"))
                jsonType = JsonType.Start;
            else if (json.IsDefined("multiplier"))
                jsonType = JsonType.Multiplier;

			return jsonType;
		}
    }
}