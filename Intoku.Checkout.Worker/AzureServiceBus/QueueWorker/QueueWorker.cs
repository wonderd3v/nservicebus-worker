using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;


namespace Intoku.Checkout.Worker.AzureServiceBus.QueueWorker
{
    public abstract class QueueWorker<TMessage> : BackgroundService
    {
        protected ILogger<QueueWorker<TMessage>> Logger { get; }
        protected IConfiguration Configuration { get; }

        protected QueueWorker(IConfiguration configuration, ILogger<QueueWorker<TMessage>> logger)

        {
            Configuration = configuration;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var queueName = Configuration.GetValue<string>("SERVICEBUS_QUEUE_NAME");

            //const string ServiceBusConnectionString = "Endpoint=sb://intoku.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=R+tOdMuqU01TJpE6xcT9hwARKROGNfdZBmnMmzR3HZ8=";
            
            //var managementClient = new ManagementClient(ServiceBusConnectionString);
            //managementClient.CreateQueueAsync(queueName).GetAwaiter().GetResult();

            const string queueName = "ORDER_PROCESSOR_STRIPE";
            var messageProcessor = CreateServiceBusProcessor(queueName);

            messageProcessor.ProcessMessageAsync += HandleMessageAsync;
            messageProcessor.ProcessErrorAsync += HandleReceivedExceptionAsync;

            Logger.LogInformation($"Starting message pump on queue {queueName} in namespace {messageProcessor.FullyQualifiedNamespace}");
            await messageProcessor.StartProcessingAsync(stoppingToken);
            Logger.LogInformation("Message pump started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Logger.LogInformation("Closing message pump");
            await messageProcessor.CloseAsync(cancellationToken: stoppingToken);
            Logger.LogInformation("Message pump closed : {Time}", DateTimeOffset.UtcNow);
        }

        private ServiceBusProcessor CreateServiceBusProcessor(string queueName)
        {
            var serviceBusClient = ServiceBusAuthentication.AuthenticateToAzureServiceBus(Configuration, Logger);
           
            var messageProcessor = serviceBusClient.CreateProcessor(queueName);
            return messageProcessor;
        }
        private async Task HandleMessageAsync(ProcessMessageEventArgs processMessageEventArgs)
        {
            try
            {
                var rawMessageBody = Encoding.UTF8.GetString(processMessageEventArgs.Message.Body.ToArray());
                Logger.LogInformation("Received message {MessageId} with body {MessageBody}",
                    processMessageEventArgs.Message.MessageId, rawMessageBody);

                var message = JsonConvert.DeserializeObject<TMessage>(rawMessageBody);
                if (message != null)
                {
                    await ProcessMessage(message, processMessageEventArgs.Message.MessageId,
                        processMessageEventArgs.Message.ApplicationProperties,
                        processMessageEventArgs.CancellationToken);
                }
                else
                {
                    Logger.LogError(
                        "Unable to deserialize to message contract {ContractName} for message {MessageBody}",
                        typeof(TMessage), rawMessageBody);
                }

                Logger.LogInformation("Message {MessageId} processed", processMessageEventArgs.Message.MessageId);

                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle message");
            }
        }

        private Task HandleReceivedExceptionAsync(ProcessErrorEventArgs exceptionEvent)
        {
            Logger.LogError(exceptionEvent.Exception, "Unable to process message");
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessage(TMessage message, string messageId, IReadOnlyDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
