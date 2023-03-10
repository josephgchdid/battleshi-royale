using Newtonsoft.Json;

namespace game_service.Entities.DTO;

public class Response
{
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    [JsonProperty("status")]
    public int HttpStatus { get; set; }

}
