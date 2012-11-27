using System;
using System.IO;
using System.Net.Sockets;
using KruispuntGroep6.Communication.Json;

namespace KruispuntGroep6.Communication.Client
{
	/// <summary>
	/// Class used to send messages to the controller.
	/// </summary>
    class Send
    {
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Sends a message to the controller.
		/// </summary>
		/// <param name="tcpClient">The TcpClient is used to determine the current client connection</param>
		/// <param name="message">The string is used to determine the message to send</param>
		public void SendMessage(TcpClient tcpClient, string message)
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
			}
			// Gonna catch 'em all... Pokémon!
			catch (SocketException e)
			{
				Console.WriteLine(String.Format(strings.SocketException, e.Message));
			}
			catch (System.IO.IOException e)
			{
				Console.WriteLine(String.Format(strings.IOException, e.Message));
			}
			catch (Exception e)
			{
				Console.WriteLine(String.Format(strings.Exception, e.Message));
			}
		}
	}
}