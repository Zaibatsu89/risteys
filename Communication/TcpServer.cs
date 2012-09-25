using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KruispuntGroep6.Communication
{
	public class TcpServer
	{
		public TcpServer()
		{
			try
			{
				Console.Title = "Kruispunt Groep 6: TCP Server";

				IPAddress localAddress = IPAddress.Parse("127.0.0.1");

				// Define the kind of socket we want: Internet, Stream, TCP
				Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				// Define the address we want to claim: the first IP address found earlier, at port 1337
				IPEndPoint ipEndpoint = new IPEndPoint(localAddress, 1337);

				// Bind the socket to the end point
				listenSocket.Bind(ipEndpoint);

				// Start listening, only allow 1 connection to queue at the same time
				listenSocket.Listen(1);
				listenSocket.BeginAccept(new AsyncCallback(ReceiveCallback), listenSocket);
				Console.WriteLine("Server is waiting on socket {0}", listenSocket.LocalEndPoint);

				// Start being important while the world rotates
				while (true)
				{
					// Write a message and sleep for 2 seconds
					Console.WriteLine("Busy Waiting....");
					Thread.Sleep(2000);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Caught Exception: {0}", ex.ToString());
			}
		}

		private static void ReceiveCallback(IAsyncResult AsyncCall)
		{
			// Data is send in bytes -- so we need to convert a C# string to a Byte[]
			ASCIIEncoding encoding = new ASCIIEncoding();
			Byte[] message = encoding.GetBytes("Hoi Simulator, ik ben Controller. Leuk om je te ontmoeten!");

			// The original listening socket is returned in the AsyncCall, we need to call "EndAccept" to
			// receive the client socket which we can use to send and receive data.
			Socket listener = (Socket)AsyncCall.AsyncState;
			Socket client = listener.EndAccept(AsyncCall);

			Console.WriteLine("Received Connection from {0}", client.RemoteEndPoint);
			client.Send(message);

			// End of the incoming connection
			Console.WriteLine("Ending the connection");
			client.Close();

			// At the end of the connection, we need to tell the OS that we can receive another call
			listener.BeginAccept(new AsyncCallback(ReceiveCallback), listener);
		}
	}
}