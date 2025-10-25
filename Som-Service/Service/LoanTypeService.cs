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


        public async Task<List<VW_BorrowerLoanInfo>> GetBorrowerLoanInfo(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<VW_BorrowerLoanInfo>(
                    "sp_GetBorrowerWithLoan",
                    new { CompId = compId },
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList(); // convert IEnumerable to List
            }
        }

        public async Task<List<VW_BorrowerLoanInfo>> GetBorrowerLoanInfoByBrwId(int compId, int brwId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<VW_BorrowerLoanInfo>(
                    "sp_GetBorrowerWithLoanByBrwId",
                    new { CompId = compId, brwId= brwId },
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList(); // convert IEnumerable to List
            }
        }

        public async Task<List<VW_LoanSensionViewModel>> GetLoanSensionDetails(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Stored Procedure call
                var result = await connection.QueryAsync<VW_LoanSensionViewModel>(
                    "sp_GetLoanSensionByCompId",
                    new { CompId = compId },
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
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

        public async Task<List<VW_LoanPaidHistory>> LoanPaidHistory(int compId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<VW_LoanPaidHistory>(
                    "sp_GetLoanPaidHistory",
                    new { CompId = compId },
                    commandType: CommandType.StoredProcedure
                );

                return result.AsList();
            }
        }

        public async Task<List<VW_LoanPaidHistory>> LoanPaidHistoryByLoanId(int compId, int loanid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<VW_LoanPaidHistory>(
                    "sp_GetLoanPaidHistoryByLoanId",
                    new { CompId = compId,loanid=loanid },
                    commandType: CommandType.StoredProcedure
                );

                return result.AsList();
            }
        }

        public async Task<VW_Response> SaveLoanPaid(LoanPaidHistory model)
        {
            VW_Response response = new VW_Response();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@compId", model.CompId, DbType.Int32);
                    parameters.Add("@loanId", model.LoanId, DbType.Int32);
                    parameters.Add("@paybleAmount", model.Payble, DbType.Decimal);
                    parameters.Add("@paidAmount", model.PaidAmount, DbType.Decimal);
                    parameters.Add("@principle", model.Principle, DbType.Decimal);
                    parameters.Add("@interest", model.Interest, DbType.Decimal);
                    parameters.Add("@pDate", model.PDate, DbType.String);
                    parameters.Add("@pMonth", model.PMonth, DbType.String);
                    parameters.Add("@pYear", model.PYear, DbType.Int32);
                    parameters.Add("@pBy", model.pBy, DbType.String);

                    await connection.ExecuteAsync(
                        "sp_LoanPaid",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.StatusCode = 200;
                    response.Message = "Loan payment saved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<VW_Response> SaveLoanSension(VW_LoanSensionRequest model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // -----------------------------
                // 1️⃣ Handle Photo (Base64 → file)
                // -----------------------------
                string photoDbPath = null;

                if (!string.IsNullOrEmpty(model.Photo))
                {
                    try
                    {
                        // Directory setup
                
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        // Base64 cleanup
                        var base64Data = model.Photo.Contains(",") ? model.Photo.Split(',').Last() : model.Photo;
                        byte[] photoBytes = Convert.FromBase64String(base64Data);

                        // Unique file name
                        var ext = ".jpg"; // default
                        if (model.Photo.Contains("image/png")) ext = ".png";
                        else if (model.Photo.Contains("image/jpeg")) ext = ".jpg";

                        var fileName = $"photo_{DateTime.Now:yyyyMMddHHmmssfff}_{new Random().Next(1000, 9999)}{ext}";
                        var fullPath = Path.Combine(uploadDir, fileName);

                        // Save file
                        await File.WriteAllBytesAsync(fullPath, photoBytes);

                        // Relative path DB te rakhbo
                        photoDbPath = Path.Combine("Uploads", fileName).Replace("\\", "/");
                    }
                    catch (Exception ex)
                    {
                        return new VW_Response
                        {
                            StatusCode = 0,
                            Message = "Photo processing failed: " + ex.Message
                        };
                    }
                }

                // -----------------------------
                // 2️⃣ SP Parameters
                // -----------------------------
                var parameters = new DynamicParameters();
                parameters.Add("@compId", model.CompId);
                parameters.Add("@fullName", model.FullName);
                parameters.Add("@phone", model.Phone);
                parameters.Add("@email", model.Email);
                parameters.Add("@bAddress", model.BAddress);
                parameters.Add("@nId", model.NId);
                parameters.Add("@dOB", model.DOB);
                parameters.Add("@father", model.Father);
                parameters.Add("@mother", model.Mother);
                parameters.Add("@photo", photoDbPath); // relative path DB te
                parameters.Add("@loanType", model.LoanType);
                parameters.Add("@Amount", model.Amount);
                parameters.Add("@sDate", model.SDate);
                parameters.Add("@sMonth", model.SMonth);
                parameters.Add("@sYear", model.SYear);
                parameters.Add("@sBy", model.SBy);

                // SQL Messages capture
                string sqlMsg = null;
                connection.InfoMessage += (sender, e) => { sqlMsg = e.Message; };

                // Call SP
                var newBrwId = await connection.QuerySingleAsync<int>(
                    "sp_LoanSension",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return new VW_Response
                {
                    StatusCode = 1,
                    Message = sqlMsg ?? $"Loan sension successful. New Borrower ID: {newBrwId}"
                };
            }
            catch (SqlException ex)
            {
                return new VW_Response
                {
                    StatusCode = 0,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new VW_Response
                {
                    StatusCode = 0,
                    Message = $"Error: {ex.Message}"
                };
            }
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
                        parameters.Add("@ActivityPeriod", k.ActivityPeriod);
                        parameters.Add("@DelayInterest", k.DelayInterest);
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
                        parameters.Add("@ActivityPeriod", k.ActivityPeriod);
                        parameters.Add("@DelayInterest", k.DelayInterest);

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
