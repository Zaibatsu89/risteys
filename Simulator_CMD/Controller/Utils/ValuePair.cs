using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.Utils
{
    class ValuePair
    {
        public int[] matrix;
        public string destination;

        public ValuePair(int[] matrix, string destination)
        {
            this.matrix = matrix;
            this.destination = destination;
        }
    }
}
