using System.Text;
using System.Text.Json;

namespace MVMedia.Web.Services;

public class ApiAuthService
{
    private readonly HttpClient _httpClient;
    public ApiAuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var loginMOdel = new
        {
            Username = username,
            Password = password
        };

        var content = new StringContent(JsonSerializer.Serialize(loginMOdel), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsJsonAsync("api/user/login", content);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var tokenObj = JsonSerializer.Deserialize<UserToken>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return tokenObj?.Token;
    }
}

public class UserToken
{
    public string Token { get; set; }
}
