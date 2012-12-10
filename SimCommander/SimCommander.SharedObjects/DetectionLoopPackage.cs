using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCommander.SharedObjects
{
    public class DetectionLoopPackage
    {
        private string light = "Null";
        private string type = "Null";
        private string distance = "Null";
        private bool isEmpty = true;
        private string destination = "Null";

        public DetectionLoopPackage(string light, string type, string distance, string isEmtpy, string detination)
        {
            this.light = light;
            this.type = type;
            this.distance = distance;
            this.IsEmpty = isEmtpy;
            this.Destination = destination;
        }

        public string getMatrixId()
        {
            return light;
        }

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

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
               type = value;
            }
        }

        public string Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }

        public string IsEmpty
        {
            get
            {
                return isEmpty.ToString();
            }
            set
            {
                if (value == "true")
                    isEmpty = true;
                else
                    isEmpty = false;
            }
        }

        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }
    }
}
