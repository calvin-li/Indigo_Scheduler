using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Indigo_Scheduler.Controllers
{
    [Route("check")]
    [ApiController]
    public class AuthController : IndigoController
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> BasicAuth()
        {
            string auth = Request.Headers["Authorization"][0].Split(' ')[1];
            auth = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

            if (BCrypt.Net.BCrypt.Verify(
                    auth,
                    "$2a$10$LFaAEmH4LDFwOPL6MQnjUOWI.fb7rptGNOztTRCkIOvjUu8l5KRFi")
                // username = a, password = API Key
            )
            {
                if (!ApiKeySet)
                {
                    DataClient.DefaultRequestHeaders.Add("x-api-key", auth.Split(":")[1]);
                    ApiKeySet = true;
                }

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
