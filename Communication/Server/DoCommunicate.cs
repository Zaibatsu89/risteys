using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using KruispuntGroep6.Communication.Json;

namespace KruispuntGroep6.Communication.Server
{
	class DoCommunicate
	{
		TcpClient client;							// TcpClient used to communicate with.
		StreamReader reader;						// Reader used to read messages from a network stream.
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		public DoCommunicate(TcpClient tcpClient)
		{
			//create our TcpClient
			client = tcpClient;
			//create our StreamReader object to read the current stream
			reader = new StreamReader(client.GetStream());
			//create a new thread
			Thread clientThread = new Thread(new ThreadStart(RunClient));
			//start the new thread
			clientThread.Start();
		}

		private void RunClient()
		//use a try...catch to catch any exceptions
		{
			// Begins to listen for incoming connection attempts from controller or simulator.
			try
			{
				//set out line variable to an empty string
				string data = string.Empty;
				while (true)
				{
					//read the current line
					data = reader.ReadLine();
					//display this line
					// If String is exit message, display it and break.
					if (data.Equals(strings.Exit))
					{
						Console.WriteLine(strings.Disconnected, strings.HiIAmSimulator);
						client.Close();
						break;
					}
					// If String is welcome message.
					else if (data.StartsWith(strings.Hi))
					{
						Console.WriteLine(string.Format(strings.Connected, data));

						// Send a welcome message back to the client.
						data = strings.HiIAmController;
					}
					// If String isn't welcome message, display him as JSON.
					else if (data.StartsWith(strings.BraceOpen))
					{
						data = JsonConverter.JsonObjectToMessage(data);
					}
					else if (data.StartsWith(strings.BracketOpen))
					{
						data = JsonConverter.JsonArrayToMessage(data);
					}

					//send our message
					Server.SendMessage(data);
				}
			}
			catch (Exception)
			{
				Console.WriteLine(strings.ReceiveError);
			}
		}
	}
}