using MVMedia.Adm.Models;

namespace MVMedia.Adm.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyViewModel>> GetAllCompanies();
    Task<CompanyViewModel> GetCompanyById(int id);
    Task<CompanyViewModel> AddCompany(CompanyViewModel company);
    Task<CompanyViewModel> UpdateCompany(CompanyViewModel company);
}
