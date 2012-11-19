using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller
{
    public class TrafficLightPackage
    {
        public enum TrafficLightState
        {
            green = 1,
            yellow,
            red,
            outOfOrder
        }

        private string light = "Null";
        private TrafficLightState state = TrafficLightState.red;

        public string Light
        {
            get
            {
                return light;
            }
            set
            {
                light = value;
            }
        }

        public TrafficLightState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
    }
}
