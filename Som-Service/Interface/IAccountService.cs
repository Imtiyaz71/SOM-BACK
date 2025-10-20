using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IAccountService
    {
        public Task<List<VM_kistiandSubs>> GetKistiReceive(int compId);
        public Task<List<VM_kistiandSubs>> GetSubscriptionReceive(int compId);
        public Task<List<VW_RegularSubscription>> GetRegularSubscriptionReceive(int compId);
        public Task<List<SomityAccounts>> GetAccountBalance(int compId);
        public Task<List<Vendor>> GetVendor();
        public Task<List<VW_BalanceSegment>> GetBalanceSegment(int compId);
        public Task<List<VW_BalanceSegment>> GetBalanceSegmentById(int id);
        public Task<List<VW_BalanceAddHistory>> GetBalanceAddHistory(int compId);
        public Task<List<VW_SomityAccTransection>> GetSomityAccTransection(VW_AccDrCr model);
        public Task<List<VW_BalanceWithdraw>> GetBalanceWithDraw(int compId);
        public Task<List<VW_MemberProjectAccount>> GetProjectAccountByMemberAndProject(int? compId, int? memNo, int? projectId);
        public Task<List<VW_MemberBalance>> GetMemberBalance(int compId);
        public Task<List<VW_Journal>> GetJournal(int compId);
        public Task<List<VW_ProjectAccountSummary>> GetProjectAccountSummary(int compId);
        public Task<string> SaveKistiAmount(VM_SaveKistiandSubs model);
        public Task<string> SavesubscriptionAmount(VM_SaveKistiandSubs model);
        public Task<string> SaveRegularSubs(VM_RegularSubs model);
        public Task<string> SaveAccountSegment(BalanceSegemnt model);
        public Task<string> AddBalanceWithdraw(BalanceWithdraw model);
        public Task<string> BounceBalanceWithdraw(VWBounceBalanceWithdrwal model);
        public Task<VW_Response> SaveRepay(RePay model);
    }
}
