using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using EventGridWebPublisher.Services;

namespace EventGridWebPublisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicroService_EventGrid_AzureFunction : ControllerBase
    {
        private readonly FakeProductRepository _productRepository;
        //public string topicEndpoint = "https://eventgridpoc.southafricanorth-1.eventgrid.azure.net/api/events";
        //string topicKey = "Bd5518E0A+pNVg3M3BzO3uSnvifP6+AYGJdHjLbM8UY=";

        public string topicEndpoint = "https://consoleeventgridtopic.westus2-1.eventgrid.azure.net/api/events";
        string topicKey = "UZi01xLBpzRRtnTaHlAd4HPaJFrHt5qUbnCruvrCj38=";

        public MicroService_EventGrid_AzureFunction(FakeProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // POST MicroService_EventGrid_AzureFunction
        [HttpPost]
        public void Post([FromBody]Product value)
        {
            _productRepository.FakeSource.Add(value);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", topicKey);

            List<CustomEvents<Product>> events = new List<CustomEvents<Product>>();

            var productEvent = new CustomEvents<Product>();
            productEvent.EventType = "AddRecord";

            productEvent.Subject = "Products Data";
            productEvent.Data = new Product() { Name = value.Name, Id = value.Id };

            events.Add(productEvent);
            string jsonContent = JsonConvert.SerializeObject(events);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            httpClient.PostAsync(topicEndpoint, content);
        }


        

        //        [
        //   {   "id": "1807",
        //       "eventType": "recordInserted",
        //       "subject": "myapp/vehicles/motorcycles",
        //       "eventTime": "2017-08-10T21:03:07+00:00",
        //       "data": {     
        //                  "make": "Ducati",
        //                  "model": "Monster"   
        //                },  
        //        "dataVersion": "1.0",   
        //        "metadataVersion": "1",   
        //        "topic": "/subscriptions/327a134d-863e-4349-814d-e18b3e7b8ea4/resourceGroups/ram-dev-poc/providers/Microsoft.EventGrid/topics/EventGridPOC" 
        //   }
        //]

    }

}