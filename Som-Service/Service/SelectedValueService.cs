using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class SelectedValueService : ISelectedValueService
    {
        private readonly string _connectionString;

        public SelectedValueService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Authorizer>> GetSelectedAuthorizer()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var user = await connection.QueryAsync<Authorizer>(
                "sp_Authorizer",
                commandType: CommandType.StoredProcedure
            );

            return user.ToList();
        }


        public async Task<List<Designation>> GetSelectedDesignation()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var user = await connection.QueryAsync<Designation>(
                "sp_designation",
                commandType: CommandType.StoredProcedure
            );

            return user.ToList();
        }
    }
}
