using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class BoardDTO
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("playerId")]
    public string PlayerId { get; set; }
    
    [JsonProperty("squares")]
    public List<List<Coordinates>> Squares { get; set; }
    
    [JsonProperty("placementShips")]
    public List<ShipDTO> PlacementShips { get; set; }
}
