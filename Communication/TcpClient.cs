using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace KruispuntGroep6.Communication
{
	public partial class TcpClient : Form
	{
		private Button btnClear;
		private Button btnConnect;
		private Button btnDisconnect;
		private Button btnJsonGenerator;
		private Button btnSend;
		private BackgroundWorker bwClearBtn = new BackgroundWorker();
		private BackgroundWorker bwConnectBtn = new BackgroundWorker();
		private BackgroundWorker bwConStatusTb = new BackgroundWorker();
		private BackgroundWorker bwDisconnectBtn = new BackgroundWorker();
		private BackgroundWorker bwJsonGeneratorBtn = new BackgroundWorker();
		private BackgroundWorker bwJsonGeneratorBool = new BackgroundWorker();
		private BackgroundWorker bwJsonGeneratorTb = new BackgroundWorker();
		private BackgroundWorker bwNewTextBool = new BackgroundWorker();
		private BackgroundWorker bwNewTextTb = new BackgroundWorker();
		private BackgroundWorker bwResultsLb = new BackgroundWorker();
		private BackgroundWorker bwSendBtn = new BackgroundWorker();
		private byte[] byteData = new byte[1024];
		private int intPort = 1337;
		private int intSize = 1024;
		private IPAddress ipAddr = new IPAddress(0);
		private JsonGenerator jsonGenerator = new JsonGenerator();
		private ListBox lbResults;
		private TextBox tbConStatus;
		private TextBox tbJsonGenerator;
		private TextBox tbNewText;
		private Socket scktClient;
		private string strIP = "127.0.0.1";

		public TcpClient()
		{
			this.InitializeComponent();

			btnClear = new Button();
			btnClear.Enabled = false;
			btnClear.Parent = this;
			btnClear.Text = "Clear";
			btnClear.Location = new Point(210, 20);
			btnClear.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnClear.Click += new EventHandler(btnClear_Click);
			btnClear.TabStop = false;

			btnConnect = new Button();
			btnConnect.Parent = this;
			btnConnect.Text = "Connect";
			btnConnect.Location = new Point(285, 20);
			btnConnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnConnect.Click += new EventHandler(btnConnect_Click);
			btnConnect.TabStop = false;

			btnDisconnect = new Button();
			btnDisconnect.Enabled = false;
			btnDisconnect.Parent = this;
			btnDisconnect.Text = "Disconnect";
			btnDisconnect.Location = new Point(285, 52);
			btnDisconnect.Size = new Size(6 * Font.Height, 2 * Font.Height);
			btnDisconnect.Click += new EventHandler(btnDisconnect_Click);
			btnDisconnect.TabStop = false;

			btnJsonGenerator = new Button();
			btnJsonGenerator.Enabled = false;
			btnJsonGenerator.Parent = this;
			btnJsonGenerator.Text = "Generate JSON inputdata file";
			btnJsonGenerator.Location = new Point(60, 382);
			btnJsonGenerator.Size = new Size(14 * Font.Height, 2 * Font.Height);
			btnJsonGenerator.Click += new EventHandler(btnJsonGenerator_Click);
			btnJsonGenerator.TabIndex = 3;

			btnSend = new Button();
			btnSend.Enabled = false;
			btnSend.Parent = this;
			btnSend.Text = "Send";
			btnSend.Location = new Point(210, 52);
			btnSend.Size = new Size(5 * Font.Height, 2 * Font.Height);
			btnSend.Click += new EventHandler(btnSend_Click);
			btnSend.TabIndex = 1;

			bwClearBtn.DoWork += new DoWorkEventHandler(bwClearBtn_DoWork);
			bwClearBtn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwClearBtn_RunWorkerCompleted);

			bwConnectBtn.DoWork += new DoWorkEventHandler(bwConnectBtn_DoWork);
			bwConnectBtn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConnectBtn_RunWorkerCompleted);

			bwConStatusTb.DoWork += new DoWorkEventHandler(bwConStatusTb_DoWork);
			bwConStatusTb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConStatusTb_RunWorkerCompleted);

			bwDisconnectBtn.DoWork += new DoWorkEventHandler(bwDisconnectBtn_DoWork);
			bwDisconnectBtn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDisconnectBtn_RunWorkerCompleted);

			bwJsonGeneratorBool.DoWork += new DoWorkEventHandler(bwJsonGeneratorBool_DoWork);
			bwJsonGeneratorBool.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwJsonGeneratorBool_RunWorkerCompleted);

			bwJsonGeneratorBtn.DoWork += new DoWorkEventHandler(bwJsonGeneratorBtn_DoWork);
			bwJsonGeneratorBtn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwJsonGeneratorBtn_RunWorkerCompleted);

			bwJsonGeneratorTb.DoWork += new DoWorkEventHandler(bwJsonGeneratorTb_DoWork);
			bwJsonGeneratorTb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwJsonGeneratorTb_RunWorkerCompleted);

			bwNewTextBool.DoWork += new DoWorkEventHandler(bwNewTextBool_DoWork);
			bwNewTextBool.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwNewTextBool_RunWorkerCompleted);

			bwNewTextTb.DoWork += new DoWorkEventHandler(bwNewTextTb_DoWork);
			bwNewTextTb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwNewTextTb_RunWorkerCompleted);

			bwResultsLb.DoWork += new DoWorkEventHandler(bwResultsLb_DoWork);
			bwResultsLb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwResultsLb_RunWorkerCompleted);

			bwSendBtn.DoWork += new DoWorkEventHandler(bwSendBtn_DoWork);
			bwSendBtn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSendBtn_RunWorkerCompleted);

			this.FormBorderStyle = FormBorderStyle.FixedSingle;

			this.ipAddr = IPAddress.Parse(strIP);

			Label lblConStatus = new Label();
			lblConStatus.AutoSize = true;
			lblConStatus.Location = new Point(10, 330);
			lblConStatus.Parent = this;
			lblConStatus.Text = "Connection Status:";

			Label lblJsonGenerator = new Label();
			lblJsonGenerator.AutoSize = true;
			lblJsonGenerator.Location = new Point(10, 360);
			lblJsonGenerator.Parent = this;
			lblJsonGenerator.Text = "How much inputs?";

			Label lblNewText = new Label();
			lblNewText.AutoSize = true;
			lblNewText.Location = new Point(10, 30);
			lblNewText.Parent = this;
			lblNewText.Text = "Enter text string:";

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
			tbConStatus.Text = "Disconnected";

			tbJsonGenerator = new TextBox();
			tbJsonGenerator.Enabled = false;
			tbJsonGenerator.Location = new Point(10, 385);
			tbJsonGenerator.MaxLength = 5;
			tbJsonGenerator.Parent = this;
			tbJsonGenerator.Size = new Size(38, 2 * Font.Height);
			tbJsonGenerator.TabIndex = 2;

			tbNewText = new TextBox();
			tbNewText.Enabled = false;
			tbNewText.Location = new Point(10, 55);
			tbNewText.MaxLength = 30;
			tbNewText.Parent = this;
			tbNewText.Size = new Size(190, 2 * Font.Height);
			tbNewText.TabIndex = 0;
			tbNewText.TextChanged += new EventHandler(tbNewText_TextChanged);

			this.Text = "Kruispunt Groep 6: TCP Client";
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			if (!bwClearBtn.IsBusy)
				bwClearBtn.RunWorkerAsync(false);
			if (!bwNewTextTb.IsBusy)
				bwNewTextTb.RunWorkerAsync(String.Empty);
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (!bwConStatusTb.IsBusy)
				bwConStatusTb.RunWorkerAsync("Connecting...");

			Socket newsock = new Socket(AddressFamily.InterNetwork,
								  SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint iep = new IPEndPoint(ipAddr, intPort);
			newsock.BeginConnect(iep, new AsyncCallback(Connected), newsock);
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			scktClient.Close();

			if (!bwClearBtn.IsBusy)
				bwClearBtn.RunWorkerAsync(false);
			if (!bwConnectBtn.IsBusy)
				bwConnectBtn.RunWorkerAsync(true);
			if (!bwConStatusTb.IsBusy)
				bwConStatusTb.RunWorkerAsync("Disconnected");
			if (!bwDisconnectBtn.IsBusy)
				bwDisconnectBtn.RunWorkerAsync(false);
			if (!bwJsonGeneratorBool.IsBusy)
				bwJsonGeneratorBool.RunWorkerAsync(false);
			if (!bwJsonGeneratorBtn.IsBusy)
				bwJsonGeneratorBtn.RunWorkerAsync(false);
			if (!bwJsonGeneratorTb.IsBusy)
				bwJsonGeneratorTb.RunWorkerAsync(String.Empty);
			if (!bwNewTextBool.IsBusy)
				bwNewTextBool.RunWorkerAsync(false);
			if (!bwNewTextTb.IsBusy)
				bwNewTextTb.RunWorkerAsync(String.Empty);
			if (!bwSendBtn.IsBusy)
				bwSendBtn.RunWorkerAsync(false);
		}

		private void btnJsonGenerator_Click(object sender, EventArgs e)
		{
			// TODO: generate JSON inputdata file based on number of inputs and named input.json and saved in working directory.

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
					if (!bwResultsLb.IsBusy)
						bwResultsLb.RunWorkerAsync("JSON file saved as input.json");
				}
				else
				{
					if (!bwResultsLb.IsBusy)
						bwResultsLb.RunWorkerAsync("Error saving JSON file as input.json");
				}
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			byte[] message = Encoding.ASCII.GetBytes(tbNewText.Text);
			try
			{
				scktClient.BeginSend(message, 0, message.Length, SocketFlags.None,
					new AsyncCallback(SendData), scktClient);
			}
			catch (SocketException se)
			{
				if (!bwResultsLb.IsBusy)
					bwResultsLb.RunWorkerAsync(se.Message);
			}
		}

		private void bwClearBtn_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwClearBtn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnClear.Enabled = (bool)e.Result;
		}

		private void bwConnectBtn_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwConnectBtn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnConnect.Enabled = (bool)e.Result;
		}

		private void bwConStatusTb_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwConStatusTb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.tbConStatus.Text = e.Result as String;
		}

		private void bwDisconnectBtn_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwDisconnectBtn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnDisconnect.Enabled = (bool)e.Result;
		}

		private void bwJsonGeneratorBool_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwJsonGeneratorBool_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.tbJsonGenerator.Enabled = (bool)e.Result;
		}

		private void bwJsonGeneratorBtn_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwJsonGeneratorBtn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnJsonGenerator.Enabled = (bool)e.Result;
		}

		private void bwJsonGeneratorTb_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwJsonGeneratorTb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			tbJsonGenerator.Text = e.Result as String;
		}

		private void bwNewTextBool_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwNewTextBool_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.tbNewText.Enabled = (bool)e.Result;
		}

		private void bwNewTextTb_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwNewTextTb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.tbNewText.Text = e.Result as String;
		}

		private void bwResultsLb_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwResultsLb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.lbResults.Items.Add(e.Result as String);

			// Autoscroll. Source: http://www.csharp-examples.net/autoscroll
			this.lbResults.SelectedIndex = this.lbResults.Items.Count - 1;
			this.lbResults.SelectedIndex = -1;
		}

		private void bwSendBtn_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = e.Argument;
		}

		private void bwSendBtn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnSend.Enabled = (bool)e.Result;
		}

		private void Connected(IAsyncResult ar)
		{
			scktClient = (Socket)ar.AsyncState;

			try
			{
				scktClient.EndConnect(ar);

				scktClient.BeginReceive(byteData, 0, intSize, SocketFlags.None,
					new AsyncCallback(ReceiveData), scktClient);

				if (!bwConnectBtn.IsBusy)
					bwConnectBtn.RunWorkerAsync(false);
				if (!bwConStatusTb.IsBusy)
					bwConStatusTb.RunWorkerAsync("Connected to: " + scktClient.RemoteEndPoint.ToString());
				if (!bwDisconnectBtn.IsBusy)
					bwDisconnectBtn.RunWorkerAsync(true);
				if (!bwJsonGeneratorBool.IsBusy)
					bwJsonGeneratorBool.RunWorkerAsync(true);
				if (!bwJsonGeneratorBtn.IsBusy)
					bwJsonGeneratorBtn.RunWorkerAsync(true);
				if (!bwNewTextBool.IsBusy)
					bwNewTextBool.RunWorkerAsync(true);
				if (!bwSendBtn.IsBusy)
					bwSendBtn.RunWorkerAsync(true);
			}
			catch (SocketException)
			{
				if (!bwConStatusTb.IsBusy)
					bwConStatusTb.RunWorkerAsync("Error connecting");
			}
		}

		private void ReceiveData(IAsyncResult ar)
		{
			Socket remote = (Socket)ar.AsyncState;
			int recv = remote.EndReceive(ar);

			if (!bwResultsLb.IsBusy)
				bwResultsLb.RunWorkerAsync(Encoding.ASCII.GetString(byteData, 0, recv));
		}

		private void SendData(IAsyncResult ar)
		{
			Socket remote = (Socket)ar.AsyncState;
			int sent = remote.EndSend(ar);
			try
			{
				remote.BeginReceive(byteData, 0, intSize, SocketFlags.None,
					new AsyncCallback(ReceiveData), remote);
			}
			catch (SocketException se)
			{
				if (!bwResultsLb.IsBusy)
					bwResultsLb.RunWorkerAsync(se.Message);
			}
		}

		private void tbNewText_TextChanged(object sender, EventArgs e)
		{
			int length = (sender as TextBox).Text.Length;

			if (length > 0)
			{
				if (!bwClearBtn.IsBusy)
					bwClearBtn.RunWorkerAsync(true);
			}
			else
			{
				if (!bwClearBtn.IsBusy)
					bwClearBtn.RunWorkerAsync(false);
			}
		}
	}
}