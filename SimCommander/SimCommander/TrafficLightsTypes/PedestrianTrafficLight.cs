﻿using SimCommander.Exceptions;
using SimCommander.SharedObjects;
using System;
using System.Threading;

namespace SimCommander.TrafficLichtTypes
{
    class PedestrianTrafficLight : TrafficLight
    {
        int multiplier;
        //public PedestrianTrafficLight(string name, ImmutableDictionary<string, int[]> TrafficLightMatrices) :
        public PedestrianTrafficLight(string name, int multiplier, int[] TrafficLightMatrices) :
            base(50, name, 10, 30, 30, TrafficLightMatrices)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void add()
        {
            // to check if the matrix has the right dimention of 8*8=64 element.
            if (TrafficLightMatrix.Length != 64)
                throw new InvalidTrafficLightMatrix("the number of an trafficLightMatrix needs to represent exectly 64 element");

            //MyTrafficLightMatrices.Enqueue(new ValuePair(TrafficLightMatrices[dlp.Light], dlp.Destination));
            // increment the number of waiting entities.
            numberOfWaitingEntities++;
            //Bootstrapper.MessageLoop.Enqueue(this.Name + ": " + this.numberOfWaitingEntities);
			//Console.WriteLine(this.Name + ": " + this.numberOfWaitingEntities);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void remove()
        {
            //MyTrafficLightMatrices.Dequeue();

            numberOfWaitingEntities--;
            //Bootstrapper.MessageLoop.Enqueue(this.Name + ": " + this.numberOfWaitingEntities);
			//Console.WriteLine(this.Name + ": " + this.numberOfWaitingEntities);
        }

        public override void TurnLightGreen()
        {
            this.isGreen = true;
            this.SetTrafficLight(1); // turn the light green
            Thread.Sleep(this.rand.Next(this.minGreenTime / multiplier, this.maxGreenTime / multiplier));
            TurnLightOrange();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TurnLightGreen(object str)
        {
            object[] o = (object[])str;

            //Bootstrapper.MessageLoop.Enqueue((string)o[0]);

			//Console.WriteLine((string)o[0]);

            this.isGreen = true;
            this.SetTrafficLight(1); // turn the light green
            Thread.Sleep(this.rand.Next(this.minGreenTime / multiplier, this.maxGreenTime / multiplier));
            TurnLightOrange();
            //greenTimer.Elapsed += new ElapsedEventHandler(TurnLightOrange); // after the time is elapsed turn the light orange
            //greenTimer.Interval = this.rand.Next(this.minGreenTime/multiplier, this.maxGreenTime/multiplier); 	// generates a random greentime between min and max greentime
            //greenTimer.AutoReset = false;
            //greenTimer.Enabled = true;
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
            this.isGreen = false;
            this.numberOfWaitingEntities = 0;
        }

        public override void TurnLightOff()
        {
            throw new NotImplementedException();
        }

        protected override void SetTrafficLight(int lightId)
        {
            switch (lightId)
            {
                case 1:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.GREEN.ToString()));
                    break;
                case 2:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.YELLOW.ToString()));
                    break;
                case 3:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.RED.ToString()));
                    break;
                case 4:
                    OnTrafficLightChanged(this.Name, new TrafficLightPackage(this.Name, TrafficLightPackage.TrafficLightState.BLINK.ToString()));
                    break;
                default:
                    //Bootstrapper.MessageLoop.Enqueue("Invalid LightID");
					Console.WriteLine("Invalid LightID");
                    break;
            }
        }
    }
}