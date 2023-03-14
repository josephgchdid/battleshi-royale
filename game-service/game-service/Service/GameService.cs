using System.Net;
using AutoMapper;
using game_service.Entities;
using game_service.Entities.DTO;
using game_service.Repository.Interface;
using MongoDB.Driver;

namespace game_service.Service;

public class GameService
{
    private IGameRepository gameRepository;

    private IMapper mapper;
    
    private const int MAX_PLAYERS_IN_GAME = 4, ROW_SIZE = 2, COLUMN_SIZE = 2; 
    
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


    public async Task<Response> AcceptOrRejectGameInvitation(GameInvitationAcceptanceDTO gameAcceptance)
    {
        try
        {

            var game = await gameRepository.FindAsync<Game>(
                game => game.Id == gameAcceptance.GameId);

            if (game == null)
            {
                return new Response()
                {
                    Message = "Game does not exist",
                    
                    HttpStatus = (int)HttpStatusCode.NotFound
                };
            }

            if (!game.Players.Contains(gameAcceptance.PlayerId))
            {
                return new Response()
                {
                    Message = "Could not find game",
                    
                    HttpStatus = (int)HttpStatusCode.NotFound
                };
            }

            if (gameAcceptance.PlayerAcceptedInvite)
            {
                return new Response()
                {
                    Message = "Ok",

                    HttpStatus = (int)HttpStatusCode.OK
                };
            }

            game.Players.RemoveWhere(playerId => playerId == gameAcceptance.PlayerId);

            game.Boards.RemoveAll(board => board.PlayerId == gameAcceptance.PlayerId);

            await gameRepository.ReplaceAsync(game, g => g.Id == gameAcceptance.GameId);

            return new Response()
            {
                Message = "Sorry that you did not want to join the fun loser",

                HttpStatus = (int)HttpStatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new Response()
            {
                Message = "Error while accepting game",
                    
                HttpStatus = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<Response> PlaceAllShipsOnBoard(ShipPlacementDTO placement)
    {
        try
        {
            var game = await gameRepository.FindAsync<Game>(
                game => game.Id == placement.GameId);

            if (game == null)
            {
                return new Response()
                {
                    Message = "Game does not exist",
                    
                    HttpStatus = (int)HttpStatusCode.NotFound
                };
            }

            var ships = GenerateShips(placement.Board.PlacementShips);

            if (ships.Count == 0)
            {
                return new Response()
                {
                    Message = "Invalid ship placement",
                    
                    HttpStatus = (int)HttpStatusCode.Forbidden
                };
            }

            var result = game.Boards.First(boardToFind => boardToFind.PlayerId == placement.Board.PlayerId);

            result.HasPlacedAllShips = true;
            
            result.Ships.AddRange(ships);
            
            await gameRepository.ReplaceAsync(game, g => g.Id == placement.GameId);

            return new Response()
            {
                Message = "Placed all ships on board",
                
                HttpStatus = (int)HttpStatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new Response()
            {
                Message = "Error while placing ships on board",
                    
                HttpStatus = (int)HttpStatusCode.InternalServerError
            };
        }
    }


    private List<Ship> GenerateShips(List<ShipDTO> PlacementShips)
    {
        List<Ship> ships = new();
        
        foreach(var ship in PlacementShips)
        {
            
            if (string.IsNullOrEmpty(ship.ShipType))
            {
                return new List<Ship>();
            }

            ship.ShipType = ship.ShipType.ToLower();

            if (!AvailableShipsAndSizes.ContainsKey(ship.ShipType))
            {
                return new List<Ship>();
            }

            bool isHorizontal = ship.BowCoordinate.X == ship.SternCoordinate.X;

            bool isVertical = ship.BowCoordinate.Y == ship.SternCoordinate.Y;

            if (!isHorizontal && !isVertical)
            {
                return new List<Ship>();
            }
            
            var distanceBetweenPoints = CalculateDistanceBetweenPoints(
                ship.BowCoordinate.X,
                ship.SternCoordinate.X,
                ship.BowCoordinate.Y,
                ship.SternCoordinate.Y
            );

            int shipSize = AvailableShipsAndSizes[ship.ShipType];

            if (distanceBetweenPoints + 1 > shipSize)
            {
                return new List<Ship>();
            }
            
            if (
                CheckOutOfBounds(ship.BowCoordinate.X, ship.BowCoordinate.Y)
                ||
                CheckOutOfBounds(ship.SternCoordinate.X, ship.SternCoordinate.Y)
            )
            {
                return new List<Ship>();
            }

            var anyShipIsOverLapping = PlacementShips.Any(
                iterator =>
                  (iterator.BowCoordinate.X == ship.BowCoordinate.X 
                    &&
                    iterator.BowCoordinate.Y == ship.BowCoordinate.Y
                     )
                   &&
                  (
                      iterator.SternCoordinate.X == ship.SternCoordinate.X
                      &&
                      iterator.SternCoordinate.Y == ship.SternCoordinate.Y

                  )
                  &&
                  iterator.ShipType != ship.ShipType
            );

            if (anyShipIsOverLapping)
            {
                return new List<Ship>();
            }

            var newShip = new Ship()
            {
                Id = Guid.NewGuid().ToString(),
                
                ShipSize = AvailableShipsAndSizes[ship.ShipType],

                ShipType = ship.ShipType,

                IsDestroyed = false,

                Coordinates = FillCoordinatesBetweenBowAndStern(
                    isHorizontal,
                    ship.BowCoordinate.X,
                    ship.BowCoordinate.Y,
                    AvailableShipsAndSizes[ship.ShipType]
                )
            };
            
            ships.Add(newShip);
        }

        return ships;
    }


    private int CalculateDistanceBetweenPoints(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
    }

    private bool CheckOutOfBounds(int x, int y)
    {
        return (x < 0 || x >= ROW_SIZE) || (y < 0 || y >= COLUMN_SIZE);
    }


    private List<Coordinates> FillCoordinatesBetweenBowAndStern(
        bool isHorizontal, int x, int y, int size)
    {

        var coordinates = new List<Coordinates>();

        for (int i = 0; i < size ; i++)
        {
            var currentCoordinates = new Coordinates()
            {
                X = isHorizontal ? x  : x + i,

                Y = isHorizontal ? y  + i: y,

                BombDidHit = false,

                IsBombed = false
            };
            
            coordinates.Add(currentCoordinates);
        }

        return coordinates;
    }
}
