using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KruispuntGroep6.Communication;
using System.Threading;

namespace XNASimulator
{
    class Communication
    {
		private static Client client;

		public Communication() { }

		public void Begin()
		{
			var clientThread = new Thread(ClientTask);
			clientThread.Start();
		}

		public static void ClientTask()
		{
			client = new Client();
			client.ShowDialog();
		}

        public string getTestMessage()
        {
            string message2 = "n1,red"; //light,state
            string message = "0,car,n3,s6"; //time, type, from, to
            return message;
        }

        public void Listen()
        {
           
        }
    }
}
