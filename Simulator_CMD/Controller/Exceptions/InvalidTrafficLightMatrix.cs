using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.Exceptions
{
    class InvalidTrafficLightMatrix : Exception
    {
        public InvalidTrafficLightMatrix(String message) :
            base(message)
        {
        }
    }
}
