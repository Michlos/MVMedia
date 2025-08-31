using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace MVMedia.Adm.Services;

public class ClientService : IClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/Client/";
    private readonly JsonSerializerOptions _options;
    private ClientViewModel clientVM;
    private IEnumerable<ClientViewModel> clientListVM;

    public ClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ClientViewModel>> GetAllClients()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "GetAllClients");
        //response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            clientListVM = JsonSerializer.Deserialize<IEnumerable<ClientViewModel>>(apiResponse, _options);
        }
        else
            return null;

        return clientListVM;
    }
    public async Task<ClientViewModel> GetClientById(int id)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        using (var response = await client.GetAsync(apiEndpoint + $"GetClient/{id}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                clientVM = JsonSerializer
                           .Deserialize<ClientViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return clientVM;
    }
    public async Task<ClientViewModel> AddClient(ClientViewModel clientVM)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        StringContent content = new StringContent(JsonSerializer.Serialize(clientVM),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                clientVM = JsonSerializer
                           .Deserialize<ClientViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return clientVM;
    }
    public async Task<ClientViewModel> UpdateClient(ClientViewModel clientVM)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        ClientViewModel clienteUpdated = new ClientViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, clientVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                clienteUpdated = JsonSerializer
                                 .Deserialize<ClientViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return clienteUpdated;

    }
}
