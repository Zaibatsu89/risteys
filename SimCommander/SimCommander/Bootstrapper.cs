using SimCommander.Communication;
using SimCommander.SharedObjects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimCommander
{
    class Bootstrapper
    {
        public TrafficLightController tlc;
        public Communication.Communication com;
        public static Queue<String> MessageLoop = new Queue<string>();
        public static volatile Boolean Quit;
        protected static Form _Statistics;
        protected static SimCommander.Statistics stat;
        private static String _message;

        public Bootstrapper()
        {
            tlc = new TrafficLightController();
            tlc.trafficLightChanged += new ControllerDelegates.trafficLightChangedEventHandler(tlc_trafficLightChanged);
            com = new Communication.Communication();
            com.MultiplierChanged += new delegates.MultiplierChangedHandler(com_MultiplierChanged);
            com.TimeMessage += new delegates.TimerMsgHandler(com_TimeMessage);
            com.DetectionLoopMessage += new delegates.DetectionLoopMsgHandler(com_DetectionLoopMessage);
            com.InfoMessageChanged += new delegates.OnInfoMessageHandler(com_InfoMessageChanged);

            Quit = false;
        }

        void com_InfoMessageChanged(string Message)
        {
            Bootstrapper.MessageLoop.Enqueue(Message);
        }

        void tlc_trafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            //Bootstrapper.MessageLoop.Enqueue("trafficlightChanged eventhandler triggerd: " + tlp.Light +" changed into: " + tlp.State);
            //com.WriteMessage("trafficlightChanged eventhandler triggerd: " + tlp.Light +" changed into: " + tlp.State);
            com.WriteMessage("[" + SimCommander.Communication.Json.DynamicJson.Serialize(tlp) + "]");
            Bootstrapper.MessageLoop.Enqueue("[" + SimCommander.Communication.Json.DynamicJson.Serialize(tlp) + "]");
        }

        public static void run()
        {
            Bootstrapper b = new Bootstrapper();

            while (!b.tlc.Quit)
            {
                if (MessageLoop.Count > 0)
                {
                    _message = MessageLoop.Dequeue();
                    Console.WriteLine(_message);
                    if (_Statistics != null)
                        Console.WriteLine("Statistics");

                }

                if (!b.tlc.Quit)
                    b.tlc.Quit = Quit;
            }
        }

        public static void GetStaticsFrom(Form Statistics)
        {
            _Statistics = Statistics;
            stat = (SimCommander.Statistics)Statistics;
        }

        #region eventhandlers
        
        void com_DetectionLoopMessage(DetectionLoopPackage msg)
        {
            Bootstrapper.MessageLoop.Enqueue(msg.ToString());
            tlc.DetectionLoop(msg);
        }

        void com_TimeMessage(string time)
        {
            Bootstrapper.MessageLoop.Enqueue("Time is updated: " + time);
            tlc.StartTime = time;
        }

        void com_MultiplierChanged(int Multiplier)
        {
            Bootstrapper.MessageLoop.Enqueue("Multiplier updated: " +Multiplier.ToString());
            tlc.Multiplier(Multiplier);
        }

        #endregion
    }
}