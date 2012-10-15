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
			var r = DynamicJson.Parse(Encoding.ASCII.GetString(bytes, 0, bytesRead));

			return r.Naam;
		}

		/// <summary>
		/// Gets byte array from string.
		/// </summary>
		/// <returns>Byte array used to contain 8192 bytes of serialized JSON.</returns>
		public static byte[] StringToBytes(string message)
		{
			var json = DynamicJson.Serialize(new { Naam = "Johannes", Woonplaats = "Ferwert" });

			return Encoding.ASCII.GetBytes(json);
		}
    }
}