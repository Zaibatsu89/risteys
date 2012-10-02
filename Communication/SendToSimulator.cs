using System;
using System.Net.Sockets;
using System.Text;

namespace KruispuntGroep6.Communication
{
    class SendToSimulator : SendTo
    {
		public override string Send(string message)
		{
			TcpClient controller = new TcpClient();
			string fromController = string.Empty;

			try
			{
				controller.Connect(address, port);
				NetworkStream stream = controller.GetStream();

				if (stream.CanRead && stream.CanWrite)
				{
					while (message != "exit")
					{
						byte[] bytes = Encoding.ASCII.GetBytes(message);
						stream.Write(bytes, 0, bytes.Length);

						// Reads the NetworkStream into a byte buffer
						byte[] buffer = new byte[controller.ReceiveBufferSize];
						int bytesLength = stream.Read(buffer, 0, (int)controller.ReceiveBufferSize);

						// Returns the data received from the controller to the simulator
						fromController = Encoding.ASCII.GetString(bytes, 0, bytesLength);
					}

					stream.Close();
					controller.Close();
				}
				else
					controller.Close();
			}
			catch (SocketException) { }
			catch (System.IO.IOException) { }
			catch (Exception) { }

			return fromController;
		}
	}
}