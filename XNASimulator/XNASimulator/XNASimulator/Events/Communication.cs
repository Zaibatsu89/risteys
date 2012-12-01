using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using KruispuntGroep6.Communication.Json;
using XNASimulator.Main;

namespace XNASimulator
{
    class Communication
    {
		private int connectionAttempts;
		private Form form;
		private ListBox listBox;
		private TcpClient tcpClient;
		private Thread thrReadForever;
		private System.Timers.Timer timerConnection;

		public Communication()
		{
			// Create command-line client that connects with the server
			tcpClient = new TcpClient();

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
				tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 1337);
			}
			catch (SocketException)
			{
				if (connectionAttempts > 11)
				{
					DisconnectWhileConnecting();
				}
			}

			if (tcpClient.Connected)
				Connected();
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

					if (!string.Equals(message, null))
					{
						if (!string.Equals(message, string.Empty))
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
				Console.WriteLine("Error: " + e);
			}
		}

		private void Decrypter(string message)
		{
			string[] json = message.Split(',');
			if (json[0].Equals("input"))
			{
				if (json[2].Equals("car"))
				{
					// Spawn car at 'from'
					VehicleControl.Spawn(json[3]);

					// Drive car to 'to'
					VehicleControl.Drive(json[4]);
				}
			}
		}
    }
}