using System;
using API.Entities;

namespace API.Interfaces;

public interface IMemberService
{
    void Update(Member  member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member> GetMemberAsync(string id);

    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
    Task<bool> UpdateMemberAsync(Member member);

    Task<Member?> GetMemberForUpdate(string id);
}
