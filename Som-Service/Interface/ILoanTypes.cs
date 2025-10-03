using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface ILoanTypes
    {
        public Task<List<VM_LoanTypes>> GetLoanTypes(int compId);
        public Task<VM_LoanTypes> GetLoanTypeById(int id);
        public Task<string> SaveLoanType(LoanTypes k);
    }
}
