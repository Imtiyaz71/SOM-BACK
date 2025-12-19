using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class RevenueService : IRevenueService
    {
        private readonly string _connectionString;

        public RevenueService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<RevenueDisburse>> RevenueList(int compId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@compId", compId);

                var result = await con.QueryAsync<RevenueDisburse>(
                    "sp_revDisburseList",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }


        public async Task<VW_Response> SaveDisburseRevenue(RevenueDisburse disburse)
        {
            var response = new VW_Response();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@compId", disburse.compId);
                    parameters.Add("@disamount", disburse.DisRev);
                    parameters.Add("@createby", disburse.CreateBy);

                    await con.ExecuteAsync(
                        "sp_revDisburse",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.StatusCode = 200;
                    response.Message = "Revenue disburse successfully completed.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error: " + ex.Message;
            }

            return response;
        }

        public async Task<decimal> TotalRevenue(int compId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = $"SELECT ISNULL(SUM(Amount),0) FROM RevenueAccount WHERE compId = {compId}";

                    // Amount jodi multiple row thake, SUM use kora holo
                    decimal total = await connection.ExecuteScalarAsync<decimal>(query);

                    return total;
                }
            }
            catch (Exception ex)
            {
                // Logging korte chaile ekhane korte paro
                throw new Exception("Error fetching total revenue: " + ex.Message);
            }
        }

    }
}
