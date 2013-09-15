namespace SimCommander.SharedObjects
{
    public class TrafficLightPackage
    {
		public enum TrafficLightState
		{
			BLINK,
			GREEN,
			YELLOW,
			RED
		}

        private string _light = "Null";
		private TrafficLightState _state = TrafficLightState.BLINK;

		public TrafficLightPackage(string light, string state)
        {
            _light = light;

			switch (state)
			{
				case "BLINK":
					_state = TrafficLightState.BLINK;
					break;
				case "GREEN":
					_state = TrafficLightState.GREEN;
					break;
				case "YELLOW":
					_state = TrafficLightState.YELLOW;
					break;
				case "RED":
					_state = TrafficLightState.RED;
					break;
			}
        }

        public string light
        {
            get
            {
                return _light;
            }
            set
            {
                _light = value;
            }
        }

        public string state
        {
            get
            {
				switch (_state)
				{
					case TrafficLightState.BLINK:
						return "BLINK";
					case TrafficLightState.GREEN:
						return "GREEN";
					case TrafficLightState.RED:
						return "RED";
					case TrafficLightState.YELLOW:
						return "YELLOW";
					default:
						return "NULL";
				}
            }
            set
            {
				switch (value)
				{
					case "BLINK":
						_state = TrafficLightState.BLINK;
						break;
					case "GREEN":
						_state = TrafficLightState.GREEN;
						break;
					case "YELLOW":
						_state = TrafficLightState.YELLOW;
						break;
					case "RED":
						_state = TrafficLightState.RED;
						break;
				}
            }
        }
    }
}