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

        public async Task<List<VW_Journal>> GetJournal(int compId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var result = await connection.QueryAsync<VW_Journal>(
                    "sp_GetJournalSummary",               // Stored Procedure name
                    new { compId = compId },              // Parameter
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                // Optional: log exception
                Console.WriteLine($"Error fetching journal: {ex.Message}");
                return new List<VW_Journal>();
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

        public async Task<List<VM_kistiandSubs>> GetKistiReceiveById(int compId,int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_kistiandSubs>(
                "sp_GetkistireceiveById",
                new { compId = compId,id=id },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_MemberBalance>> GetMemberBalance(int compId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompId", compId, DbType.Int32);

                var result = await db.QueryAsync<VW_MemberBalance>(
                    "sp_GetMemberBalances",   // stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }

        public async Task<List<VW_MemberProjectAccount>> GetProjectAccountByMemberAndProject(int? compId, int? memNo, int? projectId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@compId", compId, DbType.Int32);
                parameters.Add("@memno", memNo, DbType.Int32);
                parameters.Add("@projectid", projectId, DbType.Int32);

                var result = await conn.QueryAsync<VW_MemberProjectAccount>(
                    "sp_getmemberprojectaccount",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }

        public async Task<List<VW_ProjectAccountSummary>> GetProjectAccountSummary(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@compId", compId);

                var result = await connection.QueryAsync<VW_ProjectAccountSummary>(
                    "sp_ProjectBalanceSummary",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
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

        public async Task<List<VW_RegularSubscription>> GetRegularSubscriptionReceiveById(int compId, int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_RegularSubscription>(
                "sp_getregularsubsreceiveById",
                new { compId = compId,id=id },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_MonthlyExpense>> GetRevenue(int compId,int? year=null)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_MonthlyExpense>(
                "SP_GetMonthlyRevenue_ByCompId",
                new { compId = compId,year=year },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_RevenueSummary>> GetRevenueSummary(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Open connection
                    await connection.OpenAsync();

                    // Execute stored procedure
                    var result = await connection.QueryAsync<VW_RevenueSummary>(
                        "sp_GetRevenueSummary",                         // Stored Procedure Name
                        new { compId = compId },                         // Input parameter
                        commandType: CommandType.StoredProcedure);

                    // Return result as list
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    // Log or handle as needed
                    throw new Exception("Error fetching revenue summary: " + ex.Message, ex);
                }
            }
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

        public async Task<List<VM_kistiandSubs>> GetSubscriptionReceiveById(int compId, int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VM_kistiandSubs>(
                "sp_GetsubscriptionreceiveById",
                new { compId = compId,id=id },
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
                    // 🧠 Basic validation
                    if (model == null)
                        return "Error: Model cannot be null.";

                    if (model.Amount <= 0)
                        return "Error: Amount must be greater than zero.";

                    var parameters = new DynamicParameters();

                    parameters.Add("@compId", model.compId);
                    parameters.Add("@amount", model.Amount);
                    parameters.Add("@vendor", model.Vendor);
                    parameters.Add("@descri", model.Descri ?? string.Empty);

                    string spName;

                    if (model.Id == 0)
                    {
                        // New insert
                        spName = "sp_addBalancesegment";
                    }
                    else
                    {
                        // Update existing record
                        parameters.Add("@id", model.Id);
                        spName = "sp_EditBalancesegment";
                    }

                    // ⚡ Execute SP and get SQL message
                    var message = await connection.QueryFirstOrDefaultAsync<string>(
                        spName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // যদি stored procedure message না ফেরায়, fallback message দেই
                    if (string.IsNullOrEmpty(message))
                    {
                        message = model.Id == 0
                            ? "✅ Balance segment added successfully."
                            : "✅ Balance segment updated successfully.";
                    }

                    return message;
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

        public async Task<VW_Response> SaveRepay(RePay model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@compId", model.CompId);
                    parameters.Add("@memNo", model.MemNo);
                    parameters.Add("@projectid", model.ProjectId);
                    parameters.Add("@payble", model.Payble);
                    parameters.Add("@paid", model.Paid);
                    parameters.Add("@withdrwalID", model.WithdrwalID);

                    // SP call
                    var result = await con.QueryFirstOrDefaultAsync<VW_Response>(
                        "sp_repay",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Null handling
                    if (result == null)
                    {
                        return new VW_Response
                        {
                            StatusCode = 0,
                            Message = "No response from database."
                        };
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return new VW_Response
                {
                    StatusCode = 0,
                    Message = "Error: " + ex.Message
                };
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
