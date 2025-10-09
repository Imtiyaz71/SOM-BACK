using Dapper;
using Microsoft.Extensions.Configuration;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using System.Data;
using System.Data.SqlClient;
namespace Som_Service.Service
{
    public class ProjectService : IProjectService
    {
        private readonly string _connectionString;

        public ProjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<VW_Project>> GetProject(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_Project>(
                "sp_Getproject",
                new { compId = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_AssignProject>> GetProjectAssign(int compId)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_AssignProject>(
                "sp_getprojectassign",
                new { compid = compId },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_AssignProject>> GetProjectAssignByprojectid(int projectid, int compid)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_AssignProject>(
                "sp_getprojectassignByProjectId",
                new { projectid = projectid,compid=compid },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<List<VW_Project>> GetProjectByProjectId(int projectid)
        {
            using var connection = new SqlConnection(_connectionString);

            var cr = await connection.QueryAsync<VW_Project>(
                "sp_GetprojectByProjectId",
                new { projectId = projectid },
                commandType: CommandType.StoredProcedure
            );

            return cr.ToList();
        }

        public async Task<(int StatusCode, string Message)> saveassign(ProjectAssign model)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            string spName;

           
                spName = "sp_addmemberassign";
                parameters.Add("@compid", model.compId, DbType.Int32);
          
                parameters.Add("@projectid", model.projectId, DbType.Int32);
                parameters.Add("@memno", model.memNo, DbType.Int32);
          

            parameters.Add("@assignby", model.assignBy, DbType.String);
       

            try
            {
                await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                return
                     (200, "Project Assign successfully.");
                   
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                return (409, ex.Message);
            }
            catch (Exception ex)
            {
                return (500, ex.Message);
            }
        }

        public async Task<(int StatusCode, string Message)> SaveProject(Project model)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            string spName;

            if (model.ProjectId == 0)
            {
                spName = "sp_project";
                parameters.Add("@compId", model.CompId, DbType.Int32);
            }
            else
            {
                spName = "sp_editproject";
                parameters.Add("@projectid", model.ProjectId, DbType.Int32);
                parameters.Add("@compId", model.CompId, DbType.Int32);
            }

            parameters.Add("@projectname", model.ProjectName, DbType.String);
            parameters.Add("@prolocation", model.ProLocation, DbType.String);
            parameters.Add("@budget", model.Budget, DbType.Double);
            parameters.Add("@directorid", model.DiretorId, DbType.Int32);
            parameters.Add("@startdate", model.StartDate, DbType.Date);
            parameters.Add("@tentitiveenddate", model.TentitiveEndDate, DbType.Date);

            try
            {
                await conn.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                return model.ProjectId == 0
                    ? (200, "Project saved successfully.")
                    : (200, "Project edited successfully.");
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                return (409, ex.Message);
            }
            catch (Exception ex)
            {
                return (500, ex.Message);
            }
        }

    }
}
