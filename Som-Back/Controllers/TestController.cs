using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Som_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // Je role gulo access pabe ta define koro:
        [Authorize(Roles = "Admin,User")]
        [HttpGet("checkrole")]
        public IActionResult CheckRole()
        {
            if (User.IsInRole("Admin"))
            {
                return Ok("Admin");
            }
            else if (User.IsInRole("User"))
            {
                return Ok("User");
            }
            else
            {
                return Forbid();  // Role match na korle forbid koro
            }
        }
    }
}
