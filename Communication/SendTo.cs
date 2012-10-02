namespace KruispuntGroep6.Communication
{
	abstract class SendTo
	{
		protected string address = "127.0.0.1";
		protected int port = 1337;

		public abstract string Send(string message);
	}
}