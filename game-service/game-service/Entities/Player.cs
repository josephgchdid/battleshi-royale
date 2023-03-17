using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace game_service.Entities;

public class Player
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    [JsonProperty("_id")]
    public string Id { get; set; }    
    
    [BsonElement("didAcceptInvite")]
    [JsonProperty("didAcceptInvite")]
    public bool DidAcceptInvite { get; set; }
}
