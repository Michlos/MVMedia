using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]

public class CompanyController : Controller
{
    
    private readonly ICompanyService _companyService;
    private readonly IAuthtenticate _authtenticateService;
    private readonly IUserService _userService;

    public CompanyController(ICompanyService companyService, IUserService userService, IAuthtenticate authtenticateService)
    {
        _companyService = companyService;
        _userService = userService;
        _authtenticateService = authtenticateService;
    }

    [HttpGet("GetAllCompanies")]
    public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
    {
        //if (!await _userService.IsAdmin(User.GetUserId()))
        //    return Unauthorized("You are not authorized to access this resource.");
        var companies = await _companyService.GetAllCompanies();
        return Ok(companies);
    }

    [HttpGet("GetCompany/{id}")]
    public async Task<ActionResult<Company>> GetCompanyById(int id)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource.");
        var company = await _companyService.GetCompanyById(id);
        if (company == null)
            return NotFound("Company not found.");
        return Ok(company);
    }

    [HttpPost("AddCompany")]
    public async Task<ActionResult> AddCompany(CompanyAddDTO company)
    {
        //EXIST COMPANY
        var existingCompany = await _companyService.GetAllCompanies();
        if (existingCompany.Any(c => c.Name.ToLower() == company.Name.ToLower()))
        {
            if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource.");
            
            var addedCompany = await _companyService.AddCompany(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = addedCompany.Id }, addedCompany);

        }
        else
        {
            var addedCompany = await _companyService.AddCompany(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = addedCompany.Id }, addedCompany);
        }



    }

    [HttpPut]
    public async Task<ActionResult<Company>> UpdateCompany([FromBody] CompanyUpdateDTO companyDTO)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource.");
        
        var updatedCompany = await _companyService.UpdateCompany(companyDTO);
        if (updatedCompany == null)
            return NotFound("Company not found.");
        return Ok(updatedCompany);
    }

}
