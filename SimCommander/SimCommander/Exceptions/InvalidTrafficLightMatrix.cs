using System;

namespace SimCommander.Exceptions
{
    class InvalidTrafficLightMatrix : Exception
    {
        public InvalidTrafficLightMatrix(String message) :
            base(message)
        {

        }
    }
}