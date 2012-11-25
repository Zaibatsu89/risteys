using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.TrafficLichtTypes
{
    class CarTrafficLight : TrafficLight
    {
        public CarTrafficLight(string name) :
            base(10, name, 10, 30, 5)
        {

        }

		public override void add(DetectionLoopPackage dlp)
		{
			base.add(dlp);
		}

		protected override void OnTrafficLightChanged(string sender, TrafficLightPackage tlp)
		{
			base.OnTrafficLightChanged(sender, tlp);
		}

		public override void remove(DetectionLoopPackage dlp)
		{
			base.remove(dlp);
		}

		public override void SetTrafficLight(int lightId)
		{
			throw new NotImplementedException();
		}

		public override void TurnLightGreen()
		{
			throw new NotImplementedException();
		}

		public override void TurnLightOff()
		{
			throw new NotImplementedException();
		}

		public override void TurnLightOrange(object sender, System.Timers.ElapsedEventArgs args)
		{
			throw new NotImplementedException();
		}

		public override void TurnLightRed(object sender, System.Timers.ElapsedEventArgs args)
		{
			throw new NotImplementedException();
		}
    }
}
