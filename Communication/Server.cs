using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Public class used to listen for incoming connection attempts.
	/// </summary>
	public class Server
	{
		private static string address;					// String used to contain IP address of localhost.
		private const int port = 1337;					// Integer used to contain leet port number.
		private static TcpClient connection;			// TcpClient used to provide a client connection for a TCP network service.
		private static ClientHandler simulator;			// ClientHandler used to handle client.
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Constructor.
		/// </summary>
		public Server()
		{
			address = strings.Address;

			// Sets title of console window.
			Console.Title = strings.TcpServer;

			// Begins to listen for incoming connection attempts from controller or simulator.
			BeginListen();
		}

		/// <summary>
		/// Begins to listen for incoming connection attempts from controller or simulator.
		/// </summary>
		private static void BeginListen()
		{
			// TcpListener used to listen for connections from TCP network clients.
			TcpListener listener = new TcpListener(IPAddress.Parse(address), port);
			
			// Start listening for incoming connection requests.
			listener.Start();

			// Start listening for connections.
			Console.WriteLine(strings.Waiting);

			while (true)
			{
				// Accepts a pending connection request.
				connection = listener.AcceptTcpClient();

				if (connection != null)
				{
					// An incoming connection needs to be processed.
					simulator = new ClientHandler(connection);
					simulator.Start();
				}
				else
					break;
			}

			// Stop TcpListener and ClientHandler
			listener.Stop();
			simulator.Stop();
			Console.WriteLine(strings.Stopped);
			Console.Read();
		}
	}

	/// <summary>
	/// Class used to handle client.
	/// </summary>
	class ClientHandler
	{
		TcpClient ClientSocket;				// TcpClient used to provide a client connection for a TCP network service.
		Thread ClientThread;				// Thread used to start Process.
		bool ContinueProcess = false;		// Boolean used to contain True if Process needs to continue and False otherwise.
		Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ClientSocket">TcpClient used to provide a client connection for a TCP network service.</param>
		public ClientHandler(TcpClient ClientSocket)
		{
			this.ClientSocket = ClientSocket;
		}

		/// <summary>
		/// Start.
		/// </summary>
		public void Start()
		{
			ContinueProcess = true;
			ClientThread = new Thread(new ThreadStart(Process));
			ClientThread.Start();
		}

		/// <summary>
		/// Process.
		/// </summary>
		private void Process()
		{
			// Incoming data from the client.
			string data = null;

			// Data buffer for incoming data.
			byte[] bytes;

			if (ClientSocket != null)
			{
				// NetworkStream used to send and receive data.
				NetworkStream networkStream = ClientSocket.GetStream();
				// ReceiveTimeout used to set the amount of time TcpClient will wait to receive data once a read operation is initiated.
				ClientSocket.ReceiveTimeout = 100;

				while (ContinueProcess)
				{
					// Data buffer is as big as receive buffer.
					bytes = new byte[ClientSocket.ReceiveBufferSize];

					try
					{
						// Reads data from NetworkStream.
						int bytesRead = networkStream.Read(bytes, 0, (int)ClientSocket.ReceiveBufferSize);

						if (bytesRead > 0)
						{
							// String used to contain converted data buffer.
							data = Encoding.ASCII.GetString(bytes, 0, bytesRead);

							// If String isn't welcome message, display him as JSON.
							if (!data.Equals(strings.HiIAmSimulator))
							{
								string d = JsonConverter.BytesToString(bytes, bytesRead);
								Console.WriteLine(d);
							}

							// If String is exit message, display it.
							if (data.Equals(strings.Exit))
							{
								Console.WriteLine(strings.SimulatorDisconnected);
							}
							else
							{
								// Show the data on the console.
								Console.WriteLine(strings.Received, data);
							}

							byte[] sendBytes = default(byte[]);
							string message = string.Empty;

							// If String is welcome message.
							if (data.Equals(strings.HiIAmSimulator))
							{
								// Send a welcome message back to the client.
								message = strings.HiIAmController;
								sendBytes = Encoding.ASCII.GetBytes(message);

								// Show the message on the console.
								Console.WriteLine(strings.Sent, message);
							}
							else
							{
								// Echo the data back to the client.
								sendBytes = Encoding.ASCII.GetBytes(data);
							}

							// Write data to NetworkStream.
							networkStream.Write(sendBytes, 0, sendBytes.Length);

							// If data is exit message, break.
							if (data.Equals(strings.Exit)) break;
						}


					}
					// Gonna catch 'em all... Pokémon!
					catch (IOException) { } // Time-out
					catch (SocketException)
					{
						Console.WriteLine(strings.Broken);
						break;
					}
					// Sleep.
					Thread.Sleep(200);
				}

				// Close NetworkStream and ClientHandler.
				networkStream.Close();
				ClientSocket.Close();
			}
		}

		// Stop.
		public void Stop()
		{
			ContinueProcess = false;
			if (ClientThread != null && ClientThread.IsAlive)
				ClientThread.Join();
		}

		// Is alive?
		public bool Alive
		{
			get
			{
				return (ClientThread != null && ClientThread.IsAlive);
			}
		}
	}
}