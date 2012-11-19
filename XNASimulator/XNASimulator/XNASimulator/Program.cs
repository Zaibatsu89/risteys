using System.Threading;
using KruispuntGroep6.Communication;

namespace XNASimulator
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			var serverThread = new Thread(ServerTask);
			serverThread.Start();

			using (MainGame game = new MainGame())
			{
				game.Run();
			}
		}

		public static void ServerTask()
		{
			Server server = new Server();
		}
	}
}