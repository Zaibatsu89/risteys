using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace SimCommander.Communication
{
    class Server
    {
        private List<TcpClient> clients;
        private TcpListener server;
        private string address;
        public static volatile bool quit;
        public Queue<String> message;

        public object lockthis = new object();

        public Server()
        {
            clients = new List<TcpClient>();
            // detect current ip address and uses it
            SetAddress();
            OnInfoMessage("server adres: " + address);

            quit = false;
            message = new Queue<string>();

            //OnInfoMessage(string.Format(Strings.HiIAmController +
            //    " and I serve from {0}:1337 forever", address));

            // start the server
            //server.Start();

            // create a thread witch handels connect requests
            new Thread(new ThreadStart(connect)).Start();

        }

        private void connect()
        {
            Thread.Sleep(1000);
            OnInfoMessage(string.Format("the server is started on {0}:1337", this.address.ToString()));

            // create our TCPListener object
            server = new TcpListener(IPAddress.Parse(address), Int32.Parse(Strings.Port));
            server.Start();
            new Thread(new ThreadStart(write)).Start();
            while (!quit)
            {

                //create a null connection
                TcpClient client = null;
                //check if there are any pending connection requests
                if (server.Pending())
                {
                    //if there are pending requests create a new connection
                    client = server.AcceptTcpClient();

                    lock (lockthis)
                    {
                        //add client to clients
                        clients.Add(client);
                    }

                    //create a new DoCommunicate Object
                    ClientReader reader = new ClientReader(client);
                    reader.InfoMessageChanged += new delegates.OnInfoMessageHandler(reader_InfoMessageChanged);
                    reader.MessageRecieved += new delegates.OnMessageRecievedHandler(reader_MessageRecieved);
                }
            }
        }

        private class ClientReader
        {

            private StreamReader reader;
            private TcpClient client;

            public ClientReader(TcpClient client)
            {
                this.client = client;

                this.reader = new StreamReader(client.GetStream());

                new Thread(new ThreadStart(messageReader)).Start();
            }

            private void messageReader()
            {
                string data = "";

                while (!quit)
                {
					try
					{
						data = reader.ReadLine();
					}
					catch (IOException)
					{
						OnInfoMessage("Connection lost");
						client.Close();
						quit = true;
						break;
					}

					if (data != null)
					{
						if (data != String.Empty)
						{
							if (data.Equals(Strings.Exit))
							{
								OnInfoMessage("Connection lost");
								client.Close();
								quit = true;
								break;
							}
							if (data.Equals(Strings.HiIAmSimulator))
								OnInfoMessage("Hi I'am a Simulator");
							else
								OnMessageRecieved(data);
						}
					}
                }
            }

            #region events

            public event delegates.OnInfoMessageHandler InfoMessageChanged;

            protected virtual void OnInfoMessage(string message)
            {
                if (InfoMessageChanged != null)
                {
                    Control target = InfoMessageChanged.Target as Control;
                    if (target != null && target.InvokeRequired)
                        target.Invoke(InfoMessageChanged, new object[] { message });
                    else
                        InfoMessageChanged(message);
                }
            }

            public event delegates.OnMessageRecievedHandler MessageRecieved;

            protected virtual void OnMessageRecieved(string message)
            {
                if (MessageRecieved != null)
                {
                    Control target = MessageRecieved.Target as Control;
                    if (target != null && target.InvokeRequired)
                        target.Invoke(MessageRecieved, new object[] { message });
                    else
                        MessageRecieved(message);
                }
            }

            #endregion
        }

        private void write()
        {
            StreamWriter writer;
            string __message = "";
            
            while (!quit)
            {
                if (message.Count > 0)
                {
					for (int i = clients.Count - 1; i >= 0; i--)
                    {
						TcpClient client = clients[i];

                        try
                        {
                            //check if the message is empty, of the particular
                            //index of out array is null, if it is then continue
                            if (!string.Equals(message.Peek().Trim(), string.Empty) || !TcpClient.Equals(client, null))
                            {
                                writer = new StreamWriter(client.GetStream());
                                __message = message.Dequeue();
                                writer.WriteLine(__message);

                                writer.Flush();
                            }
                        }
                        catch (Exception e)
                        {
                            OnInfoMessage(e.Message);
                            clients.Remove(client);
                        }
                    }
                }
            }
        }

        private void SetAddress()
        {
            // Get internet IP address
            address = string.Empty;
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString().Equals(Strings.Internet))
                {
                    address = ip.ToString();
                }
            }
            // If there is no internet, use localhost
            if (address.Equals(string.Empty))
            {
                address = Strings.Localhost;
            }
        }

        private void reader_MessageRecieved(string message)
        {
            OnMessageRecieved(message);
        }

        private void reader_InfoMessageChanged(string Message)
        {
            OnInfoMessage(Message);
        }

        #region eventhandlers

        public event delegates.OnInfoMessageHandler InfoMessageChanged;

        protected virtual void OnInfoMessage(string message)
        {
            if (InfoMessageChanged != null)
            {
                Control target = InfoMessageChanged.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(InfoMessageChanged, new object[] { message });
                else
                    InfoMessageChanged(message);
            }
        }

        public event delegates.OnMessageRecievedHandler MessageRecieved;

        protected virtual void OnMessageRecieved(string message)
        {
            if (MessageRecieved != null)
            {
                Control target = MessageRecieved.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(MessageRecieved, new object[] { message });
                else
                    MessageRecieved(message);
            }
        }

        #endregion
    }
}