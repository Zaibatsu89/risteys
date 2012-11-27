using System;
using System.Linq;

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
						time = getParameter(message, 1),
						type = getParameter(message, 2),
						from = getParameter(message, 3),
						to = getParameter(message, 4),
					});
					break;
				case "stoplight":
					message = DynamicJson.Serialize(new
					{
						light = getParameter(message, 1),
						state = getParameter(message, 2),
					});
					break;
				case "detector":
					message = DynamicJson.Serialize(new
					{
						light = getParameter(message, 1),
						type = getParameter(message, 2),
						loop = getParameter(message, 3),
						empty = getParameter(message, 4),
						to = getParameter(message, 5),
					});
					break;
				case "start":
					message = DynamicJson.Serialize(new
					{
						starttime = getParameter(message, 1)
					});
					break;
				case "multiplier":
					message = DynamicJson.Serialize(new
					{
						multiplier = getParameter(message, 1)
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

			int doublePoint = nthOccurence(json, ':', which);
			int endPoint = json.IndexOf('}');
			int separatorPoint = json.IndexOf(',', doublePoint);

			if (separatorPoint > 0)
				parameter = json.Substring(doublePoint, separatorPoint - doublePoint);
			else
				parameter = json.Substring(doublePoint, endPoint - doublePoint);

			// Remove trash
			parameter = parameter.Remove(parameter.Length - 1);
			parameter = parameter.Remove(0, 2);

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

		private static int nthOccurence(string s, char t, int n)
		{
			return s.TakeWhile(c => ((n -= (c == t ? 1 : 0))) > 0).Count();
		}
    }
}