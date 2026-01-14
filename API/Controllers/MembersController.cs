using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Route: is now in base controller  localhost:5000/api/members
    [Authorize]
    public class MembersController : BaseController
    {
        private readonly ILogger<MembersController> _logger;
        
        private readonly IMemberService _memberService;
        public MembersController(ILogger<MembersController> logger, IMemberService memberService)
        {
            _logger = logger;           
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult> GetMembers()
        {
            var members = await _memberService.GetMembersAsync();
            return Ok(members);
        }

        [HttpGet("{id}")] // localhost:5000/api/members/3
        public async Task<ActionResult> GetMember(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Member ID is required.");
            }

            var member = await _memberService.GetMemberAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult> GetPhotosForMember(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Member ID is required.");
            }

            var photos = await _memberService.GetPhotosForMemberAsync(id);
            return Ok(photos);
        }
    }
}
