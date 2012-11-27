using System;

namespace KruispuntGroep6.Communication.Json
{
	/// <summary>
	/// Class to convert JSON from and to byte array or string.
	/// </summary>
    public class JsonConverter
    {
		/// <summary>
		/// Converts readable message to dynamic json string.
		/// </summary>
		/// <param name="message">String used to contain a readable message.</param>
		/// <returns>String used to contain a dynamic json.</returns>
		public static string MessageToJson(string message)
		{
			string json = message;

			var dynJson = DynamicJson.Parse(message);

			json = (getType(json));

			switch (json)
			{
				case "input":
					json += ",";
					json += dynJson.time;
					json +=	",";
					json +=	dynJson.type;
					json +=	",";
					json +=	dynJson.from;
					json +=	",";
					json +=	dynJson.to;
					break;
				case "stoplight":
					json += ",";
					json += dynJson.light;
					json += ",";
					json += dynJson.state;
					break;
				case "detector":
					json += ",";
					json += dynJson.light;
					json += ",";
					json += dynJson.type;
					json += ",";
					json += dynJson.loop;
					json += ",";
					json += dynJson.empty;
					json += ",";
					json += dynJson.to;
					break;
				case "start":
					json += ",";
					json += dynJson.starttime;
					break;
				case "multiplier":
					json += ",";
					json += dynJson.multiplier;
					break;
				default:
					throw new Exception(String.Format("Message {0} heeft geen herkenbaar type!", getType(json)));
			}

			return json;
		}

		/// <summary>
		/// Converts dynamic json string to readable message.
		/// </summary>
		/// <param name="message">String used to contain a dynamic json.</param>
		/// <returns>String used to contain a readable message.</returns>
		public static string JsonToMessage(string json)
		{
			string message = json;

			switch (getType(json))
			{
				case "input":
					message = DynamicJson.Serialize(new
					{
						time = getParameter(message, 0),
						type = getParameter(message, 1),
						from = getParameter(message, 2),
						to = getParameter(message, 3),
					});
					break;
				case "stoplight":
					message = DynamicJson.Serialize(new
					{
						light = getParameter(message, 0),
						state = getParameter(message, 1),
					});
					break;
				case "detector":
					message = DynamicJson.Serialize(new
					{
						light = getParameter(message, 0),
						type = getParameter(message, 1),
						loop = getParameter(message, 2),
						empty = getParameter(message, 3),
						to = getParameter(message, 4),
					});
					break;
				case "start":
					message = DynamicJson.Serialize(new
					{
						starttime = getParameter(message, 0)
					});
					break;
				case "multiplier":
					message = DynamicJson.Serialize(new
					{
						multiplier = getParameter(message, 0)
					});
					break;
				default:
					throw new Exception(String.Format("Json {0} heeft geen herkenbaar type!", getType(json)));
			}

			return message;
		}

		private static string getParameter(string json, int which)
		{
			string parameter = string.Empty;

			int index = which;
			if (which.Equals(1))
				index = json.IndexOf(':', json.IndexOf(':') + 1);
			if (which.Equals(2))
				index = json.IndexOf(':', json.IndexOf(':', json.IndexOf(':') + 1) + 1);
			if (which.Equals(3))
				index = json.IndexOf(':', json.IndexOf(':', json.IndexOf(':', json.IndexOf(':') + 1) + 1) + 1);
			if (which.Equals(4))
				index = json.IndexOf(':', json.IndexOf(':', json.IndexOf(':', json.IndexOf(':', json.IndexOf(':') + 1) + 1) + 1) + 1);

			int doublePoint = json.IndexOf(':', index) + 1;
			int doublePoint2 = json.IndexOf(',', doublePoint);
			int endPoint = json.IndexOf('}');
			if (doublePoint2 < 1)
				parameter = json.Substring(doublePoint, endPoint - doublePoint);
			else if (doublePoint < doublePoint2)
				parameter = json.Substring(doublePoint, doublePoint2 - doublePoint);

			// Remove trash
			parameter = parameter.Remove(parameter.Length - 1);
			parameter = parameter.Remove(0, 1);

			return parameter;
		}

		/// <summary>
		/// Gets the json type of a dynamic json string.
		/// </summary>
		/// <param name="message">String used to contain a dynamic json.</param>
		/// <returns>String used to contain the json type.</returns>
		private static string getType(string message)
		{
			string jsonType = string.Empty;
			
			if (message.Contains("from"))
				jsonType = "input";
			else if (message.Contains("state"))
				jsonType = "stoplight";
			else if (message.Contains("loop"))
				jsonType = "detector";
			else if (message.Contains("starttime"))
				jsonType = "start";
			else if (message.Contains("multiplier"))
				jsonType = "multiplier";

			return jsonType;
		}
    }
}