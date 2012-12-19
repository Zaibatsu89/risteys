using System.Threading;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class used to be the first called class.
	/// </summary>
	public static class Program
	{
		private static string address;	// String used to be the IP address used by Client.
		private static Client.Client client;	// Client used to be the instance of Client.
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
				if (client != null)
				{
					address = client.GetAddress();
				}
			}

			var simulatorThread = new Thread(SimulatorTask);
			simulatorThread.Start();
		}

		/// <summary>
		/// The client task.
		/// </summary>
		public static void ClientTask()
		{
			client = new Client.Client();
			client.ShowDialog();
		}

		/// <summary>
		/// The server task.
		/// </summary>
		public static void ServerTask()
		{
			Server.Server server = new Server.Server();
		}

		/// <summary>
		/// The simulator task.
		/// </summary>
		public static void SimulatorTask()
		{
			using (game = new MainGame(address))
			{
				client.SetSimulator(game);
				game.Run();
			}
		}
	}
}