using batch_service.repository;
using Confluent.Kafka;

namespace batch_service.consumer;

public class BatchConsumerService : IHostedService
{
    private readonly ConsumerConfig consumerConfig;

    private readonly List<string> batch;

    private DateTime lastBatchReceived; 
    
    private readonly IMongoRepositoryFactory mongoRepositoryFactory;
    
    private const int  MAX_BATCH_SIZE = 1000, MAX_POLLING_INTERVAL_MS = 15000;
    
    public BatchConsumerService(
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

    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

        consumerBuilder.Subscribe("battleship_batch_updates");

        Task.Run(() => StartConsumerLoop(consumerBuilder, cancellationToken));
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void StartConsumerLoop(IConsumer<Ignore, string> consumer, CancellationToken cancellationToken)
    {
        
        Console.WriteLine("Started listening to message requests");

        while (true)
        {
            var message = consumer.Consume(TimeSpan.FromMilliseconds(100));

            if (message == null)
            {
                continue;
            }

            if (message.IsPartitionEOF)
            {
                continue;
            }
            
            batch.Add(message.Message.Value);

            bool hasExceededPollingTime =
                DateTime.Now - lastBatchReceived >= TimeSpan.FromMilliseconds(MAX_POLLING_INTERVAL_MS);
            
            if (batch.Count >= MAX_BATCH_SIZE || hasExceededPollingTime)
            {
                SaveBatch(batch);
                
                batch.Clear();
                
                lastBatchReceived = DateTime.Now;
                
            }
        }
    }
    
    private void SaveBatch(List<string> batches)
    {
        var repository = mongoRepositoryFactory.Create();
        
        repository.CreateManyAsync(batches).Wait();
        
        Console.WriteLine($"Saved {batches.Count} new documents");
    }

}
