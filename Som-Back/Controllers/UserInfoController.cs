using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;

namespace Som_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfo _usersinfoervice;

        public UserInfoController(IUserInfo userinfoservice)
        {
            _usersinfoervice = userinfoservice;
        }

        [HttpGet("userbasicinfo")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetBasicUserInfo()
       {
            var user = await _usersinfoervice.GetUserInfoBasic();

            if (user == null)
                return NotFound("No user found.");

            return Ok(user);
        }
        [HttpGet("userbasicinfobyuser")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetBasicUserInfoByUser(string Username)
        {
            var user = await _usersinfoervice.GetUserInfoBasicByUser(Username);

            if (user == null)
                return NotFound("No menus User.");

            return Ok(user);
        }
        [HttpGet("userinfoall")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetUserInfo(string Username)
        {
            var user = await _usersinfoervice.GetUserInfo(Username);

            if (user == null)
                return NotFound("No User FOund.");

            return Ok(user);
        }
        [HttpGet("userphotobyusername")]
        public async Task<IActionResult> GetUserPhoto(string Username)
        {
            var user = await _usersinfoervice.GetUserInfo(Username);
            string path = user.Photo;
            if(path == null)
                {
                path = "Uploads/avatar.png";
            }
            var filePath = Path.Combine(path); // tumaar path jekhane photo ase
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg"); // mime type dynamic korte paro
        }
        [HttpGet("mapdetails")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> GetMapDetails()
        {
            var map = await _usersinfoervice.GetMapdetails();

            if (map == null || map.Count == 0)
                return NotFound("No Details found for this role.");

            return Ok(map);
        }
        [HttpPost("savebasicuserinfo")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> SaveBasicUserInfo([FromBody] UserInfoBasic basic)
        {
            var existingUser = await _usersinfoervice.GetUserInfoBasicByUser(basic.Username);
            if (existingUser != null)
                return BadRequest("Username already exists. Please choose a different one.");

            var user = await _usersinfoervice.SaveUserInfoBasic(basic);

            if (user == null)
                return BadRequest("Failed to save user info");

            return Ok(user);
        }
        [HttpPost("saveusereducation")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> SaveEducationInfo([FromBody] UserInfoEducation basic)
        {
            var existingUser = await _usersinfoervice.GetUserInfoEducationByUser(basic.Username);
            if (existingUser != null)
                return BadRequest("Username already exists. Please choose a different one.");

            var user = await _usersinfoervice.SaveUserInfoEducation(basic);

            if (user == null)
                return NotFound("Bad Request");

            return Ok(user);
        }
        [HttpPost("edituserinfo")]
        [Authorize]
        public async Task<IActionResult> EditUserInfo([FromForm] IFormFile file, [FromForm] VW_UserInfo user)
        {
            try
            {
                // Service method call (photo path thakle save korbe, na thakle current photo use korbe)
                var result = await _usersinfoervice.EditUserInfo(file, user);

                if (result.Contains("Error"))
                    return BadRequest(new { message = result });

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
        [HttpPost("uploadphoto")]
        [Authorize]
        public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, [FromForm] string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is required");

            // Create UserPhoto object
            var userPhoto = new UserPhoto
            {
                Username = username
            };

            // Call service method
            var resultMsg = await _usersinfoervice.SaveUserPhoto(file, userPhoto);

            if (resultMsg.Contains("successfully"))
                return Ok(new { message = resultMsg });
            else
                return BadRequest(new { message = resultMsg });
        }
        [HttpPost("savemapper")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> SaveMapping([FromBody] Mapper basic)
        {
           
            var user = await _usersinfoervice.UserMap(basic);

            if (user == null)
                return BadRequest("Failed to save user info");

            return Ok(user);
        }
        [HttpDelete("delete-userinfo")]
        [Authorize]  // Require login for menu fetching (optional)
        public async Task<IActionResult> DeleteUser(string username,string deleteby)
        {

            var result = await _usersinfoervice.DeleteUser(username, deleteby);

            if (string.IsNullOrEmpty(result))
                return BadRequest(new { message = "Failed to delete user info" });

            return Ok(new { message = result });
        }
    }
}
