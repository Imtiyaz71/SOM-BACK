using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
namespace Som_Service.Service
{
    public class AdvisoryService : IAdvisoryService
    {
        private readonly string _connectionString;

        public AdvisoryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<VW_Response> AddAdvisory(Advisory model)
        {
            var response = new VW_Response();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompId", model.CompId);
                    parameters.Add("@MemNo", model.MemNo);
                    parameters.Add("@AdRole", model.AdRole);
                    parameters.Add("@Validity", model.Validity);
                    parameters.Add("@CStatus", model.CStatus);

                    var newId = await conn.ExecuteScalarAsync<int>(
                        "sp_InsertAdvisory",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.StatusCode = 200;
                    response.Message = "Advisory entry added successfully.";
                }
            }
            catch (SqlException ex)
            {
                // Duplicate entry RAISERROR handling
                if (ex.Number == 50000) // RAISERROR custom error
                {
                    response.StatusCode = 409; // Conflict
                    response.Message = ex.Message;
                }
                else
                {
                    response.StatusCode = 500;
                    response.Message = $"SQL Error: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }

        public async Task<VW_Response> AddAdvisoryRole(AdvisoryRole model)
        {
            var response = new VW_Response();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompId", model.CompId);
                    parameters.Add("@Roles", model.Roles);

                    await conn.ExecuteAsync("sp_InsertAdvisoryRole", parameters, commandType: CommandType.StoredProcedure);

                    response.StatusCode = 200;
                    response.Message = "Advisory role inserted successfully.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }
        public async Task<List<VW_AdvisoryList>> GetAdvisoryList(int compId, int cStatus)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompId", compId);
                parameters.Add("@CStatus", cStatus);

                var result = await conn.QueryAsync<VW_AdvisoryList>(
                    "sp_GetAdvisoryList",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.AsList();
            }
        }
        public async Task<VW_Response> DeleteAdvisoryRole(int CompId, int id)
        {
            var response = new VW_Response();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompId", CompId);
                    parameters.Add("@Id", id);

                    int rowsAffected = await conn.ExecuteAsync(
                        "sp_DeleteAdvisoryRole",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.Message = "Advisory role deleted successfully.";
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.Message = "No matching advisory role found.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }
        // 🔹 Get Advisory Roles by Company ID
        public async Task<List<AdvisoryRole>> GetAdvisoryRole(int compId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompId", compId);

                var result = await conn.QueryAsync<AdvisoryRole>(
                    "sp_GetAdvisoryRoles",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.AsList();
            }
        }

        public async Task<VW_Response> DeactiveAdvisory(int compId, int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@compId", compId, DbType.Int32);
                p.Add("@id", id, DbType.Int32);
                p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await con.ExecuteAsync("sp_deactiveAdvisory", p, commandType: CommandType.StoredProcedure);

                int result = p.Get<int>("@Result");

                return new VW_Response
                {
                    StatusCode = result == 1 ? 200 : 404,
                    Message = result == 1 ? "Advisory deactivated successfully." : "Advisory not found."
                };
            }
        }
    }
}
