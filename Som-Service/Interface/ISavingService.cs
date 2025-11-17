using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface ISavingService
    {
        public Task<VW_Response> SaveSavingAccount(SavingsAccount model);
        public Task<List<VM_SavingAccount>> GetSavingsAccountList(int compId);
        public Task<VW_Response> SaveAccountOperation(VM_AccountOperation model);
    }
}
