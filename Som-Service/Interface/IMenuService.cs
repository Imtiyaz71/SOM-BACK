using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IMenuService
    {
        Task<List<ParentMenu>> GetParentMenu(int compId);
        Task<List<Menu>> GetMenusByRoleAsync(string roleName);
        Task<List<ChildMenu>> GetMenusByRoleAsync2(int compId,int parentId,string roleName);
        public Task<VW_Response> SaveCompanyModule(CompanyModule model);
        public Task<VW_Response> SaveComapnyMenuEligiblity(EligMenu model);
    }
}
