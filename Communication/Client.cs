using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace KruispuntGroep6.Communication
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
		private string[] inputJSON;
		private int inputJSONnumber;
		private int intPort = 1337;
		private IPAddress ipAddr = new IPAddress(0);
		private JsonGenerator jsonGenerator = new JsonGenerator();
		private ListBox lbResults;
		private int previousTime;
		private Strings strings = new Strings();
		private TcpClient tcpSimulator;
		private TextBox tbConStatus, tbJsonGenerator, tbNewText;
		private System.Timers.Timer timer = new System.Timers.Timer(1000);

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

			this.ipAddr = IPAddress.Parse(strings.Address);

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
			backgroundWorker_Set(new Tuple<string, dynamic>("btnClear", false));
			backgroundWorker_Set(new Tuple<string, dynamic>("tbNewText",
				string.Empty));
		}

		/// <summary>
		/// Connects to controller, step 1.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnConnect_Click(object sender, EventArgs e)
		{
			backgroundWorker_Set(new Tuple<string, dynamic>("tbConStatus",
				strings.Connecting));

			string message = strings.HiIAmSimulator;

			Connect(message);
		}

		/// <summary>
		/// Connects to controller, step 2.
		/// </summary>
		/// <param name="message">String used to determine the message to be received.</param>
		private void Connect(string message)
		{
			IPEndPoint iep = new IPEndPoint(ipAddr, intPort);
			// TcpClient used to provide a client connection for a TCP network service.
			tcpSimulator = new TcpClient();
			// Set timeouts
			tcpSimulator.ReceiveTimeout = 10000;
			tcpSimulator.SendTimeout = 10000;
			// String used to contain returned message from controller.
			string fromController = string.Empty;

			try
			{
				// Connects client to localhost and leet port.
				tcpSimulator.Connect(iep);
				// NetworkStream used to send and receive data.
				NetworkStream stream = tcpSimulator.GetStream();

				// If NetworkStream supports reading and writing.
				if (stream.CanWrite && stream.CanRead)
				{
					// Byte array used to contain message.
					byte[] bytes = Encoding.ASCII.GetBytes(message);

					// Write data to NetworkStream.
					stream.Write(bytes, 0, bytes.Length);

					// Show sent data in results list.
					backgroundWorker_Set(new Tuple<string, dynamic>("lbResults",
						"Sent: " + Encoding.ASCII.GetString(bytes)));

					// Reads the NetworkStream into a byte buffer
					byte[] buffer = new byte[tcpSimulator.ReceiveBufferSize];
					int bytesLength = stream.Read(buffer, 0, (int)tcpSimulator.ReceiveBufferSize);

					// Returns the data received from the simulator to the controller
					fromController = Encoding.ASCII.GetString(buffer, 0, bytesLength);

					// Update GUI
					backgroundWorker_Set(new Tuple<string, dynamic>("btnConnect", false));
					backgroundWorker_Set(new Tuple<string, dynamic>("tbConStatus",
						strings.Connected + tcpSimulator.Client.RemoteEndPoint.ToString()));
					backgroundWorker_Set(new Tuple<string, dynamic>("btnDisconnect", true));
					backgroundWorker_Set(new Tuple<string, dynamic>("boolJsonGenerator", true));
					backgroundWorker_Set(new Tuple<string, dynamic>("btnJsonGenerator", true));
					backgroundWorker_Set(new Tuple<string, dynamic>("boolNewText", true));
					backgroundWorker_Set(new Tuple<string, dynamic>("lbResults",
						"Received: " + fromController));

					// Close stream
					stream.Close();

					// Read JSON input file
					readJSON();

					// Send start time to controller
					sendStartTime();
				}
				else
					tcpSimulator.Close();
			}
			// Gonna catch 'em all... Pokémon!
			catch (SocketException e)
			{
				Console.WriteLine(String.Format(strings.SocketException, e.Message));
			}
			catch (System.IO.IOException e)
			{
				Console.WriteLine(String.Format(strings.IOException, e.Message));
			}
			catch (Exception e)
			{
				Console.WriteLine(String.Format(strings.Exception, e.Message));
			}
		}

		/// <summary>
		/// Read JSON input file.
		/// </summary>
		private void readJSON()
		{
			Console.Write("Reading JSON input file... ");

			inputJSON = File.ReadAllLines(@"..\..\..\input.json");
			inputJSONnumber = 0;

			Console.Write("Done.\n");
		}

		/// <summary>
		/// Sends start time to controller.
		/// </summary>
		private void sendStartTime()
		{
			string jsonStartTime = DynamicJson.Serialize(new{starttime = DateTime.UtcNow.ToString("HH:mm")});

			SendToController(jsonStartTime);

			previousTime = 0;
			timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timer.Start();
		}

		/// <summary>
		/// Sends a JSON every second.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">ElapsedEventArgs used to determine elapsed event arguments.</param>
		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			int time = (int)DynamicJson.Parse(inputJSON[inputJSONnumber]).time;

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
					timer.Stop();
					break;
				}
			}

			if (timer.Enabled)
			{
				SendToController(inputJSON[inputJSONnumber]);
				inputJSONnumber++;
			}

			if (inputJSONnumber < inputJSON.Length)
			{
				previousTime = (int)DynamicJson.Parse(inputJSON[inputJSONnumber]).time;
			}
			else
			{
				timer.Stop();
			}
		}

		/// <summary>
		/// Shows progress of items.
		/// </summary>
		/// <param name="current">Integer used to determine current item number.</param>
		/// <param name="count">Integer used to determine total amount of items.</param>
		private void showProgress(int current, int count)
		{
			int left = Console.CursorLeft;
			int top = Console.CursorTop;

			Console.CursorLeft += 3;
			Console.Write("%");
			Console.CursorLeft -= 4;
			Console.Write(Convert.ToInt32((float)current / (float)count * (float)100).ToString());
			Console.CursorLeft = left;
			Console.CursorTop = top;
		}

		/// <summary>
		/// Disconnects from controller.
		/// </summary>
		/// <param name="sender">object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			SendToController(strings.Exit);
			tcpSimulator.Close();
			timer.Stop();

			// Update GUI
			backgroundWorker_Set(new Tuple<string, dynamic>("btnClear", false));
			backgroundWorker_Set(new Tuple<string, dynamic>("btnConnect", true));
			backgroundWorker_Set(new Tuple<string, dynamic>("tbConStatus",
				strings.Disconnected));
			backgroundWorker_Set(new Tuple<string, dynamic>("btnDisconnect", false));
			backgroundWorker_Set(new Tuple<string, dynamic>("tbJsonGenerator",
				string.Empty));
			backgroundWorker_Set(new Tuple<string, dynamic>("boolNewText", false));
			backgroundWorker_Set(new Tuple<string, dynamic>("tbNewText",
				string.Empty));
			backgroundWorker_Set(new Tuple<string, dynamic>("btnSend", false));
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

			if (tbJsonGenerator.Text != String.Empty)
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
					backgroundWorker_Set(new Tuple<string, dynamic>("lbResults",
						strings.JsonSaved));
				}
				else
				{
					backgroundWorker_Set(new Tuple<string, dynamic>("lbResults",
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
			SendToController send2Controller = new SendToController();
			send2Controller.Send(message);

			// Show sent data in results list.
			backgroundWorker_Set(new Tuple<string, dynamic>("lbResults",
				String.Format(strings.Sent, message)));
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
				backgroundWorker_Set(new Tuple<string, dynamic>("btnClear", true));
				backgroundWorker_Set(new Tuple<string, dynamic>("btnSend", true));
			}
			else
			{
				// Update GUI
				backgroundWorker_Set(new Tuple<string, dynamic>("btnClear", false));
				backgroundWorker_Set(new Tuple<string, dynamic>("btnSend", false));
			}
		}
	}
}