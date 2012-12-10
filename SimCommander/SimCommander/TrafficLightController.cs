using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SimCommander.SharedObjects;
using SimCommander.TrafficLichtTypes;
using SimCommander.ControllerDelegates;
using form = System.Windows.Forms;

namespace SimCommander
{
    class TrafficLightController
    {
        protected Queue<DetectionLoopPackage> DetectionQueue; //needs to be changed into Queue<DetectionLoopMessage> DetectionQueue;
        protected bool interrupt;
        protected int[] ControllerMatrix;
        protected int multiplier;
        protected Timer clock;
        protected DateTime time;

        #region immutable trafficlight dictionaries

        /// <summary>
        /// An immutable dictionare with diraction layout's based on matrices.
        /// </summary>
        public ImmutableDictionary<string, int[]> TRAFFICLGHTMATRICES;

        /// <summary>
        /// An immutable dictionary witch contains all the traffic light's.
        /// </summary>
        public ImmutableDictionary<string, TrafficLight> TRAFFICLIGHTS;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TrafficLightController()
        {
            DetectionQueue = new Queue<DetectionLoopPackage>();
            interrupt = false;
            ControllerMatrix = new int[64];
            multiplier = 1;
            Quit = false;
            time = new DateTime(1970, 01, 01, 00, 00, 00);

            initTrafficLights();

            new Thread(new ThreadStart(processDetectionMessage)).Start();
            new Thread(new ThreadStart(produceTrafficLightMessage)).Start();

        }

        /// <summary>
        /// 
        /// </summary>
        private void initTrafficLights()
        {
            #region An immutable dictionare with diraction layout's based on matrices.
            /// <summary>
            /// An immutable dictionare with diraction layout's based on matrices.
            /// </summary>
            TRAFFICLGHTMATRICES = new ImmutableDictionary<string, int[]>(
                new Dictionary<string, int[]>()
            {
                {"N1", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 1, 0, 0, 0, 0, 0, 0, 
	                                1, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 1, 0, 0, 0, 0, 
	                                0, 1, 0, 1, 1, 0, 0, 0, 
	                                0, 1, 0, 0, 1, 1, 0, 0, 
	                                0, 1, 0, 0, 0, 1, 1, 1, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
               {"N2", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                1, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 1, 0, 0, 0, 0, 
	                                0, 1, 0, 1, 1, 0, 0, 0, 
	                                0, 1, 0, 0, 1, 1, 0, 0, 
	                                0, 1, 0, 0, 0, 1, 1, 1, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"N3", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 1, 0, 0, 0, 0, 
	                                1, 1, 1, 1, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0}},
                {"N4", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 1, 0, 0, 0, 
	                                0, 0, 0, 0, 1, 0, 0, 0, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"N5", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 1, 1, 
	                                0, 0, 0, 0, 0, 0, 0, 0}},
                {"E1", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                1, 1, 1, 1, 1, 1, 1, 1, 
	                                0, 0, 0, 0, 1, 1, 0, 0, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"E2", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                1, 1, 1, 1, 1, 0, 1, 0, 
	                                0, 0, 0, 0, 1, 1, 1, 1, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"E3", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 1, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0}},
                {"E4", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                1, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 0, 0, 1, 1, 1, 1, 1, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0}},
                {"E5", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 1, 1, 1, 1, 1, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"S1", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                1, 1, 0, 0, 0, 0, 1, 0, 
	                                0, 1, 1, 0, 0, 0, 1, 0, 
	                                0, 0, 1, 1, 0, 0, 1, 0, 
	                                0, 0, 0, 1, 1, 0, 1, 0, 
	                                0, 0, 0, 0, 1, 1, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 1, 
	                                0, 0, 0, 0, 0, 0, 1, 0}},
                {"S2", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                1, 1, 0, 0, 0, 0, 1, 0, 
	                                0, 1, 1, 0, 0, 0, 1, 0, 
	                                0, 0, 1, 1, 0, 0, 1, 0, 
	                                0, 0, 0, 1, 1, 0, 1, 0, 
	                                0, 0, 0, 0, 1, 1, 0, 0, 
	                                0, 0, 0, 0, 0, 1, 1, 1, 
	                                0, 0, 0, 0, 0, 1, 0, 0}},
                {"S3", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 1, 1, 1, 1, 
	                                0, 0, 0, 0, 1, 0, 0, 0}},
                {"S4", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 1, 1, 0, 
	                                0, 0, 0, 0, 0, 1, 0, 0, 
	                                0, 0, 0, 0, 1, 1, 0, 0, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 0, 1, 0, 0, 0, 0, 
	                                0, 0, 0, 1, 0, 0, 0, 0}},
                {"S5", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                1, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 0, 0, 0, 0, 0}},
                {"W1", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 1, 1, 0, 
	                                0, 0, 0, 0, 1, 1, 0, 0, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                1, 1, 1, 1, 1, 1, 1, 1, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"W2", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 1, 1, 0, 
	                                0, 0, 0, 0, 1, 1, 0, 0, 
	                                0, 0, 0, 1, 1, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                1, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 1, 1, 1, 1, 1, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"W3", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                1, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 0, 0, 0, 0, 0, 0}},
                {"W4", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                1, 1, 0, 0, 0, 0, 0, 0, 
	                                0, 1, 1, 0, 0, 0, 0, 0, 
	                                0, 0, 1, 1, 0, 0, 0, 0, 
	                                0, 0, 0, 1, 1, 1, 1, 1, 
	                                0, 0, 0, 0, 0, 0, 0, 0}},
                {"W5", new int[]{// n0 n1 n2 n3 n4 n5 n6 n7
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 1, 0, 
	                                1, 1, 1, 1, 1, 1, 1, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0, 
	                                0, 0, 0, 0, 0, 0, 0, 0}}
            });

            #endregion

            #region old trafficlights
            /// <summary>
            /// An immutable dictionary witch contains all the traffic light's.
            /// </summary>
            //TRAFFICLIGHTS = new ImmutableDictionary<string, TrafficLight>(
            //    new SortedDictionary<string, TrafficLight>()
            //{
            //    {"N1", new BusTrafficLight("N1", multiplier, TRAFFICLGHTMATRICES)},
            //    {"N2", new BusTrafficLight("N2", multiplier, TRAFFICLGHTMATRICES)},
            //    {"N3", new BusTrafficLight("N3", multiplier, TRAFFICLGHTMATRICES)},
            //    {"N4", new BusTrafficLight("N4", multiplier, TRAFFICLGHTMATRICES)},
            //    {"N5", new BusTrafficLight("N5", multiplier, TRAFFICLGHTMATRICES)},
            //    {"E1", new BusTrafficLight("E1", multiplier, TRAFFICLGHTMATRICES)},
            //    {"E2", new BusTrafficLight("E2", multiplier, TRAFFICLGHTMATRICES)},
            //    {"E3", new BusTrafficLight("E3", multiplier, TRAFFICLGHTMATRICES)},
            //    {"E4", new BusTrafficLight("E4", multiplier, TRAFFICLGHTMATRICES)},
            //    {"E5", new BusTrafficLight("E5", multiplier, TRAFFICLGHTMATRICES)},
            //    {"S1", new BusTrafficLight("S1", multiplier, TRAFFICLGHTMATRICES)},
            //    {"S2", new BusTrafficLight("S2", multiplier, TRAFFICLGHTMATRICES)},
            //    {"S3", new BusTrafficLight("S3", multiplier, TRAFFICLGHTMATRICES)},
            //    {"S4", new BusTrafficLight("S4", multiplier, TRAFFICLGHTMATRICES)},
            //    {"S5", new BusTrafficLight("S5", multiplier, TRAFFICLGHTMATRICES)},
            //    {"W1", new BusTrafficLight("W1", multiplier, TRAFFICLGHTMATRICES)},
            //    {"W2", new BusTrafficLight("W2", multiplier, TRAFFICLGHTMATRICES)},
            //    {"W3", new BusTrafficLight("W3", multiplier, TRAFFICLGHTMATRICES)},
            //    {"W4", new BusTrafficLight("W4", multiplier, TRAFFICLGHTMATRICES)},
            //    {"W5", new BusTrafficLight("W5", multiplier, TRAFFICLGHTMATRICES)}
            //});

            #endregion

            /// <summary>
            /// An immutable dictionary witch contains all the traffic light's.
            /// </summary>
            TRAFFICLIGHTS = new ImmutableDictionary<string, TrafficLight>(
                new SortedDictionary<string, TrafficLight>()
            {
                {"N1", new BusTrafficLight("N1", multiplier, TRAFFICLGHTMATRICES["N1"])},
                {"N2", new BusTrafficLight("N2", multiplier, TRAFFICLGHTMATRICES["N2"])},
                {"N3", new BusTrafficLight("N3", multiplier, TRAFFICLGHTMATRICES["N3"])},
                {"N4", new BusTrafficLight("N4", multiplier, TRAFFICLGHTMATRICES["N4"])},
                {"N5", new BusTrafficLight("N5", multiplier, TRAFFICLGHTMATRICES["N5"])},
                {"E1", new BusTrafficLight("E1", multiplier, TRAFFICLGHTMATRICES["E1"])},
                {"E2", new BusTrafficLight("E2", multiplier, TRAFFICLGHTMATRICES["E2"])},
                {"E3", new BusTrafficLight("E3", multiplier, TRAFFICLGHTMATRICES["E3"])},
                {"E4", new BusTrafficLight("E4", multiplier, TRAFFICLGHTMATRICES["E4"])},
                {"E5", new BusTrafficLight("E5", multiplier, TRAFFICLGHTMATRICES["E5"])},
                {"S1", new BusTrafficLight("S1", multiplier, TRAFFICLGHTMATRICES["S1"])},
                {"S2", new BusTrafficLight("S2", multiplier, TRAFFICLGHTMATRICES["S2"])},
                {"S3", new BusTrafficLight("S3", multiplier, TRAFFICLGHTMATRICES["S3"])},
                {"S4", new BusTrafficLight("S4", multiplier, TRAFFICLGHTMATRICES["S4"])},
                {"S5", new BusTrafficLight("S5", multiplier, TRAFFICLGHTMATRICES["S5"])},
                {"W1", new BusTrafficLight("W1", multiplier, TRAFFICLGHTMATRICES["W1"])},
                {"W2", new BusTrafficLight("W2", multiplier, TRAFFICLGHTMATRICES["W2"])},
                {"W3", new BusTrafficLight("W3", multiplier, TRAFFICLGHTMATRICES["W3"])},
                {"W4", new BusTrafficLight("W4", multiplier, TRAFFICLGHTMATRICES["W4"])},
                {"W5", new BusTrafficLight("W5", multiplier, TRAFFICLGHTMATRICES["W5"])}
            });

            foreach (KeyValuePair<string, TrafficLight> tl in TRAFFICLIGHTS)
            {
                tl.Value.TrafficLightChanged += new trafficLightChangedEventHandler(OnTrafficLightChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateInfo"></param>
        private void ClockTick(object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            //Bootstrapper.MessageLoop.Enqueue(StartTime.TimeOfDay.ToString());
            Bootstrapper.MessageLoop.Enqueue(time.ToString("h:mm:ss.fff"));
            time = time.AddMilliseconds(1000/multiplier);
        }

        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            set
            {
                if (this.time.ToString() != new DateTime(1970, 01, 01, 00, 00, 00).ToString())
                {
                    // this must be a restart
                    restart();
                    Bootstrapper.MessageLoop.Enqueue("This is a restart");
                }
                else
                {
                    if (value != null)
                    {
                        this.time = DateTime.Parse(value);
                        TimerCallback tc = ClockTick;
                        AutoResetEvent are = new AutoResetEvent(false);
                        clock = new Timer(tc, are, 1000, 1000 / multiplier);
                    }
                }
            }
            get
            {
                return this.time.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Quit
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void DetectionLoop(/*DetectionLoopMessage String*/ DetectionLoopPackage msg)
        {
            DetectionQueue.Enqueue(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        public void Multiplier(int speed)
        {
            if (speed <= 1)
            {
                throw new Exception("Invalid multiplier value given");
            }
            this.multiplier = speed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void restart()
        {
            // call firts quit to close the threads
            Quit = true; 
            DetectionQueue = new Queue<DetectionLoopPackage>();
            interrupt = false;
            ControllerMatrix = new int[64];
            multiplier = 1;
            // before enable the threads again u need to set 
            // quit to false;
            Quit = false;
            
            // start the threads again

            new Thread(new ThreadStart(processDetectionMessage)).Start();
            new Thread(new ThreadStart(produceTrafficLightMessage)).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void processDetectionMessage()
        {
            Bootstrapper.MessageLoop.Enqueue("DEBUG: processDetectionMessage started");

            while (!Quit)
            {

                Thread.Sleep(new Random().Next(150, 330));
                //if(this.DetectionQueue.Count > 0)
                //    Bootstrapper.MessageLoop.Enqueue(this.DetectionQueue.Dequeue());
                // get a detectionLoop message from the queue to process

                if (this.DetectionQueue.Count > 0)
                {
                    DetectionLoopPackage dlm = this.DetectionQueue.Dequeue();

                    // for safety reason we try to change the type to lowercase in 
                    // case one team uses it with a capital char at the front.
                    if(dlm.Distance.ToLower() == "far")
                        this.TRAFFICLIGHTS[dlm.Light.ToUpper()].add();
                    else if (dlm.Distance.ToLower() == "close" && this.TRAFFICLIGHTS[dlm.Light.ToUpper()].NumberOfWaitingEntities > 0)
                    {
                        this.TRAFFICLIGHTS[dlm.Light.ToUpper()].remove();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void produceTrafficLightMessage()
        {
            Thread.CurrentThread.Name= "trafficLightController-Thread";
            //Thread t;
            Bootstrapper.MessageLoop.Enqueue("DEBUG: produceTrafficLightMessage started");
            // get a the first trafficlight to start with.
            TrafficLight old = TRAFFICLIGHTS["N1"];
            while (!Quit)
            {
                foreach (string tl in TRAFFICLIGHTS.Keys)
                {
                    if (tl != old.Name && TRAFFICLIGHTS[tl].CompareTo(old) > 0)
                        old = TRAFFICLIGHTS[tl];
                }

                //if (old.NumberOfWaitingEntities > 0 && old.isGreen == false && Utils.Utils.collisionCheck(ControllerMatrix, old.TrafficLightMatrix))
                if (old.NumberOfWaitingEntities > 0 && old.isGreen == false)
                {
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLightController: " + Thread.CurrentThread.Name + " turns light: " + old.Name + " to green");
                    new Thread(new ThreadStart(old.TurnLightGreen)).Start();
                    //t = new Thread(new ParameterizedThreadStart(old.TurnLightGreen));
                    ////t.Start("hallo");
                    //t.Start(new object[] { "Hallo" });
                    // FIX: Otherwise the same trafficlight will be set again before it sets teh isGreen flag to true
                    Thread.Sleep(250);
                }
                // reset the current selected trafficlight to the first one in the dictionary
                old = TRAFFICLIGHTS["N1"];
            }
        }

        // create an event to send trafficlightChanges

        public event trafficLightChangedEventHandler trafficLightChanged;

        protected virtual void OnTrafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            form.Control target = trafficLightChanged.Target as form.Control;
            if (target != null && target.InvokeRequired)
                target.Invoke(trafficLightChanged, new object[] {sender, tlp });
            else
                trafficLightChanged(sender, tlp);
        }
    }
}
