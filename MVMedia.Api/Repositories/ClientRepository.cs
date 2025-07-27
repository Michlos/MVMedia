using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

using System.Threading.Tasks;

namespace MVMedia.Api.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApiDbContext _context;

    public ClientRepository(ApiDbContext context)
    {
        _context = context;
    }

    public void AddClient(Client client)
    {
        client.CreatedAt = DateTime.UtcNow; // Set the creation timestamp
        _context.Clients.Add(client);
    }
    public async Task<Client> UpdateClient(ClientUpdateDTO clientDTO)
    {
        Client existinggClient = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clientDTO.Id);
        if (existinggClient == null)
            throw new ArgumentException($"Client with id {clientDTO.Id} not found.");
        
        var entry = _context.Attach(existinggClient);
        
        if(clientDTO.Name is not null && clientDTO.Name != existinggClient.Name)
            entry.Property(c => c.Name).IsModified = true;
        if(clientDTO.CPF is not null && clientDTO.CPF != existinggClient.CPF)
            entry.Property(c => c.CPF).IsModified = true;
        if(clientDTO.CNPJ is not null && clientDTO.CNPJ != existinggClient.CNPJ)
            entry.Property(c => c.CNPJ).IsModified = true;
        if(clientDTO.Email is not null && clientDTO.Email != existinggClient.Email)
            entry.Property(c => c.Email).IsModified = true;
        if(clientDTO.Phone is not null && clientDTO.Phone != existinggClient.Phone)
            entry.Property(c => c.Phone).IsModified = true;
        if(clientDTO.Address is not null && clientDTO.Address != existinggClient.Address)
            entry.Property(c => c.Address).IsModified = true;
        if(clientDTO.City is not null && clientDTO.City != existinggClient.City)
            entry.Property(c => c.City).IsModified = true;
        if(clientDTO.State is not null && clientDTO.State != existinggClient.State)
            entry.Property(c => c.State).IsModified = true;
        if(clientDTO.ZipCode is not null && clientDTO.ZipCode != existinggClient.ZipCode)
            entry.Property(c => c.ZipCode).IsModified = true;
        if(clientDTO.Country is not null && clientDTO.Country != existinggClient.Country)
            entry.Property(c => c.Country).IsModified = true;
        if(clientDTO.IsActive != existinggClient.IsActive)
            entry.Property(c => c.IsActive).IsModified = true;

        existinggClient.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existinggClient;
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client> GetClientById(int id)
    {
        return await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();

    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }


}
