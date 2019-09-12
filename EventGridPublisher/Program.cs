using Library;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventGridPublisher
{
    class Program
    {
        static void Main(string[] args)
        {


            //var apiKey = "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";
            //var client = new SendGridClient(apiKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("ganeshshirsath@winjit.com", "Ganesh Shirsath"),
            //    Subject = "Hello World from the SendGrid CSharp SDK!",
            //    PlainTextContent = "Hello, Email!",
            //    HtmlContent = "<strong>Hello, Email!</strong>"
            //};
            //msg.AddTo(new EmailAddress("ganeshshirsath@winjit.com", "Test User"));
            //var response = client.SendEmailAsync(msg);



            // Execute().Wait();


            //            var msg = new SendGridMessage();

            //            msg.SetFrom(new EmailAddress("dx@example.com", "SendGrid DX Team"));

            //            var recipients = new List<EmailAddress>
            //{
            //    new EmailAddress("ganeshshirsath@winjit.com", "Ganesh Shirsath"),
            //    new EmailAddress("santoshv@winjit.com", "Santosh V")
            //};
            //            msg.AddTos(recipients);

            //            msg.SetSubject("Testing the SendGrid C# Library");

            //            msg.AddContent(MimeType.Text, "Hello World plain text!");
            //            msg.AddContent(MimeType.Html, "<p>Hello World!</p>");


            var newEvent = PublishEventGridEvent();
            newEvent.Wait();
            Console.WriteLine(newEvent.Result.Content.ReadAsStreamAsync().Result);

            Console.ReadKey();
        }

        private static async Task<HttpResponseMessage> PublishEventGridEvent()
        {
            var eventTopicEndpoint = "https://console2eventgridtopic.westus2-1.eventgrid.azure.net/api/events";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "TInqQ9rqZrnj0nQwcZPGIq/eJ0GgSiwp55dbc5Ih0bc=");
            var customData = new CustomData() { Name = "John Doe", Age = 32, Height = 176, Weight = 76 };
            var customEventData = CustomEvent<CustomData>.CreateCustomEvent(customData);
            var jsonContent = JsonConvert.SerializeObject(customEventData);
            var httpRequestContent = new StringContent("[" + jsonContent + "]", Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(eventTopicEndpoint, httpRequestContent);
        }


        static async Task Execute()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ganeshshirsath@winjit.com", "Ganesh Shirsath"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("ganeshshirsath@winjit.com", "Test User"));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
