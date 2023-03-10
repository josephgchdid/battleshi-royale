using game_service.Entities.DTO;
using game_service.Service;
using Microsoft.AspNetCore.Mvc;

namespace game_service.Controller;

[ApiController]
[Route("api/gameService/game")]
public class GameController : ControllerBase
{
    private GameService gameService { get; set; }

    public GameController(GameService _gameService)
    {
        gameService = _gameService;
    }
    
    [HttpPost]
    [Route("createAndInvite")]
    public async Task<IActionResult> CreateGameAndInvitePlayers([FromBody] GameInvitationDTO gameInvitation)
    {
        var result =
            await gameService.invitePlayersAndInitializeGame(gameInvitation);

        return StatusCode(result.HttpStatus, result);
    }
}
