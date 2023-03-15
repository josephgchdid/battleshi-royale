namespace batch_service.repository.impl;

public class MongoRepositoryFactory : IMongoRepositoryFactory
{
    private readonly IServiceScope scope;
    
    public MongoRepositoryFactory(IServiceScopeFactory serviceScopeFactory)
    {
        scope = serviceScopeFactory.CreateScope();
    }

    public IMongoRepository Create()
    {
        return scope.ServiceProvider.GetService<IMongoRepository>();
    }
    
    public void Dispose()
    {
        scope.Dispose();
    }

}
