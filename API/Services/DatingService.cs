using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services;

public class DatingService : IDatingService
{
    private readonly IDatingRepository _datingRepository;

    public DatingService(IDatingRepository datingRepository)
    {
        _datingRepository = datingRepository;
    }

    public async Task<AppUser> GetMemberAsync(string id)
    {
        return await _datingRepository.GetMemberAsync(id);
    }

    public Task<IReadOnlyList<AppUser>> GetMembersAsync()
    {
        return _datingRepository.GetMembersAsync();
    }

    public async Task<bool> RegisterUserAsync(AppUser user)
    {
        return await _datingRepository.RegisterUserAsync(user);
    }
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _datingRepository.EmailExistsAsync(email);
    }

    public Task<AppUser> GetUserAsync(string email)
    {
        return _datingRepository.GetUserAsync(email);
    }
}
