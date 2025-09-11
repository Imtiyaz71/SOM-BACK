using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Service.Interface;

namespace Som_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectedValueController : ControllerBase
    {
        private readonly ISelectedValueService _selectedService;

        public SelectedValueController(ISelectedValueService selectedService)
        {
            _selectedService = selectedService;
        }
        [HttpGet("genderlist")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetGender()
        {
            var autho = await _selectedService.GetGender();

            if (autho == null || autho.Count == 0)
                return NotFound("No Gender found for this role.");

            return Ok(autho);
        }
        [HttpGet("activationlist")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetAllActivation()
        {
            var autho = await _selectedService.GetActivation();

            if (autho == null || autho.Count == 0)
                return NotFound("No Activation found for this role.");

            return Ok(autho);
        }
        [HttpGet("authorizerlist")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetAllAuthorizer()
        {
            var autho = await _selectedService.GetSelectedAuthorizer();

            if (autho == null || autho.Count == 0)
                return NotFound("No Authorize found for this role.");

            return Ok(autho);
        }
        [HttpGet("designationlist")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetAllDesignation()
        {
            var desig = await _selectedService.GetSelectedDesignation();

            if (desig == null || desig.Count == 0)
                return NotFound("No Designation found for this role.");

            return Ok(desig);
        }
    }
}
