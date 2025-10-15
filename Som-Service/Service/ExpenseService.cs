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

        public async Task<string> AddExpense(Expense model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var parameters = new
                    {
                        extype = model.exType,
                        compId = model.compId,
                        amount = model.amount,
                        descri = model.Descri,
                        eDate = model.eDate,
                        eMonth = model.eMonth,
                        eBy = model.eBy,
                        eyear = model.eYear
                    };

                    await connection.ExecuteAsync("sp_addexpense", parameters, commandType: CommandType.StoredProcedure);

                    return "Expense saved successfully";
                }
            }
            catch (Exception ex)
            {
                // Exception handle kore meaningful message return korlam
                return $"Failed to save expense: {ex.Message}";
            }
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

        public async Task<string> DeleteExpenseType(int id)
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

        public async Task<List<VW_Expense>> GetExpense(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_Expense>(
                "sp_getexpense",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
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

        public async Task<List<VW_MonthlyExpense>> GetMonthlyExpense(int compId, int year)
        {
            using (var conn = new SqlConnection(_connectionString)) // _connectionString tomader DB connection
            {
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@compId", compId, DbType.Int32);
                parameters.Add("@year", year, DbType.Int32); // optional, null will return all years

                var result = await conn.QueryAsync<VW_MonthlyExpense>(
                    "sp_getMonthlyExpense",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
    }
}
