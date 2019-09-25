using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Indigo_Scheduler.Controllers
{
    [EnableCors]
    public class IndigoController : ControllerBase
    {
        public static bool ApiKeySet;
        public static readonly HttpClient DataClient = InitializeDataClient();

        private static HttpClient InitializeDataClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://crud-api.azurewebsites.net")
            };
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        internal async static Task<T> DataCall<T>(Task<T> dataTask)
        {
            try
            {
                return await dataTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        internal static List<Event> GetEvents()
        {
            string response = DataCall(DataClient.GetStringAsync("api/read/events")).Result;
            JObject json = JsonConvert.DeserializeObject<JObject>(response);
            string eventsRaw = json.SelectToken("message").ToString();
            return JsonConvert.DeserializeObject<List<Event>>(eventsRaw);
        }

        internal static void SaveEvents(Event[] events)
        {
            _ = DataCall(DataClient.PutAsJsonAsync(
                "api/update/events",
                events))
                .Result;
        }

        [HttpOptions]
        public ActionResult Options()
        {
            return Ok();
        }
    }

    public class EnableCors : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Microsoft.AspNetCore.Http.IHeaderDictionary headers = context.HttpContext.Response.Headers;
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add("Access-Control-Allow-Headers", "*");
            headers.Add("Access-Control-Allow-Credentials", "*");

            base.OnActionExecuting(context);
        }
    }
}
