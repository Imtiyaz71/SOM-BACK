using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IStaffService
    {
        public Task<List<StaffDesignation>> GetStaffDesignation(int compId);
        public Task<List<VW_Staff>> GetStaffInfo(int compId);
        public Task<VW_Response> SaveStaffDesignation(StaffDesignation model);
        public Task<VW_Response> DeleteStaffDesignation(int Id,int compId);
        public Task<VW_Response> SaveStaffInfo(Staff model);
        public Task<VW_Response> DeactiveStaff(int id);
        public Task<List<VW_ArchiveStaff>> GetArchiveStaff(int compId);
    }
}
