using System;
using System.Security.Cryptography;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly ILogger<MembersController> _logger;
    private readonly IDatingService _datingService;
    private readonly ITokenService _tokenService;

    public AccountController(ILogger<MembersController> logger, IDatingService datingService, ITokenService tokenService)
    {
        _logger = logger;
        _datingService = datingService;
        _tokenService = tokenService;
    }

    [HttpPost("register")] // POST: api/account/register        
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        // check if email exists
        if (await _datingService.EmailExistsAsync(registerDto.Email))
        {
            return BadRequest("Email is already taken");
        }

        // Additional validation can be added here    
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            Email =  registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        await _datingService.RegisterUserAsync(user);

        return Ok();
    }

    [HttpPost("login")] // POST: api/account/login        
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _datingService.GetUserAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return user.ToUserDto(_tokenService);
    }
}
