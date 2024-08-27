using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles;


public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Specify source and target

        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandReadDto,Command>();   
        CreateMap<Command,CommandReadDto>();
        CreateMap<PlatformPublishedDto,Platform>()
            .ForMember(dest=> dest.ExternalId, opt => opt.MapFrom(source => source.Id));

    }
}