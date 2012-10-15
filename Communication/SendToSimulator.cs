using System;
using System.Net.Sockets;
using System.Text;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class used to send messages to simulator.
	/// </summary>
    class SendToSimulator : SendTo
    {
		/// <summary>
		/// Sends message to simulator.
		/// </summary>
		/// <param name="message">String used to contain message.</param>
		/// <returns>String used to contain returned message from controller.</returns>
		public override string Send(string message)
		{
			// TcpClient used to provide a client connection for a TCP network service.
			TcpClient controller = new TcpClient();
			// String used to contain returned message from controller.
			string fromController = string.Empty;

			try
			{
				// Connects client to localhost and leet port.
				controller.Connect(address, port);
				// NetworkStream used to send and receive data.
				NetworkStream stream = controller.GetStream();

				// If NetworkStream supports reading and writing.
				if (stream.CanRead && stream.CanWrite)
				{
					// Byte array used to contain message.
					byte[] bytes = JsonConverter.StringToBytes(message);
					// Write data to NetworkStream.
					stream.Write(bytes, 0, bytes.Length);

					// Reads the NetworkStream into a byte buffer.
					byte[] buffer = new byte[controller.ReceiveBufferSize];
					int bytesLength = stream.Read(buffer, 0, (int)controller.ReceiveBufferSize);

					// Returns the data received from the controller to the simulator.
					fromController = Encoding.ASCII.GetString(bytes, 0, bytesLength);

					// Close NetworkStream and TcpClient.
					stream.Close();
					controller.Close();
				}
				else
					// Close TcpClient.
					controller.Close();
			}
			// Gonna catch 'em all... Pokémon!
			catch (SocketException) { }
			catch (System.IO.IOException) { }
			catch (Exception) { }

			// String used to contain returned message from controller.
			return fromController;
		}
	}
}