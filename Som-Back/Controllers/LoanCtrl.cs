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
        [HttpGet("borrowerloan")]
        [Authorize]
        public async Task<IActionResult> GetLoanBorrower([FromQuery] int compId)
        {
            if (compId <= 0)
                return BadRequest("Invalid Company Id.");

            var mem = await _loanservice.GetBorrowerLoanInfo(compId);

            if (mem == null || !mem.Any())
                return NotFound("No Loan  found.");

            return Ok(mem);
        }
        [HttpPost("SaveLoanSension")]
        [Authorize]
        public async Task<IActionResult> SaveLoanSension([FromBody] VW_LoanSensionRequest model)
        {
            if (model == null)
                return BadRequest(new VW_Response { StatusCode = 0, Message = "Invalid request" });

            var response = await _loanservice.SaveLoanSension(model);

            if (response.StatusCode == 1)
                return Ok(response);
            else
                return BadRequest(response);
        }
        [HttpGet("loanpaiddetails")]
        public async Task<IActionResult> GetLoanSensionDetails(int compId)
        {
            try
            {
                var loanDetails = await _loanservice.GetLoanSensionDetails(compId);

                if (loanDetails == null || !loanDetails.Any())
                    return NotFound(new { message = "No loan data found for this company." });

                return Ok(loanDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
        [HttpPost("save-loan-paid")]
        [Authorize]
        public async Task<IActionResult> SaveLoanPaid([FromBody] LoanPaidHistory model)
        {
            if (model == null)
                return BadRequest(new VW_Response { StatusCode = 400, Message = "Invalid data." });

            var result = await _loanservice.SaveLoanPaid(model);

            if (result.StatusCode == 200)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet("loan-paid-history")]
        public async Task<IActionResult> GetLoanPaidHistory(int compId)
        {
            var data = await _loanservice.LoanPaidHistory(compId);
            return Ok(data);
        }
    }
}
