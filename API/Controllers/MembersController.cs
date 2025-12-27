using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Route: is now in base controller  localhost:5000/api/members
    public class MembersController : BaseController
    {
        private readonly ILogger<MembersController> _logger;
        private readonly IDatingService _datingService;

        public MembersController(ILogger<MembersController> logger, IDatingService datingService)
        {
            _logger = logger;
            _datingService = datingService;
        }

        [HttpGet]
        public async Task<ActionResult> GetMembers()
        {
            var members = await _datingService.GetMembersAsync();
            return Ok(members);
        }

        [Authorize]
        [HttpGet("{id}")] // localhost:5000/api/members/3
        public async Task<ActionResult> GetMember(string id)
        {
            var member = await _datingService.GetMemberAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
    }
}
