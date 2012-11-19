using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedObject;
using System.Windows.Forms;
using Controller.ControllerDelegates;
using System.Threading;
using Controller.Utils;

namespace Controller
{
    /// <summary>
    /// Control's the trafficLights
    /// </summary>
    class __TrafficLightController
    {

        protected bool quit;

        /// <summary>
        /// An immutable dictionare with diraction layout's based on matrices.
        /// </summary>
        ImmutableDictionary<string, int[]> TRAFFICLGHTMATRICES = new ImmutableDictionary<string, int[]>(
            new Dictionary<string, int[]>()
            {
                {"johannes", new int[] { 1, 2, 3 }}
            });

        /// <summary>
        /// An immutable dictionary witch contains all the traffic light's.
        /// </summary>
        ImmutableDictionary<string, TrafficLight> TRAFFICLIGHTS = new ImmutableDictionary<string, TrafficLight>(
            new SortedDictionary<string, TrafficLight>()
            {
                {"Johannes", new TrafficLichtTypes.BusTrafficLight("Johannes")}
            });


        /// <summary>
        /// 
        /// </summary>
        public __TrafficLightController()
        {
            // initialize the trafficLights
            foreach (KeyValuePair<string, TrafficLight> tl in TRAFFICLIGHTS)
	        {
		         tl.Value.TrafficLightChanged +=new trafficLightChangedEventHandler(OnTrafficLightChanged); 
	        }

            quit = false;

            // create a different thread so that the incoming messages
            // separately from the outgoing messages can be handled
            new Thread(new ThreadStart(TrafficLightBroker)).Start();
        }

        #region public members
        
        /// <summary>
        /// 
        /// </summary>
        protected void TrafficLightBroker()
        {
            int[] trafficLightMatrix = new int[] {1,2,3};
            // create a local variable a store a sorted copy of the dictionaty in it
            // the dictionary will sort it's values in this case by using .valueCollection and .Values
            SortedDictionary<string, TrafficLight>.ValueCollection sortedDictionary = Utils.Utils.TRAFFICLIGHTS.Values;
            while (!quit)
            {
                if (Utils.Utils.collisionCheck(ref trafficLightMatrix, sortedDictionary.First().TrafficLightMatrix))
                    sortedDictionary.First().TurnLightGreen();
                // sort the dictionary based on de dictionary values using a custom comparer in the @see TrafficLight class
                sortedDictionary = Utils.Utils.TRAFFICLIGHTS.Values;
            }
        }

        /// <summary>
        /// handels the incomming packages for the trafficlights.
        /// </summary>
        /// <param name="dlp">incoming trafficlight message</param>
        public void DetectionLoopMessage(DetectionLoopPackage dlp)
        {
            throw new NotImplementedException("this functionality does not yet exist");
        }
        #endregion

        #region events

        // event witch will fire when a trafficlight changes state, so 
        // that a message can be send to the simulator.
        public event trafficLightChangedEventHandler TrafficLichtChanged;

        /// <summary>
        /// Event implementation which will be triggerd when a trafficlight state change.
        /// (forwards the event from the different trafficlight classes which fire an event when the 
        /// state of a trafficlight changes.)
        /// </summary>
        /// <param name="sender">trafficlight id </param>
        /// <param name="tlp">trafficlight package witch needs to be send to the simulator</param>
        protected virtual void OnTrafficLightChanged(string sender, TrafficLightPackage tlp)
        {
            if (TrafficLichtChanged != null)
            {
                // check if the event message needs to be invoked (in case of using in forms)
                // otherwise fire the event directly.
                Control target = TrafficLichtChanged.Target as Control;
                if (target != null && target.InvokeRequired)
                    target.Invoke(TrafficLichtChanged, new object[] { sender, tlp });
                else
                    TrafficLichtChanged(sender, tlp);
            }
        }

        #endregion
    }
}
