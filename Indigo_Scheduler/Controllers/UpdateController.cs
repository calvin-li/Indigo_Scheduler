using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Indigo_Scheduler.Controllers
{
    [Route("update")]
    [ApiController]
    public class UpdateController: IndigoController
    {
        [HttpPost]
        public ActionResult<IEnumerable<string>> UpdateEvent()
        {
            string changedEventRaw;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                changedEventRaw = reader.ReadToEndAsync().Result;
            }

            Event changedEvent = JsonConvert.DeserializeObject<Event>(changedEventRaw);
            List<Event> currentEvents = GetEvents();
            int eventIndex = currentEvents.FindIndex(e => e.id.Equals(changedEvent.id));

            currentEvents.RemoveAt(eventIndex);
            try
            {
                InsertEvent(currentEvents, changedEvent);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            SaveEvents(currentEvents.ToArray());
            currentEvents = GetEvents();
            return Ok();
        }

        private static void InsertEvent(List<Event> currentEvents, Event changedEvent)
        {
            if (changedEvent.dateTime.Ticks < new DateTime(2019, 1, 1).Ticks ||
                changedEvent.dateTime.Ticks > new DateTime(2019, 1, 3).Ticks)
            {
                throw new ArgumentException("Start Date out of bounds");
            }

            int i = 0;
            while (i < currentEvents.Count && changedEvent > currentEvents[i])
            {
                i += 1;
            }

            if (i < currentEvents.Count && changedEvent.ConflictsBefore(currentEvents[i]) ||
                i > 0 && currentEvents[i-1].ConflictsBefore(changedEvent))
            {
                throw new ArgumentException("Modified event conflicts with current schedule");
            }

            currentEvents.Insert(i, changedEvent);
        }
    }
}
