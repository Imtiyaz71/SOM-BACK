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
    public class FinanceCtrl : ControllerBase
    {
        private readonly ISavingService _savingservice;
        private readonly IRevenueService _revenueservice;
        public FinanceCtrl(ISavingService savingService, IRevenueService revenueservice)
        {
            _savingservice = savingService;
            _revenueservice = revenueservice;
        }
        [HttpGet("totalrevenue")]
        [Authorize]
        public async Task<IActionResult> GetTotalRevenue(int compId)
        {
            try
            {
                var total = await _revenueservice.TotalRevenue(compId);

                return Ok(new
                {
                    status = "success",
                    compId = compId,
                    totalRevenue = total
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }
        [HttpGet("get-saving-account")]
        [Authorize]
        public async Task<IActionResult> GetSavingAccountList(int compId)
        {
            var res = await _savingservice.GetSavingsAccountList(compId);

            if (res == null || res.Count == 0)
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "No records found",
                    Data = new List<VM_SavingAccount>()
                });
            }

            return Ok(new
            {
                Status = 1,
                Message = "Success",
                Data = res
            });
        }

        [HttpPost("save-saving-account")]
        [Authorize]
        public async Task<IActionResult> SaveSavingAccount(SavingsAccount model)
        {
            var res = await _savingservice.SaveSavingAccount(model);

            if (res == null)
            {
                return Ok(new { Status = 0, Message = "Failed to save", Id = 0 });
            }

            return Ok(new
            {
                Status = res.StatusCode,
                Message = res.Message
               
            });
        }
        [HttpPost("save-account-operation")]
        [Authorize]
        public async Task<IActionResult> SaveAccountOperation([FromBody] VM_AccountOperation model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid request payload."
                });
            }

            try
            {
                var result = await _savingservice.SaveAccountOperation(model);

                if (result.StatusCode==1)
                {
                    return Ok(new
                    {
                        Status = true,
                        Message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Server Error: " + ex.Message
                });
            }
        }
        [HttpGet("revenue-list")]
        [Authorize]
        public async Task<IActionResult> GetRevenueList(int compId)
        {
            var data = await _revenueservice.RevenueList(compId);
            return Ok(data);
        }

        [HttpPost("save-revenue-disburse")]
        [Authorize]
        public async Task<IActionResult> SaveDisburse([FromBody] RevenueDisburse model)
        {
            if (model == null)
            {
                return BadRequest(new VW_Response
                {
                    StatusCode = 400,
                    Message = "Invalid input data."
                });
            }

            var result = await _revenueservice.SaveDisburseRevenue(model);

            if (result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(500, result);
        }
    
}
}
