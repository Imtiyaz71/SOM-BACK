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
        public async Task<IActionResult> GetLoanType([FromQuery] int compId)
        {
            if (compId <= 0)
                return BadRequest("Invalid Company Id.");

            var mem = await _loanservice.GetLoanTypes(compId);

            if (mem == null || !mem.Any())
                return NotFound("No Loan Type found.");

            return Ok(mem);
        }

        [HttpGet("loantypebyid")]
        [Authorize]
        public async Task<IActionResult> GetLoanTypeById([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Loan Type Id.");

            var mem = await _loanservice.GetLoanTypeById(id);

            if (mem == null)
                return NotFound("No Loan Type found.");

            return Ok(mem);
        }

        [HttpPost("saveloantype")]
        [Authorize]
        public async Task<IActionResult> SaveLoanType([FromBody] LoanTypes k)
        {
            if (k == null || string.IsNullOrWhiteSpace(k.TypeName))
                return BadRequest("Invalid Loan Type data.");

            var res = await _loanservice.SaveLoanType(k);

            return Ok(res ?? "Failed to save Loan Type");
        }
    }
}
