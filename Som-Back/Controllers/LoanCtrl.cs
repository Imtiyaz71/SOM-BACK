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
    public class LoanCtrl : ControllerBase
    {
        private readonly ILoanTypes _loanservice;

        public LoanCtrl(ILoanTypes loanservice)
        {
            _loanservice = loanservice;
        }
        [HttpGet("loantype")]
        [Authorize]
        public async Task<IActionResult> GetLoanType(int compId)
        {
            var mem = await _loanservice.GetLoanTypes(compId);

            if (mem == null)
                return NotFound("No Loan Type found.");

            return Ok(mem);
        }
        [HttpGet("loantypebyid")]
        [Authorize]
        public async Task<IActionResult> GetLoanTypeById(int id)
        {
            var mem = await _loanservice.GetLoanTypeById(id);

            if (mem == null)
                return NotFound("No Loan Type found.");

            return Ok(mem);
        }
        [HttpPost("saveloantype")]
        [Authorize]
        public async Task<IActionResult> SaveLoanType(LoanTypes k)
        {
            var res = await _loanservice.SaveLoanType(k);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Loan Type");
        }
    }
}
