namespace SimCommander.Utils
{
    public class Utils
    {
        public static bool collisionCheck(int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
        {
            int evaluator;

            for (int i = 0; i < 64; i++)
            {

                evaluator = trafficLightMatrixSource[i] + (trafficLightMatrixEval[i] == 0 ? 0 : 1);
                if (evaluator > 1)
                    return false;
            }
            return true;
        }

        public static void addCollisionCheck(ref int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
        {
            int[] evaluator = new int[64];

            for (int i = 0; i < 64; i++)
            {

                evaluator[i] = trafficLightMatrixSource[i] + (trafficLightMatrixEval[i] == 0 ? 0 : 1);
                if (evaluator[i] > 1)
                    return;
            }

            trafficLightMatrixSource = evaluator;
        }

        public static void removeCollisionMatrix(ref int[] trafficLightMatrixSource, int[] trafficLightMatrixEval)
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