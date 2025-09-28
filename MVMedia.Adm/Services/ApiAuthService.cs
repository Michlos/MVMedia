using System.Text;
using System.Text.Json;

namespace MVMedia.Adm.Services;

public class ApiAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiAuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var client = _httpClientFactory.CreateClient("MVMediaAPI");
        var loginData = new { Username = username, Password = password };
        var response = await client.PostAsJsonAsync("api/User/login", loginData);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<UserToken>();
        return result?.Token;
    }
}

public class UserToken
{
    public string? Token { get; set; }
}
