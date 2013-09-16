using SimCommander.Communication.Json;
using SimCommander.SharedObjects;
using System.Linq;
using System.Windows.Forms;

namespace SimCommander.Communication
{
    public class Communication
    {
        #region old

        //private Random rand;
        //public Communication()
        //{
        //    rand = new Random();
        //    //simulate the simulator :P with a TimerMassage
        //    new Thread(new ThreadStart(MessageEventTest)).Start();
        //    new Thread(new ThreadStart(DetectioneEventLoopTest)).Start();
        //}

        //void MessageEventTest()
        //{
        //    Thread.Sleep(2000);
        //    OnTimeMessage(new DateTime(2012, 11, 21, 16, 47, 25).ToString());
        //}

        //void DetectioneEventLoopTest() 
        //{
        //    while (true)
        //    {
        //        // create a delay 2.5 sec that's 0.5sec longer then the TimeMessage is fired
        //        Thread.Sleep(2500);
        //        //OnDetectionLoopMessage("hello message!!");

        //        Strings[] s = new Strings[] { "N1", "N3", "N2", "S1", "S4", "S5", "E3", "E1", "E5", "W1", "W2", "W5" };
        //        Strings[] s2 = new Strings[] { "Car", "bus", "bicycle", "pedestrian" };

        //        DetectionLoopPackage dpl = new DetectionLoopPackage();
        //        dpl.Light = s[rand.Next(0, 11)];
        //        dpl.Type = s2[rand.Next(0,3)];
        //        dpl.Destination = s[rand.Next(0, 11)];
        //        dpl.IsEmpty = rand.Next(0,1)== 1? "true": "false";
        //        dpl.Distance = rand.Next(0, 2) == 1 ? "far" : "close";
        //        OnDetectionLoopMessage(dpl);
        //    }
        //}

        #endregion

        Server s;

        public Communication()
        {
            s = new Server();
            s.MessageRecieved += new delegates.OnMessageRecievedHandler(s_MessageRecieved);
            s.InfoMessageChanged += new delegates.OnInfoMessageHandler(s_InfoMessageChanged);
        }

        public void WriteMessage(string Message)
        {
            s.message.Enqueue(Message);
        }

        private void s_InfoMessageChanged(string Message)
        {
            OnInfoMessageRecieved(Message);
        }

        private void s_MessageRecieved(string message)
        {
            var json = DynamicJson.Parse(message);
            var count = ((dynamic[])json).Count();

            switch (GetType(message))
            {
                case "trafficLoop":
                    var dLight = ((dynamic[])json).Select(d => d.light);
                    var dType = ((dynamic[])json).Select(d => d.type);
                    var loop = ((dynamic[])json).Select(d => d.loop);
                    var empty = ((dynamic[])json).Select(d => d.empty);
                    var dTo = ((dynamic[])json).Select(d => d.to);

                    for(int i = 0; i < count; i++)
                        OnDetectionLoopMessage(new DetectionLoopPackage((string)dLight.ElementAt(i), (string)dType.ElementAt(i), (string)loop.ElementAt(i), (string)empty.ElementAt(i), (string)dTo.ElementAt(i)));
                    break;
                case "start":

                    //var starttime = ((dynamic[])json).Select(d => d.starttime);

                    //OnTimeMessage((string)starttime.ElementAt(0));
                    OnTimeMessage(json.starttime);
                    break;
                case "multiplier":

                    //int strMultiplier = json.multiplier;
                    var multiplier = json.multiplier;

                    OnMultiplierChanged((int) multiplier);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the json type of a dynamic json string.
        /// </summary>
        /// <param name="message">String used to contain a dynamic json.</param>
        /// <returns>String used to contain the json type.</returns>
        private static string GetType(string message)
        {
            string jsonType = string.Empty;

            if (message.Contains("state"))
                jsonType = "trafficLight";
            else if (message.Contains("loop"))
                jsonType = "trafficLoop";
            else if (message.Contains("starttime"))
                jsonType = "start";
            else if (message.Contains("multiplier"))
                jsonType = "multiplier";

            return jsonType;
        }

        #region events

        public event delegates.TimerMsgHandler TimeMessage;

        protected virtual void OnTimeMessage(string msg)
        {
            if (TimeMessage != null)
            {
                Control target = TimeMessage.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(TimeMessage, new object[] { msg });
                else
                    TimeMessage(msg);
            }
        }

        public event delegates.DetectionLoopMsgHandler DetectionLoopMessage;

        protected virtual void OnDetectionLoopMessage(DetectionLoopPackage msg)
        {
            if (DetectionLoopMessage != null)
            {
                Control target = DetectionLoopMessage.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(DetectionLoopMessage, new object[] { msg });
                else
                    DetectionLoopMessage(msg);
            }
        }

        public event delegates.MultiplierChangedHandler MultiplierChanged;

        protected virtual void OnMultiplierChanged(int multiplier)
        {
            if (MultiplierChanged != null)
            {
                Control target = MultiplierChanged.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(MultiplierChanged, new object[] { multiplier });
                else
                    MultiplierChanged(multiplier);
            }
        }

        public event delegates.OnInfoMessageHandler InfoMessageChanged;

        protected virtual void OnInfoMessageRecieved(string InfoMessage)
        {
            if (InfoMessageChanged != null)
            {
                Control target = InfoMessageChanged.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(InfoMessageChanged, new object[] { InfoMessage });
                else
                    InfoMessageChanged(InfoMessage);
            }
        }

        #endregion
    }
}