using MVMedia.Web.Models;
using MVMedia.Web.Service.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MVMedia.Web.Service
{
    public class MediaService : IMediaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string apiEndpoint = "/api/media/";
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
            var response = await client.GetAsync(apiEndpoint + "GetAllMedia");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                mediaListVM = JsonSerializer.Deserialize<IEnumerable<MediaViewModel>>(apiResponse, _options);
            }
            else
                return null;

            return mediaListVM;
        }


    }
}
