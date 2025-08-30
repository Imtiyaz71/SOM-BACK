using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }
    [HttpGet("parentmenu")]
    [Authorize]  // Require login for menu fetching (optional)
    public async Task<IActionResult> GetParentMenu()
    {
        var menus = await _menuService.GetParentMenu();

        if (menus == null || menus.Count == 0)
            return NotFound("No menus found for this role.");

        return Ok(menus);
    }
    [HttpGet("childmenu")]
    [Authorize]  // Require login for menu fetching (optional)
    public async Task<IActionResult> GetChildMenus(string roleName)
    {
        var menus = await _menuService.GetMenusByRoleAsync(roleName);

        if (menus == null || menus.Count == 0)
            return NotFound("No menus found for this role.");

        return Ok(menus);
    }
}
