using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Exceptions;
using Controller;
using System.Timers;
using Controller.ControllerDelegates;
using System.Windows.Forms;

namespace Controller
{
    public abstract class TrafficLight
    {
        protected int typeMultiplier;
        protected string name;

        // FIX: must be public because it is used in the CompareTo methode. It is a copy of
        // the Queue row.count but this is of base type int which has no method CompareTo there
        // for you need to use Int16, Int32 or Int64
        protected int numberOfWaitingEntities;
        protected int[] trafficLightMatrix;

        protected int minGreenTime;
        protected int maxGreenTime;
        protected int orangeTime;

        protected System.Timers.Timer greenTimer;
        protected System.Timers.Timer orangeTimer;

        protected Random rand;


        public TrafficLight(int typeMultiplier, string name, 
        int minGreenTime, int maxGreenTime, int orangeTime)
        {
            // this multiplier is used to multiply the number of entities 
            // how bigger it's result how greater the priority will be.
            this.typeMultiplier = typeMultiplier;
            this.name = name;


            this.minGreenTime = minGreenTime;
            this.maxGreenTime = maxGreenTime;
            this.orangeTime = orangeTime;
            this.greenTimer = new System.Timers.Timer();
            this.orangeTimer = new System.Timers.Timer();
            this.numberOfWaitingEntities = 0;
            this.rand = new Random();

            // do some check's

            if(this.maxGreenTime == 0)
                throw new TrafficLightInitializationException("Max greenTime must be greater than 0");

            if (this.minGreenTime < 0)
                throw new TrafficLightInitializationException("Min greenTime could not be less then 0");

            if (this.orangeTime < 0)
                throw new TrafficLightInitializationException("OrangeTime must be greater than 0");

        }

        #region properties

        // to let other classes determine the green state.
        // especially used by the trafficLight controller class
        public bool isGreen
        {
            get;
            protected set;
        }

        public int[] TrafficLightMatrix
        {
            get;
            private set;
        }

        #endregion
        
        #region IComparable implentation

        public int CompareTo(object obj)
        {
            if(obj is TrafficLight)
            {
                TrafficLight t = (TrafficLight)obj;
                return numberOfWaitingEntities.CompareTo(t.numberOfWaitingEntities);
            }

            throw new ArgumentException("TrafficLight.CompareTo, Error invalid argument type");

        }

        #endregion

        #region public methods

        /// <summary>
        /// DEPRICATED: But still in use and not checked
        /// add's a car, bus, enz. entity to a trafficlight
        /// </summary>
        /// <param name="matrix"></param>
        public virtual void add(DetectionLoopPackage dlp)
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (Utils.Utils.TRAFFICLIGHTMATRIXES[dlp.Light].Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            // increment the number of waiting entities.
            numberOfWaitingEntities++;
        }

        /// <summary>
        /// DEPRICATED: But still in use and not checked
        /// should remove a car, bus, enz. entity to a trafficlight.
        /// </summary>
        /// <param name="matrix"></param>
        public virtual void remove(DetectionLoopPackage dlp)
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (Utils.Utils.TRAFFICLIGHTMATRIXES[dlp.Light].Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            //decrease the number of waiting entities.
            numberOfWaitingEntities--;
        }

        #endregion

        #region public abstract methods

        /// <summary>
        /// Truns this trafficLight green
        /// </summary>
        public abstract void TurnLightGreen();

        /// <summary>
        /// Turns this trafficLight Orange
        /// </summary>
        /// <param name="sender">TrafficLight id</param>
        /// <param name="args">Timer event arguments</param>
        public abstract void TurnLightOrange(object sender, ElapsedEventArgs args);

        /// <summary>
        /// Turns this trafficLight Red
        /// </summary>
        /// <param name="sender">TrafficLight id</param>
        /// <param name="args">Timer event arguments</param>
        public abstract void TurnLightRed(object sender, ElapsedEventArgs args);

        /// <summary>
        /// Turns this trafficLight in it's off (blinking) state
        /// </summary>
        public abstract void TurnLightOff();

        /// <summary>
        /// Used the set a the trafficLight to it's state
        /// </summary>
        /// <param name="lightId">the light state id (1. green 2. orange 3. red 4. off state (blinking))</param>
        public abstract void SetTrafficLight(int lightId);

        #endregion

        #region events

        // event witch will fire when a trafficlight changes state, so 
        // that a message can be send to the simulator.
        public event trafficLightChangedEventHandler TrafficLightChanged;

        /// <summary>
        /// Event implementation which will be triggerd when a trafficlight state change.
        /// </summary>
        /// <param name="sender">trafficlight id </param>
        /// <param name="tlp">trafficlight package witch needs to be send to the simulator</param>
        protected virtual void OnTrafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            if (TrafficLightChanged != null)
            {
                // check if the event message needs to be invoked (in case of using in forms)
                // otherwise fire the event directly.
                Control target = TrafficLightChanged.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(TrafficLightChanged, new object[] { sender, tlp });
                else
                    TrafficLightChanged(sender, tlp);
            }
        }

        #endregion
    }
}
