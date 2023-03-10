

using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class GameDTO
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("createdId")]
    public DateTime CreatedAt { get; set; }
    
    [JsonProperty("allPlayersAreReady")]
    public bool AllPlayersAreReady { get; set; }
    
    [JsonProperty("players")]
    public HashSet<string> Players { get; set; }
    
    [JsonProperty("boards")]
    public List<BoardDTO> Boards { get; set; }
}
