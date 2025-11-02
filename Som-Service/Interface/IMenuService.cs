using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IMenuService
    {
        Task<List<ParentMenu>> GetModule();
        Task<List<ParentMenu>> GetParentMenu(int compId);
        Task<List<ChildMenu>> GetMenusByParent(int parentid);
        Task<List<ChildMenu>> GetMenusByRoleAsync2(int compId,int parentId,string roleName);
        public Task<VW_Response> SaveCompanyModule(CompanyModule model);
        public Task<VW_Response> SaveComapnyMenuEligiblity(EligMenu model);
        public Task<VW_Response> SaveCompanyMenuEligibilityMultiple(int compId, int roleId, List<int> menuIds);
    }
}
