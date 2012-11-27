using System.Threading;
using KruispuntGroep6.Communication.Client;
using KruispuntGroep6.Communication.Server;

namespace XNASimulator
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
			var serverThread = new Thread(ServerTask);
			serverThread.Start();

			var clientThread = new Thread(ClientTask);
			clientThread.Start();

			using (MainGame game = new MainGame())
			{
				game.Run();
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
	}
}