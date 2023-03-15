using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AzureAppToDeploy
{
    public interface IQueueManager
    {
        Task AddMessageToQueue(QueueMessageModel msg);
    }


    public class QueueManager : IQueueManager
    {
        IConfig _config;
        CloudQueueClient _queueClient;
        CloudStorageAccount _cloudStorageAccount;

        public QueueManager(IConfig config)
        {
            _config = config;
            _cloudStorageAccount = CloudStorageAccount.Parse(_config.AzureStorageConnectionString);
            // Create the queue client
            _queueClient = _cloudStorageAccount.CreateCloudQueueClient();
        }

        public async Task AddMessageToQueue(QueueMessageModel msg)
        {
            // Retrieve a reference to a queue
            CloudQueue queue = _queueClient.GetQueueReference("template");
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue
            var json = JsonConvert.SerializeObject(msg);
            CloudQueueMessage message = new CloudQueueMessage(json);
            await queue.AddMessageAsync(message);
        }
    }
}
