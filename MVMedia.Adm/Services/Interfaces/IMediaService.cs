using MVMedia.Adm.DTOs;
using MVMedia.Adm.Models;

namespace MVMedia.Adm.Services.Interfaces;

public interface IMediaService
{
    Task<IEnumerable<MediaViewModel>> GetAllMedia();
    Task<MediaViewModel> GetMediaById(int id);
    Task<ClientWithMediaDTO> GetMediaByClientId(int clientId);
    Task<MediaViewModel> AddMedia(MediaViewModel media);
    Task<MediaViewModel> UpdateMedia(MediaViewModel media);
    Task<bool> DeleteMedia(int id);
    //Task<IEnumerable<MediaViewModel>> GetMediaByClientId(int clientId);
}
