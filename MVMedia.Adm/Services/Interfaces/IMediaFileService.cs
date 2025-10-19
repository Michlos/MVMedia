﻿using MVMedia.Adm.Models;

namespace MVMedia.Adm.Services.Interfaces;

public interface IMediaFileService
{
    Task<MediaFileViewModel> AddMediaFile(MediaFileViewModel mediaFile);
    Task<MediaFileViewModel> UpdateMediaFile(MediaFileViewModel mediaFile, string oldFileName);
    Task<MediaFileViewModel> GetMediaFileById(Guid id);
    Task<bool> DeleteMediaFile(Guid id);
    Task<IEnumerable<MediaFileViewModel>> GetAllMediaFiles();

    Task<IEnumerable<string>> GetAllMediaFileURI();
}
