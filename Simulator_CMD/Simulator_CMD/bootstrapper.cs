using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Controller;

namespace Simulator_CMD
{
    class bootstrapper
    {

        protected static bool quit = false;
        protected static Queue<IEvent> Event = new Queue<IEvent>();
        protected static Queue<String> MessageLoop = new Queue<string>();

        private TrafficLightController tlc;

        /// <summary>
        /// standerd constructor: initialize the different components of the controller
        /// </summary>
        public bootstrapper()
        {
            tlc = new TrafficLightController();
        }

        /// <summary>
        /// spin's up the controller 
        /// </summary>
        public static void run()
        {
            // create the work thread
            new Thread(new ThreadStart(new bootstrapper().start)).Start();
            // start the eventloop
            while (!quit )
            {
                if(Event.Count >0)
                    Event.Dequeue().Execute();
                if(MessageLoop.Count > 0)
                    Console.WriteLine(MessageLoop.Dequeue());
            }

            Console.WriteLine("Goodbye");
            Thread.Sleep(500);
        }

        /// <summary>
        /// under constuction
        /// </summary>
        public void start()
        {
            tlc.detactionLoopMessage(new DetectionLoopPackage());
        }

    }
}
