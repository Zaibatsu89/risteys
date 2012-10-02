using System.Threading;
using KruispuntGroep6.Communication;

namespace XNASimulator
{
	static class Program
	{
		private static Client client;
		private static Server server;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			var clientThread = new Thread(ClientTask);
			clientThread.Start();

			var serverThread = new Thread(ServerTask);
			serverThread.Start();

			/* not needed yet
			using (Game1 game = new Game1())
			{
				game.Run();
			}
			 */
		}

		public static void ClientTask()
		{
			client = new Client();
			client.ShowDialog();
		}

		public static void ServerTask()
		{
			server = new Server();
		}
	}
}
