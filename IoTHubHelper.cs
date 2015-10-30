using System;

public class Class1
{
	public static string ReceiveFromAzure()
    {
        DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("<replace>", TransportType.Http1);

        Message receivedMessage;
        string messageData;

        while (true)
        {
            receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
