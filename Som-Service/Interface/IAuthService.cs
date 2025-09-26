using Som_Models.Models;
namespace Som_Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(Login model);
        Task<CompanyInfo> CompanyInfo();
        Task<string> SaveCompany(CompanyInfo info);
    }
}
