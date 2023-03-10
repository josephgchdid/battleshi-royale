using MongoDB.Driver;
using System.Linq.Expressions;

namespace game_service.Repository.Interface;

public interface IGameRepository
{
    public Task CreateAsync<T>(T obj);

    public Task<T> FindAsync<T>(Expression<Func<T, bool>> funcExpression);

    public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>> funcExpression, int limit, int skip);

    public Task<bool> ReplaceAsync<T>(T collection,Expression<Func<T, bool>> funcExpression);
    
    public IMongoCollection<T> GetCollection<T>();
}
