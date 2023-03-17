using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace game_service.Entities;

public class Coordinates
{
    [BsonElement("x")]
    [JsonProperty("x")]
    public int X { get; set; }
    
    [BsonElement("y")]
    [JsonProperty("y")]
    public int Y { get; set; }
    
    [BsonElement("isBombed")]
    [JsonProperty("isBombed")]
    public bool IsBombed { get; set; }
    
    [BsonElement("bombDidHit")]
    [JsonProperty("bombDidHit")]
    public bool BombDidHit { get; set; }
}
