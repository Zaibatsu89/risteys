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
        static bool collisionCheck(ref int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
        {
            int[] evaluator = new int[64];

            for (int i = 0; i < 64; i++)
            {

                evaluator[i] = trafficLightMatrixSource[i] + (trafficLightMatrixEval[i] == 0 ? 0 : 1);
                if (evaluator[i] > 1)
                    return false;
            }

            trafficLightMatrixSource = evaluator;
            return true;
        }


        static void removeCollisionMatrix(ref int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
        {
            int[] evaluator = new int[64];

            for (int i = 0; i < 64; i++)
            {

                evaluator[i] = trafficLightMatrixSource[i] - (trafficLightMatrixEval[i] == 0 ? 0 : 1);
               
            }

            trafficLightMatrixSource = evaluator;
        }
    }
}
