using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimCommander.SharedObjects;

namespace SimCommander.TrafficLichtTypes
{
    class PedestrianTrafficLight : TrafficLight
    {
        public PedestrianTrafficLight(string name, ImmutableDictionary<string, int[]> TrafficLightMatrices) :
            base(10, name, 10, 30, 30, TrafficLightMatrices)
        {

        }


        public override void TurnLightGreen()
        {
            throw new NotImplementedException();
        }

        public override void TurnLightOrange(object sender, System.Timers.ElapsedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public override void TurnLightRed(object sender, System.Timers.ElapsedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public override void TurnLightOff()
        {
            throw new NotImplementedException();
        }

        protected override void SetTrafficLight(int lightId)
        {
            throw new NotImplementedException();
        }
    }
}
