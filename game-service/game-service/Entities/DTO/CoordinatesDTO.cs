using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class CoordinatesDTO
{
    [JsonProperty("x")]
    public int X { get; set; }
    
    [JsonProperty("y")]
    public int Y { get; set; }
    
    [JsonProperty("isBombed")]
    public bool IsBombed { get; set; }
    
    [JsonProperty("bombDidHit")]
    public bool BombDidHit { get; set; }
}
