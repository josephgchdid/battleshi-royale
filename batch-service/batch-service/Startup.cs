using batch_service.consumer;
using batch_service.context;
using batch_service.repository;
using batch_service.repository.impl;


namespace batch_service;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddScoped<MongoContext>();
        
        services.AddSingleton<IMongoRepositoryFactory, MongoRepositoryFactory>();
        
        services.AddScoped<IMongoRepository,MongoRepository>();

        services.AddHostedService<BatchCreateService>();

        services.AddHostedService<BatchUpdateService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        
    }
}
