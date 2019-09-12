using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.EventGrid.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EventGridSubscriber.Controllers
{
    [Produces("application/json")]
    public class EventGridEventHandlerController : Controller
    {
        [HttpPost]
        [Route("api/EventGridEventHandler")]
        public JObject Post([FromBody]object request)
        {


            var apiKey = "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";
           // var apiKey = "SG.eOd246RaRh - pQZNKYzokew.ozAiqL56DD0CENawIhojzH6h5kyZsaYIQ2WsKRfAILw";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ganeshshirsath@winjit.com", "Ganesh Shirsath"),
                Subject = "Microservice to Microservice Communication",
                PlainTextContent = "Hello, Created WebHook to test Microservice to Microservice Communication!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("ganeshshirsath@winjit.com", "Test User"));
            var response = client.SendEmailAsync(msg); //await client.SendEmailAsync(msg);
            

            //Deserializing the request 
            var eventGridEvent = JsonConvert.DeserializeObject<Library.EventGridEvent[]>(request.ToString())
                .FirstOrDefault();
            var data = eventGridEvent.Data as JObject;

            // Validate whether EventType is of "Microsoft.EventGrid.SubscriptionValidationEvent"
            if (string.Equals(eventGridEvent.EventType, "Microsoft.EventGrid.SubscriptionValidationEvent", StringComparison.OrdinalIgnoreCase))
            {
                var eventData = data.ToObject<Microsoft.Azure.EventGrid.Models.SubscriptionValidationEventData>();
                var responseData = new SubscriptionValidationResponseData
                {
                    ValidationResponse = eventData.ValidationCode
                };

                if (responseData.ValidationResponse != null)
                {
                    return JObject.FromObject(responseData);
                }
               // return new JObject(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted));
            }
            else
            {
                // Handle your custom event
                var eventData = data.ToObject<CustomData>();
                var customEvent = CustomEvent<CustomData>.CreateCustomEvent(eventData);
                return JObject.FromObject(customEvent);

                //return new JObject(new HttpResponseMessage(System.Net.HttpStatusCode.Ambiguous));
            }

            return new JObject(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
        }
    }
}