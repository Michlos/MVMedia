﻿using MVMedia.Api.DTOs;
using MVMedia.API.Models;

namespace MVMedia.Api.Repositories.Interfaces;

public interface IClientRepository
{
    Task<Client> AddClient(Client client);
    Task<Client> UpdateClient(ClientUpdateDTO client);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetClientById(int id);

    Task<bool> SaveAllAsync();
}
