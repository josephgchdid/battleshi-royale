using MongoDB.Bson.Serialization.Attributes;

namespace game_service.Entities;

public class Coordinates
{
    [BsonElement("x")]
    public int X { get; set; }
    
    [BsonElement("y")]
    public int Y { get; set; }
    
    [BsonElement("isBombed")]
    public bool IsBombed { get; set; }
    
    [BsonElement("bombDidHit")]
    public bool BombDidHit { get; set; }
}
