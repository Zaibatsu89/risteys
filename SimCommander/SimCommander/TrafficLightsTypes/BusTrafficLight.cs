using System.Collections.Generic;
using SimCommander.Exceptions;
using SimCommander.Utils;
using SimCommander.SharedObjects;
using System.Timers;
using System.Threading;

namespace SimCommander.TrafficLichtTypes
{
    class BusTrafficLight : TrafficLight
    {
        //protected Queue<ValuePair> MyTrafficLightMatrices;
        protected int multiplier;

        public BusTrafficLight(string name, int multiplier, ImmutableDictionary<string, int[]> TrafficLightMatrices) :
            base(10, name, 10, 30, 30, TrafficLightMatrices)
        {
            //this.MyTrafficLightMatrices = new Queue<ValuePair>();
            this.multiplier = multiplier;
        }

        #region public members

        /// <summary>
        /// 
        /// </summary>
        public override void add()
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (TrafficLightMatrices[Name].Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            //MyTrafficLightMatrices.Enqueue(new ValuePair(TrafficLightMatrices[dlp.Light], dlp.Destination));
            Bootstrapper.MessageLoop.Enqueue(this.Name + ": " + this.numberOfWaitingEntities);
            // increment the number of waiting entities.
            numberOfWaitingEntities++;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void remove()
        {
            //MyTrafficLightMatrices.Dequeue();

            numberOfWaitingEntities--;
        }

        /// <summary>
        /// used to see what's the direction of (in this case the next bus) the next verhilce
        /// </summary>
        /// <returns>at this moment a wrong type</returns>
        public bool peek()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void TurnLightGreen()
        {
            this.SetTrafficLight(1); // turn the light green
            this.isGreen = true;
            greenTimer.Elapsed += new ElapsedEventHandler(TurnLightOrange); // after the time is elapsed turn the light orange
            //greenTimer.Interval = this.rand.Next(this.minGreenTime/multiplier, this.maxGreenTime/multiplier); 	// generates a random greentime between min and max greentime
            //greenTimer.AutoReset = false;
            //greenTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public override void TurnLightOrange(object sender, ElapsedEventArgs args)
        {
            this.SetTrafficLight(2);
            orangeTimer.Elapsed += new ElapsedEventHandler(TurnLightRed);
            //orangeTimer.Interval = this.orangeTime/multiplier;
            //orangeTimer.AutoReset = false;
            //orangeTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public override void TurnLightRed(object sender, ElapsedEventArgs args)
        {
            this.SetTrafficLight(3);
            this.isGreen = false;
            this.numberOfWaitingEntities = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void TurnLightOff()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lightId"></param>
        protected override void SetTrafficLight(int lightId)
        {
            switch (lightId)
            {
                case 1:
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " GreenTime: " + this.greenTimer.Interval);
                    Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.green));
                    break;
                case 2:
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " YelloTime: " + this.orangeTimer.Interval);
                    Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.yellow));
                    break;
                case 3:
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " RED HAS NO TIMER");
                    Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.red));
                    break;
                default:
                    Bootstrapper.MessageLoop.Enqueue("Invalid LightID");
                    break;
            }
        }

        #endregion
    }
}
