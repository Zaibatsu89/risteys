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
		private static Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private static AutoResetEvent connectionWaitHandle = new AutoResetEvent(false); // AutoResetEvent used to spread incoming connections

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
			TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

			// Set timeouts
			listener.Server.ReceiveTimeout = 10000;
			listener.Server.SendTimeout = 10000;

			// Start listening for incoming connection requests.
			listener.Start();

			// Start listening for connections.
			Console.WriteLine(strings.Waiting);

			// True forever
			while (true)
			{
				// Accepts a pending connection request.
				IAsyncResult result = listener.BeginAcceptTcpClient(HandleAsyncConnection, listener);

				// Wait until a client has begun handling an event.
				connectionWaitHandle.WaitOne();

				// Reset wait handle or the loop goes as fast as it can (after first request).
				connectionWaitHandle.Reset();
			}
		}

		/// <summary>
		/// Handle an async connection
		/// </summary>
		/// <param name="result">An IAsyncResult</param>
		private static void HandleAsyncConnection(IAsyncResult result)
		{
			/*AsyncState gets a user-defined object that qualifies
			 *or contains information about an asynchronous operation.*/
			TcpListener listener = (TcpListener)result.AsyncState;
			/*EndAcceptTcpClient asynchronously accepts an incoming
			 *connection attempt and creates this TcpClient to handle
			 *remote host communication.*/
			TcpClient client = listener.EndAcceptTcpClient(result);

			//Inform the main thread this connection is now handled.
			connectionWaitHandle.Set();

			//Process TcpClient.
			Process(client);
		}

		/// <summary>
		/// Process TcpClient.
		/// </summary>
		private static void Process(TcpClient client)
		{
			// Boolean used to contain True if Process needs to continue and False otherwise.
			bool ContinueProcess = true;

			// Incoming data from the client.
			string data = null;

			// Data buffer for incoming data.
			byte[] receiveBytes;

			if (client != null)
			{
				// NetworkStream used to send and receive data.
				NetworkStream networkStream = client.GetStream();
				// ReceiveTimeout used to set the amount of time TcpClient will wait to receive data once a read operation is initiated.
				client.ReceiveTimeout = 100;

				while (ContinueProcess)
				{
					// Data buffer is as big as receive buffer.
					receiveBytes = new byte[client.ReceiveBufferSize];

					try
					{
						// Reads data from NetworkStream.
						int bytesRead = networkStream.Read(receiveBytes, 0, (int)client.ReceiveBufferSize);

						if (bytesRead > 0)
						{
							// String used to contain converted data buffer.
							data = Encoding.ASCII.GetString(receiveBytes, 0, bytesRead);

							// If String is exit message, display it.
							if (data.Equals(strings.Exit))
							{
								Console.WriteLine(strings.SimulatorDisconnected);
							}
							// If String isn't welcome message, display him as JSON.
							else if (!data.Equals(strings.HiIAmSimulator) && (data.StartsWith("{") || data.StartsWith("[")))
							{
								string message = JsonConverter.BytesToString(receiveBytes, bytesRead);
								Console.WriteLine(String.Format(strings.Received, message));
							}
							else
							{
								// Show the data on the console.
								Console.WriteLine(String.Format(strings.Received, data));
							}

							byte[] sendBytes = default(byte[]);

							// If String is welcome message.
							if (data.Equals(strings.HiIAmSimulator))
							{
								// Send a welcome message back to the client.
								sendBytes = Encoding.ASCII.GetBytes(strings.HiIAmController);

								// Show the message on the console.
								Console.WriteLine(String.Format(strings.Sent, strings.HiIAmController));
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
					catch (SocketException e)
					{
						Console.WriteLine(String.Format("SocketException: {0}", e.Message));
					}
					catch (System.IO.IOException e)
					{
						Console.WriteLine(String.Format("IOException: {0}", e.Message));
					}
					catch (Exception e)
					{
						Console.WriteLine(String.Format("Exception: {0}", e.Message));
					}
					// Short sleep.
					Thread.Sleep(1);
				}

				// Close NetworkStream and ClientHandler.
				networkStream.Close();
				client.Close();
			}
		}
	}
}