using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SimCommander.Communication;
using SimCommander.SharedObjects;

namespace SimCommander
{
    class Bootstrapper
    {
        public TrafficLightController tlc;
        public Communication.Communication com;
        public static Queue<String> MessageLoop = new Queue<string>();
        public static Boolean Quit;

        public Bootstrapper()
        {
            tlc = new TrafficLightController();
            tlc.trafficLightChanged += new ControllerDelegates.trafficLightChangedEventHandler(tlc_trafficLightChanged);
            com = new Communication.Communication();
            //com.MultiplierChanged += new MultiplierChangedHandler(com_MultiplierChanged);
            com.TimeMessage += new TimerMsgHandler(com_TimeMessage);
            com.DetectionLoopMessage += new DetectionLoopMsgHandler(com_DetectionLoopMessage);

            Quit = false;
        }

        void tlc_trafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            Bootstrapper.MessageLoop.Enqueue("trafficlightChanged eventhandler triggerd: " + tlp.Light +" changed into: " + tlp.State);
        }

        public static void run()
        {

            Bootstrapper b = new Bootstrapper();
            //new Thread(new ThreadStart(b.start)).Start();

            while (!b.tlc.Quit)
            {
                if (MessageLoop.Count > 0)
                    Console.WriteLine(MessageLoop.Dequeue());

                if (!b.tlc.Quit)
                    b.tlc.Quit = Quit;
            }
        }

        public void start()
        {

        }

        #region eventhandlers
        
        void com_DetectionLoopMessage(DetectionLoopPackage msg)
        {
            //Console.WriteLine(msg);
            tlc.DetectionLoop(msg);
        }

        void com_TimeMessage(string time)
        {
            tlc.StartTime = time;
            //tlc.setTime(time);
        }

        void com_MultiplierChanged(int Multiplier)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
