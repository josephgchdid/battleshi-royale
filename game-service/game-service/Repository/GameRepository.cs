using MongoDB.Driver;

namespace game_service.Repository;

using System.Linq.Expressions;
using Interface;

public class GameRepository : IGameRepository
{
    private MongoContext context { get; }

    public GameRepository(MongoContext _context)
    {
        context = _context;
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


}
