using Som_Models.Models;
using Som_Models.VW_Models;
namespace Som_Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(Login model);
        Task<CompanyInfo> CompanyInfo(int cid);
        public Task<List<CompanyInfo>> GetAllCompanyInfo();
        Task<string> SaveCompany(CompanyInfo info);
    }
}
