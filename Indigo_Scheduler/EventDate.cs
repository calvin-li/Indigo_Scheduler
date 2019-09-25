using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Indigo_Scheduler
{
    public class EventDate
    {
        [JsonProperty]
        internal readonly DateTime date;

        [JsonProperty]
        internal List<Event> events;

        public EventDate(DateTime date)
        {
            this.date = date;
            events = new List<Event>();
        }
    }
}
