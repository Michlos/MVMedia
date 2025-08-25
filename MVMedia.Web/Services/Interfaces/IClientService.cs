using MVMedia.Web.Models;

namespace MVMedia.Web.Services.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientViewModel>> GetAllClients();
    Task<ClientViewModel> GetClientById(int id);
    Task<ClientViewModel> AddClient(ClientViewModel client);
    Task<ClientViewModel> UpdateClient(ClientViewModel client);
}
