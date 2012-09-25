using System;

namespace KruispuntGroep6.Communication
{
    public class JsonInput
    {
		dynamic json;
		string waarde;
		JsonType jsonType;

		public JsonInput()
		{
			json = DynamicJson.Parse(getRandomJSON());
			waarde = string.Empty;
		}

        public string getType()
        {
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
            else
                jsonType = JsonType.Default;

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
				waarde = "onbekende json";

			return waarde;
        }

		private string getRandomJSON()
		{
			Random random = new Random();
			int randomInt = random.Next(4);
			string randomString = String.Empty;

			switch (randomInt)
			{
				case 0:
					randomString = @"{""time"":0, ""type"":""car"", ""from"":""N4"", ""to"":""S6""}";
					break;
				case 1:
					randomString = @"{""light"":""N1"", ""state"":""red""}";
					break;
				case 2:
					randomString = @"{""light"":""N1"", ""type"":""bus"", ""loop"":""far"", ""empty"":true, ""to"":""E6""}";
					break;
				case 3:
					randomString = @"{""starttime"":""23:00""}";
					break;
				case 4:
					randomString = @"{""multiplier"":1}";
					break;
			}

			return randomString;
		}
    }
}