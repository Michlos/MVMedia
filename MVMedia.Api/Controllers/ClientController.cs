using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;


    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("GetAllClients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {
        // This method should call the repository to get all clients
        // For now, returning an empty list
        var clients = await _clientService.GetAllClients();
        return Ok(clients);

    }
    [HttpGet("GetClient/{id}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        // This method should call the repository to get a client by ID
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
