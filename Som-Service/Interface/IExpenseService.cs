using Som_Models.Models;

namespace Som_Service.Interface
{
    public interface IExpenseService
    {
        public Task<List<ExpenseType>> GetExpenseType(int compId);
        public Task<string> AddExpenseType(ExpenseType model);
        public Task<string> DeleteExpense(int id);
    }
}
