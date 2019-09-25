using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Indigo_Scheduler.Controllers
{
    [Route("list")]
    [ApiController]
    public class ListController: IndigoController
    {
        [HttpGet]
        public ActionResult<string> ListEvents()
        {
            List<Event> eventList = GetEvents();

            DateTime eventStartDate = new DateTime(2019, 1, 1);
            // Days are off by one for some reason
            EventDate[] events = {
                new EventDate(eventStartDate.AddDays(0)),
                new EventDate(eventStartDate.AddDays(1)),
                new EventDate(eventStartDate.AddDays(2)),
            };

            foreach (Event e in eventList)
            {
                int day = e.dateTime.Day;
                events[day-1].events.Add(e);
            }
            return JsonConvert.SerializeObject(events);
        }
    }
}
