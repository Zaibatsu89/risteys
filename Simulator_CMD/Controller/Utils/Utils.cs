using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.TrafficLichtTypes;
using SharedObject;

namespace Controller.Utils
{
    public class Utils
    {
        // create a public readonly lookup table so that nobody (accidentally) can mashup it
        // private dictionary/lookup table to map a diraction to a trafficLight matrix
        public static readonly ImmutableDictionary<string, int[]> TRAFFICLIGHTMATRIXES = new ImmutableDictionary<string, int[]>(
            new Dictionary<string, int[]>()
            {
                {"johannes", new int[]{26,11,1986}},
                {"johannesc", new int[]{26,11,1986}}
            });

        public static readonly ImmutableDictionary<string, TrafficLight> TRAFFICLIGHTS = new ImmutableDictionary<string, TrafficLight>(
            new SortedDictionary<string, TrafficLight>()
            {
                {"N1", new BusTrafficLight("N1")},
                {"N2", new BusTrafficLight("N2")},
                {"N3", new BusTrafficLight("N3")},
                {"N4", new BusTrafficLight("N4")},
                {"N5", new BusTrafficLight("N5")},
                {"O1", new BusTrafficLight("O1")},
                {"O2", new BusTrafficLight("O2")},
                {"O3", new BusTrafficLight("O3")},
                {"O4", new BusTrafficLight("O4")},
                {"O5", new BusTrafficLight("O5")},
                {"Z1", new BusTrafficLight("Z1")},
                {"Z2", new BusTrafficLight("Z2")},
                {"Z3", new BusTrafficLight("Z3")},
                {"Z4", new BusTrafficLight("Z4")},
                {"Z5", new BusTrafficLight("Z5")},
                {"W1", new BusTrafficLight("W1")},
                {"W2", new BusTrafficLight("W2")},
                {"W3", new BusTrafficLight("W3")},
                {"W4", new BusTrafficLight("W4")},
                {"W5", new BusTrafficLight("W5")}
            });

        internal static int[] addMatrix(int[] trafficLightMatrix, int[] matrix)
        {
            for (int i = 0; i < 64; i++)
            {
                trafficLightMatrix[i] = +matrix[i];
            }
            return trafficLightMatrix;
        }

        internal static bool collisionCheck(ref int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
        {
            int evaluator = -1;

            for (int i = 0; i < 64; i++)
            {
                //evaluator = trafficLightMatrixSource[i] + (trafficLightMatrixEval[i] == 0 ? 0 : 1);
                if (evaluator > 1)
                    return false;

                trafficLightMatrixSource[i] = evaluator;
            }

            return true;
        }

        internal static int[] getTrafficLightMatrix(string startpoint, string endpoint)
        {
            return getTrafficLightMatrix(startpoint + " - " + endpoint);
        }

        internal static int[] getTrafficLightMatrix(string matrixId)
        {
            return TRAFFICLIGHTMATRIXES[matrixId];
        }

        internal static int[] removeMatrix(int[] trafficLightMatrix, int[] matrix)
        {
            for (int i = 0; i < 64; i++)
            {
                trafficLightMatrix[i] = -matrix[i];
            }
            return trafficLightMatrix;
        }
    }
}
