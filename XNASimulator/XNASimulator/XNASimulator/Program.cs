using System.Threading;
using KruispuntGroep6.Communication.Client;
using KruispuntGroep6.Communication.Server;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Simulator
{
	/// <summary>
	/// Class used to be the first called class.
	/// </summary>
	public static class Program
	{
		private static string address;	// String used to be the IP address used by Client.
		private static MainGame game;	// MainGame used to be the instance of Simulator.

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
            var serverThread = new Thread(ServerTask);
            serverThread.Start();

            var clientThread = new Thread(ClientTask);
            clientThread.Start();

			address = string.Empty;

			while (address.Equals(string.Empty))
			{
				address = Client.GetAddress();
			}

			while (true)
			{
				var simulatorThread = new Thread(SimulatorTask);
				simulatorThread.Start();

				bool disconnected = false;

				while (!disconnected)
				{
					disconnected = Client.GetDisconnected();
				}

				Client.SetDisconnected(false);

				game.Exit();
			}
		}
		
		/// <summary>
		/// The client task.
		/// </summary>
		public static void ClientTask()
		{
			Client client = new Client();
			client.ShowDialog();
		}

		/// <summary>
		/// The server task.
		/// </summary>
		public static void ServerTask()
		{
			Server server = new Server();
		}

		/// <summary>
		/// The simulator task.
		/// </summary>
		public static void SimulatorTask()
		{
			using (game = new MainGame(address))
			{
				game.Run();
			}
		}
	}
}