using AutoMapper;
using game_service.Entities;
using game_service.Entities.DTO;

namespace game_service.Mapper;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Game, GameDTO>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<GameDTO, Game>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Board, BoardDTO>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<BoardDTO, Board>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Coordinates, CoordinatesDTO>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CoordinatesDTO, Coordinates>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}
