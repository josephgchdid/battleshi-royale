using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class GameInvitationAcceptanceDTO
{
    [JsonProperty("gameId")]
    public string GameId { get; set; }
    
    [JsonProperty("playerId")]
    public string PlayerId { get; set; }
    
    [JsonProperty("playerAcceptedInvite")]
    public bool PlayerAcceptedInvite { get; set; }
}
