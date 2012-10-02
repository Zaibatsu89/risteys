using System;
using System.Net.Sockets;
using System.Text;

namespace KruispuntGroep6.Communication
{
    class SendToController : SendTo
    {
		public override string Send(string message)
		{
			TcpClient simulator = new TcpClient();
			string fromSimulator = string.Empty;

			try
			{
				simulator.Connect(address, port);
				NetworkStream stream = simulator.GetStream();

				if (stream.CanRead && stream.CanWrite)
				{
					//while (message != "exit")
					//{
						byte[] bytes = Encoding.ASCII.GetBytes(message);
						stream.Write(bytes, 0, bytes.Length);

						// Reads the NetworkStream into a byte buffer
						byte[] buffer = new byte[simulator.ReceiveBufferSize];
						int bytesLength = stream.Read(buffer, 0, (int)simulator.ReceiveBufferSize);

						// Returns the data received from the simulator to the controller
						fromSimulator = Encoding.ASCII.GetString(bytes, 0, bytesLength);
					//}

					stream.Close();
					simulator.Close();
				}
				else
					simulator.Close();
			}
			catch (SocketException) { }
			catch (System.IO.IOException) { }
			catch (Exception) { }

			return fromSimulator;
		}
	}
}