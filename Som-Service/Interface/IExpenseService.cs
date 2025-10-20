using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IExpenseService
    {
        public Task<List<ExpenseType>> GetExpenseType(int compId);
        public Task<string> AddExpenseType(ExpenseType model);
        public Task<string> DeleteExpenseType(int id);
        public Task<List<VW_Expense>> GetExpense(int compId);
        public Task<List<VW_MonthlyExpense>> GetMonthlyExpense(int compId,int year);
        public Task<string> AddExpense(Expense model);
        public Task<VW_Response> AddProjectWiseExpense(ProjectWiseExpense model);
        public Task<List<VW_ProjectWiseExpense>> GetProjectExpense(int compId);
    }
}
