using System;
using API.Entities;

namespace API.Interfaces;

public interface IDatingService
{
    Task<AppUser> GetMemberAsync(string id);
    Task<IReadOnlyList<AppUser>> GetMembersAsync();
}
