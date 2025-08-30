using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
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
    public async Task<List<Menu>> GetParentMenu()
    {
        using var connection = new SqlConnection(_connectionString);

        var menus = await connection.QueryAsync<Menu>(
            "sp_menus",                 // Stored procedure name
            commandType: CommandType.StoredProcedure
        );

        return menus.AsList();
    }
}
