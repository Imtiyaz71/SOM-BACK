using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

public class MenuService : IMenuService
{
    private readonly string _connectionString;

    public MenuService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Menu>> GetMenusByRoleAsync(string roleName)
    {
        using var connection = new SqlConnection(_connectionString);

        var menus = await connection.QueryAsync<Menu>(
            "sp_childmenu",                     // Stored procedure name
            new { role = roleName },            // Parameter object (name must match SP param)
            commandType: CommandType.StoredProcedure
        );

        return menus.AsList();
    }

    public async Task<List<ChildMenu>> GetMenusByRoleAsync2(int compId, int parentId, string roleName)
    {
        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@compId", compId);
                parameters.Add("@parentId", parentId);
                parameters.Add("@designation", roleName);

                var menus = await conn.QueryAsync<ChildMenu>(
                    "sp_childmenu2",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return menus.ToList();
            }
        }
        catch (Exception ex)
        {
            // optionally log ex.Message
            return new List<ChildMenu>(); // return empty list on error
        }
    }

    public async Task<List<ParentMenu>> GetParentMenu(int compId)
    {
        using var connection = new SqlConnection(_connectionString);

        var menus = await connection.QueryAsync<ParentMenu>(
            "sp_menus",                     // Stored procedure name
            new { compId = compId },            // Parameter object (name must match SP param)
            commandType: CommandType.StoredProcedure
        );

        return menus.AsList();
    }

    public async Task<VW_Response> SaveComapnyMenuEligiblity(EligMenu model)
    {
        VW_Response res = new VW_Response();

        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@compId", model.CompId);
                parameters.Add("@roleId", model.RoleId);
                parameters.Add("@menuId", model.MenuId);

                var insertedId = await conn.QuerySingleAsync<int>(
                    "sp_InsertEligMenu",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                res.StatusCode = 200;
                res.Message = "Eligibility saved successfully. New ID: " + insertedId;
            }
        }
        catch (Exception ex)
        {
            res.StatusCode = 500;
            res.Message = "Error while saving eligibility: " + ex.Message;
        }

        return res;
    }


    public async Task<VW_Response> SaveCompanyModule(CompanyModule model)
    {
        VW_Response res = new VW_Response();

        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@compId", model.CompId);
                param.Add("@ParentMenuId", model.ParentMenuId);

                var newId = await conn.QuerySingleAsync<int>(
                    "sp_InsertCompanyModule",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                res.StatusCode = 200; // success
                res.Message = "Company module saved successfully. New ID: " + newId;
            }
        }
        catch (Exception ex)
        {
            res.StatusCode = 500; // server error
            res.Message = "Error while saving company module: " + ex.Message;
        }

        return res;
    }
}
