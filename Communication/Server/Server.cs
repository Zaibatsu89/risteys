using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace KruispuntGroep6.Communication.Server
{
	/// <summary>
	/// Public class used to listen for incoming connection attempts.
	/// </summary>
	public class Server
	{
		private static List<TcpClient> clients;			// List<TcpClient> used to contain list of connected clients.
		private static TcpListener server;				// TcpListener used to contain the server.
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Constructor.
		/// </summary>
		public Server()
		{
			// Sets title of console window.
			Console.Title = strings.TcpServer;

			// Create new list of clients
			clients = new List<TcpClient>();
			//create our TCPListener object
			server = new System.Net.Sockets.TcpListener(IPAddress.Parse(strings.Address), strings.Port);
			//check to see if the server is running
			//while (true) do the commands
			while (true)
			{
				//start the chat server
				server.Start();
				//create a null connection
				TcpClient client = null;
				//check if there are any pending connection requests
				if (server.Pending())
				{
					//if there are pending requests create a new connection
					client = server.AcceptTcpClient();
					//add client to clients
					clients.Add(client);
					//display a message letting the user know they're connected
					StreamWriter writer = new StreamWriter(client.GetStream());
					writer.WriteLine(strings.HiIAmController);
					Console.WriteLine(String.Format(strings.Sent, strings.HiIAmController));
					//display what the connection has to say
					StreamReader reader = new StreamReader(client.GetStream());
					Console.WriteLine(String.Format(strings.Received, reader.ReadLine()));
					//create a new DoCommunicate Object
					DoCommunicate comm = new DoCommunicate(client);
				}
			}
		}

		/// <summary>
		/// Send message to all clients.
		/// </summary>
		/// <param name="message">String used to contain the message to send</param>
		public static void SendMessage(string message)
		{
			//create our StreamWriter object
			StreamWriter writer;
            //loop through and write any messages to the window
			foreach (TcpClient client in clients)
			{
				try
				{
					//check if the message is empty, of the particular
					//index of out array is null, if it is then continue
					if (!string.Equals(message.Trim(), string.Empty) || !TcpClient.Equals(client, null))
						continue;
					//Use the GetStream method to get the current memory
					//stream for this index of our TCPClient array
					writer = new StreamWriter(client.GetStream());
					//send our message
					writer.WriteLine(message);
					//make sure the buffer is empty
					writer.Flush();
					//dispose of our writer
					writer = null;
					//show message in console
					Console.WriteLine(String.Format(strings.Sent, message));
				}
				catch (Exception)
				{
					// Remove client
					clients.Remove(client);
				}
			}
		}
	}
}