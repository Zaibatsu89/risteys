using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KruispuntGroep6.Communication
{
	public class Server
	{
		private static string address;
		private const int port = 1337;
		private static TcpClient connection;
		private static ClientHandler simulator;
		private static Strings strings = new Strings();

		public Server()
		{
			address = strings.Address;

			Console.Title = strings.TcpServer;

			BeginListen();
		}

		private static void BeginListen()
		{
			TcpListener listener = new TcpListener(IPAddress.Parse(address), port);
			
			listener.Start();

			// Start listening for connections
			Console.WriteLine(strings.Waiting);

			while (true)
			{
				connection = listener.AcceptTcpClient();

				if (connection != null)
				{
					// An incoming connection needs to be processed
					simulator = new ClientHandler(connection);
					simulator.Start();
				}
				else
					break;
			}

			listener.Stop();
			simulator.Stop();
			Console.WriteLine(strings.Stopped);
			Console.Read();
		}
	}

	class ClientHandler
	{
		TcpClient ClientSocket;
		Thread ClientThread;
		bool ContinueProcess = false;
		Strings strings = new Strings();

		public ClientHandler(TcpClient ClientSocket)
		{
			this.ClientSocket = ClientSocket;
		}

		public void Start()
		{
			ContinueProcess = true;
			ClientThread = new Thread(new ThreadStart(Process));
			ClientThread.Start();
		}

		private void Process()
		{
			// Incoming data from the client
			string data = null;

			// Data buffer for incoming data
			byte[] bytes;

			if (ClientSocket != null)
			{
				NetworkStream networkStream = ClientSocket.GetStream();
				ClientSocket.ReceiveTimeout = 100;

				while (ContinueProcess)
				{
					bytes = new byte[ClientSocket.ReceiveBufferSize];
					
					try
					{
						int BytesRead = networkStream.Read(bytes, 0, (int)ClientSocket.ReceiveBufferSize);
						if (BytesRead > 0)
						{
							data = Encoding.ASCII.GetString(bytes, 0, BytesRead);

							if (data.Equals(strings.Exit))
							{
								Console.WriteLine(strings.SimulatorDisconnected);
							}
							else
							{
								// Show the data on the console
								Console.WriteLine(strings.Received, data);
							}

							byte[] sendBytes = default(byte[]);
							string message = string.Empty;

							if (data.Equals(strings.HiIAmSimulator))
							{
								// Send a custom message back to the client
								message = strings.HiIAmController;
								sendBytes = Encoding.ASCII.GetBytes(message);

								// Show the message on the console
								Console.WriteLine(strings.Sent, message);
							}
							else
							{
								// Echo the data back to the client.
								sendBytes = Encoding.ASCII.GetBytes(data);
							}

							networkStream.Write(sendBytes, 0, sendBytes.Length);

							if (data.Equals(strings.Exit)) break;
						}


					}
					catch (IOException) { } // Time-out
					catch (SocketException)
					{
						Console.WriteLine(strings.Broken);
						break;
					}
					Thread.Sleep(200);
				}

				networkStream.Close();
				ClientSocket.Close();
			}
		}

		public void Stop()
		{
			ContinueProcess = false;
			if (ClientThread != null && ClientThread.IsAlive)
				ClientThread.Join();
		}

		public bool Alive
		{
			get
			{
				return (ClientThread != null && ClientThread.IsAlive);
			}
		}
	}
}