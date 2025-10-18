using MVMedia.Api.Context;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MVMedia.Api.Videos;

namespace MVMedia.Api.Repositories;

public class MediaFileRepository : IMediaFileRepository
{
    private readonly ApiDbContext _context;
    private readonly VideoSettings _videoSettings;

    public MediaFileRepository(ApiDbContext context, VideoSettings mediaSettings)
    {
        _context = context;
        _videoSettings = mediaSettings;
    }

    public async Task<MediaFile> AddMediaFile(MediaFile mediaFile)
    {
        mediaFile.UploadedAt = DateTime.UtcNow;
        await _context.MediaFiles.AddAsync(mediaFile);
        await _context.SaveChangesAsync();
        return mediaFile;
    }

    public async Task<bool> DeleteMediaFile(Guid id)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile == null)
            return false;

        //var filePath = Path.Combine(Directory.GetCurrentDirectory(), mediaFile.FilePath);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), _videoSettings.VideoPath, mediaFile.FileName);


        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        _context.MediaFiles.Remove(mediaFile);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ICollection<MediaFile>> GetAllMediaFiles()
    {
        return await _context.MediaFiles.ToListAsync();
    }

    public async Task<MediaFile> GetMediaFileById(Guid id)
    {
        return await _context.MediaFiles.FindAsync(id).AsTask();
    }

    public async Task<MediaFile> UpdateMediaFile(MediaFile mediaFile)
    {
        var existingMediaFile = await _context.MediaFiles.FindAsync(mediaFile.Id);
        if (existingMediaFile == null)
            throw new ArgumentException($"MediaFile with Id {mediaFile.Id} not found.");

        //ATUALIZA OS CAMPOS NECESSÁRIOS
        if (mediaFile.FileName == null && mediaFile.FileName != existingMediaFile.FileName)
            existingMediaFile.FileName = mediaFile.FileName;
        if (mediaFile.FileSize == 0 && mediaFile.FileSize != existingMediaFile.FileSize)
            existingMediaFile.FileSize = mediaFile.FileSize;
        if (mediaFile.IsPublic != existingMediaFile.IsPublic)
            existingMediaFile.IsPublic = mediaFile.IsPublic;
        if (mediaFile.IsActive != existingMediaFile.IsActive)
            existingMediaFile.IsActive = mediaFile.IsActive;

        existingMediaFile.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existingMediaFile;
    }
}
