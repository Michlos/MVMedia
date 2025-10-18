﻿using MVMedia.Api.Models;

namespace MVMedia.Api.Services.Interfaces;

public interface IMediaFileService
{
    Task<MediaFile> AddMediaFile(MediaFile mediaFile);
    Task<MediaFile> UpdateMediaFile(MediaFile mediaFile);
    Task<MediaFile> GetMediaFileById(Guid id);
    Task<bool> DeleteMediaFile(Guid id);
    Task<ICollection<MediaFile>> GetAllMediaFiles();
}
