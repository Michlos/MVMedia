using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.API.Models;

namespace MVMedia.Api.DTOs.Mapping;

public class EntitiesToDTOMappingProfile : Profile
{
    public EntitiesToDTOMappingProfile()
    {

        //CLIENTS MAPS
        //TO GET
        CreateMap<Client, ClientGetDTO>().ReverseMap();
        //TO UPDATE
        CreateMap<ClientUpdateDTO, Client>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ReverseMap();
        //TO ADD
        CreateMap<ClientAddDTO, Client>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
        //TO SUMMARY CLIENTE FOR MEDIAS LIST
        CreateMap<Client, ClientSummaryDTO>();


        //MEDIAS MAPS
        //TO GET
        CreateMap<Media, MediaGetDTO>().ReverseMap()
            .ForMember(dest => dest.Client, opt => opt.Ignore());
        //TO ADD
        CreateMap<MediaAddDTO, Media>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        //TO SUMMARY CLIENTE FOR MEDIAS LIST
        CreateMap<Media, MediaListItemDTO>();
        CreateMap<MediaUpdateDTO, Media>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

    }
}
