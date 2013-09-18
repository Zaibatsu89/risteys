using System;
using System.Linq;

namespace KruispuntGroep6.Simulator.Json
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

			if (message.Contains("from") || message.Contains("FROM"))
				jsonType = "INPUT";
			else if (message.Contains("state") || message.Contains("STATE"))
				jsonType = "STOPLIGHT";
			else if (message.Contains("loop") || message.Contains("LOOP"))
				jsonType = "DETECTOR";
			else if (message.Contains("starttime") || message.Contains("STARTTIME"))
				jsonType = "STARTTIME";
			else if (message.Contains("multiplier") || message.Contains("MULTIPLIER"))
				jsonType = "MULTIPLIER";

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
			string jsonType = GetType(strJson);
			var count = ((dynamic[])json).Count();
			string message = string.Empty;

			switch (jsonType)
			{
				case "DETECTOR":
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
						bool boolEmpty = empty.ElementAt(i);
						string strEmpty = boolEmpty.ToString();
						string strTo = dTo.ElementAt(i);

						message += "[";
						message += jsonType;
						message += ",";
						message += strLight.ToUpper();
						message += ",";
						message += strType.ToUpper();
						message += ",";
						message += strLoop.ToUpper();
						message += ",";
						message += strEmpty.ToUpper();
						message += ",";
						message += strTo.ToUpper();
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "INPUT":
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

						message += "[";
						message += jsonType;
						message += ",";
						message += strTime.ToUpper();
						message += ",";
						message += strType.ToUpper();
						message += ",";
						message += strFrom.ToUpper();
						message += ",";
						message += strTo.ToUpper();
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "MULTIPLIER":
					var multiplier = ((dynamic[])json).Select(d => d.multiplier);

					for (int i = 0; i < count; i++)
					{
						string strMultiplier = multiplier.ElementAt(i);

						message += "[";
						message += jsonType;
						message += ",";
						message += strMultiplier.ToUpper();
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;
				case "STOPLIGHT":
					var light = ((dynamic[])json).Select(d => d.light);
					var state = ((dynamic[])json).Select(d => d.state);

					for (int i = 0; i < count; i++)
					{
						string strLight = light.ElementAt(i);
						string strState = state.ElementAt(i);

						message += "[";
						message += jsonType;
						message += ",";
						message += strLight.ToUpper();
						message += ",";
						message += strState.ToUpper();
						message += "],";
					}

					message = message.Remove(message.Length - 1);

					break;

				case "STARTTIME":
					var starttime = ((dynamic[])json).Select(d => d.starttime);

					for (int i = 0; i < count; i++)
					{
						string strStarttime = starttime.ElementAt(i);

						message += "[";
						message += jsonType;
						message += ",";
						message += strStarttime.ToUpper();
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
				case "DETECTOR":
					string strDetectorLight = json.light;
					string strDetectorType = json.type;
					string strLoop = json.loop;
					bool boolEmpty = json.empty;
					string strEmpty = boolEmpty.ToString();
					string strDetectorTo = json.to;

					message = message.Insert(0, "[");
					message += ",";
					message += strDetectorLight.ToUpper();
					message += ",";
					message += strDetectorType.ToUpper();
					message += ",";
					message += strLoop.ToUpper();
					message += ",";
					message += strEmpty.ToUpper();
					message += ",";
					message += strDetectorTo.ToUpper();
					message += "]";

					break;
				case "INPUT":
					string strTime = json.time;
					string strType = json.type;
					string strFrom = json.from;
					string strTo = json.to;

					message = message.Insert(0, "[");
					message += ",";
					message += strTime.ToUpper();
					message += ",";
					message += strType.ToUpper();
					message += ",";
					message += strFrom.ToUpper();
					message += ",";
					message += strTo.ToUpper();
					message += "]";

					break;
				case "MULTIPLIER":
					string strMultiplier = json.multiplier;

					message = message.Insert(0, "[");
					message += ",";
					message += strMultiplier.ToUpper();
					message += "]";

					break;
				case "STARTTIME":
					string strStarttime = json.starttime;

					message = message.Insert(0, "[");
					message += ",";
					message += strStarttime.ToUpper();
					message += "]";

					break;
				case "STOPLIGHT":
					string strLight = json.light;
					string strState = json.state;

					message = message.Insert(0, "[");
					message += ",";
					message += strLight.ToUpper();
					message += ",";
					message += strState.ToUpper();
					message += "]";

					break;
				default:
					throw new Exception(string.Format("JSON {0} heeft geen herkenbaar type!", strJson));
			}

			return message;
		}
	}
}