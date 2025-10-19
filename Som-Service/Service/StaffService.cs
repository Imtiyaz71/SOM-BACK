using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Som_Service.Service
{
    public class StaffService : IStaffService
    {
        private readonly string _connectionString;
        public StaffService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<VW_Response> DeactiveStaff(int id)
        {
            var response = new VW_Response();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    // Stored procedure call
                    var result = await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_deactivestaff",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.StatusCode = 200; // ✅ success
                    response.Message = result ?? "Staff successfully deactivated and archived.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500; // ❌ error
                response.Message = "Error: " + ex.Message;
            }

            return response;
        }


        public async Task<VW_Response> DeleteStaffDesignation(int Id, int compId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                parameters.Add("@compId", compId);

                // SP returns a SELECT message
                var message = await connection.QueryFirstOrDefaultAsync<string>(
                    "sp_DeleteStaffDesignation",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var response = new VW_Response
                {
                    StatusCode = message.Contains("deleted successfully") ? 200 :
                                 message.Contains("not found") ? 404 : 500,
                    Message = message
                };

                return response;
            }
            catch (Exception ex)
            {
                return new VW_Response
                {
                    StatusCode = 500,
                    Message = $"Error occurred: {ex.Message}"
                };
            }
        }

        public async Task<List<VW_ArchiveStaff>> GetArchiveStaff(int compId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@compId", compId, DbType.Int32);

            var result = await connection.QueryAsync<VW_ArchiveStaff>(
                "sp_getArchiveStaffByComp",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.AsList();
        }

        public async Task<List<StaffDesignation>> GetStaffDesignation(int compId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@compId", compId);

                var result = await connection.QueryAsync<StaffDesignation>(
                    "sp_GetStaffDesignations",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch
            {
                return new List<StaffDesignation>();
            }
        }

        public async Task<List<VW_Staff>> GetStaffInfo(int compId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString)) // replace with your connection string
                {
                    await conn.OpenAsync();

                    var staffList = await conn.QueryAsync<VW_Staff>(
                        "sp_GetStaff",
                        new { CompId = compId },
                        commandType: CommandType.StoredProcedure
                    );

                    return staffList.AsList(); // convert IEnumerable to List
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error
                throw new Exception("An error occurred while fetching staff info: " + ex.Message);
            }
        }

        public async Task<VW_Response> SaveStaffDesignation(StaffDesignation model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                string sp = "";
                var parameters = new DynamicParameters();
                parameters.Add("@compId", model.CompId);
                parameters.Add("@Designation", model.Designation);
                if(model.Id==0)
                {
                    sp = "sp_InsertStaffDesignation";
                }
                else
                {
                    sp = "sp_EditStaffDesignation";
                    parameters.Add("@Id", model.Id);
                }

                var message = await connection.QueryFirstOrDefaultAsync<string>(
                   sp,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return new VW_Response
                {
                    StatusCode = message.Contains("successfully") ? 200 : 400,
                    Message = message
                };
            }
            catch (Exception ex)
            {
                return new VW_Response
                {
                    StatusCode = 500,
                    Message = "Exception: " + ex.Message
                };
            }
        }

        public async Task<VW_Response> SaveStaffInfo(Staff model)
        {
            var response = new VW_Response();

            try
            {
                // Handle photo if Base64 is provided
                if (!string.IsNullOrEmpty(model.Photo))
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Generate unique file name
                    var fileName = $"staff_{Guid.NewGuid()}.jpg";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Convert Base64 to byte[]
                    var imageBytes = Convert.FromBase64String(model.Photo);
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    // Save relative path to DB (optional: "Uploads/file.jpg")
                    model.Photo = Path.Combine("Uploads", fileName).Replace("\\", "/");
                }

                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", model.Id);
                    parameters.Add("@CompId", model.CompId);
                    parameters.Add("@FullName", model.FullName);
                    parameters.Add("@NId", model.NId);
                    parameters.Add("@FullAddress", model.FullAddress);
                    parameters.Add("@StaffType", model.StaffType);
                    parameters.Add("@Photo", model.Photo);  // path, not base64
                    parameters.Add("@CreateBy", model.CreateBy);
                    parameters.Add("@Phone", model.phone);
                    parameters.Add("@Email", model.email);

                    var staffId = await conn.ExecuteScalarAsync<int>(
                        "sp_SaveStaff",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.StatusCode = 200;
                    response.Message = model.Id == 0
                        ? $"Staff inserted successfully with Id {staffId}."
                        : $"Staff updated successfully with Id {staffId}.";
                }
            }
            catch (SqlException ex)
            {
                response.StatusCode = 400;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred: " + ex.Message;
            }

            return response;
        }
    }
}
