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
    public class DashboardCtrl : ControllerBase
    {
        private readonly IDashboardService _dashboardservice;

        public DashboardCtrl(IDashboardService dashboardservice)
        {
            _dashboardservice = dashboardservice;
        }
        [HttpGet("dashcount")]
        [Authorize]
        public async Task<IActionResult> GetAccountBalance(int compId)
        {
            var mem = await _dashboardservice.GetDashboardCounts(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
    }
}
