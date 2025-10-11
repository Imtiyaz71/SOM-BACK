using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly string _connectionString;

        public SubscriptionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
 

        public async Task<List<VM_SubscriptionTypes>> GetSubscriptionTypes(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_SubscriptionTypes>(
                "sp_subscriptiontypes",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<VM_SubscriptionTypes> GetSubscriptionTypesById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var mem = await connection.QueryFirstOrDefaultAsync<VM_SubscriptionTypes>(
                "sp_subscriptiontypesById",
                new { id = id },
                commandType: CommandType.StoredProcedure
            );

            return mem;
        }

        public async Task<List<VM_SubscriptionTypes>> GetSubscriptionTypesByProject(int compId, int projectid)
        {
            List<VM_SubscriptionTypes> result = new List<VM_SubscriptionTypes>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var cr = await connection.QueryAsync<VM_SubscriptionTypes>(
                    "sp_kistitypesByProjectId",
                    new { compId = compId, projectid = projectid },
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

        public async Task<string> SaveSubscriptionType(SubscriptionTypes k)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    string storedProcedure = "";
                    string successMessage = "";

                    parameters.Add("@TypeName", k.TypeName);
                    parameters.Add("@crid", k.crid);
                    parameters.Add("@Amount", k.Amount);
                    parameters.Add("@projectid", k.projectId);

                
                    if (k.Id == 0)
                    {
                        parameters.Add("@compId", k.compId);
                        storedProcedure = "sp_savesubscriptionType";
                        successMessage = "Type Added";
                    }
                    else
                    {
                        parameters.Add("@id", k.Id);
                        storedProcedure = "sp_editsubscriptionType";
                        successMessage = "Type Updated";
                    }

                    int rows = await con.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    return rows > 0 ? successMessage : "No changes made.";
                }
                catch (Exception ex)
                {
                    return "error: " + ex.Message;
                }
            }
        }

    }
}
