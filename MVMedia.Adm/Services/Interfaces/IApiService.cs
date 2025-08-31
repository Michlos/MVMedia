using Microsoft.Extensions.Primitives;

namespace MVMedia.Adm.Services.Interfaces;

public interface IApiService
{
    Task<string> AutenticateAsync(string login, string password);
}
