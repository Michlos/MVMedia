using MVMedia.Api.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IMediaFileRepository
{
    Task<MediaFile> AddMediaFile(MediaFile mediaFile);
    Task<MediaFile> UpdateMediaFile(MediaFile mediaFile);
    Task<MediaFile> GetMediaFileById(Guid id);
    Task<bool> DeleteMediaFile(Guid id);
    Task<ICollection<MediaFile>> GetAllMediaFiles();
}
