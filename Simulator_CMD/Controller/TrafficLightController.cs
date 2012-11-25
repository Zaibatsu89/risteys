using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    public class TrafficLightController : IDisposable
    {

        private int[] trafficLightMatrix;
        private bool quit;

        // Trafficlight with the highest priority 
        //private string trafficLightID;

        public TrafficLightController()
        {
            // create a 8*8 matrix with the default int value of zero
            trafficLightMatrix = new int[64];
            // create a new thread which will handle the priority of the trafficlights
            new Thread(new ThreadStart(trafficLightBroker)).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void trafficLightBroker()
        {
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
        /// 
        /// </summary>
        /// <param name="dlp"></param>
        public void detactionLoopMessage(DetectionLoopPackage dlp)
        {
            if (dlp.Type.ToLower() == "far")
            {
                Utils.Utils.TRAFFICLIGHTS[dlp.Light].add(dlp);
            }
            else if (Utils.Utils.TRAFFICLIGHTS[dlp.Light].isGreen && dlp.Type.ToLower() == "close")
                Utils.Utils.TRAFFICLIGHTS[dlp.Light].remove(dlp);
        }

        public void Dispose()
        {
            this.quit = true;
        }
    }
}
