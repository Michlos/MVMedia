using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.API.Models;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;


    public ClientController(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

   [HttpGet("GetAllClients")]
   public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {
        // This method should call the repository to get all clients
        // For now, returning an empty list
        var clients = await _clientRepository.GetAllClients();
        var clientsDTO = _mapper.Map<IEnumerable<ClientGetDTO>>(clients);
        return Ok(clientsDTO);

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

        var clientDTO = _mapper.Map<ClientGetDTO>(client);
        return Ok(clientDTO);
    }


    [HttpPost]
    public async Task<ActionResult<Client>> AddClient(ClientAddDTO clientDTO)
    {
        // This method should call the repository to add a new client
        // For now, returning the client as is

        var client = _mapper.Map<Client>(clientDTO);

        _clientRepository.AddClient(client);
        if (await _clientRepository.SaveAllAsync())
        {
            return Ok(client);
        }
        return BadRequest("Failed to add client");
    }

    [HttpPut]
    public async Task<ActionResult<Client>> UpdateClient([FromBody] ClientUpdateDTO clientDTO)
    {
        if(clientDTO.Id == 0)
            return BadRequest("Is not possible to update a client without an ID");
        var existingClient = await _clientRepository.GetClientById(clientDTO.Id);
        if (existingClient == null)
            return NotFound($"Client with id {clientDTO.Id} not found!");
        if (clientDTO == null)
            return BadRequest("Invalid client data");
        await _clientRepository.UpdateClient(clientDTO);
        return Ok(clientDTO);

    }
}
