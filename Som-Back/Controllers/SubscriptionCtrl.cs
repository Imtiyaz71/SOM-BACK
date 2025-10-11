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
    public class SubscriptionCtrl : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionservice;

        public SubscriptionCtrl(ISubscriptionService subscriptionservice)
        {
            _subscriptionservice = subscriptionservice;
        }
       
        [HttpGet("subscriptiontypes")]
        [Authorize]
        public async Task<IActionResult> GetSubscriptionTypes(int compId)
        {
            var mem = await _subscriptionservice.GetSubscriptionTypes(compId);

            if (mem == null)
                return NotFound("No Subscription Type found.");

            return Ok(mem);
        }
        [HttpGet("subscriptiontypebyid")]
        [Authorize]
        public async Task<IActionResult> GetSubscriptionTypeById(int id)
        {
            var mem = await _subscriptionservice.GetSubscriptionTypesById(id);

            if (mem == null)
                return NotFound("No Subscription Type found.");

            return Ok(mem);
        }
        [HttpGet("subscriptionbyproject")]
        [Authorize]
        public async Task<IActionResult> GetSubscriptionByProject(int compId,int projectid)
        {
            var mem = await _subscriptionservice.GetSubscriptionTypesByProject(compId,projectid);

            if (mem == null)
                return NotFound("No Subscription Type found.");

            return Ok(mem);
        }
        [HttpPost("savesubscriptiontype")]
        [Authorize]
        public async Task<IActionResult> SaveKistiType(SubscriptionTypes k)
        {
            var res = await _subscriptionservice.SaveSubscriptionType(k);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Subscription Type");
        }
    }
}
