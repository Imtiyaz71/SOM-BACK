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
    public class MeetingService : IMeetingService
    {
        private readonly string _connectionString;

        public MeetingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<VW_Response> AddMeeting(Meeting model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@CompId", model.CompId, DbType.Int32);
                p.Add("@Title", model.Title, DbType.String);
                p.Add("@Biboroni", model.Biboroni, DbType.String);
                p.Add("@MeetingDate", model.MeetingDate, DbType.String);
                p.Add("@MeetingMonth", model.MeetingMonth, DbType.String);
                p.Add("@MeetingYear", model.MeetingYear, DbType.Int32);

                await con.ExecuteAsync("sp_InsertMeeting", p, commandType: CommandType.StoredProcedure);

                return new VW_Response
                {
                    StatusCode = 200,
                    Message = "Meeting added successfully."
                };
            }
        }

        // 🔹 Get all meetings by company
        public async Task<List<Meeting>> GetMeeting(int compId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@CompId", compId, DbType.Int32);

                var result = await con.QueryAsync<Meeting>("sp_GetMeetings", p, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<Meeting> GetMeetingById(int compId, int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@CompId", compId, DbType.Int32);
                p.Add("@id", id, DbType.Int32);

                var result = await con.QueryFirstOrDefaultAsync<Meeting>(
                    "sp_GetMeetingsById",
                    p,
                    commandType: CommandType.StoredProcedure
                );

                return result; // will return null if not found
            }
        }
    }
}
