using System;
using System.Threading;
using KruispuntGroep6.Communication.Client;
using KruispuntGroep6.Communication.Server;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Simulator
{
	public static class Program
	{
		private static MainGame game;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
            var serverThread = new Thread(ServerTask);
            serverThread.Start();

            var clientThread = new Thread(ClientTask);
            clientThread.Start();

			while (true)
			{
				var simulatorThread = new Thread(SimulatorTask);
				simulatorThread.Start();

				bool disconnected = false;

				while (!disconnected)
				{
					disconnected = Client.GetDisconnected();
				}

				game.Exit();
				simulatorThread.Abort();
			}
		}
		
		public static void ClientTask()
		{
			Client client = new Client();
			client.ShowDialog();
		}

		public static void ServerTask()
		{
			Server server = new Server();
		}

		public static void SimulatorTask()
		{
			string address = string.Empty;

			while (address.Equals(string.Empty))
			{
				address = Client.GetAddress();
			}

			using (game = new MainGame(address))
			{
				game.Run();
			}
		}
	}
}