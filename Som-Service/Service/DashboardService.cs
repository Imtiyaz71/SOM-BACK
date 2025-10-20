using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<VW_DashboardCount> GetDashboardCounts(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "sp_GetDashboardCounts";

                var result = await connection.QueryFirstOrDefaultAsync<VW_DashboardCount>(
                    query,
                    new { CompId = compId },
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new VW_DashboardCount();
            }
        }

    }
}
