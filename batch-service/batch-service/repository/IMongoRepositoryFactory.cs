namespace batch_service.repository;


public interface IMongoRepositoryFactory : IDisposable
{
    IMongoRepository Create();
}

