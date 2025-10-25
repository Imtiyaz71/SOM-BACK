namespace Som_Models.VW_Models
{
    public class VW_LoanSensionViewModel
    {
        public int LoanId { get; set; }
        public string BorrowerName { get; set; }
        public string Phone { get; set; }
        public string LoanType { get; set; }

        public int TotalMonths { get; set; }
        public int ActiveMonths { get; set; }

        public decimal InterestRate { get; set; }
        public decimal DelayInterestRate { get; set; }

        public decimal Principal { get; set; }
        public DateTime StartDate { get; set; }
        public int CompId { get; set; }

        // Calculated / Derived Fields
        public DateTime EndContractAt { get; set; }
        public DateTime ActiveStartDate { get; set; }

        public int PaidMonths { get; set; }
        public int ActiveMonthRunning { get; set; }
        public int RemainingMonths { get; set; }

        public decimal MonthlyPrincipal { get; set; }
        public decimal MonthlyInterest { get; set; }

        public decimal AccruedInterest { get; set; }
        public decimal DelayInterest { get; set; }
        public decimal TotalInterestTillNow { get; set; }

        public decimal TotalPaidAmount { get; set; }
        public decimal TotalPayableAmount { get; set; }
        public decimal RemainingPayable { get; set; }
    }
}
