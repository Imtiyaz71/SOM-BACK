using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IAccountService
    {
        public Task<List<VM_kistiandSubs>> GetKistiReceive(int compId);
        public Task<List<VM_kistiandSubs>> GetSubscriptionReceive(int compId);
        public Task<string> SaveKistiAmount(VM_SaveKistiandSubs model);
        public Task<string> SavesubscriptionAmount(VM_SaveKistiandSubs model);
    }
}
