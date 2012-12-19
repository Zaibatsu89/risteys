using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using KruispuntGroep6.Communication.Json;

namespace KruispuntGroep6.Communication.Server
{
	/// <summary>
	/// Class used to do the communication.
	/// </summary>
	class DoCommunicate
	{
		TcpClient client;	// TcpClient used to communicate with.
		StreamReader reader;	// Reader used to read messages from a network stream.
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tcpClient">TcpClient used to determine the TCP client.</param>
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

		/// <summary>
		/// Runs the client.
		/// Uses a try...catch to catch any exceptions.
		/// </summary>
		private void RunClient()
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
					// If String is exit message, close client and break.
					if (data.Equals(strings.Exit))
					{
						Console.WriteLine(strings.Disconnected);
						client.Close();
						break;
					}
					// If String is welcome message.
					else if (data.StartsWith(strings.Hi))
					{
						Console.WriteLine(string.Format(strings.Connected, data));
					}
					// Else, display message.
					else
					{
						Console.WriteLine(string.Format(strings.Received, data));
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