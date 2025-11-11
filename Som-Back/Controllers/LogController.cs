using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Models.Models;
using Som_Service.Interface;

namespace Som_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LogController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] Login model)
        //{
        //    var token = await _authService.LoginAsync(model);
        //    if (token == null)
        //        return Unauthorized("Invalid credentials");

        //    return Ok(new { token });
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var response = await _authService.LoginAsync(model);

            // 1️⃣ Null check
            if (response == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // 2️⃣ Expired subscription check
            if (!string.IsNullOrEmpty(response.Message) &&
                response.Message.Contains("expired", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new { message = response.Message });
            }

            // 3️⃣ Invalid login check (no token)
            if (string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(new { message = response.Message ?? "Invalid login attempt" });
            }

            // 4️⃣ Success response
            return Ok(new
            {
                token = response.Token,
                role = response.Role,
                fullname = response.Fullname,
                username = response.Username,
                cname = response.cName,
                cid = response.cId,
                message = response.Message
            });
        }

        [Authorize]
        [HttpGet("cominfo")]
        public async Task<IActionResult> CompanyInfo(int cid)
        {
            var info = await _authService.CompanyInfo(cid);
            if (info == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { info });
        }
        [Authorize]
        [HttpPost("companyadd")]
        public async Task<IActionResult> Addcompany([FromBody] CompanyInfo model)
        {
            var res = await _authService.SaveCompany(model);
            if (res == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { res });
        }
        [Authorize]
        [HttpGet("allcominfo")]
        public async Task<IActionResult> AllCompanyInfo()
        {
            var info = await _authService.GetAllCompanyInfo();
            if (info == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { info });
        }
      
    }
}
