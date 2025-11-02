using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IAdminPanel
    {
        Task<AdminLoginResponse> LoginAsync(AdminPanel model);
        public Task<List<VW_CompanyModule>> GetCompanyModule();
        public Task<List<VW_ShowCompanyMenu>> GetCompanyMenu();
        public Task<List<VW_ShowCompanyMenu>> GetCompanyMenuByCompany(int compId);
        public Task<VW_Response> DeleteCompanyModule(int id);
        public Task<VW_Response> EditClientStatus(ClientStatus model);
        public Task<List<VW_ClientStatus>> GetClientStatus();
    }
}
