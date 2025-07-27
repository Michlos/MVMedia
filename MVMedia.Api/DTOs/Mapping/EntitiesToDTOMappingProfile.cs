using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.API.Models;

namespace MVMedia.Api.DTOs.Mapping;

public class EntitiesToDTOMappingProfile : Profile
{
    public EntitiesToDTOMappingProfile()
    {

        //CLIENTS MAPS
        CreateMap<Client, ClientGetDTO>().ReverseMap();
        CreateMap<ClientUpdateDTO, Client>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        CreateMap<ClientAddDTO, Client>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        //MEDIAS MAPS
        CreateMap<Media, MediaGetDTO>().ReverseMap();
    }
}
