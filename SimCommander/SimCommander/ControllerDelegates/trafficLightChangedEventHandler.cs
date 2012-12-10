using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimCommander.SharedObjects;

namespace SimCommander.ControllerDelegates
{
    public delegate void trafficLightChangedEventHandler(string sender, TrafficLightPackage tlp);
}
