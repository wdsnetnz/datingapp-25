using System;
using API.Entities;

namespace API.Interfaces;

public interface IDatingRepository
{
    Task<AppUser?> GetMemberAsync(string id);
    Task<IReadOnlyList<AppUser>> GetMembersAsync();
}
