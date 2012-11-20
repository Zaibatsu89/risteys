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

			/* For later...
			var dynJson = DynamicJson.Parse(message);

			switch (getMessageType(message))
			{
				case "starttime":
					json = dynJson.starttime;
					break;
				case "time":
					json = Convert.ToString(dynJson.time);
					break;
				default:
					throw new Exception(String.Format("{0} is niet bekend als type!", getMessageType(message)));
			}*/

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

			/* For later...
			switch (getJsonType(json))
			{
				case "starttime":
					message = DynamicJson.Serialize(new {starttime = message});
					break;
				case "time":
					message = DynamicJson.Serialize(new {time = message, type = "car", from = "N3", to = "S6"});
					break;
				default:
					throw new Exception(String.Format("{0} is niet bekend als type!", getJsonType(json)));
			}*/

			return message;
		}

		/// <summary>
		/// Gets the json type of a dynamic json string.
		/// </summary>
		/// <param name="message">String used to contain a dynamic json.</param>
		/// <returns>String used to contain the json type.</returns>
		public static string getJsonType(string message)
		{
			// TODO: return the json type.
			return message;
		}

		/// <summary>
		/// Gets the json type of a readable message.
		/// </summary>
		/// <param name="message">String used to contain a readable message.</param>
		/// <returns>String used to contain the json type.</returns>
		public static string getMessageType(string message)
		{
			return message.Equals(string.Empty) ? string.Empty : (message.Split('"'))[1];
		}
    }
}