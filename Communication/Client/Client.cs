using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using KruispuntGroep6.Communication.Json;

namespace KruispuntGroep6.Communication.Client
{
	/// <summary>
	/// Client class of Simulator used to handle inputs, sends messages to the controller and shows simulator logs.
	/// </summary>
	public partial class Client : Form
	{
		private string address;	// String used to contain the IP address of the internet connection.
		private Button btnClear, btnConnect, btnDisconnect, btnJsonGenerator, btnSend;	// Buttons used to contain buttons.
		private ComboBox cbJsonType;	// ComboBox used to contain the three JSON types that can be generated.
		private int connectionAttempts;	// Integer used to contain the current connection attempt number.
		private static bool disconnected;	// Boolean used in the test session.
		private bool hasRealAddress;	// Boolean used to contain the server address that the user has put in.
		private string[] inputJSON;		// String array used to contain all lines of the JSON input data file.
		private int inputJSONnumber;	// Integer used to contain the current input JSON number.
		private JsonGenerator jsonGenerator = new JsonGenerator();	// JsonGenerator used to generate JSON input data file.
		private Label lblConStatusValue;	// Label used to contain the connection status value.
		private ListBox lbResults;	// ListBox used to contain messages to keep the user informed.
		private int previousInputJSONnumber; // Integer used to contain the previous input JSON number.
		private int previousTime;	// Integer used to contain the previous input JSON time value.
		private static string realAddress;	// String used to contain the server address to be used by Simulator via Program.
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private TcpClient tcpClient;	// TcpClient used to contain the heart of this class: the TCP client.
		private TextBox tbAddress, tbJsonGenerator, tbNewText; // TextBoxes used to contain boxes to input text.
		private Thread thrConnect, thrReadForever; // Threads used to contain functions that needs to be executed at the same time.
		private System.Timers.Timer timerConnection, timerJson; // Timers used to contain a stopwatch.

		public Client()
		{
			this.InitializeComponent();

			// Generate GUI
			btnClear = new Button();
			btnClear.Click += new EventHandler(btnClear_Click);
			btnClear.Enabled = false;
			btnClear.Location = new Point(6 * Font.Height - 3, 7 * Font.Height - 6);
			btnClear.Parent = this;
			btnClear.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnClear.Text = strings.Clear;

			btnConnect = new Button();
			btnConnect.Click += new EventHandler(btnConnect_Click);
			btnConnect.Location = new Point(17 * Font.Height - 3, Font.Height - 6);
			btnConnect.Parent = this;
			btnConnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnConnect.Text = strings.Connect;

			btnDisconnect = new Button();
			btnDisconnect.Click += new EventHandler(btnDisconnect_Click);
			btnDisconnect.Enabled = false;
			btnDisconnect.Location = new Point(24 * Font.Height - 3, Font.Height - 6);
			btnDisconnect.Parent = this;
			btnDisconnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnDisconnect.Text = strings.Disconnect;

			btnJsonGenerator = new Button();
			btnJsonGenerator.Click += new EventHandler(btnJsonGenerator_Click);
			btnJsonGenerator.Location = new Point(21 * Font.Height - 3, 10 * Font.Height - 6);
			btnJsonGenerator.Parent = this;
			btnJsonGenerator.Size = new Size(9 * Font.Height, 2 * Font.Height);
			btnJsonGenerator.Text = strings.GenerateJSON;

			btnSend = new Button();
			btnSend.Click += new EventHandler(btnSend_Click);
			btnSend.Enabled = false;
			btnSend.Location = new Point(25 * Font.Height - 3, 7 * Font.Height - 6);
			btnSend.Parent = this;
			btnSend.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnSend.Text = strings.Send;

			cbJsonType = new ComboBox();
			cbJsonType.Items.Add(strings.JsonTypeInput);
			cbJsonType.Items.Add(strings.JsonTypeDetector);
			cbJsonType.Items.Add(strings.JsonTypeStoplight);
			cbJsonType.Location = new Point(14 * Font.Height, 10 * Font.Height - 3);
			cbJsonType.Parent = this;
			cbJsonType.Size = new Size(6 * Font.Height, 2 * Font.Height);
			cbJsonType.Text = strings.JsonTypeInput;

			this.FormBorderStyle = FormBorderStyle.FixedSingle;

			Label lblAddress = new Label();
			lblAddress.AutoSize = true;
			lblAddress.Location = new Point(Font.Height - 3, Font.Height);
			lblAddress.Parent = this;
			lblAddress.Text = strings.Address;

			Label lblConStatus = new Label();
			lblConStatus.AutoSize = true;
			lblConStatus.Location = new Point(Font.Height - 3, 4 * Font.Height);
			lblConStatus.Parent = this;
			lblConStatus.Text = strings.ConnectionStatus;

			lblConStatusValue = new Label();
			lblConStatusValue.AutoSize = true;
			lblConStatusValue.Location = new Point(9 * Font.Height - 3, 4 * Font.Height);
			lblConStatusValue.Parent = this;
			lblConStatusValue.Text = strings.LabelDisconnected;

			Label lblJsonGenerator = new Label();
			lblJsonGenerator.AutoSize = true;
			lblJsonGenerator.Location = new Point(Font.Height - 3, 10 * Font.Height);
			lblJsonGenerator.Parent = this;
			lblJsonGenerator.Text = strings.JsonGenerator;

			Label lblNewText = new Label();
			lblNewText.AutoSize = true;
			lblNewText.Location = new Point(Font.Height - 3, 7 * Font.Height);
			lblNewText.Parent = this;
			lblNewText.Text = strings.NewText;

			lbResults = new ListBox();
			lbResults.Location = new Point(Font.Height - 3, 12 * Font.Height);
			lbResults.Parent = this;
			lbResults.Size = new Size(29 * Font.Height, 22 * Font.Height);
			lbResults.TabStop = false;

			this.MaximizeBox = false;

			this.MessageChanged += new MessageChangedHandler(ChangeMessage);

			this.Size = new Size(31 * Font.Height, 36 * Font.Height);

			// Set address
			SetAddress();

			tbAddress = new TextBox();
			tbAddress.Location = new Point(9 * Font.Height, Font.Height - 3);
			tbAddress.Parent = this;
			tbAddress.Size = new Size(7 * Font.Height, 2 * Font.Height);
			tbAddress.Text = address;

			tbJsonGenerator = new TextBox();
			tbJsonGenerator.Location = new Point(9 * Font.Height, 10 * Font.Height - 3);
			tbJsonGenerator.MaxLength = 5;
			tbJsonGenerator.Parent = this;
			tbJsonGenerator.Size = new Size(4 * Font.Height, 2 * Font.Height);

			tbNewText = new TextBox();
			tbNewText.Enabled = false;
			tbNewText.Location = new Point(12 * Font.Height, 7 * Font.Height - 3);
			tbNewText.Parent = this;
			tbNewText.Size = new Size(12 * Font.Height, 2 * Font.Height);
			tbNewText.TextChanged += new EventHandler(tbNewText_TextChanged);

			this.Text = strings.TcpClient;
		}

		/// <summary>
		/// Read JSON input file.
		/// </summary>
		private bool readJSON()
		{
			bool success = true;

			try
			{
				// if inputJSON isn't born yet, setting this boolean false forces
				// reading the json input file and resetting the input json number
				bool inputFilesAreEqual = false;

				// if inputJSON exists, set the real value of inputFilesAreEqual
				if (!Object.Equals(inputJSON, null))
				{
					inputFilesAreEqual = Enumerable.SequenceEqual(File.ReadAllLines(
						strings.JsonTypeInput + strings.JsonInputFileExtension), inputJSON);
				}

				//when the user generates a new json input file,
				//and the client handles the previous file,
				//abort that file and make the new file his
				if (!inputFilesAreEqual)
				{
					inputJSON = File.ReadAllLines(strings.JsonTypeInput + strings.JsonInputFileExtension);
					inputJSONnumber = 0;
				}

				//reset current input json number,
				//if the whole JSON input file is sent already
				if (inputJSONnumber >= inputJSON.Length)
				{
					inputJSONnumber = 0;
				}

				// if the whole JSON input file isn't sent already,
				// continue with the inputJSONnumber from before the
				// connection loss or disconnect
				if (inputJSONnumber > 0)
					previousInputJSONnumber = inputJSONnumber;
			}
			catch (FileNotFoundException)
			{
				OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonReadingError1);
				OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonReadingError2);
				success = false;
				Disconnect();
			}

			return success;
		}

		private delegate void MessageChangedHandler(object sender, Type type, string msg, bool visable);

		private event MessageChangedHandler MessageChanged;

		/// <summary>
		/// Clears input text.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnClear_Click(object sender, EventArgs e)
		{
			btnClear.Enabled = false;
			tbNewText.Text = string.Empty;
		}

		/// <summary>
		/// Connects to controller, step 1.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnConnect_Click(object sender, EventArgs e)
		{
			btnConnect.Enabled = false;
			tbAddress.Enabled = false;
			lblConStatusValue.Text = strings.Connecting;

			timerConnection = new System.Timers.Timer(5000);
			timerConnection.Elapsed += new ElapsedEventHandler(timerConnection_Elapsed);
			timerConnection.Start();

			// Get address from textbox
			address = tbAddress.Text;

			realAddress = address;

			if (!hasRealAddress)
			{
				// Wait till Program has the real address
				Thread.Sleep(555);
				hasRealAddress = true;
			}

			// Try to connect in the background...
			thrConnect = new Thread(new ThreadStart(Connect));
			thrConnect.Start();

			// Attempt to connect, because we don't want to wait for the timer.
			ConnectAttempt();
		}

		/// <summary>
		/// Disconnects from controller.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			if (!Thread.Equals(thrReadForever, null))
			{
				thrReadForever.Abort();
			}
			if (!System.Timers.Timer.Equals(timerJson, null))
			{
				timerJson.Stop();
			}
			if (Int32.Equals(connectionAttempts, 0))
			{
				SendToController(strings.Exit);
				Disconnect();
			}
			else
			{
				DisconnectWhileConnecting();
			}

			disconnected = true;
		}

		/// <summary>
		/// Generates JSON input file.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnJsonGenerator_Click(object sender, EventArgs e)
		{
			int nrOfInputs = -1;

			bool onlyNumbers = true;

			if (!string.Equals(tbJsonGenerator.Text, string.Empty))
			{
				foreach (char c in tbJsonGenerator.Text)
				{
					if (!Char.IsNumber(c))
						onlyNumbers = false;
				}

				if (onlyNumbers)
					nrOfInputs = Int32.Parse(tbJsonGenerator.Text);
			}

			if (nrOfInputs > 0)
			{
				for (int i = 0; i < nrOfInputs; i++)
				{
					jsonGenerator.GenerateJSON(cbJsonType.SelectedItem.ToString(), i, nrOfInputs);
				}

				if (jsonGenerator.SaveJSONFile(cbJsonType.SelectedItem.ToString()))
				{
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSaved +
						cbJsonType.SelectedItem.ToString() + strings.JsonInputFileExtension);
				}
				else
				{
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSavingError +
						cbJsonType.SelectedItem.ToString() + strings.JsonInputFileExtension);
				}
			}
		}

		/// <summary>
		/// Sends a message to the controller, step 1.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnSend_Click(object sender, EventArgs e)
		{
			SendToController(tbNewText.Text);
		}

		private void ChangeMessage(object sender, Type type, string text, bool enabled)
		{
			switch (type.Name)
			{
				case "Button":
					if (!text.Equals(string.Empty))
					{
						(sender as Button).Text = text;
					}
					(sender as Button).Enabled = enabled;
					break;
				case "ComboBox":
					(sender as ComboBox).Enabled = enabled;
					break;
				case "Label":
					if (!text.Equals(string.Empty))
					{
						(sender as Label).Text = text;
					}
					(sender as Label).Enabled = enabled;
					break;
				case "ListBox":
					if (!text.Equals(string.Empty))
					{
						(sender as ListBox).Items.Add(text);

						// Autoscroll. Source: http://www.csharp-examples.net/autoscroll
						(sender as ListBox).SelectedIndex = this.lbResults.Items.Count - 1;
						(sender as ListBox).SelectedIndex = -1;
					}

					(sender as ListBox).Enabled = enabled;
					break;
				case "TextBox":
					if (!text.Equals(string.Empty))
					{
						(sender as TextBox).Text = text;
					}
					(sender as TextBox).Enabled = enabled;
					break;
				default:
					throw new Exception(string.Format("Type {0} wordt niet herkend!", type.Name));
			}
		}

		private void Connect()
		{
			try
			{
				tcpClient = new TcpClient();
				tcpClient.Connect(IPAddress.Parse(address), strings.Port);
			}
			catch (SocketException)
			{
				if (connectionAttempts > 8)
				{
					OnMessageChanged(lbResults, lbResults.GetType(), strings.ConnectionError);
					DisconnectWhileConnecting();
				}
			}

			if (tcpClient.Connected)
			{
				Connected();
			}
		}

		private void ConnectAttempt()
		{
			// Get address from textbox
			address = tbAddress.Text;

			// Try to connect in the background...
			thrConnect.Abort();
			thrConnect = new Thread(new ThreadStart(Connect));
			thrConnect.Start();

			connectionAttempts++;

			OnMessageChanged(lbResults, lbResults.GetType(),
				string.Format(strings.ConnectionAttempt, connectionAttempts + strings.With + address));
		}

		// Connected to controller.
		private void Connected()
		{
			//reset disconnected
			disconnected = false;
			//reset connection attempts
			connectionAttempts = 0;
			//reset previous input json number
			previousInputJSONnumber = 0;
			//stop connection timer
			timerConnection.Stop();
			//create welcome message
			string message = strings.HiIAmSimulator;
			//send this message to controller
			SendToController(message);

			// Update GUI
			OnMessageChanged(btnConnect, btnConnect.GetType(), enabled: false);
			OnMessageChanged(lblConStatusValue, lblConStatusValue.GetType(),
				string.Format(strings.Connected, tcpClient.Client.RemoteEndPoint.ToString()));
			OnMessageChanged(btnDisconnect, btnDisconnect.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType());

			// Read JSON input file and if that's a success, send start time to controller
			if (readJSON())
			{
				// Disable JSON generator GUI, to prevent bugs
				OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType(), enabled: false);
				OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType(), enabled: false);
				OnMessageChanged(cbJsonType, cbJsonType.GetType(), enabled: false);

				SendStartTime();

				// Read messages forever
				thrReadForever = new Thread(new ThreadStart(ReadForever));
				thrReadForever.Start();
			}

			//abort connect thread
			thrConnect.Abort();
		}

		private void Disconnect()
		{
			// Update GUI
			OnMessageChanged(tbAddress, tbAddress.GetType(), address);
			OnMessageChanged(btnClear, btnClear.GetType(), enabled: false);
			OnMessageChanged(btnConnect, btnConnect.GetType());
			OnMessageChanged(lblConStatusValue, lblConStatusValue.GetType(), strings.LabelDisconnected);
			OnMessageChanged(btnDisconnect, btnDisconnect.GetType(), enabled: false);
			OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType());
			OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType());
			OnMessageChanged(cbJsonType, cbJsonType.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType(), enabled: false);
			OnMessageChanged(btnSend, btnSend.GetType(), enabled: false);
		}

		private void DisconnectWhileConnecting()
		{
			OnMessageChanged(lbResults, lbResults.GetType(), strings.ConnectingAborted);
			connectionAttempts = 0;
			timerConnection.Stop();
			Disconnect();
		}

		private int GetTime(string strJson)
		{
			strJson = strJson.Substring(strJson.IndexOf("{"), strJson.IndexOf("}") - strJson.IndexOf("{") + 1);
			var json = DynamicJson.Parse(strJson);
			return int.Parse(json.time);
		}

		/// <summary>
		/// Reads messages from controller, forever.
		/// </summary>
		private void ReadForever()
		{
			//create our StreamReader Object, based on the current NetworkStream
			StreamReader reader = new StreamReader(tcpClient.GetStream());
			// Begins to listen for incoming connection attempts from controller or simulator.
			try
			{
				while (true)
				{
					string message = reader.ReadLine();

					if (!string.Equals(message, null))
					{
						if (!string.Equals(message, string.Empty))
						{
							OnMessageChanged(lbResults, lbResults.GetType(), string.Format(strings.Received, message));
						}
					}
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private void SendJSONObject()
		{
			int time = 0;

			if (previousInputJSONnumber > 0)
			{
				inputJSONnumber = previousInputJSONnumber;
				previousInputJSONnumber = 0;
			}

			if (timerJson.Enabled)
			{
				time = GetTime(inputJSON[inputJSONnumber]);
			}

			while (time.Equals(previousTime))
			{
				SendToController(inputJSON[inputJSONnumber]);

				inputJSONnumber++;

				if (inputJSONnumber < inputJSON.Length)
				{
					time = GetTime(inputJSON[inputJSONnumber]);
				}
				else
				{
					timerJson.Stop();
					break;
				}
			}

			if (inputJSONnumber < inputJSON.Length)
			{
				previousTime = GetTime(inputJSON[inputJSONnumber]);
			}
			else
			{
				timerJson.Stop();
				OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSent);

				// Enable generation of JSON file
				OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType());
				OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType());
				OnMessageChanged(cbJsonType, cbJsonType.GetType());
			}
		}

		/// <summary>
		/// Sends start time to controller.
		/// </summary>
		private void SendStartTime()
		{
			string jsonStartTime = DynamicJson.Serialize(new { starttime = DateTime.UtcNow.ToString(strings.DateTimeFormat) });

			if (Int32.Equals(previousInputJSONnumber, 0))
			{
				SendToController(jsonStartTime);
			}

			timerJson = new System.Timers.Timer(1000);
			timerJson.Elapsed += new ElapsedEventHandler(timerJson_Elapsed);
			timerJson.Start();

			// Send the first JSON, because we don't want to wait for the timer.
			SendJSONObject();
		}

		/// <summary>
		/// Sends a message to the controller, step 2.
		/// </summary>
		/// <param name="message">String used to determine the message to be send.</param>
		private void SendToController(string message)
		{
			Send send2Controller = new Send();
			send2Controller.SendMessage(tcpClient, message);

			// Show sent data in results list.
			OnMessageChanged(lbResults, lbResults.GetType(), string.Format(strings.Sent, message));
		}

		private void SetAddress()
		{
			// Get internet IP address
			address = string.Empty;
			IPHostEntry host;
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily.ToString().Equals(strings.Internet))
				{
					address = ip.ToString();
				}
			}
			// If there is no internet, use localhost
			if (address.Equals(string.Empty))
			{
				address = strings.Localhost;
			}
		}

		/// <summary>
		/// Update GUI buttons Clear and Send if the input text changes.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void tbNewText_TextChanged(object sender, EventArgs e)
		{
			int length = (sender as TextBox).Text.Length;

			if (length > 0)
			{
				// Update GUI
				btnClear.Enabled = true;
				btnSend.Enabled = true;
			}
			else
			{
				// Update GUI
				btnClear.Enabled = false;
				btnSend.Enabled = false;
			}
		}

		private void timerConnection_Elapsed(object sender, ElapsedEventArgs e)
		{
			ConnectAttempt();

			// Update GUI
			OnMessageChanged(btnDisconnect, btnDisconnect.GetType());
		}

		/// <summary>
		/// Sends a JSON every second.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">ElapsedEventArgs used to determine elapsed event arguments.</param>
		private void timerJson_Elapsed(object sender, ElapsedEventArgs e)
		{
			SendJSONObject();
		}

		protected virtual void OnMessageChanged(object sender, Type type, string text = "", bool enabled = true)
		{
			Control target = MessageChanged.Target as Control;
			if (target != null && target.InvokeRequired)
				target.Invoke(MessageChanged, new object[] { sender, type, text, enabled });
			else
				ChangeMessage(sender, type, text, enabled);
		}

		public static string GetAddress()
		{
			if (!object.Equals(realAddress, null))
			{
				return realAddress;
			}
			else
			{
				return string.Empty;
			}
		}

		public static bool GetDisconnected()
		{
			if (disconnected)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}