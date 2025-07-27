using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    public readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

   [HttpGet("GetAllClients")]
   public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {
        // This method should call the repository to get all clients
        // For now, returning an empty list
        return Ok(await _clientRepository.GetAllClients());

    }
    [HttpGet("GetClient/{id}")]
    public async Task<ActionResult<Client>> GetClientById(int id)
    {
        // This method should call the repository to get a client by ID
        var client = await _clientRepository.GetClientById(id);
        if (client == null)
        {
            return NotFound($"Client with id {id} not found!");
        }
        return Ok(client);
    }


    [HttpPost]
    public async Task<ActionResult<Client>> AddClient(Client client)
    {
        // This method should call the repository to add a new client
        // For now, returning the client as is
        _clientRepository.AddClient(client);
        if (await _clientRepository.SaveAllAsync())
        {
            return Ok(client);
        }
        return BadRequest("Failed to add client");
    }
}
