using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using KruispuntGroep6.Simulator.Json;
using KruispuntGroep6.Simulator.Main;

namespace KruispuntGroep6.Simulator.Client
{
	/// <summary>
	/// Class used to send messages to Controller and shows logs of Simulator.
	/// </summary>
	public partial class Client : Form
	{
		private string address;	// String used to contain the IP address of the internet connection.
		private Button btnClear,
			btnConnect,
			btnDisconnect,
			btnJsonGenerator,
			btnSend,
			btnSimulatorConnect,
			btnSimulatorDisconnect;	// Buttons used to contain buttons.
		private ComboBox cbJsonType;	// ComboBox used to contain the three JSON types that can be generated.
		private int connectionAttempts;	// Integer used to contain the current connection attempt number.
		private string[] detectorJSON;	// String array used to contain all lines of the JSON detector data file.
		private int detectorJSONnumber;	// Integer used to contain the current detector JSON number.
		private bool hasRealAddress;	// Boolean used to contain Controller address that the user has put in.
		private string[] inputJSON;		// String array used to contain all lines of the JSON input data file.
		private int inputJSONnumber;	// Integer used to contain the current input JSON number.
		private JsonGenerator jsonGenerator = new JsonGenerator();	// JsonGenerator used to generate JSON data file.
		private string jsonType;	// String used to contain the selected JSON type.
		private Label lblConStatusValue;	// Label used to contain the connection status value.
		private ListBox lbResults;	// ListBox used to contain messages to keep the user informed.
		private int previousDetectorJSONnumber; // Integer used to contain the previous detector JSON number.
		private int previousInputJSONnumber; // Integer used to contain the previous input JSON number.
		private int previousStoplightJSONnumber; // Integer used to contain the previous stoplight JSON number.
		private int previousTime;	// Integer used to contain the previous input JSON time value.
		private string realAddress;	// String used to contain Controller address to be used by Simulator via Program.
		private MainGame game; // MainGame used to be the instance of Simulator.
		private string[] stoplightJSON;	// String array used to contain all lines of the JSON stoplight data file.
		private int stoplightJSONnumber;	// Integer used to contain the current stoplight JSON number.
		private Strings strings = new Strings();	// Strings used to store various strings used in the GUI.
		private TcpClient tcpClient;	// TcpClient used to contain the heart of this class: the TCP client.
		private TextBox tbAddress, tbJsonGenerator, tbNewText; // TextBoxes used to contain boxes to input text.
		private Thread thrConnect; // Thread used to contain functions that needs to be executed at the same time.
		private System.Timers.Timer timerConnection, timerJson; // Timers used to contain a stopwatch.
		private int timerJsonElapsed;

		public Client()
		{
			this.InitializeComponent();

			// Generate GUI
			this.btnClear = new Button();
			this.btnClear.Click += new EventHandler(BtnClear_Click);
			this.btnClear.Enabled = false;
			this.btnClear.Location = new Point(6 * Font.Height - 3, 7 * Font.Height - 6);
			this.btnClear.Parent = this;
			this.btnClear.Size = new Size(5 * Font.Height, 2 * Font.Height);
			this.btnClear.Text = strings.Clear;

			this.btnConnect = new Button();
			this.btnConnect.Click += new EventHandler(BtnConnect_Click);
			this.btnConnect.Location = new Point(17 * Font.Height - 3, Font.Height - 6);
			this.btnConnect.Parent = this;
			this.btnConnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			this.btnConnect.Text = strings.Connect;

			this.btnDisconnect = new Button();
			this.btnDisconnect.Click += new EventHandler(BtnDisconnect_Click);
			this.btnDisconnect.Enabled = false;
			this.btnDisconnect.Location = new Point(24 * Font.Height - 3, Font.Height - 6);
			this.btnDisconnect.Parent = this;
			this.btnDisconnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			this.btnDisconnect.Text = strings.Disconnect;

			this.btnJsonGenerator = new Button();
			this.btnJsonGenerator.Click += new EventHandler(BtnJsonGenerator_Click);
			this.btnJsonGenerator.Location = new Point(21 * Font.Height - 3, 10 * Font.Height - 6);
			this.btnJsonGenerator.Parent = this;
			this.btnJsonGenerator.Size = new Size(9 * Font.Height, 2 * Font.Height);
			this.btnJsonGenerator.Text = strings.GenerateJSON;

			this.btnSend = new Button();
			this.btnSend.Click += new EventHandler(BtnSend_Click);
			this.btnSend.Enabled = false;
			this.btnSend.Location = new Point(25 * Font.Height - 3, 7 * Font.Height - 6);
			this.btnSend.Parent = this;
			this.btnSend.Size = new Size(5 * Font.Height, 2 * Font.Height);
			this.btnSend.Text = strings.Send;

			this.btnSimulatorConnect = new Button();
			this.btnSimulatorConnect.Click += new EventHandler(BtnSimulatorConnect_Click);
			this.btnSimulatorConnect.Enabled = false;
			this.btnSimulatorConnect.Location = new Point(17 * Font.Height - 3, 4 * Font.Height - 6);
			this.btnSimulatorConnect.Parent = this;
			this.btnSimulatorConnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			this.btnSimulatorConnect.Text = strings.Connect;

			this.btnSimulatorDisconnect = new Button();
			this.btnSimulatorDisconnect.Click += new EventHandler(BtnSimulatorDisconnect_Click);
			this.btnSimulatorDisconnect.Enabled = false;
			this.btnSimulatorDisconnect.Location = new Point(24 * Font.Height - 3, 4 * Font.Height - 6);
			this.btnSimulatorDisconnect.Parent = this;
			this.btnSimulatorDisconnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			this.btnSimulatorDisconnect.Text = strings.Disconnect;

			this.cbJsonType = new ComboBox();
			this.cbJsonType.Items.Add(strings.JsonTypeInput);
			this.cbJsonType.Items.Add(strings.JsonTypeDetector);
			this.cbJsonType.Items.Add(strings.JsonTypeStoplight);
			this.cbJsonType.Location = new Point(14 * Font.Height, 10 * Font.Height - 3);
			this.cbJsonType.Parent = this;
			this.cbJsonType.Size = new Size(6 * Font.Height, 2 * Font.Height);
			this.cbJsonType.Text = strings.JsonTypeInput;

			this.FormBorderStyle = FormBorderStyle.FixedSingle;

			Label lblAddress = new Label();
			lblAddress.AutoSize = true;
			lblAddress.Location = new Point(Font.Height - 3, Font.Height);
			lblAddress.Parent = this;
			lblAddress.Text = strings.Address;

			this.lblConStatusValue = new Label();
			this.lblConStatusValue.AutoSize = false;
			this.lblConStatusValue.Location = new Point(Font.Height - 3, 4 * Font.Height);
			this.lblConStatusValue.Parent = this;
			this.lblConStatusValue.Size = new Size(13 * Font.Height - 11, Font.Height);
			this.lblConStatusValue.Text = strings.LabelDisconnected;

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

			Label lblSimulator = new Label();
			lblSimulator.AutoSize = true;
			lblSimulator.Location = new Point(13 * Font.Height - 3, 4 * Font.Height);
			lblSimulator.Parent = this;
			lblSimulator.Text = strings.Simulator;

			this.lbResults = new ListBox();
			this.lbResults.Location = new Point(Font.Height - 3, 12 * Font.Height);
			this.lbResults.Parent = this;
			this.lbResults.Size = new Size(29 * Font.Height, 22 * Font.Height);
			this.lbResults.TabStop = false;

			this.MaximizeBox = false;

			this.MessageChanged += new MessageChangedHandler(ChangeMessage);

			this.SetAddress();

			this.Size = new Size(31 * Font.Height, 36 * Font.Height);

			this.tbAddress = new TextBox();
			this.tbAddress.Location = new Point(9 * Font.Height, Font.Height - 3);
			this.tbAddress.Parent = this;
			this.tbAddress.Size = new Size(7 * Font.Height, 2 * Font.Height);
			this.tbAddress.Text = address;

			this.tbJsonGenerator = new TextBox();
			this.tbJsonGenerator.Location = new Point(9 * Font.Height, 10 * Font.Height - 3);
			this.tbJsonGenerator.MaxLength = 5;
			this.tbJsonGenerator.Parent = this;
			this.tbJsonGenerator.Size = new Size(4 * Font.Height, 2 * Font.Height);

			this.tbNewText = new TextBox();
			this.tbNewText.Enabled = false;
			this.tbNewText.Location = new Point(12 * Font.Height, 7 * Font.Height - 3);
			this.tbNewText.Parent = this;
			this.tbNewText.Size = new Size(12 * Font.Height, 2 * Font.Height);
			this.tbNewText.TextChanged += new EventHandler(tbNewText_TextChanged);

			this.Text = strings.TcpClient;
		}

		/// <summary>
		/// Reads JSON input file.
		/// </summary>
		private bool ReadJSON()
		{
			bool success = true;

			try
			{
				// get JSON type
				jsonType = cbJsonType.SelectedItem.ToString();

				switch (jsonType)
				{
					case "detectors":
						// if detectorJSON isn't born yet, setting this boolean false forces
						// reading the JSON detector file and resetting the detector JSON number
						bool detectorFilesAreEqual = false;

						// if detectorJSON exists, set the real value of detectorFilesAreEqual
						if (detectorJSON != null)
						{
							detectorFilesAreEqual = Enumerable.SequenceEqual(File.ReadAllLines(
								strings.JsonTypeDetector + strings.JsonFileExtension), detectorJSON);
						}

						//when the user generates a new JSON detector file,
						//and the client handles the previous file,
						//abort that file and make the new file his
						if (!detectorFilesAreEqual)
						{
							detectorJSON = File.ReadAllLines(strings.JsonTypeDetector + strings.JsonFileExtension);
							detectorJSONnumber = 0;
						}

						//reset current detector JSON number,
						//if the whole JSON detector file is sent already
						if (detectorJSONnumber >= detectorJSON.Length)
						{
							detectorJSONnumber = 0;
						}

						// if the whole JSON detector file isn't sent already,
						// continue with the detectorJSONnumber from before the
						// connection loss or disconnect
						if (detectorJSONnumber > 0)
						{
							previousDetectorJSONnumber = detectorJSONnumber;
						}
						break;
					case "inputs":
						// if inputJSON isn't born yet, setting this boolean false forces
						// reading the JSON input file and resetting the input JSON number
						bool inputFilesAreEqual = false;

						// if inputJSON exists, set the real value of inputFilesAreEqual
						if (inputJSON != null)
						{
							inputFilesAreEqual = Enumerable.SequenceEqual(File.ReadAllLines(
								strings.JsonTypeInput + strings.JsonFileExtension), inputJSON);
						}

						//when the user generates a new JSON input file,
						//and the client handles the previous file,
						//abort that file and make the new file his
						if (!inputFilesAreEqual)
						{
							inputJSON = File.ReadAllLines(strings.JsonTypeInput + strings.JsonFileExtension);
							inputJSONnumber = 0;
						}

						//reset current input JSON number,
						//if the whole JSON input file is sent already
						if (inputJSONnumber >= inputJSON.Length)
						{
							inputJSONnumber = 0;
						}

						// if the whole JSON input file isn't sent already,
						// continue with the inputJSONnumber from before the
						// connection loss or disconnect
						if (inputJSONnumber > 0)
						{
							previousInputJSONnumber = inputJSONnumber;
						}
						break;
					case "stoplights":
						// if stoplightJSON isn't born yet, setting this boolean false forces
						// reading the JSON stoplight file and resetting the stoplight JSON number
						bool stoplightFilesAreEqual = false;

						// if stoplightJSON exists, set the real value of stoplightFilesAreEqual
						if (stoplightJSON != null)
						{
							stoplightFilesAreEqual = Enumerable.SequenceEqual(File.ReadAllLines(
								strings.JsonTypeStoplight + strings.JsonFileExtension), stoplightJSON);
						}

						//when the user generates a new JSON stoplight file,
						//and the client handles the previous file,
						//abort that file and make the new file his
						if (!stoplightFilesAreEqual)
						{
							stoplightJSON = File.ReadAllLines(strings.JsonTypeStoplight + strings.JsonFileExtension);
							stoplightJSONnumber = 0;
						}

						//reset current stoplight JSON number,
						//if the whole JSON stoplight file is sent already
						if (stoplightJSONnumber >= stoplightJSON.Length)
						{
							stoplightJSONnumber = 0;
						}

						// if the whole JSON stoplight file isn't sent already,
						// continue with the stoplightJSONnumber from before the
						// connection loss or disconnect
						if (stoplightJSONnumber > 0)
						{
							previousStoplightJSONnumber = stoplightJSONnumber;
						}
						break;
				}
			}
			catch (FileNotFoundException)
			{
				OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonReadingError1);
				OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonReadingError2);
				success = false;
				Disconnected(false);
			}

			return success;
		}

		/// <summary>
		/// Handles change of Message.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="type">Type used to determine the type of the Control.</param>
		/// <param name="msg">String used to determine the Text value of a Button, Label, ListBox or TextBox control.</param>
		/// <param name="visible">Boolean used to determine the Enabled value of a Button, ComboBox, Label, ListBox or TextBox control.</param>
		private delegate void MessageChangedHandler(object sender, Type type, string msg, bool visible);

		/// <summary>
		/// After Message changed.
		/// </summary>
		private event MessageChangedHandler MessageChanged;

		/// <summary>
		/// Clears input text.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnClear_Click(object sender, EventArgs e)
		{
			btnClear.Enabled = false;
			tbNewText.Text = string.Empty;
		}

		/// <summary>
		/// Connects to Controller.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnConnect_Click(object sender, EventArgs e)
		{
			btnConnect.Enabled = false;
			tbAddress.Enabled = false;
			lblConStatusValue.Text = strings.Connecting;

			timerConnection = new System.Timers.Timer(1000);
			timerConnection.Elapsed += new ElapsedEventHandler(timerConnection_Elapsed);
			timerConnection.Start();

			// Attempt to connect, because we don't want to wait for the timer.
			ConnectAttempt();
		}

		/// <summary>
		/// Disconnects from Controller.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnDisconnect_Click(object sender, EventArgs e)
		{
			if (timerJson != null)
			{
				timerJson.Stop();
			}

			if (connectionAttempts.Equals(0))
			{
				SendToController(strings.Exit, false);
				Disconnected(true);
			}
			else
			{
				DisconnectWhileConnecting();
			}

			if (tcpClient != null)
			{
				tcpClient = null;
			}
		}

		/// <summary>
		/// Generates JSON input file.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnJsonGenerator_Click(object sender, EventArgs e)
		{
			int nrOfInputs = -1;

			bool onlyNumbers = true;

			if (!tbJsonGenerator.Text.Equals(string.Empty))
			{
				foreach (char c in tbJsonGenerator.Text)
				{
					if (!char.IsNumber(c))
						onlyNumbers = false;
				}

				if (onlyNumbers)
					nrOfInputs = int.Parse(tbJsonGenerator.Text);
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
						cbJsonType.SelectedItem.ToString() + strings.JsonFileExtension);
				}
				else
				{
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSavingError +
						cbJsonType.SelectedItem.ToString() + strings.JsonFileExtension);
				}
			}
		}

		/// <summary>
		/// Sends a message to Controller.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnSend_Click(object sender, EventArgs e)
		{
			SendToController(tbNewText.Text);
		}

		/// <summary>
		/// Connects to Simulator.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnSimulatorConnect_Click(object sender, EventArgs e)
		{
			// Read JSON input file and if that's a success, send start time to Controller
			if (ReadJSON())
			{
				// Disable JSON generator GUI, to prevent bugs
				OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType(), enabled: false);
				OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType(), enabled: false);
				OnMessageChanged(cbJsonType, cbJsonType.GetType(), enabled: false);
				OnMessageChanged(btnSimulatorConnect, btnSimulatorConnect.GetType(), enabled: false);
				OnMessageChanged(btnSimulatorDisconnect, btnSimulatorConnect.GetType());

				SendStartTime();
			}
		}

		/// <summary>
		/// Disconnects from Simulator.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">EventArgs used to determine event arguments.</param>
		private void BtnSimulatorDisconnect_Click(object sender, EventArgs e)
		{
			if (timerJson != null)
			{
				timerJson.Stop();
			}

			Disconnected(false);
		}

		/// <summary>
		/// Changes a Message.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="type"></param>
		/// <param name="text"></param>
		/// <param name="enabled"></param>
		private void ChangeMessage(object sender, Type type, string text, bool enabled)
		{
			switch (type.Name)
			{
				case "Button":
					if (text != null)
					{
						(sender as Button).Text = text;
					}
					(sender as Button).Enabled = enabled;
					break;
				case "ComboBox":
					(sender as ComboBox).Enabled = enabled;
					break;
				case "Label":
					if (text != null)
					{
						(sender as Label).Text = text;
					}
					(sender as Label).Enabled = enabled;
					break;
				case "ListBox":
					if (text != null)
					{
						(sender as ListBox).Items.Add(text);

						// Autoscroll. Source: http://www.csharp-examples.net/autoscroll
						(sender as ListBox).SelectedIndex = this.lbResults.Items.Count - 1;
						(sender as ListBox).SelectedIndex = -1;
					}

					(sender as ListBox).Enabled = enabled;
					break;
				case "TextBox":
					if (text != null)
					{
						(sender as TextBox).Text = text;
					}
					(sender as TextBox).Enabled = enabled;
					break;
				default:
					throw new Exception(string.Format("Type {0} wordt niet herkend!", type.Name));
			}
		}

		/// <summary>
		/// Connects to Controller.
		/// </summary>
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
					DisconnectWhileConnecting(false);
				}
			}

			if (tcpClient != null)
			{
				if (tcpClient.Connected)
				{
					Connected();
				}
			}
		}

		/// <summary>
		/// Attempts to connect to Controller.
		/// </summary>
		private void ConnectAttempt()
		{
			// Get address from textbox
			address = tbAddress.Text;

			// Try to connect in the background...
			thrConnect = new Thread(new ThreadStart(Connect));
			thrConnect.Start();

			if (tcpClient != null)
			{
				connectionAttempts++;

				OnMessageChanged(lbResults, lbResults.GetType(),
					string.Format(strings.ConnectionAttempt, connectionAttempts + strings.With + address));
			}
		}

		/// <summary>
		/// Connected to Controller.
		/// </summary>
		private void Connected()
		{
			// Send tcpClient to the static recieve class
			Recieve.tcpClient = tcpClient;
			// Send tcpClient to the static send class
			Send.tcpClient = tcpClient;

			//stop connection timer
			timerConnection.Stop();
			//reset connection attempts
			connectionAttempts = 0;
			//reset previous detector json number
			previousDetectorJSONnumber = 0;
			//reset previous input json number
			previousInputJSONnumber = 0;
			//reset previous stoplight json number
			previousStoplightJSONnumber = 0;

			// Update GUI
			OnMessageChanged(btnConnect, btnConnect.GetType(), enabled: false);
			OnMessageChanged(lblConStatusValue, lblConStatusValue.GetType(),
				string.Format(strings.Connected, tcpClient.Client.RemoteEndPoint.ToString()));
			OnMessageChanged(btnDisconnect, btnDisconnect.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType());
			OnMessageChanged(btnSend, btnSend.GetType());

			// Get address from textbox
			address = tbAddress.Text;

			realAddress = address;

			if (!hasRealAddress)
			{
				hasRealAddress = true;
			}

			OnMessageChanged(btnSimulatorConnect, btnSimulatorConnect.GetType());
		}

		/// <summary>
		/// Disconnected from Controller.
		/// </summary>
		/// <param name="isController">Boolean used to determine if the Client is disconnected from Controller.</param>
		private void Disconnected(bool isController)
		{
			// Update GUI
			OnMessageChanged(tbAddress, tbAddress.GetType(), address);
			OnMessageChanged(btnClear, btnClear.GetType(), enabled: false);

			if (isController)
			{
				OnMessageChanged(btnConnect, btnConnect.GetType());
				OnMessageChanged(lblConStatusValue, lblConStatusValue.GetType(), strings.LabelDisconnected);
				OnMessageChanged(btnDisconnect, btnDisconnect.GetType(), enabled: false);
			}

			OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType());
			OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType());
			OnMessageChanged(cbJsonType, cbJsonType.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType());
			OnMessageChanged(tbNewText, tbNewText.GetType(), enabled: false);
			OnMessageChanged(btnSend, btnSend.GetType(), enabled: false);

			if (isController)
			{
				OnMessageChanged(btnSimulatorConnect, btnSimulatorConnect.GetType(), enabled: false);
			}
			else
			{
				OnMessageChanged(btnSimulatorConnect, btnSimulatorConnect.GetType());
			}

			OnMessageChanged(btnSimulatorDisconnect, btnSimulatorDisconnect.GetType(), enabled: false);
		}

		/// <summary>
		/// Disconnects from Controller while connecting to Controller.
		/// </summary>
		private void DisconnectWhileConnecting(bool show = true)
		{
			if (show)
			{
				OnMessageChanged(lbResults, lbResults.GetType(), strings.ConnectingAborted);
			}
			connectionAttempts = 0;
			timerConnection.Stop();
			Disconnected(true);
		}

		/// <summary>
		/// Gets encrypted JSON.
		/// </summary>
		/// <param name="json">String used to contain a JSON object or JSON array.</param>
		/// <returns>Strings used to contain a message.</returns>
		private string GetEncryptedJson(string json)
		{
			string encryptedJson = string.Empty;

			if (json.StartsWith(strings.BraceOpen))
			{
				encryptedJson = JsonConverter.JsonObjectToMessage(json);
			}
			else if (json.StartsWith(strings.BracketOpen))
			{
				encryptedJson = JsonConverter.JsonArrayToMessage(json);
			}

			return encryptedJson;
		}

		/// <summary>
		/// Gets the Time value of a JSON message.
		/// </summary>
		/// <param name="message">String used to contain a JSON message.</param>
		/// <returns>Integer used to contain a time in seconds.</returns>
		private int GetTime(string message)
		{
			int time = 0;

			string json = message.Split(char.Parse(strings.BracketClose))[0].Remove(0, 1).Split(char.Parse(strings.Comma))[1];

			if (!json.Equals(string.Empty))
			{
				time = int.Parse(json);
			}

			return time;
		}

		/// <summary>
		/// Sends JSON object to Controller.
		/// </summary>
		private void SendJSONObject()
		{
			switch (jsonType)
			{
				case "detectors":
					if (previousDetectorJSONnumber > 0)
					{
						detectorJSONnumber = previousDetectorJSONnumber;
						previousDetectorJSONnumber = 0;
					}

					// Send the message and show it in the results listbox.
					SendToController(detectorJSON[detectorJSONnumber]);

					detectorJSONnumber++;

					if (detectorJSONnumber >= detectorJSON.Length)
					{
						SentLastJson(jsonType);
					}
					break;
				case "inputs":
					int inputTime = 0;

					if (previousInputJSONnumber > 0)
					{
						inputJSONnumber = previousInputJSONnumber;
						previousInputJSONnumber = 0;
					}

					if (timerJson.Enabled)
					{
						inputTime = GetTime(GetEncryptedJson(inputJSON[inputJSONnumber]));
					}

					while (inputTime.Equals(previousTime) && inputTime <= timerJsonElapsed)
					{
						string message = GetEncryptedJson(inputJSON[inputJSONnumber]);

						// Don't send the message, but show it in the results listbox.
						SendToController(message, false);

						// Send the message, but don't show it in the results listbox.
						game.Communication.Decrypter(message);

						inputJSONnumber++;

						if (inputJSONnumber < inputJSON.Length)
						{
							inputTime = GetTime(GetEncryptedJson(inputJSON[inputJSONnumber]));
						}
						else
						{
							timerJson.Stop();
							break;
						}
					}

					if (inputJSONnumber < inputJSON.Length)
					{
						previousTime = GetTime(GetEncryptedJson(inputJSON[inputJSONnumber]));
					}
					else
					{
						SentLastJson(jsonType);
					}
					break;
				case "stoplights":
					if (previousStoplightJSONnumber > 0)
					{
						stoplightJSONnumber = previousStoplightJSONnumber;
						previousStoplightJSONnumber = 0;
					}

					// Send the message and show it in the results listbox.
					SendToController(stoplightJSON[stoplightJSONnumber]);

					stoplightJSONnumber++;

					if (stoplightJSONnumber >= stoplightJSON.Length)
					{
						SentLastJson(jsonType);
					}
					break;
			}
		}

		/// <summary>
		/// Sends start time to Controller.
		/// </summary>
		private void SendStartTime()
		{
			string time = DateTime.Now.ToString(strings.DateTimeFormat);
			string jsonStartTime = DynamicJson.Serialize(new object[] { new { starttime = time } });
			string jsonMultiplier = DynamicJson.Serialize(new object[] { new { multiplier = 1 } });

			if (previousDetectorJSONnumber.Equals(0) &&
				previousInputJSONnumber.Equals(0) &&
				previousStoplightJSONnumber.Equals(0))
			{
				SendToController(jsonStartTime, true, false);
				SendToController(jsonMultiplier, true, false);
			}

			previousTime = 1;

			timerJson = new System.Timers.Timer(1000);
			timerJson.Elapsed += new ElapsedEventHandler(timerJson_Elapsed);
			timerJsonElapsed = 1;
			timerJson.Start();

			// Start recieving messages from the controller
			Recieve.ReceiveMessage(game);
		}

		/// <summary>
		/// Sends a message to Controller.
		/// </summary>
		/// <param name="message">String used to determine the message to be send.</param>
		private void SendToController(string message, bool send = true, bool show = true)
		{
			if (send)
			{
				Send.SendMessage(message);
			}

			if (show)
			{
				// Show sent data in results list.
				OnMessageChanged(lbResults, lbResults.GetType(), string.Format(strings.Sent, message));
			}
		}

		/// <summary>
		/// Stops the JSON timer and enables the generation of JSON file after the last JSON from file is sent.
		/// </summary>
		/// <param name="jsonType">String used to determine the type of the JSON.</param>
		private void SentLastJson(String jsonType)
		{
			timerJson.Stop();

			switch (jsonType)
			{
				case "detectors":
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSentDetector);
					break;
				case "inputs":
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSentInput);
					break;
				case "stoplights":
					OnMessageChanged(lbResults, lbResults.GetType(), strings.JsonSentStoplight);
					break;
			}

			// Enable generation of JSON file
			OnMessageChanged(btnJsonGenerator, btnJsonGenerator.GetType());
			OnMessageChanged(tbJsonGenerator, tbJsonGenerator.GetType());
			OnMessageChanged(cbJsonType, cbJsonType.GetType());
			OnMessageChanged(btnSimulatorConnect, btnSimulatorConnect.GetType());
			OnMessageChanged(btnSimulatorDisconnect, btnSimulatorConnect.GetType(), enabled: false);
		}

		/// <summary>
		/// Sets the IP address used by Simulator.
		/// </summary>
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

		/// <summary>
		/// Attempts to connect to Controller, after each tick of connection timer.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">ElapsedEventArgs used to determine elapsed event arguments.</param>
		private void timerConnection_Elapsed(object sender, ElapsedEventArgs e)
		{
			ConnectAttempt();

			// Update GUI
			OnMessageChanged(btnDisconnect, btnDisconnect.GetType());
		}

		/// <summary>
		/// Sends a JSON every second.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="e">ElapsedEventArgs used to determine elapsed event arguments.</param>
		private void timerJson_Elapsed(object sender, ElapsedEventArgs e)
		{
			SendJSONObject();

			timerJsonElapsed++;
		}

		/// <summary>
		/// Invokes the Control.
		/// </summary>
		/// <param name="sender">Object used to determine the sender.</param>
		/// <param name="type">Type used to determine the type of the Control.</param>
		/// <param name="text">String used to determine the Text value of a Button, Label, ListBox or TextBox control.</param>
		/// <param name="enabled">Boolean used to determine the Enabled value of a Button, ComboBox, Label, ListBox or TextBox control.</param>
		protected virtual void OnMessageChanged(object sender, Type type, string text = null, bool enabled = true)
		{
			Control target = MessageChanged.Target as Control;

			if (target != null && target.InvokeRequired)
			{
				target.Invoke(MessageChanged, new object[] {sender, type, text, enabled});
			}
			else
			{
				ChangeMessage(sender, type, text, enabled);
			}
		}

		/// <summary>
		/// Gets the IP address used by Simulator.
		/// </summary>
		/// <returns>String used to </returns>
		public string GetAddress()
		{
			if (realAddress != null)
			{
				return realAddress;
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Sets the Simulator.
		/// </summary>
		/// <param name="simulator"></param>
		public void SetSimulator(MainGame simulator)
		{
			this.game = simulator;
		}
	}
}