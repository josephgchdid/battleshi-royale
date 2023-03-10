using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class ShipPlacementDTO
{
    [JsonProperty("gameId")]
    public string GameId { get; set; }
    
    [JsonProperty("board")]
    public BoardDTO Board { get; set; }
}
