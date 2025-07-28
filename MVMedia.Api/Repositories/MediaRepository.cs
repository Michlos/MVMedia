using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly ApiDbContext _context;

    public MediaRepository(ApiDbContext context)
    {
        _context = context;
    }

    public void AddMedia(Media media)
    {
        //media.CreatedAt = DateTime.UtcNow; // Set the creation timestamp
        _context.Medias.Add(media);
    }
    public void UpdateMedia(Media media)
    {
        media.UpdatedAt = DateTime.UtcNow; // Update the timestamp
        _context.Medias.Entry(media).State = EntityState.Modified;
    }

    public async Task<IEnumerable<Media>> GetAllMedia()
    {
        return await _context.Medias.ToListAsync();
    }

    public async Task<IEnumerable<Media>> GetMediaByClientId(int clientId)
    {
        return await _context.Medias.Where(m => m.ClientId == clientId).ToListAsync();
    }

    public async Task<Media> GetMediaById(int id)
    {
        return await _context.Medias.Where(m => m.Id == id).FirstOrDefaultAsync();
    }


    public async Task<bool> SaveAllAsync()
    {
        //SaveChanges return 0 if error changes in database
        return await _context.SaveChangesAsync() > 0;
    }
}
