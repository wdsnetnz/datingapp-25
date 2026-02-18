using System;
using API.Entities;
using API.Interfaces;

namespace API.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public Task<Member> GetMemberAsync(string id)
    {
        return _memberRepository.GetMemberAsync(id);
    }

    public Task<Member?> GetMemberForUpdate(string id)
    {
       return _memberRepository.GetMemberForUpdate(id);
    }

    public Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return _memberRepository.GetMembersAsync();
    }

    public Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        return _memberRepository.GetPhotosForMemberAsync(memberId);
    }

    public Task<bool> SaveAllAsync()
    {
        return _memberRepository.SaveAllAsync();
    }

    public void Update(Member member)
    {
        _memberRepository.Update(member);
    
    }

    public Task<bool> UpdateMemberAsync(Member member)
    {
        _memberRepository.Update(member);
        return _memberRepository.SaveAllAsync();
    }
}
