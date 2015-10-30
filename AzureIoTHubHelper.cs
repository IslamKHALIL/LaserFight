using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserFight.Helpers
{
    public class AzureIoTHubHelper
    {
        private static readonly string _ConnectionString = "<connection string>";

        public event Action<string> Message_Received;

        public AzureIoTHubHelper(string deviceId)
        {
            Receive(deviceId);
        }

        public async Task Receive(string deviceId)
        {
            try
            {
                DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_ConnectionString, deviceId, TransportType.Http1);

                Message receivedMessage;
                string messageData;

                while (true)
                {
                    receivedMessage = await deviceClient.ReceiveAsync();

                    if (receivedMessage != null)
                    {
                        messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                        Message_Received?.Invoke(messageData);
                        await deviceClient.CompleteAsync(receivedMessage);
                    }
                }
            }
            catch (Exception e)
            {
                await Receive(deviceId);
            }
        }

        public async Task Send(string id)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_ConnectionString, id, TransportType.Http1);
            var msg = new Message(Encoding.UTF8.GetBytes(id));
            await deviceClient.SendEventAsync(msg);
        }
    }
}
