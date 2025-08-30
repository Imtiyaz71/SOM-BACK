using Som_Models.Models;

namespace Som_Service.Interface
{
    public interface IMenuService
    {
        Task<List<Menu>> GetParentMenu();
        Task<List<Menu>> GetMenusByRoleAsync(string roleName);
    }
}
