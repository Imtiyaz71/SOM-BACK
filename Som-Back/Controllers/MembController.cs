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
    public class MembController : ControllerBase
    {
        private readonly IMemberService _memberervice;

        public MembController(IMemberService memberservice)
        {
            _memberervice = memberservice;
        }
        [HttpGet("memberinfoall")]
        [Authorize] 
        public async Task<IActionResult> GetAllMember()
        {
            var mem = await _memberervice.Getmember();

            if (mem == null)
                return NotFound("No Member found.");

            return Ok(mem);
        }
        [HttpGet("memberinfobyid")]
        [Authorize]  
        public async Task<IActionResult> GetAllMemberById(int memno)
        {
            var mem = await _memberervice.GetmemberById(memno);

            if (mem == null)
                return NotFound("No Member found.");

            return Ok(mem);
        }
        [HttpGet("memberphoto")]
        public async Task<IActionResult> GetUserPhoto(int memno)
        {
            var user = await _memberervice.GetmemberById(memno);
            string path = user.Photo;
            if (path == null)
            {
                path = "Uploads/avatar.png";
            }
            var filePath = Path.Combine(path); // tumaar path jekhane photo ase
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg"); // mime type dynamic korte paro
        }
        [HttpGet("memberdocument")]
        public async Task<IActionResult> GetUserDocument(int memno)
        {
            var user = await _memberervice.GetmemberById(memno);

            // DB te jodi PDF er file path save thake
            string path = user.IdenDocu;
            if (string.IsNullOrEmpty(path))
            {
                // default pdf or error handle
                return NotFound("No document found");
            }

            var filePath = Path.Combine(path); // or use your upload folder + filename

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            // For PDF, content-type:
            return File(fileBytes, "application/pdf");
        }

        [Authorize]
        [HttpPost("savemember")]
        public async Task<IActionResult> SaveMemberInfo([FromBody] Members mem)
        {
            if (mem == null)
                return BadRequest(new { message = "Member data is null" });

            try
            {
                var result = await _memberervice.SaveMember(mem);

                // result যদি error string return করে তাহলে BadRequest
                if (result.StartsWith("Error") || result.StartsWith("No"))
                    return BadRequest(new { message = result });

                // নাহলে OK message
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost("transfermember")]
        public async Task<IActionResult> TransferMember([FromBody] Members mem)
        {
            if (mem == null)
                return BadRequest(new { message = "Member data is null" });

            try
            {
                var result = await _memberervice.TransferMember(mem);

                // result যদি error string return করে তাহলে BadRequest
                if (result.StartsWith("Error") || result.StartsWith("No"))
                    return BadRequest(new { message = result });

                // নাহলে OK message
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpPost("editmember")]
        [Authorize]
        public async Task<IActionResult> EditMember([FromBody] Members mem)
        {
            var member = await _memberervice.EditMember(mem);

            if (member == null)
                return BadRequest("Failed to Edit Member Data");

            return Ok(member);
        }
    }
}
