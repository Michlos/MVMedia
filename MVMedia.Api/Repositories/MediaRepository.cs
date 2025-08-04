﻿using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.DTOs;
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
    public async Task<Media> UpdateMedia(MediaUpdateDTO mediaDTO)
    {
        var existingMedia = await _context.Medias.FirstOrDefaultAsync(m => m.Id == mediaDTO.Id);
        if (existingMedia == null)
            throw new ArgumentException($"Media with id {mediaDTO.Id} not found.");

        // Atualiza os campos conforme necessário
        if (mediaDTO.Title is not null && mediaDTO.Title != existingMedia.Title)
            existingMedia.Title = mediaDTO.Title;
        if (mediaDTO.Description is not null && mediaDTO.Description != existingMedia.Description)
            existingMedia.Description = mediaDTO.Description;
        if (mediaDTO.IsActive != existingMedia.IsActive)
            existingMedia.IsActive = mediaDTO.IsActive;
        if (mediaDTO.MediaUrl is not null && mediaDTO.MediaUrl != existingMedia.MediaUrl)
            existingMedia.MediaUrl = mediaDTO.MediaUrl;
        if (mediaDTO.Notes is not null && mediaDTO.Notes != existingMedia.Notes)
            existingMedia.Notes = mediaDTO.Notes;

        existingMedia.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existingMedia;
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
