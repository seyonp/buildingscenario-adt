using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace EventHubSender
{
    internal class Program
    {
        private const string connectionString = "Endpoint=sb://adt-eh-ns221014.servicebus.windows.net/;SharedAccessKeyName=tempdatahubpolicy;SharedAccessKey=d1GeK4+/z8k6CGRhZBlpZ/+md9tfb5QuPEMXDVRcIuk=;EntityPath=tempdatahub";
        private const string eventHubName = "tempdatahub";
        static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                // Create a batch of events 
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                // Prepare a message 
                dynamic jsonMsg = JObject.FromObject(new { newMsg = "{ \"deviceid\" : \"Temp_07-MT2\", \n \"temp\" : 234.569 }" });
                var jsonString = jsonMsg.newMsg.ToString();
                //Console.WriteLine(value);
                var payload = Encoding.UTF8.GetBytes(jsonString);

                // Add event to the batch. An event is a represented by a collection of bytes and metadata. 
                eventBatch.TryAdd(new EventData(payload));

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                //Console.WriteLine("A batch of 1 event has been published.");
            }
        }
    }
}
