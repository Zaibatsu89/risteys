using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace KruispuntGroep6.Communication.Server
{
	/// <summary>
	/// Class used to listen for incoming connection attempts.
	/// </summary>
	public class Server
	{
		private static string address;					// String used to contain the IP address of the internet connection.
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
			// Get IP address
			GetAddress();

			// inform the human being
			Console.WriteLine(string.Format(strings.HiIAmController +
				" and I serve from {0}:1337 forever", address));
			//create our TCPListener object
			server = new System.Net.Sockets.TcpListener(IPAddress.Parse(address), strings.Port);
			//check to see if the server is running
            //while (true) do the commands
            while (true)
			{
				try
				{
					//start the server
					server.Start();
				}
				catch (SocketException)
				{
					Console.WriteLine(strings.OneServer);
					Console.ReadKey();
					break;
				}
				//create a null connection
				TcpClient client = null;
				//check if there are any pending connection requests
				if (server.Pending())
				{
					//if there are pending requests create a new connection
					client = server.AcceptTcpClient();
					//add client to clients
					clients.Add(client);
					//create a new DoCommunicate Object
					DoCommunicate comm = new DoCommunicate(client);
				}
			}
		}

		/// <summary>
		/// Gets the IP address of this server.
		/// </summary>
		private static void GetAddress()
		{
			// Get internet IP address
			address = string.Empty;
			IPHostEntry host;
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily.ToString().Equals(strings.Internet))
				{
					address = ip.ToString();
				}
			}
			// If there is no internet, use localhost
			if (address.Equals(string.Empty))
			{
				address = strings.Localhost;
			}
		}

		/// <summary>
		/// Sends message to all clients.
		/// </summary>
		/// <param name="message">String used to contain the message to send</param>
		public static void SendMessage(string message)
		{
			//create our StreamWriter object
			StreamWriter writer;

			//create client to be removed
			TcpClient clientToBeRemoved = null;

            //loop through and write any messages to the window
			foreach (TcpClient client in clients)
			{
				try
				{
					//check if the message is empty, of the particular
					//index of out array is null, if it is then continue
					if (!string.Equals(message.Trim(), string.Empty) || client != null)
					{
						//Use the GetStream method to get the current memory
						//stream for this index of our TCPClient array
						writer = new StreamWriter(client.GetStream());
						//send our message
						writer.WriteLine(message);
						//make sure the buffer is empty
						writer.Flush();
						//dispose of our writer
						writer = null;
					}
				}
				catch (Exception)
				{
					// Remove client
					clientToBeRemoved = client;
				}
			}

			//if client to be removed isn't null, then remove that client for clients list
			if (clientToBeRemoved != null)
			{
				clients.Remove(clientToBeRemoved);
			}

			//show message in console
			Console.WriteLine(String.Format(strings.Sent, message));
		}
	}
}