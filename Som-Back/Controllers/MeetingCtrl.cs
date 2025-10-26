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
    public class MeetingCtrl : ControllerBase
    {
        private readonly IMeetingService _meetingservice;

        public MeetingCtrl(IMeetingService meetingservice)
        {
            _meetingservice = meetingservice;
        }
        [HttpGet("get-meeting")]
        [Authorize]
        public async Task<IActionResult> GetMeeting(int compId)
        {
            var mem = await _meetingservice.GetMeeting(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("get-meeting-id")]
        [Authorize]
        public async Task<IActionResult> GetMeetingById(int compId,int id)
        {
            var mem = await _meetingservice.GetMeetingById(compId,id);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost]
        [Route("add-meeting")]
        [Authorize]
        public async Task<IActionResult> AddMeeting([FromBody] Meeting model)
        {
            if (model == null)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid expense data"
                });
            }

            var response = await _meetingservice.AddMeeting(model);

            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
