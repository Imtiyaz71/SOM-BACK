namespace Som_Models.VW_Models
{
    public class VW_RevenueSummary
    {
        public int Years { get; set; }                 // Year (e.g. 2025)
        public string Months { get; set; } = string.Empty;   // Month name (e.g. "October")
        public string Dates { get; set; } = string.Empty;    // Formatted date (e.g. "Oct 25, 2025")
        public string Descri { get; set; } = string.Empty;   // Description of the income
        public decimal Amount { get; set; }                 // Individual revenue amount
        public decimal DateTotal { get; set; }              // Total for that date
        public decimal MonthTotal { get; set; }             // Total for that month
        public decimal YearTotal { get; set; }              // Total for that year
        public decimal TotalRevenue { get; set; }           // Overall total revenue for the company
        public decimal AccountBalance { get; set; }         // RevenueAccount table amount
        public decimal Difference { get; set; }
    }
}
