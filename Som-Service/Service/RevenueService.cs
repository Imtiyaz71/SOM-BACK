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
