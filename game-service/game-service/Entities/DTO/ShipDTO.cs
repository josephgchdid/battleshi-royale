using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class ShipDTO
{
    [JsonProperty("shipType")]
    public string ShipType { get; set; }
    
    [JsonProperty("shipSize")]
    public int ShipSize { get; set; }
    
    [JsonProperty("bowCoordinates")]
    public CoordinatesDTO BowCoordinate { get; set; }
    
    [JsonProperty("sternCoordinates")]
    public CoordinatesDTO SternCoordinate { get; set; }
}
