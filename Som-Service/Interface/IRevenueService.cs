namespace Som_Service.Interface
{
    public interface IRevenueService
    {
        public Task<decimal> TotalRevenue(int compId);
    }
}
