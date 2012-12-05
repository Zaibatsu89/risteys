using System.Threading;
using KruispuntGroep6.Communication.Client;
using KruispuntGroep6.Communication.Server;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Simulator
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
			
			string address = string.Empty;

			while (address.Equals(string.Empty))
			{
				address = Client.GetAddress();
			}

			using (MainGame game = new MainGame(address))
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