using Azure.Messaging.ServiceBus;

namespace WorkerServiceDeviceQueueSender
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _queueName = "incomingrequestsopendoor";
        private readonly string __serviceBusConnString = "Endpoint=sb://gruppo4-pi-cloud.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=eTCoUTZbpTMk7HpOvBIbPO0gBns3yelg4+ASbFFMD8M=";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // the client that owns the connection and can be used to create senders and receivers
                ServiceBusClient client;

                // the sender used to publish messages to the queue
                ServiceBusSender sender;

                client = new ServiceBusClient(__serviceBusConnString);
                sender = client.CreateSender(_queueName);


                string deviceGeneratedCode = "12345";

                // create a message that we can send. UTF-8 encoding is used when providing a string.
                ServiceBusMessage message = new ServiceBusMessage(deviceGeneratedCode);

                // Add device-specific property
                message.ApplicationProperties.Add("DeviceId", "Device1");

                // send the message
                await sender.SendMessageAsync(message);


                Console.WriteLine("Press any key to end the application");
                Console.ReadKey();
            }
        }
    }
}