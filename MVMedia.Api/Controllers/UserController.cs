﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : Controller
{
    
    private readonly IAuthtenticate _authenticateService;
    private readonly IUserService _userService;

    public UserController(IAuthtenticate authtenticate, IUserService userService)
    {
        
        _authenticateService = authtenticate;
        _userService = userService;

    }

   

    [HttpPost("register")]
    public async Task<ActionResult<UserToken>> AddUser(UserDTO userDTO)
    {
        if (!await _userService.IsAdmin(User.GetUserId()))
            return Unauthorized("You are not authorized to access this resource");

        if (userDTO == null)
            return BadRequest("Invalid user data.");
        
        var existingUser = await _authenticateService.UserExists(userDTO.Login, userDTO.Email);
        if (existingUser)
            return BadRequest("User already exists.");

        var user = await _userService.Add(userDTO);
        if (user == null)
            return BadRequest("Error creating user.");
        
        var token = _authenticateService.GenerateToken(user.Id, user.Login);
        return new UserToken { Token = token };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserToken>> Select(LoginModel loginModel)
    {
        var exists = await _authenticateService.UserExists(loginModel.Username);
        if (!exists) return Unauthorized("User not found");

        var isActive = await _authenticateService.UserIsActive(loginModel.Username);
        if (!isActive) return Unauthorized("User is not active");

        var result = await _authenticateService.Autheniticate(loginModel.Username, loginModel.Password);
        if (!result) return Unauthorized("User or Password invalid");

        var user = await _authenticateService.GetUserByUserName(loginModel.Username);
        
        var token = _authenticateService.GenerateToken(user.Id, user.Login);

        return new UserToken 
        {
            Token = token
        };
    }

}
