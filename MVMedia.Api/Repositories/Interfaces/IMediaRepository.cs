using MVMedia.Api.DTOs;
using MVMedia.API.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IMediaRepository
{
    void AddMedia(Media media);
    Task<Media> UpdateMedia(MediaUpdateDTO media);
    Task<IEnumerable<Media>> GetAllMedia();
    Task<Media> GetMediaById(int id);
    Task<IEnumerable<Media>> GetMediaByClientId(int clientId);

    Task<bool> SaveAllAsync();
}
