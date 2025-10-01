using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface ISubscriptionService
    {
       
        public Task<List<VM_SubscriptionTypes>> GetSubscriptionTypes(int compId);
        public Task<VM_SubscriptionTypes> GetSubscriptionTypesById(int id);
        public Task<string> SaveSubscriptionType(SubscriptionTypes k);
    }
}
