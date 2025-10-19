using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class LoanTypeService : ILoanTypes
    {
        private readonly string _connectionString;

        public LoanTypeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<VM_LoanTypes> GetLoanTypeById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryFirstOrDefaultAsync<VM_LoanTypes>(
                "sp_loantypesById",
                new { id = id },
                commandType: CommandType.StoredProcedure
            );

            return mem;
        }

        public async Task<List<VM_LoanTypes>> GetLoanTypes(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_LoanTypes>(
                "sp_loantypes",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<string> SaveLoanType(LoanTypes k)
        {
            string result = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();

                    if (k.Id == 0)
                    {
                        // Insert
                        parameters.Add("@CompId", k.CompId);
                        parameters.Add("@TypeName", k.TypeName);
                        parameters.Add("@Interest", k.Interest);
                        parameters.Add("@TimePeriodMonths", k.TimePeriodMonths);
                        parameters.Add("@UpdateBy", k.UpdateBy);

                        await con.ExecuteAsync(
                            "sp_SaveLoanType",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        result = "Type Added"; // rows ignore
                    }
                    else
                    {
                        // Update
                        parameters.Add("@Id", k.Id);
                        parameters.Add("@CompId", k.CompId);
                        parameters.Add("@TypeName", k.TypeName);
                        parameters.Add("@Interest", k.Interest);
                        parameters.Add("@TimePeriodMonths", k.TimePeriodMonths);
                        parameters.Add("@UpdateBy", k.UpdateBy);

                        await con.ExecuteAsync(
                            "sp_EditLoanType",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        result = "Type Updated"; // rows ignore
                    }
                }
                catch (Exception ex)
                {
                    result = "Error: " + ex.Message;
                }
            }

            return result;
        }

    }
}
