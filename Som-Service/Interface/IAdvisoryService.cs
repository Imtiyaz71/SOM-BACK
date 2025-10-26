using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IAdvisoryService
    {
        public Task<List<AdvisoryRole>> GetAdvisoryRole(int compId);
        public Task<VW_Response> AddAdvisoryRole(AdvisoryRole model);
        public Task<VW_Response> DeleteAdvisoryRole(int CompId,int id);
        public Task<VW_Response> AddAdvisory(Advisory model);
        public Task<List<VW_AdvisoryList>> GetAdvisoryList(int compId, int cStatus);
        public Task<VW_Response> DeactiveAdvisory(int CompId, int id);
    }
}
