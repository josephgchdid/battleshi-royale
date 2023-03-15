using batch_service.context;
using MongoDB.Bson;

namespace batch_service.repository.impl;

public class MongoRepository : IMongoRepository
{
    private readonly MongoContext context;

    private readonly string collectionName;
    
    public MongoRepository(
        MongoContext _context,
        IConfiguration _configuration
        )
    {
        context = _context;

        collectionName = _configuration.GetValue<string>("DataBaseSettings:CollectionName");
    }

    
    public async Task CreateManyAsync(List<string> collection)
    {
        var bsonDocuments = new List<BsonDocument>();


        foreach (var jsonString in collection)
        {
            var bsonDocument = BsonDocument.Parse(jsonString);

            bsonDocuments.Add(bsonDocument);
        }

        var mongoCollection =  context.database.GetCollection<BsonDocument>(collectionName);
        
        await  mongoCollection.InsertManyAsync(bsonDocuments);
    }
}
