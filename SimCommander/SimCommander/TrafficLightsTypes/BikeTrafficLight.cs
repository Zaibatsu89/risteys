using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimCommander.SharedObjects;
using System.Threading;
using SimCommander.Exceptions;

namespace SimCommander.TrafficLichtTypes
{
    class BikeTrafficLight : TrafficLight
    {
        int multiplier;
        //public BikeTrafficLight(string name, ImmutableDictionary<string, int[]> TrafficLightMatrices) :
        public BikeTrafficLight(string name, int multiplier, int[] TrafficLightMatrices) :
            base(10, name, 10, 30, 30, TrafficLightMatrices)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void add()
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (trafficLightMatrix.Length != 64)
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


        public override void TurnLightGreen()
        {
            this.SetTrafficLight(1); // turn the light green
            this.isGreen = true;
            Thread.Sleep(this.rand.Next(this.minGreenTime / multiplier, this.maxGreenTime / multiplier));
            TurnLightOrange();
        }

        //public override void TurnLightOrange(object sender, System.Timers.ElapsedEventArgs args)
        public override void TurnLightOrange()
        {
            this.SetTrafficLight(2);
            Thread.Sleep(this.orangeTime / multiplier);
            TurnLightRed();
        }

        //public override void TurnLightRed(object sender, System.Timers.ElapsedEventArgs args)
        public override void TurnLightRed()
        {
            this.SetTrafficLight(3);
        }

        public override void TurnLightOff()
        {
            this.SetTrafficLight(4);
        }

        protected override void SetTrafficLight(int lightId)
        {
            switch (lightId)
            {
                case 1:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.green));
                    break;
                case 2:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.yellow));
                    break;
                case 3:
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
    }
}
