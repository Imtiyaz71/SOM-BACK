namespace Som_Models.VW_Models
{
    public class VW_LoanSensionViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public int TimePeriodMonths { get; set; }
        public int ActivityPeriod { get; set; }
        public decimal Interest { get; set; }
        public decimal DelayInterestRate { get; set; }
        public decimal Principal { get; set; }
        public DateTime SDate { get; set; }
        public DateTime EndContractAt { get; set; }
        public int MonthsPassed { get; set; }
        public int ActiveMonthRunning { get; set; }
        public int PaidMonths { get; set; }
        public int RemainingMonth { get; set; }
        public decimal MonthWiseInterest { get; set; }
        public decimal MonthlyPrincipal { get; set; }
        public decimal MonthlyPrinciplePayable { get; set; }
        public decimal RunningInterestTotal { get; set; }
        public decimal CalculatedDelayInterest { get; set; }
        public decimal TotalPayable { get; set; }
        public int CompId { get; set; }
    }
}
