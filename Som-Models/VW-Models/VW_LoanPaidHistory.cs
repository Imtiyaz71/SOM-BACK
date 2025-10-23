namespace Som_Models.VW_Models
{
    public class VW_LoanPaidHistory
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public decimal Payble { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Interest { get; set; }
        public decimal Principle { get; set; }
        public DateTime PDate { get; set; }
        public int PMonth { get; set; }
        public int PYear { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
