using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using Som_Service.Service;

namespace Som_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelCtrl : ControllerBase
    {
        private readonly IAdminPanel _adminpanelservice;

        public AdminPanelCtrl(IAdminPanel adminpanelservice)
        {
            _adminpanelservice = adminpanelservice;
        }
        [HttpPost("admin-login")]
        public async Task<IActionResult> Login([FromBody] AdminPanel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminpanelservice.LoginAsync(model);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new
            {
                token = result.Token,
                fullName = result.Fullname,
                username = result.Username,
              
            });
    }
        [HttpGet("getcompanymodule")]
        [Authorize]
        public async Task<IActionResult> GetComapnyModule()
        {
            var mem = await _adminpanelservice.GetCompanyModule();

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost("delete-company-module")]
        [Authorize]
        public async Task<IActionResult> DeleteModule(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid Module ID!"
                });
            }

            var result = await _adminpanelservice.DeleteCompanyModule(id);

            return result.StatusCode switch
            {
                1 => Ok(result),         // Deleted successfully
                0 => NotFound(result),   // Record not found
                _ => StatusCode(500, result) // Server error
            };
        }
        [HttpGet("getcompanymenu")]
        [Authorize]
        public async Task<IActionResult> GetComapnyMenu()
        {
            var mem = await _adminpanelservice.GetCompanyMenu();

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("getcompanymenubyid")]
        [Authorize]
        public async Task<IActionResult> GetComapnyMenuById(int compId)
        {
            var mem = await _adminpanelservice.GetCompanyMenuByCompany(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
    }
}
