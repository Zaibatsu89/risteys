using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimCommander.TrafficLichtTypes;
using SimCommander.SharedObjects;

namespace SimCommander.Utils
{
    public class Utils
    {

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
            int evaluator;

            for (int i = 0; i < 64; i++)
            {
                //
                evaluator = trafficLightMatrixSource[i] + (trafficLightMatrixEval[i] == 0 ? 0 : 1);
                if (evaluator > 1)
                    return false;

                trafficLightMatrixSource[i] = evaluator;
            }

            return true;
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
