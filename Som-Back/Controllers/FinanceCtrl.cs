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

        public FinanceCtrl(ISavingService savingService)
        {
            _savingservice = savingService;
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

    }
}
