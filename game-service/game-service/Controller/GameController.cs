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

    [HttpPost]
    [Route("invitation")]
    public async Task<IActionResult> AcceptOrRejectInvite([FromBody] GameInvitationAcceptanceDTO acceptanceDto)
    {
        var result =
            await gameService.AcceptOrRejectGameInvitation(acceptanceDto);

        return StatusCode(result.HttpStatus, result);
    }
    
    [HttpPost]
    [Route("shipPlacement")]
    public async Task<IActionResult> PlaceShipsOnBoard([FromBody] ShipPlacementDTO shipPlacement)
    {
        var result =
            await gameService.PlaceAllShipsOnBoard(shipPlacement);

        return StatusCode(result.HttpStatus, result);
    }
}
