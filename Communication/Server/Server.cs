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
			Console.WriteLine(string.Format(strings.HiIAmController, address));
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
					//create a new DoCommunicate object
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
	}
}