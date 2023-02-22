using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Collections;

namespace AzureIotDeviceOperations.Services
{
    public class ManageIOTDeviceCommunicationService
    {
        static string DeviceConnectionString = "HostName=<yourIotHubName>.azure-devices.net;DeviceId=<yourIotDeviceName>;SharedAccessKey=<yourIotDeviceAccessKey>";
        static DeviceClient Client = null;
        static RegistryManager registryManager;
        static string iotHubConnectionString = "{iot hub connection string}";

        public ManageIOTDeviceCommunicationService(IConfiguration configuration)
        {

        }

        public static async void InitClient()
        {
            try
            {
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString,
                  TransportType.Mqtt);
                await Client.GetTwinAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }

        public static async void ReportConnectivity()
        {
            try
            {

                TwinCollection reportedProperties, connectivity;
                reportedProperties = new TwinCollection();
                connectivity = new TwinCollection();
                connectivity["type"] = "cellular";
                reportedProperties["connectivity"] = connectivity;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task AddTagsAndQuery()
        {
            registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            var twin = await registryManager.GetTwinAsync("myDeviceId");
            var patch =
                @"{
            tags: {
                location: {
                    region: 'US',
                    plant: 'Redmond43'
                }
            }
        }";
            await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);

            var query = registryManager.CreateQuery(
              "SELECT * FROM devices WHERE tags.location.plant = 'Redmond43'", 100);
            var twinsInRedmond43 = await query.GetNextAsTwinAsync();
            

            query = registryManager.CreateQuery("SELECT * FROM devices WHERE tags.location.plant = 'Redmond43' AND properties.reported.connectivity.type = 'cellular'", 100);
            var twinsInRedmond43UsingCellular = await query.GetNextAsTwinAsync();
            
        }

        private static async void SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient)
        {
            try
            {

                double minTemperature = 20;
                double minHumidity = 60;
                Random rand = new Random();

                int messageCount = 0;

                while (messageCount <= 10)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;

                    // Create JSON message  

                    var telemetryDataPoint = new
                    {

                        temperature = currentTemperature,
                        humidity = currentHumidity
                    };

                    string messageString = "";



                    messageString = JsonConvert.SerializeObject(telemetryDataPoint);

                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    // Add a custom application property to the message.  
                    // An IoT hub can filter on these properties without access to the message body.  
                    //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");  

                    // Send the telemetry message  
                    await s_deviceClient.SendEventAsync(message);
                    
                    await Task.Delay(1000 * 10);
                    messageCount++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
