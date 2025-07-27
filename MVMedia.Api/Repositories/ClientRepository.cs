using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Context;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

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
        _context.Clients.Add(client);
    }
    public void UpdateClient(Client client)
    {
        client.UpdatedAt = DateTime.UtcNow;
        _context.Entry(client).State = EntityState.Modified;
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
