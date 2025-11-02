using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using Som_Service.Service;

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
    public async Task<IActionResult> GetParentMenu(int compId)
    {
        var menus = await _menuService.GetParentMenu(compId);

        if (menus == null || menus.Count == 0)
            return NotFound("No menus found for this Company.");

        return Ok(menus);
    }
    [HttpGet("childmenu")]
    [Authorize]  // Require login for menu fetching (optional)
    public async Task<IActionResult> GetChildMenus(int parentid)
    {
        var menus = await _menuService.GetMenusByParent(parentid);

        if (menus == null || menus.Count == 0)
            return NotFound("No menus found for this role.");

        return Ok(menus);
    }
    [HttpPost("save-module")]
    [Authorize]
    public async Task<IActionResult> SaveModule([FromBody] CompanyModule model)
    {
        if (model == null)
        {
            return BadRequest(new VW_Response
            {
                StatusCode = 400,
                Message = "Invalid Request!"
            });
        }

        var result = await _menuService.SaveCompanyModule(model);

        // Map SP result to proper HTTP response
        return result.StatusCode switch
        {
            200 => Ok(result),         // Inserted successfully
            409 => Conflict(result),   // Duplicate
            _ => StatusCode(500, result) // Server error
        };
    }

    [HttpPost("save-menu-eligibility")]
    [Authorize]
    public async Task<IActionResult> SaveEligibility([FromBody] EligMenu model)
    {
        if (model == null)
        {
            return BadRequest(new VW_Response
            {
                StatusCode = 400,
                Message = "Invalid Request!"
            });
        }

        var result = await _menuService.SaveComapnyMenuEligiblity(model);

        return StatusCode(result.StatusCode, result);
    }
    [HttpPost("save-menu-eligibility-request")]
    [Authorize]
    public async Task<IActionResult> SaveEligibility([FromBody] SaveMenuEligibilityRequest model)
    {
        if (model == null || model.MenuIds == null || model.MenuIds.Count == 0)
        {
            return BadRequest(new VW_Response
            {
                StatusCode = 400,
                Message = "Invalid Request! No menu selected."
            });
        }

        var result = await _menuService.SaveCompanyMenuEligibilityMultiple(
            model.CompId,
            model.RoleId,
            model.MenuIds
        );

        return StatusCode(result.StatusCode, result);
    }

[HttpGet("get-child-menus-byrole")]
    [Authorize]
    public async Task<IActionResult> GetChildMenusByRole(int compId, int parentId, string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            return BadRequest("Role name is required.");
        }

        var menus = await _menuService.GetMenusByRoleAsync2(compId, parentId, roleName);

        if (menus == null || !menus.Any())
        {
            return NotFound("No menus found for this role.");
        }

        return Ok(menus);
    }
    [Authorize]
    [HttpGet("modules")]
    public async Task<IActionResult> GetModule()
    {
        var info = await _menuService.GetModule();
        if (info == null)
            return Unauthorized("Invalid credentials");

        return Ok(new { info });
    }
}
