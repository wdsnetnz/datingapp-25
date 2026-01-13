using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet("auth")]
        public IActionResult GetAuthError()
        {
            return Unauthorized();
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFoundError()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequestError()   
        {
            return BadRequest("This is a bad request");
        }
    }
}
