using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class GameInvitationDTO
{
    [JsonProperty("initialPlayerId")]
    public string initialPlayerId { get; set; }

    [JsonProperty("invitedPlayers")]
    public List<string> invitedPlayers { get; set; }
}
