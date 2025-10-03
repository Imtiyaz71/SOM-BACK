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


                    if (k.Id == 0)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@TypeName", k.TypeName);
                        parameters.Add("@crid", k.crid);
                        parameters.Add("@Amount", k.Amount);

                        parameters.Add("@compId", k.compId);

                        int rows = await con.ExecuteAsync(
                            "sp_saveloanType",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        if (rows > 0)
                        {
                            result = "Type Added";
                        }
                    }
                    else
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@id", k.Id);
                        parameters.Add("@TypeName", k.TypeName);
                        parameters.Add("@crid", k.crid);
                        parameters.Add("@Amount", k.Amount);

                        int rows = await con.ExecuteAsync(
                            "sp_editloanType",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        if (rows > 0)
                        {
                            result = "Type Update";
                        }
                    }
                }
                catch (Exception ex)
                {

                    result = "error: " + ex.Message;
                }
            }

            return result;
        }
    }
}
