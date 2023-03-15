﻿using batch_service.repository;
using Confluent.Kafka;

namespace batch_service.consumer;

public class BatchConsumerService : AbstractConsumerService, IHostedService
{
    
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


    public BatchConsumerService(IConfiguration _configuration, IMongoRepositoryFactory _mongoRepositoryFactory) 
        : base(_configuration, _mongoRepositoryFactory)
    {
    }

    protected override void DoBatchAction(List<string> batchData)
    {
        var repository = mongoRepositoryFactory.Create();
        
        repository.CreateManyAsync(batchData).Wait();
        
        Console.WriteLine($"Saved {batchData.Count} new documents");
    }
}
