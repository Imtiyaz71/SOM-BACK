using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;


namespace Som_Service.Service
{
    public class AccountService : IAccountService
    {
        private readonly string _connectionString;

        public AccountService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<VM_kistiandSubs>> GetKistiReceive(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_kistiandSubs>(
                "sp_Getkistireceive",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VM_kistiandSubs>> GetSubscriptionReceive(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_kistiandSubs>(
                "sp_Getsubscriptionreceive",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<string> SaveKistiAmount(VM_SaveKistiandSubs model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@projectid", model.projectid);
                    parameters.Add("@typeid", model.typeid);
                    parameters.Add("@compId", model.compId);
                    parameters.Add("@memNo", model.memNo);
                    //parameters.Add("@crid", model.crid);
                    parameters.Add("@paybleamount", model.paybleamount);
                    parameters.Add("@recamount", model.recamount);
                    parameters.Add("@remark", model.remark);
                    parameters.Add("@recdate", model.recdate);
                    parameters.Add("@recmonth", model.recmonth);
                    parameters.Add("@recyear", model.recyear);
                    parameters.Add("@trnasBy", model.transby);

                    await connection.ExecuteAsync(
                        "sp_SavekistiReceive",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "Subscription receive saved successfully.";
                }
                catch (Exception ex)
                {
                    // handle or log exception properly
                    return $"Error: {ex.Message}";
                }
            }
        }

        public async Task<string> SavesubscriptionAmount(VM_SaveKistiandSubs model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@typeid", model.typeid);
                    parameters.Add("@compId", model.compId);
                    parameters.Add("@memNo", model.memNo);
                    //parameters.Add("@crid", model.crid);
                    parameters.Add("@paybleamount", model.paybleamount);
                    parameters.Add("@recamount", model.recamount);
                    parameters.Add("@remark", model.remark);
                    parameters.Add("@recdate", model.recdate);
                    parameters.Add("@recmonth", model.recmonth);
                    parameters.Add("@recyear", model.recyear);
                    parameters.Add("@trnasBy", model.transby);

                    await connection.ExecuteAsync(
                        "sp_SaveSubscriptionReceive",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "Subscription receive saved successfully.";
                }
                catch (Exception ex)
                {
                    // handle or log exception properly
                    return $"Error: {ex.Message}";
                }
            }
        }
    }
}
