namespace Som_Models.VW_Models
{
    public class VM_LoanTypes
    {
        public int Id { get; set; }               // Primary Key
        public int CompId { get; set; }           // Company Id
        public string TypeName { get; set; }      // Loan type name
        public decimal Interest { get; set; }     // Interest rate (e.g., 12.50)
        public int TimePeriodMonths { get; set; } // Loan period in months
        public DateTime? CreateDate { get; set; }  // Created date
        public DateTime? UpdateDate { get; set; } // Updated date (nullable)
        public string UpdateBy { get; set; }
        public decimal DelayInterest { get; set; }
        public int ActivityPeriod { get; set; }
    }
}
