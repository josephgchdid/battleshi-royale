namespace batch_service.repository;

public interface IMongoRepository
{
    public Task CreateManyAsync(List<string> collection);
}
