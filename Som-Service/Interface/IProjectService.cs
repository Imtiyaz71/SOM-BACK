using Som_Models.Models;
using Som_Models.VW_Models;
using System.Linq.Expressions;

namespace Som_Service.Interface
{
    public interface IProjectService
    {
        public Task<List<VW_Project>> GetProject(int compId);
        public Task<List<VW_Project>> GetProjectByProjectId(int projectid);
        public Task<(int StatusCode, string Message)> SaveProject(Project model);
        public Task<(int StatusCode, string Message)> saveassign(ProjectAssign model);
        public Task<List<VW_AssignProject>> GetProjectAssign(int compId);
        public Task<List<VW_AssignProject>> GetProjectAssignByprojectid(int projectid,int compid);
    }
}
