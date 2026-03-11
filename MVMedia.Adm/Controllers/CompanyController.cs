using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;

namespace MVMedia.Adm.Controllers;

public class CompanyController : Controller
{

    private readonly ICompanyService _companyService;
    //private readonly IClientService _clientService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
        //_clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyViewModel>>> Index()
    {
        var result = await _companyService.GetAllCompanies();
        if (result == null)
            return View("Error", new string[] { "Something went wrong while fetching the companies." });

        return View(result);
    }

    [HttpGet]
    public async Task<ActionResult> AddCompany()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddCompany(CompanyViewModel companyVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _companyService.AddCompany(companyVM);
            if (result != null)
                return RedirectToAction(nameof(Index));
            else
                return View("Error", new string[] { "Something went wrong while adding the company." });
        }
        return View(companyVM);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateCompany(int id)
    {
        var result = await _companyService.GetCompanyById(id);
        if (result is null)
            return View("Error", new string[] { "Something went wrong while fetching the company." });
        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateCompany(CompanyViewModel companyVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _companyService.UpdateCompany(companyVM);
            if (result != null)
                return RedirectToAction(nameof(Index));
            else
                return View("Error", new string[] { "Something went wrong while updating the company." });
        }
        return View(companyVM);
    }

    [HttpGet]
    public async Task<ActionResult> CompanyDetail(int id)
    {
        var company = await _companyService.GetCompanyById(id);
        if (company == null)
            return View("Error", new string[] { "Company not found." });
        //var clients = await _clientService.GetAllClients();
        //if (clients == null)
        //    return View("Error", new string[] { "Something went wrong while fetching the clients." });
        //var companyClients = clients.Where(c => c.CompanyId == id).ToList();
        //ViewBag.CompanyName = company.Name;
        return View(company);
    }

    ////LIST CLIENTS FOR A COMPANY
    //[HttpGet]
    //public async Task<ActionResult> ListClients(int companyId)
    //{
    //    var company = await _companyService.GetCompanyById(companyId);
    //    if (company == null)
    //        return View("Error", new string[] { "Company not found." });
    //    var clients = await _clientService.GetAllClients();
    //    if (clients == null)
    //        return View("Error", new string[] { "Something went wrong while fetching the clients." });
    //    var companyClients = clients.Where(c => c.CompanyId == companyId).ToList();
    //    ViewBag.CompanyName = company.Name;
    //    return View(companyClients);
    //}

}
