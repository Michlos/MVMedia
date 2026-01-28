using MVMedia.Adm.DTOs;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace MVMedia.Adm.Services;

public class MediaService : IMediaService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/MediaFile/";
    private readonly JsonSerializerOptions _options;
    private MediaViewModel? mediaVM;
    private IEnumerable<MediaViewModel>? mediaListVM;

    public MediaService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<MediaViewModel>> GetAllMedia()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "ListActiveMediaFiles");
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            mediaListVM = JsonSerializer.Deserialize<IEnumerable<MediaViewModel>>(apiResponse, _options);
        }
        else
            return null;
        return mediaListVM;
    }
    public async Task<MediaViewModel> GetMediaById(int id)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + $"GetMedia/{id}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            mediaVM = JsonSerializer.Deserialize<MediaViewModel>(apiResponse, _options);
        }
        else
            return null;
        return mediaVM;
    }

    public async Task<MediaViewModel> AddMedia(MediaViewModel media)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        StringContent content = new StringContent(JsonSerializer.Serialize(media),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                mediaVM = JsonSerializer.Deserialize<MediaViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return mediaVM;
    }

    public async Task<ClientWithMediaDTO> GetMediaByClientId(int clientId)
    {
        var clientApi = _clientFactory.CreateClient("MVMediaAPI");
        var response = await clientApi.GetAsync(apiEndpoint + $"GetMediasByCliente/{clientId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(clientApi + " - " + response.ReasonPhrase);
        }
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        var rawJson = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(rawJson))
        {
            // Retorna uma instância vazia de ClientWithMediaDTO
            return new ClientWithMediaDTO
            {
                Client = null,
                Medias = new List<MediaListItemDTO>()
            };
        }
        else
        {
            return JsonSerializer.Deserialize<ClientWithMediaDTO>(rawJson, _options);
        }
    }


    public async Task<MediaViewModel> UpdateMedia(MediaViewModel mediaVM)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        MediaViewModel mediaUpdated = new MediaViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, mediaVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                mediaUpdated = JsonSerializer.Deserialize<MediaViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

        }
        return mediaUpdated;
    }

    public async Task<bool> DeleteMedia(int id)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }
}
