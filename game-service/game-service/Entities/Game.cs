using MongoDB.Bson.Serialization.Attributes;

namespace game_service.Entities;

public class Game
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreateAt { get; set; }
    
    [BsonElement("allPlayersAreReady")]
    public bool AllPlayersAreReady { get; set; }
    
    [BsonElement("players")]
    public HashSet<string> Players { get; set; }
    
    [BsonElement("boards")]
    public List<Board> Boards { get; set; }
}
