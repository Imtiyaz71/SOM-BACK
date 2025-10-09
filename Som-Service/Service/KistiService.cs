using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class KistiService : IKistiService
    {
        private readonly string _connectionString;

        public KistiService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Cr>> GetCrData()
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<Cr>(
                "sp_cr",
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VM__KistiTypes>> GetKistiTypes(int compId)
        {
            List<VM__KistiTypes> result = new List<VM__KistiTypes>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var cr = await connection.QueryAsync<VM__KistiTypes>(
                    "sp_kistitypes",
                    new { compId = compId },
                    commandType: CommandType.StoredProcedure
                );

                result = cr.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // optionally log here or rethrow
            }

            return result;
        }


        public async Task<VM__KistiTypes> GetKistiTypesById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryFirstOrDefaultAsync<VM__KistiTypes>(
                "sp_kistitypesById",
                new { id = id },
                commandType: CommandType.StoredProcedure
            );

            return mem;
        }

        public async Task<string> SaveKistiType(KistiTypes k)
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
                        parameters.Add("@projectid", k.projectid);
                        int rows = await con.ExecuteAsync(
                            "sp_savekistiType",
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
                        parameters.Add("@projectid", k.projectid);
                        int rows = await con.ExecuteAsync(
                            "sp_editkistiType",
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
