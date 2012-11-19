namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Abstract class used to be implemented by classes SendToController and SendToSimulator.
	/// </summary>
	abstract class SendTo
	{
		protected string address = "127.0.0.1";		// String used to contain IP address of localhost.
		protected int port = 1337;					// Integer used to contain leet port number.
		protected Strings strings = new Strings();	// Strings used to store various strings used in the GUI.

		/// <summary>
		/// Sends message.
		/// </summary>
		/// <param name="message">String used to contain message.</param>
		/// <returns>String used to contain returned message.</returns>
		public abstract string Send(string message);
	}
}