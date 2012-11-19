using System;
using System.Text;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class to convert JSON from and to byte array or string.
	/// </summary>
    public class JsonConverter
    {
		/// <summary>
		/// Gets string from byte array.
		/// </summary>
		/// <param name="bytes">Byte array used to contain 8192 bytes of serialized JSON.</param>
		/// <param name="bytesRead">Integer used to contain number of chars in byte array.</param>
		/// <returns>String used to contain dynamic JSON value of Naam.</returns>
		public static string BytesToString(byte[] bytes, int bytesRead)
		{
			/* For later...
			string message = Encoding.ASCII.GetString(bytes, 0, bytesRead); 
			
			var json = DynamicJson.Parse(message);

			switch (getType(message))
			{
				case "starttime":
					message = json.starttime;
					break;
				case "time":
					message = Convert.ToString(json.time);
					break;
				default:
					throw new Exception(String.Format("{0} is niet bekend als type!", getType(message)));
			}*/

			return Encoding.ASCII.GetString(bytes, 0, bytesRead);
		}

		/// <summary>
		/// Gets byte array from string.
		/// </summary>
		/// <returns>Byte array used to contain 8192 bytes of serialized JSON.</returns>
		public static byte[] StringToBytes(string message)
		{
			return Encoding.ASCII.GetBytes(message);
		}

		public static string getType(string message)
		{
			return message.Equals(string.Empty) ? string.Empty : (message.Split('"'))[1];
		}
    }
}