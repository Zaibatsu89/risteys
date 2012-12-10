using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SimCommander.SharedObjects;

namespace SimCommander.Communication
{
    public delegate void TimerMsgHandler(string time);
    public delegate void DetectionLoopMsgHandler(DetectionLoopPackage msg);
    public delegate void MultiplierChangedHandler(int Multiplier);


    public class Communication
    {
        private Random rand;
        public Communication()
        {
            rand = new Random();
            //simulate the simulator :P with a TimerMassage
            new Thread(new ThreadStart(MessageEventTest)).Start();
            new Thread(new ThreadStart(DetectioneEventLoopTest)).Start();
        }

        void MessageEventTest()
        {
            Thread.Sleep(2000);
            OnTimeMessage(new DateTime(2012, 11, 21, 16, 47, 25).ToString());
        }
        
        void DetectioneEventLoopTest() 
        {
            while (true)
            {
                // create a delay 2.5 sec that's 0.5sec longer then the TimeMessage is fired
                Thread.Sleep(2500);
                //OnDetectionLoopMessage("hello message!!");

                String[] s = new String[] { "N1", "N3", "N2", "S1", "S4", "S5", "E3", "E1", "E5", "W1", "W2", "W5" };
                String[] s2 = new String[] { "Car", "bus", "bicycle", "pedestrian" };

                DetectionLoopPackage dpl = new DetectionLoopPackage();
                dpl.Light = s[rand.Next(0, 11)];
                dpl.Type = s2[rand.Next(0,3)];
                dpl.Destination = s[rand.Next(0, 11)];
                dpl.IsEmpty = rand.Next(0,1)== 1? "true": "false";
                dpl.Distance = rand.Next(0, 2) == 1 ? "far" : "close";
                OnDetectionLoopMessage(dpl);
            }
        }


        #region events

        public event TimerMsgHandler TimeMessage;

        protected virtual void OnTimeMessage(string msg)
        {
            Control target = TimeMessage.Target as Control;
            if (target != null && target.InvokeRequired)
                target.Invoke(TimeMessage, new object[] { msg });
            else
                TimeMessage(msg);
        }

        public event DetectionLoopMsgHandler DetectionLoopMessage;

        protected virtual void OnDetectionLoopMessage(DetectionLoopPackage msg)
        {
            Control target = DetectionLoopMessage.Target as Control;
            if (target != null && target.InvokeRequired)
                target.Invoke(DetectionLoopMessage, new object[] { msg });
            else
                DetectionLoopMessage(msg);
        }

        public event MultiplierChangedHandler MultiplierChanged;

        protected virtual void OnMultiplierChanged(int multiplier)
        {
            Control target = MultiplierChanged.Target as Control;
            if (target != null && target.InvokeRequired)
                target.Invoke(MultiplierChanged, new object[] { multiplier });
            else
                MultiplierChanged(multiplier);
        }

        #endregion

        
    }
}
