using MongoDB.Bson.Serialization.Attributes;

namespace game_service.Entities;

public class Player
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }    
    
    [BsonElement("didAcceptInvite")]
    public bool DidAcceptInvite { get; set; }
}
