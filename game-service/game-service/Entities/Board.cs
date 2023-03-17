using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace game_service.Entities;

public class Board
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    [JsonProperty("_id")]
    public string Id { get; set; }
    
    [BsonElement("playerId")]
    [JsonProperty("playerId")]
    public string PlayerId { get; set; }
    
    [BsonElement("hasPlacedAllShips")]
    [JsonProperty("hasPlacedAllShips")]
    public bool HasPlacedAllShips { get; set; }
    
    [BsonIgnore]
    [JsonIgnore]
    public List<Ship> Ships { get; set; }
    
    [BsonElement("squares")]
    [JsonProperty("squares")]
    public List<List<Coordinates>> Squares { get; set; }
}
