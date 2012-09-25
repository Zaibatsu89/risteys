using System.Threading;
using KruispuntGroep6.Communication;

namespace XNASimulator
{
	static class Program
	{
		private static TcpClient client;
		private static TcpServer server;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			var clientThread = new Thread(ClientTask);
			clientThread.Start();

			var serverThread = new Thread(ServerTask);
			serverThread.Start();

			using (Game1 game = new Game1())
			{
				game.Run();
			}
		}

		public static void ClientTask()
		{
			client = new TcpClient();
			client.ShowDialog();
		}

		public static void ServerTask()
		{
			server = new TcpServer();
		}
	}
}
