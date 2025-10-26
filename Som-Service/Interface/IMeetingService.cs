using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IMeetingService
    {
        public Task<List<Meeting>> GetMeeting(int compId);
        public Task<Meeting> GetMeetingById(int compId,int id);
        public Task<VW_Response> AddMeeting(Meeting model);
    }
}
