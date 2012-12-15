using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KruispuntGroep6.Simulator.ObjectControllers;
using XNASimulator.Globals;

namespace KruispuntGroep6.Simulator.Events
{
    class Communication
    {
		private string address;
		private int connectionAttempts;
		private TcpClient tcpClient;
		private Thread thrReadForever;
		private TileControl tileControl;
		private System.Timers.Timer timerConnection;
        private VehicleControl vehicleControl;

		public Communication(string address, TileControl tileControl, VehicleControl vehicleControl)
		{
			this.address = address;
			this.tileControl = tileControl;
            this.vehicleControl = vehicleControl;

			timerConnection = new System.Timers.Timer(5000);
			timerConnection.Elapsed += new System.Timers.ElapsedEventHandler(timerConnection_Elapsed);
			timerConnection.Start();

			// Try to connect.
			Connect();
		}

		private void timerConnection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// Try to connect.
			Connect();

			connectionAttempts++;
		}

		private void Connect()
		{
			try
			{
				// Create command-line client that connects with the server
				tcpClient = new TcpClient();
				tcpClient.Connect(IPAddress.Parse(address), 1337);
			}
			catch (SocketException)
			{
				if (connectionAttempts > 8)
				{
					DisconnectWhileConnecting();
				}
			}

			if (tcpClient.Connected)
			{
				Connected();
			}
		}

		private void DisconnectWhileConnecting()
		{
			connectionAttempts = 0;
			timerConnection.Stop();
		}

		// Connected to controller.
		private void Connected()
		{
			//reset connection attempts
			connectionAttempts = 0;
			//stop connection timer
			timerConnection.Stop();
			//create message
			string message = "Hi, I am communication";
			//create a StreamWriter based on the current NetworkStream
			StreamWriter writer = new StreamWriter(tcpClient.GetStream());
			//write our message
			writer.WriteLine(message);
			//ensure the buffer is empty
			writer.Flush();

			// Read messages forever
			thrReadForever = new Thread(new ThreadStart(ReadForever));
			thrReadForever.Start();
		}

		/// <summary>
		/// Reads messages from controller, forever.
		/// </summary>
		private void ReadForever()
		{
			//create our StreamReader Object, based on the current NetworkStream
			StreamReader reader = new StreamReader(tcpClient.GetStream());
			// Begins to listen for incoming connection attempts from controller or simulator.
			try
			{
				while (true)
				{
					string message = reader.ReadLine();

					if (message != null)
					{
						if (!message.Equals(string.Empty))
						{
							// Pass message to Decrypter
							Decrypter(message);
						}
					}
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private void Decrypter(string message)
		{
			string[] jsonArray = message.Split(']');

			foreach (string jsonObjectWithBracket in jsonArray)
			{
				if (!jsonObjectWithBracket.Equals(string.Empty))
				{
					string jsonObject = string.Empty;

					if (jsonObjectWithBracket.StartsWith(","))
					{
						jsonObject = jsonObjectWithBracket.Remove(0, 2);
					}
					else
					{
						jsonObject = jsonObjectWithBracket.Remove(0, 1);
					}

					string[] jsonParameters = jsonObject.Split(',');

					if (jsonParameters[0].Equals("input"))
					{
						if (!jsonParameters[2].Equals("pedestrian"))
						{
							// Spawn car at 'from' and drive it to 'to'
							vehicleControl.Spawn(jsonParameters[2], jsonParameters[3], jsonParameters[4]);
						}
					}
					else if (jsonParameters[0].Equals("stoplight"))
					{
						LightsEnum lightsEnum = LightsEnum.Red;

						switch (jsonParameters[2])
						{
							case "blink":
								lightsEnum = LightsEnum.Blink;
								break;
							case "green":
								lightsEnum = LightsEnum.Green;
								break;
							case "red":
								lightsEnum = LightsEnum.Red;
								break;
							case "yellow":
								lightsEnum = LightsEnum.Yellow;
								break;
						}

						tileControl.ChangeLights(jsonParameters[1], lightsEnum);
					}
				}
			}
		}
    }
}