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
    public class AccountCtrl : ControllerBase
    {
        private readonly IAccountService _accountservice;

        public AccountCtrl(IAccountService accountservice)
        {
            _accountservice = accountservice;
        }
        [HttpGet("kistireceive")]
        [Authorize]
        public async Task<IActionResult> GetKistiReceive(int compId)
        {
            var mem = await _accountservice.GetKistiReceive(compId);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("subscriptionreceive")]
        [Authorize]
        public async Task<IActionResult> GetSubscriptionReceive(int compId)
        {
            var mem = await _accountservice.GetSubscriptionReceive(compId);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpPost("savekistiamount")]
        [Authorize]
        public async Task<IActionResult> SaveKistiAmount(VM_SaveKistiandSubs k)
        {
            var res = await _accountservice.SaveKistiAmount(k);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Kisti Type");
        }
        [HttpPost("savesubscriptionamount")]
        [Authorize]
        public async Task<IActionResult> SaveSubscriptionAmount(VM_SaveKistiandSubs k)
        {
            var res = await _accountservice.SavesubscriptionAmount(k);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Kisti Type");
        }
    }
}
