using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IDashboardService
    {
        public Task<VW_DashboardCount> GetDashboardCounts(int compId);
        public Task<VW_ReceiveDashboardSummary> GetReceiveDashboardSummaryAsync(int compId);
    }
}
