using MVMedia.API.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IMediaRepository
{
    void AddMedia(Media media);
    void UpdateMedia(Media media);
    Task<IEnumerable<Media>> GetAllMedia();
    Task<Media> GetMediaById(int id);
    Task<IEnumerable<Media>> GetMediaByClientId(int clientId);

    Task<bool> SaveAllAsync();
}
