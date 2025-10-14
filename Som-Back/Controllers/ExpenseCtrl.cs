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
    public class ExpenseCtrl : ControllerBase
    {
        private readonly IExpenseService _expenseservice;

        public ExpenseCtrl(IExpenseService expenseservice)
        {
            _expenseservice = expenseservice;
        }
        [HttpGet("get-expense-type")]
        [Authorize]
        public async Task<IActionResult> GetExpenseType(int compId)
        {
            var mem = await _expenseservice.GetExpenseType(compId);

            if (mem == null)
                return NotFound("No Data found.");

            return Ok(mem);
        }
        [HttpPost("save-expense-type")]
        [Authorize]
        public async Task<IActionResult> SaveExpenseType(ExpenseType model)
        {
            var res = await _expenseservice.AddExpenseType(model);
            return Ok(new { Message = res ?? "Failed to save" });
        }

        [HttpPost("delete-expense-type")]
        [Authorize]
        public async Task<IActionResult> DeleteExpenseType(int id)
        {
            var res = await _expenseservice.DeleteExpense(id);
            return Ok(new { Message = res ?? "Failed to delete" });
        }
    }
}
