using System;
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
}
