namespace Som_Models.Models
{
    public class LoanPaidHistory
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int LoanId { get; set; }
        public float Payble { get; set; }
        public float PaidAmount { get; set; }
        public float Principle { get; set; }
        public float Interest { get; set; }
        public string PDate { get; set; }       // varchar(50) mapping
        public string PMonth { get; set; }      // varchar(50) mapping
        public int PYear { get; set; }
        public string pBy { get; set; }
    }
}
