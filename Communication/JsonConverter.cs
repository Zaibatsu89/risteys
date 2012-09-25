using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace KruispuntGroep6.Communication
{
    public class JsonConverter
    {
        List<JsonInputObjects> Objects;

        public JsonConverter()
        {
            Objects = new List<JsonInputObjects>();
        }
    }

    public class JsonInputObjects
    {
        public int Time { get; set; }

        public string Type { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}
