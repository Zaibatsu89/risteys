using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.TrafficLichtTypes
{
    class CarTrafficLight : TrafficLight
    {
        public CarTrafficLight(string name) :
            base(10, name, 10, 30, 5)
        {

        }
    }
}
