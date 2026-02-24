using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        private readonly IPhotoService _photoService;

        public MembersController(ILogger<MembersController> logger, IMemberService memberService, IPhotoService photoService)
        {
            _logger = logger;           
            _memberService = memberService;
            _photoService = photoService;
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

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto){
            var memberId = User.GetMemberId();
            
            // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(memberId))
            {
                return BadRequest("Member ID is required.");
            }

            var member = await _memberService.GetMemberForUpdate(memberId);
            if (member == null)
            {
                return NotFound("Member not found.");
            }

            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

            var result = await _memberService.UpdateMemberAsync(member);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update member.");
            }

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm]IFormFile file)
        {
            var member = await _memberService.GetMemberForUpdate(User.GetMemberId());

            if (member == null)
            {
                return BadRequest("Member not found.");
            }

            var result = await _photoService.UploadPhotoAsync(file);           

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };

            if(member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;                
            }

            member.Photos.Add(photo);

            if(await _memberService.SaveAllAsync()) return photo;          

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await _memberService.GetMemberForUpdate(User.GetMemberId());

            if (member == null)
            {
                return BadRequest("Member not found.");
            }

            var photo = member.Photos.SingleOrDefault(p => p.Id == photoId);

            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return NotFound("Photo not found.");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            var result = await _memberService.SaveAllAsync();
            if (!result)
            {
                return BadRequest("Failed to set main photo.");
            }

            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await _memberService.GetMemberForUpdate(User.GetMemberId());

            if (member == null)
            {
                return BadRequest("Member not found.");
            }

            var photo = member.Photos.SingleOrDefault(p => p.Id == photoId);

            if (photo == null || member.ImageUrl == photo.Url)
            {
                return BadRequest("This photo is the main photo and cannot be deleted.");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest($"{result.Error.Message} Failed to delete photo.");
                }
            }

            member.Photos.Remove(photo);

            if(await _memberService.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo.");
        }
    }
}
