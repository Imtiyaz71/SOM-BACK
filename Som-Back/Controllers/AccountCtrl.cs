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
        [HttpGet("vendor")]
        [Authorize]
        public async Task<IActionResult> GetVendor()
        {
            var mem = await _accountservice.GetVendor();

            if (mem == null)
                return NotFound("No Vendor Type found.");

            return Ok(mem);
        }
        [HttpGet("accountbalance")]
        [Authorize]
        public async Task<IActionResult> GetAccountBalance(int compId)
        {
            var mem = await _accountservice.GetAccountBalance(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("getvalancesegment")]
        [Authorize]
        public async Task<IActionResult> GetBalanceSegment(int compId)
        {
            var mem = await _accountservice.GetBalanceSegment(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("getvalancesegmentbyid")]
        [Authorize]
        public async Task<IActionResult> GetBalanceSegmentById(int id)
        {
            var mem = await _accountservice.GetBalanceSegmentById(id);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
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
        [HttpGet("regularsubscriptionreceive")]
        [Authorize]
        public async Task<IActionResult> GetRegularSerbscriptionRec(int compId)
        {
            var mem = await _accountservice.GetRegularSubscriptionReceive(compId);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("balance-add-history")]
        [Authorize]
        public async Task<IActionResult> GetBalanceAddHistory(int compId)
        {
            var mem = await _accountservice.GetBalanceAddHistory(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("get-Balance-withdraw")]
        [Authorize]
        public async Task<IActionResult> GetBalanceWithdraw(int compId)
        {
            var mem = await _accountservice.GetBalanceWithDraw(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpGet("memberprojectaccount")]
        [Authorize]
        public async Task<IActionResult> GetProjectAccountByMemberAndProject(
          [FromQuery] int? compId,
          [FromQuery] int? memNo,
          [FromQuery] int? projectId)
        {
            try
            {
                var result = await _accountservice.GetProjectAccountByMemberAndProject(compId, memNo, projectId);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong", Details = ex.Message });
            }
        }
        [HttpPost("getsomityacctransection")]
        [Authorize]
        public async Task<IActionResult> GetSomityTransection([FromBody] VW_AccDrCr model)
        {
            if (model == null)
                return BadRequest("Input is null");

            var result = await _accountservice.GetSomityAccTransection(model);
            return Ok(result);
        }
        [HttpPost("savebalancesegment")]
        [Authorize]
        public async Task<IActionResult> SaveBalanceSegment(BalanceSegemnt model)
        {
            var res = await _accountservice.SaveAccountSegment(model);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Balance Segment");
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
        [HttpPost("saveregularsubscription")]
        [Authorize]
        public async Task<IActionResult> SaveSubscriptionRegular([FromBody] VM_RegularSubs model)
        {
            var res = await _accountservice.SaveRegularSubs(model);
            return Ok(res ?? "Failed to save Subscription");
        }
        [HttpPost("save-withdraw")]
        [Authorize]
        public async Task<IActionResult> SaveWithdraw([FromBody] BalanceWithdraw model)
        {
            if (model == null)
                return BadRequest("Invalid request data.");

            var res = await _accountservice.AddBalanceWithdraw(model);

            // result string check kore response return
            if (res.StartsWith("Error"))
                return StatusCode(500, res);
            else
                return Ok(new { Message = res ?? "Failed to save" });
          
        }
        [HttpPost("bounce-balance-withdraw")]
        [Authorize]
        public async Task<IActionResult> Bounce([FromBody] VWBounceBalanceWithdrwal model)
        {
            var result = await _accountservice.BounceBalanceWithdraw(model);
            if (result.StartsWith("Error"))
                return StatusCode(500, result);

            return Ok(new { Message = result ?? "Failed to save" });
        }
        [HttpPost("repay-balance")]
        [Authorize]
        public async Task<IActionResult> RePay([FromBody] RePay model)
        {
            if (model.Paid <= 0)
                return BadRequest(new VW_Response { StatusCode = 0, Message = "Paid amount must be greater than zero." });

            var result = await _accountservice.SaveRepay(model);

            if (result.StatusCode == 0 && result.Message.StartsWith("Error"))
                return StatusCode(500, result);

            return Ok(result);
        }
    }
}
