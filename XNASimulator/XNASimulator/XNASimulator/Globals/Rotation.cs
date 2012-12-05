using System;

namespace KruispuntGroep6.Simulator.Globals
{
    enum RotationEnum
    { 
        Up, 
        Right, 
        Down, 
        Left 
    }

    struct Rotation
    {
        public static float getRotation(RotationEnum rotation)
        {
            double angle = 0;

            switch (rotation)
            {
                case RotationEnum.Up:
                    angle = 0;
                    break;
                case RotationEnum.Right:
                    angle = 90;
                    break;
                case RotationEnum.Down:
                    angle = 180;
                    break;
                case RotationEnum.Left:
                    angle = 270;
                    break;
            }

            angle = Math.PI * angle / 180.0;
            return (float)angle;
        }
    }
}