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
    public class ProjectCtrl : ControllerBase
    {
        private readonly IProjectService _projectservice;

        public ProjectCtrl(IProjectService projectservice)
        {
            _projectservice = projectservice;
        }
        [HttpGet("getproject")]
        [Authorize]
        public async Task<IActionResult> Getproject(int compId)
        {
            var mem = await _projectservice.GetProject(compId);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("getprojectbyprojectid")]
        [Authorize]
        public async Task<IActionResult> GetprojectById(int projectid)
        {
            var mem = await _projectservice.GetProjectByProjectId(projectid);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("getassign")]
        [Authorize]
        public async Task<IActionResult> GetAssign(int compId)
        {
            var mem = await _projectservice.GetProjectAssign(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("getassignbyproject")]
        [Authorize]
        public async Task<IActionResult> GetAssignById(int projectid, int compid)
        {
            var mem = await _projectservice.GetProjectAssignByprojectid(projectid,compid);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost("save-project")]
        [Authorize]
        public async Task<IActionResult> SaveProject([FromBody] Project model)
        {
            if (model == null)
                return BadRequest("Project data is required.");

            var result = await _projectservice.SaveProject(model); // result: (int StatusCode, string Message)

            // Use different variable names to avoid conflict
            return StatusCode(result.StatusCode, new { message = result.Message });
        }

        [HttpPost("save-assign-project")]
        [Authorize]
        public async Task<IActionResult> SaveProjectAssign([FromBody] ProjectAssign model)
        {
            if (model == null)
                return BadRequest("Project data is required.");

            var result = await _projectservice.saveassign(model); // result: (int StatusCode, string Message)

            // Use different variable names to avoid conflict
            return StatusCode(result.StatusCode, new { message = result.Message });
        }
        [HttpPost("cancel-assign")]
        public async Task<IActionResult> CancelProjectAssign(int compId, int memNo, int projectId)
        {
            try
            {
                var result = await _projectservice.ProjectAssignCancle(compId, memNo, projectId);

                if (result == null)
                    return NotFound(new { StatusCode = -1, Message = "No response from database." });

                if (result.StatusCode == 1)
                    return Ok(result); // ✅ success

                return BadRequest(result); // ❌ validation or logical error
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = -99,
                    Message = "Internal server error: " + ex.Message
                });
            }
        }
    }
}
