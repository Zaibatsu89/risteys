using SimCommander.SharedObjects;

namespace SimCommander.Communication
{
    public class delegates
    {
        public delegate void TimerMsgHandler(string time);
        public delegate void DetectionLoopMsgHandler(DetectionLoopPackage msg);
        public delegate void MultiplierChangedHandler(int Multiplier);
        public delegate void OnMessageRecievedHandler(string message);
    }
}