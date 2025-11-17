using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class SavingService : ISavingService
    {
        private readonly string _connectionString;

        public SavingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<VM_SavingAccount>> GetSavingsAccountList(int compId)
        {
            List<VM_SavingAccount> list = new List<VM_SavingAccount>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompId", compId);

                    list = (await conn.QueryAsync<VM_SavingAccount>(
                        "sp_GetSavingsAccounts",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )).ToList();
                }
            }
            catch (Exception ex)
            {
                // Optional: Log exception
                return new List<VM_SavingAccount>();
            }

            return list;
        }


        public async Task<VW_Response> SaveSavingAccount(SavingsAccount model)
        {
            var response = new VW_Response();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@Id", model.Id);
                    parameters.Add("@CompId", model.CompId);
                    parameters.Add("@AccountName", model.AccountName);
                    parameters.Add("@Organization", model.Organization);
                    parameters.Add("@AccountNo", model.AccountNo);
                    parameters.Add("@Branch", model.Branch);
                    parameters.Add("@CreateDate", model.CreateDate);
                    parameters.Add("@CreateBy", model.CreateBy);

                    // OUTPUT Parameters
                    parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                    parameters.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("sp_SaveSavingsAccount", parameters, commandType: CommandType.StoredProcedure);

                    // Reading Output Values
                    response.StatusCode = parameters.Get<int>("@Status");
                    response.Message = parameters.Get<string>("@Message")+" "+ parameters.Get<int>("@ReturnId");
                   
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
             
            }

            return response;
        }

    }
}
