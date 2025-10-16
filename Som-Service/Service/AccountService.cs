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

        public async Task<string> AddBalanceWithdraw(BalanceWithdraw model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@compId", model.compId);
                parameters.Add("@memNo", model.memNo);
                parameters.Add("@fProject", model.fProject);
                parameters.Add("@amount", model.amount);
                parameters.Add("@remarks", model.remarks);
                parameters.Add("@wdate", model.wDate);
                parameters.Add("@wMonth", model.wMonth);
                parameters.Add("@wYear", model.wYear);
                parameters.Add("@wBy", model.wBy);

                try
                {
                    await connection.ExecuteAsync("sp_InsertBalanceWithdraw", parameters, commandType: CommandType.StoredProcedure);
                    return "Balance withdraw successful.";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        
        }

        public async Task<string> BounceBalanceWithdraw(VWBounceBalanceWithdrwal model)
        {
            if (model == null)
                return "Invalid input.";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", model.id, DbType.Int32);
                    parameters.Add("@fProject", model.fProject, DbType.Int32);
                    parameters.Add("@memNo", model.memNo, DbType.Int32);
                    parameters.Add("@compId", model.compId, DbType.Int32);

                    await conn.ExecuteAsync(
                        "sp_bounceBalanceWithdraw",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "Bounce successful";
                }
            }
            catch (Exception ex)
            {
                // log করতে পারো এখানে
                return "Error: " + ex.Message;
            }
        }

        public async Task<List<SomityAccounts>> GetAccountBalance(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<SomityAccounts>(
                "sp_GetAccountBalance",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_BalanceAddHistory>> GetBalanceAddHistory(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_BalanceAddHistory>(
                "sp_GetBalanceAddHistory",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_BalanceSegment>> GetBalanceSegment(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_BalanceSegment>(
                "sp_GetBalancesegment",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_BalanceSegment>> GetBalanceSegmentById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_BalanceSegment>(
                "sp_GetBalancesegmentById",
                new { id = id },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_BalanceWithdraw>> GetBalanceWithDraw(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@compId", compId);

                    var result = await connection.QueryAsync<VW_BalanceWithdraw>(
                        "sp_GetBalanceWithdrawList",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching balance withdraw list: " + ex.Message);
                }
            }
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

        public async Task<List<VW_RegularSubscription>> GetRegularSubscriptionReceive(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_RegularSubscription>(
                "sp_getregularsubsreceive",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_SomityAccTransection>> GetSomityAccTransection(VW_AccDrCr model)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startDate", model.startDate, DbType.Date);
                parameters.Add("@endDate", model.endDate, DbType.Date);
                parameters.Add("@compId", model.compId, DbType.Int32);
                parameters.Add("@crType", model.crType, DbType.Int32);

                var result = await conn.QueryAsync<VW_SomityAccTransection>(
                    "sp_getAccDrCr",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
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

        public async Task<List<Vendor>> GetVendor()
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<Vendor>(
                "sp_getvendor",
                
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<string> SaveAccountSegment(BalanceSegemnt model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Validate input
                    if (model == null)
                        return "Error: Model cannot be null.";

                    if (model.Amount <= 0)
                        return "Error: Amount must be greater than zero.";

                    var parameters = new DynamicParameters();

                    // Common parameters
                    parameters.Add("@compId", model.compId);  // Ensure this type matches DB
                    parameters.Add("@amount", model.Amount);
                    parameters.Add("@vendor", model.Vendor);
                    parameters.Add("@descri", model.Descri ?? string.Empty);
                    string spName;

                    if (model.Id == 0)
                    {
                     
                        // Insert new record
                 
                        spName = "sp_addBalancesegment";
                    }
                    else
                    {
                        // Update existing record
                        parameters.Add("@id", model.Id);
                        spName = "sp_EditBalancesegment";
                    }

                    // Execute stored procedure
                    await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);

                    return model.Id == 0 ? "✅ Balance segment added successfully." : "✅ Balance segment updated successfully.";
                }
                catch (SqlException sqlEx)
                {
                
                    return $"SQL Error: {sqlEx.Message}";
                }
                catch (Exception ex)
                {
                  
                    return $"Error: {ex.Message}";
                }
            }
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

        public async Task<string> SaveRegularSubs(VM_RegularSubs model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@compId", model.compId);
                    parameters.Add("@memNo", model.memNo);
                    //parameters.Add("@crid", model.crid);
                    parameters.Add("@paybleamount", model.paybleamount);
                    parameters.Add("@recamount", model.recamount);

                    parameters.Add("@recdate", model.recdate);
                    parameters.Add("@recmonth", model.recmonth);
                    parameters.Add("@recyear", model.recyear);
                    parameters.Add("@trnasBy", model.trnasBy);

                    await connection.ExecuteAsync(
                        "sp_SaveRegularSubsReceive",
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
                    parameters.Add("@projectid", model.projectid);
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
