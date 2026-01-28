using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace MVMedia.Adm.Services;

public class MediaListService : IMediaListService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/MediaList/";
    private readonly JsonSerializerOptions _options;
    private MediaListViewModel? mediaListVM;

    public MediaListService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<MediaListViewModel> AddMediaList(MediaListViewModel mediaList)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var formData = new List<KeyValuePair<string, string>>
        {
            new("Name", mediaList.Name),
            new("CreateDate", mediaList.CreateDate.ToString("o")),
            new("IsActive", mediaList.IsActive.ToString())
        };
        var content = new FormUrlEncodedContent(formData);

        var response = await client.PostAsync(apiEndpoint + "AddMediaList", content);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            mediaListVM = JsonSerializer.Deserialize<MediaListViewModel>(apiResponse, _options);
            return mediaListVM;
        }
        else
        {
            return null;
        }
    }

    public async Task<MediaListViewModel> GetMediaList()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "GetActiveMediaList");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            mediaListVM = JsonSerializer.Deserialize<MediaListViewModel>(apiResponse, _options);
            return mediaListVM;
        }
        else
        {
            return null;
        }
    }

    public Task AddMediaList(ActionResult<IEnumerable<MediaFileViewModel>> mediaList)
    {
        throw new NotImplementedException();
    }

    public async Task<MediaListViewModel> AddMediaList(IEnumerable<MediaListViewModel> mediaList)
    {
        // Envia apenas o primeiro item da lista, pois o endpoint espera um único objeto
        var first = mediaList?.FirstOrDefault();
        if (first == null)
            return null;

        return await AddMediaList(first);
    }
}
