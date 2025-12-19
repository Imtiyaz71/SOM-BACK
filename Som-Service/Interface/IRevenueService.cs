using Som_Models.Models;
using Som_Models.VW_Models;

namespace Som_Service.Interface
{
    public interface IRevenueService
    {
        public Task<decimal> TotalRevenue(int compId);
        public Task<List<RevenueDisburse>> RevenueList(int compId);
        public Task<VW_Response> SaveDisburseRevenue(RevenueDisburse disburse);
    }
}
