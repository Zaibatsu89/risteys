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
			//create a new thread
			Thread clientThread = new Thread(new ThreadStart(startClient));
			//start the new thread
			clientThread.Start();
		}

		private void runClient()
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
						Console.WriteLine(strings.SimulatorDisconnected);
						client.Close();
						break;
					}
					// If String isn't welcome message, display him as JSON.
					else if (!data.Equals(strings.HiIAmSimulator) && (data.StartsWith("{") || data.StartsWith("[")))
					{
						string message = JsonConverter.MessageToJson(data);
						Console.WriteLine(String.Format(strings.Received, message));
					}
					else
					{
						// Show the data on the console.
						Console.WriteLine(String.Format(strings.Received, data));
					}

					string strSend = string.Empty;

					// If String is welcome message.
					if (data.Equals(strings.HiIAmSimulator))
					{
						// Send a welcome message back to the client.
						strSend = strings.HiIAmController;

						// Show the message on the console.
						Console.WriteLine(String.Format(strings.Sent, strSend));
					}
					else
					{
						// Echo the data back to the client.
						strSend = data;
					}

					//send our message
					Server.SendMessage(strSend);
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Cannot receive from a client.");
			}
		}

		private void startClient()
		{
			//create our StreamReader object to read the current stream
			reader = new StreamReader(client.GetStream());
			//create a new thread for this user
			Thread clientThread = new Thread(new ThreadStart(runClient));
			//start the thread
			clientThread.Start();
		}
	}
}