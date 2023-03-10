using MongoDB.Bson.Serialization.Attributes;

namespace game_service.Entities;

public class Board
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }
    
    [BsonElement("playerId")]
    public string PlayerId { get; set; }
    
    [BsonElement("hasPlacedAllShips")]
    public bool HasPlacedAllShips { get; set; }
    
    [BsonElement("ships")]
    public List<Ship> Ships { get; set; }
    
    [BsonElement("squares")]
    public List<List<Coordinates>> Squares { get; set; }
}
