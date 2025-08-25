using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Services.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly IUserService _userService;


    public ClientController(IClientService clientService, IUserService userService)
    {
        _clientService = clientService;
        _userService = userService;

    }

    [HttpGet("GetAllClients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {


        //AUTENTICAÇÃO DESABILITADA PARA TESTES
        //if (!await _userService.IsAdmin(User.GetUserId()))
        //    return Unauthorized("You are not authorized to access this resource");

        var clients = await _clientService.GetAllClients();
        return Ok(clients);

    }
    [HttpGet("GetClient/{id}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource");

        var clientUpdated = await _clientService.GetClientById(id);
        if (clientUpdated == null)
        {
            return NotFound($"Client with id {id} not found!");
        }
        return Ok(clientUpdated);
    }


    [HttpPost]
    public async Task<ActionResult<Client>> AddClient(ClientAddDTO clientDTO)
    {
        //AUTENTICAÇÃO DESABILITADA PARA TESTES
        //if (!await _userService.IsAdmin(User.GetUserId()))
        //    return Unauthorized("You are not authorized to access this resource");


        var clientAdded = await _clientService.AddClient(clientDTO);
        if (clientAdded == null)
        {
            return BadRequest("Failed to add client");
        }
        return Ok(clientAdded);
    }

    [HttpPut]
    public async Task<ActionResult<Client>> UpdateClient([FromBody] ClientUpdateDTO clientDTO)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource");

        if (clientDTO.Id == 0)
            return BadRequest("Is not possible to update a client without an ID");

        var existingClient = await _clientService.GetClientById(clientDTO.Id);
        if (existingClient == null)
            return NotFound($"Client with id {clientDTO.Id} not found!");
        
        if (clientDTO == null)
            return BadRequest("Invalid client data");
        
        await _clientService.UpdateClient(clientDTO);
        return Ok(clientDTO);

    }



}
