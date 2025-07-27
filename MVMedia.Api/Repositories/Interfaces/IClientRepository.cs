using MVMedia.API.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IClientRepository
{
    void AddClient(Client client);
    void UpdateClient(Client client);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetClientById(int id);

    Task<bool> SaveAllAsync();
}
