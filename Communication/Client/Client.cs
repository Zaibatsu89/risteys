using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using KruispuntGroep6.Communication.Json;

namespace KruispuntGroep6.Communication.Client
{
	/// <summary>
	/// Client class of XNASimulator used to handle inputs, sends messages to the controller and shows simulator logs.
	/// </summary>
	public partial class Client : Form
	{
		private BackgroundWorker backgroundWorker;
		private delegate void backgroundWorker_Handler(Tuple<string, dynamic> argument);
		private Button btnClear, btnConnect, btnDisconnect, btnJsonGenerator, btnSend;
		private byte[] byteData = new byte[1024];
		private int connectionAttempts;
		private string[] inputJSON;
		private int inputJSONnumber;
		private JsonGenerator jsonGenerator = new JsonGenerator();
		private ListBox lbResults;
		private int previousInputJSONnumber;
		private int previousTime;
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private static TcpClient tcpClient;
		private TextBox tbConStatus, tbJsonGenerator, tbNewText;
		private Thread thrReadForever;
		private System.Timers.Timer timerConnection;
		private System.Timers.Timer timerJson;

		public Client()
		{
			this.InitializeComponent();

			// Initialize background worker
			backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);

			// Generate GUI
			btnClear = new Button();
			btnClear.Enabled = false;
			btnClear.Parent = this;
			btnClear.Text = strings.Clear;
			btnClear.Location = new Point(210, 20);
			btnClear.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnClear.Click += new EventHandler(btnClear_Click);
			btnClear.TabStop = false;

			btnConnect = new Button();
			btnConnect.Parent = this;
			btnConnect.Text = strings.Connect;
			btnConnect.Location = new Point(285, 20);
			btnConnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnConnect.Click += new EventHandler(btnConnect_Click);
			btnConnect.TabStop = false;

			btnDisconnect = new Button();
			btnDisconnect.Enabled = false;
			btnDisconnect.Parent = this;
			btnDisconnect.Text = strings.Disconnect;
			btnDisconnect.Location = new Point(285, 52);
			btnDisconnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnDisconnect.Click += new EventHandler(btnDisconnect_Click);
			btnDisconnect.TabStop = false;

			btnJsonGenerator = new Button();
			btnJsonGenerator.Parent = this;
			btnJsonGenerator.Text = strings.GenerateJSON;
			btnJsonGenerator.Location = new Point(60, 382);
			btnJsonGenerator.Size = new Size(14 * Font.Height, 2 * Font.Height);
			btnJsonGenerator.Click += new EventHandler(btnJsonGenerator_Click);
			btnJsonGenerator.TabIndex = 3;

			btnSend = new Button();
			btnSend.Enabled = false;
			btnSend.Parent = this;
			btnSend.Text = strings.Send;
			btnSend.Location = new Point(210, 52);
			btnSend.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnSend.Click += new EventHandler(btnSend_Click);
			btnSend.TabIndex = 1;

			this.FormBorderStyle = FormBorderStyle.FixedSingle;

			Label lblConStatus = new Label();
			lblConStatus.AutoSize = true;
			lblConStatus.Location = new Point(10, 330);
			lblConStatus.Parent = this;
			lblConStatus.Text = strings.ConnectionStatus;

			Label lblJsonGenerator = new Label();
			lblJsonGenerator.AutoSize = true;
			lblJsonGenerator.Location = new Point(10, 360);
			lblJsonGenerator.Parent = this;
			lblJsonGenerator.Text = strings.JsonGenerator;

			Label lblNewText = new Label();
			lblNewText.AutoSize = true;
			lblNewText.Location = new Point(10, 30);
			lblNewText.Parent = this;
			lblNewText.Text = strings.NewText;

			lbResults = new ListBox();
			lbResults.Location = new Point(10, 85);
			lbResults.Parent = this;
			lbResults.Size = new Size(360, 18 * Font.Height);
			lbResults.TabStop = false;

			this.MaximizeBox = false;

			this.Size = new Size(385, 455);

			tbConStatus = new TextBox();
			tbConStatus.Location = new Point(110, 325);
			tbConStatus.Parent = this;
			tbConStatus.Size = new Size(200, 2 * Font.Height);
			tbConStatus.TabStop = false;
			tbConStatus.Text = strings.Disconnected;

			tbJsonGenerator = new TextBox();
			tbJsonGenerator.Location = new Point(10, 385);
			tbJsonGenerator.MaxLength = 5;
			tbJsonGenerator.Parent = this;
			tbJsonGenerator.Size = new Size(38, 2 * Font.Height);
			tbJsonGenerator.TabIndex = 2;

			tbNewText = new TextBox();
			tbNewText.Enabled = false;
			tbNewText.Location = new Point(10, 55);
			tbNewText.Parent = this;
			tbNewText.Size = new Size(190, 2 * Font.Height);
			tbNewText.TabIndex = 0;
			tbNewText.TextChanged += new EventHandler(tbNewText_TextChanged);

			this.Text = strings.TcpClient;
		}

		/// <summary>
		/// Clears input text.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnClear_Click(object sender, EventArgs e)
		{
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonClear, false));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxNewText,
				string.Empty));
		}

		/// <summary>
		/// Connects to controller, step 1.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnConnect_Click(object sender, EventArgs e)
		{
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxConStatus,
				strings.Connecting));
			
			tcpClient = new TcpClient();

			timerConnection = new System.Timers.Timer(5000);
			timerConnection.Elapsed += new ElapsedEventHandler(timerConnection_Elapsed);
			timerConnection.Start();

			// Try to connect.
			Connect();
		}

		private void timerConnection_Elapsed(object sender, ElapsedEventArgs e)
		{
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
				string.Format(strings.ConnectionAttempt, connectionAttempts + 1)));

			// Try to connect.
			Connect();

			connectionAttempts++;

			// Update GUI
			if (connectionAttempts > 0)
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonDisconnect, true));
		}

		/// <summary>
		/// Tries to connect to controller.
		/// </summary>
		private void Connect()
		{
			// Connect in the background...
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.Connect, tcpClient));
		}

		private void DisconnectWhileConnecting()
		{
			connectionAttempts = 0;
			timerConnection.Stop();
			Disconnect();
		}

		// Connected to controller.
		private void Connected()
		{
			//reset connection attempts
			connectionAttempts = 0;
			//reset previous input json number
			previousInputJSONnumber = 0;
			//stop connection timer
			timerConnection.Stop();
			//create message
			string message = strings.HiIAmSimulator;
			//create a StreamWriter based on the current NetworkStream
			StreamWriter writer = new StreamWriter(tcpClient.GetStream());
			//write our message
			writer.WriteLine(message);
			//ensure the buffer is empty
			writer.Flush();

			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
				string.Format(strings.Sent, message)));

			// Update GUI
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonConnect, false));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxConStatus,
				strings.Connected + tcpClient.Client.RemoteEndPoint.ToString()));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonDisconnect, true));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.BooleanNewText, true));

			// Read JSON input file and if that's a success, send start time to controller
			if (readJSON())
			{
				// Disable JSON generator GUI, to prevent bugs
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.BooleanJsonGenerator, false));
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonJsonGenerator, false));
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxJsonGenerator,
					string.Empty));
				
				sendStartTime();

				// Read messages forever
				thrReadForever = new Thread(new ThreadStart(ReadForever));
				thrReadForever.Start();
			}
		}

		/// <summary>
		/// Read JSON input file.
		/// </summary>
		private bool readJSON()
		{
			bool success = true;

			try
			{
				//don't read the inputJSON file, if it is already read earlier
				if (Object.Equals(inputJSON, null))
					inputJSON = File.ReadAllLines(strings.JsonInputFilePath);

				//reset current input json number, if the whole JSON input file is sent already
				if (inputJSONnumber >= inputJSON.Length)
				{
					inputJSONnumber = 0;
					previousTime = 0;
				}

				// if the whole JSON input file isn't sent already,
				// continue with the inputJSONnumber from before the
				// connection loss or disconnect
				if (inputJSONnumber > 0)
					previousInputJSONnumber = inputJSONnumber;
			}
			catch (FileNotFoundException)
			{
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
					strings.JsonReadingError1));
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
					strings.JsonReadingError2));
				success = false;
				Disconnect();
			}

			return success;
		}

		/// <summary>
		/// Sends start time to controller.
		/// </summary>
		private void sendStartTime()
		{
			string jsonStartTime = DynamicJson.Serialize(new{starttime = DateTime.UtcNow.ToString(strings.DateTimeFormat)});

			if (Int32.Equals(previousInputJSONnumber, 0))
			{
				SendToController(jsonStartTime);
			}

			timerJson = new System.Timers.Timer(1000);
			timerJson.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timerJson.Start();

			// Send the first JSON, because we don't want to wait for the timer.
			SendJSONObject();
		}

		/// <summary>
		/// Sends a JSON every second.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">ElapsedEventArgs used to determine elapsed event arguments.</param>
		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			SendJSONObject();
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
				time = (int)DynamicJson.Parse(inputJSON[inputJSONnumber]).time;
			}

			while (time.Equals(previousTime))
			{
				SendToController(inputJSON[inputJSONnumber]);

				inputJSONnumber++;

				if (inputJSONnumber < inputJSON.Length)
				{
					time = (int)DynamicJson.Parse(inputJSON[inputJSONnumber]).time;
				}
				else
				{
					timerJson.Stop();
					break;
				}
			}

			if (inputJSONnumber < inputJSON.Length)
			{
				previousTime = (int)DynamicJson.Parse(inputJSON[inputJSONnumber]).time;
			}
			else
			{
				timerJson.Stop();
			}
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
							backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
								string.Format(strings.Received, message)));
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

		/// <summary>
		/// Disconnects from controller.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			if (!Thread.Equals(thrReadForever, null))
				thrReadForever.Abort();
			if (!System.Timers.Timer.Equals(timerJson, null))
				timerJson.Stop();
			if (Int32.Equals(connectionAttempts, 0))
			{
				SendToController(strings.Exit);
				Disconnect();
			}
			else
			{
				DisconnectWhileConnecting();
			}
		}

		private void Disconnect()
		{
			// Update GUI
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonClear, false));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonConnect, true));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxConStatus,
				strings.Disconnected));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonDisconnect, false));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.BooleanJsonGenerator, true));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonJsonGenerator, true));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxJsonGenerator,
				string.Empty));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.BooleanNewText, false));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.TextBoxNewText,
				string.Empty));
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonSend, false));
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
					jsonGenerator.GenerateJSON(i, nrOfInputs);
				}

				if (jsonGenerator.SaveJSONFile())
				{
					backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
						strings.JsonSaved));
				}
				else
				{
					backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
						strings.JsonSavingError));
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

		/// <summary>
		/// Sends a message to the controller, step 2.
		/// </summary>
		/// <param name="message">String used to determine the message to be send.</param>
		private void SendToController(string message)
		{
			Send send2Controller = new Send();
			send2Controller.SendMessage(tcpClient, message);

			// Show sent data in results list.
			backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
				string.Format(strings.Sent, message)));
		}

		/// <summary>
		/// The background worker is doing work, step 1.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">DoWorkEventArgs used to determine do work event arguments.</param>
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			backgroundWorker_Worker((Tuple<string, dynamic>) e.Argument);
		}

		/// <summary>
		/// Creates a new background worker.
		/// </summary>
		/// <param name="argument">Tuple<string, dynamic> used to determine
		///	the GUI object and it's argument</param>
		private void backgroundWorker_Set(Tuple<string, dynamic> argument)
		{
			backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
			if (!backgroundWorker.IsBusy)
			{
				backgroundWorker.RunWorkerAsync(argument);
			}
		}

		/// <summary>
		/// The background worker is doing work, step 2.
		/// </summary>
		/// <param name="argument">Tuple<string, dynamic> used to determine
		///	the GUI object and it's argument</param>
		private void backgroundWorker_Worker(Tuple<string, dynamic> argument)
		{
			switch (argument.Item1)
			{
				case "Connect":
					try
					{
						tcpClient.Connect(IPAddress.Parse(strings.Address), strings.Port);
					}
					catch (SocketException)
					{
						if (connectionAttempts > 11)
						{
							backgroundWorker_Set(new Tuple<string, dynamic>(strings.ListBoxResults,
								strings.ConnectionError));
							DisconnectWhileConnecting();
						}
					}

					if (tcpClient.Connected)
						Connected();
					break;
				case "btnClear":
					if (this.btnClear.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.btnClear.Enabled = argument.Item2;
					break;
				case "btnConnect":
					if (this.btnConnect.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.btnConnect.Enabled = argument.Item2;
					break;
				case "tbConStatus":
					if (this.tbConStatus.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.tbConStatus.Text = argument.Item2;
					break;
				case "btnDisconnect":
					if (this.btnDisconnect.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.btnDisconnect.Enabled = argument.Item2;
					break;
				case "boolJsonGenerator":
					if (this.tbJsonGenerator.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.tbJsonGenerator.Enabled = argument.Item2;
					break;
				case "btnJsonGenerator":
					if (this.btnJsonGenerator.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.btnJsonGenerator.Enabled = argument.Item2;
					break;
				case "tbJsonGenerator":
					if (this.tbJsonGenerator.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.tbJsonGenerator.Text = argument.Item2;
					break;
				case "boolNewText":
					if (this.tbNewText.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.tbNewText.Enabled = argument.Item2;
					break;
				case "tbNewText":
					if (this.tbNewText.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.tbNewText.Text = argument.Item2;
					break;
				case "lbResults":
					if (this.lbResults.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.lbResults.Items.Add(argument.Item2);

					// Autoscroll. Source: http://www.csharp-examples.net/autoscroll
					this.lbResults.SelectedIndex = this.lbResults.Items.Count - 1;
					this.lbResults.SelectedIndex = -1;
					break;
				case "btnSend":
					if (this.btnSend.InvokeRequired)
					{
						this.Invoke(new backgroundWorker_Handler(this.backgroundWorker_Worker), argument);
						return;
					}

					this.btnSend.Enabled = argument.Item2;
					break;
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
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonClear, true));
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonSend, true));
			}
			else
			{
				// Update GUI
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonClear, false));
				backgroundWorker_Set(new Tuple<string, dynamic>(strings.ButtonSend, false));
			}
		}
	}
}