using System;
using Newtonsoft.Json;

namespace Indigo_Scheduler
{
    public class Event
    {
        [JsonProperty]
        internal Guid id;
        [JsonProperty]
        internal string name;
        [JsonProperty]
        internal DateTime dateTime;
        [JsonProperty]
        internal int duration;
        [JsonProperty]
        internal string brief;

        public Event()
        {
        }

        [JsonConstructor]
        public Event(
            Guid id,
            string name,
            DateTime dateTime,
            int duration,
            string brief)
        {
            this.id = id;
            this.name = name;
            this.dateTime = dateTime;
            this.duration = duration;
            this.brief = brief;
        }

        public static bool operator <(Event lhs, Event rhs)
        {
            return lhs.dateTime.Ticks < rhs.dateTime.Ticks;
        }

        public static bool operator >(Event lhs, Event rhs)
        {
            return lhs.dateTime.Ticks > rhs.dateTime.Ticks;
        }

        internal bool ConflictsWith(Event e)
        {
            return ConflictsBefore(e) || e.ConflictsBefore(this);
        }

        internal bool ConflictsBefore(Event e)
        {
            return dateTime.AddMinutes(duration).Ticks > e.dateTime.Ticks;
        }
    }
}