using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.Exceptions
{
    class TrafficLightInitializationException : Exception
    {
        public TrafficLightInitializationException(String message) :
            base(message)
        {
        }
    }
}
