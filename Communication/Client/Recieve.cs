using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using KruispuntGroep6.Communication.Json;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Communication.Client
{
	/// <summary>
	/// Class used to receive messages from the controller.
	/// </summary>
    public static class Recieve
    {
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private static StreamReader reader; // Reader used to read messages from a network stream.
		private static String recievedMessage; // String used to contain the current recieved message from the Controller.
		private static MainGame simulator; // MainGame used to be the instance of Simulator.

		public static TcpClient tcpClient { get; set; } // TcpClient used to contain the TCP client.

		public static String message { get; set; } // Message received from the controller.

		/// <summary>
		/// Sends a message to the controller.
		/// </summary>
		/// <param name="tcpClient">The TcpClient is used to determine the current client connection</param>
		/// <param name="message">The string is used to determine the message to send</param>
		public static void ReceiveMessage(MainGame mainGame)
		{
			simulator = mainGame;

			recievedMessage = string.Empty;

			//create our StreamReader object to read the current stream
			reader = new StreamReader(tcpClient.GetStream());

			//create a new thread
			Thread clientThread = new Thread(new ThreadStart(RunClient));
			//start the new thread
			clientThread.Start();
		}

		/// <summary>
		/// Runs the client.
		/// Uses a try...catch to catch any exceptions.
		/// </summary>
		private static void RunClient()
		{
			// Begins to listen for incoming connection attempts from controller or simulator.
			try
			{
				while (true)
				{
					//read the current line
					message = reader.ReadLine();

					if (message != null)
					{
						if (!recievedMessage.Equals(message))
						{
							recievedMessage = Recieve.message;

							recievedMessage = JsonConverter.JsonArrayToMessage(recievedMessage);

							simulator.Communication.Decrypter(recievedMessage);
						}
					}
				}
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