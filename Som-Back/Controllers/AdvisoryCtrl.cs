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
    public class AdvisoryCtrl : ControllerBase
    {
        private readonly IAdvisoryService _advisoryservice;

        public AdvisoryCtrl(IAdvisoryService advisoryservice)
        {
            _advisoryservice = advisoryservice;
        }
        [HttpGet("get-advisory-role")]
        [Authorize]
        public async Task<IActionResult> GetAdvisoryRole(int compId)
        {
            var mem = await _advisoryservice.GetAdvisoryRole(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost]
        [Route("add-advisory-role")]
        [Authorize]
        public async Task<IActionResult> AddAdvisoryRole([FromBody] AdvisoryRole model)
        {
            if (model == null)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid expense data"
                });
            }

            var response = await _advisoryservice.AddAdvisoryRole(model);

            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }
        [HttpPost]
        [Route("delete-advisory-role")]
        [Authorize]
        public async Task<IActionResult> DeleteAdvisoryRole(int compId,int id)
        {
            if (compId == null && id==null)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid expense data"
                });
            }

            var response = await _advisoryservice.DeleteAdvisoryRole(compId,id);

            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }
        [HttpGet]
        [Authorize]
        [Route("get-advisory-list")]
        public async Task<IActionResult> GetAdvisoryList(int compId, int cStatus = 1)
        {
            if (compId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid company ID."
                });
            }

            var advisoryList = await _advisoryservice.GetAdvisoryList(compId, cStatus);

            return Ok(advisoryList);
        }
        [HttpPost]
        [Authorize]
        [Route("add-advisory")]
        public async Task<IActionResult> AddAdvisory([FromBody] Advisory model)
        {
            if (model == null ||
                model.CompId <= 0 ||
                model.MemNo <= 0 ||
                model.AdRole <= 0 ||
                string.IsNullOrWhiteSpace(model.Validity))
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid advisory data."
                });
            }

            var response = await _advisoryservice.AddAdvisory(model);

            if (response.StatusCode == 200)
                return Ok(response);
            else if (response.StatusCode == 409)
                return Conflict(response); // Duplicate entry
            else
                return StatusCode(500, response);
        }
        [HttpPost]
        [Route("deactiveadvisory")]
        [Authorize]
        public async Task<IActionResult> DeactiveAdvisory(int compId, int id)
        {
            if (compId == null && id == null)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid expense data"
                });
            }

            var response = await _advisoryservice.DeactiveAdvisory(compId, id);

            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
