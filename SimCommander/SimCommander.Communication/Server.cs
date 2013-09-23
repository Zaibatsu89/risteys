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

        public object lockthis = new object();

        public Server()
        {
            clients = new List<TcpClient>();
            // detect current ip address and uses it
            SetAddress();
			//Console.WriteLine("server adres: " + address);

            //OnInfoMessage(string.Format(Strings.HiIAmController +
            //    " and I serve from {0}:1337 forever", address));

            // start the server
            //server.Start();

            // create a thread witch handels connect requests
            new Thread(new ThreadStart(connect)).Start();

        }

        private void connect()
        {
			Console.WriteLine(string.Format("The server is started on {0}:1337", this.address.ToString()));

            // create our TCPListener object
            server = new TcpListener(IPAddress.Parse(address), Int32.Parse(Strings.Port));
            server.Start();

            while (true)
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

                while (true)
                {
					try
					{
						data = reader.ReadLine();
					}
					catch (IOException)
					{
						Console.WriteLine("Connection lost");
						client.Close();
						break;
					}

					if (data != null)
					{
						if (data != String.Empty)
						{
							if (data.Equals(Strings.Exit))
							{
								Console.WriteLine("Connection lost");
								client.Close();
								break;
							}
							if (data.Equals(Strings.HiIAmSimulator))
								Console.WriteLine("Hi I'am a Simulator");
							else
								OnMessageRecieved(data);
						}
					}
                }
            }

            #region events
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

        public void write(string message)
        {
            StreamWriter writer;
            
			for (int i = clients.Count - 1; i >= 0; i--)
            {
				TcpClient client = clients[i];

                try
                {
                    //check if the message is empty, of the particular
                    //index of out array is null, if it is then continue
                    if (!string.Equals(message, string.Empty) || !TcpClient.Equals(client, null))
                    {
                        writer = new StreamWriter(client.GetStream());
                        writer.WriteLine(message);

                        writer.Flush();
                    }
                }
                catch (Exception e)
                {
					Console.WriteLine(e.Message);
                    clients.Remove(client);
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
        #region eventhandlers
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