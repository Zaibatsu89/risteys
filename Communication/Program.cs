using System.Threading;
using System.Windows.Forms;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class used to be the first called class.
	/// </summary>
	public static class Program
	{
		private static Client.Client client;	// Client used to be the instance of Client.
		private static MainGame simulator;	// MainGame used to be the instance of Simulator.
		private static bool simulatorAllowed = true;	// Boolean used to determine whether
														// the Simulator is allowed to start.

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
			//new Thread(new ThreadStart(ServerTask)).Start();

			client = new Client.Client();
			client.FormClosed += new FormClosedEventHandler(client_FormClosed);

			new Thread(new ThreadStart(ClientTask)).Start();

			string address = string.Empty;

			while (address.Equals(string.Empty))
			{
				if (client != null)
				{
					address = client.GetAddress();
				}
				else
				{
					address = int.MaxValue.ToString();
					simulatorAllowed = false;
				}
			}

			if (simulatorAllowed)
			{
				var simulatorThread = new Thread(SimulatorTask);
				simulatorThread.Start();
			}
		}

		/// <summary>
		/// The Client task.
		/// </summary>
		public static void ClientTask()
		{
			client.ShowDialog();
		}

		/// <summary>
		/// The Server task.
		/// </summary>
		public static void ServerTask()
		{
			Server.Server server = new Server.Server();
		}

		/// <summary>
		/// The Simulator task.
		/// </summary>
		public static void SimulatorTask()
		{
			using (simulator = new MainGame())
			{
				client.SetSimulator(simulator);
				simulator.Run();
			}
		}
		
		/// <summary>
		/// The form closed event of Client.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private static void client_FormClosed(object sender, FormClosedEventArgs e)
		{
			client = null;
		}
	}
}