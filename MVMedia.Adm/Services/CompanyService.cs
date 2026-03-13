using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace MVMedia.Adm.Services;


public class CompanyService : ICompanyService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/Company/";
    private readonly JsonSerializerOptions _options;
    private CompanyViewModel companyVM;
    private IEnumerable<CompanyViewModel> companyListVM;

    public CompanyService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CompanyViewModel> AddCompany(CompanyViewModel companyVM)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        StringContent content = new StringContent(JsonSerializer.Serialize(companyVM),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                companyVM = JsonSerializer
                    .Deserialize<CompanyViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return companyVM;
    }

    public async Task<IEnumerable<CompanyViewModel>> GetAllCompanies()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "GetAllCompanies");

        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            companyListVM = JsonSerializer
                .Deserialize<IEnumerable<CompanyViewModel>>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return companyListVM;
    }


    public async Task<CompanyViewModel> GetCompanyById(int id)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        using (var response = await client.GetAsync(apiEndpoint + $"GetCompany/{id}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                companyVM = JsonSerializer
                    .Deserialize<CompanyViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

        }
        return companyVM;
    }

    public async Task<CompanyViewModel> UpdateCompany(CompanyViewModel companyVM)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        
        using (var response = await client.PutAsJsonAsync(apiEndpoint, companyVM))
        {
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                companyVM = JsonSerializer.Deserialize<CompanyViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }

        return companyVM;
    }

}
