using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVMedia.Web.Models;

namespace MVMedia.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public List<MediaFile> MediaFiles { get; set; } = new();
    public string? ApiError { get; set; }
    public string ApiBaseUrl { get; private set; } = string.Empty;

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        try
        {
            ApiBaseUrl = _configuration["ApiBaseUrl"]?.TrimEnd('/') ?? "";
            var client = _httpClientFactory.CreateClient("Default");
            var apiUrl = $"{ApiBaseUrl}/api/MediaFile/ListActiveMediaFiles";
            var result = await client.GetFromJsonAsync<List<MediaFile>>(apiUrl);
            if (result != null)
                MediaFiles = result;
        }
        catch (Exception ex)
        {
            ApiError = ex.Message;
        }
    }
}
