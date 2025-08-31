using MVMedia.Adm.Models;

namespace MVMedia.Adm.Services.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientViewModel>> GetAllClients();
    Task<ClientViewModel> GetClientById(int id);
    Task<ClientViewModel> AddClient(ClientViewModel client);
    Task<ClientViewModel> UpdateClient(ClientViewModel client);
}
