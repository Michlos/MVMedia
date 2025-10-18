using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Services;

public class MediaFileService : IMediaFileService
{
    private readonly IMediaFileRepository _mediaFileRepository;

    public MediaFileService(IMediaFileRepository mediaFileRepository)
    {
        _mediaFileRepository = mediaFileRepository;
    }

    public async Task<MediaFile> AddMediaFile(MediaFile mediaFile)
    {
        var mediaFileAdded = await _mediaFileRepository.AddMediaFile(mediaFile);
        return mediaFileAdded;
    }

    public async Task<bool> DeleteMediaFile(Guid id)
    {
        return await _mediaFileRepository.DeleteMediaFile(id);
    }

    public async Task<ICollection<MediaFile>> GetAllMediaFiles()
    {
        return await _mediaFileRepository.GetAllMediaFiles();
    }

    public async Task<MediaFile> GetMediaFileById(Guid id)
    {
        return await _mediaFileRepository.GetMediaFileById(id);
    }

    public async Task<MediaFile> UpdateMediaFile(MediaFile mediaFile)
    {
        var updatedMediaFile = await _mediaFileRepository.UpdateMediaFile(mediaFile);
        return updatedMediaFile;
    }
}
