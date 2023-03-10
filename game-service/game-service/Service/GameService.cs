using System.Net;
using AutoMapper;
using game_service.Entities;
using game_service.Entities.DTO;
using game_service.Repository.Interface;

namespace game_service.Service;

public class GameService
{
    private IGameRepository gameRepository;

    private IMapper mapper;
    
    private const int MAX_PLAYERS_IN_GAME = 4, ROW_SIZE = 7, COLUMN_SIZE = 7; 
    
    public static Dictionary<string, int> AvailableShipsAndSizes = new()
    {
        ["carrier"] = 5, 
        
        ["battleship"] = 4,
        
        ["cruiser"] = 3,
        
        ["submarine"] = 3, 
        
        ["destroyer"] = 2,
        
        ["raft"] = 1
    };
    
    public GameService(
        IGameRepository _gameRepository,
        IMapper _mapper
        )
    {
        gameRepository = _gameRepository;

        mapper = _mapper;
    }

    public async Task<Response> invitePlayersAndInitializeGame(GameInvitationDTO gameInvitation)
    {
        try
        {
            gameInvitation.invitedPlayers.Add(gameInvitation.initialPlayerId);

            if (gameInvitation.invitedPlayers.Count > MAX_PLAYERS_IN_GAME)
            {
                return new Response()
                {
                    Message = $"Max players allowed is {MAX_PLAYERS_IN_GAME} players",
                    
                    HttpStatus = (int)HttpStatusCode.NotAcceptable
                };
            }

            string gameId = Guid.NewGuid().ToString();

            var game = new Game()
            {
                Id = gameId,
                
                Boards = new(),
                
                AllPlayersAreReady = false, 
                
                Players = gameInvitation.invitedPlayers,
                
                CreateAt = DateTime.Now
            };

            foreach (var player in gameInvitation.invitedPlayers)
            {
                var board = new Board()
                {
                    Id = Guid.NewGuid().ToString(),
                    
                    PlayerId = player,
                    
                    HasPlacedAllShips = false,
                    
                    Ships = new(),
                    
                    Squares = new()
                };

                for (int row = 0; row < ROW_SIZE; row++)
                {
                    var currentSquareRow = new List<Coordinates>();
                    
                    for (int col = 0; col < COLUMN_SIZE; col++)
                    {
                        currentSquareRow.Add(
                            new Coordinates() { X = col, Y = row, IsBombed = false, BombDidHit = false }
                        );
                    }
                    
                    board.Squares.Add(currentSquareRow);
                }
                
                game.Boards.Add(board);
            }
            
            //send request to invite players 
            
            await gameRepository.CreateAsync(game);

            var gameDTO = mapper.Map<GameDTO>(game);

            return new Response()
            {
                Data = gameDTO,

                HttpStatus = (int)HttpStatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new Response()
            {
                Message = "Error while creating game, please try again in a bit",
                    
                HttpStatus = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
