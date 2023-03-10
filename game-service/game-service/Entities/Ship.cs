using MongoDB.Bson.Serialization.Attributes;

namespace game_service.Entities;

public class Ship
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }

    [BsonElement("shipSize")]
    public int ShipSize { get; set; }
    
    [BsonElement("shipType")]
    public string ShipType { get; set; }
    
    [BsonElement("isDestroyed")]
    public bool IsDestroyed { get; set; }
    
    [BsonElement("coordinates")]
    public List<Coordinates> Coordinates { get; set; }
}
