using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace LaserFight.AzureIoTHubRegister
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "{iothub connection string}";

        static void Main(string[] args)
        {
            AddDeviceAsync("<the device's id that will be given to the player>");
        }

        private async static Task AddDeviceAsync(string deviceId)
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
