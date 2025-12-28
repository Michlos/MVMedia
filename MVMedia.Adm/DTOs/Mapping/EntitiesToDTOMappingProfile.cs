using AutoMapper;
using MVMedia.Adm.Models;

namespace MVMedia.Adm.DTOs.Mapping;

public class EntitiesToDTOMappingProfile : Profile
{
    public EntitiesToDTOMappingProfile()
    {
        CreateMap<ClientViewModel, ClientSummaryDTO>();
        CreateMap<MediaViewModel, MediaListItemDTO>();
        CreateMap<MediaFileViewModel, MediaFileDTO>();
    }
}
