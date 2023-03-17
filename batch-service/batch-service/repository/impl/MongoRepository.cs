using batch_service.context;
using batch_service.entity;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

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

    public async Task UpdateManyAsync(List<string> collection)
    {
        

        var bsonDocuments = new List<WriteModel<BsonDocument>>();

        foreach (var jsonString in collection)
        {
            var obj = JsonConvert.DeserializeObject<UpdateModel>(jsonString);
            
            var filter = Builders<BsonDocument>.Filter.Eq("_id", obj.Id);
            
            // Define an update to apply to the selected document.
            var update = Builders<BsonDocument>.Update.Set(obj.UpdateField, BsonValue.Create(obj.NewValue));

            // Add the update request to the list of requests.
            bsonDocuments.Add(new UpdateOneModel<BsonDocument>(filter, update));
            
        }
        
        var mongoCollection =  context.database.GetCollection<BsonDocument>(collectionName);

        var res = await mongoCollection.BulkWriteAsync(bsonDocuments);
        
        Console.WriteLine($"matched {res.MatchedCount} and modified {res.ModifiedCount}");
    }
}
