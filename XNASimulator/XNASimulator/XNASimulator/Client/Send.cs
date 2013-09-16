using System;
using System.IO;
using System.Net.Sockets;

namespace KruispuntGroep6.Simulator.Client
{
	/// <summary>
	/// Class used to send messages to the controller.
	/// </summary>
	public static class Send
	{
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		public static TcpClient tcpClient { get; set; } // TcpClient used to contain the TCP client.

		/// <summary>
		/// Sends a message to the controller.
		/// </summary>
		/// <param name="tcpClient">The TcpClient is used to determine the current client connection</param>
		/// <param name="message">The string is used to determine the message to send</param>
		public static void SendMessage(string message)
		{
			string retMessage = string.Empty;

			try
			{
				//create a StreamWriter based on the current NetworkStream
				StreamWriter writer = new StreamWriter(tcpClient.GetStream());
				//write our message
				writer.WriteLine(message);
				//ensure the buffer is empty
				writer.Flush();
				
				//show message in console
				Console.WriteLine(string.Format(strings.Sent, message));
			}
			// Gonna catch 'em all... Pokémon!
			catch (SocketException e)
			{
				Console.WriteLine(string.Format(strings.SocketException, e.Message));
			}
			catch (IOException e)
			{
				Console.WriteLine(string.Format(strings.IOException, e.Message));
			}
			catch (Exception e)
			{
				Console.WriteLine(string.Format(strings.Exception, e.Message));
			}
		}
	}
}