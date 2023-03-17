using batch_service.repository;
using Confluent.Kafka;

namespace batch_service.consumer;

public abstract class AbstractConsumerService
{
    protected readonly ConsumerConfig consumerConfig;

    protected readonly List<string> batch;

    protected  DateTime lastBatchReceived; 
    
    protected readonly IMongoRepositoryFactory mongoRepositoryFactory;
    
    private const int  MAX_BATCH_SIZE = 1000, MAX_POLLING_INTERVAL_MS = 5000;
  
    protected AbstractConsumerService(
        IConfiguration _configuration,
        IMongoRepositoryFactory _mongoRepositoryFactory
    )
    {
        batch = new List<string>();

        mongoRepositoryFactory = _mongoRepositoryFactory;
        
        lastBatchReceived = DateTime.Now;
        
        consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = _configuration.GetValue<string>("Kafka:Server"),
            GroupId = "kafka-dotnet",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SecurityProtocol = SecurityProtocol.Plaintext,
            SaslUsername = _configuration.GetValue<string>("Kafka:Username"),
            SaslPassword = _configuration.GetValue<string>("Kafka:Password")
        };
        
    }
    
    protected void StartConsumerLoop(IConsumer<Ignore, string> consumer, CancellationToken cancellationToken)
    {
        
        Console.WriteLine("Started listening to message requests");

        while (true)
        {
            var message = consumer.Consume(TimeSpan.FromMilliseconds(100));

            bool hasExceededPollingTime =
                DateTime.Now - lastBatchReceived >= TimeSpan.FromMilliseconds(MAX_POLLING_INTERVAL_MS);
            
            if (batch.Count >= MAX_BATCH_SIZE || hasExceededPollingTime)
            {
                lastBatchReceived = DateTime.Now;
                
                if (batch.Count == 0)
                {
                    continue;
                }
                
                DoBatchAction(batch);
                
                Console.WriteLine("finished the batch action ");
                
                batch.Clear();
                
            }
            
            if (message == null)
            {
                continue;
            }

            if (message.IsPartitionEOF)
            {
                continue;
            }
            
            batch.Add(message.Message.Value);

            Console.WriteLine($"added to batch : {batch.Count}");
        }
    }

    protected abstract void DoBatchAction(List<string> batchData);
}
