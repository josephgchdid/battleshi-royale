using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace game_service.Entities;

public class Game
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    [JsonProperty("_id")]
    public string Id { get; set; }

    [BsonElement("createdAt")]
    [JsonProperty("createdAt")]
    public DateTime CreateAt { get; set; }
    
    [BsonElement("allPlayersAreReady")]
    [JsonProperty("allPlayersAreReady")]
    public bool AllPlayersAreReady { get; set; }
    
    [BsonElement("players")]
    [JsonProperty("players")]
    public List<Player> Players { get; set; }
    
    [BsonElement("boards")]
    [JsonProperty("boards")]
    public List<Board> Boards { get; set; }
}
