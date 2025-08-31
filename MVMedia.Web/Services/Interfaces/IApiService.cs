using Microsoft.Extensions.Primitives;

namespace MVMedia.Web.Services.Interfaces;

public interface IApiService
{
    Task<string> AutenticateAsync(string login, string password);
}
