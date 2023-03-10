using MongoDB.Driver;

namespace game_service.Repository;

using System.Linq.Expressions;
using Interface;
using MongoDB;

public class GameRepository : IGameRepository
{
    private MongoContext context { get; }

    public GameRepository(MongoContext _context)
    {
        context = _context;
    }
    
    public async Task CreateAsync<T>(T obj)
    {
        await GetCollection<T>().InsertOneAsync(obj);
    }

    public async Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

        var result = await GetCollection<T>().FindAsync(filter);

        return result.FirstOrDefault();

    }

    public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, int limit, int skip)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Where(funcExpression);

        return await GetCollection<T>()
            .Find(filter)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();

    }

    public IMongoCollection<T> GetCollection<T>()
    {
        return context.database.GetCollection<T>(typeof(T).Name);
    }
    
    public async Task<bool> ReplaceAsync<T>(T collection,Expression<Func<T, bool>> funcExpression)
    {
         
        var updatedResult = await GetCollection<T>()
            .ReplaceOneAsync(filter:funcExpression, replacement: collection);

        return updatedResult.IsAcknowledged
               && updatedResult.ModifiedCount > 0;

    }

}
