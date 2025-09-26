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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var token = await _authService.LoginAsync(model);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token });
        }
        [Authorize]
        [HttpGet("cominfo")]
        public async Task<IActionResult> CompanyInfo()
        {
            var info = await _authService.CompanyInfo();
            if (info == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { info });
        }

        [HttpPost("companyadd")]
        public async Task<IActionResult> Addcompany([FromBody] CompanyInfo model)
        {
            var res = await _authService.SaveCompany(model);
            if (res == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { res });
        }
    }
}
