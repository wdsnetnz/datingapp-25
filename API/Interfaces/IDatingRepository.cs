using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IDatingRepository
{
    Task<AppUser?> GetMemberAsync(string id);
    Task<IReadOnlyList<AppUser>> GetMembersAsync();
    Task<bool> RegisterUserAsync(AppUser user);
    Task<bool> EmailExistsAsync(string email);
    Task<AppUser?> GetUserAsync(string email);
}
