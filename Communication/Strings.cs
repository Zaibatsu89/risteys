namespace KruispuntGroep6.Communication
{
	/// <summary>
	/// Class used to store various strings used in the GUI. Handy when multiple languages needs to be supported.
	/// </summary>
	class Strings
	{
		public string Address
		{
			get { return "127.0.0.1"; }
		}

		public string BooleanJsonGenerator
		{
			get { return "boolJsonGenerator"; }
		}

		public string BooleanNewText
		{
			get { return "boolNewText"; }
		}

		public string Broken
		{
			get { return "The connection is broken!"; }
		}

		public string ButtonClear
		{
			get { return "btnClear"; }
		}

		public string ButtonConnect
		{
			get { return "btnConnect"; }
		}

		public string ButtonDisconnect
		{
			get { return "btnDisconnect"; }
		}

		public string ButtonJsonGenerator
		{
			get { return "btnJsonGenerator"; }
		}

		public string ButtonSend
		{
			get { return "btnSend"; }
		}

		public string Clear
		{
			get { return "Clear"; }
		}

		public string Connect
		{
			get { return "Connect"; }
		}

		public string Connected
		{
			get { return "Connected: "; }
		}

		public string Connecting
		{
			get { return "Connecting..."; }
		}

		public string ConnectionAttempt
		{
			get { return "Connecting attempt {0}..."; }
		}

		public string ConnectionError
		{
			get { return "Connection error"; }
		}

		public string ConnectionStatus
		{
			get { return "Connection status"; }
		}

		public string DateTimeFormat
		{
			get { return "HH:mm"; }
		}

		public string Disconnect
		{
			get { return "Disconnect"; }
		}

		public string Disconnected
		{
			get { return "Disconnected"; }
		}

		public string Exception
		{
			get { return "Exception: {0}"; }
		}

		public string Exit
		{
			get { return "exit"; }
		}

		public string GenerateJSON
		{
			get { return "Generate JSON input data file"; }
		}

		public string HiIAmController
		{
			get { return "Hi, I am controller"; }
		}

		public string HiIAmSimulator
		{
			get { return "Hi, I am simulator"; }
		}

		public string IOException
		{
			get { return "IOException: {0}"; }
		}

		public string JsonGenerator
		{
			get { return "How many inputs?"; }
		}

		public string JsonInputFilePath
		{
			get { return @"..\..\..\input.json"; }
		}

		public string JsonReadingError1
		{
			get { return "Couldn't find JSON input file."; }
		}

		public string JsonReadingError2
		{
			get { return "Did you already generate one below with specified amount of inputs?"; }
		}

		public string JsonSaved
		{
			get { return "JSON file saved as input.json"; }
		}

		public string JsonSavingError
		{
			get { return "Error saving JSON file as input.json"; }
		}

		public string ListBoxResults
		{
			get { return "lbResults"; }
		}

		public string NewText
		{
			get { return "Enter text string:"; }
		}

		public int Port
		{
			get { return 1337; }
		}

		public string Received
		{
			get { return "Received: {0}"; }
		}

		public string Send
		{
			get { return "Send"; }
		}

		public string Sent
		{
			get { return "Sent: {0}"; }
		}

		public string SimulatorDisconnected
		{
			get { return "The simulator has disconnected"; }
		}

		public string SocketException
		{
			get { return "SocketException: {0}"; }
		}

		public string Stopped
		{
			get { return "\nThe controller stopped. Press <enter> to continue."; }
		}

		public string TextBoxConStatus
		{
			get { return "tbConStatus"; }
		}

		public string TextBoxJsonGenerator
		{
			get { return "tbJsonGenerator"; }
		}

		public string TextBoxNewText
		{
			get { return "tbNewText"; }
		}

		public string TcpClient
		{
			get { return "Kruispunt Groep 6: TCP Client"; }
		}

		public string TcpServer
		{
			get { return "Kruispunt Groep 6: TCP Server"; }
		}

		public string Waiting
		{
			get { return "Waiting for simulator..."; }
		}
	}
}