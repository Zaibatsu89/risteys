using System;
using System.Linq;

namespace KruispuntGroep6.Communication.Json
{
	/// <summary>
	/// Class used to convert a JSON array or a JSON object to a readable message.
	/// </summary>
    public class JsonConverter
    {
		/// <summary>
		/// Gets the type of a JSON string.
		/// </summary>
		/// <param name="message">String used to contain a dynamic JSON.</param>
		/// <returns>String used to contain the JSON type.</returns>
		private static string GetType(string message)
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

		/// <summary>
		/// Converts dynamic JSON array string to readable message.
		/// </summary>
		/// <param name="json">String used to contain a dynamic JSON array.</param>
		/// <returns>String used to contain a readable message.</returns>
		public static string JsonArrayToMessage(string strJson)
		{
			var json = DynamicJson.Parse(strJson);
			string message = GetType(strJson);
			var count = ((dynamic[])json).Count();

			switch (message)
			{
				case "detector":
					var dLight = ((dynamic[])json).Select(d => d.light);
					var dType = ((dynamic[])json).Select(d => d.type);
					var loop = ((dynamic[])json).Select(d => d.loop);
					var empty = ((dynamic[])json).Select(d => d.empty);
					var dTo = ((dynamic[])json).Select(d => d.to);

					for (int i = 0; i < count; i++)
					{
						string strLight = dLight.ElementAt(i);
						string strType = dType.ElementAt(i);
						string strLoop = loop.ElementAt(i);
						string strEmpty = empty.ElementAt(i);
						string strTo = dTo.ElementAt(i);

						if (i > 0)
						{
							message += "[detector";
						}
						else
						{
							message = message.Insert(0, "[");
						}

						message += ",";
						message += strLight;
						message += ",";
						message += strType;
						message += ",";
						message += strLoop;
						message += ",";
						message += strEmpty;
						message += ",";
						message += strTo;
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "input":
					var time = ((dynamic[])json).Select(d => d.time);
					var type = ((dynamic[])json).Select(d => d.type);
					var from = ((dynamic[])json).Select(d => d.from);
					var to = ((dynamic[])json).Select(d => d.to);

					for (int i = 0; i < count; i++)
					{
						string strTime = time.ElementAt(i);
						string strType = type.ElementAt(i);
						string strFrom = from.ElementAt(i);
						string strTo = to.ElementAt(i);

						if (i > 0)
						{
							message += "[input";
						}
						else
						{
							message = message.Insert(0, "[");
						}

						message += ",";
						message += strTime;
						message += ",";
						message += strType;
						message += ",";
						message += strFrom;
						message += ",";
						message += strTo;
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "multiplier":
					var multiplier = ((dynamic[])json).Select(d => d.multiplier);

					for (int i = 0; i < count; i++)
					{
						string strMultiplier = multiplier.ElementAt(i);

						if (i > 0)
						{
							message += "[multiplier";
						}
						else
						{
							message = message.Insert(0, "[");
						}
						message += ",";
						message += strMultiplier;
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "stoplight":
					var light = ((dynamic[])json).Select(d => d.light);
					var state = ((dynamic[])json).Select(d => d.state);

					for (int i = 0; i < count; i++)
					{
						string strLight = light.ElementAt(i);
						string strState = state.ElementAt(i);

						if (i > 0)
						{
							message += "[stoplight";
						}
						else
						{
							message = message.Insert(0, "[");
						}

						message += ",";
						message += strLight;
						message += ",";
						message += strState;
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				
				case "start":
					var starttime = ((dynamic[])json).Select(d => d.starttime);

					for (int i = 0; i < count; i++)
					{
						string strStarttime = starttime.ElementAt(i);

						if (i > 0)
						{
							message += "[starttime";
						}
						else
						{
							message = message.Insert(0, "[");
						}
						message += ",";
						message += strStarttime;
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				
				default:
					throw new Exception(string.Format("JSON {0} heeft geen herkenbaar type!", strJson));
			}

			return message;
		}

		/// <summary>
		/// Converts dynamic JSON object string to readable message.
		/// </summary>
		/// <param name="json">String used to contain a dynamic JSON object.</param>
		/// <returns>String used to contain a readable message.</returns>
		public static string JsonObjectToMessage(string strJson)
		{
			var json = DynamicJson.Parse(strJson);
			string message = GetType(strJson);

			switch (message)
			{
				case "detector":
					string strDetectorLight = json.light;
					string strDetectorType = json.type;
					string strLoop = json.loop;
					string strEmpty = json.empty;
					string strDetectorTo = json.to;

					message = message.Insert(0, "[");
					message += ",";
					message += strDetectorLight;
					message += ",";
					message += strDetectorType;
					message += ",";
					message += strLoop;
					message += ",";
					message += strEmpty;
					message += ",";
					message += strDetectorTo;
					message += "]";

					break;
				case "input":
					string strTime = json.time;
					string strType = json.type;
					string strFrom = json.from;
					string strTo = json.to;

					message = message.Insert(0, "[");
					message += ",";
					message += strTime;
					message += ",";
					message += strType;
					message += ",";
					message += strFrom;
					message += ",";
					message += strTo;
					message += "]";

					break;
				case "multiplier":
					string strMultiplier = json.multiplier;

					message = message.Insert(0, "[");
					message += ",";
					message += strMultiplier;
					message += "]";

					break;
				case "start":
					string strStarttime = json.starttime;

					message = message.Insert(0, "[");
					message += ",";
					message += strStarttime;
					message += "]";

					break;
				case "stoplight":
					string strLight = json.light;
					string strState = json.state;

					message = message.Insert(0, "[");
					message += ",";
					message += strLight;
					message += ",";
					message += strState;
					message += "]";

					break;
				default:
					throw new Exception(string.Format("JSON {0} heeft geen herkenbaar type!", strJson));
			}

			return message;
		}
    }
}