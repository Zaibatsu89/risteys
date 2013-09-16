using System;
using System.Threading;

namespace SimCommander
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(Bootstrapper.run)).Start();
        }
    }
}