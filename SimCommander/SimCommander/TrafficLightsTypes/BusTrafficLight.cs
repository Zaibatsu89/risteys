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

        //public BusTrafficLight(string name, int multiplier, ImmutableDictionary<string, int[]> TrafficLightMatrices) :
        public BusTrafficLight(string name, int multiplier, int[] TrafficLightMatrices) :
            base(10, name, 10, 30, 30, TrafficLightMatrices)
        {
            //this.MyTrafficLightMatrices = new Queue<ValuePair>();
            this.multiplier = multiplier;
        }

        #region public members

        /// <summary>
        /// adds a entity from the trafficlight list 
        /// </summary>
        public override void add()
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (trafficLightMatrix.Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            //MyTrafficLightMatrices.Enqueue(new ValuePair(TrafficLightMatrices[dlp.Light], dlp.Destination));
            // increment the number of waiting entities.
            numberOfWaitingEntities++;
            Bootstrapper.MessageLoop.Enqueue(this.Name + ": " + this.numberOfWaitingEntities);
        }

        /// <summary>
        /// removes a entity from the trafficlight list
        /// </summary>
        public override void remove()
        {
            //MyTrafficLightMatrices.Dequeue();

            numberOfWaitingEntities--;
            Bootstrapper.MessageLoop.Enqueue(this.Name + ": " + this.numberOfWaitingEntities);
        }

        // TODO: needs to be implemented
        ///// <summary>
        ///// used to see what's the direction of (in this case the next bus) the next verhilce
        ///// </summary>
        ///// <returns>at this moment a wrong type</returns>
        //public bool peek()
        //{
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        public override void TurnLightGreen()
        {
            this.isGreen = true;
            this.SetTrafficLight(1); // turn the light green
            Thread.Sleep(this.rand.Next(this.minGreenTime / multiplier, this.maxGreenTime / multiplier));
            TurnLightOrange();
            //greenTimer.Elapsed += new ElapsedEventHandler(TurnLightOrange); // after the time is elapsed turn the light orange
            //greenTimer.Interval = this.rand.Next(this.minGreenTime/multiplier, this.maxGreenTime/multiplier); 	// generates a random greentime between min and max greentime
            //greenTimer.AutoReset = false;
            //greenTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TurnLightGreen(object str)
        {
            object[] o = (object[])str;

            Bootstrapper.MessageLoop.Enqueue((string)o[0]);

            this.isGreen = true;
            this.SetTrafficLight(1); // turn the light green
            Thread.Sleep(this.rand.Next(this.minGreenTime / multiplier, this.maxGreenTime / multiplier));
            TurnLightOrange();
            //greenTimer.Elapsed += new ElapsedEventHandler(TurnLightOrange); // after the time is elapsed turn the light orange
            //greenTimer.Interval = this.rand.Next(this.minGreenTime/multiplier, this.maxGreenTime/multiplier); 	// generates a random greentime between min and max greentime
            //greenTimer.AutoReset = false;
            //greenTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //public override void TurnLightOrange(object sender, ElapsedEventArgs args)
        public override void TurnLightOrange()
        {
            this.SetTrafficLight(2);
            Thread.Sleep(this.orangeTime / multiplier);
            TurnLightRed();
            //orangeTimer.Elapsed += new ElapsedEventHandler(TurnLightRed);
            //orangeTimer.Interval = this.orangeTime/multiplier;
            //orangeTimer.AutoReset = false;
            //orangeTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //public override void TurnLightRed(object sender, ElapsedEventArgs args)
        public override void TurnLightRed()
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
            this.SetTrafficLight(4);
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
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    //base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.green));
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.green));
                    break;
                case 2:
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " YelloTime: " + this.orangeTimer.Interval);
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    //base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.yellow));
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.yellow));
                    break;
                case 3:
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " RED HAS NO TIMER");
                    //Bootstrapper.MessageLoop.Enqueue("TrafficLight: " + this.Name + " LightID: " + lightId.ToString() + " thread: " + Thread.CurrentThread.Name);
                    //base.OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.red));
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.red));
                    break;
                case 4:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.outOfOrder));
                    break;
                default:
                    Bootstrapper.MessageLoop.Enqueue("Invalid LightID");
                    break;
            }
        }

        #endregion
    }
}
