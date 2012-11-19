using System.Collections.Generic;
using Controller.Exceptions;
using Controller.Utils;
using System.Timers;

namespace Controller.TrafficLichtTypes
{
    class BusTrafficLight : TrafficLight
    {
        protected Queue<ValuePair> TrafficLightMatrices;

        public BusTrafficLight(string name) :
            base(10, name, 10, 30, 5)
        {
            TrafficLightMatrices = new Queue<ValuePair>();

        }

        #region public members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dlp"></param>
        public override void add(DetectionLoopPackage dlp)
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (Utils.Utils.TRAFFICLIGHTMATRIXES[dlp.Light].Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            TrafficLightMatrices.Enqueue(new ValuePair(Utils.Utils.TRAFFICLIGHTMATRIXES[dlp.Light], dlp.Destination));

            // increment the number of waiting entities.
            numberOfWaitingEntities++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dlp"></param>
        public override void remove(DetectionLoopPackage dlp)
        {
            TrafficLightMatrices.Dequeue();

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
        public void TurnLightGreen()
        {
            this.SetLight(1); // turn the light green
            greenTimer.Elapsed += new ElapsedEventHandler(TurnLightOrange); // after the time is elapsed turn the light orange
            greenTimer.Interval = this.rand.Next(this.minGreenTime, this.maxGreenTime); 	// generates a random greentime between min and max greentime
            greenTimer.AutoReset = false;
            greenTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void TurnLightOrange(object sender, ElapsedEventArgs args)
        {
            this.SetLight(2);
            orangeTimer.Elapsed += new ElapsedEventHandler(TurnLightRed);
            orangeTimer.Interval = this.orangeTime;
            orangeTimer.AutoReset = false;
            orangeTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void TurnLightRed(object sender, ElapsedEventArgs args)
        {
            this.SetLight(3);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TurnLightOff()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lightId"></param>
        private void SetLight(int lightId)
        {


        }

        #endregion
    }
}
