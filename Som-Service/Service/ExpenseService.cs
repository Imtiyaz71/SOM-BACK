using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Som_Service.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly string _connectionString;

        public ExpenseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> AddExpenseType(ExpenseType model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", model.Id);
                    parameters.Add("@compId", model.compId);
                    parameters.Add("@typename", model.TypeName);
            
                    await connection.ExecuteAsync(
                        "sp_addExpenseType",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "Expense Type Save and Change.";
                }
                catch (Exception ex)
                {
                    // handle or log exception properly
                    return $"Error: {ex.Message}";
                }
            }
        }

        public async Task<string> DeleteExpense(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
       
                    await connection.ExecuteAsync(
                        "sp_deleteExpenseType",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "Expense Type Deleted.";
                }
                catch (Exception ex)
                {
                    // handle or log exception properly
                    return $"Error: {ex.Message}";
                }
            }
        }

        public async Task<List<ExpenseType>> GetExpenseType(int compId)
        {

            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<ExpenseType>(
                "sp_getExpenseTypeList",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }
    }
}
