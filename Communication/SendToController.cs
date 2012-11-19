using System;
using System.Net.Sockets;
using System.Text;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class used to send messages to controller.
	/// </summary>
    class SendToController : SendTo
    {
		

		/// <summary>
		/// Sends message to controller.
		/// </summary>
		/// <param name="message">String used to contain message.</param>
		/// <returns>String used to contain returned message from simulator.</returns>
		public override string Send(string message)
		{
			// TcpClient used to provide a client connection for a TCP network service.
			TcpClient simulator = new TcpClient();
			// String used to contain returned message from simulator.
			string fromSimulator = string.Empty;

			try
			{
				// Connects client to localhost and leet port.
				simulator.Connect(address, port);
				// NetworkStream used to send and receive data.
				NetworkStream stream = simulator.GetStream();

				// If NetworkStream supports reading and writing.
				if (stream.CanRead && stream.CanWrite)
				{
					// Byte array used to contain message.
					byte[] bytes = JsonConverter.StringToBytes(message);

					// Write data to NetworkStream.
					stream.Write(bytes, 0, bytes.Length);

					// Reads the NetworkStream into a byte buffer.
					byte[] buffer = new byte[simulator.ReceiveBufferSize];
					int bytesLength = stream.Read(buffer, 0, (int)simulator.ReceiveBufferSize);

					// Returns the data received from the simulator to the controller.
					fromSimulator = Encoding.ASCII.GetString(bytes, 0, bytesLength);
				}

				// Close NetworkStream and TcpClient.
				stream.Close();
				simulator.Close();
			}
			// Gonna catch 'em all... Pokémon!
			catch (SocketException e)
			{
				Console.WriteLine(String.Format(strings.SocketException, e.Message));
			}
			catch (System.IO.IOException e)
			{
				Console.WriteLine(String.Format(strings.IOException, e.Message));
			}
			catch (Exception e)
			{
				Console.WriteLine(String.Format(strings.Exception, e.Message));
			}

			// String used to contain returned message from simulator.
			return fromSimulator;
		}
	}
}