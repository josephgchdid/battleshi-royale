using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class GameInvitationDTO
{
    [JsonProperty("initialPlayerId")]
    public string initialPlayerId { get; set; }

    [JsonProperty("invitedPlayers")]
    public HashSet<string> invitedPlayers { get; set; }
}
