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
    public class StaffCtrl : ControllerBase
    {
        private readonly IStaffService _staffservice;

        public StaffCtrl(IStaffService staffservice)
        {
            _staffservice = staffservice;
        }
        [HttpGet("getstaffdesignation")]
        [Authorize]
        public async Task<IActionResult> GetStaffDesignation(int compId)
        {
            var mem = await _staffservice.GetStaffDesignation(compId);

            if (mem == null)
                return NotFound("No Designation Type found.");

            return Ok(mem);
        }
        [HttpGet("getstaffinfo")]
        [Authorize]
        public async Task<IActionResult> GetStaffInfo(int compId)
        {
            var mem = await _staffservice.GetStaffInfo(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost("save-staff-designation")]
        [Authorize]
        public async Task<IActionResult> GetSomityTransection([FromBody] StaffDesignation model)
        {
            if (model == null)
                return BadRequest("Input is null");

            var result = await _staffservice.SaveStaffDesignation(model);
            return Ok(result);
        }
        [HttpPost("delete-staff-designation")]
        [Authorize]
        public async Task<IActionResult> DeleteStaffDesignation(int id, int compId)
        {
            var result = await _staffservice.DeleteStaffDesignation(id, compId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("save-staff")]
        [Authorize]
        public async Task<IActionResult> SaveStaff([FromBody] Staff model)
        {
            if (model == null)
                return BadRequest(new VW_Response { StatusCode = 400, Message = "Invalid staff data." });

            var result = await _staffservice.SaveStaffInfo(model);

            if (result.StatusCode == 200)
                return Ok(result);

            if (result.StatusCode == 400)
                return BadRequest(result);

            return StatusCode(500, result); // unexpected errors
        }
        [HttpPost("deactivate-staff")]
        [Authorize]
        public async Task<IActionResult> DeactivateStaff(int id)
        {
            var result = await _staffservice.DeactiveStaff(id);

            if (result.StatusCode == 200)
                return Ok(result);
            else
                return StatusCode(result.StatusCode, result);
        }
        [HttpGet("get-archive-staff")]
        public async Task<IActionResult> GetArchiveStaff(int compId)
        {
            var result = await _staffservice.GetArchiveStaff(compId);
            return Ok(result);
        }
    }
}
