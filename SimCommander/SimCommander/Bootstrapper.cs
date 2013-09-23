using System;
using SimCommander.Communication;
using SimCommander.SharedObjects;

namespace SimCommander
{
    class Bootstrapper
    {
        public TrafficLightController tlc;
        public Communication.Communication com;

        public Bootstrapper()
        {
            tlc = new TrafficLightController();
            tlc.trafficLightChanged += new ControllerDelegates.trafficLightChangedEventHandler(tlc_trafficLightChanged);
            com = new Communication.Communication();
            com.MultiplierChanged += new delegates.MultiplierChangedHandler(com_MultiplierChanged);
            com.TimeMessage += new delegates.TimerMsgHandler(com_TimeMessage);
            com.DetectionLoopMessage += new delegates.DetectionLoopMsgHandler(com_DetectionLoopMessage);
        }

        void tlc_trafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            com.WriteMessage("[" + DynamicJson.Serialize(tlp) + "]");
			Console.WriteLine("[" + DynamicJson.Serialize(tlp) + "]");
        }

        public static void run()
        {
            Bootstrapper b = new Bootstrapper();
        }
        #region eventhandlers
        
        void com_DetectionLoopMessage(DetectionLoopPackage msg)
        {
			Console.WriteLine(msg.ToString());
            tlc.DetectionLoop(msg);
        }

        void com_TimeMessage(string time)
        {
			Console.WriteLine("Time is updated: " + time);
            tlc.StartTime = time;
        }

        void com_MultiplierChanged(int Multiplier)
        {
			Console.WriteLine("Multiplier updated: " + Multiplier.ToString());
            tlc.Multiplier(Multiplier);
        }

        #endregion
    }
}