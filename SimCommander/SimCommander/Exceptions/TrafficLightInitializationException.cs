using System;

namespace SimCommander.Exceptions
{
    class TrafficLightInitializationException : Exception
    {
        public TrafficLightInitializationException(String message) :
            base(message)
        {

        }
    }
}